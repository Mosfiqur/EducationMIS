import { Injectable } from '@angular/core';
import {
  HttpResponse,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoadingSpinnerService } from './loading-spinner.service';


@Injectable({
  providedIn: 'root'
})
export class LoadingSpinnerInterceptor implements HttpInterceptor {
  private requests: HttpRequest<any>[] = [];

  constructor(private loaderService: LoadingSpinnerService) { }

  removeRequest(req: HttpRequest<any>) {
    const i = this.requests.indexOf(req);
    if (i >= 0) {
      this.requests.splice(i, 1);
    }
    this.loaderService.isLoading.next(this.requests.length > 0);
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //this.requests.push(req);
    this.loaderService.showLoadingScreen(req);
    return Observable.create(observer => {
      const subscription = next.handle(req)
        .subscribe(
          event => {
            if (event instanceof HttpResponse) {
              this.loaderService.hideLoadingScreen(req);
              observer.next(event);
            }
          },
          err => {          
            this.loaderService.hideLoadingScreen(req);
            observer.error(err);
          },
          () => {
            this.loaderService.hideLoadingScreen(req);
            observer.complete();
          });
      // remove request from queue when cancelled
      return () => {
        this.loaderService.hideLoadingScreen(req);
        subscription.unsubscribe();
      };
    });
  }
}