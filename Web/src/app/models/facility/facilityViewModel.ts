import { FacilityType } from 'src/app/_enums/facilityType';
import { EntityDynamicColumn } from '../dynamicColumn/entityDynamicColumn';
import { FacilityStatus } from 'src/app/_enums/facilityStatus';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';

export class FacilityViewModel {
    "id": number;
    "facilityName": string;
    "facilityCode": string;
    "targetedPopulation": TargetPopulation;
    "facilityStatus": FacilityStatus;
    "latitude": string;
    "longitude": string;
    "donors": string;
    "nonEducationPartner": string;
    "programPartnerId": number;
    "programmingPartnerName": string;
    "implementationPartnerId": number;
    "implemantationPartnerName": string;
    "campId": number;
    "campName":string;
    "paraName": string;
    "paraId": string;
    "blockId": number;
    "blockName": string;
    "facilityType": FacilityType;
    "teacherId": number;
    "teacherName":string;
    "collectionStatus":number;
    // "noOfLfEstablishedInRohingyaCommunity": number;
    // "noOfClassroomsInLfInRohingyaCommunity": number;
    // "noOfShiftsInLfInRohingyaCommunity": number;
    // "noOfFunctionalCommunityLatrinesNearLF": number;
    // "noOfLatrinesEstablishedForBoys": number;
    // "noOfLatrinesEstablishedForGirls": number;
    // "iDofTheLatrinesAccessibleToTheLC": string;
    // "noOfHandwashingStationEstablished": number;
    // "noOfFemaleRohingyaLearningFacilitatorsTrained": number;
    // "noOfMaleRohingyaLearningFacilitatorsTrained": number;
    // "noOfFemaleHcLearningFacilitatorsTrained": number;
    // "noOfMaleHcLearningFacilitatorsTrained": number;
    // "noOfDrrAwarenessSessionsConductedInHc": number;
    // "noOfRohingyaLearningFacilityEducationCommitteesEstablishedInRohingyaCamps": number;
    // "noOfFemaleRohingyaCaregiversSensitizedOnCYRPAP": number;
    // "noOfMaleRohingyaCaregiversSensitizedOnCYRPAP": number;
    // "noOfFemaleHcCaregiversSensitizedOnCYRPAP": number;
    // "noOfMaleHcCaregiversSensitizedOnCYRPAP": number;
    // "noOfRohingyaGirlsAged3_24YearsOldEngagedInSCI": number;
    // "noOfRohingyaBoysAged3_24YearsOldEngagedInSCI": number;
    // "noOfChildrenBenefittingFromFood": number;
    // "noOfChildrenBenefittingFromSchool_Classroom_ToiletRehabilitation": number;
    // "otherRapidIntervention": string;
    "remarks": string;

    properties:EntityDynamicColumn[];
}