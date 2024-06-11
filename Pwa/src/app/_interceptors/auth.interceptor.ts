import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UserDB } from '../localdb/UserDB';
import { concatMap } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
})
export class AuthInterceptor implements HttpInterceptor {

    constructor(private userDb: UserDB) {

    }
    intercept(
        req: HttpRequest<any>,
        next: HttpHandler): Observable<HttpEvent<any>> {
        return this.userDb.getUser().pipe(
            concatMap(data => {
                if (data && data.length > 0) {
                    const token = data[0].token;
                    if (token) {
                        const request = req.clone({
                            setHeaders: {
                                Authorization: 'Bearer ' + token
                            }
                        });
                        return next.handle(request);
                    } 
                    else{
                        return next.handle(req);
                    }
                }
                else {
                    return next.handle(req);
                }
            })
        );
    }
}