import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class Globals {
  loginId : number = -1;
  maxPageSize: number =  2147483647;   
  defaultPageSize: number =  10;   
}