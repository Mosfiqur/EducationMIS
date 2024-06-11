import { Component, Output, EventEmitter, Input } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { IDynamicCell } from 'src/app/models/cellEditorModels/IDynamicCell';
import { IDynamicColumn } from 'src/app/models/cellEditorModels/IDynamicColumn';
import { ISelectListItem } from 'src/app/helpers/select-list.model';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { Convert } from 'src/app/utility/Convert';
import { ListItem } from 'src/app/models/cellEditorModels/ListItem';
import { DateFactory } from 'src/app/utility/DateFactory';

@Component({
    templateUrl: './fw-cell-editor.component.html',
    styles: [

        `
        .check {
            height: 25px;
    width: 25px;
    float: left;
    margin-right: 15px;
        }
        `
    ]
})
export class FwCellEditorComponent {
    
    public cell: IDynamicCell
    public column: IDynamicColumn
    public dataType: any;

    public cellForm: FormGroup;
    public inputType?: string;
    public selectDataSource?: ISelectListItem[]
    
    constructor(
        private fb: FormBuilder,
        private activeModal: NgbActiveModal        
    ){
        this.dataType = ColumnDataType;
    }

    

    public listItems: ListItem[] = [];

    public isAllSelected: boolean = false;

    ngOnInit(): void {     
        // build form according to 
        this.inputType = Convert.toInputTypeName(this.column.columnDataType);

        this.buildForm();
    }

    onModelChange(isSelected: boolean, model: ListItem){
        //console.log(model, isSelected);     
        if(!this.column.isMultiValued){
            this.listItems.forEach(x => x.isSelected = false);    
        }
        model.isSelected = isSelected;        
    }
    buildForm(){
        if(this.column.columnDataType == ColumnDataType.Int 
            || this.column.columnDataType == ColumnDataType.Text
            || this.column.columnDataType == ColumnDataType.Decimal            
            || this.column.columnDataType == ColumnDataType.Datetime 
            || this.column.columnDataType == ColumnDataType.Boolean           
            ){
            this.cellForm = this.fb.group({
                value: ["", [Validators.required]]            
            });

            if(this.cell && this.cell.values){
                this.cellForm.patchValue({
                    value: this.parseValueForEdit()
                });
            }
        }

        if(this.column.columnDataType == ColumnDataType.List){
            
            this.listItems = 
            this.column.listItems
            .map(item => {
                if(!this.cell.values)
                {
                    item.isSelected = false;
                }else{
                    item.isSelected = this.cell.values
                    .map(x => parseInt(x))
                    .includes(parseInt(item.value.toString()));
                }                
                return item;
            });
        }        
    }

    buildListItem(aListItem?: ListItem): FormGroup{
        return this.fb.group({
            id: [aListItem ? aListItem.id : 0, []],
            title: [aListItem ? aListItem.title : "", [Validators.required]],
            value: [aListItem ? aListItem.value : "", [Validators.required]]
        });
    }

    parseValueForEdit() : any{
        switch(this.column.columnDataType){
            case ColumnDataType.Int:
                return parseInt(this.cell.values[0]);                
            case ColumnDataType.Decimal:
                return parseFloat(this.cell.values[0]);                
            case ColumnDataType.Text:
                return this.cell.values[0];
            case ColumnDataType.Datetime:
                return DateFactory.toNgbDateStruct(this.cell.values[0]);
            case ColumnDataType.Boolean:
                return this.cell.values[0]
            default: 
                throw Error("Invalid data type");
        }
    }

    parseValueForSave(value: any){
        switch(this.column.columnDataType){
            case ColumnDataType.Int:
                return parseInt(value);                
            case ColumnDataType.Decimal:
                return parseFloat(value);                
            case ColumnDataType.Text:
                return value
            case ColumnDataType.Datetime:
                return DateFactory.createFromNgbDateStruct(value);
            case ColumnDataType.Boolean:
                return value;
            default: 
                throw Error("Invalid data type");
        }
    }



    handleCancel(){                    
        this.activeModal.close({isCanceled: true});        
    }

    handleDelete(){
        this.activeModal.close({isDeleted: true});
    }

    onSubmit(){        
        if(!this.cellForm.valid){            
            return;
        }            
        
        let val = this.cellForm.get('value').value;
        let cell = {
            value: [this.parseValueForSave(val)]
        };

        this.activeModal.close({cell: cell});
    }

    saveList(){
        let cell= {
            value: this.listItems.filter(x => x.isSelected).map(x => x.value)
        };

        this.activeModal.close({cell: cell})
    }
}