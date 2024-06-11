import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { Globals } from 'src/app/globals';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { ModalService } from 'src/app/services/modal.service';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { EntityType } from 'src/app/_enums/entityType';


export abstract class FrameworkBaseComponent {
    public dynamicColumns: IDynamicColumn[] = [];

    constructor(
        protected dynamicColumnService: DynamicColumnService,
        protected globals: Globals,
        protected modalService: ModalService
    ) {

    }
    async loadDynamicColumns(entityType: EntityType) {
        return this.dynamicColumnService.getColumns({
            entityType: entityType,
            pageNo: 1,
            pageSize: this.globals.maxPageSize
        })
            .then(response => {
                this.dynamicColumns = response.data.map(x => ({ ...x, columnName: x.name, columnNameInBangla: x.nameInBangla }));
            });
    }

    abstract pushEmptyCell(column: IDynamicColumn);

    protected async addNewDynamicColumn(entityType: EntityType) {
        return this.modalService.open<DynamicColumnComponent, IDynamicColumn>(DynamicColumnComponent, { entityTypeId: entityType })
            .then(async column => {
                if (column) {
                    this.dynamicColumns.push(column);
                    this.pushEmptyCell(column);
                    this.loadDynamicColumns(entityType);
                }
            });
    };
}