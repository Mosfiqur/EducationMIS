import { Gender } from 'src/app/_enums/gender';
import { Month } from 'src/app/_enums/month';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { ITargetFrameworkDynamicCell } from './target-framework-dynamic-cell.model';


export interface ITargetFramework {
    id?: number;
    campId?: number;
    campName:string;
    gender: Gender;
    ageGroupId: number;
    ageGroupName:string;
    peopleInNeed: number;
    target: number;
    startYear: number;
    startMonth: Month;
    endYear: number;
    endMonth: Month;    
    dynamicCells?: ITargetFrameworkDynamicCell[];
    isSelected?: boolean;
    targetedPopulation: TargetPopulation;
    upazilaId: number;
    unionId: number;    

}
