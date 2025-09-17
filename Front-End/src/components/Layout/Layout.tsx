import React from 'react';
import { Header } from './Header';
import { Footer } from './Footer';
import { ChatPopup } from '../Chat/ChatPopup';

interface LayoutProps {
  children: React.ReactNode;
}

export const Layout: React.FC<LayoutProps> = ({ children }) => {
  return (
    <div className="min-h-screen flex flex-col">
      <Header />
      <main className="flex-1">
        {children}
      </main>
      <Footer />
      <ChatPopup />
    </div>
  );
};