import React from 'react';
import { 
  TrendingUp, 
  DollarSign, 
  ShoppingCart, 
  Users,
  Package,
  AlertCircle,
  CheckCircle,
  Clock
} from 'lucide-react';
import StatsCard from './StatsCard';
import Chart from './Chart';

const Dashboard: React.FC = () => {
  const stats = [
    {
      title: 'Total Revenue',
      value: '$45,231.89',
      change: '+20.1%',
      trend: 'up',
      icon: DollarSign,
      color: 'blue'
    },
    {
      title: 'Orders',
      value: '2,350',
      change: '+180.1%',
      trend: 'up',
      icon: ShoppingCart,
      color: 'emerald'
    },
    {
      title: 'Products',
      value: '12,234',
      change: '+19%',
      trend: 'up',
      icon: Package,
      color: 'purple'
    },
    {
      title: 'Active Users',
      value: '573',
      change: '+201',
      trend: 'up',
      icon: Users,
      color: 'orange'
    }
  ];

  const recentOrders = [
    { id: '#3210', customer: 'Olivia Martin', status: 'Completed', amount: '$1,999.00', date: '2025-01-15' },
    { id: '#3209', customer: 'Jackson Lee', status: 'Processing', amount: '$39.00', date: '2025-01-15' },
    { id: '#3208', customer: 'Isabella Nguyen', status: 'Completed', amount: '$299.00', date: '2025-01-14' },
    { id: '#3207', customer: 'William Kim', status: 'Pending', amount: '$99.00', date: '2025-01-14' },
    { id: '#3206', customer: 'Sofia Davis', status: 'Completed', amount: '$39.00', date: '2025-01-13' }
  ];

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Completed':
        return <CheckCircle className="h-4 w-4 text-emerald-500" />;
      case 'Processing':
        return <Clock className="h-4 w-4 text-blue-500" />;
      case 'Pending':
        return <AlertCircle className="h-4 w-4 text-yellow-500" />;
      default:
        return <Clock className="h-4 w-4 text-gray-500" />;
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Completed':
        return 'bg-emerald-50 text-emerald-700 border-emerald-200';
      case 'Processing':
        return 'bg-blue-50 text-blue-700 border-blue-200';
      case 'Pending':
        return 'bg-yellow-50 text-yellow-700 border-yellow-200';
      default:
        return 'bg-gray-50 text-gray-700 border-gray-200';
    }
  };

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div>
        <h2 className="text-3xl font-bold text-gray-900">Dashboard</h2>
        <p className="mt-2 text-gray-600">
          Welcome back! Here's what's happening with your store today.
        </p>
      </div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {stats.map((stat, index) => (
          <StatsCard key={index} {...stat} />
        ))}
      </div>

      {/* Charts and Tables */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        {/* Revenue Chart */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
          <div className="flex items-center justify-between mb-6">
            <h3 className="text-lg font-semibold text-gray-900">Revenue Overview</h3>
            <TrendingUp className="h-5 w-5 text-emerald-500" />
          </div>
          <Chart />
        </div>

        {/* Recent Orders */}
        <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
          <div className="flex items-center justify-between mb-6">
            <h3 className="text-lg font-semibold text-gray-900">Recent Orders</h3>
            <button className="text-sm text-blue-600 hover:text-blue-700 font-medium">
              View all
            </button>
          </div>
          <div className="space-y-4">
            {recentOrders.map((order) => (
              <div key={order.id} className="flex items-center justify-between p-3 hover:bg-gray-50 rounded-lg transition-colors">
                <div className="flex items-center space-x-3">
                  <div className="flex-shrink-0">
                    {getStatusIcon(order.status)}
                  </div>
                  <div>
                    <p className="text-sm font-medium text-gray-900">{order.customer}</p>
                    <p className="text-xs text-gray-500">{order.id} • {order.date}</p>
                  </div>
                </div>
                <div className="text-right">
                  <p className="text-sm font-medium text-gray-900">{order.amount}</p>
                  <span className={`inline-flex items-center px-2 py-1 rounded-full text-xs font-medium border ${getStatusColor(order.status)}`}>
                    {order.status}
                  </span>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      {/* Additional Metrics */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
          <h4 className="text-sm font-medium text-gray-500 mb-2">Conversion Rate</h4>
          <div className="flex items-center space-x-2">
            <span className="text-2xl font-bold text-gray-900">3.2%</span>
            <span className="text-sm text-emerald-600 font-medium">+0.5%</span>
          </div>
          <div className="mt-4 bg-gray-200 rounded-full h-2">
            <div className="bg-blue-600 h-2 rounded-full" style={{ width: '32%' }}></div>
          </div>
        </div>

        <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
          <h4 className="text-sm font-medium text-gray-500 mb-2">Average Order Value</h4>
          <div className="flex items-center space-x-2">
            <span className="text-2xl font-bold text-gray-900">$127</span>
            <span className="text-sm text-emerald-600 font-medium">+12%</span>
          </div>
          <div className="mt-4 bg-gray-200 rounded-full h-2">
            <div className="bg-emerald-600 h-2 rounded-full" style={{ width: '65%' }}></div>
          </div>
        </div>

        <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
          <h4 className="text-sm font-medium text-gray-500 mb-2">Customer Satisfaction</h4>
          <div className="flex items-center space-x-2">
            <span className="text-2xl font-bold text-gray-900">4.8</span>
            <span className="text-sm text-emerald-600 font-medium">+0.2</span>
          </div>
          <div className="mt-4 bg-gray-200 rounded-full h-2">
            <div className="bg-yellow-500 h-2 rounded-full" style={{ width: '96%' }}></div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;