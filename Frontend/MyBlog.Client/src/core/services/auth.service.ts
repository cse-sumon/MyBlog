import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { LoginDto, RegisterDto, AuthResponse, User } from '../models/auth-models';
import { environment } from '../../config/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly apiUrl = `${environment.apiUrl}/Auth`;
  private readonly tokenKey = 'auth_token';
  private readonly userKey = 'current_user';

  private userSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
  public user$ = this.userSubject.asObservable();

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasValidToken());
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private http: HttpClient) {
    this.initializeAuth();
  }

  /**
   * Initialize authentication state from stored data
   */
  private initializeAuth(): void {
    const token = this.getToken();
    const user = this.getUserFromStorage();

    if (token && user && !this.isTokenExpired(token)) {
      this.userSubject.next(user);
      this.isAuthenticatedSubject.next(true);
    } else {
      this.clearAuth();
    }
  }

  /**
   * Login user with username and password
   */
  login(credentials: LoginDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Login`, credentials).pipe(
      tap(response => this.handleAuthResponse(response))
    );
  }

  /**
   * Register new user
   */
  register(userData: RegisterDto): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/Register`, userData).pipe(
      tap(response => this.handleAuthResponse(response))
    );
  }

  /**
   * Handle authentication response
   */
  private handleAuthResponse(response: AuthResponse): void {
    this.saveToken(response.token);
    this.saveUser(response.user);
    this.userSubject.next(response.user);
    this.isAuthenticatedSubject.next(true);
  }

  /**
   * Logout user
   */
  logout(): void {
    this.clearAuth();
  }

  /**
   * Get stored JWT token
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Save JWT token to localStorage
   */
  private saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  /**
   * Get current user
   */
  getCurrentUser(): User | null {
    return this.userSubject.value;
  }

  /**
   * Get user from storage
   */
  private getUserFromStorage(): User | null {
    const raw = localStorage.getItem(this.userKey);
    if (!raw) return null;
    // guard against literal strings like 'undefined' or 'null' which can appear
    if (raw === 'undefined' || raw === 'null') {
      localStorage.removeItem(this.userKey);
      return null;
    }

    try {
      return JSON.parse(raw) as User;
    } catch (err) {
      console.warn('AuthService: stored user value is invalid JSON, clearing it.', err);
      localStorage.removeItem(this.userKey);
      return null;
    }
  }

  /**
   * Save user to localStorage
   */
  private saveUser(user: User): void {
    localStorage.setItem(this.userKey, JSON.stringify(user));
  }

  /**
   * Check if user has a specific role
   */
  hasRole(role: string): boolean {
    const user = this.getCurrentUser();
    return user ? user.roles.includes(role) : false;
  }

  /**
   * Check if any of the provided roles match
   */
  hasAnyRole(roles: string[]): boolean {
    const user = this.getCurrentUser();
    return user ? roles.some(role => user.roles.includes(role)) : false;
  }

  /**
   * Check if user has valid token
   */
  hasValidToken(): boolean {
    const token = this.getToken();
    return token ? !this.isTokenExpired(token) : false;
  }

  /**
   * Check if token is expired
   */
  private isTokenExpired(token: string): boolean {
    try {
      const payload = this.decodeToken(token);
      const exp = payload.exp * 1000; // Convert to milliseconds
      return Date.now() >= exp;
    } catch {
      return true;
    }
  }

  /**
   * Decode JWT token
   */
  private decodeToken(token: string): any {
    const parts = token.split('.');
    if (parts.length !== 3) {
      throw new Error('Invalid token');
    }

    const decoded = JSON.parse(atob(parts[1]));
    return decoded;
  }

  /**
   * Clear all authentication data
   */
  private clearAuth(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.userKey);
    this.userSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }
}
