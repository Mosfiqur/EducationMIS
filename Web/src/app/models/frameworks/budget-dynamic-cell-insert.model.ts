import { IDynamicCell } from './dynamic-cell.model';


export interface IBudgetDynamicCellInsertModel {
    budgetFrameworkId: number;
    dynamicCells: IDynamicCell[];
}
