import { Injectable } from '@angular/core';
import { ApiConstant } from '../ApiConstant';

@Injectable({providedIn: 'root'})
export class TargetEndpoints {
    private base : string =  ApiConstant.baseUrl + "TargetFramework/";

    add: string  = this.base + "Add";
    update: string  = this.base + "Update";
    delete: string  = this.base + "Delete";
    deleteMultiple: string  = this.base + "DeleteMultiple";
    getAll: string  = this.base + "GetAll";
    insertDynamicCell: string  = this.base + "InsertDynamicCell";
    updateDynamicCell: string  = this.base + "UpdateDynamicCell";
    deleteDynamicCell: string  = this.base + "DeleteDynamicCell";   
    getById: string = this.base + "GetById";

}