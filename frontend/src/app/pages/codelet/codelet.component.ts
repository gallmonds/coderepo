import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AlgorithmService } from '../../services/algorithm.service';
import { UserService } from '../../services/user.service';

import { AlgorithmDetailDto } from '../../models/algorithm-detail.dto';
import { HeaderComponent } from '../../shared/header/header.component';
import { FooterComponent } from '../../shared/footer/footer.component';
import { CommentAlgorithmDto } from '../../models/comment-algorithm.dto';
@Component({
  selector: 'app-codelet',
  standalone: true,
  templateUrl: './codelet.component.html',
  styleUrls: ['./codelet.component.css'],
  imports: [CommonModule, FormsModule, RouterModule, HeaderComponent, FooterComponent]
})
export class CodeletComponent implements OnInit {
  algorithm: AlgorithmDetailDto | null = null;
  selectedLanguage: any = null;
  currentCode: string = '';
  copyButtonText: string = 'Copy';
  loggedIn: boolean = false;
  rating: number = 0;
  availableLanguages: any[] = [];

  newComment: string = '';
  userId: number | null = null;

  @ViewChild('docsModal') docsModal!: ElementRef;

  constructor(
    private route: ActivatedRoute,
    private algorithmService: AlgorithmService,
    private userService: UserService,
    private http: HttpClient
  ) { }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (isNaN(id)) return;

    this.algorithmService.getAlgorithmById(id).subscribe({
      next: (data) => {
        if (!data) return;

        this.algorithm = data;
        this.availableLanguages = data.languages;

        if (this.availableLanguages.length > 0) {
          this.selectedLanguage = this.availableLanguages[0];
          this.loadCode(id, this.selectedLanguage.codeletPath);
        } else {
          this.currentCode = '// No language files available.';
        }
      },
      error: () => { }
    });

    this.loggedIn = this.userService.isAuthenticated();
    if (this.loggedIn) {
      this.userId = this.userService.getUserIdFromToken();
    }
  }

  onLanguageChange(): void {
    if (!this.algorithm || !this.selectedLanguage) return;
    this.loadCode(this.algorithm.algorithmId, this.selectedLanguage.codeletPath);
  }

  loadCode(algorithmId: number, codeletPath: string): void {
    const lang = codeletPath.split('/').pop() || '';
    this.algorithmService.getLatestCodeFilePath(algorithmId, lang).subscribe({
      next: (res) => {
        const fileUrl = `http://localhost:8080/${res.path}`;
        this.http.get(fileUrl, { responseType: 'text' }).subscribe({
          next: (code) => (this.currentCode = code),
          error: () => (this.currentCode = '// Failed to load code from resolved path.')
        });
      },
      error: () => {
        this.currentCode = '// Could not resolve latest file.';
      }
    });
  }

  copyCode(): void {
    navigator.clipboard.writeText(this.currentCode).then(() => {
      this.copyButtonText = 'Copied!';
      setTimeout(() => (this.copyButtonText = 'Copy'), 2000);
    });
  }

  toggleDocsModal(): void {
    const modal = this.docsModal.nativeElement;
    modal.classList.toggle('hidden');
    modal.classList.toggle('flex');
  }

  setRating(rating: number): void {
    this.rating = rating;
  }

  uploadInOtherLanguage(): void { }

  submitComment(): void {
      console.log('Clicked Send');
      if (!this.newComment.trim()) {
    console.log('❌ No comment text');
    return;
  }
  if (!this.algorithm) {
    console.log('❌ Algorithm not loaded');
    return;
  }
  if (!this.loggedIn) {
    console.log('❌ Not logged in');
    return;
  }
  if (!this.userId) {
    console.log('❌ No userId');
    return;
  }
  if (!this.newComment.trim() || !this.algorithm || !this.loggedIn || !this.userId) return;

  const dto: CommentAlgorithmDto = {
    contentId: this.algorithm.algorithmId,
    body: this.newComment.trim(),
    replyToId: null 
  };

  this.algorithmService.commentAlgorithm(dto).subscribe({
    next: () => {
      this.newComment = '';
      this.refreshComments(); 
    },
    error: (err) => {
      console.error('Failed to post comment:', err);
    }
  });
}
refreshComments(): void {
  const id = this.algorithm?.algorithmId;
  if (!id) return;

  this.algorithmService.getAlgorithmById(id).subscribe({
    next: (data) => {
      if (!data) return;
      this.algorithm!.comments = data.comments;
    },
    error: (err) => console.error('Failed to refresh comments', err)
  });
}

}