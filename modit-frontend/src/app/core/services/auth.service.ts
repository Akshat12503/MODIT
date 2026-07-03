import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse, LoginRequest, RegisterRequest } from '../models/models';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = `${environment.apiUrl}/Auth`;

  // Signal-based reactive current user state (Angular 21 idiomatic approach)
  currentUser = signal<AuthResponse | null>(this.loadUserFromStorage());

  constructor(private http: HttpClient, private router: Router) {}

  private loadUserFromStorage(): AuthResponse | null {
    // SSR Check: Only access localStorage if running in the browser
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      const raw = localStorage.getItem('modit_user');
      return raw ? JSON.parse(raw) : null;
    }
    return null;
  }

  register(data: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data).pipe(
      tap(res => this.setSession(res))
    );
  }

  login(data: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, data).pipe(
      tap(res => this.setSession(res))
    );
  }

  private setSession(res: AuthResponse) {
    // SSR Check
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      localStorage.setItem('modit_token', res.token);
      localStorage.setItem('modit_user', JSON.stringify(res));
    }
    this.currentUser.set(res);
  }

  logout() {
    // SSR Check
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      localStorage.removeItem('modit_token');
      localStorage.removeItem('modit_user');
    }
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    // SSR Check
    if (typeof window !== 'undefined' && typeof window.localStorage !== 'undefined') {
      return localStorage.getItem('modit_token');
    }
    return null;
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}