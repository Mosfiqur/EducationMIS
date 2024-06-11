import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { InstanceStatus } from 'src/app/_enums/instance-status';
import { IScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';

@Component({
    selector: 'instance-status',
    templateUrl: './instance-status.component.html'
})
export class InstanceStatusComponent implements OnChanges {
    ngOnChanges(changes: SimpleChanges): void {
        if (changes.instance) {
            this.instance = changes.instance.currentValue;
        }
    }
    instanceStatusEnum: any;
    constructor() {
        this.instanceStatusEnum = InstanceStatus;
    }
    @Input('instance') instance: IScheduleInstance;
}