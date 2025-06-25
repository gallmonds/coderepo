import { Component } from '@angular/core';
import { NgFor } from '@angular/common';
import { FormsModule } from '@angular/forms'; 

@Component({
  selector: 'app-my-profile',
  standalone: true,
  imports: [ NgFor, FormsModule],
  templateUrl: './my-profile.component.html'
})
export class MyProfileComponent {
  user = {
    username: 'name_dev',
    avatar: 'https://i.pravatar.cc/150',
    email: 'gmail@example.com'
  };

  password = '';
  confirmPassword = '';
  myCodelets = [
    { title: 'Palindrome Checker', createdAt: '2025-06-10' },
    { title: 'FizzBuzz Algorithm', createdAt: '2025-06-15' },
    { title: 'Binary Search', createdAt: '2025-06-17' }
  ];

  updateProfile() {
    if (this.password && this.password !== this.confirmPassword) {
      alert('Passwords do not match!');
      return;
    }
    alert('Profile updated!');
  }
}
