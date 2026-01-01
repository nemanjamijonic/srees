export interface Pole {
  id: number;
  name: string;
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
  name: string;
  latitude: number;
  longitude: number;
  address: string;
  poleType: number;
  regionId: number;
}

export interface UpdatePoleRequest {
  name: string;
  latitude: number;
  longitude: number;
  address: string;
  poleType: number;
  regionId: number;
}

export interface PoleSelectOption {
  id: number;
  name: string;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}
