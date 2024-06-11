import { Component, OnInit } from '@angular/core';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-camp-wise-report',
  templateUrl: './camp-wise-report.component.html',
  styleUrls: ['./camp-wise-report.component.scss']
})
export class CampWiseReportComponent implements OnInit {

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

  public downloadCampWiseReport(): void {
    if (this.facilityInstances.length > 0) {
      this.reportingService.getCampWiseReport(this.facilityInstances[0].id);
    }
  }


}
