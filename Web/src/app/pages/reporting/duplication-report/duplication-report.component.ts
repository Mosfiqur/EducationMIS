import { Component, OnInit } from '@angular/core';

import { Globals } from 'src/app/globals';
import { IDropdownSettings } from 'src/app/lib/multi-select';
import { IDuplicationReportQueryModel } from 'src/app/models/dashboard/duplication-report-query.model';

import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { EducationPartnerService } from 'src/app/services/educationPartner.service';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-duplication-report',
  templateUrl: './duplication-report.component.html',
  styleUrls: ['./duplication-report.component.scss']
})
export class DuplicationReportComponent implements OnInit {

  public espList: ISelectListItem[] = [];   
  public dropdownSettings:IDropdownSettings = {};
  constructor(
    private globals: Globals,
    private espService: EducationPartnerService,
    private reportingService: ReportingService
  ) { }

  public filters = {    
    selectedPPs: [],
    selectedIPs: []
  }

  ngOnInit() {
    this.dropdownSettings = {
      ...this.globals.multiSelectSettings,
    } as IDropdownSettings;

    this.espService.getAll()
    .then(result => {
      this.espList = result.map(x => ({id: x.id, text: x.partnerName}));      
    });
  }


  download(){
    this.reportingService.getDuplicationReport(this.getFilters());
  }

  getFilters(){
    let filter: IDuplicationReportQueryModel = {        
        programmingPartnerIds: this.filters.selectedIPs.map( x=> x.id),
        implementingPartnerIds: this.filters.selectedPPs.map(x=> x.id)        
    }
    return filter;
}

}
