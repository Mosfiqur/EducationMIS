import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { facilityWiseIndicatorViewModel, BeneficiaryWiseIndicatorViewModel } from '../models/viewModel/facilityWiseIndicatorViewModel';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { ApiConstant } from '../utility/ApiConstant';
import { facilityDynamicCellAddViewModel, BeneficiaryDynamicCellAddViewModel } from '../models/viewModel/facilityDynamicCellAddViewModel';
import { FacilityViewModel } from '../models/viewModel/facilityViewModel';

@Injectable({
  providedIn: 'root'
})
export class OnlineFacilityService {

  constructor(private httpClientService: HttpClientService) { }

  getFacilityIndicator(instanceId, facilityId, pageSize, pageNo): Promise<IPagedResponse<facilityWiseIndicatorViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetFacilityIndicator + "?PageSize=" + pageSize
      + "&PageNo=" + pageNo + '&instanceId=' + instanceId + '&facilityId=' + facilityId)
      .then(res => {
        return res;
      });
  }

  getBeneficiaryIndicator(instanceId,beneficiaryId, pageSize,pageNo): Promise<IPagedResponse<BeneficiaryWiseIndicatorViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryIndicator + "?PageSize=" + pageSize 
                                + "&PageNo=" + pageNo+'&instanceId='+instanceId+'&beneficiaryId='+beneficiaryId)
        .then(res => {
            return res;
        });
      }


  saveFacilityCell(model): Promise<IPagedResponse<facilityDynamicCellAddViewModel>> {
    return this.httpClientService.postAsync(ApiConstant.SaveFacilityCell, model);
  }

  getFacilityById(id): Promise<FacilityViewModel> {
    return this.httpClientService.getAsync(ApiConstant.GetFacilityById + "/" + id)
      .then(res => {
        return res;
      });
  }

  saveBeneficiaryCell(model): Promise<IPagedResponse<BeneficiaryDynamicCellAddViewModel>>{
    return this.httpClientService.postAsync(ApiConstant.SaveBeneficiaryCell,model);
  }
  
}