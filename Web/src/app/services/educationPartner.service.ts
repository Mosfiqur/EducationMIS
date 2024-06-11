import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { InstanceViewModel } from '../models/instance/instanceViewModel';
import { EducationSectorPartner } from '../models/educationSectorPartner/educationSectorPartner';

@Injectable({
    providedIn: 'root'
})
export class EducationPartnerService {

    constructor(private httpClientService: HttpClientService) {

    }
    getAll(): Promise<EducationSectorPartner[]> {
        return this.httpClientService.getAsync(ApiConstant.EducationSectorPartner)
            .then(res => {
                return res;
            });
    }

}