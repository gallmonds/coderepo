import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { UserService } from '../../services/user.service';
import { take } from 'rxjs';

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
  private router = inject(Router);

  ngOnInit(): void {
    this.auth.isLoggedIn().pipe(take(1)).subscribe(isLogged => {
      if (!isLogged) return;

      const userId = this.auth.getUserIdFromToken();
      if (!userId) {
        this.auth.logout();
        this.router.navigate(['/login']);
      } else {
        this.userService.fetchAndSetUser(userId);
      }
    });
  }
}