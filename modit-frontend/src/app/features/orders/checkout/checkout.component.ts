import { Component, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent {
  // Mock cart items based on our previous BOM/Product flows
  cartItems = signal([
    { id: '1', name: 'UltraTech Portland Cement', quantity: 100, price: 380, unit: 'bags' },
    { id: '2', name: 'Tata Tiscon 550SD TMT Bar (12mm)', quantity: 500, price: 65, unit: 'kg' }
  ]);

  paymentMethod = signal<'BNPL' | 'NetBanking' | 'UPI'>('BNPL');
  gstNumber = signal('07AAAAA0000A1Z5'); // Mock Delhi GSTIN
  
  isProcessing = signal(false);
  orderComplete = signal(false);

  // Clean, efficient reactive calculations using Signals
  subtotal = computed(() => this.cartItems().reduce((sum, item) => sum + (item.price * item.quantity), 0));
  
  // Standard 18% GST (9% CGST + 9% SGST) for construction materials
  cgst = computed(() => this.subtotal() * 0.09);
  sgst = computed(() => this.subtotal() * 0.09);
  
  grandTotal = computed(() => this.subtotal() + this.cgst() + this.sgst());

  placeOrder() {
    this.isProcessing.set(true);
    
    // Simulate API call to backend for order processing and credit check
    setTimeout(() => {
      this.isProcessing.set(false);
      this.orderComplete.set(true);
    }, 2000);
  }
}