import { Component, OnInit } from '@angular/core';
import { ReportingService } from 'src/app/services/reporting.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  selectedReportValue:number=1;
  selectedReportName:string="5w Dashboard";
  reports = [
    { name: "5w Dashboard", value: 'five-w-report' },
    { name: "Summary report", value: 'summary-report' },
    { name: "Gap analysis report", value: 'gap-analysis-report' },
    { name: "Damage report", value: 'damage-report' },
    { name: "Camp wise report", value: 'camp-wise-report' },
    { name: "Duplication report", value: 'duplication-report' }
  ]

  selectedRoute: any;
  constructor(private reportingService:ReportingService, private toast: ToastrService,
    private router: Router) { }

  ngOnInit() {
    this.router.navigate(['unicef/reporting/home/five-w-report'])
  }
  selectReportClicked(report){
    this.selectedReportValue=report.value;
    this.selectedReportName=report.name
  }

  onRouteSelected(route: any){
    // if(!route)
    //   this.selectFirstReport();      
    this.selectedReportName=route.name
  }
  selectFirstReport(){
    this.onRouteSelected(this.reports[0]);
  }
  
}
