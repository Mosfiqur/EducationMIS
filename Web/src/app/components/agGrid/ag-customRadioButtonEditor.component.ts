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
    ValidatorFn, Validators
} from '@angular/forms';
import { ICellEditorAngularComp } from 'ag-grid-angular';

@Component({
    selector: 'ageditor-cell',
    template: `<div  class="mood">
    <form [formGroup]="form" (ngSubmit)="submit()">
    
    <div *ngFor="let val of values; let i = index">

      <label>
      <input type="radio" value={{val.value}} formControlName="radioButton">
        <span>{{val.title}}</span>
      </label>
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

export class AgCustomRadioButtonEditor implements ICellEditorAngularComp, AfterViewInit {
    private params: any;
    constructor(private formBuilder: FormBuilder) {
        this.form = this.formBuilder.group({
            radioButton: ['', Validators.required]
        });

        this.addCheckboxes();
    }
    @ViewChild('container', { read: ViewContainerRef }) public container;

    form: FormGroup;
    values = [];
    selectedValues = [];
    returnValues: [];

    private addCheckboxes() {
        this.form.patchValue({radioButton:this.selectedValues[0]});
    }

    submit() {
        // const selectedOrderIds = this.form.value.orders
        //     .map((checked, i) => checked ? this.values[i].id : null)
        //     .filter(v => v !== null);

        let val:any=[]
        val.push(this.form.value.radioButton);
        this.returnValues = val;
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
        this.values = params.values != null ? params.values : [];
        this.selectedValues = params.value;
        this.addCheckboxes()
    }

    getValue(): any {
        return this.returnValues != null ? this.returnValues : this.selectedValues;
    }

    isPopup(): boolean {
        return true;

    }

}
