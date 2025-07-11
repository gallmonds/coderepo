import { Component } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { AlgorithmService } from '../../services/algorithm.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserProfileDto } from '../../models/user-profile.dto';
import { AlgorithmSummaryDto } from '../../models/algorithm-summary.dto';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-public-profile',
  standalone: true,
  imports: [NgFor, NgIf, FormsModule, RouterModule, CommonModule],
  templateUrl: './public-profile.component.html'
})
export class PublicProfileComponent {
  user: UserProfileDto | null = null;
  codelets: AlgorithmSummaryDto[] = [];
  selectedFilter: string = 'most_recent';
  private userId: number = 0;
  editing = false;
  editUsername = '';
  editBiography = '';
  isOwnProfile = false;
  isUploading = false;
  uploadSuccess = false;
  uploadError = false;

  constructor(
    private auth: AuthService,
    private userService: UserService,
    private algorithmService: AlgorithmService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.userId = +id;
        const loggedInUserId = this.userService.getUserIdFromToken();
        this.isOwnProfile = loggedInUserId === this.userId;

        this.userService.getUserProfile(this.userId).subscribe({
          next: (data) => {
            this.user = data;
            this.userService.setCurrentUser(data);
            this.editUsername = data.username;
            this.editBiography = data.biography || '';
            this.loadCodelets();
          },
          error: (err) => {
            this.user = null;
            this.userService.clearCurrentUser();
            if (err.status === 404 || err.status == 400) {
              this.router.navigate(['/404']);
            }
          }
        });
      }
    });
  }

  loadCodelets(): void {
    this.algorithmService.getAlgorithms(this.selectedFilter, 1, 5, true, this.userId).subscribe({
      next: (data) => this.codelets = data,
      error: () => this.codelets = []
    });
  }

  get avatarUrl(): string {
    return this.user?.profilePic
      ? `http://localhost:8080/static/${this.user.profilePic}`
      : 'http://localhost:8080/static/media/pfp/nopfp.png';
  }

  get formattedJoinDate(): string {
    return new Date(this.user?.createdAt ?? '').toISOString().split('T')[0];
  }

  confirmEdit(): void {
    if (!this.editUsername.trim()) {
      alert('Username cannot be empty.');
      return;
    }

    const dto = {
      username: this.editUsername.trim(),
      biography: this.editBiography.trim()
    };

    this.userService.updateProfile(dto).subscribe({
      next: (res: any) => {
        if (res.token) {
          this.auth.saveToken(res.token);
        }

        if (this.user) {
          this.user.username = dto.username;
          this.user.biography = dto.biography;
        }

        this.editing = false;
      },
      error: (err) => {
        console.error('Failed to update profile:', err);
        alert('Failed to update profile.');
      }
    });
  }

  cancelEdit(): void {
    this.editing = false;
    if (this.user) {
      this.editUsername = this.user.username;
      this.editBiography = this.user.biography || '';
    }
  }

  triggerFileInput(): void {
    const input = document.querySelector<HTMLInputElement>('input[type="file"]');
    input?.click();
  }

  uploadProfilePicture(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) return;

    const file = input.files[0];
    const formData = new FormData();
    formData.append('file', file);

    this.isUploading = true;
    this.uploadSuccess = false;
    this.uploadError = false;

    this.userService.uploadProfilePicture(formData).subscribe({
      next: (res) => {
        if (this.user) {
          this.user.profilePic = res.profilePic;
        }
        this.uploadSuccess = true;
        setTimeout(() => this.uploadSuccess = false, 3000);
      },
      error: (err) => {
        console.error('Failed to upload profile picture:', err);
        this.uploadError = true;
        setTimeout(() => this.uploadError = false, 3000);
      },
      complete: () => {
        this.isUploading = false;
      }
    });

    input.value = '';
  }
}
