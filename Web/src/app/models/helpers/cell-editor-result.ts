import { IDynamicCell, IBudgetFrameworkDynamicCell, IMonitoringFrameworkDynamicCell } from '../frameworks/dynamic-cell.model';

export interface ICellEditorResult {
    isDeleted: boolean;
    cell?: IDynamicCell | IBudgetFrameworkDynamicCell | IMonitoringFrameworkDynamicCell
}