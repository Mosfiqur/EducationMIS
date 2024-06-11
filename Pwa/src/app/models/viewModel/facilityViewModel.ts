import { CollectionStatus } from 'src/app/_enums/collectionStatus';

export class FacilityViewModel {
    "id": number;
    "name": string;
    "facilityCode": string;
    "programPartnerId": number;
    "programPartnerName": string;
    "implementationPartnerId": number;
    "implementationPartnerName": string;
    "campId": number;
    "campName":string;
    "campSSID":string;
    "blockId":number;
    "blockName":string;
    "collectionStatus":CollectionStatus;
}