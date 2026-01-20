import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const authService = inject(AuthService);
  
  // URLs that don't require authentication
  const publicUrls = [
    '/auth/login',
    '/auth/register',
    '/statistics'
  ];
  
  // Check if the request URL matches any public URL pattern
  const isPublicUrl = publicUrls.some(url => req.url.includes(url));
  
  // If it's a public URL, continue without adding Authorization header
  if (isPublicUrl) {
    return next(req);
  }
  
  // Get the current user (which contains the JWT token)
  const currentUser = authService.currentUserValue;
  
  // If user is logged in and has a token, add Authorization header
  if (currentUser && currentUser.token) {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${currentUser.token}`
      }
    });
    return next(clonedRequest);
  }
  
  // If no token, continue with the original request
  return next(req);
};
