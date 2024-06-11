import { IDonor } from './donor.model';
import { IBudgetFrameworkDynamicCell } from './dynamic-cell.model';
import { IProject } from './project.model';



export interface IBudgetFramework {    
    id?: number;
    project?: IProject;
    donor?: IDonor;
    donorId?: number;
    projectId?: number;
    donorName?: string;
    projectName?: string;
    amount: number;
    startDate: Date;
    endDate: Date;
    isSelected?: boolean;
    dynamicCells?: IBudgetFrameworkDynamicCell[];
}