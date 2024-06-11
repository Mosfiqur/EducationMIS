import { IDynamicCell } from './dynamic-cell.model';

export interface ITargetFrameworkInsertModel {
    targetFrameworkId: number;
    dynamicCells: IDynamicCell[];
}

export interface ITargetFrameworkDynamicCell extends IDynamicCell {
    targetFrameworkId: number;
}



export class TargetFrameworkDynamicCell implements ITargetFrameworkDynamicCell{
    
    targetFrameworkId: number;    
    id?: number;
    entityDynamicColumnId: number;
    value: string[];    
    columnName: string;
    isSelected?: boolean;
    constructor(
            targetFrameworkId:number, 
            frameworkDynamicColumnId: number,             
            value?: string[], 
            columnName?: string,
            id?:number
            )
    {
        this.targetFrameworkId = targetFrameworkId;
        this.entityDynamicColumnId = frameworkDynamicColumnId;
        this.value = value;
        this.columnName = columnName;        
        this.id = id
        
        this.isSelected = false;
    }

    static  empty(): TargetFrameworkDynamicCell{
        return {
            targetFrameworkId: 0,
            entityDynamicColumnId: 0,
            value: [],
            columnName: "",
            id: 0
          }
    }

    

}