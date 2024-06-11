import { DatePipe, DecimalPipe } from '@angular/common';
import { Component, Input, Output, EventEmitter, OnChanges, SimpleChanges, DoCheck, KeyValueDiffer, KeyValueDiffers, OnInit } from '@angular/core';
import { Globals } from 'src/app/globals';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { IFrameworkDynamicColumn } from 'src/app/models/frameworks/framework-dynamic-column.model';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { BooleanPipe } from 'src/app/_pipe/boolean.pipe';


@Component({
    selector: 'cell-viewer',
    templateUrl: './fw-cell-viewer.component.html',
    styles: [

        `
        .cell-text {
            max-width: 150px;
            min-width: 50px;
            text-overflow: ellipsis!important;            
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;        
        }
        `
    ]
})
export class FwCellViewerComponent implements OnChanges, DoCheck, OnInit{
        
    @Input('cell') cell: IDynamicCell;
    @Input('value') value: any
    @Input('showAdd') showAdd: boolean;
    @Input('showEdit') showEdit: boolean;
    @Input('showDelete') showDelete: boolean;

    @Output() edit = new EventEmitter<any>();
    @Output() delete = new EventEmitter<any>();
    @Output() add = new EventEmitter<any>();

    public displayValue: string = "";

    private differ: any;
    constructor(
        private datePipe: DatePipe, 
        private decimalPipe: DecimalPipe, 
        private booleanPipe: BooleanPipe,
        private global: Globals,
        private differs: KeyValueDiffers
        ){
        
    }
    ngOnInit(): void {
        this.differ = this.differs.find({}).create();        
    }
    ngDoCheck(): void {        
        
        let changes = this.differ.diff(this.cell);

        if(changes){            
            this.generateDisplayValue(this.cell);
        }
    }
    ngOnChanges(changes: SimpleChanges): void {
        
        if(changes.cell && changes.cell.isFirstChange() && changes.cell.currentValue){
            this.generateDisplayValue(changes.cell.currentValue);
        }        
    }

    generateDisplayValue(cell: IDynamicCell){
        if(!cell.values)
            return;
            switch(cell.dataType){
                case ColumnDataType.Integer:
                    this.displayValue = cell.values[0];
                    break;
                case ColumnDataType.Decimal:
                    this.displayValue = this.decimalPipe.transform(cell.values[0], "1.2-2");
                    break;                
                case ColumnDataType.Text:
                    this.displayValue = cell.values[0];
                    break;
                case ColumnDataType.Datetime:
                    this.displayValue = this.datePipe.transform(cell.values[0], this.global.shortDateFormat);
                    break;                
                case ColumnDataType.Boolean:
                    this.displayValue = this.booleanPipe.transform(cell.values[0]);
                    break;                
                case ColumnDataType.List:       
                    if(cell.values.length > 0 && cell.listType && cell.listType.listItems){

                        this.displayValue = cell.listType.listItems.filter(listItem => 
                            cell.values.map(x => parseInt(x)).includes(listItem.value))
                            .map(listItem => listItem.title)
                            .join(', ');                        
                    }
                    
                    break;                
            }
    }
    
    onEdit(event){        
        this.edit.emit(event);
    }

    onDelete(){
        this.delete.emit();
    }

    onAdd(event){
        this.add.emit(event);
    }
}