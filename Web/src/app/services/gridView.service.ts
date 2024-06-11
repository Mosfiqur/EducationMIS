import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from '../models/beneficiary/beneficiaryViewModel';
import { GridView } from '../models/gridView/gridView';
import { EntityType } from '../_enums/entityType';
import { EntityDynamicColumn } from '../models/dynamicColumn/entityDynamicColumn';

@Injectable({
    providedIn: 'root'
})

export class GridviewService {
    
    constructor(private httpClientService: HttpClientService){

    }

    getBeneficiaryView(pageSize,pageNo): Promise<IPagedResponse<GridView>> {
        return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryGridView+ '?EntityTypeId=' + EntityType.Beneficiary+"&PageSize="+pageSize+"&PageNo="+pageNo)        
        .then(res => {           
             
            return res;
        });        
    }
    getFacilityView(pageSize,pageNo): Promise<IPagedResponse<GridView>> {
        return this.httpClientService.getAsync(ApiConstant.GetFacilityGridView+ '?EntityTypeId=' + EntityType.Facility+"&PageSize="+pageSize+"&PageNo="+pageNo)        
        .then(res => {           
             
            return res;
        });        
    }
    // addColumnToGridView(model):Promise<any>{
    //     return this.httpClientService.postAsync(ApiConstant.AddColumnToGrid,model)        
    //     .then(res => {           
    //         return res;
    //     });
    // }

    addBeneficiaryColumnToGridView(model):Promise<any>{
        return this.httpClientService.postAsync(ApiConstant.AddBeneficiaryColumnToView,model)        
        .then(res => {           
            return res;
        });
    }

    addFacilityColumnToGridView(model):Promise<any>{
        return this.httpClientService.postAsync(ApiConstant.AddFacilityColumnToView,model)        
        .then(res => {           
            return res;
        });
    }

    // deleteColumnToView(gridViewId,entityColumnId,):Promise<any>{
    //     return this.httpClientService.deleteAsync(ApiConstant.deleteColumnToViewGrid +
    //         '?gridViewId='+gridViewId +'&entityDynamicColumnId='+entityColumnId);
    // }

    deleteBeneficiaryColumnToView(gridViewId,entityColumnId,):Promise<any>{
        return this.httpClientService.deleteAsync(ApiConstant.DeleteBeneficiaryColumnToView +
            '?gridViewId='+gridViewId +'&entityDynamicColumnId='+entityColumnId);
    }

    deleteFacilityColumnToView(gridViewId,entityColumnId,):Promise<any>{
        return this.httpClientService.deleteAsync(ApiConstant.DeleteFacilityColumnToView +
            '?gridViewId='+gridViewId +'&entityDynamicColumnId='+entityColumnId);
    }
}