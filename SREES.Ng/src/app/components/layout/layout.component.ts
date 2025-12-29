import { Component } from '@angular/core';
import { Router } from '@angular/router';
// import { AuthService } from './../../services/auth.service.ts';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-layout',
  imports: [CommonModule, RouterModule],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  isSidebarOpen = false;
  isCrudOpen = false;

  constructor(
    // private authService: AuthService,
    private router: Router
  ) {}

  get username(): string {
    return '';
    // return this.authService.getCurrentUser()?.username || '';
  }

  get userRole(): string {
    return '';
    // return this.authService.getCurrentUser()?.role || 'guest';
  }

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  toggleSubmenu(event: Event): void {
    event.preventDefault();
    this.isCrudOpen = !this.isCrudOpen;
  }

  logout(): void {
    // this.authService.logout();
    this.router.navigate(['/login']);
  }
}