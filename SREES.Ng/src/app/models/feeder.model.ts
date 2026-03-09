export interface Feeder {
  id: number;
  name: string;
  feederType: number;
  substationId: number | null;
  suppliedRegions: number[] | null;
  latitude: number | null;
  longitude: number | null;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreateFeederRequest {
  name: string;
  feederType: number;
  substationId: number | null;
  suppliedRegions: number[] | null;
  latitude: number | null;
  longitude: number | null;
}

export interface UpdateFeederRequest {
  name: string;
  feederType: number;
  substationId: number | null;
  suppliedRegions: number[] | null;
  latitude: number | null;
  longitude: number | null;
}

export interface FeederSelectOption {
  id: number;
  name: string;
}

export interface FeederFilterRequest {
  searchTerm?: string;
  feederType?: number;
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
