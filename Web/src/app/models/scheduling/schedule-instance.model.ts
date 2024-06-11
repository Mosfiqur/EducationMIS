import { InstanceStatus } from 'src/app/_enums/instance-status';

export interface IScheduleInstance {
    id?: number;
    scheduleId: number;
    title: string;
    dataCollectionDate: Date;
    status: InstanceStatus;
    isSelected?: boolean;
}

export interface IScheduleInstanceUpdateModel {
    id: number;
    scheduleId: number;
    title: string;
}

export class ScheduleInstance implements IScheduleInstance {
    id?: number;
    scheduleId: number;
    title: string;
    dataCollectionDate: Date;
    status: InstanceStatus;
    isSelected?: boolean;
}