import { Pipe, PipeTransform } from '@angular/core';
import { InstanceStatus } from '../_enums/instance-status';

@Pipe({
    name: 'instanceStatus',
    pure: true
})
export class InstanceStatusPipe implements PipeTransform {
    constructor(){

    }
    transform(value: any, ...args: any[]) {
        return InstanceStatus[value];
    }
}