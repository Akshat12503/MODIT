export enum UserRole {
  Customer = 0,
  Contractor = 1,
  Architect = 2,
  Supplier = 3,
  Admin = 4
}

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  phone: string;
  role: UserRole;
  companyName?: string;
  businessType?: string;
  gstNumber?: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface AuthResponse {
  userId: number;
  name: string;
  email: string;
  role: string;
  token: string;
}

export interface Category {
  id: number;
  name: string;
  parentCategoryId?: number;
}

export interface VendorOffer {
  vendorProductId: number;
  vendorId: number;
  vendorShopName: string;
  price: number;
  stockQty: number;
  minOrderQty: number;
  isAvailable: boolean;
  vendorRating: number;
}

export interface Product {
  id: number;
  name: string;
  categoryName: string;
  unit: string;
  description?: string;
  imageUrl?: string;
  brandName?: string;
  vendorOffers: VendorOffer[];
}

export interface Vendor {
  id: number;
  userId: number;
  shopName: string;
  serviceZones: string;
  rating: number;
  deliveryRadiusKm: number;
  isApproved: boolean;
}

export interface CartItem {
  cartItemId: number;
  vendorProductId: number;
  productName: string;
  vendorShopName: string;
  price: number;
  quantity: number;
  subtotal: number;
}

export interface Cart {
  cartId: number;
  items: CartItem[];
  totalAmount: number;
}

export enum PaymentMode {
  Prepaid = 0,
  COD = 1,
  Credit = 2
}

export interface OrderItem {
  productName: string;
  vendorShopName: string;
  quantity: number;
  priceAtOrder: number;
  subtotal: number;
}

export interface Order {
  id: number;
  totalAmount: number;
  status: string;
  deliveryAddress: string;
  paymentMode: string;
  createdAt: string;
  items: OrderItem[];
}

export interface RequiredMaterialItem {
  productName: string;
  quantity: number;
  unit: string;
}

export interface QuotationResponse {
  id: number;
  vendorId: number;
  vendorShopName: string;
  vendorRating: number;
  quotedPrice: number;
  deliveryTimeEstimate: string;
  notes?: string;
  status: string;
  createdAt: string;
}

export interface QuotationRequest {
  id: number;
  projectDescription: string;
  requiredMaterials: RequiredMaterialItem[];
  deadline: string;
  status: string;
  createdAt: string;
  responses: QuotationResponse[];
}