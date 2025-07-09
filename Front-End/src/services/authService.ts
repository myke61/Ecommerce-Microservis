import { UserManager, User, UserManagerSettings } from 'oidc-client-ts';

const IDENTITY_SERVER_URL = import.meta.env.VITE_IDENTITY_SERVER_URL || 'https://localhost:5001';
const CLIENT_ID = import.meta.env.VITE_OAUTH_CLIENT_ID || 'ReactWeb';
const CLIENT_SECRET = import.meta.env.VITE_OAUTH_CLIENT_SECRET || 'ReactWeb';

class AuthService {
  private userManager: UserManager;

  constructor() {
    const settings: UserManagerSettings = {
      authority: IDENTITY_SERVER_URL,
      client_id: CLIENT_ID,
      client_secret: CLIENT_SECRET,
      redirect_uri: `${window.location.origin}/auth/callback`,
      post_logout_redirect_uri: `${window.location.origin}/`,
      response_type: 'code',
      scope: 'openid profile email productApi',
      automaticSilentRenew: true,
      silent_redirect_uri: `${window.location.origin}/auth/silent-callback`,
      filterProtocolClaims: true,
      loadUserInfo: true,
      monitorSession: true,
      checkSessionInterval: 10000,
      revokeAccessTokenOnSignout: true,
    };
    console.log('AuthService initialized with settings:', settings);

    this.userManager = new UserManager(settings);

    // Event handlers
    this.userManager.events.addUserLoaded((user: User) => {
      console.log('User loaded:', user);
    });

    this.userManager.events.addUserUnloaded(() => {
      console.log('User unloaded');
    });

    this.userManager.events.addAccessTokenExpiring(() => {
      console.log('Access token expiring');
    });

    this.userManager.events.addAccessTokenExpired(() => {
      console.log('Access token expired');
    });

    this.userManager.events.addSilentRenewError((error) => {
      console.error('Silent renew error:', error);
    });

    this.userManager.events.addUserSignedOut(() => {
      console.log('User signed out');
    });
  }

  // Start the login process
  async login(): Promise<void> {
    try {
      await this.userManager.signinRedirect();
    } catch (error) {
      console.error('Login error:', error);
      throw error;
    }
  }

  // Handle the callback after login
  async handleCallback(): Promise<User | null> {
    try {
      const user = await this.userManager.signinRedirectCallback();
      return user;
    } catch (error) {
      console.error('Callback error:', error);
      throw error;
    }
  }

  // Get current user
  async getUser(): Promise<User | null> {
    try {
      return await this.userManager.getUser();
    } catch (error) {
      console.error('Get user error:', error);
      return null;
    }
  }

  // Logout
  async logout(): Promise<void> {
    try {
      await this.userManager.signoutRedirect();
    } catch (error) {
      console.error('Logout error:', error);
      throw error;
    }
  }

  // Silent logout (without redirect)
  async logoutSilent(): Promise<void> {
    try {
      await this.userManager.removeUser();
    } catch (error) {
      console.error('Silent logout error:', error);
      throw error;
    }
  }

  // Renew token silently
  async renewToken(): Promise<User | null> {
    try {
      return await this.userManager.signinSilent();
    } catch (error) {
      console.error('Token renewal error:', error);
      throw error;
    }
  }

  // Check if user is authenticated
  async isAuthenticated(): Promise<boolean> {
    const user = await this.getUser();
    return user !== null && !user.expired;
  }

  // Get access token
  async getAccessToken(): Promise<string | null> {
    const user = await this.getUser();
    return user?.access_token || null;
  }

  // Handle silent callback
  async handleSilentCallback(): Promise<void> {
    try {
      await this.userManager.signinSilentCallback();
    } catch (error) {
      console.error('Silent callback error:', error);
      throw error;
    }
  }
}

export const authService = new AuthService();