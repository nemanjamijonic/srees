export interface Outage {
  id: number;
  userId: number;
  regionId: number;
  outageStatus: OutageStatus;
  description: string;
  resolvedAt: string | null;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export type OutageStatus = 'Reported' | 'Assigned' | 'InProgress' | 'Resolved';

export interface CreateOutageRequest {
  userId: number;
  regionId: number;
  description: string;
}

export interface UpdateOutageStatusRequest {
  newStatus: OutageStatus;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}