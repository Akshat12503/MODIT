import { Component, signal, computed, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

export interface Product {
  id: string;
  name: string;
  category: string;
  image: string;
  basePrice: number;
  unit: string;
  vendorsCount: number;
}

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.scss'
})
export class ProductListComponent implements OnInit {
  // Signals for state management
  products = signal<Product[]>([]);
  searchQuery = signal('');
  selectedCategory = signal('All');

  categories = ['All', 'Cement', 'Steel', 'Bricks', 'Aggregates', 'Electrical'];

  // Computed signal to automatically filter products when search or category changes
  filteredProducts = computed(() => {
    const query = this.searchQuery().toLowerCase();
    const category = this.selectedCategory();
    
    return this.products().filter(p => {
      const matchesSearch = p.name.toLowerCase().includes(query);
      const matchesCategory = category === 'All' || p.category === category;
      return matchesSearch && matchesCategory;
    });
  });

  constructor(public auth: AuthService) {}

  ngOnInit() {
    // Mock data for the initial prototype UI
    this.products.set([
      { id: '1', name: 'UltraTech Portland Cement', category: 'Cement', image: '🧱', basePrice: 380, unit: 'bag', vendorsCount: 4 },
      { id: '2', name: 'Tata Tiscon 550SD TMT Bar (12mm)', category: 'Steel', image: '🏗️', basePrice: 65, unit: 'kg', vendorsCount: 6 },
      { id: '3', name: 'Red Clay Bricks (Class A)', category: 'Bricks', image: '🟥', basePrice: 8, unit: 'piece', vendorsCount: 12 },
      { id: '4', name: 'River Sand (Coarse)', category: 'Aggregates', image: '⏳', basePrice: 45, unit: 'cft', vendorsCount: 3 },
      { id: '5', name: 'Finolex Copper Wire (1.5 sq mm)', category: 'Electrical', image: '⚡', basePrice: 1250, unit: 'coil', vendorsCount: 5 },
      { id: '6', name: 'Ambuja PPC Cement', category: 'Cement', image: '🧱', basePrice: 365, unit: 'bag', vendorsCount: 2 }
    ]);
  }

  setCategory(cat: string) {
    this.selectedCategory.set(cat);
  }

  logout() {
    this.auth.logout();
  }
}