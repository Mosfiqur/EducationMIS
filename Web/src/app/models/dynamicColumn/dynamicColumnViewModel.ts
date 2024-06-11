import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { listObject } from '../common/listObjectViewModel';
import { listItem } from '../common/listItemViewModel';

export class DynamicColumnViewModel {
    "id": number;
    "name": string;
    "nameInBangla": string;
    "columnDataType": ColumnDataType;
    listObject:listObject;
    listItems:listItem[];
}
