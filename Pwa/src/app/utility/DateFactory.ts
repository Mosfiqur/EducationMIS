import { NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

export class DateFactory {
    static create(year: number, month: number, day: number): Date{
        return new Date(year, month, day, 12, 0, 0);
    }

    static createFromNgbDateStruct(date: NgbDateStruct): Date{
        return new Date(date.year, date.month - 1, date.day, 12, 0, 0);
    }

    static toNgbDateStruct(dateStr: string): NgbDateStruct{        
        let date = new Date(dateStr);
        return new NgbDate(date.getFullYear(), date.getMonth()+1, date.getDate());
    }
}