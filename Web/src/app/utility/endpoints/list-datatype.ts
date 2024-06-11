import { Injectable } from '@angular/core';
import { ApiConstant } from '../ApiConstant';

@Injectable({
    providedIn: 'root'
})
export class ListDataTypeEndpoints{

    getAll: string = ApiConstant.baseUrl + "ListDataType/GetAll";
    getById: string = ApiConstant.baseUrl + "ListDataType/GetById";
    add: string = ApiConstant.baseUrl + "ListDataType/add";
    update: string = ApiConstant.baseUrl + "ListDataType/update";
}