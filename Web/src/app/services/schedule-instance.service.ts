import { Injectable } from '@angular/core';
import { IScheduleInstance, IScheduleInstanceUpdateModel } from '../models/scheduling/schedule-instance.model';
import { ApiConstant } from '../utility/ApiConstant';
import { HttpClientService } from './httpClientService';
import { EntityType } from '../_enums/entityType';

@Injectable({
    providedIn: 'root'
})
export class ScheduleInstanceService {

    constructor(
        private httpClient: HttpClientService
    ){

    }

    updateInstance(model: IScheduleInstanceUpdateModel,entityType): Promise<void>{  
        if(entityType === EntityType.Beneficiary){
            return this.httpClient.putAsync(ApiConstant.baseUrl + "Schedule/UpdateBeneficiaryScheduleInstance", model);
        }
        else{
            return this.httpClient.putAsync(ApiConstant.baseUrl + "Schedule/UpdateFacilityScheduleInstance", model);
        }   
        // return this.httpClient.putAsync(ApiConstant.baseUrl + "Schedule/UpdateScheduleInstance", model);
    }
}