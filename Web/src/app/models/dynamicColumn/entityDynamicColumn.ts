import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { ListItem } from './listItem';

export class EntityDynamicColumn {
    entityColumnId: number;
    properties: string;
    values: [];
    isVersionColumn: boolean;
    isFixed: boolean;
    isMultiValued: boolean;
    dataType: ColumnDataType;
    columnListId: number;
    columnListName: string;
    listItem: ListItem[];
    columnName?: string;
}
