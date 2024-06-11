import { InstanceStatus } from 'src/app/_enums/instance-status';

export class Instance{
    id:number ;
    scheduleId:number;
    title: string
    dataCollectionDate: string
    status: InstanceStatus
}