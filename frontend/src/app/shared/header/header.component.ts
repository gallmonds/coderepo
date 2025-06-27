import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { UserProfileDto } from '../../models/user-profile.dto';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent {
  user: UserProfileDto | null = null;
  showNotifications = false;
  searchText = '';

  constructor(
    private auth: AuthService,
    private userService: UserService
  ) { }

  ngOnInit(): void {
    const id = this.auth.getUserIdFromToken();
    if (id) {
      this.userService.getUserProfile(id).subscribe({
        next: (data) => {
          this.user = data;
          this.userService.setCurrentUser(data);
        },
        error: () => {
          this.user = null;
          this.userService.clearCurrentUser();
        }
      });
    }
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
  }

  logout() {
    this.auth.logout();
  }

  get avatarUrl(): string {
    return this.user?.profilePic
      ? `http://localhost:8080/static/${this.user.profilePic}`
      : 'http://localhost:8080/static/media/pfp/nopfp.png';
  }

  onSearchKey(e: KeyboardEvent) {
    if (e.key === 'Enter' && this.searchText.trim()) {
      window.location.href = `/search-results?query=${encodeURIComponent(this.searchText.trim())}`;
    }
  }

}

