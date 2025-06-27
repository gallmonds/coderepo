import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private auth = inject(AuthService);
  private userService = inject(UserService);

  ngOnInit(): void {
    const userId = this.auth.getUserIdFromToken();
    if (userId) {
      this.userService.fetchAndSetUser(userId);
    }
  }
}
