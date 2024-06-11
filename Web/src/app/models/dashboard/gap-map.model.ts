import { Month } from 'src/app/_enums/month';

export interface IGapMapModel {    
    campId: number;
    campName: string;
    startYear: number;
    startMonth: Month;
    endYear: number;
    endMonth: Month;
    peopleInNeed: number;
    target: number;
    outreach: number;
    gap: number;
    shapeCoordinates: IShapeCoordinate[]
}

export interface IShapeCoordinate {
    latitude: number;
    longitude: number;
    sequenceNo?: number;
}