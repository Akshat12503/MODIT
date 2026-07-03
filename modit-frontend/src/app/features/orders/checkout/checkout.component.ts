import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.scss'
})
export class CheckoutComponent {
  public cartService = inject(CartService);

  paymentMethod = signal<'BNPL' | 'NetBanking' | 'UPI'>('BNPL');
  gstNumber = signal('07AAAAA0000A1Z5');
  
  isProcessing = signal(false);
  orderComplete = signal(false);

  placeOrder() {
    this.isProcessing.set(true);
    
    setTimeout(() => {
      this.isProcessing.set(false);
      this.orderComplete.set(true);
      this.cartService.clearCart();
    }, 2000);
  }
}