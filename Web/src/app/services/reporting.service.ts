import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from '../models/beneficiary/beneficiaryViewModel';
import { BeneficiaryEditViewModel } from '../models/beneficiary/beneficiaryEditViewModel';
import { FacilityViewModel } from '../models/facility/facilityViewModel';
import { TeacherViewModel } from '../models/user/teacherViewModel';
import { FacilityEditViewModel } from '../models/facility/facilityEditViewModel';
import { IDuplicationReportQueryModel } from '../models/dashboard/duplication-report-query.model';

@Injectable({
    providedIn: 'root'
})
export class ReportingService {

    constructor(private httpClientService: HttpClientService) {

    }
    get5WReport(instanceId): Promise<any> {
        return this.httpClientService.download( `${ApiConstant.Generate5WReport}/${instanceId}`, {})
            
    }
    getCampWiseReport(instanceId): Promise<any> {
        return this.httpClientService.download( `${ApiConstant.GenerateCampWiseReport}/${instanceId}`, {})
            
    }
    getGapAnalysisReport(instanceId: number){
        return this.httpClientService.download(ApiConstant.baseUrl + "Report/GetGapAnalysisReport", {instanceid: instanceId});
    }
    getDuplicationReport(query: IDuplicationReportQueryModel){
        return this.httpClientService.download(ApiConstant.baseUrl + "Report/GetDuplicationReport", query);
    }
    getDamageReport(instanceId: number){
        return this.httpClientService.download(ApiConstant.baseUrl + "Report/GetDamageReport", {instanceId: instanceId});
    }

    getSummaryReport(instanceId: number){
        return this.httpClientService.download(ApiConstant.baseUrl + "Report/GetSummaryReport", {instanceId: instanceId});
    }
}