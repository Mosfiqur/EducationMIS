import { Injectable } from '@angular/core';
import { ApiConstant } from '../ApiConstant';

@Injectable({providedIn: 'root'})
export class CommonEndpoints {
    private base = ApiConstant.baseUrl + "Common/";
    getCamps: string  = this.base + "GetCamps";
    getReportingFrequencies: string  = this.base + "GetReportingFrequencies";
    getAgeGroups: string = this.base + "GetAgeGroups";
}