import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, map } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { ResponsePackage } from '../models/response-package.model';
import { LoginResponse } from '../models/login-response.model';
import { RegisterRequest } from '../models/register-request.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7058/api/auth';
  private userSubject = new BehaviorSubject<LoginResponse | null>(null);
  public user$ = this.userSubject.asObservable();

  constructor(private http: HttpClient) {
    // Attempt to load stored user from localStorage on initialization
    const storedUser = localStorage.getItem('user');
    if (storedUser) {
      try {
        this.userSubject.next(JSON.parse(storedUser));
      } catch (e) {
        localStorage.removeItem('user');
      }
    }
  }

  public get currentUserValue(): LoginResponse | null {
    return this.userSubject.value;
  }

  login(loginRequest: LoginRequest): Observable<ResponsePackage<LoginResponse>> {
    return this.http.post<ResponsePackage<LoginResponse>>(`${this.apiUrl}/login`, loginRequest)
      .pipe(map(response => {
        if (response.data) {
          localStorage.setItem('user', JSON.stringify(response.data));
          this.userSubject.next(response.data);
        }
        return response;
      }));
  }

  register(registerRequest: RegisterRequest): Observable<ResponsePackage<LoginResponse>> {
    return this.http.post<ResponsePackage<LoginResponse>>(`${this.apiUrl}/register`, registerRequest)
      .pipe(map(response => {
        // Automatically login after successful registration if token is returned
        if (response.data) {
          localStorage.setItem('user', JSON.stringify(response.data));
          this.userSubject.next(response.data);
        }
        return response;
      }));
  }

  logout() {
    localStorage.removeItem('user');
    this.userSubject.next(null);
  }

  isLoggedIn(): boolean {
    return this.userSubject.value !== null;
  }

  getCurrentUser(): LoginResponse | null {
    return this.userSubject.value;
  }
}
//         };
        
//         localStorage.setItem('currentUser', JSON.stringify(user));
//         this.currentUserSubject.next(user);
//         observer.next(true);
//         observer.complete();
//       }, 1000);
//     });
//   }

//   logout(): void {
//     localStorage.removeItem('currentUser');
//     this.currentUserSubject.next(null);
//   }

//   isLoggedIn(): boolean {
//     return this.currentUserSubject.value !== null;
//   }

//   getCurrentUser(): User | null {
//     return this.currentUserSubject.value;
//   }

//   hasRole(role: UserRole): boolean {
//     const user = this.getCurrentUser();
//     return user?.role === role;
//   }
// }