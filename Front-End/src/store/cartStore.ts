import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { CartItem, Product } from '../types';
import { apiService } from '../services/api';
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
          // For now, we'll handle cart locally since the API might not be ready
          // await apiService.addToCart(product.id, quantity, options);
          
          const defaultVariant = product.variants?.find(v => v.isDefault) || product.variants?.[0];
          const price = defaultVariant?.price || 99.99;
          
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
              isLoading: false,
            }));
          } else {
            const newItem: CartItem = {
              id: `${product.id}-${Date.now()}`,
              productId: product.id,
              product: {
                ...product,
                price: price,
                stock: defaultVariant?.stock || 0,
                rating: 4.5,
                reviewCount: 100,
                tags: ['popular'],
              },
              quantity,
              selectedVariant: defaultVariant,
              ...options,
            };

            set(state => ({
              items: [...state.items, newItem],
              isLoading: false,
            }));
          }

          get().calculateTotals();
          toast.success('Added to cart');
        } catch (error) {
          set({ isLoading: false });
          toast.error('Failed to add to cart');
        }
      },

      removeItem: async (itemId: string) => {
        try {
          // await apiService.removeFromCart(itemId);
          set(state => ({
            items: state.items.filter(item => item.id !== itemId),
          }));
          get().calculateTotals();
          toast.success('Removed from cart');
        } catch (error) {
          toast.error('Failed to remove item');
        }
      },

      updateQuantity: async (itemId: string, quantity: number) => {
        if (quantity <= 0) {
          get().removeItem(itemId);
          return;
        }

        try {
          // await apiService.updateCartItem(itemId, quantity);
          set(state => ({
            items: state.items.map(item =>
              item.id === itemId ? { ...item, quantity } : item
            ),
          }));
          get().calculateTotals();
        } catch (error) {
          toast.error('Failed to update quantity');
        }
      },

      clearCart: () => {
        set({ items: [], total: 0, itemCount: 0 });
      },

      loadCart: async () => {
        set({ isLoading: true });
        try {
          // const cartData = await apiService.getCart();
          // For now, we'll use local storage
          set({
            isLoading: false,
          });
          get().calculateTotals();
        } catch (error) {
          set({ isLoading: false });
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
    }),
    {
      name: 'cart-storage',
      partialize: (state) => ({
        items: state.items,
        total: state.total,
        itemCount: state.itemCount,
      }),
    }
  )
);