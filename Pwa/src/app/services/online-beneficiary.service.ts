import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { InstanceViewModel } from '../models/instance/instanceViewModel';
import { FacilityViewModel } from '../models/viewModel/facilityViewModel';
import { BeneficiaryViewModel } from '../models/viewModel/beneficiaryViewModel';
import { IndicatorViewModel } from '../models/indicator/IndicatorViewModel';
import { ListDataTypeViewModel } from '../models/viewModel/ListDataTypeViewModel';
import { BeneficiaryFacilityViewModel } from '../models/viewModel/BeneficiaryFacilityViewModel';
import { resolve } from 'url';


@Injectable({
  providedIn: 'root'
})
export class OnlineBeneficiaryService {

  constructor(private httpClientService: HttpClientService) { }

  async getRunningInstance(scheduleFor, pageSize, pageNo): Promise<IPagedResponse<InstanceViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetRunningInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo + "&ScheduleFor=" + scheduleFor);
  }

  async GetAllForDevice(instanceId,pageSize, pageNo,searchText?): Promise<IPagedResponse<BeneficiaryFacilityViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetAllForDevice + "?PageSize=" + pageSize 
    + "&PageNo=" + pageNo+"&SearchText=" + searchText+"&InstanceId="+instanceId)
      .then(res => {
        return res;
      });
  }

  async getAllFacility(pageSize, pageNo,instanceId,searchText?): Promise<IPagedResponse<BeneficiaryFacilityViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetAllFacilityObject + "?PageSize="
       + pageSize +"&SearchText=" + searchText+ "&PageNo=" + pageNo + "&instanceId=" + instanceId)
      .then(res => {
        return res;
      });
  }

 async saveBeneficiary(beneficiary):Promise<any>{
    return this.httpClientService.postAsync(ApiConstant.SaveBeneficiary, beneficiary);
}

  async GetByFacilityId(instanceId, facilityId, pageSize, pageNo,searchText): Promise<IPagedResponse<BeneficiaryViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetByFacilityId + "?instanceId=" + instanceId +
      "&facilityId=" + facilityId + "&PageSize=" + pageSize + "&PageNo=" + pageNo+"&SearchText=" + searchText);
  }

  async GetIndicatorByInstance(instanceId, pageSize, pageNo): Promise<IPagedResponse<IndicatorViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetIndicatorsByInstance + "?PageSize=" + pageSize + "&PageNo=" + pageNo + '&instanceId=' + instanceId)
      .then(res => {
        return res;
      });
  }

  async GetAllListTypeData(pageNo, pageSize): Promise<IPagedResponse<ListDataTypeViewModel>> {
    return this.httpClientService.getAsync(ApiConstant.GetAllListDataType + `?pageNo=1&pageSize=` + pageSize)
      .then(result => {
        return result;
      });
  }

  async GetBeneficiaryById(beneficiaryId, instanceId):Promise<IPagedResponse<BeneficiaryViewModel>>{
    return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryById+"/"+beneficiaryId+"/"+instanceId)
      .then((result) => {
        return result;
      })
  }

  async DeactivateBeneficiary(model):Promise<any>{
    return this.httpClientService.postAsync(ApiConstant.DeactivateBeneficiary,model).then((res) => {
      return res;
    });
  }
}
