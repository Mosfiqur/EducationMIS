import { AbstractControl, ValidatorFn } from '@angular/forms';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';

export class CustomValidators {
    public static dateRangeValidator(startControlName: string, endControlName: string): ValidatorFn {
        return function dateRangeValidatorFn(control: AbstractControl): {[key: string]: Boolean} | null{
            let start = control.get(startControlName).value as NgbDate;
            let end = control.get(endControlName).value as NgbDate;          
            if(!start || !end)
              return null;
            let startDate = new NgbDate(start.year, start.month, start.day);
            let endDate = new NgbDate(end.year, end.month, end.day);
            
            if(startDate.after(endDate)){
              return {
                invalidDateRange: true
              }
            }
            return null;  
          }
    }

    public static dateRangeValidatorForTarget(startControlName: string, endControlName: string): ValidatorFn {
      return function dateRangeValidatorFn(control: AbstractControl): {[key: string]: Boolean} | null{
          let start = control.get(startControlName).value;
          let end = control.get(endControlName).value;        
          if(!start || !end)
            return null;
          let startDate = new NgbDate(parseInt(start.startYear), parseInt(start.startMonth), 0);
          let endDate = new NgbDate(parseInt(end.endYear), parseInt(end.endMonth), 0);
          
          if(startDate.after(endDate)){
            return {
              invalidDateRange: true
            }
          }
          return null;  
        }
  }
    
    

    public static confirmPassword(password: string, confirm: string): ValidatorFn{
      return function confirmPassword(c: AbstractControl): {[key: string]: Boolean} | null{        
          const newP = c.get(password).value;
          const confP = c.get(confirm).value;
          if(!newP){
              return null;
          }
      
          if(newP != confP){
              return {
                  confirmPasswordError: true
              }
          }
          return null;
      }
    }
}

