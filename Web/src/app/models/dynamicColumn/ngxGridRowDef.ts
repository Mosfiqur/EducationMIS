import { EntityDynamicColumn } from './entityDynamicColumn';
import { ColumnDataType } from '../../../app/_enums/column-data-type';

export class NgxGridRowDef {
    row = {};
    constructor( properties: EntityDynamicColumn[]) {
        //this.row['id']=entityId;
        properties.forEach(a => {
            var fieldName = "field" + a.entityColumnId.toString();

            if (a.dataType == ColumnDataType.List) {

                this.row[fieldName] = a.values.filter(v => v != null);

                // if (a.values.filter(v=>v!=null) != null) {
                //     this.row[fieldName] = a.values;
                // }
                // else {
                //     this.row[fieldName] = null;
                // }
            }
            else {
                this.row[fieldName] = a.values.filter(v => v != null).toString();
                // if (a.values != null) {
                //     this.row[fieldName] = a.values.toString();
                // }
                // else {
                //     this.row[fieldName] = "";
                // }
            }

        })

    }
}