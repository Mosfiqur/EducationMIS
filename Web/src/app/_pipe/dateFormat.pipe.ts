import { PhoneNumber } from 'google-libphonenumber';

import { Pipe, PipeTransform } from '@angular/core';

@Pipe({name: 'dateFormat'})
export class dateFormatPipe implements PipeTransform {
  transform(value: Date): string {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];;
                
    return months[value.getMonth()] + ' ' +value.getDate()+  this.getDayNumberSuffix(value.getDate()) + ' ' +  value.getFullYear();
  
    }
    getDayNumberSuffix(day) {
        if (day >= 11 && day <= 13) {
            return "th";
        }
        switch (day % 10) {
        case 1:
            return "st";
        case 2:
            return "nd";
        case 3:
            return "rd";
        default:
            return "th";
        };
    }
}
