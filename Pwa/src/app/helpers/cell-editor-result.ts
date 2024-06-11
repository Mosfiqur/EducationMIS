import { IDynamicCell } from '../models/cellEditorModels/IDynamicCell';

export interface ICellEditorResult {
    isDeleted: boolean;
    cell?: IDynamicCell;
    isCanceled?: boolean;
    isClosed?: boolean;
}