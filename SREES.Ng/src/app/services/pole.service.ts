import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Pole, CreatePoleRequest, UpdatePoleRequest, PoleSelectOption, ApiResponse } from '../models/pole.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class PoleService {
  private apiUrl = 'https://localhost:7058/api/poles';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<Pole[]>> {
    return this.http.get<ApiResponse<Pole[]>>(this.apiUrl);
  }

  getAllForSelect(): Observable<ApiResponse<PoleSelectOption[]>> {
    return this.http.get<ApiResponse<PoleSelectOption[]>>(`${this.apiUrl}/getAllForSelect`);
  }

  getById(id: number): Observable<ApiResponse<Pole>> {
    return this.http.get<ApiResponse<Pole>>(`${this.apiUrl}/${id}`);
  }

  create(pole: CreatePoleRequest): Observable<ApiResponse<Pole>> {
    return this.http.post<ApiResponse<Pole>>(this.apiUrl, pole);
  }

  update(id: number, pole: UpdatePoleRequest): Observable<ApiResponse<Pole>> {
    return this.http.put<ApiResponse<Pole>>(`${this.apiUrl}/${id}`, pole);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.apiUrl}/statistics`);
  }
}
