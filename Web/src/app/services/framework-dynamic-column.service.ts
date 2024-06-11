import { HttpClientService } from './httpClientService';
import { IFrameworkDynamicColumn } from '../models/frameworks/framework-dynamic-column.model';
import { ApiConstant } from '../utility/ApiConstant';
import { FrameworkDynamicColumnEndpoints } from '../utility/endpoints/frameworkDynamicColumns';
import { FrameworkDynamicColumnQueryModel } from '../models/queryModels/framework-dynamic-column.query.model';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class FrameworkDynamicColumnService {
    
    constructor(private httpClient: HttpClientService){
        
    }

    async addColumn(model: IFrameworkDynamicColumn) : Promise<IFrameworkDynamicColumn>{
        return this.httpClient.postAsync(FrameworkDynamicColumnEndpoints.add, model);
    }

    
    async updateColumn(model: IFrameworkDynamicColumn) {
        return this.httpClient.putAsync(FrameworkDynamicColumnEndpoints.update, model);
    }

    async getById(id: number) : Promise<IFrameworkDynamicColumn> {
        return this.httpClient.getAsync(`${FrameworkDynamicColumnEndpoints.getById}?id=${id}`);
    }

    async delete(id: number) {
        return this.httpClient.deleteAsync(`${FrameworkDynamicColumnEndpoints.delete}?id=${id}`);
    }
    
    async getByFrameworkType(query: FrameworkDynamicColumnQueryModel) : Promise<IPagedResponse<IFrameworkDynamicColumn>> {
        return this.httpClient.getAsync(`${FrameworkDynamicColumnEndpoints.getByFrameworkType}?frameworkType=${query.frameworkType}&pageNo=${query.pageNo}&pageSize=${query.pageSize}`);
    }
}