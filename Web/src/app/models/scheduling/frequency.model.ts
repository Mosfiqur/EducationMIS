import { ISchedule } from './schedule.model';
import { ScheduleType } from 'src/app/_enums/schedule-type';

export interface IFrequency {
    id?: number;
    scheduleId?: number;
    interval: number;
    day?: number;
    month?: number;
    daysOfWeek?: number[];
}

export class Frequency implements IFrequency{    
    public static createNew(scheduleType: string, startDate: Date){
        switch(ScheduleType[scheduleType]){
            case ScheduleType.Weekly:
                return this.createWeekly(startDate);                
            case ScheduleType.BiWeekly:
                return this.createBiWeekly(startDate); 
            case ScheduleType.Monthly:
                return this.createMonthly(startDate);                
        }
    }

    private static createWeekly(startDate: Date): IFrequency {
        return {
            interval: 1,
            daysOfWeek: [startDate.getDay()]
        }
    }

    private static createBiWeekly(startDate: Date): IFrequency {
        return {
            interval: 2,
            daysOfWeek: [startDate.getDay()]
        }
    }

    private static createMonthly(startDate: Date): IFrequency {
        return {
            interval: 1,
            day: startDate.getDate()
        }
    }

    id?: number;
    scheduleId?: number;
    interval: number;
    day?: number;
    month?: number;
    daysOfWeek?: any[];

    constructor(
        interval: number, 
        day?: number,
        month?: number,
        daysOfWeek?: number[],
        id?: number,
        scheduleId?: number
    ){
        this.id = id;
        this.scheduleId = scheduleId;
        this.interval = interval;
        this.day = day;
        this.month = month;
        this.daysOfWeek = daysOfWeek;
    }
}