export interface Pole {
  id: number;
  latitude: number;
  longitude: number;
  address: string;
  poleType: number;
  regionId: number;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreatePoleRequest {
  latitude: number;
  longitude: number;
  address: string;
  poleType: number;
  regionId: number;
}

export interface UpdatePoleRequest {
  latitude: number;
  longitude: number;
  address: string;
  poleType: number;
  regionId: number;
}

export interface PoleSelectOption {
  id: number;
  address: string;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}
