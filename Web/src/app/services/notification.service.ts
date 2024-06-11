import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from '../models/beneficiary/beneficiaryViewModel';
import { BeneficiaryEditViewModel } from '../models/beneficiary/beneficiaryEditViewModel';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { IImportResult } from '../models/import/import-result.model';
import { BeneficiaryGridQueryModel } from '../models/beneficiary/beneficiaryGridQueryModel';
import { INotificationResponse } from '../models/responseModels/notificationResponseModel';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { AuthService } from './auth.service';

@Injectable({
    providedIn: 'root'
})

export class NotificationService {
   
    constructor(private httpClientService: HttpClientService, private http: Http,private authService: AuthService){

    }

    getAll(query) {
        const token = this.authService.getToken();
        let headers      = new Headers({ 'Authorization': 'Bearer '+token,'Content-Type': 'application/json' }); // ... Set content type to JSON
        let options       = new RequestOptions({ headers: headers }); // Create a request option
        return this.http.get(`${ApiConstant.getAllNotification}?pageSize=${query.pageSize}&pageNo=${query.pageNo}`,options)                  
    }
   
    readNotification(notificationId): Promise<any> {
        return this.httpClientService.postAsync(`${ApiConstant.readNotifiaction}/${notificationId}`, {});
    }

    clearNotification(): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.clearNotification, {});
    }
      
}