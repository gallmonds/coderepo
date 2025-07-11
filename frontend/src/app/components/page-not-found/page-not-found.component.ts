// src/app/components/page-not-found/page-not-found.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-page-not-found',
  standalone: true,
  template: `
    <div class="min-h-screen flex items-center justify-center bg-gray-900 text-white">
      <div class="text-center">
        <h1 class="text-5xl font-bold mb-4">404</h1>
        <p class="text-xl mb-6">Uh oh! We searched everywhere, but there wasn't any page. :c</p>
        <a href="/" class="text-blue-400">Return to homepage</a>
      </div>
    </div>
  `
})
export class PageNotFoundComponent {}
