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
export class FacilityScheduleService {

    constructor(
        private httpClientService: HttpClientService,
        private endpoints: SchedulingEndpoints
    ){

    }

    getSchedules(query: IBaseQueryModel) : Promise<IPagedResponse<ISchedule>>{
        return this.httpClientService
            .getAsync(`${this.endpoints.getFacilitySchedules}?scheduleFor=${EntityType.Facility}&pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }

    getCurrentScheduleInstances(query: IBaseQueryModel) : Promise<IPagedResponse<IScheduleInstance>>{
        return this.httpClientService
            .getAsync(`${this.endpoints.getFacilityCurrentScheduleInstances}?scheduleFor=${EntityType.Facility}&pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }

    createSchedule(model: ISchedule) {
        model.scheduleFor = EntityType.Facility;
        return this.httpClientService.postAsync(this.endpoints.createFacilitySchedule, model);
    }

    updateSchedule(model: ISchedule) {
        model.scheduleFor = EntityType.Facility;
        return this.httpClientService.putAsync(this.endpoints.updateFacilitySchedule, model);
    }

    deleteSchedule(id: number) {
        return this.httpClientService.deleteAsync(`${this.endpoints.deleteFacilitySchedule}?scheduleId=${id}`);
    }

    startCollection(model: IStartCollectionModel) {
        return this.httpClientService.putAsync(this.endpoints.startFacilityCollection, model);
    }

    startFacilityCollection(model: IStartCollectionModel) {
        return this.httpClientService.putAsync(this.endpoints.startFacilityCollection, model);
    }

    getCurrentSchedule() : Promise<Schedule> {
        return this.httpClientService.getAsync(`${this.endpoints.getFacilityCurrentSchedule}?scheduleFor=${EntityType.Facility}`)
    }
   
    getCompletedInstances(query: IBaseQueryModel){
        return this.httpClientService
            .getAsync(`${this.endpoints.GetFacilityCompletedInstances}?scheduleFor=${EntityType.Facility}&pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }
}