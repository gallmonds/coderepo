import { Component, OnInit } from '@angular/core';
import { HeaderComponent } from '../../shared/header/header.component';
import { FooterComponent } from '../../shared/footer/footer.component';
import { UserProfileDto } from '../../models/user-profile.dto';
import { UserService } from '../../services/user.service';
import { AlgorithmService } from '../../services/algorithm.service';
import { AlgorithmSummaryDto } from '../../models/algorithm-summary.dto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [HeaderComponent, FooterComponent, CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  user: UserProfileDto | null = null;
  popular: AlgorithmSummaryDto[] = [];
  recent: AlgorithmSummaryDto[] = [];
  loadingPopular = true;
  loadingRecent = true;

  constructor(
    private userService: UserService,
    private algorithmService: AlgorithmService
  ) { }

  ngOnInit(): void {
    this.userService.currentUser$.subscribe(u => this.user = u);

    this.algorithmService.getAlgorithms('most_popular', 1, 3).subscribe({
      next: data => {
        this.popular = data;
        this.loadingPopular = false;
      },
      error: () => {
        this.loadingPopular = false;
      }
    });

    this.algorithmService.getAlgorithms('most_recent', 1, 3).subscribe({
      next: data => {
        this.recent = data;
        this.loadingRecent = false;
      },
      error: () => {
        this.loadingRecent = false;
      }
    });
  }

  get username(): string {
    return this.user?.username ?? 'Guest';
  }

  getProfilePic(path?: string): string {
    return path
      ? `http://localhost:8080/static/${path}`
      : 'http://localhost:8080/static/media/pfp/nopfp.png';
  }
  
}