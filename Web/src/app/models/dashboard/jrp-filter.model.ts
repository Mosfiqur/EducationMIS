import { AgeGroup } from 'src/app/_enums/ageGroup';
import { Gender } from 'src/app/_enums/gender';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';

export class JrpFilter {
    beneficiaryInstanceId?: string;
    facilityInstanceId?: string;
    programPartnerId?: string;
    implementationPartnerId?: string;
    campId?: string;
    gender?: string;
    disability?: string;
    targetedPopulationId?: string;
    age?: string;
    level?: string;

    constructor(){
        this.beneficiaryInstanceId = "";
        this.facilityInstanceId = "";
        this.programPartnerId = "";
        this.campId = "";
        this.gender = "";
        this.disability = "";
        this.targetedPopulationId = "";
        this.age = "";
        this.level = "";

    }

}