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

export interface RegionFilterRequest {
  searchTerm?: string;
  dateFrom?: string;
  dateTo?: string;
  pageNumber: number;
  pageSize: number;
}

export interface PaginatedResponse<T> {
  data: T;
  totalCount: number;
  totalPages: number;
  currentPage: number;
  pageSize: number;
}