export enum NotificationType {
  OutageReported = 1,
  OutageStatusChanged = 2,
  OutageResolved = 3,
  OutageAssigned = 4,
  SystemAlert = 5,
  MaintenancePlanned = 6
}

export interface Notification {
  id: number;
  userId: number;
  customerId: number | null;
  outageId: number | null;
  title: string;
  message: string;
  notificationType: NotificationType;
  isRead: boolean;
  readAt: string | null;
  createdAt: string;
}

export interface CreateNotificationRequest {
  userId: number;
  customerId?: number | null;
  outageId?: number | null;
  title: string;
  message: string;
  notificationType: NotificationType;
}
