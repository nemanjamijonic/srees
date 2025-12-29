export interface Region {
  id: number;
  name: string;
  latitude: number;
  longitude: number;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreateRegionRequest {
  name: string;
  latitude: number;
  longitude: number;
}

export interface UpdateRegionRequest {
  name: string;
  latitude: number;
  longitude: number;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}