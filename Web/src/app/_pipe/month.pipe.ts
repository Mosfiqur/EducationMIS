import { Pipe, PipeTransform } from '@angular/core';
import { Month } from '../_enums/month';


@Pipe({
    name: 'month',
    pure: true
})
export class MonthPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {

        let format:string = "";

        if(args){
            format = args[0];
        }

        if(format === 'MMM'){
            return Month[value].substr(0, 3);
        }
        if(format === 'MMMM'){
            return Month[value];
        }
        if(format === 'MM'){
            return Month[value];    
        }
        return Month[value];
    }
}