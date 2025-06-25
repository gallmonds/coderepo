import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from '../../shared/header/header.component';
import { FooterComponent } from '../../shared/footer/footer.component';

@Component({
  selector: 'app-codelet-settings',
  standalone: true,
  imports: [CommonModule, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './codelet-settings.component.html',
  styleUrls: ['./codelet-settings.component.css']
})
export class CodeletSettingsComponent implements OnInit {
  title: string = 'Lorem Ipsum';
  description: string = 'This codelet checks if a value is numeric and verifies its parity.';
  selectedLanguage: string = 'TypeScript';

  codeExamples: { [key: string]: string } = {
    'JavaScript': `function isEven(n) {\n  return n % 2 === 0;\n}`,
    'TypeScript': `function isEven(n: number): boolean {\n  return n % 2 === 0;\n}`,
    'Python': `def is_even(n):\n    return n % 2 == 0`,
    'Java': `public boolean isEven(int n) {\n    return n % 2 == 0;\n}`,
    'C#': `public bool IsEven(int n) {\n    return n % 2 == 0;\n}`
  };

  code: string = '';

  languages: string[] = Object.keys(this.codeExamples);

  ngOnInit(): void {
    this.code = this.codeExamples[this.selectedLanguage];
  }

  onLanguageChange(): void {
    this.code = this.codeExamples[this.selectedLanguage];
  }

  saveChanges() {
    console.log('Saved:', {
      title: this.title,
      description: this.description,
      language: this.selectedLanguage,
      code: this.code
    });
    alert('Changes saved successfully ✅');
  }
}
