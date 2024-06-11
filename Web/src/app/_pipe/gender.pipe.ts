import { Pipe, PipeTransform } from '@angular/core';
import { Gender } from '../_enums/gender';

@Pipe({
    name: 'gender',
    pure: true
})
export class GenderPipe implements PipeTransform {
    transform(value: any, ...args: any[]) {
        return Gender[value];
    }
}