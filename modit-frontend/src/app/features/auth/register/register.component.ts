
import { Component, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { UserRole } from '../../../core/models/models';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  phone = '';
  role: UserRole = UserRole.Customer;
  companyName = '';
  businessType = '';
  gstNumber = '';

  UserRole = UserRole; // expose enum to template

  errorMessage = signal<string | null>(null);
  loading = signal(false);

  constructor(private auth: AuthService, private router: Router) {}

  get isBusinessRole(): boolean {
    return this.role === UserRole.Contractor || this.role === UserRole.Architect || this.role === UserRole.Supplier;
  }

  onSubmit() {
    this.errorMessage.set(null);
    this.loading.set(true);

    this.auth.register({
      name: this.name,
      email: this.email,
      password: this.password,
      phone: this.phone,
      role: Number(this.role),
      companyName: this.isBusinessRole ? this.companyName : undefined,
      businessType: this.isBusinessRole ? this.businessType : undefined,
      gstNumber: this.isBusinessRole ? this.gstNumber : undefined
    }).subscribe({
      next: () => {
        this.loading.set(false);
        this.router.navigate(['/products']);
      },
      error: (err) => {
        this.loading.set(false);
        this.errorMessage.set(err.error?.message || 'Registration failed.');
      }
    });
  }
}