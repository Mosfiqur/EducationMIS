import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { ListItem } from './ListItem';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';


export class EntityDynamicColumn {
    entityColumnId: number;
    properties: string;
    columnNameInBangla:string;
    values: [];
    isVersionColumn: boolean;
    isFixed: boolean;
    isMultiValued: boolean;
    dataType: ColumnDataType;
    status:CollectionStatus;
    columnListId: number;
    columnListName: string;
    listItem: ListItem[];
}
