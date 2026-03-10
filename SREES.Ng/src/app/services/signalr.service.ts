import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import * as signalR from '@microsoft/signalr';
import { Notification } from '../models/notification.model';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class SignalRService implements OnDestroy {
  private hubConnection: signalR.HubConnection | null = null;
  private hubUrl = 'https://localhost:7058/hubs/notifications';

  // Connection state
  private connectionStateSubject = new BehaviorSubject<signalR.HubConnectionState>(
    signalR.HubConnectionState.Disconnected
  );
  public connectionState$ = this.connectionStateSubject.asObservable();

  // Notifications stream
  private notificationSubject = new Subject<Notification>();
  public notification$ = this.notificationSubject.asObservable();

  // Unread count stream
  private unreadCountSubject = new BehaviorSubject<number>(0);
  public unreadCount$ = this.unreadCountSubject.asObservable();

  constructor(private authService: AuthService) {
    // Auto-connect when user logs in
    this.authService.user$.subscribe(user => {
      if (user) {
        this.startConnection();
      } else {
        this.stopConnection();
      }
    });
  }

  /**
   * Start SignalR connection
   */
  async startConnection(): Promise<void> {
    const user = this.authService.currentUserValue;
    if (!user?.token) {
      console.warn('Cannot start SignalR connection: No authentication token');
      return;
    }

    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      console.log('SignalR already connected');
      return;
    }

    try {
      this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(this.hubUrl, {
          accessTokenFactory: () => user.token
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Information)
        .build();

      // Register event handlers
      this.registerEventHandlers();

      // Start connection
      await this.hubConnection.start();
      console.log('SignalR Connected successfully');
      this.connectionStateSubject.next(signalR.HubConnectionState.Connected);

    } catch (error) {
      console.error('SignalR Connection Error:', error);
      this.connectionStateSubject.next(signalR.HubConnectionState.Disconnected);
      // Retry connection after 5 seconds
      setTimeout(() => this.startConnection(), 5000);
    }
  }

  /**
   * Stop SignalR connection
   */
  async stopConnection(): Promise<void> {
    if (this.hubConnection) {
      try {
        await this.hubConnection.stop();
        console.log('SignalR Disconnected');
      } catch (error) {
        console.error('Error stopping SignalR connection:', error);
      }
      this.hubConnection = null;
      this.connectionStateSubject.next(signalR.HubConnectionState.Disconnected);
    }
  }

  /**
   * Register SignalR event handlers
   */
  private registerEventHandlers(): void {
    if (!this.hubConnection) return;

    // Handle incoming notifications
    this.hubConnection.on('ReceiveNotification', (notification: Notification) => {
      console.log('Received notification:', notification);
      this.notificationSubject.next(notification);
    });

    // Handle unread count updates
    this.hubConnection.on('UpdateUnreadCount', (count: number) => {
      console.log('Unread count updated:', count);
      this.unreadCountSubject.next(count);
    });

    // Handle reconnection events
    this.hubConnection.onreconnecting((error) => {
      console.log('SignalR Reconnecting...', error);
      this.connectionStateSubject.next(signalR.HubConnectionState.Reconnecting);
    });

    this.hubConnection.onreconnected((connectionId) => {
      console.log('SignalR Reconnected:', connectionId);
      this.connectionStateSubject.next(signalR.HubConnectionState.Connected);
    });

    this.hubConnection.onclose((error) => {
      console.log('SignalR Connection closed:', error);
      this.connectionStateSubject.next(signalR.HubConnectionState.Disconnected);
    });
  }

  /**
   * Join a region group to receive region-specific notifications
   */
  async joinRegionGroup(regionId: number): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.hubConnection.invoke('JoinRegionGroup', regionId);
        console.log(`Joined region group: ${regionId}`);
      } catch (error) {
        console.error('Error joining region group:', error);
      }
    }
  }

  /**
   * Leave a region group
   */
  async leaveRegionGroup(regionId: number): Promise<void> {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) {
      try {
        await this.hubConnection.invoke('LeaveRegionGroup', regionId);
        console.log(`Left region group: ${regionId}`);
      } catch (error) {
        console.error('Error leaving region group:', error);
      }
    }
  }

  /**
   * Check if connected
   */
  isConnected(): boolean {
    return this.hubConnection?.state === signalR.HubConnectionState.Connected;
  }

  /**
   * Get current unread count
   */
  getUnreadCount(): number {
    return this.unreadCountSubject.value;
  }

  /**
   * Update unread count locally (e.g., after marking as read)
   */
  updateUnreadCount(count: number): void {
    this.unreadCountSubject.next(count);
  }

  ngOnDestroy(): void {
    this.stopConnection();
  }
}
