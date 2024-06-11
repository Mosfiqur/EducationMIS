import { ColumnDataType } from 'src/app/_enums/column-data-type'
import { EntityDynamicColumn } from './entityDynamicColumn';
import { ListItem } from './listItem';
import { GridItemDef } from './gridItemDef';

export class NonEditableGridColumnDef {
    field: string;
    headerName: string;
    columnId: number;
    cellRenderer: string;
    cellEditor: string;
    editable: boolean;
    cellEditorParams: GridItemDef = new GridItemDef();
    suppressToolPanel: boolean;
    hide: boolean;
    checkboxSelection: boolean;
    headerCheckboxSelection: boolean;
    headerCheckboxSelectionFilteredOnly: boolean;

    constructor(properties: EntityDynamicColumn) {
        this.field = "field" + properties.entityColumnId.toString();
        this.headerName = properties.properties;
        this.columnId = properties.entityColumnId;
        if (properties.dataType == ColumnDataType.Text) {
            this.cellEditor = "agLargeTextCellEditor";
        }
        else if (properties.dataType == ColumnDataType.List && properties.isMultiValued) {
            this.cellEditor = "listEditor";
            this.cellRenderer = "listRenderer";
        }
        else if (properties.dataType == ColumnDataType.List && !properties.isMultiValued) {
            this.cellEditor = "listRadioButtonEditor";
            this.cellRenderer = "listRadioButtonRenderer";
        }
        //this.editable = true;
        if (properties.columnListId != null && properties.dataType == ColumnDataType.List) {
            this.cellEditorParams.values = properties.listItem
        }
    }

}