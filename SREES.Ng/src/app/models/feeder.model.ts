export interface Feeder {
  id: number;
  name: string;
  feederType: number;
  substationId: number | null;
  suppliedRegions: number[] | null;
  createdAt: string;
  lastUpdateTime: string;
  guid: string;
}

export interface CreateFeederRequest {
  name: string;
  feederType: number;
  substationId: number | null;
  suppliedRegions: number[] | null;
}

export interface UpdateFeederRequest {
  name: string;
  feederType: number;
  substationId: number | null;
  suppliedRegions: number[] | null;
}

export interface FeederSelectOption {
  id: number;
  name: string;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}
