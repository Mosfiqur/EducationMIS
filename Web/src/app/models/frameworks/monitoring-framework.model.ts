

import { IMonitoringFrameworkDynamicCell } from './dynamic-cell.model';
import { IObjectiveIndicator } from './objective-indicator.model';

export interface IMonitoringFramework{
    id?: number;
    objective: string;
    objectiveIndicators?: IObjectiveIndicator[]    
}

export interface IMonitoringFrameworkUpdateModel extends IMonitoringFramework {
    unit: string;
    baseLinve: string;
    target: string;
    dynamicCells: IMonitoringFrameworkDynamicCell[]
}