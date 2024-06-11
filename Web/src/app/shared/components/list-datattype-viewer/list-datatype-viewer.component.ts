import { Component, EventEmitter, Input, Output } from '@angular/core';
import { IListDataType } from 'src/app/models/dynamicColumn/list-datatype';

@Component({
    selector: 'list-viewer',
    templateUrl: './list-datatype-viewer.component.html'

})
export class ListDataTypeViewer{

    @Input('list') list: IListDataType;    
    constructor(){

    }
}