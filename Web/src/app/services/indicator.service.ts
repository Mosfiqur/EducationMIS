import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { IndicatorViewModel } from '../models/indicator/indicatorViewModel';
import { IAddIndicatorModel } from '../models/indicator/add-indicator.model';

@Injectable({
    providedIn: 'root'
})

export class IndicatorService {

    constructor(private httpClientService: HttpClientService) {

    }

    // getIndicator(pageSize, pageNo,instanceId): Promise<IPagedResponse<IndicatorViewModel>> {
    //     return this.httpClientService.getAsync(ApiConstant.GetIndicatorsByInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo+'&instanceId='+instanceId)
    //         .then(res => {
    //             return res;
    //         });
    // }

    getBeneficiaryIndicator(pageSize, pageNo,instanceId): Promise<IPagedResponse<IndicatorViewModel>> {
        return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryIndicatorsByInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo+'&instanceId='+instanceId)
            .then(res => {
                return res;
            });
    }

    getFacilityIndicator(pageSize, pageNo,instanceId): Promise<IPagedResponse<IndicatorViewModel>> {
        return this.httpClientService.getAsync(ApiConstant.GetFacilityIndicatorsByInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo+'&instanceId='+instanceId)
            .then(res => {
                return res;
            });
    }

    // getIndicator(pageSize, pageNo,instanceId): Promise<IPagedResponse<IndicatorViewModel>> {
    //     return this.httpClientService.getAsync(ApiConstant.GetIndicatorsByInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo+'&instanceId='+instanceId)
    //         .then(res => {
    //             return res;
    //         });
    // }

    // getIndicator(pageSize, pageNo,instanceId): Promise<IPagedResponse<IndicatorViewModel>> {
    //     return this.httpClientService.getAsync(ApiConstant.GetIndicatorsByInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo+'&instanceId='+instanceId)
    //         .then(res => {
    //             return res;
    //         });
    // }

    // save(model: any): Promise<any> {
    //     console.log('Saving indicators: ', model);
    //     return this.httpClientService.postAsync(ApiConstant.SaveIndicator, model);
    // }

    saveBeneficiary(model: any): Promise<any> {
        console.log('Saving indicators: ', model);
        return this.httpClientService.postAsync(ApiConstant.SaveBeneficiaryIndicator, model);
    }

    saveFacility(model: any): Promise<any> {
        console.log('Saving indicators: ', model);
        return this.httpClientService.postAsync(ApiConstant.SaveFacilityIndicator, model);
    }

    // Update(model: any): Promise<any> {
    //     console.log('Update indicators: ', model);
    //     return this.httpClientService.postAsync(ApiConstant.UpdateIndicator, model);
    // }

    UpdateBeneficiary(model: any): Promise<any> {
        console.log('Update indicators: ', model);
        return this.httpClientService.postAsync(ApiConstant.UpdateBeneficiaryIndicator, model);
    }

    UpdateFacility(model: any): Promise<any> {
        console.log('Update indicators: ', model);
        return this.httpClientService.postAsync(ApiConstant.UpdateFacilityIndicator, model);
    }

    // remove(deleteModel){
    //     return this.httpClientService.postAsync(ApiConstant.DeleteIndicator,deleteModel);
    // }

    removeBeneficiary(deleteModel){
        return this.httpClientService.postAsync(ApiConstant.DeleteBeneficiaryIndicator,deleteModel);
    }
    
    removeFacility(deleteModel){
        return this.httpClientService.postAsync(ApiConstant.DeleteFacilityIndicator,deleteModel);
    }
}