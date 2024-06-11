import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { BeneficiaryFilter } from './beneficiaryFilterModel';

export class BeneficiaryApprovalGridQueryModel{
    collectionStatus?:CollectionStatus;
    instanceId:number;
    pageSize:number;
    pageNo:number;

    filter:BeneficiaryFilter;
}