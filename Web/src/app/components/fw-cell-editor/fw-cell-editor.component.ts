import { Component, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

import { ModalService } from 'src/app/services/modal.service';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { Convert } from 'src/app/utility/Convert';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { DateFactory } from 'src/app/utility/DateFactory';
import { ListItem } from 'src/app/models/dynamicColumn/listItem';
import { Globals } from 'src/app/globals';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { Subscription } from 'rxjs';

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
export class FwCellEditorComponent implements OnDestroy{
    public validationErrors = {};
    public cell: IDynamicCell
    public column: IDynamicColumn
    public dataType: any;

    public cellForm: FormGroup;
    public inputType?: string;
    public selectDataSource?: ISelectListItem[]
    private subscriptions: Map<string, Subscription> = new Map();
    constructor(
        private fb: FormBuilder,
        private activeModal: NgbActiveModal,
        private globals: Globals
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
        console.log(model, isSelected);     
        if(!this.column.isMultiValued){
            this.listItems.forEach(x => x.isSelected = false);    
        }
        model.isSelected = isSelected;        
    }
    buildForm(){

        let msg: {[controlName: string] : {[error: string]: string}} = {
            value: {}
        };
        this.cellForm = this.fb.group({
            value: ["", [Validators.required]]                
        });
        if(this.column.columnDataType == ColumnDataType.Integer 
            || this.column.columnDataType == ColumnDataType.Text
            || this.column.columnDataType == ColumnDataType.Decimal            
            || this.column.columnDataType == ColumnDataType.Datetime 
            || this.column.columnDataType == ColumnDataType.Boolean           
            ){
            

            if(this.cell && this.cell.values){
                this.cellForm.patchValue({
                    value: this.parseValueForEdit()
                });
            }else{
                this.cellForm.patchValue({
                    value: this.getDefaultValue()
                });
            }
            
        }
        if(this.column.columnDataType == ColumnDataType.List && this.column.listItems){            
            if(this.column && this.column.listItems && this.column.listItems.length > 0){
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
        
        let c = this.cellForm.get('value');

        if(this.column.columnDataType == ColumnDataType.Integer){            
            c.setValidators([Validators.max(this.globals.maxInt), Validators.min(this.globals.minInt)]);            
            msg.value.max = ValidationMessage.max(this.globals.maxInt);
            msg.value.min = ValidationMessage.min(this.globals.minInt);            
        }

        if(this.column.columnDataType == ColumnDataType.Decimal){
            c.setValidators([Validators.max(this.globals.maxDecimal), Validators.min(this.globals.minDecimal)]);            
            msg.value.max = ValidationMessage.max(this.globals.maxDecimal);
            msg.value.min = ValidationMessage.min(this.globals.minDecimal);
        }

     
        c.updateValueAndValidity();

        this.subscriptions = new ValidationErrorBuilder()
        .withGroup(this.cellForm)
        .useMessageContainer(this.validationErrors)      
        .withMessages(msg)          
        .build();
        
    }

    buildListItem(aListItem?: ListItem): FormGroup{
        return this.fb.group({
            id: [aListItem ? aListItem.id : 0, []],
            title: [aListItem ? aListItem.title : "", [Validators.required]],
            value: [aListItem ? aListItem.value : "", [Validators.required]]
        });
    }

    getDefaultValue(): any{
        switch(this.column.columnDataType){
            case ColumnDataType.Integer:
                return 0;
            case ColumnDataType.Decimal:
                return 0.0;
            default: 
                return "";
        }
    }
    parseValueForEdit() : any{
       // var a=isNaN(parseInt(this.cell.values[0]))?0:parseInt(this.cell.values[0]);
        switch(this.column.columnDataType){
            case ColumnDataType.Integer:
                return isNaN(parseInt(this.cell.values[0]))?parseInt('0'):parseInt(this.cell.values[0]);               
            case ColumnDataType.Decimal:
                return isNaN(parseFloat(this.cell.values[0]))?parseFloat('0'):parseFloat(this.cell.values[0]);
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
            case ColumnDataType.Integer:
                return parseInt(value);                
            case ColumnDataType.Decimal:
                return parseFloat(value);                
            case ColumnDataType.Text:
                return value
            case ColumnDataType.Datetime:
                debugger;
                return DateFactory.toDynamicColumnDateString(value);
            case ColumnDataType.Boolean:
                return value;
            default: 
                throw Error("Invalid data type");
        }
    }



    handleCancel(){                    
        this.activeModal.close();        
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

    ngOnDestroy(): void {
        this.subscriptions.forEach(x=> x.unsubscribe());
    }
}