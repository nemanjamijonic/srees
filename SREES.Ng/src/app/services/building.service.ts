import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Building, CreateBuildingRequest, UpdateBuildingRequest, BuildingSelectOption, ApiResponse } from '../models/building.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class BuildingService {
  private apiUrl = 'https://localhost:7058/api/buildings';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<Building[]>> {
    return this.http.get<ApiResponse<Building[]>>(this.apiUrl);
  }

  getAllForSelect(): Observable<ApiResponse<BuildingSelectOption[]>> {
    return this.http.get<ApiResponse<BuildingSelectOption[]>>(`${this.apiUrl}/select`);
  }

  getById(id: number): Observable<ApiResponse<Building>> {
    return this.http.get<ApiResponse<Building>>(`${this.apiUrl}/${id}`);
  }

  create(building: CreateBuildingRequest): Observable<ApiResponse<Building>> {
    return this.http.post<ApiResponse<Building>>(this.apiUrl, building);
  }

  update(id: number, building: UpdateBuildingRequest): Observable<ApiResponse<Building>> {
    return this.http.put<ApiResponse<Building>>(`${this.apiUrl}/${id}`, building);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.apiUrl}/statistics`);
  }
}
