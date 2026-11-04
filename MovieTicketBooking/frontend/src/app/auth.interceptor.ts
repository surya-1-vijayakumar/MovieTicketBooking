import { HttpInterceptorFn } from '@angular/common/http';

const TOKEN_KEY = 'movie-ticket-booking.token';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const token = sessionStorage.getItem(TOKEN_KEY);
  if (!token) {
    return next(request);
  }

  return next(request.clone({
    setHeaders: { Authorization: `Bearer ${token}` }
  }));
};
