import { indicatorGetViewModel } from './indicatorGetViewModel';


export class facilityWiseIndicatorViewModel{
    facilityId:number;
    facilityName:string;
    instanceId:number;
    indicators:indicatorGetViewModel[]
}

export class BeneficiaryWiseIndicatorViewModel{
    facilityId:number;
    facilityName:string;
    instanceId:number;
    indicators:indicatorGetViewModel[]
}