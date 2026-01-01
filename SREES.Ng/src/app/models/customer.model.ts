export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  address: string;
  buildingId: number | null;
  isActive: boolean;
  customerType: number;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreateCustomerRequest {
  firstName: string;
  lastName: string;
  address: string;
  buildingId: number | null;
  isActive: boolean;
  customerType: number;
}

export interface UpdateCustomerRequest {
  firstName: string;
  lastName: string;
  address: string;
  buildingId: number | null;
  isActive: boolean;
  customerType: number;
}

export interface CustomerSelectOption {
  id: number;
  fullName: string;
}

export interface CustomerFilterRequest {
  searchTerm?: string;
  customerType?: number;
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
