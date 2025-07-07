export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: 'customer' | 'admin';
  avatar?: string;
}

export interface Product {
  id: string;
  name: string;
  code: string;
  description: string;
  slug: string;
  isDeleted: boolean;
  brandId: string;
  brand: Brand | null;
  categoryId: string;
  category: Category | null;
  variants: ProductVariant[] | null;
  images: ProductImage[] | null;
  createdDate: string;
  updatedDate: string;
  
  // Computed/derived properties for UI compatibility
  price?: number;
  originalPrice?: number;
  stock?: number;
  rating?: number;
  reviewCount?: number;
  tags?: string[];
  specifications?: Record<string, string>;
  isNew?: boolean;
  isFeatured?: boolean;
}

export interface Brand {
  id: string;
  name: string;
  slug?: string;
  description?: string;
  logo?: string;
  logoUrl?: string;
}

export interface ProductVariant {
  id: string;
  productId: string;
  sku: string;
  price: number;
  originalPrice?: number;
  stock?: number;
  stockQuantity?: number;
  attributes: Record<string, string>; // size, color, etc.
  isDefault: boolean;
}

export interface ProductImage {
  id: string;
  productId: string;
  url?: string;
  imageUrl?: string;
  altText?: string;
  sortOrder?: number;
  displayOrder?: number;
  isMain: boolean;
}

export interface ProductListResponse {
  products: Product[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface CartItem {
  id: string;
  productId: string;
  product: Product;
  quantity: number;
  selectedSize?: string;
  selectedColor?: string;
  selectedVariant?: ProductVariant;
}

export interface Order {
  id: string;
  userId: string;
  items: CartItem[];
  total: number;
  subtotal: number;
  tax: number;
  shipping: number;
  status: 'pending' | 'processing' | 'shipped' | 'delivered' | 'cancelled';
  shippingAddress: Address;
  billingAddress: Address;
  paymentMethod: string;
  createdAt: string;
  updatedAt: string;
}

export interface Address {
  id?: string;
  firstName: string;
  lastName: string;
  company?: string;
  address1: string;
  address2?: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  phone?: string;
}

export interface Category {
  id: string;
  name: string;
  slug: string;
  description?: string;
  image?: string;
  parentId?: string;
  children?: Category[];
}

export interface AuthTokens {
  accessToken: string;
  refreshToken: string;
  expiresAt: number;
}