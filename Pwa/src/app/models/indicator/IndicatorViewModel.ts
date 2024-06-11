import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { listObject, listItem } from './listObject';


export class IndicatorViewModel {
    id: number;
    columnOrder: number;
    entityDynamicColumnId: number;
    indicatorName: string;
    indicatorNameInBangla: string;
    columnDataType:ColumnDataType;
    isMultivalued:boolean;
    listObject:listObject;
    listItems:listItem[];
}


