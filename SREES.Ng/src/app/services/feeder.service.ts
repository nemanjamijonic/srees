import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Feeder, CreateFeederRequest, UpdateFeederRequest, FeederSelectOption, ApiResponse, FeederFilterRequest, PaginatedResponse } from '../models/feeder.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class FeederService {
  private apiUrl = 'https://localhost:7058/api/feeders';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<Feeder[]>> {
    return this.http.get<ApiResponse<Feeder[]>>(this.apiUrl);
  }

  getFiltered(filterRequest: FeederFilterRequest): Observable<ApiResponse<PaginatedResponse<Feeder[]>>> {
    let params = new HttpParams()
      .set('pageNumber', filterRequest.pageNumber.toString())
      .set('pageSize', filterRequest.pageSize.toString());

    if (filterRequest.searchTerm) {
      params = params.set('searchTerm', filterRequest.searchTerm);
    }
    if (filterRequest.feederType !== undefined && filterRequest.feederType !== null) {
      params = params.set('feederType', filterRequest.feederType.toString());
    }
    if (filterRequest.dateFrom) {
      params = params.set('dateFrom', filterRequest.dateFrom);
    }
    if (filterRequest.dateTo) {
      params = params.set('dateTo', filterRequest.dateTo);
    }

    return this.http.get<ApiResponse<PaginatedResponse<Feeder[]>>>(`${this.apiUrl}/filtered`, { params });
  }

  getAllForSelect(): Observable<ApiResponse<FeederSelectOption[]>> {
    return this.http.get<ApiResponse<FeederSelectOption[]>>(`${this.apiUrl}/select`);
  }

  getById(id: number): Observable<ApiResponse<Feeder>> {
    return this.http.get<ApiResponse<Feeder>>(`${this.apiUrl}/${id}`);
  }

  create(feeder: CreateFeederRequest): Observable<ApiResponse<Feeder>> {
    return this.http.post<ApiResponse<Feeder>>(this.apiUrl, feeder);
  }

  update(id: number, feeder: UpdateFeederRequest): Observable<ApiResponse<Feeder>> {
    return this.http.put<ApiResponse<Feeder>>(`${this.apiUrl}/${id}`, feeder);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.apiUrl}/statistics`);
  }
}
