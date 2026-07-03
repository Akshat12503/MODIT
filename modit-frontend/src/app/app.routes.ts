import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'products',
    loadComponent: () => import('./features/catalog/product-list/product-list.component').then(m => m.ProductListComponent)
  },
  {
    path: 'product/:id', // NEW ROUTE
    loadComponent: () => import('./features/catalog/product-detail/product-detail.component').then(m => m.ProductDetailComponent)
  },
  {
    path: 'bom-upload',
    loadComponent: () => import('./features/ai-tools/bom-upload/bom-upload.component').then(m => m.BomUploadComponent)
  },
  {
    path: 'vendor-dashboard',
    loadComponent: () => import('./features/vendor-dashboard/vendor-dashboard.component').then(m => m.VendorDashboardComponent)
  }
];