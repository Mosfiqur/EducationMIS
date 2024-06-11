import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { AuthService } from './auth.service';
import { Http, Headers, RequestOptions } from '@angular/http';

@Injectable({
    providedIn: 'root'
})

export class NotificationService {

    constructor(private httpClientService: HttpClientService, private http: Http, private authService: AuthService) {

    }

    getAll(query) {
        const token = this.authService.userObject.token;
        let headers = new Headers({ 'Authorization': 'Bearer ' + token, 'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options = new RequestOptions({ headers: headers }); // Create a request option
        return this.http.get(`${ApiConstant.getAllNotification}?pageSize=${query.pageSize}&pageNo=${query.pageNo}`, options)
    }

    readNotification(notificationId): Promise<any> {
        return this.httpClientService.postAsync(`${ApiConstant.readNotifiaction}/${notificationId}`, {});
    }

    clearNotification(): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.clearNotification, {});
    }

}