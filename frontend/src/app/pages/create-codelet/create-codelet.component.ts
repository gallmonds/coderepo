import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AlgorithmService } from '../../services/algorithm.service';

@Component({
  selector: 'app-create-codelet',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './create-codelet.component.html'
})
export class CreateCodeletComponent {
  title = '';
  summary = '';
  isPrivate = false;
  tagInput = '';
  tags: { id: number, name: string }[] = [];
  suggestions: { id: number, name: string }[] = [];
  loading = false;
  selectedSuggestionIndex = -1;
  pendingNewTags: string[] = [];

  constructor(
    private algorithmService: AlgorithmService,
    private router: Router
  ) { }

  async onTagInputChange(): Promise<void> {
    const query = this.tagInput.trim();
    if (query.length > 0) {
      try {
        this.suggestions = await this.algorithmService.searchTags(query).toPromise() || [];
      } catch {
        this.suggestions = [];
      }
    } else {
      this.suggestions = [];
    }
    this.selectedSuggestionIndex = -1;
  }

  async selectSuggestion(tag: { id: number, name: string }): Promise<void> {
    if (!this.tags.some(t => t.id === tag.id)) {
      this.tags.push(tag);
    }
    this.resetTagInput();
  }

  async handleKeyDown(event: KeyboardEvent): Promise<void> {
    const key = event.key;
    const max = this.suggestions.length;

    if (key === 'ArrowDown') {
      event.preventDefault();
      this.selectedSuggestionIndex = (this.selectedSuggestionIndex + 1) % (max + 1);
    } else if (key === 'ArrowUp') {
      event.preventDefault();
      this.selectedSuggestionIndex =
        (this.selectedSuggestionIndex - 1 + (max + 1)) % (max + 1);
    } else if (key === 'Enter' || key === 'Tab') {
      event.preventDefault();
      const query = this.tagInput.trim();

      if (!query) return;

      if (this.selectedSuggestionIndex >= 0 && this.selectedSuggestionIndex < this.suggestions.length) {
        const selected = this.suggestions[this.selectedSuggestionIndex];
        await this.selectSuggestion(selected);
      } else {
        this.addNewTagFromInput();
      }
    }
  }

  addNewTagFromInput(): void {
    const name = this.tagInput.trim();
    if (!name) return;

    const lowerName = name.toLowerCase();

    if (
      !this.tags.some(t => t.name.toLowerCase() === lowerName) &&
      !this.pendingNewTags.includes(lowerName)
    ) {
      this.tags.push({ id: 0, name });
      this.pendingNewTags.push(lowerName);
    }

    this.resetTagInput();
  }

  resetTagInput(): void {
    this.tagInput = '';
    this.suggestions = [];
    this.selectedSuggestionIndex = -1;
  }

  removeTag(index: number): void {
    this.tags.splice(index, 1);
  }

  async onSubmit(): Promise<void> {
    this.loading = true;
    try {
      const finalTags: { id: number; name: string }[] = [...this.tags];

      for (const name of this.pendingNewTags) {
        await this.algorithmService.createTag(name).toPromise();
        const found = await this.algorithmService.searchTags(name).toPromise();
        const created = found?.find(t => t.name.toLowerCase() === name.toLowerCase());
        if (!created) throw new Error(`Failed to retrieve tag: ${name}`);
        const dummyIndex = finalTags.findIndex(t => t.name.toLowerCase() === name.toLowerCase() && t.id === 0);
        if (dummyIndex !== -1) finalTags.splice(dummyIndex, 1);

        finalTags.push(created);
      }

      const result = await this.algorithmService.createAlgorithm({
        title: this.title,
        description: this.summary,
        isPrivate: this.isPrivate
      }).toPromise();

      if (!result) throw new Error('Algorithm creation returned empty response');

      const newAlgorithmId = parseInt(result.split('/').pop()!);
      if (isNaN(newAlgorithmId)) throw new Error('Invalid ID extracted from creation response');

      const tagIds = finalTags.map(t => t.id);
      if (tagIds.length > 0) {
        await this.algorithmService.assignTags(newAlgorithmId, tagIds).toPromise();
      }

      this.router.navigate(['/codelet', newAlgorithmId]);
    } catch (err) {
      console.error('Error creating codelet:', err);
      alert('Failed to publish Codelet. Please check your tags and try again.');
    } finally {
      this.loading = false;
    }
  }

  get shouldShowCreateOption(): boolean {
    const input = this.tagInput.trim().toLowerCase();
    return !!input && !this.suggestions.some(s => s.name.toLowerCase() === input);
  }
}
