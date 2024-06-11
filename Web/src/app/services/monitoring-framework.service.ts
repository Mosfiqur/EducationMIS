import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';

import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { IMonitoringFramework, IMonitoringFrameworkUpdateModel } from '../models/frameworks/monitoring-framework.model';
import { IMonitoringDynamicCellInsertModel, IMonitoringFrameworkDynamicCell } from '../models/frameworks/dynamic-cell.model';
import { IObjectiveIndicator } from '../models/frameworks/objective-indicator.model';
import { MonitoringFrameworkEndpoints } from '../utility/endpoints/monitoring-framework';


@Injectable({
    providedIn: 'root'
})
export class MonitoringFrameworkService {
  

    constructor(
        private httpClientService: HttpClientService, 
        private endpoint: MonitoringFrameworkEndpoints
        ){}

    async getAll(query: IBaseQueryModel): Promise<IPagedResponse<IMonitoringFramework>> {
        return this.httpClientService.getAsync(`${this.endpoint.getAll}?pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }

    async create(model: IMonitoringFramework): Promise<IMonitoringFramework> {
        return this.httpClientService.postAsync(this.endpoint.create, model);
    }

    async update(model: IMonitoringFramework) {
        return this.httpClientService.putAsync(this.endpoint.update, model);
    }

    async insertDynamicCell(model: IMonitoringDynamicCellInsertModel):Promise<void> {
        return this.httpClientService.postAsync(this.endpoint.insertDynamicCell, model);
    }
        
    async updateDynamicCell(model: IMonitoringFrameworkDynamicCell) {
        return this.httpClientService.putAsync(this.endpoint.updateDynamicCell, model);
    }

    async deleteDynamicCell(model: IMonitoringFrameworkDynamicCell) {
        return this.httpClientService.postAsync(this.endpoint.deleteDynamicCell, model);
    }

    async addIndicator(indicator: IObjectiveIndicator): Promise<IObjectiveIndicator> {
        return this.httpClientService.postAsync(this.endpoint.addIndicator, indicator);
    }

    async updateIndicator(indicator: IObjectiveIndicator): Promise<void> {
        return this.httpClientService.putAsync(this.endpoint.updateIndcator, indicator);
    }

    async getObjectiveIndicator(query: IBaseQueryModel): Promise<IPagedResponse<IObjectiveIndicator>> {
        return this.httpClientService.getAsync(`${this.endpoint.getObjectiveIndicator}?pageSize=${query.pageSize}&pageNo=${query.pageNo}&searchText=${query.searchText}`);
    }
  
}