import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer, CreateCustomerRequest, UpdateCustomerRequest, CustomerSelectOption, ApiResponse, CustomerFilterRequest, PaginatedResponse } from '../models/customer.model';
import { EntityCountStatistics } from '../models/statistics.model';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  private apiUrl = 'https://localhost:7058/api/customers';
  private headers = { 'Content-Type': 'application/json' };

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<Customer[]>> {
    return this.http.get<ApiResponse<Customer[]>>(this.apiUrl);
  }

  getFiltered(filterRequest: CustomerFilterRequest): Observable<ApiResponse<PaginatedResponse<Customer[]>>> {
    let params = new HttpParams()
      .set('pageNumber', filterRequest.pageNumber.toString())
      .set('pageSize', filterRequest.pageSize.toString());

    if (filterRequest.searchTerm) {
      params = params.set('searchTerm', filterRequest.searchTerm);
    }
    if (filterRequest.customerType !== undefined && filterRequest.customerType !== null) {
      params = params.set('customerType', filterRequest.customerType.toString());
    }
    if (filterRequest.dateFrom) {
      params = params.set('dateFrom', filterRequest.dateFrom);
    }
    if (filterRequest.dateTo) {
      params = params.set('dateTo', filterRequest.dateTo);
    }

    return this.http.get<ApiResponse<PaginatedResponse<Customer[]>>>(`${this.apiUrl}/filtered`, { params });
  }

  getAllForSelect(): Observable<ApiResponse<CustomerSelectOption[]>> {
    return this.http.get<ApiResponse<CustomerSelectOption[]>>(`${this.apiUrl}/getAllForSelect`);
  }

  getById(id: number): Observable<ApiResponse<Customer>> {
    return this.http.get<ApiResponse<Customer>>(`${this.apiUrl}/${id}`);
  }

  create(customer: CreateCustomerRequest): Observable<ApiResponse<Customer>> {
    return this.http.post<ApiResponse<Customer>>(this.apiUrl, customer);
  }

  update(id: number, customer: UpdateCustomerRequest): Observable<ApiResponse<Customer>> {
    return this.http.put<ApiResponse<Customer>>(`${this.apiUrl}/${id}`, customer);
  }

  delete(id: number): Observable<ApiResponse<null>> {
    return this.http.delete<ApiResponse<null>>(`${this.apiUrl}/${id}`);
  }

  getStatistics(): Observable<ApiResponse<EntityCountStatistics[]>> {
    return this.http.get<ApiResponse<EntityCountStatistics[]>>(`${this.apiUrl}/statistics`);
  }
}
