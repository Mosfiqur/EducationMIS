import { IDynamicCell } from "../frameworks/dynamic-cell.model";

export interface IFacilityDynamicCellAddModel {
    instanceId: number;
    facilityId: number;
    dynamicCells: IDynamicCell[];
}