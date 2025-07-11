import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { UserProfileDto } from '../models/user-profile.dto';

@Injectable({ providedIn: 'root' })
export class UserService {
  private api = 'http://localhost:5131/api/user';
  private currentUserSubject = new BehaviorSubject<UserProfileDto | null>(null);
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) { }

  getUserProfile(id: number): Observable<UserProfileDto> {
    return this.http.get<UserProfileDto>(`${this.api}/users/${id}`);
  }

  setCurrentUser(user: UserProfileDto) {
    this.currentUserSubject.next(user);
  }

  getCurrentUser(): UserProfileDto | null {
    return this.currentUserSubject.value;
  }

  clearCurrentUser() {
    this.currentUserSubject.next(null);
  }

  fetchAndSetUser(id: number): void {
    this.getUserProfile(id).subscribe({
      next: user => this.setCurrentUser(user),
      error: err => {
        console.error('Failed to fetch user profile:', err);
        this.clearCurrentUser();
      }
    });
  }
  isAuthenticated(): boolean {
    return localStorage.getItem('jwt_token') !== null;
  }
  getUserIdFromToken(): number | null {
    const token = localStorage.getItem('jwt_token');
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.sub ? parseInt(payload.sub) : null;
  }
  updateProfile(dto: { username: string; biography: string }): Observable<{ message: string; token: string }> {
  return this.http.put<{ message: string; token: string }>(`${this.api}/users/profile`, dto);
  }
  uploadProfilePicture(formData: FormData): Observable<{ profilePic: string }> {
    return this.http.post<{ profilePic: string }>(`${this.api}/profile/picture`, formData);
  }
  getUsernameFromToken(): string | null {
  const token = localStorage.getItem('jwt_token');
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload.username
      || payload.unique_name
      || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      || null;
  } catch {
    return null;
  }
}


}
