import { InstanceStatus } from 'src/app/_enums/instanceStatus';

export class InstanceViewModel{
    id: number;
    scheduleId: number;
    title: string;
    dataCollectionDate: string; 
    endDate: string; 
    status: InstanceStatus;
}