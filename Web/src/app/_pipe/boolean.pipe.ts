import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'boolean',
    pure: true
})
export class BooleanPipe implements PipeTransform {
    constructor(){

    }
    transform(value: any, ...args: any[]) {
        return value === "Yes" ? "Yes" : "No";
    }
}