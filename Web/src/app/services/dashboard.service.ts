import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HttpClientService } from './httpClientService';
import { DashBoardEndpoints } from '../utility/endpoints/dashboard';
import { ITotalCountsModel } from '../models/dashboard/total-counts.model';
import { IGapMapModel } from '../models/dashboard/gap-map.model';
import { IGapMapQueryModel } from '../models/queryModels/gap-map-query.model';
import { ILcMapQueryModel } from '../models/dashboard/lc-map-query.model';
import { ILcMapModel } from '../models/dashboard/lc-map.model';
import { IJrpParameterInfo } from '../models/dashboard/jrp-parameter-info.model';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { EmbeddedDashboard } from '../models/dashboard/embedded-dashboard.model';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {

    constructor(
        private http: HttpClientService,
        private endpoints: DashBoardEndpoints
    ){

        
    }

    getTotalCounts() :Promise<ITotalCountsModel>{
        return this.http.getAsync(this.endpoints.getTotalCounts);
    }

    getGapMap(query: IGapMapQueryModel) : Promise<IGapMapModel[]>{
        return this.http.postAsync(this.endpoints.getGapMaps, query)
    }

    getLcMap(query: ILcMapQueryModel) : Promise<ILcMapModel[]>{
        return this.http.postAsync(this.endpoints.getLcMaps, query);        
    }

    saveJrpParameterInfo(data:IJrpParameterInfo):Promise<any>{
        return this.http.postAsync(this.endpoints.addJrpParameter, data);        
    }
    updateJrpParameterInfo(data:IJrpParameterInfo):Promise<any>{
        return this.http.postAsync(this.endpoints.updateJrpParameter, data);        
    }
    deleteJrpParameterInfo(id:number):Promise<any>{
        return this.http.postAsync(this.endpoints.deleteJrpParameter+"/"+id, {});        
    }
    getJrpData(query : IBaseQueryModel):Promise<IJrpParameterInfo[]>{
        return this.http.getAsync(`${this.endpoints.getJrpData}?pageSize=${query.pageSize}&pageNo=${query.pageNo}`);
    }
    getJrpChartData(data):Promise<any>{
        return this.http.postAsync(this.endpoints.getJrpChart, data);        
    }


    
    saveEmbeddedDashboard(data:EmbeddedDashboard):Promise<any>{
        return this.http.postAsync(this.endpoints.addEmbeddedDashboard, data);        
    }
    updateEmbeddedDashboard(data:EmbeddedDashboard):Promise<any>{
        return this.http.postAsync(this.endpoints.updateEmbeddedDashboard, data);        
    }
    deleteEmbeddedDashboard(id:number):Promise<any>{
        return this.http.postAsync(this.endpoints.deleteEmbeddedDashboard+"/"+id, {});        
    }
    getEmbeddedDashboard():Promise<EmbeddedDashboard[]>{
        return this.http.getAsync(`${this.endpoints.getEmbeddedDashboard}`);
    }
}