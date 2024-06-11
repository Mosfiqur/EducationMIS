import { IDynamicColumn } from '../models/cellEditorModels/IDynamicColumn';

export interface IColumnEditorResult {
    isDeleted: boolean;
    column?: IDynamicColumn;
}