import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AlgorithmService } from '../../services/algorithm.service';
import { AlgorithmSummaryDto } from '../../models/algorithm-summary.dto';

@Component({
  selector: 'app-search-results',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {
  results: AlgorithmSummaryDto[] = [];
  query = '';
  page = 1;
  pageSize = 5;
  loading = false;
  totalPages = 1;

  constructor(private route: ActivatedRoute, private algoService: AlgorithmService) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.query = params['query'] || '';
      this.page = 1;
      this.fetch();
    });
  }

  fetch(): void {
    if (!this.query) return;

    this.loading = true;
    this.algoService.searchAlgorithms(this.query, this.page, this.pageSize).subscribe({
      next: data => {
        this.results = data;
        this.totalPages = data.length < this.pageSize ? this.page : this.page + 1; // estimativo
        this.loading = false;
      },
      error: () => {
        this.results = [];
        this.loading = false;
      }
    });
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.fetch();
    }
  }

  prevPage(): void {
    if (this.page > 1) {
      this.page--;
      this.fetch();
    }
  }

  getProfilePic(path?: string): string {
    return path
      ? `http://localhost:8080/static/${path}`
      : 'http://localhost:8080/static/media/pfp/nopfp.png';
  }
}
