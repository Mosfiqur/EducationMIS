import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { BudgetEndpoints } from '../utility/endpoints/budget';
import { IBudgetFrameworkDynamicCell } from '../models/frameworks/dynamic-cell.model';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { IBudgetFramework } from '../models/frameworks/budget-framework.model';
import { ISelectListItem } from '../models/helpers/select-list.model';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Globals } from '../globals';
import { IDonor } from '../models/frameworks/donor.model';
import { Observable } from 'rxjs';
import { IProject } from '../models/frameworks/project.model';
import { IBudgetDynamicCellInsertModel } from '../models/frameworks/budget-dynamic-cell-insert.model';

@Injectable({
    providedIn: 'root'
})
export class BudgetService {

        
    constructor(
        private httpClientService: HttpClientService,
        private endpoints: BudgetEndpoints,
        private httpClient: HttpClient,
        private globals: Globals
    ){

    }

    getById(id: number) : Observable<IBudgetFramework> {
        return this.httpClient.get<IBudgetFramework>(`${this.endpoints.getById}?id=${id}`);
    }

    async getAll(query: IBaseQueryModel) : Promise<IPagedResponse<IBudgetFramework>>{
        return this.httpClientService.getAsync(`${this.endpoints.getAll}?pageNo=${query.pageNo}&pageSize=${query.pageSize}`);
    }

    async addBudget(model: IBudgetFramework) {
        return this.httpClientService.postAsync(this.endpoints.add, model);
    }

    async addDonor(model: IDonor) {
        return this.httpClientService.postAsync(this.endpoints.addDonor, model);
    }

    async addProject(model: IProject) {
        return this.httpClientService.postAsync(this.endpoints.addProject, model);
    }

    async updateBudget(model: Partial<IBudgetFramework>) {
        return this.httpClientService.putAsync(this.endpoints.update, model);
    }

    async deleteBudget(id: number) {
        return this.httpClientService.deleteAsync(`${this.endpoints.delete}?budgetId=${id}`);
    }
    async deleteMultipleBudget(ids: number[]) {
        return this.httpClientService.postAsync(`${this.endpoints.deleteMultiple}`,ids);
    }
    async insertDynamicCell(model: IBudgetDynamicCellInsertModel): Promise<void> {
        return this.httpClientService.postAsync(this.endpoints.insertDynamicCell, model);
    }

    async updateDynamicCell(model: IBudgetFrameworkDynamicCell) {
        return this.httpClientService.putAsync(this.endpoints.updateDynamicCell, model);
    }

    async deleteDynamicCell(model: IBudgetFrameworkDynamicCell) {
        return this.httpClientService.postAsync(this.endpoints.deleteDynamicCell, model);
    }

    
    searchDonor(key: string) : Observable<IDonor[]>{        
        let api = `${this.endpoints.getAllDonors}?pageNo=1&pageSize=${this.globals.maxPageSize}&searchText=${key}`;
        return this.httpClient.get<IPagedResponse<IDonor>>(api)
        .pipe(
            map(x=> x.data)
        );
    }

    searchProject(key: string) : Observable<IProject[]>{        
        let api = `${this.endpoints.getAllProjects}?pageNo=1&pageSize=${this.globals.maxPageSize}&searchText=${key}`;
        return this.httpClient.get<IPagedResponse<IProject>>(api)
        .pipe(
            map(x=> x.data)
        );
    }
}