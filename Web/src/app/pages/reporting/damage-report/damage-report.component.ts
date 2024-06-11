import { Component, OnInit } from '@angular/core';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({  
  templateUrl: './damage-report.component.html',
  styleUrls: ['./damage-report.component.scss']
})
export class DamageReportComponent implements OnInit {
  public facilityInstances: InstanceViewModel[] = []

  constructor(private reportingService: ReportingService) { }

  ngOnInit() {
  }
  
  public currentInstance: IScheduleInstance;
 
  onInstanceSelected(instance: IScheduleInstance[]){    
    this.currentInstance = instance[0];
  }

  download(){
    this.reportingService.getDamageReport(this.currentInstance.id);
  }

}
