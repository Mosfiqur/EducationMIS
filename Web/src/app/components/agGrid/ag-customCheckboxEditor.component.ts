import {
    AfterViewInit,
    Component,
    ViewChild,
    ViewContainerRef,
} from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    FormArray,
    FormControl,
    ValidatorFn
} from '@angular/forms';
import { ICellEditorAngularComp } from 'ag-grid-angular';

@Component({
    selector: 'ageditor-cell',
    template: `<div  class="mood">
    <form [formGroup]="form" (ngSubmit)="submit()">
    
    <div formArrayName="orders" *ngFor="let order of ordersFormArray.controls; let i = index">
      <input class="controlCheckbox" type="checkbox" [formControlName]="i"/>
      {{values[i].title}}
    </div>
    
    <div id="submitbuttonContainer">
    <button>submit</button>
    </div>
    </form>
    </div>
    `,
    styles: [
        `
        .controlCheckbox{
            padding: 6px;
        }
        .mood {
            border-radius: 15px;
          border: 1px solid grey;
          background: #e6e6e6;
          padding: 15px;
          text-align: left;
          display: inline-block;
          outline: none;
          z-index:99999;
          overflow:auto !important;
        }
        #submitbuttonContainer{
            margin-top:5px;
        }
      `,
    ],
})

export class AgCustomCheckboxEditor implements ICellEditorAngularComp, AfterViewInit {
    private params: any;
    constructor(private formBuilder: FormBuilder) {
        this.form = this.formBuilder.group({
            orders: new FormArray([])
        });

        this.addCheckboxes();
    }
    @ViewChild('container', { read: ViewContainerRef }) public container;
    
    form: FormGroup;
    values = [];
    selectedValues = [];
    returnValues:[];

    get ordersFormArray() {
        return this.form.controls.orders as FormArray;
    }

    isExist(data): boolean {
        var rValue = false;
        for (var i = 0; i < this.selectedValues.length; i++) {
            if (this.selectedValues[i] == data.value) {
                rValue = true;
                break;
            }
        }
        return rValue;
    }

    private addCheckboxes() {
        this.values.forEach((data) => {
            this.ordersFormArray.push(new FormControl(this.isExist(data)));
        });
    }

    submit() {
        const selectedOrderIds = this.form.value.orders
            .map((checked, i) => checked ? this.values[i].value : null)
            .filter(v => v !== null);
        this.returnValues = selectedOrderIds;        
        this.params.api.stopEditing();
    }

    // dont use afterGuiAttached for post gui events - hook into ngAfterViewInit instead for this
    ngAfterViewInit() {
        // window.setTimeout(() => {
        //   this.container.element.nativeElement.focus();
        // });
    }

    agInit(params: any): void {

        this.params = params;
        this.values=params.values;        
        this.selectedValues = params.value;        
        this.addCheckboxes()
    }

    getValue(): any {
        return this.returnValues!=null?this.returnValues:this.selectedValues;
    }

    isPopup(): boolean {
        return true;
        
    }

}
