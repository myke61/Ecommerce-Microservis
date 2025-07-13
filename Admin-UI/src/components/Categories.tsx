import React, { useState } from 'react';
import { Plus, Search, Edit, Trash2, Tag } from 'lucide-react';

const Categories: React.FC = () => {
  const [searchTerm, setSearchTerm] = useState('');
  
  const categories = [
    {
      id: 1,
      name: 'Electronics',
      description: 'Gadgets, devices and electronic accessories',
      productCount: 156,
      status: 'Active',
      color: 'blue'
    },
    {
      id: 2,
      name: 'Clothing',
      description: 'Fashion apparel and accessories',
      productCount: 89,
      status: 'Active',
      color: 'purple'
    },
    {
      id: 3,
      name: 'Furniture',
      description: 'Home and office furniture',
      productCount: 34,
      status: 'Active',
      color: 'emerald'
    },
    {
      id: 4,
      name: 'Books',
      description: 'Educational and entertainment books',
      productCount: 267,
      status: 'Active',
      color: 'orange'
    },
    {
      id: 5,
      name: 'Sports',
      description: 'Sports equipment and fitness gear',
      productCount: 45,
      status: 'Inactive',
      color: 'red'
    },
    {
      id: 6,
      name: 'Beauty',
      description: 'Cosmetics and personal care products',
      productCount: 78,
      status: 'Active',
      color: 'pink'
    }
  ];

  const getColorClasses = (color: string) => {
    const colors = {
      blue: 'bg-blue-100 text-blue-800',
      purple: 'bg-purple-100 text-purple-800',
      emerald: 'bg-emerald-100 text-emerald-800',
      orange: 'bg-orange-100 text-orange-800',
      red: 'bg-red-100 text-red-800',
      pink: 'bg-pink-100 text-pink-800'
    };
    return colors[color as keyof typeof colors] || 'bg-gray-100 text-gray-800';
  };

  const getStatusColor = (status: string) => {
    return status === 'Active' 
      ? 'bg-emerald-50 text-emerald-700 border-emerald-200'
      : 'bg-gray-50 text-gray-700 border-gray-200';
  };

  const filteredCategories = categories.filter(category =>
    category.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    category.description.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h2 className="text-3xl font-bold text-gray-900">Categories</h2>
          <p className="mt-2 text-gray-600">Organize your products into categories</p>
        </div>
        <button className="mt-4 sm:mt-0 inline-flex items-center px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition-colors duration-200">
          <Plus className="h-4 w-4 mr-2" />
          Add Category
        </button>
      </div>

      {/* Search Bar */}
      <div className="bg-white p-4 rounded-xl shadow-sm border border-gray-200">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-4 w-4 text-gray-400" />
          <input
            type="text"
            placeholder="Search categories..."
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
          />
        </div>
      </div>

      {/* Categories Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredCategories.map((category) => (
          <div key={category.id} className="bg-white p-6 rounded-xl shadow-sm border border-gray-200 hover:shadow-md transition-shadow duration-200">
            <div className="flex items-start justify-between mb-4">
              <div className={`p-3 rounded-lg ${getColorClasses(category.color)}`}>
                <Tag className="h-6 w-6" />
              </div>
              <div className="flex space-x-2">
                <button className="text-gray-400 hover:text-blue-600 transition-colors">
                  <Edit className="h-4 w-4" />
                </button>
                <button className="text-gray-400 hover:text-red-600 transition-colors">
                  <Trash2 className="h-4 w-4" />
                </button>
              </div>
            </div>
            
            <h3 className="text-lg font-semibold text-gray-900 mb-2">{category.name}</h3>
            <p className="text-sm text-gray-600 mb-4">{category.description}</p>
            
            <div className="flex items-center justify-between">
              <div>
                <span className="text-2xl font-bold text-gray-900">{category.productCount}</span>
                <p className="text-xs text-gray-500">Products</p>
              </div>
              <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium border ${getStatusColor(category.status)}`}>
                {category.status}
              </span>
            </div>
          </div>
        ))}
      </div>

      {/* Summary Stats */}
      <div className="bg-white p-6 rounded-xl shadow-sm border border-gray-200">
        <h3 className="text-lg font-semibold text-gray-900 mb-4">Category Summary</h3>
        <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
          <div className="text-center">
            <div className="text-2xl font-bold text-blue-600">{categories.length}</div>
            <div className="text-sm text-gray-500">Total Categories</div>
          </div>
          <div className="text-center">
            <div className="text-2xl font-bold text-emerald-600">
              {categories.filter(c => c.status === 'Active').length}
            </div>
            <div className="text-sm text-gray-500">Active Categories</div>
          </div>
          <div className="text-center">
            <div className="text-2xl font-bold text-purple-600">
              {categories.reduce((sum, c) => sum + c.productCount, 0)}
            </div>
            <div className="text-sm text-gray-500">Total Products</div>
          </div>
          <div className="text-center">
            <div className="text-2xl font-bold text-orange-600">
              {Math.round(categories.reduce((sum, c) => sum + c.productCount, 0) / categories.length)}
            </div>
            <div className="text-sm text-gray-500">Avg Products</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Categories;