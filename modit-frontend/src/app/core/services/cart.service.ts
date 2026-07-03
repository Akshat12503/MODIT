import { Injectable, signal, computed } from '@angular/core';

export interface CartItem {
  id: string;
  productId: string;
  name: string;
  vendorName: string;
  price: number;
  quantity: number;
  unit: string;
}

@Injectable({ providedIn: 'root' })
export class CartService {
  items = signal<CartItem[]>([]);

  subtotal = computed(() => 
    this.items().reduce((sum, item) => sum + (item.price * item.quantity), 0)
  );
  
  cgst = computed(() => this.subtotal() * 0.09);
  sgst = computed(() => this.subtotal() * 0.09);
  grandTotal = computed(() => this.subtotal() + this.cgst() + this.sgst());

  addToCart(item: CartItem) {
    this.items.update(currentItems => {
      const existing = currentItems.find(i => i.productId === item.productId && i.vendorName === item.vendorName);
      if (existing) {
        return currentItems.map(i => 
          i.id === existing.id ? { ...i, quantity: i.quantity + item.quantity } : i
        );
      }
      return [...currentItems, item];
    });
  }

  clearCart() {
    this.items.set([]);
  }
}