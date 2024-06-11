import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { TargetEndpoints } from '../utility/endpoints/target';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { ITargetFramework } from '../models/frameworks/target-framework.model';
import { ITargetFrameworkDynamicCell, ITargetFrameworkInsertModel } from '../models/frameworks/target-framework-dynamic-cell.model';

@Injectable({
    providedIn: 'root'
})
export class TargetService {
  
        
    constructor(
        private httpClientService: HttpClientService,
        private endpoints: TargetEndpoints
    ){

    }
    async getById(id: number) : Promise<ITargetFramework>{

        if(id == 0){
            return Promise.resolve({
                ageGroupId: null,
                campId: null,
                endMonth: null,
                endYear: null,
                gender: null,
                peopleInNeed: null,
                startMonth: null,
                startYear: null,
                target: null,
                targetedPopulation: null,
                unionId: null,
                upazilaId: null,
                ageGroupName:null,
                campName:null
            });
        }

        return this.httpClientService.getAsync(
            `${this.endpoints.getById}?id=${id}`
        )
    }

    async getAll(query: IBaseQueryModel) : Promise<IPagedResponse<ITargetFramework>>{
        return this.httpClientService.getAsync(`${this.endpoints.getAll}?pageNo=${query.pageNo}&pageSize=${query.pageSize}`);
    }

    async addTarget(model: ITargetFramework) {
        return this.httpClientService.postAsync(this.endpoints.add, model);
    }

    async updateTarget(model: ITargetFramework) {
        return this.httpClientService.putAsync(this.endpoints.update, model);
    }

    async deleteTarget(id: number) {
        return this.httpClientService.deleteAsync(`${this.endpoints.delete}?targetFrameworkId=${id}`);
    }
    async deleteMultipleTarget(ids: number[]) {
        return this.httpClientService.postAsync(`${this.endpoints.deleteMultiple}`,ids);
    }
    async insertDynamicCell(model: ITargetFrameworkInsertModel): Promise<void> {
        return this.httpClientService.postAsync(this.endpoints.insertDynamicCell, model);
    }

    async updateDynamicCell(model: ITargetFrameworkDynamicCell) {
        return this.httpClientService.putAsync(this.endpoints.updateDynamicCell, model);
    }

    async deleteDynamicCell(model: ITargetFrameworkDynamicCell) {
        return this.httpClientService.postAsync(this.endpoints.deleteDynamicCell, model);
    }
}