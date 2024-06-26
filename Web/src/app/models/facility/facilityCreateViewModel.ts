import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { FacilityType } from 'src/app/_enums/facilityType';
import { FacilityStatus } from 'src/app/_enums/facilityStatus';

export class FacilityCreateViewModel {
    "id": number;
    "name": string;
    "facilityCode": string;
    "targetedPopulation": TargetPopulation;
    "facilityStatus": FacilityStatus;
    "latitude": string;
    "longitude": string;
    "donors": string;
    "nonEducationPartner": string;
    "programPartnerId": number;
    "programPartnerName": string;
    "implementationPartnerId": number;
    "implementationPartnerName": string;
    "campId": number;
    "paraName": string;
    "paraId": string;
    "blockId": number;
    "facilityType": FacilityType;
    "teacherId": number;

    // "noOfLfEstablishedInRohingyaCommunity": number;
    // "noOfClassroomsInLfInRohingyaCommunity": number;
    // "noOfShiftsInLfInRohingyaCommunity": number;
    // "noOfFunctionalCommunityLatrinesNearLF": number;
    // "noOfLatrinesEstablishedForBoys": number;
    // "noOfLatrinesEstablishedEorGirls": number;
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
}