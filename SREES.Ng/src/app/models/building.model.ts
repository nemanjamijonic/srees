export interface Building {
  id: number;
  latitude: number;
  longitude: number;
  ownerName: string;
  address: string;
  regionId: number;
  poleId: number;
  poleType: number;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreateBuildingRequest {
  latitude: number;
  longitude: number;
  ownerName: string;
  address: string;
  regionId: number;
  poleId: number;
}

export interface UpdateBuildingRequest {
  latitude: number;
  longitude: number;
  ownerName: string;
  address: string;
  regionId: number;
  poleId: number;
}

export interface BuildingSelectOption {
  id: number;
  address: string;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}
