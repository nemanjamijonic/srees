// import { Component } from '@angular/core';
// import { Router } from '@angular/router';
// import { AuthService } from '../../../services/auth.service';
// import { FormsModule } from '@angular/forms';
// import { CommonModule } from '@angular/common';
// import { RouterModule } from '@angular/router';

// @Component({
//   selector: 'app-register',
//   imports: [FormsModule, CommonModule, RouterModule],
//   templateUrl: './register.component.html',
//   styleUrl: './register.component.scss'
// })
// export class RegisterComponent {
//   userData = {
//     username: '',
//     email: '',
//     password: ''
//   };
//   loading = false;

//   constructor(
//     private authService: AuthService,
//     private router: Router
//   ) {}

//   onSubmit(): void {
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