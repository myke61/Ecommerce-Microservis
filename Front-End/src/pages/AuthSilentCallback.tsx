import React, { useEffect } from 'react';
import { authService } from '../services/authService';

export const AuthSilentCallback: React.FC = () => {
  useEffect(() => {
    const handleSilentCallback = async () => {
      try {
        await authService.handleSilentCallback();
      } catch (error) {
        console.error('Silent callback error:', error);
      }
    };

    handleSilentCallback();
  }, []);

  return (
    <div style={{ display: 'none' }}>
      Silent callback processing...
    </div>
  );
};