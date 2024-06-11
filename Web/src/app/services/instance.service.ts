import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { InstanceViewModel } from '../models/instance/instanceViewModel';
import { EntityType } from '../_enums/entityType';

@Injectable({
    providedIn: 'root'
})

export class InstanceService {

    constructor(private httpClientService: HttpClientService) {

    }

    getNotPendingInstances(scheduleFor, pageSize, pageNo): Promise<IPagedResponse<InstanceViewModel>> {
        if(scheduleFor === EntityType.Beneficiary){
            return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryNotPendingInstances + "?PageSize=" + pageSize + "&PageNo=" + pageNo + "&ScheduleFor=" + scheduleFor)
            .then(res => {
                return res;
            });
        }
        else{
            return this.httpClientService.getAsync(ApiConstant.GetFacilityNotPendingInstances + "?PageSize=" + pageSize + "&PageNo=" + pageNo + "&ScheduleFor=" + scheduleFor)
            .then(res => {
                return res;
            });
        }
    }

    getRunningInstance(scheduleFor, pageSize, pageNo): Promise<IPagedResponse<InstanceViewModel>> {
        return this.httpClientService.getAsync(ApiConstant.GetRunningInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo + "&ScheduleFor=" + scheduleFor)
            .then(res => {
                return res;
            });
    }

    getInstanceStatusWise(scheduleFor:number, instanceStatus, pageSize, pageNo): Promise<IPagedResponse<InstanceViewModel>> {
        if (scheduleFor == EntityType.Beneficiary) {
            return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryInstancesStatusWise + "?PageSize=" + pageSize + "&PageNo=" + pageNo + "&ScheduleFor=" + scheduleFor + "&InstanceStatus=" + instanceStatus)
                .then(res => {
                    return res;
                });
        }
        else {
            return this.httpClientService.getAsync(ApiConstant.GetFacilityInstancesStatusWise + "?PageSize=" + pageSize + "&PageNo=" + pageNo + "&ScheduleFor=" + scheduleFor + "&InstanceStatus=" + instanceStatus)
                .then(res => {
                    return res;
                });
        }
    }

    // getBeneficiaryInstanceStatusWise(scheduleFor, instanceStatus, pageSize, pageNo): Promise<IPagedResponse<InstanceViewModel>> {

    // }

    // getFacilityInstanceStatusWise(scheduleFor, instanceStatus, pageSize, pageNo): Promise<IPagedResponse<InstanceViewModel>> {

    // }
}