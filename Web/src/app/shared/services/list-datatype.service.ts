import { Injectable } from '@angular/core';
import { Globals } from 'src/app/globals';
import { IListDataType } from 'src/app/models/dynamicColumn/list-datatype';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { HttpClientService } from 'src/app/services/httpClientService';
import { ApiConstant } from 'src/app/utility/ApiConstant';
import { ListDataTypeEndpoints } from 'src/app/utility/endpoints/list-datatype';

@Injectable({
    providedIn: "root"
})
export class ListDataTypeService {
    constructor(
        private http: HttpClientService, 
        private endpoints: ListDataTypeEndpoints,
        private globals: Globals
        ){

    }

    getAll(): Promise<IListDataType[]>{
        return this.http.getAsync(`${this.endpoints.getAll}?pageNo=1&pageSize=${this.globals.maxPageSize}`)
        .then((pagedResponse: IPagedResponse<IListDataType>) => {
            return pagedResponse.data;
        });
    }

    getById(id: number): Promise<IListDataType>{
        return this.http.getAsync(`${this.endpoints.getById}?id=${id}`);
    }

    add(aListDataType: IListDataType): Promise<IListDataType> {
        return this.http.postAsync(`${this.endpoints.add}`, aListDataType);
    }

    update(aListDataType: IListDataType) {
        return this.http.postAsync(`${this.endpoints.update}`, aListDataType);
    }
}