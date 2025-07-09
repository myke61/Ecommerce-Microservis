import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Package, CheckCircle, AlertCircle } from 'lucide-react';
import { useAuthStore } from '../store/authStore';
import toast from 'react-hot-toast';

export const AuthCallback: React.FC = () => {
  const navigate = useNavigate();
  const { handleAuthCallback, isLoading } = useAuthStore();
  const [progress, setProgress] = useState(0);
  const [status, setStatus] = useState<'processing' | 'success' | 'error'>('processing');

  useEffect(() => {
    const processCallback = async () => {
      try {
        // Simulate progress for better UX
        const progressInterval = setInterval(() => {
          setProgress(prev => {
            if (prev >= 90) {
              clearInterval(progressInterval);
              return 90;
            }
            return prev + 10;
          });
        }, 200);

        await handleAuthCallback();
        
        clearInterval(progressInterval);
        setProgress(100);
        setStatus('success');
        
        toast.success('Successfully signed in!');
        
        // Small delay to show success state
        setTimeout(() => {
          navigate('/');
        }, 1500);
        
      } catch (error) {
        console.error('Auth callback error:', error);
        setStatus('error');
        toast.error('Authentication failed. Please try again.');
        
        setTimeout(() => {
          navigate('/login');
        }, 3000);
      }
    };

    processCallback();
  }, [handleAuthCallback, navigate]);

  const getStatusIcon = () => {
    switch (status) {
      case 'success':
        return <CheckCircle className="h-12 w-12 text-green-600" />;
      case 'error':
        return <AlertCircle className="h-12 w-12 text-red-600" />;
      default:
        return (
          <div className="animate-spin rounded-full h-12 w-12 border-4 border-blue-600 border-t-transparent"></div>
        );
    }
  };

  const getStatusMessage = () => {
    switch (status) {
      case 'success':
        return 'Authentication successful! Redirecting...';
      case 'error':
        return 'Authentication failed. Redirecting to login...';
      default:
        return 'Authenticating with Identity Server...';
    }
  };

  const getStatusColor = () => {
    switch (status) {
      case 'success':
        return 'text-green-600';
      case 'error':
        return 'text-red-600';
      default:
        return 'text-gray-600';
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-blue-50 via-white to-purple-50 flex flex-col justify-center py-12 sm:px-6 lg:px-8">
      <div className="sm:mx-auto sm:w-full sm:max-w-md">
        <div className="flex justify-center">
          <div className="bg-white p-3 rounded-full shadow-lg">
            <Package className="h-12 w-12 text-blue-600" />
          </div>
        </div>
        <h2 className="mt-6 text-center text-3xl font-bold text-gray-900">
          Processing Authentication
        </h2>
        <p className="mt-2 text-center text-sm text-gray-600">
          Please wait while we complete your sign-in process...
        </p>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-white py-8 px-4 shadow-xl sm:rounded-xl sm:px-10 border border-gray-100">
          <div className="flex flex-col items-center space-y-6">
            {/* Status Icon */}
            <div className="flex justify-center">
              {getStatusIcon()}
            </div>

            {/* Status Message */}
            <p className={`text-center font-medium ${getStatusColor()}`}>
              {getStatusMessage()}
            </p>
            
            {/* Progress Bar */}
            <div className="w-full bg-gray-200 rounded-full h-3 overflow-hidden">
              <div 
                className={`h-3 rounded-full transition-all duration-500 ease-out ${
                  status === 'success' 
                    ? 'bg-green-500' 
                    : status === 'error' 
                    ? 'bg-red-500' 
                    : 'bg-gradient-to-r from-blue-500 to-purple-500'
                }`}
                style={{ width: `${progress}%` }}
              ></div>
            </div>

            {/* Progress Percentage */}
            <div className="text-sm text-gray-500">
              {progress}% Complete
            </div>

            {/* Processing Steps */}
            {status === 'processing' && (
              <div className="w-full space-y-2">
                <div className="flex items-center text-sm text-gray-600">
                  <div className="w-2 h-2 bg-blue-500 rounded-full mr-3 animate-pulse"></div>
                  Verifying authorization code...
                </div>
                <div className="flex items-center text-sm text-gray-600">
                  <div className="w-2 h-2 bg-blue-500 rounded-full mr-3 animate-pulse"></div>
                  Exchanging tokens...
                </div>
                <div className="flex items-center text-sm text-gray-600">
                  <div className="w-2 h-2 bg-blue-500 rounded-full mr-3 animate-pulse"></div>
                  Loading user profile...
                </div>
              </div>
            )}

            {/* Success Message */}
            {status === 'success' && (
              <div className="text-center space-y-2">
                <p className="text-sm text-green-600 font-medium">
                  Welcome back! You have been successfully authenticated.
                </p>
                <p className="text-xs text-gray-500">
                  Redirecting you to the homepage...
                </p>
              </div>
            )}

            {/* Error Message */}
            {status === 'error' && (
              <div className="text-center space-y-2">
                <p className="text-sm text-red-600 font-medium">
                  Something went wrong during authentication.
                </p>
                <p className="text-xs text-gray-500">
                  You will be redirected to the login page to try again.
                </p>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};