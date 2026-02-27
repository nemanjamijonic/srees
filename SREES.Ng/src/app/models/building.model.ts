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

export interface BuildingFilterRequest {
  searchTerm?: string;
  poleType?: number;
  dateFrom?: string;
  dateTo?: string;
  pageNumber: number;
  pageSize: number;
}

export interface PaginatedResponse<T> {
  totalCount: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
  data: T;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}
