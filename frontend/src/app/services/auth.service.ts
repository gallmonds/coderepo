import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})


export class AuthService {
  private apiUrl = 'http://localhost:5131/api/auth';
  private tokenKey = 'jwt_token';
  private loggedIn = new BehaviorSubject<boolean>(this.hasValidToken());

  constructor(private http: HttpClient) { }

  login(credentials: { username: string, password: string }): Observable<any> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/login`, credentials)
      .pipe(tap(response => this.setToken(response.token)));
  }

  register(data: { username: string, email: string, password: string }): Observable<any> {
    return this.http.post<{ token: string }>(`${this.apiUrl}/register`, data)
      .pipe(tap(response => this.setToken(response.token)));
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.loggedIn.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): Observable<boolean> {
    return this.loggedIn.asObservable();
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
    this.loggedIn.next(true);
  }

  private hasValidToken(): boolean {
    const token = this.getToken();
    if (!token) return false;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      const exp = payload.exp;
      return Date.now() / 1000 < exp;
    } catch (e) {
      return false;
    }
  }

  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('jwt_token');
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.sub ? parseInt(payload.sub, 10) : null;
    } catch {
      return null;
    }
  }
}
