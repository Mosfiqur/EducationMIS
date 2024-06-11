import { Component, OnInit } from '@angular/core';
import { IScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-summary-report',
  templateUrl: './summary-report.component.html',
  styleUrls: ['./summary-report.component.scss']
})
export class SummaryReportComponent implements OnInit {

  constructor(
    private reportingService: ReportingService
  ) { }

  ngOnInit() {
  }
  
  public currentInstance: IScheduleInstance;
 
  onInstanceSelected(instance: IScheduleInstance[]){
    console.log(instance);
    this.currentInstance = instance[0];
  }

  download(){
    this.reportingService.getSummaryReport(this.currentInstance.id);
  }

}
