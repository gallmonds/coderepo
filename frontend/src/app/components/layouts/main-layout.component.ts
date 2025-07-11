import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../shared/header/header.component';
import { FooterComponent } from '../../shared/footer/footer.component';

@Component({
    selector: 'app-main-layout',
    standalone: true,
    imports: [RouterOutlet, HeaderComponent, FooterComponent],
    template: `
    <div class="flex flex-col min-h-screen bg-gray-900 text-white">
        <app-header></app-header>
        <main class="flex-grow p-6 pt-24">
            <router-outlet></router-outlet>
        </main>
        <app-footer></app-footer>
    </div>
    `
})
export class MainLayoutComponent { }