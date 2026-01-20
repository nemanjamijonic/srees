import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  credentials = {
    email: '',
    password: ''
  };
  error = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (this.credentials.email && this.credentials.password) {
      this.loading = true;
      this.error = '';
      
      this.authService.login(this.credentials).subscribe({
        next: (response) => {
          this.loading = false;
          if (response.data && response.data.token) {
            this.router.navigate(['/dashboard']);
          } else {
             this.error = response.message || 'Login failed';
          }
        },
        error: (err) => {
          this.loading = false;
          if (err.error && err.error.message) {
            this.error = err.error.message; 
          } else {
            this.error = 'Login failed. Please check your credentials.';
          }
        }
      });
    }
  }
}
// Old commented code starts here
//     if (this.credentials.username && this.credentials.password) {
//       this.loading = true;
      
//       this.authService.login(this.credentials.username, this.credentials.password)
//         .subscribe({
//           next: (success) => {
//             this.loading = false;
//             if (success) {
//               this.router.navigate(['/dashboard']);
//             } else {
//               alert('Invalid credentials');
//             }
//           },
//           error: () => {
//             this.loading = false;
//             alert('Login failed');
//           }
//         });
//     }
//   }
// }