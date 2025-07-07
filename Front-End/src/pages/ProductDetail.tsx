import React, { useEffect, useState } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { 
  Heart, 
  ShoppingCart, 
  Star, 
  ChevronLeft, 
  ChevronRight, 
  Minus, 
  Plus,
  Truck,
  Shield,
  RefreshCw,
  ArrowLeft,
  Share2
} from 'lucide-react';
import { Product } from '../types';
import { apiService } from '../services/api';
import { useCartStore } from '../store/cartStore';
import toast from 'react-hot-toast';

export const ProductDetail: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [selectedVariant, setSelectedVariant] = useState<any>(null);
  const [quantity, setQuantity] = useState(1);
  const [isWishlisted, setIsWishlisted] = useState(false);
  const { addItem, isLoading: cartLoading } = useCartStore();

  useEffect(() => {
    if (id) {
      loadProduct(id);
    }
  }, [id]);

  const loadProduct = async (productId: string) => {
    try {
      setLoading(true);
      const response = await apiService.getProduct(productId);
      setProduct(response);
      
      // Set default variant and main image
      if (response.variants && response.variants.length > 0) {
        setSelectedVariant(response.variants[0]);
      }
      
      // Set main image as first image
      if (response.images && response.images.length > 0) {
        const mainImageIndex = response.images.findIndex(img => img.isMain);
        setCurrentImageIndex(mainImageIndex >= 0 ? mainImageIndex : 0);
      }
    } catch (error) {
      console.error('Failed to load product:', error);
      toast.error('Product not found');
      navigate('/products');
    } finally {
      setLoading(false);
    }
  };

  const handleAddToCart = async () => {
    if (!product || !selectedVariant) return;
    
    try {
      await addItem(product, quantity, {
        selectedVariant: selectedVariant,
      });
    } catch (error) {
      console.error('Failed to add to cart:', error);
    }
  };

  const nextImage = () => {
    if (product?.images) {
      setCurrentImageIndex((prev) => 
        prev === product.images!.length - 1 ? 0 : prev + 1
      );
    }
  };

  const prevImage = () => {
    if (product?.images) {
      setCurrentImageIndex((prev) => 
        prev === 0 ? product.images!.length - 1 : prev - 1
      );
    }
  };

  const handleQuantityChange = (change: number) => {
    const newQuantity = quantity + change;
    if (newQuantity >= 1 && newQuantity <= (selectedVariant?.stockQuantity || 0)) {
      setQuantity(newQuantity);
    }
  };

  if (loading) {
    return (
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
        <div className="animate-pulse">
          <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
            <div className="aspect-square bg-gray-200 rounded-lg"></div>
            <div className="space-y-4">
              <div className="h-8 bg-gray-200 rounded"></div>
              <div className="h-4 bg-gray-200 rounded w-3/4"></div>
              <div className="h-6 bg-gray-200 rounded w-1/2"></div>
              <div className="h-20 bg-gray-200 rounded"></div>
            </div>
          </div>
        </div>
      </div>
    );
  }

  if (!product) {
    return (
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-16 text-center">
        <h1 className="text-2xl font-bold text-gray-900">Product not found</h1>
        <Link to="/products" className="mt-4 text-blue-600 hover:text-blue-700">
          Back to Products
        </Link>
      </div>
    );
  }

  const currentImage = product.images?.[currentImageIndex];
  const price = selectedVariant?.price || 0;
  const stock = selectedVariant?.stockQuantity || 0;
  const isInStock = stock > 0;

  // Mock rating data
  const rating = 4.2 + Math.random() * 0.6;
  const reviewCount = Math.floor(Math.random() * 200) + 10;

  return (
    <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
      {/* Breadcrumb */}
      <nav className="flex items-center space-x-2 text-sm text-gray-600 mb-8">
        <Link to="/" className="hover:text-gray-900">Home</Link>
        <span>/</span>
        <Link to="/products" className="hover:text-gray-900">Products</Link>
        <span>/</span>
        <Link to={`/categories/${product.category?.slug}`} className="hover:text-gray-900">
          {product.category?.name}
        </Link>
        <span>/</span>
        <span className="text-gray-900">{product.name}</span>
      </nav>

      {/* Back Button */}
      <button
        onClick={() => navigate(-1)}
        className="flex items-center space-x-2 text-gray-600 hover:text-gray-900 mb-6"
      >
        <ArrowLeft className="h-4 w-4" />
        <span>Back</span>
      </button>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-12">
        {/* Image Gallery */}
        <div className="space-y-4">
          {/* Main Image */}
          <div className="relative aspect-square bg-gray-100 rounded-lg overflow-hidden group">
            <img
              src={currentImage?.imageUrl || 'https://images.pexels.com/photos/230544/pexels-photo-230544.jpeg'}
              alt={product.name}
              className="w-full h-full object-cover"
              onError={(e) => {
                e.currentTarget.src = 'https://images.pexels.com/photos/230544/pexels-photo-230544.jpeg';
              }}
            />
            
            {/* Navigation Arrows */}
            {product.images && product.images.length > 1 && (
              <>
                <button
                  onClick={prevImage}
                  className="absolute left-4 top-1/2 transform -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 opacity-0 group-hover:opacity-100 transition-opacity"
                >
                  <ChevronLeft className="h-5 w-5" />
                </button>
                <button
                  onClick={nextImage}
                  className="absolute right-4 top-1/2 transform -translate-y-1/2 bg-white/80 hover:bg-white rounded-full p-2 opacity-0 group-hover:opacity-100 transition-opacity"
                >
                  <ChevronRight className="h-5 w-5" />
                </button>
              </>
            )}

            {/* Image Counter */}
            {product.images && product.images.length > 1 && (
              <div className="absolute bottom-4 right-4 bg-black/50 text-white px-3 py-1 rounded-full text-sm">
                {currentImageIndex + 1} / {product.images.length}
              </div>
            )}
          </div>

          {/* Thumbnail Images */}
          {product.images && product.images.length > 1 && (
            <div className="grid grid-cols-4 gap-2">
              {product.images.map((image, index) => (
                <button
                  key={image.id}
                  onClick={() => setCurrentImageIndex(index)}
                  className={`aspect-square rounded-lg overflow-hidden border-2 ${
                    index === currentImageIndex
                      ? 'border-blue-600'
                      : 'border-gray-200 hover:border-gray-300'
                  }`}
                >
                  <img
                    src={image.imageUrl}
                    alt={`${product.name} ${index + 1}`}
                    className="w-full h-full object-cover"
                    onError={(e) => {
                      e.currentTarget.src = 'https://images.pexels.com/photos/230544/pexels-photo-230544.jpeg';
                    }}
                  />
                </button>
              ))}
            </div>
          )}
        </div>

        {/* Product Info */}
        <div className="space-y-6">
          {/* Header */}
          <div>
            <div className="flex items-center justify-between mb-2">
              <Link
                to={`/brands/${product.brand?.id}`}
                className="text-blue-600 hover:text-blue-700 font-medium"
              >
                {product.brand?.name}
              </Link>
              <div className="flex items-center space-x-2">
                <button
                  onClick={() => setIsWishlisted(!isWishlisted)}
                  className={`p-2 rounded-full ${
                    isWishlisted
                      ? 'bg-red-100 text-red-600'
                      : 'bg-gray-100 text-gray-600 hover:bg-gray-200'
                  }`}
                >
                  <Heart className={`h-5 w-5 ${isWishlisted ? 'fill-current' : ''}`} />
                </button>
                <button className="p-2 rounded-full bg-gray-100 text-gray-600 hover:bg-gray-200">
                  <Share2 className="h-5 w-5" />
                </button>
              </div>
            </div>
            
            <h1 className="text-3xl font-bold text-gray-900 mb-2">{product.name}</h1>
            <p className="text-gray-600 mb-4">Product Code: {product.code}</p>

            {/* Rating */}
            <div className="flex items-center space-x-2 mb-4">
              <div className="flex items-center">
                {[...Array(5)].map((_, i) => (
                  <Star
                    key={i}
                    className={`h-5 w-5 ${
                      i < Math.floor(rating)
                        ? 'text-yellow-400 fill-current'
                        : 'text-gray-300'
                    }`}
                  />
                ))}
              </div>
              <span className="text-sm text-gray-600">
                {rating.toFixed(1)} ({reviewCount} reviews)
              </span>
            </div>
          </div>

          {/* Price */}
          <div className="border-t border-b border-gray-200 py-6">
            <div className="flex items-center space-x-4">
              <span className="text-3xl font-bold text-gray-900">
                ${price.toFixed(2)}
              </span>
              {selectedVariant?.originalPrice && (
                <span className="text-xl text-gray-500 line-through">
                  ${selectedVariant.originalPrice.toFixed(2)}
                </span>
              )}
            </div>
            
            {/* Stock Status */}
            <div className="mt-2">
              {isInStock ? (
                <span className="text-green-600 font-medium">
                  ✓ In Stock ({stock} available)
                </span>
              ) : (
                <span className="text-red-600 font-medium">
                  ✗ Out of Stock
                </span>
              )}
            </div>
          </div>

          {/* Variants */}
          {product.variants && product.variants.length > 1 && (
            <div>
              <h3 className="text-lg font-medium text-gray-900 mb-3">Variants</h3>
              <div className="grid grid-cols-2 gap-2">
                {product.variants.map((variant) => (
                  <button
                    key={variant.id}
                    onClick={() => setSelectedVariant(variant)}
                    className={`p-3 border rounded-lg text-left ${
                      selectedVariant?.id === variant.id
                        ? 'border-blue-600 bg-blue-50'
                        : 'border-gray-200 hover:border-gray-300'
                    }`}
                  >
                    <div className="font-medium">{variant.sku}</div>
                    <div className="text-sm text-gray-600">${variant.price.toFixed(2)}</div>
                    <div className="text-xs text-gray-500">Stock: {variant.stockQuantity}</div>
                  </button>
                ))}
              </div>
            </div>
          )}

          {/* Quantity and Add to Cart */}
          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium text-gray-700 mb-2">
                Quantity
              </label>
              <div className="flex items-center space-x-3">
                <button
                  onClick={() => handleQuantityChange(-1)}
                  disabled={quantity <= 1}
                  className="p-2 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <Minus className="h-4 w-4" />
                </button>
                <span className="w-12 text-center font-medium">{quantity}</span>
                <button
                  onClick={() => handleQuantityChange(1)}
                  disabled={quantity >= stock}
                  className="p-2 border border-gray-300 rounded-lg hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  <Plus className="h-4 w-4" />
                </button>
              </div>
            </div>

            <div className="flex space-x-4">
              <button
                onClick={handleAddToCart}
                disabled={!isInStock || cartLoading}
                className="flex-1 bg-blue-600 text-white py-3 px-6 rounded-lg font-medium hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed flex items-center justify-center space-x-2"
              >
                {cartLoading ? (
                  <div className="animate-spin rounded-full h-5 w-5 border-2 border-white border-t-transparent"></div>
                ) : (
                  <>
                    <ShoppingCart className="h-5 w-5" />
                    <span>Add to Cart</span>
                  </>
                )}
              </button>
              <button className="px-6 py-3 border border-gray-300 rounded-lg font-medium hover:bg-gray-50">
                Buy Now
              </button>
            </div>
          </div>

          {/* Features */}
          <div className="grid grid-cols-3 gap-4 pt-6 border-t border-gray-200">
            <div className="text-center">
              <Truck className="h-8 w-8 text-blue-600 mx-auto mb-2" />
              <div className="text-sm font-medium">Free Shipping</div>
              <div className="text-xs text-gray-500">On orders over $50</div>
            </div>
            <div className="text-center">
              <Shield className="h-8 w-8 text-green-600 mx-auto mb-2" />
              <div className="text-sm font-medium">Warranty</div>
              <div className="text-xs text-gray-500">1 year warranty</div>
            </div>
            <div className="text-center">
              <RefreshCw className="h-8 w-8 text-purple-600 mx-auto mb-2" />
              <div className="text-sm font-medium">Easy Returns</div>
              <div className="text-xs text-gray-500">30-day returns</div>
            </div>
          </div>
        </div>
      </div>

      {/* Description */}
      <div className="mt-16">
        <div className="border-b border-gray-200">
          <nav className="-mb-px flex space-x-8">
            <button className="border-b-2 border-blue-600 py-4 px-1 text-sm font-medium text-blue-600">
              Description
            </button>
            <button className="border-b-2 border-transparent py-4 px-1 text-sm font-medium text-gray-500 hover:text-gray-700">
              Specifications
            </button>
            <button className="border-b-2 border-transparent py-4 px-1 text-sm font-medium text-gray-500 hover:text-gray-700">
              Reviews ({reviewCount})
            </button>
          </nav>
        </div>
        
        <div className="py-8">
          <div className="prose max-w-none">
            <p className="text-gray-700 leading-relaxed">
              {product.description}
            </p>
            
            {/* Category Info */}
            {product.category && (
              <div className="mt-6 p-4 bg-gray-50 rounded-lg">
                <h4 className="font-medium text-gray-900 mb-2">Category</h4>
                <Link
                  to={`/categories/${product.category.slug}`}
                  className="text-blue-600 hover:text-blue-700"
                >
                  {product.category.name}
                </Link>
                {product.category.description && (
                  <p className="text-sm text-gray-600 mt-1">
                    {product.category.description}
                  </p>
                )}
              </div>
            )}

            {/* Brand Info */}
            {product.brand && (
              <div className="mt-4 p-4 bg-gray-50 rounded-lg">
                <h4 className="font-medium text-gray-900 mb-2">Brand</h4>
                <div className="flex items-center space-x-3">
                  {product.brand.logoUrl && (
                    <img
                      src={product.brand.logoUrl}
                      alt={product.brand.name}
                      className="w-12 h-12 object-contain"
                      onError={(e) => {
                        e.currentTarget.style.display = 'none';
                      }}
                    />
                  )}
                  <div>
                    <Link
                      to={`/brands/${product.brand.id}`}
                      className="text-blue-600 hover:text-blue-700 font-medium"
                    >
                      {product.brand.name}
                    </Link>
                    {product.brand.description && (
                      <p className="text-sm text-gray-600">
                        {product.brand.description}
                      </p>
                    )}
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};