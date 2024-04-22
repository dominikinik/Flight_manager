import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { UserService } from './user.service';

export const customInterceptor: HttpInterceptorFn = (req, next) => {

  const userSrv = inject(UserService);

  const token = localStorage.getItem('angular17TokenData');

  let cloneRequest;
  if (token) {
    cloneRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  } else {
    cloneRequest = req.clone();
  }

  return next(cloneRequest).pipe(
    catchError((error: HttpErrorResponse) => {

      if (error.status === 401) {
        const isRefrsh = confirm("Tocken refreshed :)");

        if (isRefrsh) {
          userSrv.$refreshToken.next(true);
        }
      }
      return throwError("");
    })
  );
}
