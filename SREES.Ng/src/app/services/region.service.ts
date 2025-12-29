import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Region, CreateRegionRequest, UpdateRegionRequest, ApiResponse } from '../models/region.model';
import { RegionSelectOption } from '../models/region-select.model';

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
}