import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { User as OidcUser } from 'oidc-client-ts';
import { User, AuthTokens } from '../types';
import { authService } from '../services/authService';

interface AuthState {
  user: User | null;
  tokens: AuthTokens | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  initiateLogin: () => Promise<void>;
  handleAuthCallback: () => Promise<void>;
  logout: () => Promise<void>;
  logoutSilent: () => Promise<void>;
  checkAuth: () => Promise<void>;
  renewToken: () => Promise<void>;
  setUser: (user: User) => void;
}

const mapOidcUserToUser = (oidcUser: OidcUser): User => {
  return {
    id: oidcUser.profile.sub || '1',
    email: oidcUser.profile.email || '',
    firstName: oidcUser.profile.given_name || oidcUser.profile.name || 'User',
    lastName: oidcUser.profile.family_name || '',
    role: 'customer',
    avatar: oidcUser.profile.picture,
  };
};

const mapOidcUserToTokens = (oidcUser: OidcUser): AuthTokens => {
  return {
    accessToken: oidcUser.access_token,
    refreshToken: oidcUser.refresh_token || '',
    expiresAt: oidcUser.expires_at ? oidcUser.expires_at * 1000 : Date.now() + 3600000,
  };
};

export const useAuthStore = create<AuthState>()(
  persist(
    (set, get) => ({
      user: null,
      tokens: null,
      isAuthenticated: false,
      isLoading: false,

      initiateLogin: async () => {
        set({ isLoading: true });
        try {
          await authService.login();
        } catch (error) {
          console.error('Login initiation error:', error);
          set({ isLoading: false });
          throw error;
        }
      },

      handleAuthCallback: async () => {
        set({ isLoading: true });
        try {
          const oidcUser = await authService.handleCallback();
          
          if (oidcUser) {
            const user = mapOidcUserToUser(oidcUser);
            const tokens = mapOidcUserToTokens(oidcUser);

            // Store access token in localStorage for API calls
            localStorage.setItem('access_token', tokens.accessToken);
            
            set({
              user,
              tokens,
              isAuthenticated: true,
              isLoading: false,
            });
          } else {
            set({ isLoading: false });
            throw new Error('No user returned from callback');
          }
        } catch (error) {
          console.error('Auth callback error:', error);
          set({ isLoading: false });
          throw error;
        }
      },

      logout: async () => {
        set({ isLoading: true });
        try {
          await authService.logout();
          localStorage.removeItem('access_token');
          set({
            user: null,
            tokens: null,
            isAuthenticated: false,
            isLoading: false,
          });
          // Redirect to home page after logout
          window.location.href = '/';
        } catch (error) {
          console.error('Logout error:', error);
          // Even if logout fails, clear local state
          localStorage.removeItem('access_token');
          set({
            user: null,
            tokens: null,
            isAuthenticated: false,
            isLoading: false,
          });
          // Redirect to home page even if logout fails
          window.location.href = '/';
        }
      },

      logoutSilent: async () => {
        try {
          await authService.logoutSilent();
          localStorage.removeItem('access_token');
          set({
            user: null,
            tokens: null,
            isAuthenticated: false,
          });
        } catch (error) {
          console.error('Silent logout error:', error);
          // Even if logout fails, clear local state
          localStorage.removeItem('access_token');
          set({
            user: null,
            tokens: null,
            isAuthenticated: false,
          });
        }
      },

      checkAuth: async () => {
        try {
          const oidcUser = await authService.getUser();
          
          if (oidcUser && !oidcUser.expired) {
            const user = mapOidcUserToUser(oidcUser);
            const tokens = mapOidcUserToTokens(oidcUser);

            localStorage.setItem('access_token', tokens.accessToken);
            
            set({
              user,
              tokens,
              isAuthenticated: true,
            });
          } else {
            // User is not authenticated or token expired
            localStorage.removeItem('access_token');
            set({
              user: null,
              tokens: null,
              isAuthenticated: false,
            });
          }
        } catch (error) {
          console.error('Check auth error:', error);
          localStorage.removeItem('access_token');
          set({
            user: null,
            tokens: null,
            isAuthenticated: false,
          });
        }
      },

      renewToken: async () => {
        try {
          const oidcUser = await authService.renewToken();
          
          if (oidcUser) {
            const user = mapOidcUserToUser(oidcUser);
            const tokens = mapOidcUserToTokens(oidcUser);

            localStorage.setItem('access_token', tokens.accessToken);
            
            set({
              user,
              tokens,
              isAuthenticated: true,
            });
          }
        } catch (error) {
          console.error('Token renewal error:', error);
          // If renewal fails, logout
          get().logoutSilent();
        }
      },

      setUser: (user: User) => {
        set({ user });
      },
    }),
    {
      name: 'auth-storage',
      partialize: (state) => ({
        user: state.user,
        tokens: state.tokens,
        isAuthenticated: state.isAuthenticated,
      }),
    }
  )
);