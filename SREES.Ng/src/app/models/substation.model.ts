export interface Substation {
  id: number;
  substationType: number;
  latitude: number;
  longitude: number;
  name: string;
  regionId: number;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreateSubstationRequest {
  substationType: number;
  latitude: number;
  longitude: number;
  name: string;
  regionId: number;
}
export interface SubstationSelectOption {
  id: number;
  name: string;
}
export interface UpdateSubstationRequest {
  substationType: number;
  latitude: number;
  longitude: number;
  name: string;
  regionId: number;
}

export interface SubstationFilterRequest {
  searchTerm?: string;
  substationType?: number;
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