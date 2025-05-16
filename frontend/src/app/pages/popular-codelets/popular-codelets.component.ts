import { Component, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-popular-codelets',
  templateUrl: './popular-codelets.component.html',
})
export class PopularCodeletsComponent {
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
