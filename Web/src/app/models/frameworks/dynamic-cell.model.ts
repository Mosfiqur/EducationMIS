import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { IListDataType } from '../dynamicColumn/list-datatype';

export interface IDynamicCell{
    id?: number;    
    entityDynamicColumnId: number;
    dataType?: ColumnDataType;
    columnName?: string;
    value: string[];
    values?: string[];
    listType?: IListDataType;
}





export interface IBudgetFrameworkDynamicCell extends IDynamicCell{
    budgetFrameworkId: number;
}

export class BudgetFrameworkDynamicCell implements IBudgetFrameworkDynamicCell {

    budgetFrameworkId: number;
    id?: number;
    entityDynamicColumnId: number;
    value: string[];
    values: string[];
    columnName: string;
    isSelected?: boolean;
    constructor(
        budgetFrameworkId: number,
        frameworkDynamicColumnId: number,
        value?: string[],
        columnName?: string,
        id?: number
    ) {
        this.budgetFrameworkId = budgetFrameworkId;
        this.entityDynamicColumnId = frameworkDynamicColumnId;
        this.value = value;
        this.values = value;
        this.columnName = columnName;
        this.id = id;
        this.isSelected = false;
    }

    static empty(): IBudgetFrameworkDynamicCell {
        return {
            budgetFrameworkId: 0,
            entityDynamicColumnId: 0,
            value: [],
            columnName: "",
            id: 0
        };
    }



}

export interface IMonitoringDynamicCellInsertModel {
    objectiveIndicatorId: number;
    dynamicCells: IDynamicCell[]
}

export interface IMonitoringFrameworkDynamicCell extends IDynamicCell{
    objectiveIndicatorId: number;
}



export class MonitoringFrameworkDynamicCell implements IMonitoringFrameworkDynamicCell {
    objectiveIndicatorId: number;
    id?: number;
    entityDynamicColumnId: number;
    columnName?: string;
    value: string[];

    constructor( objectiveIndicatorId: number,
        frameworkDynamicColumnId: number,
        value?: string[],
        columnName?: string,
        id?: number){
        this.id = id;
        this.objectiveIndicatorId= objectiveIndicatorId;
        this.columnName = columnName;
        this.value = value;
        this.entityDynamicColumnId = frameworkDynamicColumnId;
    }
}
