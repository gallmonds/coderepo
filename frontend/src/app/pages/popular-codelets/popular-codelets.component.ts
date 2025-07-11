import { Component, OnInit } from '@angular/core';
import { AlgorithmService } from '../../services/algorithm.service';
import { AlgorithmSummaryDto } from '../../models/algorithm-summary.dto';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-popular-codelets', 
  templateUrl: './popular-codelets.component.html',
  imports: [NgFor, NgIf, RouterModule, CommonModule]
})
export class PopularCodeletsComponent implements OnInit {
  popular: AlgorithmSummaryDto[] = [];
  page = 1;
  pageSize = 10;
  loading = true;

  constructor(private algorithmService: AlgorithmService) {}

  ngOnInit(): void {
    this.fetchPopular();
  }

  fetchPopular(): void {
    this.loading = true;
    this.algorithmService.getAlgorithms('most_popular', this.page, this.pageSize).subscribe({
      next: data => {
        this.popular = data;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  nextPage(): void {
    this.page++;
    this.fetchPopular();
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.fetchPopular();
    }
  }

  getProfilePic(path?: string): string {
    return path
      ? `http://localhost:8080/static/${path}`
      : 'http://localhost:8080/static/media/pfp/nopfp.png';
  }
}
