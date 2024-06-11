import { Component, OnInit } from '@angular/core';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-five-w-report',
  templateUrl: './five-w-report.component.html',
  styleUrls: ['./five-w-report.component.scss']
})
export class FiveWReportComponent implements OnInit {
  public facilityInstances: InstanceViewModel[] = []

  constructor(private reportingService: ReportingService) { }

  ngOnInit() {
  }
  facilityInstanceValue_changed(data) {
    this.facilityInstances = data;
  }

  removeFacilityInstance(c) {
    var ins = this.facilityInstances.indexOf(c);
    this.facilityInstances.splice(ins, 1);
  }

  public download5wReport(): void {
    if (this.facilityInstances.length > 0) {
      this.reportingService.get5WReport(this.facilityInstances[0].id);
    }
  }

}
