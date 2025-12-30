import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Substation, CreateSubstationRequest, UpdateSubstationRequest, SubstationSelectOption } from '../models/substation.model';
import { ApiResponse } from '../models/region.model';

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
}