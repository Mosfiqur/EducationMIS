import { BeneficiaryFilter } from './beneficiaryFilterModel';

export class BeneficiaryGridQueryModel{
    viewId:number;
    instanceId:number;
    pageSize:number;
    pageNo:number;
    collectionStatus:number;
    
    filter:BeneficiaryFilter;
}