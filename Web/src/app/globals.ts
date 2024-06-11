import { Injectable } from '@angular/core';
import { IDropdownSettings } from './lib/multi-select';


@Injectable({
  providedIn: 'root'
})
export class Globals {
  loginId : number = -1;
  maxPageSize: number =  2147483647;   
  defaultPageSize: number =  10; 
  searchDebounce: number = 400;
  maxInt: number = 2147483647;
  minInt: number = -2147483648;
  maxDecimal: number = 7.922816251426434e+28;
  minDecimal: number = -7.922816251426434e+28;
  shortDateFormat: string = "dd-MMM-yy";
  dynamicColumnDateFormat: string = 'DD-MMM-YYYY';
  multiSelectSettings: IDropdownSettings = {
    singleSelection: false,    
    selectAllText: 'Select All',
    unSelectAllText: 'UnSelect All',
    itemsShowLimit: 3,
    allowSearchFilter: true,
    idField: 'id',
    textField: 'text'
  }
  bsDatepickerConfig = {
    dateInputFormat: 'YYYY-MM-DD',
    placement: 'bottom',
    placeholderText: 'i.e: yyyy-mm-dd'  
  }
  
}