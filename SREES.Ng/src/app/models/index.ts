export interface User {
  id: string;
  username: string;
  email: string;
  role: UserRole;
  location?: {
    latitude: number;
    longitude: number;
  };
}

export enum UserRole {
  ADMIN = 'admin',
  CUSTOMER = 'customer',
  GUEST = 'guest'
}

export interface Fault {
  id: string;
  reportedBy: string;
  location: {
    latitude: number;
    longitude: number;
  };
  description: string;
  status: FaultStatus;
  reportedAt: Date;
  resolvedAt?: Date;
  affectedLine?: string;
  estimatedLocation?: string;
}

export enum FaultStatus {
  REPORTED = 'reported',
  IN_PROGRESS = 'in_progress',
  RESOLVED = 'resolved'
}

export interface TransformerStation {
  id: string;
  name: string;
  location: {
    latitude: number;
    longitude: number;
  };
  capacity: number;
  status: 'active' | 'inactive';
}

export interface PowerLine {
  id: string;
  name: string;
  fromStation: string;
  toStation: string;
  voltage: number;
  status: 'active' | 'inactive';
  faultHistory: Fault[];
}

export interface NetworkModel {
  stations: TransformerStation[];
  lines: PowerLine[];
}

export interface Notification {
  id: string;
  userId: string;
  message: string;
  type: 'fault_update' | 'maintenance' | 'info';
  createdAt: Date;
  read: boolean;
}