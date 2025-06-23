import { Component, ElementRef, ViewChild, OnInit } from '@angular/core';
import { NgIf, NgFor, NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../shared/header/header.component';
import { FooterComponent } from '../../shared/footer/footer.component';

@Component({
  selector: 'app-codelet',
  standalone: true,
  imports: [NgIf, NgFor, NgClass, FormsModule, HeaderComponent, FooterComponent],
  templateUrl: './codelet.component.html',
  styleUrl: './codelet.component.css'
})
export class CodeletComponent implements OnInit {
  codeletId = 1;
  copyButtonText = '📋 Copy';
  rating = 0;
  isDocsModalOpen = false;
  loggedIn = true;

  availableLanguages: string[] = [];
  selectedLanguage: string = '';
  codeExamples: { [key: string]: string } = {};

  @ViewChild('codeBlock') codeBlock!: ElementRef<HTMLElement>;
  @ViewChild('docsModal') docsModal!: ElementRef<HTMLElement>;

  ngOnInit(): void {
    this.loadLanguagesAndCode();
  }

  loadLanguagesAndCode(): void {
    this.availableLanguages = ['Python', 'JavaScript', 'Lua'];
    this.codeExamples = {
      Python: `
number = coderepo.web.numvar
if number.isdigit():
    number = int(number)
    if number % 2 == 0:
        coderepo.web.res("Number is even")
    else:
        coderepo.web.res("Number is odd")
else:
    coderepo.web.res("Not a valid number")
      `,
      JavaScript: `
const number = parseInt(coderepo.web.numvar);
if (!isNaN(number)) {
  if (number % 2 === 0) {
    coderepo.web.res("Number is even");
  } else {
    coderepo.web.res("Number is odd");
  }
} else {
  coderepo.web.res("Not a valid number");
}
      `,
      Lua: `
number = tonumber(coderepo.web.numvar)
if number then
  if number % 2 == 0 then
    coderepo.web.res("Number is even")
  else
    coderepo.web.res("Number is odd")
  end
else
  coderepo.web.res("Not a valid number")
end
      `
    };

    this.selectedLanguage = this.availableLanguages[0];
  }

  get currentCode(): string {
    return this.codeExamples[this.selectedLanguage] || '';
  }

  copyCode(): void {
    const code = this.codeBlock.nativeElement.innerText.trim();
    navigator.clipboard.writeText(code).then(() => {
      this.copyButtonText = '✅ Copied';
      setTimeout(() => {
        this.copyButtonText = '📋 Copy';
      }, 2000);
    });
  }

  setRating(value: number): void {
    this.rating = value;
  }

  toggleDocsModal(): void {
    this.isDocsModalOpen = !this.isDocsModalOpen;
    const modal = this.docsModal.nativeElement;
    modal.classList.toggle('hidden');
    modal.classList.toggle('flex');
  }

  uploadInOtherLanguage(): void {
    alert('Functionality coming soon: Upload in another language.');
  }
}
