import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { SchedulingEndpoints } from '../utility/endpoints/scheduling';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { ISchedule, Schedule } from '../models/scheduling/schedule.model';

import { IStartCollectionModel } from '../models/scheduling/start-collection.model';
import { EntityType } from '../_enums/entityType';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { IScheduleInstance } from '../models/scheduling/schedule-instance.model';

@Injectable({
    providedIn: 'root'
})
export class BeneficiaryScheduleService {

    constructor(
        private httpClientService: HttpClientService,
        private endpoints: SchedulingEndpoints
    ){

    }

    getSchedules(query: IBaseQueryModel) : Promise<IPagedResponse<ISchedule>>{
        return this.httpClientService
            .getAsync(`${this.endpoints.getBeneficiarySchedules}?scheduleFor=${EntityType.Beneficiary}&pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }

    
    getCurrentScheduleInstances(query: IBaseQueryModel) : Promise<IPagedResponse<IScheduleInstance>>{
        return this.httpClientService
            .getAsync(`${this.endpoints.getBeneficiaryCurrentScheduleInstances}?scheduleFor=${EntityType.Beneficiary}&pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }

    createSchedule(model: ISchedule) {
        model.scheduleFor = EntityType.Beneficiary;
        return this.httpClientService.postAsync(this.endpoints.createBeneficiarySchedule, model);
    }

    updateSchedule(model: ISchedule) {
        model.scheduleFor = EntityType.Beneficiary;
        return this.httpClientService.putAsync(this.endpoints.updateBeneficiarySchedule, model);
    }

    deleteSchedule(id: number) {
        return this.httpClientService.deleteAsync(`${this.endpoints.deleteBeneficiarySchedule}?scheduleId=${id}`);
    }

    startCollection(model: IStartCollectionModel,entityType:EntityType) {
        if(entityType === EntityType.Beneficiary){
            return this.startBeneficiaryCollection(model);            
        }
        else{
            return this.startFacilityCollection(model);
        }
        // return this.httpClientService.putAsync(this.endpoints.startBeneficiaryCollection, model);
    }

    startBeneficiaryCollection(model: IStartCollectionModel) {
        return this.httpClientService.putAsync(this.endpoints.startBeneficiaryCollection, model);
    }

    startFacilityCollection(model: IStartCollectionModel) {
        return this.httpClientService.putAsync(this.endpoints.startFacilityCollection, model);
    }

    getCurrentSchedule() : Promise<Schedule> {
        return this.httpClientService.getAsync(`${this.endpoints.getBeneficiaryCurrentSchedule}?scheduleFor=${EntityType.Beneficiary}`)
    }
   
    getCompletedInstances(query: IBaseQueryModel){
        return this.httpClientService
            .getAsync(`${this.endpoints.GetBeneficiaryCompletedInstances}?scheduleFor=${EntityType.Beneficiary}&pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }

    completeInstance(instanceId: number) {
        return this.httpClientService.postAsync(`${this.endpoints.CompleteBeneficiaryInstance}/${instanceId}`, {});
    }

}