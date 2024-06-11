import { IMonitoringFrameworkDynamicCell } from './dynamic-cell.model';


export interface IObjectiveIndicator {
    id?: number;
    indicator: string;
    unit: string;
    baseLine: number;
    target: number;
    startDate: Date;
    endDate: Date;
    organizationId: number;
    organizationName: string;
    reportingFrequencyId: number;
    frequencyName: string;
    monitoringFrameworkId: number;
    dynamicCells: IMonitoringFrameworkDynamicCell[];
}
