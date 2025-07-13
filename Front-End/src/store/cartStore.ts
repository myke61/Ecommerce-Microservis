import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { CartItem, Product } from '../types';
import { apiService } from '../services/api';
import { useAuthStore } from './authStore';
import toast from 'react-hot-toast';

interface CartState {
  items: CartItem[];
  isLoading: boolean;
  total: number;
  itemCount: number;
  addItem: (product: Product, quantity?: number, options?: any) => Promise<void>;
  removeItem: (itemId: string) => Promise<void>;
  updateQuantity: (itemId: string, quantity: number) => Promise<void>;
  clearCart: () => void;
  loadCart: () => Promise<void>;
  calculateTotals: () => void;
  syncWithServer: () => Promise<void>;
}

export const useCartStore = create<CartState>()(
  persist(
    (set, get) => ({
      items: [],
      isLoading: false,
      total: 0,
      itemCount: 0,

      addItem: async (product: Product, quantity = 1, options = {}) => {
        set({ isLoading: true });
        try {
          const { isAuthenticated } = useAuthStore.getState();
          
          if (isAuthenticated) {
            // User is logged in - send to API
            try {
              const result = await apiService.addToBasket(product.id);
              console.log('Basket API response:', result);
              toast.success('Added to basket');
              // Reload cart from server to get updated data
              await get().loadCart();
            } catch (error) {
              console.error('Failed to add to server basket:', error);
              toast.error(`Failed to add to basket: ${error.message}`);
              set({ isLoading: false });
              return;
            }
          } else {
            // User is not logged in - store locally
            const price = product.price || 99.99;
            
            const existingItem = get().items.find(item => 
              item.productId === product.id && 
              JSON.stringify(options) === JSON.stringify({ 
                selectedSize: item.selectedSize, 
                selectedColor: item.selectedColor 
              })
            );

            if (existingItem) {
              set(state => ({
                items: state.items.map(item =>
                  item.id === existingItem.id
                    ? { ...item, quantity: item.quantity + quantity }
                    : item
                ),
              }));
            } else {
              const newItem: CartItem = {
                id: `local-${product.id}-${Date.now()}`,
                productId: product.id,
                product: {
                  ...product,
                  price: price,
                  stock: product.stock || 999,
                  rating: 4.5,
                  reviewCount: 100,
                  tags: ['popular'],
                },
                quantity,
                selectedVariant: null,
                ...options,
              };

              set(state => ({
                items: [...state.items, newItem],
              }));
            }

            get().calculateTotals();
            toast.success('Added to cart');
          }
        } catch (error) {
          console.error('Add to cart error:', error);
          toast.error('Failed to add to cart');
        } finally {
          set({ isLoading: false });
        }
      },

      removeItem: async (itemId: string) => {
        try {
          const { isAuthenticated } = useAuthStore.getState();
          const item = get().items.find(i => i.id === itemId);
          
          if (isAuthenticated && !itemId.startsWith('local-') && item) {
            // Remove from server
            const result = await apiService.removeFromBasket(item.productId);
            console.log('Remove from basket API response:', result);
            await get().loadCart();
          } else {
            // Remove from local storage
            set(state => ({
              items: state.items.filter(item => item.id !== itemId),
            }));
            get().calculateTotals();
          }
          
          toast.success('Removed from cart');
        } catch (error) {
          console.error('Remove from cart error:', error);
          toast.error('Failed to remove item');
        }
      },

      updateQuantity: async (itemId: string, quantity: number) => {
        if (quantity <= 0) {
          get().removeItem(itemId);
          return;
        }

        try {
          const { isAuthenticated } = useAuthStore.getState();
          const item = get().items.find(i => i.id === itemId);
          
          if (isAuthenticated && !itemId.startsWith('local-') && item) {
            // For authenticated users, call AddItem API instead of UpdateItem
            const currentQuantity = item.quantity;
            const quantityDifference = quantity - currentQuantity;
            
            if (quantityDifference > 0) {
              // Adding items - call AddItem for each additional item
              for (let i = 0; i < quantityDifference; i++) {
                await apiService.addToBasket(item.productId);
              }
            } else if (quantityDifference < 0) {
              // Removing items - call RemoveItem for each item to remove
              for (let i = 0; i < Math.abs(quantityDifference); i++) {
                await apiService.removeFromBasket(item.productId);
              }
            }
            // Reload cart from server to get updated data
            await get().loadCart();
          } else {
            // Update locally
            set(state => ({
              items: state.items.map(item =>
                item.id === itemId ? { ...item, quantity } : item
              ),
            }));
            get().calculateTotals();
          }
        } catch (error) {
          console.error('Update quantity error:', error);
          toast.error('Failed to update quantity');
        }
      },

      clearCart: () => {
        set({ items: [], total: 0, itemCount: 0 });
      },

      loadCart: async () => {
        const { isAuthenticated } = useAuthStore.getState();
        
        if (!isAuthenticated) {
          // For non-authenticated users, just calculate totals from local storage
          get().calculateTotals();
          return;
        }

        set({ isLoading: true });
        try {
          const basketData = await apiService.getBasket();
          console.log('Basket API response:', basketData);
          
          // Transform server basket data to match our CartItem interface
          const serverItems: CartItem[] = basketData.basketItems?.map((item: any, index: number) => ({
            id: `server-${item.productId}-${index}`, // Generate unique ID
            productId: item.productId,
            product: {
              id: item.productId,
              name: item.productName,
              price: item.unitPrice,
              code: '',
              description: '',
              slug: '',
              brand: null,
              category: null,
              variants: null,
              images: [{
                id: `image-${item.productId}`,
                productId: item.productId,
                imageUrl: 'https://images.pexels.com/photos/230544/pexels-photo-230544.jpeg',
                isMain: true,
              }],
              createdDate: new Date().toISOString(),
              updatedDate: new Date().toISOString(),
            },
            quantity: item.quantity,
            selectedVariant: null,
          })) || [];

          set({
            items: serverItems,
            total: basketData.basketAmount || 0,
            isLoading: false,
          });
          
          get().calculateTotals();
        } catch (error) {
          console.error('Load cart error:', error);
          set({ isLoading: false });
          // If server fails, keep local items
          get().calculateTotals();
        }
      },

      calculateTotals: () => {
        const { items } = get();
        const total = items.reduce((sum, item) => {
          const price = item.selectedVariant?.price || item.product.price || 0;
          return sum + (price * item.quantity);
        }, 0);
        const itemCount = items.reduce((sum, item) => sum + item.quantity, 0);
        set({ total, itemCount });
      },

      syncWithServer: async () => {
        const { isAuthenticated } = useAuthStore.getState();
        const { items } = get();
        
        if (!isAuthenticated || items.length === 0) {
          return;
        }

        // Sync local items to server when user logs in
        try {
          const localItems = items.filter(item => item.id.startsWith('local-'));
          
          for (const item of localItems) {
            try {
              await apiService.addToBasket(item.productId);
            } catch (error) {
              console.error('Failed to sync item to server:', error);
            }
          }

          // After syncing, reload from server
          await get().loadCart();
          
          if (localItems.length > 0) {
            toast.success(`Synced ${localItems.length} items to your account`);
          }
        } catch (error) {
          console.error('Sync error:', error);
        }
      },
    }),
    {
      name: 'cart-storage',
      partialize: (state) => ({
        items: state.items.filter(item => item.id.startsWith('local-')), // Only persist local items
        total: state.total,
        itemCount: state.itemCount,
      }),
    }
  )
);