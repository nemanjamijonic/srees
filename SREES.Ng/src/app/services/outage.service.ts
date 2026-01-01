import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Outage, CreateOutageRequest, UpdateOutageStatusRequest, ApiResponse } from '../models/outage.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class OutageService {
  private baseUrl = 'https://localhost:7058/api/outages';

  constructor(private http: HttpClient) { }

  getAllOutages(): Observable<ApiResponse<Outage[]>> {
    return this.http.get<ApiResponse<Outage[]>>(this.baseUrl);
  }

  getOutageById(id: number): Observable<ApiResponse<Outage>> {
    return this.http.get<ApiResponse<Outage>>(`${this.baseUrl}/${id}`);
  }

  createOutage(outage: CreateOutageRequest): Observable<ApiResponse<Outage>> {
    return this.http.post<ApiResponse<Outage>>(this.baseUrl, outage);
  }

  updateOutageStatus(id: number, statusUpdate: UpdateOutageStatusRequest): Observable<ApiResponse<Outage>> {
    return this.http.put<ApiResponse<Outage>>(`${this.baseUrl}/${id}/status`, statusUpdate);
  }

  deleteOutage(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.baseUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.baseUrl}/statistics`);
  }
}
