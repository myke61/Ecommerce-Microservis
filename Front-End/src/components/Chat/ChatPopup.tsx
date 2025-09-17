import React, { useState, useRef, useEffect } from 'react';
import { MessageCircle, X, Send, User, Bot, Minimize2, Maximize2, Wifi, WifiOff } from 'lucide-react';
import { useAuthStore } from '../../store/authStore';
import { chatService, ChatMessage } from '../../services/chatService';
import toast from 'react-hot-toast';

export const ChatPopup: React.FC = () => {
  const [isOpen, setIsOpen] = useState(false);
  const [isMinimized, setIsMinimized] = useState(false);
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [inputMessage, setInputMessage] = useState('');
  const [isLoading, setIsLoading] = useState(false);
  const [unreadCount, setUnreadCount] = useState(0);
  const [isConnected, setIsConnected] = useState(false);
  const [connectionState, setConnectionState] = useState('Disconnected');
  const [connectionError, setConnectionError] = useState<string | null>(null);
  
  const { isAuthenticated, user } = useAuthStore();
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  const scrollToBottom = () => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  };

  useEffect(() => {
    scrollToBottom();
  }, [messages]);

  useEffect(() => {
    if (isOpen) {
      setUnreadCount(0);
      inputRef.current?.focus();
    }
  }, [isOpen]);

  // SignalR connection management
  useEffect(() => {
    console.log('🎯 Chat popup useEffect triggered:', { isAuthenticated, isOpen });
    
    if (isAuthenticated && isOpen) {
      // Delay connection to avoid race conditions
      initializeConnection();
    }
    else
    {
      chatService.stopConnection();
    }

    return () => {
      // Don't stop connection immediately, let it persist
    };
  }, [isAuthenticated, isOpen]);

  // Cleanup on unmount
  useEffect(() => {
    return () => {
      if (chatService.isConnected()) {
        chatService.stopConnection();
      }
    };
  }, []);

  const startConnection = async () => {
    try {
      console.log('⏰ Starting delayed connection...');
      await chatService.startConnection();
    } catch (error) {
      console.error('Connection failed:', error);
    }
  };

  const initializeConnection = async () => {
    setConnectionError(null);
    console.log('🎯 Initializing chat connection...');
    console.log('🔍 Auth state:', { isAuthenticated, user });
    
    try {
      console.log('📞 Calling chatService.startConnection()...');
      await chatService.startConnection();
      console.log('✅ chatService.startConnection() completed');
      
      // Message handler
      const unsubscribeMessage = chatService.onMessage((message: ChatMessage) => {
        setMessages(prev => [...prev, message]);
        
        if (!isOpen) {
          setUnreadCount(prev => prev + 1);
        }
      });

      // Connection state handler
      const unsubscribeConnection = chatService.onConnectionStateChange((connected: boolean) => {
        setIsConnected(connected);
        setConnectionState(chatService.getConnectionState());
        if (connected) {
          setConnectionError(null);
        }
      });

      // Add welcome message
      if (messages.length === 0) {
        const welcomeMessage: ChatMessage = {
          id: 'welcome-' + Date.now(),
          message: `Merhaba ${user?.firstName || 'Kullanıcı'}! Size nasıl yardımcı olabilirim?`,
          userId: 'system',
          userName: 'Müşteri Desteği',
          timestamp: new Date(),
          isFromSupport: true,
        };
        setMessages([welcomeMessage]);
      }

      return () => {
        unsubscribeMessage();
        unsubscribeConnection();
      };
    } catch (error) {
      console.error('SignalR connection failed:', error);
      console.error('🔍 Error details:', {
        message: error.message,
        stack: error.stack,
        name: error.name
      });
      setConnectionError('Bağlantı kurulamadı');
      // Don't show toast immediately, user might retry
    }
  };

  const retryConnection = async () => {
    setConnectionError(null);
    try {
      await chatService.retryConnection();
      toast.success('Bağlantı kuruldu!');
    } catch (error) {
      setConnectionError('Bağlantı kurulamadı. Lütfen daha sonra tekrar deneyin.');
      toast.error('Bağlantı kurulamadı');
    }
  };

  const sendMessage = async () => {
    if (!inputMessage.trim()) return;
    
    if (!isAuthenticated) {
      toast.error('Mesaj göndermek için giriş yapmalısınız');
      return;
    }

    if (!chatService.isConnected()) {
      setConnectionError('Bağlantı yok');
      return;
    }

    const userMessage: ChatMessage = {
      id: Date.now().toString(),
      message: inputMessage,
      userId: user?.id || 'unknown',
      userName: user?.firstName || user?.name || 'Kullanıcı',
      timestamp: new Date(),
      isFromSupport: false,
    };

    // Add user message immediately
    setMessages(prev => [...prev, userMessage]);
    setInputMessage('');
    setIsLoading(true);

    try {
      await chatService.sendMessage(inputMessage);
    } catch (error) {
      console.error('Send message error:', error);
      toast.error('Mesaj gönderilemedi. Lütfen tekrar deneyin.');
      
      // Remove the message that failed to send
      setMessages(prev => prev.filter(msg => msg.id !== userMessage.id));
    } finally {
      setIsLoading(false);
    }
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      sendMessage();
    }
  };

  const formatTime = (date: Date) => {
    return date.toLocaleTimeString('tr-TR', { 
      hour: '2-digit', 
      minute: '2-digit' 
    });
  };

  const getConnectionIcon = () => {
    return isConnected ? (
      <Wifi className="w-3 h-3 text-green-400" />
    ) : (
      <WifiOff className="w-3 h-3 text-red-400" />
    );
  };

  const getConnectionText = () => {
    switch (connectionState) {
      case 'Connected':
        return 'Bağlı';
      case 'Connecting':
        return 'Bağlanıyor...';
      case 'Reconnecting':
        return 'Yeniden bağlanıyor...';
      case 'Disconnected':
        return 'Bağlantı yok';
      default:
        return connectionState;
    }
  };

  if (!isOpen) {
    return (
      <button
        onClick={() => setIsOpen(true)}
        className="fixed bottom-6 right-6 bg-gradient-to-r from-blue-600 to-purple-600 text-white p-4 rounded-full shadow-lg hover:shadow-xl transition-all duration-300 transform hover:scale-110 z-50"
      >
        <MessageCircle className="h-6 w-6" />
        {unreadCount > 0 && (
          <span className="absolute -top-2 -right-2 bg-red-500 text-white text-xs rounded-full h-6 w-6 flex items-center justify-center font-bold">
            {unreadCount}
          </span>
        )}
      </button>
    );
  }

  return (
    <div className={`fixed bottom-6 right-6 bg-white rounded-lg shadow-2xl border border-gray-200 z-50 transition-all duration-300 ${
      isMinimized ? 'w-80 h-16' : 'w-80 h-96'
    }`}>
      {/* Header */}
      <div className="bg-gradient-to-r from-blue-600 to-purple-600 text-white p-4 rounded-t-lg flex items-center justify-between">
        <div className="flex items-center space-x-2">
          <Bot className="h-5 w-5" />
          <span className="font-semibold">Müşteri Desteği</span>
          <div className="flex items-center space-x-1">
            {getConnectionIcon()}
            <span className="text-xs">{getConnectionText()}</span>
          </div>
        </div>
        <div className="flex items-center space-x-2">
          <button
            onClick={() => setIsMinimized(!isMinimized)}
            className="p-1 hover:bg-white/20 rounded transition-colors"
          >
            {isMinimized ? <Maximize2 className="h-4 w-4" /> : <Minimize2 className="h-4 w-4" />}
          </button>
          <button
            onClick={() => setIsOpen(false)}
            className="p-1 hover:bg-white/20 rounded transition-colors"
          >
            <X className="h-4 w-4" />
          </button>
        </div>
      </div>

      {!isMinimized && (
        <>
          {/* Messages */}
          <div className="h-64 overflow-y-auto p-4 space-y-3 bg-gray-50">
            {/* Connection Error */}
            {connectionError && (
              <div className="bg-red-50 border border-red-200 rounded-lg p-3 text-center">
                <p className="text-sm text-red-600 mb-2">{connectionError}</p>
                <button
                  onClick={retryConnection}
                  className="text-xs bg-red-600 text-white px-3 py-1 rounded hover:bg-red-700"
                >
                  Tekrar Dene
                </button>
              </div>
            )}

            {messages.map((message) => (
              <div
                key={message.id}
                className={`flex ${message.isFromSupport ? 'justify-start' : 'justify-end'}`}
              >
                <div className={`max-w-xs px-3 py-2 rounded-lg ${
                  message.isFromSupport
                    ? 'bg-white text-gray-800 rounded-bl-none shadow-sm border'
                    : 'bg-blue-600 text-white rounded-br-none'
                }`}>
                  {message.isFromSupport && (
                    <div className="flex items-center space-x-1 mb-1">
                      <Bot className="h-3 w-3 text-blue-600" />
                      <span className="text-xs text-gray-500 font-medium">Destek</span>
                    </div>
                  )}
                  {!message.isFromSupport && (
                    <div className="flex items-center space-x-1 mb-1">
                      <User className="h-3 w-3" />
                      <span className="text-xs opacity-75 font-medium">{message.userName}</span>
                    </div>
                  )}
                  <p className="text-sm">{message.message}</p>
                  <div className={`text-xs mt-1 ${
                    message.isFromSupport ? 'text-gray-400' : 'text-blue-100'
                  }`}>
                    {formatTime(message.timestamp)}
                  </div>
                </div>
              </div>
            ))}
            
            {isLoading && (
              <div className="flex justify-start">
                <div className="bg-white text-gray-800 rounded-lg rounded-bl-none shadow-sm border px-3 py-2">
                  <div className="flex items-center space-x-1">
                    <Bot className="h-3 w-3 text-blue-600" />
                    <span className="text-xs text-gray-500 font-medium">Destek</span>
                  </div>
                  <div className="flex space-x-1 mt-1">
                    <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce"></div>
                    <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '0.1s' }}></div>
                    <div className="w-2 h-2 bg-gray-400 rounded-full animate-bounce" style={{ animationDelay: '0.2s' }}></div>
                  </div>
                </div>
              </div>
            )}
            
            <div ref={messagesEndRef} />
          </div>

          {/* Input */}
          <div className="p-4 border-t border-gray-200">
            {!isAuthenticated ? (
              <div className="text-center">
                <p className="text-sm text-gray-600 mb-2">
                  Mesaj göndermek için giriş yapın
                </p>
                <button
                  onClick={() => window.location.href = '/login'}
                  className="text-blue-600 hover:text-blue-700 text-sm font-medium"
                >
                  Giriş Yap
                </button>
              </div>
            ) : !isConnected && !connectionError ? (
              <div className="text-center">
                <p className="text-sm text-gray-600 mb-2">
                  Chat bağlantısı kuruluyor...
                </p>
                <div className="animate-spin rounded-full h-4 w-4 border-2 border-blue-600 border-t-transparent mx-auto"></div>
              </div>
            ) : connectionError ? (
              <div className="text-center">
                <p className="text-sm text-red-600 mb-2">
                  {connectionError}
                </p>
                <button
                  onClick={retryConnection}
                  className="text-sm bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
                >
                  Bağlantıyı Yenile
                </button>
              </div>
            ) : (
              <div className="flex space-x-2">
                <input
                  ref={inputRef}
                  type="text"
                  value={inputMessage}
                  onChange={(e) => setInputMessage(e.target.value)}
                  onKeyPress={handleKeyPress}
                  placeholder="Mesajınızı yazın..."
                  disabled={isLoading}
                  className="flex-1 px-3 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent text-sm disabled:opacity-50"
                />
                <button
                  onClick={sendMessage}
                  disabled={!inputMessage.trim() || isLoading}
                  className="bg-blue-600 text-white p-2 rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                >
                  <Send className="h-4 w-4" />
                </button>
              </div>
            )}
          </div>
        </>
      )}
    </div>
  );
};