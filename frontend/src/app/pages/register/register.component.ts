import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-register',
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  form: FormGroup;
  isLoading = false;
  errorMsg: string | null = null;
  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required]
    });
  }

  onSubmit() {
  if (this.form.invalid) {
    this.form.markAllAsTouched();
    return;
  }

  const { email, username, password, confirmPassword } = this.form.value;

  if (password !== confirmPassword) {
    this.errorMsg = 'Passwords do not match.';
    return;
  }

  this.isLoading = true;
  this.errorMsg = null;

  this.auth.register({ email, username, password }).subscribe({
    next: () => this.router.navigate(['/']),
    error: err => {
      this.isLoading = false;
      this.errorMsg = err.error || 'Registration failed. Please try again.';
    },
    complete: () => this.isLoading = false
  });
}
}