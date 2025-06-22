import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { SafeHtmlPipe } from './safe-html.pipe';
import { marked } from 'marked'; 

@Component({
  selector: 'app-create-codelet',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,     
    SafeHtmlPipe
  ],
  templateUrl: './create-codelet.component.html'
})
export class CreateCodeletComponent {
  title = '';
  summary = '';
  documentation = '';
  markdownPreview = '';
  tags: string[] = [];
  collaborators = '';
  selectedFile: File | null = null;

  insertMarkdown(snippet: string): void {
    const textarea = document.getElementById('docs') as HTMLTextAreaElement;
    const start = textarea.selectionStart;
    const end = textarea.selectionEnd;
    const before = this.documentation.slice(0, start);
    const after = this.documentation.slice(end);
    this.documentation = before + snippet + after;

    setTimeout(() => {
      textarea.focus();
      textarea.setSelectionRange(start + snippet.length, start + snippet.length);
      this.updatePreview();
    });
  }

  async updatePreview(): Promise<void> {
    this.markdownPreview = await marked.parse(this.documentation);
  }

  handleTagInput(event: KeyboardEvent): void {
    if (event.key === ' ') {
      event.preventDefault();
      const input = event.target as HTMLInputElement;
      const value = input.value.trim();
      if (value && !this.tags.includes(value)) {
        if (this.tags.length < 10) { 
          this.tags.push(value);
        }
        input.value = '';
      }
    }
  }

  removeTag(index: number): void {
    this.tags.splice(index, 1);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] || null;
  }

  onSubmit(): void {
    console.log({
      title: this.title,
      summary: this.summary,
      documentation: this.documentation,
      tags: this.tags,
      collaborators: this.collaborators,
      file: this.selectedFile
    });
  }
}
