import { ApiConstant } from '../ApiConstant';
import { Injectable } from '@angular/core';


@Injectable({providedIn: 'root'})
export class SchedulingEndpoints {

    private base = ApiConstant.baseUrl + "Schedule/";

    // createSchedule: string  = this.base + "CreateSchedule";
    createBeneficiarySchedule: string  = this.base + "CreateBeneficiarySchedule";
    createFacilitySchedule: string  = this.base + "CreateFacilitySchedule";

    // updateSchedule: string  = this.base+ "UpdateSchedule";
    updateBeneficiarySchedule: string  = this.base+ "UpdateBeneficiarySchedule";
    updateFacilitySchedule: string  = this.base+ "UpdateFacilitySchedule";

    // deleteSchedule: string  = this.base + "DeleteSchedule";
    deleteBeneficiarySchedule: string  = this.base + "DeleteBeneficiarySchedule";
    deleteFacilitySchedule: string  = this.base + "DeleteFacilitySchedule";

    //startCollection: string  = this.base + "StartCollection";    
    startBeneficiaryCollection: string  = this.base + "StartBeneficiaryCollection";    
    startFacilityCollection: string  = this.base + "StartFacilityCollection";    

    // getCurrentSchedule: string  = this.base + "GetCurrentSchedule";
    getBeneficiaryCurrentSchedule: string  = this.base + "GetBeneficiaryCurrentSchedule";
    getFacilityCurrentSchedule: string  = this.base + "GetFacilityCurrentSchedule";

    //getSchedules: string  = this.base + "GetSchedules";
    getBeneficiarySchedules: string  = this.base + "GetBeneficiarySchedules";
    getFacilitySchedules: string  = this.base + "GetFacilitySchedules";

    // getCompletedInstances: string  = this.base + "GetCompletedInstances";   
    GetBeneficiaryCompletedInstances: string  = this.base + "GetBeneficiaryCompletedInstances";   
    GetFacilityCompletedInstances: string  = this.base + "GetFacilityCompletedInstances"; 

    // getCurrentScheduleInstances: string  =this.base + "GetCurrentScheduleInstances";   
    getBeneficiaryCurrentScheduleInstances: string  =this.base + "GetBeneficiaryCurrentScheduleInstances";   
    getFacilityCurrentScheduleInstances: string  =this.base + "GetFacilityCurrentScheduleInstances";   

     //completeInstance: string  = this.base + "CompleteInstance";    
    CompleteBeneficiaryInstance: string  = this.base + "CompleteBeneficiaryInstance";    
    CompleteFacilityInstance: string  = this.base + "CompleteFacilityInstance";    
}