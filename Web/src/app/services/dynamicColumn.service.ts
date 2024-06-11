import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';

import { IDynamicColumnQueryModel } from '../models/queryModels/dynamic-column.model';
import { IDynamicColumn } from '../models/dynamicColumn/dynamicColumnSaveViewModel';
import { EntityType } from '../_enums/entityType';

@Injectable({
    providedIn: 'root'
})

// TODO: Return types of each methods are a lie, need to fix.
export class DynamicColumnService {  
    constructor(private httpClientService: HttpClientService){

    }

    deleteDynamicColumn(entityColumnId: any,entityType:EntityType): Promise<any> {
        if(entityType === EntityType.Budget){
            return this.httpClientService.deleteAsync(ApiConstant.DeleteBudgetDynamicColumn + '?entityDynamicColumnId='+entityColumnId);
        }
        else if(entityType === EntityType.Target){
            return this.httpClientService.deleteAsync(ApiConstant.DeleteTargetDynamicColumn + '?entityDynamicColumnId='+entityColumnId);
        }
        else if(entityType === EntityType.User){
            return this.httpClientService.deleteAsync(ApiConstant.DeleteUserDynamicColumn + '?entityDynamicColumnId='+entityColumnId);
        }
        // return this.httpClientService.deleteAsync(ApiConstant.deleteDynamicColumn + '?entityDynamicColumnId='+entityColumnId);
      }

    save(dynamicColumn: IDynamicColumn): Promise<IDynamicColumn> {
        if(dynamicColumn.entityTypeId === EntityType.Beneficiary){
            return this.httpClientService.postAsync(ApiConstant.SaveBeneficiaryDynamicColumn, dynamicColumn);
        }
        else if(dynamicColumn.entityTypeId === EntityType.Facility){
            return this.httpClientService.postAsync(ApiConstant.SaveFacilityDynamicColumn, dynamicColumn);
        }
        else if(dynamicColumn.entityTypeId === EntityType.Monitoring){
            return this.httpClientService.postAsync(ApiConstant.SaveMonitoringDynamicColumn, dynamicColumn);
        }
        else if(dynamicColumn.entityTypeId === EntityType.Budget){
            return this.httpClientService.postAsync(ApiConstant.SaveBudgetDynamicColumn, dynamicColumn);
        }
        else if(dynamicColumn.entityTypeId === EntityType.Target){
            return this.httpClientService.postAsync(ApiConstant.SaveTargetDynamicColumn, dynamicColumn);
        }
        else if(dynamicColumn.entityTypeId === EntityType.User){
            return this.httpClientService.postAsync(ApiConstant.SaveUserDynamicColumn, dynamicColumn);
        }
        //return this.httpClientService.postAsync(ApiConstant.SaveDynamicColumn, dynamicColumn);
    }
    
    getById(id,entityTypeId:EntityType): Promise<IDynamicColumn> {
        if(entityTypeId === EntityType.Beneficiary){
            return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryDynamicColumnById+ '?id=' + id )
            .then(res => {                
                return res;                
            });        }
        else if(entityTypeId === EntityType.Facility){
            return this.httpClientService.getAsync(ApiConstant.GetFacilityDynamicColumnById+ '?id=' + id )
            .then(res => {                
                return res;                
            });        }
        else if(entityTypeId === EntityType.Monitoring){
            return this.httpClientService.getAsync(ApiConstant.GetMonitoringDynamicColumnById+ '?id=' + id )
            .then(res => {                
                return res;                
            });
        }
        else if(entityTypeId === EntityType.Budget){
            return this.httpClientService.getAsync(ApiConstant.GetBudgetDynamicColumnById+ '?id=' + id )
            .then(res => {                
                return res;                
            });        }
        else if(entityTypeId === EntityType.Target){
            return this.httpClientService.getAsync(ApiConstant.GetTargetDynamicColumnById+ '?id=' + id )
            .then(res => {                
                return res;                
            });        }
        else if(entityTypeId === EntityType.User){
            return this.httpClientService.getAsync(ApiConstant.GetUserDynamicColumnById+ '?id=' + id )
            .then(res => {                
                return res;                
            });        }
    }


    getAllColumns(pageSize, pageNo,entityType,searchText): Promise<IPagedResponse<IDynamicColumn>> {
        if(entityType === EntityType.Beneficiary){
            return this.httpClientService.getAsync(ApiConstant.GetAllBeneficiaryColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {                
                return res;                
            });
        }
        else if(entityType === EntityType.Facility){
            return this.httpClientService.getAsync(ApiConstant.GetAllFacilityColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {                
                return res;                
            });        
        }
        else if(entityType === EntityType.Monitoring){
            return this.httpClientService.getAsync(ApiConstant.GetAllMonitoringColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {                
                return res;                
            });        
        }
        else if(entityType === EntityType.Budget){
            return this.httpClientService.getAsync(ApiConstant.GetAllBudgetColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {                
                return res;                
            });       
         }
        else if(entityType === EntityType.Target){
            return this.httpClientService.getAsync(ApiConstant.GetAllTargetColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {                
                return res;                
            });        
        }
        else if(entityType === EntityType.User){
            return this.httpClientService.getAsync(ApiConstant.GetAllUserColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {                
                return res;                
            });        
        }
        // return this.httpClientService.getAsync(ApiConstant.GetAllColumn+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize + "&PageNo=" + pageNo+'&searchText='+searchText)
        //     .then(res => {                
        //         return res;                
        //     });
    }
    getColumns(query: IDynamicColumnQueryModel): Promise<IPagedResponse<IDynamicColumn>> {
        return this.getAllColumns(query.pageSize, query.pageNo, query.entityType, query.searchText || "");
    }
    getNumericColumns(query: IDynamicColumnQueryModel): Promise<IPagedResponse<IDynamicColumn>> {
        return this.httpClientService.getAsync(ApiConstant.GetNumericColumn+ '?EntityTypeId=' + query.entityType + "&PageSize=" + query.pageSize + "&PageNo=" + query.pageNo+'&searchText='+query.searchText)
        .then(res => {                
            return res;                
        });
    }
    getColumnsForIndicator(pageSize, pageNo,entityType,instanceId,searchText): Promise<IPagedResponse<IDynamicColumn>> {
        if(entityType === EntityType.Beneficiary){
            return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryColumnForIndicator+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize +"&instanceId="+instanceId+ "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {
                return res;
            });
        }
        else if(entityType === EntityType.Facility){
            return this.httpClientService.getAsync(ApiConstant.GetFacilityColumnForIndicator+ '?EntityTypeId=' + entityType + "&PageSize=" + pageSize +"&instanceId="+instanceId+ "&PageNo=" + pageNo+'&searchText='+searchText)
            .then(res => {
                return res;
            });
        }
    }
}