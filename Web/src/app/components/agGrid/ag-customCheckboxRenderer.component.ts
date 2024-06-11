import { Component } from '@angular/core';

import { ICellRendererAngularComp } from 'ag-grid-angular';


@Component({
  selector: 'aglist-cell',
  template: `
   <label title="{{generatedValue}}">{{generatedValue}}</label>
  `,
})
export class AgCustomCheckboxRenderer implements ICellRendererAngularComp {
  private params: any;

  values = [];
  selectedValues = [];
  generatedValue: string = ""

  agInit(params: any): void {
    
    this.params = params;
    this.generateValue(params);
  }

  refresh(params: any): boolean {
    
    this.params = params;
    this.generateValue(params);
    return true;
  }

  private generateValue(params) {
    
    this.values = params.colDef.cellEditorParams.values;
    this.selectedValues = params.value!=null?params.value:[];
    
    var rValue = '';
    for (var i = 0; i < this.selectedValues.length; i++) {
      rValue = rValue + this.values.filter(a => a.value == this.selectedValues[i])[0].title + ':' + this.selectedValues[i] + ','
    }
    this.generatedValue = rValue.replace(/,(?=\s*$)/, '');
  }
}
