import { IDynamicCell } from './dynamic-cell.model';

export interface IUserDynamicCellInsertModel {
    userId: number;
    dynamicCells: IDynamicCell[];
}

export interface IUserDynamicCell extends IDynamicCell {
    userId: number;
}
