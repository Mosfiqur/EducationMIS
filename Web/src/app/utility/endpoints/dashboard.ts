import { Injectable } from '@angular/core';
import { ApiConstant } from '../ApiConstant';

@Injectable({
    providedIn: 'root'
})
export class DashBoardEndpoints {
    private base = ApiConstant.baseUrl + "Dashboard/";

    getTotalCounts: string = ApiConstant.baseUrl + "Dashboard/GetTotalCounts";
    getGapMaps: string = this.base + "GetGapMaps";
    getLcMaps: string = this.base + "GetLcMaps";

    addJrpParameter: string = this.base + "AddJrpParameter";
    updateJrpParameter: string = this.base + "UpdateJrpParameter";
    deleteJrpParameter: string = this.base + "DeleteJrpParameter";
    getJrpChart: string = this.base + "GetJrpChart";
    getJrpData: string = this.base + "GetJrpParameter";

    addEmbeddedDashboard: string = this.base + "AddEmbeddedDashboard";
    updateEmbeddedDashboard: string = this.base + "UpdateEmbeddedDashboard";
    deleteEmbeddedDashboard: string = this.base + "DeleteEmbeddedDashboard";
    getEmbeddedDashboard: string = this.base + "GetEmbeddedDashboard";
}