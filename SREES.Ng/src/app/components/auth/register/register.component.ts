import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Role } from '../../../models/role.enum';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  userData = {
    firstName: '',
    lastName: '',
    email: '',
    password: '',
    role: Role.User
  };
  error = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (this.userData.firstName && this.userData.lastName && this.userData.email && this.userData.password) {
      this.loading = true;
      this.error = '';
      
      this.authService.register(this.userData).subscribe({
        next: (response) => {
          this.loading = false;
          if (response.data && response.data.token) {
            this.router.navigate(['/dashboard']);
          } else {
              this.router.navigate(['/login']);
          }
        },
        error: (err) => {
          this.loading = false;
          if (err.error && err.error.message) {
             this.error = err.error.message;
          } else {
             this.error = 'Registration failed. Please try again.';
          }
        }
      });
    }
  }
}
//
//     if (this.userData.username && this.userData.email && this.userData.password) {
//       this.loading = true;
      
//       this.authService.register(this.userData)
//         .subscribe({
//           next: (success) => {
//             this.loading = false;
//             if (success) {
//               this.router.navigate(['/dashboard']);
//             } else {
//               alert('Registration failed');
//             }
//           },
//           error: () => {
//             this.loading = false;
//             alert('Registration failed');
//           }
//         });
//     }
//   }
// }