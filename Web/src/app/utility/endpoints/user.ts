import { Injectable } from '@angular/core';
import { IUserQueryModel } from 'src/app/models/queryModels/user-query.model';
import { ApiConstant } from '../ApiConstant';

@Injectable({providedIn: 'root'})
export class UserEndpoints {
    
    private base : string =  ApiConstant.baseUrl + "User/";

    add: string  = this.base + "Add";
    update: string  = this.base + "Update";
    delete: string  = this.base + "Delete";
    getAll: string  = this.base + "GetAll";
    insertDynamicCell: string  = this.base + "InsertDynamicCell";
    updateDynamicCell: string  = this.base + "UpdateDynamicCell";
    deleteDynamicCell: string  = this.base + "DeleteDynamicCell";   
    getById: string = this.base + "GetById";
    exportFiltered: string = this.base + "ExportFilteredUsers";
}