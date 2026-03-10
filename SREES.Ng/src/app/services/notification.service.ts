import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResponsePackage } from '../models/response-package.model';
import { Notification, CreateNotificationRequest } from '../models/notification.model';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private baseUrl = 'https://localhost:7058/api/notifications';

  constructor(private http: HttpClient) {}

  /**
   * Get all notifications for a specific user
   */
  getByUserId(userId: number): Observable<ResponsePackage<Notification[]>> {
    return this.http.get<ResponsePackage<Notification[]>>(`${this.baseUrl}/user/${userId}`);
  }

  /**
   * Get unread notifications for a specific user
   */
  getUnreadByUserId(userId: number): Observable<ResponsePackage<Notification[]>> {
    return this.http.get<ResponsePackage<Notification[]>>(`${this.baseUrl}/user/${userId}/unread`);
  }

  /**
   * Get count of unread notifications for a specific user
   */
  getUnreadCountByUserId(userId: number): Observable<ResponsePackage<number>> {
    return this.http.get<ResponsePackage<number>>(`${this.baseUrl}/user/${userId}/unread/count`);
  }

  /**
   * Create a new notification (Admin only)
   */
  create(notification: CreateNotificationRequest): Observable<ResponsePackage<Notification>> {
    return this.http.post<ResponsePackage<Notification>>(this.baseUrl, notification);
  }

  /**
   * Mark a notification as read
   */
  markAsRead(notificationId: number): Observable<ResponsePackage<boolean>> {
    return this.http.put<ResponsePackage<boolean>>(`${this.baseUrl}/${notificationId}/read`, {});
  }

  /**
   * Mark all notifications as read for a specific user
   */
  markAllAsRead(userId: number): Observable<ResponsePackage<boolean>> {
    return this.http.put<ResponsePackage<boolean>>(`${this.baseUrl}/user/${userId}/read-all`, {});
  }

  /**
   * Delete a notification
   */
  delete(notificationId: number): Observable<ResponsePackage<string>> {
    return this.http.delete<ResponsePackage<string>>(`${this.baseUrl}/${notificationId}`);
  }
}
