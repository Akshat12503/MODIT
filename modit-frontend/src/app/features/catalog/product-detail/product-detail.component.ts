import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CartService } from '../../../core/services/cart.service';

interface VendorQuote {
  id: string;
  vendorName: string;
  pricePerUnit: number;
  deliveryTimeDays: number;
  rating: number;
  isAiRecommended: boolean;
}

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss'
})
export class ProductDetailComponent implements OnInit {
  productId = signal<string | null>(null);
  
  product = signal({
    name: 'UltraTech Portland Cement',
    category: 'Cement',
    image: '🧱',
    unit: 'bag',
    description: 'High-quality ordinary portland cement suitable for all general construction purposes.'
  });

  quotes = signal<VendorQuote[]>([
    { id: 'v1', vendorName: 'Delhi NCR Builders Hub', pricePerUnit: 380, deliveryTimeDays: 2, rating: 4.8, isAiRecommended: true },
    { id: 'v2', vendorName: 'Gurugram Cement Suppliers', pricePerUnit: 375, deliveryTimeDays: 5, rating: 4.2, isAiRecommended: false },
    { id: 'v3', vendorName: 'Noida Construct-All', pricePerUnit: 390, deliveryTimeDays: 1, rating: 4.9, isAiRecommended: false },
  ]);

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private cartService: CartService
    
  ) {}

  ngOnInit() {
    this.productId.set(this.route.snapshot.paramMap.get('id'));
  }

  addToCart(quote: VendorQuote) {
    this.cartService.addToCart({
      id: Math.random().toString(36).substring(2, 9),
      productId: this.productId() || 'unknown',
      name: this.product().name,
      vendorName: quote.vendorName,
      price: quote.pricePerUnit,
      quantity: 100, // Default bulk quantity
      unit: this.product().unit
    });
    
    alert(`${this.product().name} added to cart!`);
  }

  requestNegotiation(vendorId: string) {
    alert('Agentic AI is initiating automated negotiation with vendor: ' + vendorId);
  }
}