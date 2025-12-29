// import { Component } from '@angular/core';
// import { Router } from '@angular/router';
// import { AuthService } from '../../../services/auth.service';
// import { FormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';
// import { RouterModule } from '@angular/router';

// @Component({
//   selector: 'app-login',
//   imports: [FormsModule, CommonModule, RouterModule],
//   templateUrl: './login.component.html',
//   styleUrl: './login.component.scss'
// })
// export class LoginComponent {
//   credentials = {
//     username: '',
//     password: ''
//   };
//   loading = false;

//   constructor(
//     private authService: AuthService,
//     private router: Router
//   ) {}

//   onSubmit(): void {
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