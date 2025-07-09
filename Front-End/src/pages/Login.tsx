import React from 'react';
import { Link } from 'react-router-dom';
import { Package, LogIn, Shield, Lock, CheckCircle } from 'lucide-react';
import { useAuthStore } from '../store/authStore';
import toast from 'react-hot-toast';

export const Login: React.FC = () => {
  const { initiateLogin, isLoading } = useAuthStore();

  const handleLogin = async () => {
    try {
      await initiateLogin();
    } catch (error) {
      console.error('Login error:', error);
      toast.error('Failed to initiate login. Please try again.');
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
          Welcome to EcomStore
        </h2>
        <p className="mt-2 text-center text-sm text-gray-600">
          Sign in to access your account and start shopping{' '}
          <Link
            to="/"
            className="font-medium text-blue-600 hover:text-blue-500 transition-colors"
          >
            or continue browsing
          </Link>
        </p>
      </div>

      <div className="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
        <div className="bg-white py-8 px-4 shadow-xl sm:rounded-xl sm:px-10 border border-gray-100">
          <div className="space-y-6">
            <div className="text-center">
              <div className="flex justify-center mb-4">
                <div className="bg-gradient-to-r from-blue-100 to-purple-100 p-3 rounded-full">
                  <Shield className="h-8 w-8 text-blue-600" />
                </div>
              </div>
              <h3 className="text-lg font-semibold text-gray-900 mb-2">
                Secure Identity Server Login
              </h3>
              <p className="text-sm text-gray-600 mb-6">
                Click the button below to securely sign in through our Identity Server using 
                industry-standard OAuth2.0 and OpenID Connect protocols.
              </p>
            </div>

            <button
              onClick={handleLogin}
              disabled={isLoading}
              className="w-full flex justify-center items-center space-x-3 py-4 px-6 border border-transparent rounded-xl shadow-sm text-base font-semibold text-white bg-gradient-to-r from-blue-600 to-purple-600 hover:from-blue-700 hover:to-purple-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 disabled:opacity-50 disabled:cursor-not-allowed transition-all duration-300 transform hover:scale-105 hover:shadow-lg"
            >
              {isLoading ? (
                <>
                  <div className="animate-spin rounded-full h-5 w-5 border-2 border-white border-t-transparent"></div>
                  <span>Redirecting to Identity Server...</span>
                </>
              ) : (
                <>
                  <LogIn className="h-5 w-5" />
                  <span>Sign in with Identity Server</span>
                </>
              )}
            </button>

            {/* Security Features */}
            <div className="mt-8 bg-gray-50 rounded-lg p-4">
              <h4 className="text-sm font-medium text-gray-900 mb-3 flex items-center">
                <Lock className="h-4 w-4 mr-2 text-green-600" />
                Security Features
              </h4>
              <div className="space-y-2 text-xs text-gray-600">
                <div className="flex items-center">
                  <CheckCircle className="h-3 w-3 mr-2 text-green-500" />
                  <span>OAuth2.0 Authorization Code Flow</span>
                </div>
                <div className="flex items-center">
                  <CheckCircle className="h-3 w-3 mr-2 text-green-500" />
                  <span>OpenID Connect (OIDC) Protocol</span>
                </div>
                <div className="flex items-center">
                  <CheckCircle className="h-3 w-3 mr-2 text-green-500" />
                  <span>Automatic Token Renewal</span>
                </div>
                <div className="flex items-center">
                  <CheckCircle className="h-3 w-3 mr-2 text-green-500" />
                  <span>Session Monitoring</span>
                </div>
                <div className="flex items-center">
                  <CheckCircle className="h-3 w-3 mr-2 text-green-500" />
                  <span>Secure Token Storage</span>
                </div>
              </div>
            </div>

            {/* Info Box */}
            <div className="mt-6 p-4 bg-blue-50 rounded-lg border border-blue-200">
              <div className="flex">
                <div className="flex-shrink-0">
                  <Package className="h-5 w-5 text-blue-400" />
                </div>
                <div className="ml-3">
                  <h3 className="text-sm font-medium text-blue-800">
                    First time here?
                  </h3>
                  <div className="mt-2 text-sm text-blue-700">
                    <p>
                      You'll be redirected to our secure Identity Server where you can 
                      create an account or sign in with your existing credentials.
                    </p>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};