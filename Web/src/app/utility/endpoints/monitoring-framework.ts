import { Injectable } from '@angular/core';
import { ApiConstant } from '../ApiConstant';

@Injectable({providedIn: 'root'})
export class MonitoringFrameworkEndpoints {
    private base: string = ApiConstant.baseUrl + "MonitoringFramework";

    getAll: string = this.base + "/GetAll";
    create: string = this.base + "/Add";
    update: string = this.base + "/Update";
    insertDynamicCell: string = this.base + "/InsertDynamicCell";
    updateDynamicCell: string = this.base + "/UpdateDynamicCell";
    deleteDynamicCell: string = this.base + "/DeleteDynamicCell";
    addIndicator: string = this.base + "/AddIndicator";
    updateIndcator: string = this.base + "/UpdateIndicator";
    getObjectiveIndicator: string = this.base + "/GetObjectiveIndicator";

}