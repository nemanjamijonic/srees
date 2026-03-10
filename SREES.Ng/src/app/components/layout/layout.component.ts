import { Component, OnInit, OnDestroy, HostListener, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { SignalRService } from '../../services/signalr.service';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Notification as AppNotification } from '../../models/notification.model';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import * as signalR from '@microsoft/signalr';

@Component({
  selector: 'app-layout',
  imports: [CommonModule, RouterModule],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit, OnDestroy {
  private destroy$ = new Subject<void>();
  private notificationSound!: HTMLAudioElement;
  
  isSidebarOpen = false;
  isCrudOpen = false;
  isNotificationOpen = false;
  
  // Notifications
  notifications: AppNotification[] = [];
  unreadCount = 0;
  isSignalRConnected = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private signalRService: SignalRService,
    private notificationService: NotificationService,
    private elementRef: ElementRef
  ) {}

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    const notificationWrapper = this.elementRef.nativeElement.querySelector('.notification-wrapper');
    if (notificationWrapper && !notificationWrapper.contains(target)) {
      this.isNotificationOpen = false;
    }
  }

  ngOnInit(): void {
    this.initNotificationSound();
    this.setupSignalR();
    this.loadNotifications();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private initNotificationSound(): void {
    this.notificationSound = new Audio('assets/sounds/notification.mp3');
    this.notificationSound.volume = 0.5;
  }

  private playNotificationSound(): void {
    try {
      // Prvo probaj MP3 fajl
      this.notificationSound.currentTime = 0;
      this.notificationSound.play().catch(() => {
        // Fallback: generiši beep zvuk pomoću Web Audio API
        this.playBeep();
      });
    } catch (e) {
      this.playBeep();
    }
  }

  private playBeep(): void {
    try {
      const audioContext = new (window.AudioContext || (window as any).webkitAudioContext)();
      const oscillator = audioContext.createOscillator();
      const gainNode = audioContext.createGain();
      
      oscillator.connect(gainNode);
      gainNode.connect(audioContext.destination);
      
      oscillator.frequency.value = 800; // Frekvencija u Hz
      oscillator.type = 'sine';
      
      gainNode.gain.setValueAtTime(0.3, audioContext.currentTime);
      gainNode.gain.exponentialRampToValueAtTime(0.01, audioContext.currentTime + 0.3);
      
      oscillator.start(audioContext.currentTime);
      oscillator.stop(audioContext.currentTime + 0.3);
    } catch (e) {
      console.log('Cannot play beep sound:', e);
    }
  }

  private setupSignalR(): void {
    // Listen for connection state changes
    this.signalRService.connectionState$
      .pipe(takeUntil(this.destroy$))
      .subscribe(state => {
        this.isSignalRConnected = state === signalR.HubConnectionState.Connected;
        console.log('SignalR connection state:', state, 'Connected:', this.isSignalRConnected);
      });

    // Listen for new notifications
    this.signalRService.notification$
      .pipe(takeUntil(this.destroy$))
      .subscribe(notification => {
        console.log('New notification received:', notification);
        this.notifications.unshift(notification);
        this.unreadCount++;
        // Play notification sound
        this.playNotificationSound();
        // Show browser notification if permitted
        this.showBrowserNotification(notification);
      });

    // Listen for unread count updates
    this.signalRService.unreadCount$
      .pipe(takeUntil(this.destroy$))
      .subscribe(count => {
        this.unreadCount = count;
      });
  }

  private loadNotifications(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      // Load unread notifications
      this.notificationService.getUnreadByUserId(user.userId)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (response) => {
            this.notifications = response.data || [];
            this.unreadCount = this.notifications.length;
          },
          error: (err) => console.error('Error loading notifications:', err)
        });
    }
  }

  private showBrowserNotification(notification: AppNotification): void {
    if ('Notification' in window && window.Notification.permission === 'granted') {
      new window.Notification(notification.title, {
        body: notification.message,
        icon: '/assets/icons/notification.png'
      });
    }
  }

  toggleNotifications(): void {
    this.isNotificationOpen = !this.isNotificationOpen;
  }

  closeNotifications(): void {
    this.isNotificationOpen = false;
  }

  markAsRead(notification: AppNotification): void {
    if (!notification.isRead) {
      this.notificationService.markAsRead(notification.id)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          notification.isRead = true;
          this.unreadCount = Math.max(0, this.unreadCount - 1);
        });
    }
  }

  markAllAsRead(): void {
    const user = this.authService.getCurrentUser();
    if (user) {
      this.notificationService.markAllAsRead(user.userId)
        .pipe(takeUntil(this.destroy$))
        .subscribe(() => {
          this.notifications.forEach(n => n.isRead = true);
          this.unreadCount = 0;
        });
    }
  }

  getNotificationIcon(type: number): string {
    switch (type) {
      case 1: return '⚠️'; // OutageReported
      case 2: return '🔄'; // OutageStatusChanged
      case 3: return '✅'; // OutageResolved
      case 4: return '👤'; // OutageAssigned
      case 5: return '🔔'; // SystemAlert
      case 6: return '🔧'; // MaintenancePlanned
      default: return '📢';
    }
  }

  getTimeAgo(dateString: string): string {
    const date = new Date(dateString);
    const now = new Date();
    const diffMs = now.getTime() - date.getTime();
    const diffMins = Math.floor(diffMs / 60000);
    
    if (diffMins < 1) return 'Just now';
    if (diffMins < 60) return `${diffMins}m ago`;
    
    const diffHours = Math.floor(diffMins / 60);
    if (diffHours < 24) return `${diffHours}h ago`;
    
    const diffDays = Math.floor(diffHours / 24);
    return `${diffDays}d ago`;
  }

  get username(): string {
    const user = this.authService.getCurrentUser();
    return user ? `${user.firstName} ${user.lastName}` : '';
  }

  get userRole(): string {
    return this.authService.getCurrentUser()?.role || 'guest';
  }

  toggleSidebar(): void {
    this.isSidebarOpen = !this.isSidebarOpen;
  }

  toggleSubmenu(event: Event): void {
    event.preventDefault();
    this.isCrudOpen = !this.isCrudOpen;
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}