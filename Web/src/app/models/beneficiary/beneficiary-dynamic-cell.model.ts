import { IDynamicCell } from "../frameworks/dynamic-cell.model";

export interface IBeneficiaryDynamicCellInsertModel {
    beneficiaryId: number;
    instanceId: number;
    dynamicCells: IDynamicCell[];
}

export interface IBeneficiaryDynamicCell extends IDynamicCell {
    beneficiaryId: number;
}