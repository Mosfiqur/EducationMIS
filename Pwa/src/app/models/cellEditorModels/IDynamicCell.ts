import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { ListItem } from './ListItem';

export interface IDynamicCell{
    id?: number;    
    entityDynamicColumnId: number;
    dataType?: ColumnDataType;
    columnName?: string;
    value: string[];
    values?: string[];
    listType?: IListDataType;
}

export interface IListDataType {
    id?: number;
    name: string;
    listItems: ListItem[];    
}

