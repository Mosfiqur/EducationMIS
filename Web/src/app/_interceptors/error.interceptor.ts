import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../services/auth.service';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';

@Injectable({
    providedIn: 'root'
})
export class HttpErrorInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService, private toastrService: ToastrService){

  }

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    
    return next.handle(req)
    .pipe(        
        catchError((err: HttpErrorResponse) => {
            let errorMsg = '';
            let defaultMsg = "An unexpected error occured."
        //https://developer.mozilla.org/en-US/docs/Web/API/ErrorEvent
        // The ErrorEvent interface represents events providing information related to errors in scripts or in files.
        if (err.error instanceof ErrorEvent) {
          errorMsg = `${err.error.message}`;
        }
        // server side error
        else {
  
          if(!navigator.onLine){
            errorMsg = "Could not connect to the server! Check your internet connection.";
            this.toastrService.error(errorMsg);
            return throwError(errorMsg);
          }

          
          if(err.status === 401){
            const token = this.authService.getToken();
            if(token){
              errorMsg = "Session expired please login again!"
            }else{
              errorMsg = "Username or Password does not match!";
            }
            this.authService.logout();
            this.toastrService.error(errorMsg);
            return throwError(errorMsg);
          }
          
          if(err.status == 403){                        
            errorMsg = `You don't have permission`;
            let errorNextLine= 'to perform this action';  
            this.toastrService.warning(errorNextLine,errorMsg);    
            return throwError(errorMsg);          
          }
          errorMsg = `${err.error && err.error.message ? err.error.message: ''}`;
          

          if(!errorMsg){
            this.toastrService.error(defaultMsg);    
            return throwError(defaultMsg);
          }
        }
        this.toastrService.error(errorMsg || defaultMsg);
        
         return throwError(errorMsg);
        })
    );
  }
}