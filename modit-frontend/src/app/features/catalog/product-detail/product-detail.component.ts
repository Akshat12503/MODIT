import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

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
  
  // Mocking the selected product for now
  product = signal({
    name: 'UltraTech Portland Cement',
    category: 'Cement',
    image: '🧱',
    unit: 'bag',
    description: 'High-quality ordinary portland cement suitable for all general construction purposes.'
  });

  // Mocking vendor quotes for the price comparison feature
  quotes = signal<VendorQuote[]>([
    { id: 'v1', vendorName: 'Delhi NCR Builders Hub', pricePerUnit: 380, deliveryTimeDays: 2, rating: 4.8, isAiRecommended: true },
    { id: 'v2', vendorName: 'Gurugram Cement Suppliers', pricePerUnit: 375, deliveryTimeDays: 5, rating: 4.2, isAiRecommended: false },
    { id: 'v3', vendorName: 'Noida Construct-All', pricePerUnit: 390, deliveryTimeDays: 1, rating: 4.9, isAiRecommended: false },
  ]);

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    // In a real app, we would fetch the product details using this ID via HttpClient
    this.productId.set(this.route.snapshot.paramMap.get('id'));
  }

  requestNegotiation(vendorId: string) {
    alert('Agentic AI is initiating automated negotiation with vendor: ' + vendorId);
  }
}