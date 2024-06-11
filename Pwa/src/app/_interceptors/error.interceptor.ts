import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { UserDB } from '../localdb/UserDB';
import { LoadingSpinnerService } from '../core/loading-spinner/loading-spinner.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private userDb: UserDB, private toastrService: ToastrService, private loadingSpinnerService: LoadingSpinnerService) {

  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    return next.handle(req)
      .pipe(
        catchError((err: HttpErrorResponse) => {
          let errorMsg = '';
          let defaultMsg = "An unexpected error occured.";
          //https://developer.mozilla.org/en-US/docs/Web/API/ErrorEvent
          // The ErrorEvent interface represents events providing information related to errors in scripts or in files.
          if (err.error instanceof ErrorEvent) {
            errorMsg = `${err.error.message}`;
          }
          // server side error
          else {
            if (err.status === 401) {
              
              this.userDb.getUser().subscribe((res) => {
                
                if (res && res.length > 0) {
                  errorMsg = "Session expired please login again!";
                }
                else {
                  errorMsg = "Username or Password does not match!";
                }
                this.authService.logout();
                this.toastrService.error(errorMsg || defaultMsg);
                return throwError(errorMsg);

              }, (error) => {
                this.authService.logout();
              });
            }
            else if (err.status === 403) {
              return;
            }

            else if ((!navigator.onLine || err.status === 504)) {
              errorMsg = 'Could not connect to the server! Check your internet connection.';
            }
            else {
              errorMsg = `${err.error ? err.error.message : ''}`;
            }
          }
          
          if (err.status !== 401) {
            this.toastrService.error(errorMsg || defaultMsg);
            this.loadingSpinnerService.hideLoadingScreen("OnClickAvailableOffline");
            return throwError(errorMsg);
          }
        })
      );
  }
}