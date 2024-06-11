import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Globals } from 'src/app/globals';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { ModalService } from 'src/app/services/modal.service';
import { EntityType } from 'src/app/_enums/entityType';
import { DynamicColumnComponent } from '../dynamic-column/dynamic-column.component';

@Component({
    selector: 'add-dynamic-column',
    templateUrl: './add-dynamic-column.component.html'
})
export class AddDynamicColumnComponent {
    public dynamicColumns: IDynamicColumn[] = [];
    @Input('entityType') entityType: EntityType;
    @Output('onColumnAdded') columnAddedEvent: EventEmitter<IDynamicColumn> = new EventEmitter();    
    constructor(
            protected dynamicColumnService: DynamicColumnService,
            protected globals: Globals,
            protected modalService: ModalService
            ){
                
    }

    public onAddNewDynamicColumn(){        
        this.modalService.open<DynamicColumnComponent, IDynamicColumn>(DynamicColumnComponent, { entityTypeId: this.entityType })
        .then(async column => {                  
           if(column){               
            this.dynamicColumns.push(column);                        
            this.columnAddedEvent.emit(column)
           }
        });
    }; 
}