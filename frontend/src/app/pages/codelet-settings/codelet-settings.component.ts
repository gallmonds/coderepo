import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-codelet-settings',
  imports: [],
  templateUrl: './codelet-settings.component.html',
  styleUrl: './codelet-settings.component.css'
})
export class CodeletSettingsComponent {
  copyButtonText = '📋 Copiar';

  @ViewChild('codeBlock') codeBlock!: ElementRef<HTMLElement>;

  copyCode() {
    const code = this.codeBlock.nativeElement.innerText.trim();
    navigator.clipboard.writeText(code).then(() => {
      this.copyButtonText = '✅ Copiado';
      setTimeout(() => {
        this.copyButtonText = '📋 Copiar';
      }, 2000);
    });
  }
}
