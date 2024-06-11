import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from '../models/beneficiary/beneficiaryViewModel';
import { BeneficiaryEditViewModel } from '../models/beneficiary/beneficiaryEditViewModel';
import { FacilityViewModel } from '../models/facility/facilityViewModel';
import { TeacherViewModel } from '../models/user/teacherViewModel';
import { FacilityEditViewModel } from '../models/facility/facilityEditViewModel';
import { IImportResult } from '../models/import/import-result.model';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { IFacilityDeleteModel } from '../models/facility/facility-delete.model';
import { IDynamicCell } from '../models/frameworks/dynamic-cell.model';
import { IFacilityDynamicCellAddModel } from '../models/facility/dynamic-cell-add.model';
import { IFacilityDynamicCellDeleteModel } from '../models/facility/dynamic-cell-delete.model';

@Injectable({
    providedIn: 'root'
})
export class FacilityService {
   

    constructor(private httpClientService: HttpClientService) {

    }
    getById(id): Promise<FacilityEditViewModel> {
        return this.httpClientService.getAsync(ApiConstant.GetFacilityById + "/" + id)
            .then(res => {
                return res;
            });
    }
    getAllByViewId(data): Promise<IPagedResponse<FacilityViewModel>> {
        return this.httpClientService.postAsync(ApiConstant.GetFacilityByViewId ,data)
            .then(res => {

                return res;
            });
    }
    getAllByInstanceId(data): Promise<IPagedResponse<FacilityViewModel>> {
        return this.httpClientService.postAsync(ApiConstant.GetFacilityByInstanceId ,data)
            .then(res => {
                return res;
            });
    }
    getAllLatest(query: IBaseQueryModel): Promise<IPagedResponse<FacilityViewModel>> {
        return this.httpClientService.getAsync(`${ApiConstant.GetAllFacilityLatest}?pageSize=${query.pageSize}&pageNo=${query.pageNo}&searchText=${query.searchText}`)
            .then(res => {

                return res;
            });
    }
    getAllFilterData(data): Promise<IPagedResponse<FacilityViewModel>> {
        return this.httpClientService.postAsync(ApiConstant.GetAllFilteredDataFacilityObject,data)
            .then(res => {

                return res;
            });
    }
    facilityCellSave(dynamicCell: IFacilityDynamicCellAddModel): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.SaveFacilityCell, dynamicCell);
    }

    deleteFacilityCell(dynamicCell: IFacilityDynamicCellDeleteModel): Promise<any> {        
        let str = `instanceId=${dynamicCell.instanceId}&facilityId=${dynamicCell.facilityId}&entityDynamicColumnId=${dynamicCell.entityDynamicColumnId}`;
        return this.httpClientService.deleteAsync(`${ApiConstant.DeleteFacilityCell}?${str}`);
    }

    save(facility): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.SaveFacility, facility);
    }
    update(facility): Promise<any> {
        return this.httpClientService.putAsync(ApiConstant.UpdateFacility, facility);
    }
    delete(model: IFacilityDeleteModel): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.DeleteFacility, model);
    }
    assignTeacher(facility) {
        return this.httpClientService.postAsync(ApiConstant.AssignTeacher, facility);
    }
    getTeachers(pageSize, pageNo,searchText): Promise<IPagedResponse<TeacherViewModel>> {
        return this.httpClientService.getAsync(ApiConstant.GetTeacher + "?PageSize=" + pageSize + "&PageNo=" + pageNo+"&searchText="+searchText)
            .then(res => {
                return res;
            });
    }

    importFacilities(formData: FormData):Promise<IImportResult> {
        return this.httpClientService.postAsync(ApiConstant.ImportFacilities, formData);        
    }

    downloadImportTemplate():Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Facility/DownloadFacilityImportTemplate", {});
    }

    downloadVersionedImportTemplate(instanceId: number):Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Facility/DownloadVersionedDataImportTemplate", {instanceId: instanceId});
    }


    importFacilityVersionData(formData):Promise<IImportResult>{ 
        return this.httpClientService.postAsync(ApiConstant.ImportFacilityVersionData, formData);
    }

    exportAll():Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Facility/ExportFacilities", {});
    }
    exportByInstance(instanceId: number):Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Facility/ExportVersionedFacilities/"+ instanceId, {});
    }
    exportAggregated(instanceIds: number[]):Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Facility/ExportAggFacilities/", {instanceIds: instanceIds});
    }
}