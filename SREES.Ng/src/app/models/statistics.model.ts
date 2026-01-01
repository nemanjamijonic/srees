export interface EntityCountStatistics {
  name: string;
  count: number;
  type: string | null;
}

export interface ApiResponse<T> {
  message: string;
  data: T;
}
