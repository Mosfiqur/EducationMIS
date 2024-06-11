import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { IShapeCoordinate } from './gap-map.model';

export interface ILcMapModel {
    minRadius?: number;
    maxRadius?: number;
    campCoordinates: IShapeCoordinate[];
    learningCenters: ILearningCenterModel[];   
}

export interface ILearningCenterModel {
    campId: number;
    campName: number;
    facilityId: number;
    facilityName: string;
    position: IShapeCoordinate;
    numberOfBeneficiaries: number;
    radius: number;
    targetedPopulation: TargetPopulation;
    ppName: string;
    ipName: string;
    upazilaName: string;
    unionName: string;    
    facilityCode: string;    
}