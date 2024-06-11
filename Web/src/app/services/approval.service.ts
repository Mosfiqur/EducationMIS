import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from '../models/beneficiary/beneficiaryViewModel';
import { instances } from 'chart.js';
import { FacilityViewModel } from '../models/facility/facilityViewModel';
import { BeneficiaryApprovalGridQueryModel } from '../models/beneficiary/beneficiaryApprovalGridQueryModel';

@Injectable({
    providedIn: 'root'
})

export class DataApprovalService {

    constructor(private httpClientService: HttpClientService) {

    }

    getSubmittedBeneficiaries(model: BeneficiaryApprovalGridQueryModel): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.GetSubmittedBeneficiaries ,model)
        .then(res => {
            return res;
        });
    }

    getSubmittedFacilities(data): Promise<IPagedResponse<FacilityViewModel>> {
        return this.httpClientService.postAsync(ApiConstant.GetSubmittedFacilities , data)
        .then(res => {
            return res;
        });
    }
    approveBeneficiaries(beneficiary){
        return this.httpClientService.postAsync(ApiConstant.ApproveBeneficiaries,beneficiary)
        .then(res => {
            return res;
        });
    }
    approveInactiveBeneficiaries(beneficiary){
        return this.httpClientService.postAsync(ApiConstant.ApproveInactiveBeneficiaries,beneficiary)
        .then(res => {
            return res;
        });
    }
    approveFacilities(facilities){
        return this.httpClientService.postAsync(ApiConstant.ApproveFacilities,facilities)
        .then(res => {
            return res;
        });
    }
    recollectBeneficiaries(beneficiary){
        return this.httpClientService.postAsync(ApiConstant.RecollectBeneficiaries,beneficiary)
        .then(res => {
            return res;
        });
    }
    recollectFacility(facilities){
        return this.httpClientService.postAsync(ApiConstant.RecollectFacility,facilities)
        .then(res => {
            return res;
        });
    }
}