import { Column } from "ag-grid-community";
import { ListItem } from './listItem';
import { EntityType } from 'src/app/_enums/entityType';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { IListDataType } from './list-datatype';


export interface IDynamicColumn {     
    entityColumnId: number;
    entityTypeId: EntityType;
    columnName: string;
    columnNameInBangla: string;    
    columnDataType: ColumnDataType;
    id?: number;
    columnListId?: number;
    listItems?: ListItem[];
    isMultiValued?: false;
    description?: string;
    name?: string;    
    nameInBangla?: string;    
    listObject?: IListDataType;
}

export class DynamicColumnSaveViewModel implements IDynamicColumn{
    entityColumnId: number;
    entityTypeId: EntityType;
    columnName: string;
    columnNameInBangla: string;
    columnDataType: ColumnDataType;
    id?: number;
    columnListId?: number;
    listItems?: ListItem[];
    isMultiValued?: false;
    description?: string;
    targetedPopulation:TargetPopulation;
    
}