import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { listItemViewModel } from './listItemViewModel';

export class indicatorGetViewModel{
    entityDynamicColumnId:number;
    isMultiValued:boolean;
    ColumnName:string;
    columnNameInBangla:string;
    columnDataType:ColumnDataType;
    listObjectId:number;
    listObjectName:string;
    listItem:listItemViewModel[];
    dataCollectionDate: string;
    values: string[];
}