import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { EntityDynamicColumn } from '../indicator/EntityDynamicColumn';

export class BeneficiaryFacilityViewModel{

    id:number;
    facilityCode:string;
    facilityName: string;
    campId: number;
    campName: string;
    blockId: number;
    blockName: string;
    programmingPartnerId: number;
    programmingPartnerName: string;
    implemantationPartnerId: number;
    implemantationPartnerName: string;
    collectionStatus: CollectionStatus;
    unionId:number;
    unionName:string;
    upazilaId:number;
    upazilaName:string;
    facilityType:number;
    facilityStatus:number;
    targetedPopulation:number;
    paraId:string;
    paraName:string;
    teacherId:number;
    teacherName:string;
    donors:string;
    nonEducationPartner:string;
    latitude:number;
    longitude:number;
    campSSID:string;
    remarks:string;

    properties:EntityDynamicColumn[];
}