import { EntityType } from 'src/app/_enums/entityType';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { ListItem } from './ListItem';
import { IListDataType } from './IDynamicCell';

export interface IDynamicColumn {  
    entityDynamicColumnId:number;   
    //entityColumnId: number;
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
    values:string[];
}