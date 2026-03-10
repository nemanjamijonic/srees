import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ResponsePackage } from '../models/response-package.model';
import { User, CreateUserRequest, UpdateUserRequest } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'https://localhost:7058/api/users';

  constructor(private http: HttpClient) {}

  /**
   * Get all users (Admin only)
   */
  getAll(): Observable<ResponsePackage<User[]>> {
    return this.http.get<ResponsePackage<User[]>>(this.baseUrl);
  }

  /**
   * Get user by ID (Admin only)
   */
  getById(id: number): Observable<ResponsePackage<User>> {
    return this.http.get<ResponsePackage<User>>(`${this.baseUrl}/${id}`);
  }

  /**
   * Create a new user (Admin only)
   */
  create(user: CreateUserRequest): Observable<ResponsePackage<User>> {
    return this.http.post<ResponsePackage<User>>(this.baseUrl, user);
  }

  /**
   * Update an existing user (Admin only)
   */
  update(id: number, user: UpdateUserRequest): Observable<ResponsePackage<User>> {
    return this.http.put<ResponsePackage<User>>(`${this.baseUrl}/${id}`, user);
  }

  /**
   * Delete a user (Admin only)
   */
  delete(id: number): Observable<ResponsePackage<string>> {
    return this.http.delete<ResponsePackage<string>>(`${this.baseUrl}/${id}`);
  }
}
