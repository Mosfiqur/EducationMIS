import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from '../models/beneficiary/beneficiaryViewModel';
import { BeneficiaryEditViewModel } from '../models/beneficiary/beneficiaryEditViewModel';
import { Http, Headers, Response, RequestOptions } from '@angular/http';
import { IImportResult } from '../models/import/import-result.model';
import { BeneficiaryGridQueryModel } from '../models/beneficiary/beneficiaryGridQueryModel';

@Injectable({
    providedIn: 'root'
})

export class BeneficiaryService {
   
    constructor(private httpClientService: HttpClientService, private http: Http){

    }

    getAll(data): Promise<IPagedResponse<BeneficiaryViewModel>> {
        return this.httpClientService.postAsync(ApiConstant.GetAllBeneficiary,data)        
        .then(res => {              
            return res;
        });        
    }
    getById(id, instanceId): Promise<BeneficiaryEditViewModel> {
        return this.httpClientService.getAsync(ApiConstant.GetBeneficiaryById+"/"+id+"/"+instanceId)        
        .then(res => {             
            return res;
        });        
    }
    getAllByViewId(param:BeneficiaryGridQueryModel): Promise<IPagedResponse<BeneficiaryViewModel>> {
        
        return this.httpClientService.postAsync(ApiConstant.GetBeneficiaryByViewId,param)        
        .then(res => {           
             
            return res;
        });        
    }

    getAllByInstanceId(param:BeneficiaryGridQueryModel): Promise<IPagedResponse<BeneficiaryViewModel>> {
        return this.httpClientService.postAsync(ApiConstant.GetAllByInstanceId,param)        
        .then(res => {           
             
            return res;
        });        
    }
    
    beneficiaryCellSave(dynamicCell): Promise<any> {
        return this.httpClientService.postAsync(ApiConstant.SaveBeneficiaryCell, dynamicCell);
    }
    save(beneficiary):Promise<any>{
        return this.httpClientService.postAsync(ApiConstant.SaveBeneficiary, beneficiary);
    }
    update(beneficiary):Promise<any>{
        return this.httpClientService.postAsync(ApiConstant.UpdateBeneficiary, beneficiary);
    }
    delete(data):Promise<any>{
        return this.httpClientService.postAsync(ApiConstant.DeleteBeneficiary, data);
    } 
    active(data):Promise<any>{
        return this.httpClientService.postAsync(`${ApiConstant.ActiveBeneficiary}`, data);
    }
    deactive(data):Promise<any>{
        return this.httpClientService.postAsync(`${ApiConstant.DeactiveBeneficiary}`, data);
    }
    importBeneficiaries(formData):Promise<IImportResult>{ 
        return this.httpClientService.postAsync(ApiConstant.ImportBeneficiaries, formData);
    }
    importBeneficiariesVersionData(formData):Promise<IImportResult>{ 
        return this.httpClientService.postAsync(ApiConstant.ImportBeneficiariesVersionData, formData);
    } 
    downloadBeneficiaryImportTemplate():Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Beneficiary/DownloadBeneficiaryImportTemplate", {});
    }
    downloadVersionDataImportTemplate(instanceId: number):Promise<any> {
        return this.httpClientService.downloadAsync(ApiConstant.baseUrl + "Beneficiary/DownloadVersionDataImportTemplate/" + instanceId, {});
    }
    exportAllBeneficiaries():Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Beneficiary/ExportBeneficiaries", {});
    }
    exportByInstance(instanceId: number):Promise<any> {
        return this.httpClientService.download(ApiConstant.baseUrl + "Beneficiary/ExportVersionedBeneficiaries/"+ instanceId, {});
    }

    exportAggregated(instanceIds: number[]) {
        return this.httpClientService.download(ApiConstant.baseUrl + "Beneficiary/ExportAggBeneficiaries", {
          instanceIds: instanceIds
        })
      }
      
}