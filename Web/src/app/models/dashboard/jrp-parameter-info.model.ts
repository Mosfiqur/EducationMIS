import { Formula } from 'src/app/_enums/formula';

export interface IJrpParameterInfo {
    id: number;
    name: string;
    
    targetId: number;
    targetName:string;
    indicatorId: number;
    indicatorName:string;
}