
import { Gender } from 'src/app/_enums/gender';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { Camp } from '../common/camp.model';
import { FacilityViewModel } from '../facility/facilityViewModel';

export class BeneficiaryFilter {
    dateOfBirth:DateRange;
    enrolmentDate:DateRange;
    facilities:FacilityViewModel[];
    camps:Camp[];
    sex:Gender;
    levelOfStudy:LevelOfStudy;
    disable:boolean;
    searchText:string;
}

export class DateRange {
    startDate:string;
    endDate: string
}
