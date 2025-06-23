import { Component, Input } from '@angular/core';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-public-profile',
  standalone: true,
  imports: [NgFor],
  templateUrl: './public-profile.component.html'
})
export class PublicProfileComponent {
  // Estos datos se deben inyectar desde una API en el futuro
  user = {
    username: 'sofia_dev',
    avatar: 'https://i.pravatar.cc/150?img=32',
    bio: 'Frontend Developer | Codelet Enthusiast',
    joined: '2024-12-01'
  };

  userCodelets = [
    { title: 'Regex Validator', createdAt: '2025-01-15' },
    { title: 'QuickSort Example', createdAt: '2025-03-12' },
    { title: 'Graph Traversal (DFS)', createdAt: '2025-04-22' }
  ];
}
