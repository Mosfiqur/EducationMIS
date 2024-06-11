import { ApiConstant } from '../ApiConstant';
import { Injectable } from '@angular/core';


@Injectable({providedIn: 'root'})
export class BudgetEndpoints {

    private base = ApiConstant.baseUrl + "BudgetFramework/";

    add: string  = ApiConstant.baseUrl + "BudgetFramework/Add";
    update: string  = ApiConstant.baseUrl + "BudgetFramework/Update";
    delete: string  = ApiConstant.baseUrl + "BudgetFramework/Delete";
    deleteMultiple: string  = ApiConstant.baseUrl + "BudgetFramework/DeleteMultiple";
    getAll: string  = ApiConstant.baseUrl + "BudgetFramework/GetAll";
    insertDynamicCell: string  = ApiConstant.baseUrl + "BudgetFramework/InsertDynamicCell";
    updateDynamicCell: string  = ApiConstant.baseUrl + "BudgetFramework/UpdateDynamicCell";
    deleteDynamicCell: string  = ApiConstant.baseUrl + "BudgetFramework/DeleteDynamicCell";   

    addDonor: string = this.base + "AddDonor";
    addProject: string = this.base + "AddProject";
    getAllDonors: string = this.base + "GetAllDonors";
    getAllProjects: string = this.base + "GetAllProjects";
    getById: string = this.base + "GetById";
}