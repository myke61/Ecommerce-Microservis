// API configuration for .NET backend integration
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:3030/gateway';
const IDENTITY_SERVER_URL = import.meta.env.VITE_IDENTITY_SERVER_URL || 'https://localhost:5001';

import { ProductListResponse } from '../types';

class ApiService {
  private baseURL: string;
  private identityServerURL: string;

  constructor() {
    this.baseURL = API_BASE_URL;
    this.identityServerURL = IDENTITY_SERVER_URL;
  }

  private async getAuthHeaders(): Promise<Record<string, string>> {
    const token = localStorage.getItem('access_token');
    return {
      'Content-Type': 'application/json',
      ...(token && { Authorization: `Bearer ${token}` }),
    };
  }

  private getPublicHeaders(): Record<string, string> {
    return {
      'Content-Type': 'application/json',
    };
  }

  private async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      if (response.status === 401) {
        // Handle token refresh or redirect to login
        localStorage.removeItem('access_token');
        window.location.href = '/login';
      }
      throw new Error(`API Error: ${response.status}`);
    }
    return response.json();
  }

  // Authentication methods for Identity Server integration
  async login(email: string, password: string) {
    const response = await fetch(`${this.identityServerURL}/connect/token`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        grant_type: 'password',
        username: email,
        password: password,
        client_id: 'ecommerce-spa',
        scope: 'openid profile email ecommerce-api',
      }),
    });

    return this.handleResponse(response);
  }

  async refreshToken(refreshToken: string) {
    const response = await fetch(`${this.identityServerURL}/connect/token`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      body: new URLSearchParams({
        grant_type: 'refresh_token',
        refresh_token: refreshToken,
        client_id: 'ecommerce-spa',
      }),
    });

    return this.handleResponse(response);
  }

  // Product API methods - Open API (no authorization required)
  async getProducts(params?: {
    page?: number;
    pageSize?: number;
    category?: string;
    search?: string;
    sortBy?: string;
    minPrice?: number;
    maxPrice?: number;
    featured?: boolean;
  }): Promise<ProductListResponse> {
    const queryParams = new URLSearchParams();
    if (params) {
      Object.entries(params).forEach(([key, value]) => {
        if (value !== undefined) {
          queryParams.append(key, value.toString());
        }
      });
    }

    const response = await fetch(`${this.baseURL}/product/list?${queryParams}`, {
      method: 'GET',
      headers: this.getPublicHeaders(),
    });

    return this.handleResponse<ProductListResponse>(response);
  }

  async getProduct(id: string) {
    const response = await fetch(`${this.baseURL}/product/${id}`, {
      method: 'GET',
      headers: this.getPublicHeaders(),
    });

    return this.handleResponse(response);
  }

  async getFeaturedProducts(): Promise<ProductListResponse> {
    const response = await fetch(`${this.baseURL}/product/list?featured=true&pageSize=8`, {
      method: 'GET',
      headers: this.getPublicHeaders(),
    });

    return this.handleResponse<ProductListResponse>(response);
  }

  // Category API methods - Open API
  async getCategories() {
    const response = await fetch(`${this.baseURL}/categories`, {
      headers: this.getPublicHeaders(),
    });

    return this.handleResponse(response);
  }

  // Cart API methods - Requires authentication
  async getCart() {
    const response = await fetch(`${this.baseURL}/cart`, {
      headers: await this.getAuthHeaders(),
    });

    return this.handleResponse(response);
  }

  async addToCart(productId: string, quantity: number, options?: any) {
    const response = await fetch(`${this.baseURL}/cart/items`, {
      method: 'POST',
      headers: await this.getAuthHeaders(),
      body: JSON.stringify({ productId, quantity, ...options }),
    });

    return this.handleResponse(response);
  }

  async updateCartItem(itemId: string, quantity: number) {
    const response = await fetch(`${this.baseURL}/cart/items/${itemId}`, {
      method: 'PUT',
      headers: await this.getAuthHeaders(),
      body: JSON.stringify({ quantity }),
    });

    return this.handleResponse(response);
  }

  async removeFromCart(itemId: string) {
    const response = await fetch(`${this.baseURL}/cart/items/${itemId}`, {
      method: 'DELETE',
      headers: await this.getAuthHeaders(),
    });

    return this.handleResponse(response);
  }

  // Order API methods - Requires authentication
  async createOrder(orderData: any) {
    const response = await fetch(`${this.baseURL}/orders`, {
      method: 'POST',
      headers: await this.getAuthHeaders(),
      body: JSON.stringify(orderData),
    });

    return this.handleResponse(response);
  }

  async getOrders() {
    const response = await fetch(`${this.baseURL}/orders`, {
      headers: await this.getAuthHeaders(),
    });

    return this.handleResponse(response);
  }

  async getOrder(id: string) {
    const response = await fetch(`${this.baseURL}/orders/${id}`, {
      headers: await this.getAuthHeaders(),
    });

    return this.handleResponse(response);
  }

  // User profile methods - Requires authentication
  async getUserProfile() {
    const response = await fetch(`${this.baseURL}/user/profile`, {
      headers: await this.getAuthHeaders(),
    });

    return this.handleResponse(response);
  }

  async updateUserProfile(profileData: any) {
    const response = await fetch(`${this.baseURL}/user/profile`, {
      method: 'PUT',
      headers: await this.getAuthHeaders(),
      body: JSON.stringify(profileData),
    });

    return this.handleResponse(response);
  }
}

export const apiService = new ApiService();