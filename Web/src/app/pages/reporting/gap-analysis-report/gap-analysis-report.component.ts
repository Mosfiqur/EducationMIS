import { Component, OnInit } from '@angular/core';
import { IScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-gap-analysis-report',
  templateUrl: './gap-analysis-report.component.html',
  styleUrls: ['./gap-analysis-report.component.scss']
})
export class GapAnalysisReportComponent implements OnInit {

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
    this.reportingService.getGapAnalysisReport(this.currentInstance.id);
  }
}
