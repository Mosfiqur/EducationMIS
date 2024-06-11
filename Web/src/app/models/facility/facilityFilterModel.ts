import { FacilityStatus } from 'src/app/_enums/facilityStatus'
import { FacilityType } from 'src/app/_enums/facilityType'
import { TargetPopulation } from 'src/app/_enums/targetedPopulation'
import { EducationSectorPartner } from '../educationSectorPartner/educationSectorPartner'
import { TeacherViewModel } from '../user/teacherViewModel';

export class FacilityFilter {
    unionId: number;
    unionName:string;

    upazilaId: number;
    upazilaName: string;

    targetedPopulation: TargetPopulation;
    facilityType: FacilityType;
    facilityStatus: FacilityStatus;
    programPartner: EducationSectorPartner[];
    implementationPartner: EducationSectorPartner[];
    teachers:TeacherViewModel[];
    searchText:string;
}