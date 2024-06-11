import { ColumnDataType } from 'src/app/_enums/column-data-type'
import { EntityDynamicColumn } from './entityDynamicColumn';
import { ListItem } from './listItem';
import { GridItemDef } from './gridItemDef';
import { TemplateRef } from '@angular/core';

export class NgxGridColumnDef {
    prop: string;
    name: string;
    columnId: number;
    // cellRenderer: string;
    // cellEditor: string;
    // editable: boolean;

    cellEditorParams: ListItem[];
    // suppressToolPanel: boolean;
    // hide: boolean;
    // checkboxSelection: boolean;
    // headerCheckboxSelection: boolean;
    // headerCheckboxSelectionFilteredOnly: boolean;

    // headerCheckboxable: boolean;
    // checkboxable: boolean;

    cellTemplate:any;
    headerTempalte:any;

    constructor(properties: EntityDynamicColumn) {
        this.prop = "field" + properties.entityColumnId.toString();
        this.name = properties.properties;
        this.columnId = properties.entityColumnId;
        
        // if(cellTemplate!=null){
        //     this.cellTemplate=cellTemplate;
        // }
        // if(headerTempalte!=null){
        //     this.headerTempalte=headerTempalte;
        // }
        
        if (properties.columnListId != null && properties.dataType == ColumnDataType.List) {
            this.cellEditorParams = properties.listItem
        }
    }

}