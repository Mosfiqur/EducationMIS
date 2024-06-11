import { Component, OnInit, Input, Output } from '@angular/core';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { dynamicCellViewModel } from 'src/app/models/viewModel/dynamicCellViewModel';
import { EventEmitter } from 'protractor';

@Component({
  selector: 'app-column-data-type',
  templateUrl: './column-data-type.component.html',
  styleUrls: ['./column-data-type.component.scss']
})
export class ColumnDataTypeComponent implements OnInit {

  @Input() columnDataType:ColumnDataType;
  ColumnDataTypeEnum : ColumnDataType;

  constructor() { }

  ngOnInit() {
    
  }
  
}
