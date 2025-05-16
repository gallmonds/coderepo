import { Component, AfterViewInit, ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-shared',
  templateUrl: './shared.component.html',
  styleUrls: ['./shared.component.css']
})
export class SharedComponent implements AfterViewInit {

  @ViewChild('menuBtn') menuBtn!: ElementRef;
  @ViewChild('sidebar') sidebar!: ElementRef;
  @ViewChild('overlay') overlay!: ElementRef;

  ngAfterViewInit(): void {
    this.menuBtn.nativeElement.addEventListener('click', () => {
      this.openSidebar();
    });

    this.overlay.nativeElement.addEventListener('click', () => {
      this.closeSidebar();
    });

    document.addEventListener('keydown', (event) => {
      if (event.key === 'Escape') {
        this.closeSidebar();
      }
    });
  }

  openSidebar(): void {
    this.sidebar.nativeElement.classList.remove('-translate-x-full');
    this.sidebar.nativeElement.classList.add('translate-x-0');
    this.overlay.nativeElement.classList.remove('hidden');
  }

  closeSidebar(): void {
    this.sidebar.nativeElement.classList.add('-translate-x-full');
    this.sidebar.nativeElement.classList.remove('translate-x-0');
    this.overlay.nativeElement.classList.add('hidden');
  }
}
