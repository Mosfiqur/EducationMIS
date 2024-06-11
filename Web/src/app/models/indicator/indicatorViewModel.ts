import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { listObject } from '../common/listObjectViewModel';
import { listItem } from '../common/listItemViewModel';

export class IndicatorViewModel {
    id: number;
    columnOrder: number;
    entityDynamicColumnId: number;
    indicatorName: string;
    indicatorNameInBangla: string;
    columnDataType:ColumnDataType;

    listObject:listObject;
    listItems:listItem[];
}


