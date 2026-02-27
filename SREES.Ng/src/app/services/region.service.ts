import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Region, CreateRegionRequest, UpdateRegionRequest, ApiResponse, RegionFilterRequest, PaginatedResponse } from '../models/region.model';
import { RegionSelectOption } from '../models/region-select.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class RegionService {
  private apiUrl = 'https://localhost:7058/api/regions';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<Region[]>> {
    return this.http.get<ApiResponse<Region[]>>(this.apiUrl);
  }

  getById(id: number): Observable<ApiResponse<Region>> {
    return this.http.get<ApiResponse<Region>>(`${this.apiUrl}/${id}`);
  }

  create(region: CreateRegionRequest): Observable<ApiResponse<Region>> {
    return this.http.post<ApiResponse<Region>>(this.apiUrl, region);
  }

  update(id: number, region: UpdateRegionRequest): Observable<ApiResponse<Region>> {
    return this.http.put<ApiResponse<Region>>(`${this.apiUrl}/${id}`, region);
  }

  getAllForSelect(): Observable<ApiResponse<RegionSelectOption[]>> {
    return this.http.get<ApiResponse<RegionSelectOption[]>>(`${this.apiUrl}/getAllForSelect`);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.apiUrl}/statistics`);
  }

  getFiltered(filterRequest: RegionFilterRequest): Observable<ApiResponse<PaginatedResponse<Region[]>>> {
    let params = new HttpParams()
      .set('pageNumber', filterRequest.pageNumber.toString())
      .set('pageSize', filterRequest.pageSize.toString());

    if (filterRequest.searchTerm) params = params.set('searchTerm', filterRequest.searchTerm);
    if (filterRequest.dateFrom) params = params.set('dateFrom', filterRequest.dateFrom);
    if (filterRequest.dateTo) params = params.set('dateTo', filterRequest.dateTo);

    return this.http.get<ApiResponse<PaginatedResponse<Region[]>>>(`${this.apiUrl}/filtered`, { params });
  }
}
