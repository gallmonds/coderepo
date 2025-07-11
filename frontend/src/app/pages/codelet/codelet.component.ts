import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { AlgorithmService } from '../../services/algorithm.service';
import { UserService } from '../../services/user.service';

import { AlgorithmDetailDto } from '../../models/algorithm-detail.dto';
import { CommentAlgorithmDto } from '../../models/comment-algorithm.dto';

import 'prismjs';
import 'prismjs/components/prism-python';
import 'prismjs/components/prism-c';
import 'prismjs/components/prism-cpp';
import 'prismjs/components/prism-java';
import 'prismjs/components/prism-javascript';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-go';
import 'prismjs/components/prism-lua';
import 'prismjs/components/prism-perl';
import 'prismjs/components/prism-ruby';
import 'prismjs/components/prism-rust';
import 'prismjs/components/prism-swift';

import * as Prism from 'prismjs';

@Component({
  selector: 'app-codelet',
  standalone: true,
  templateUrl: './codelet.component.html',
  styleUrls: ['./codelet.component.css'],
  imports: [CommonModule, FormsModule, RouterModule]
})
export class CodeletComponent implements OnInit {
  algorithm: AlgorithmDetailDto | null = null;
  selectedLanguage: any = null;
  currentCode: string = '';
  highlightedCode: string = '';
  copyButtonText: string = 'Copy';
  loggedIn: boolean = false;
  rating: number = 0;
  availableLanguages: any[] = [];

  newComment: string = '';
  userId: number | null = null;

  likeCount: number = 0;
  liked: boolean = false;

  isOwner: boolean = false;

  newCodeFile: File | null = null;
  allowedExtensions = ['.c', '.cpp', '.cs', '.go', '.java', '.js', '.lua', '.pl', '.py', '.rb', '.rs', '.swift'];

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

        this.likeCount = data.ratingCount;
        this.liked = data.userHasLiked ?? false;

        this.isOwner = this.userService.getUsernameFromToken() === data.owner.username;

        if (this.availableLanguages.length > 0) {
          this.selectedLanguage = this.availableLanguages[0];
          this.ensurePrismLang();
          this.loadCode(id, this.selectedLanguage.codeletPath);
        } else {
          this.currentCode = '// No language files available.';
          this.highlightedCode = this.currentCode;
        }
      },
      error: () => { }
    });

    this.loggedIn = this.userService.isAuthenticated();
    if (this.loggedIn) {
      this.userId = this.userService.getUserIdFromToken();
    }
  }

  ensurePrismLang(): void {
    if (!this.selectedLanguage) return;

    const ext = this.selectedLanguage.langName.toLowerCase();
    const prismLangMap: { [key: string]: string } = {
      c: 'c',
      cpp: 'cpp',
      cs: 'csharp',
      go: 'go',
      java: 'java',
      js: 'javascript',
      lua: 'lua',
      pl: 'perl',
      py: 'python',
      rb: 'ruby',
      rs: 'rust',
      swift: 'swift'
    };

    this.selectedLanguage.prismLang = prismLangMap[ext] || 'clike';
  }

  onLanguageChange(): void {
    if (!this.algorithm || !this.selectedLanguage) return;
    this.ensurePrismLang();
    this.loadCode(this.algorithm.algorithmId, this.selectedLanguage.codeletPath);
  }

  loadCode(algorithmId: number, codeletPath: string): void {
    const lang = codeletPath.split('/').pop() || '';
    this.algorithmService.getLatestCodeFilePath(algorithmId, lang).subscribe({
      next: (res) => {
        const fileUrl = `http://localhost:8080/${res.path}`;
        this.http.get(fileUrl, { responseType: 'text' }).subscribe({
          next: (code) => {
            this.currentCode = code;
            const grammar = Prism.languages[this.selectedLanguage.prismLang as keyof typeof Prism.languages] || Prism.languages['clike'];
            this.highlightedCode = Prism.highlight(code, grammar, this.selectedLanguage.prismLang);
          },
          error: () => {
            this.currentCode = '// Failed to load code from resolved path.';
            this.highlightedCode = this.currentCode;
          }
        });
      },
      error: () => {
        this.currentCode = '// Could not resolve latest file.';
        this.highlightedCode = this.currentCode;
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

  private getLangIdFromExtension(ext: string): number | null {
    const map: { [key: string]: number } = {
      '.c': 1,
      '.cpp': 2,
      '.cs': 3,
      '.go': 4,
      '.java': 5,
      '.js': 6,
      '.lua': 7,
      '.pl': 8,
      '.py': 9,
      '.rb': 10,
      '.rs': 11,
      '.swift': 12
    };
    return map[ext] ?? null;
  }

  uploadInOtherLanguage(): void {
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = this.allowedExtensions.join(',');

    input.onchange = (event: Event) => {
      const file = (event.target as HTMLInputElement).files?.[0];
      if (!file || !this.algorithm) return;

      const ext = '.' + file.name.split('.').pop()?.toLowerCase();
      if (!this.allowedExtensions.includes(ext)) {
        alert(`Invalid file format. Allowed: ${this.allowedExtensions.join(', ')}`);
        return;
      }

      const langId = this.getLangIdFromExtension(ext);
      if (!langId) {
        alert('Unsupported file extension.');
        return;
      }

      const formData = new FormData();
      formData.append('AlgorithmId', this.algorithm.algorithmId.toString());
      formData.append('SupportedLangId', langId.toString());
      formData.append('File', file);

      this.algorithmService.addLanguage(formData).subscribe({
        next: () => {
          alert('Language added successfully.');
          this.ngOnInit();
        },
        error: (err) => {
          console.error('❌ Failed to upload:', err);
          alert('Failed to upload language.');
        }
      });
    };

    input.click();
  }

  submitComment(): void {
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

  toggleLike(): void {
    if (!this.loggedIn || !this.algorithm) {
      window.location.href = '/login';
      return;
    }

    const body = {
      contentId: this.algorithm.algorithmId,
      typeId: 2
    };

    this.http.post('http://localhost:5131/api/algorithms/rate', body).subscribe({
      next: () => {
        this.liked = !this.liked;
        this.likeCount += this.liked ? 1 : -1;
      },
      error: (err) => {
        console.error('Error toggling like:', err);
      }
    });
  }
}
