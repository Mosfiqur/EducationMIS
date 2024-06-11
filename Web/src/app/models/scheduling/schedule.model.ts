import { IFrequency, Frequency } from './frequency.model';
import { ScheduleStatus } from 'src/app/_enums/schedule-status';
import { IScheduleInstance } from './schedule-instance.model';
import { ScheduleType } from 'src/app/_enums/schedule-type';
import { EntityType } from 'src/app/_enums/entityType';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { DateFactory } from 'src/app/utility/DateFactory';

export interface ISchedule {
    id?: number;
    scheduleName: string;
    startDate: Date;
    endDate: Date;
    scheduleType: ScheduleType;
    scheduleFor: EntityType;
    isDeleted?: boolean;
    status: ScheduleStatus;
    frequency: IFrequency;
    instances: IScheduleInstance[];
    description?: string;
    isSelected?: boolean;
}


export class Schedule implements ISchedule {

    static createNew(
            scheduleName: string, 
            scheduleType: ScheduleType,
            startDate: NgbDate, 
            endDate: NgbDate,             
            description?: string
            ){
        let newSchedule = new Schedule
          (
            scheduleName,
            scheduleType,
            DateFactory.createFromNgbDateStruct(startDate),
            DateFactory.createFromNgbDateStruct(endDate),
            Frequency.createNew(scheduleType.toString(), DateFactory.createFromNgbDateStruct(startDate)),
            description
          )
        return newSchedule;       
    }

    id?: number;
    scheduleName: string;
    startDate: Date;
    endDate: Date;
    scheduleType: ScheduleType;
    scheduleFor: EntityType;
    isDeleted?: boolean;
    status: ScheduleStatus;
    frequency: IFrequency;
    instances: IScheduleInstance[];
    description?: string;
    isSelected?: boolean
    
    constructor(
            scheduleName: string, 
            scheduleType: ScheduleType,
            startDate: Date, 
            endDate: Date,    
            frequency: IFrequency,
            description?: string
    ){
        this.scheduleName =  scheduleName,
        this.startDate = startDate,
        this.endDate = endDate,
        this.status= ScheduleStatus.Pending,
        this.scheduleType = scheduleType,
        this.frequency = frequency,
        this.description = description
    }

}

