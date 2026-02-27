import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Substation, CreateSubstationRequest, UpdateSubstationRequest, SubstationSelectOption, SubstationFilterRequest, PaginatedResponse } from '../models/substation.model';
import { ApiResponse } from '../models/region.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class SubstationService {
  private apiUrl = 'https://localhost:7058/api/substations';

  constructor(private http: HttpClient) {}

  getAll(): Observable<ApiResponse<Substation[]>> {
    return this.http.get<ApiResponse<Substation[]>>(this.apiUrl);
  }

  getAllForSelect(): Observable<ApiResponse<SubstationSelectOption[]>> {
    return this.http.get<ApiResponse<SubstationSelectOption[]>>(`${this.apiUrl}/getAllForSelect`);
  }

  getById(id: number): Observable<ApiResponse<Substation>> {
    return this.http.get<ApiResponse<Substation>>(`${this.apiUrl}/${id}`);
  }

  create(substation: CreateSubstationRequest): Observable<ApiResponse<Substation>> {
    return this.http.post<ApiResponse<Substation>>(this.apiUrl, substation);
  }

  update(id: number, substation: UpdateSubstationRequest): Observable<ApiResponse<Substation>> {
    return this.http.put<ApiResponse<Substation>>(`${this.apiUrl}/${id}`, substation);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.apiUrl}/statistics`);
  }

  getFiltered(filterRequest: SubstationFilterRequest): Observable<ApiResponse<PaginatedResponse<Substation[]>>> {
    let params = new HttpParams()
      .set('pageNumber', filterRequest.pageNumber.toString())
      .set('pageSize', filterRequest.pageSize.toString());

    if (filterRequest.searchTerm) params = params.set('searchTerm', filterRequest.searchTerm);
    if (filterRequest.substationType !== undefined && filterRequest.substationType !== null)
      params = params.set('substationType', filterRequest.substationType.toString());
    if (filterRequest.dateFrom) params = params.set('dateFrom', filterRequest.dateFrom);
    if (filterRequest.dateTo) params = params.set('dateTo', filterRequest.dateTo);

    return this.http.get<ApiResponse<PaginatedResponse<Substation[]>>>(`${this.apiUrl}/filtered`, { params });
  }
}
