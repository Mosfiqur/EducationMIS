import { ColumnDataType } from '../_enums/column-data-type';
import { ISelectListItem } from '../helpers/select-list.model';


export class Convert {
    static enumToSelectList(val: any) : ISelectListItem[]{
        return Object.keys(val)
        .filter(x => parseInt(x))
        .map(x => {
            return {
                id: parseInt(x),
                text: val[x]
            }
        });
    }

    static toEnum(item: ISelectListItem, anEnum: any){
        return anEnum[item.text];
    }

    static toInputTypeName(dataType: ColumnDataType): string {
        switch (dataType){
            case ColumnDataType.Text: 
                return "text";
            case ColumnDataType.Int:
                return "number";
            case ColumnDataType.Decimal:
                return "decimal"
        }
    }
}