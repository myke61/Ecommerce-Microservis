import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useAuthStore } from '../store/authStore';

const CHAT_HUB_URL = 'https://localhost:3030/chathub';

export interface ChatMessage {
  id: string;
  message: string;
  userId: string;
  userName: string;
  timestamp: Date;
  isFromSupport: boolean;
}

class ChatService {
  private connection: HubConnection | null = null;
  private messageHandlers: ((message: ChatMessage) => void)[] = [];
  private connectionStateHandlers: ((connected: boolean) => void)[] = [];
  private isConnecting: boolean = false;
  private reconnectAttempts: number = 0;
  private maxReconnectAttempts: number = 3;

  async startConnection(): Promise<void> {
    console.log('🚀 Starting SignalR connection...');
    
    // Debug: Check if we're already trying to connect
    console.log('🔍 Current connection state:', this.connection?.state || 'No connection');
    console.log('🔍 Is connecting:', this.isConnecting);
    
    if (this.connection?.state === 'Connected') {
      console.log('✅ Already connected');
      return;
    }

    if (this.isConnecting) {
      console.log('⏳ Already connecting...');
      return;
    }

    this.isConnecting = true;
    
    const { tokens } = useAuthStore.getState();
    if (!tokens?.accessToken) {
      console.error('❌ No access token available');
      console.log('🔍 Auth store state:', useAuthStore.getState());
      this.isConnecting = false;
      throw new Error('No access token available');
    }

    console.log('🔑 Access token found:', tokens.accessToken.substring(0, 20) + '...');

    // Stop existing connection if any
    if (this.connection) {
      try {
        console.log('🔄 Stopping existing connection...');
        await this.connection.stop();
      } catch (error) {
        console.log('Error stopping existing connection:', error);
      }
      this.connection = null;
    }

    console.log('🔧 Building SignalR connection...');
    console.log('📡 Hub URL:', CHAT_HUB_URL);

    try {
      this.connection = new HubConnectionBuilder()
      .withUrl(CHAT_HUB_URL, {
        accessTokenFactory: () => tokens.accessToken,
        skipNegotiation: false, // Let SignalR negotiate
        withCredentials: true,
        // Don't force WebSocket only, let it fallback if needed
      })
      .withAutomaticReconnect([0, 2000, 10000, 30000])
      .configureLogging(LogLevel.Information)
      .build();
      
      console.log('✅ HubConnection created successfully:', this.connection);
    } catch (error) {
      console.error('❌ Failed to create HubConnection:', error);
      this.isConnecting = false;
      throw error;
    }

    // Event handlers
    this.connection.on('ReceiveMessage', (message: ChatMessage) => {
      console.log('Received message:', message);
      this.messageHandlers.forEach(handler => handler(message));
    });

    this.connection.on('ReceiveSupportMessage', (message: ChatMessage) => {
      console.log('Received support message:', message);
      const supportMessage = { ...message, isFromSupport: true };
      this.messageHandlers.forEach(handler => handler(supportMessage));
    });

    this.connection.onclose((error) => {
      console.log('SignalR connection closed:', error);
      this.isConnecting = false;
      this.connectionStateHandlers.forEach(handler => handler(false));
    });

    this.connection.onreconnected((connectionId) => {
      console.log('SignalR reconnected:', connectionId);
      this.isConnecting = false;
      this.reconnectAttempts = 0;
      this.connectionStateHandlers.forEach(handler => handler(true));
    });

    this.connection.onreconnecting((error) => {
      console.log('SignalR reconnecting...', error);
      this.reconnectAttempts++;
      this.connectionStateHandlers.forEach(handler => handler(false));
    });

    try {
      console.log('🔌 Attempting to start connection...');
      await this.connection.start();
      console.log('✅ SignalR connection started successfully!');
      console.log('📊 Connection state:', this.connection.state);
      console.log('🆔 Connection ID:', this.connection.connectionId);
      this.isConnecting = false;
      this.reconnectAttempts = 0;
      this.connectionStateHandlers.forEach(handler => handler(true));
    } catch (error) {
      console.error('❌ SignalR connection failed:', error);
      console.error('🔍 Error details:', {
        message: error.message,
        stack: error.stack,
        name: error.name
      });
      this.isConnecting = false;
      this.connection = null;
      throw error;
    }
  }

  async stopConnection(): Promise<void> {
    this.isConnecting = false;
    if (this.connection) {
      try {
        await this.connection.stop();
      } catch (error) {
        console.log('Error stopping connection:', error);
      }
      this.connection = null;
      this.connectionStateHandlers.forEach(handler => handler(false));
    }
  }

  async sendMessage(message: string): Promise<void> {
    if (!this.connection || this.connection.state !== 'Connected') {
      console.error('❌ Cannot send message - not connected');
      console.log('📊 Current connection state:', this.connection?.state || 'No connection');
      throw new Error('SignalR connection not established');
    }

    const { user } = useAuthStore.getState();
    if (!user) {
      throw new Error('User not authenticated');
    }

    try {
      console.log('📤 Sending message:', {
        message
      });
      
      await this.connection.invoke('SendMessage', message);
      
      console.log('✅ Message sent successfully');
    } catch (error) {
      console.error('❌ Send message error:', error);
      throw error;
    }
  }

  onMessage(handler: (message: ChatMessage) => void): () => void {
    this.messageHandlers.push(handler);
    return () => {
      const index = this.messageHandlers.indexOf(handler);
      if (index > -1) {
        this.messageHandlers.splice(index, 1);
      }
    };
  }

  onConnectionStateChange(handler: (connected: boolean) => void): () => void {
    this.connectionStateHandlers.push(handler);
    return () => {
      const index = this.connectionStateHandlers.indexOf(handler);
      if (index > -1) {
        this.connectionStateHandlers.splice(index, 1);
      }
    };
  }

  isConnected(): boolean {
    return this.connection?.state === 'Connected';
  }

  getConnectionState(): string {
    return this.connection?.state || 'Disconnected';
  }

  getReconnectAttempts(): number {
    return this.reconnectAttempts;
  }

  async retryConnection(): Promise<void> {
    if (this.reconnectAttempts < this.maxReconnectAttempts) {
      console.log(`Retrying connection... Attempt ${this.reconnectAttempts + 1}`);
      await this.startConnection();
    } else {
      console.log('Max reconnect attempts reached');
      throw new Error('Max reconnect attempts reached');
    }
  }
}

export const chatService = new ChatService();