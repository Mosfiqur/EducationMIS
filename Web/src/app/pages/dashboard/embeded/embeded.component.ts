import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { EmbeddedDashboardEntryComponent } from 'src/app/components/embeddedDashboard/embeddedDashboard.component';
import { EmbeddedDashboard } from 'src/app/models/dashboard/embedded-dashboard.model';
import { DashboardService } from 'src/app/services/dashboard.service';
import { ModalService } from 'src/app/services/modal.service';

import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-embeded',
  templateUrl: './embeded.component.html',
  styleUrls: ['./embeded.component.scss']
})
export class EmbededComponent implements OnInit {

  lstEmbeddedDashboard: EmbeddedDashboard[];
  selectedDashboard: EmbeddedDashboard;
  url: SafeResourceUrl;
  height: number = window.innerHeight;
  width: number = 100;

  constructor(private dashboardService: DashboardService,
    private modalService: ModalService,
    private toast: ToastrService,
    private sanitizer: DomSanitizer) {
    this.selectedDashboard = new EmbeddedDashboard();

    
  }

  ngOnInit() {
    this.getDashboard();
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl("");

   // eval(this.testScript);
  }

  openEmbeddedDashboard() {

    this.modalService.open<EmbeddedDashboardEntryComponent, {}>
      (EmbeddedDashboardEntryComponent, {})
      .then(column => {
        this.getDashboard();
      })
  }

  editEmbeddedDashboard(c: EmbeddedDashboard) {
    this.modalService.open<EmbeddedDashboardEntryComponent, {}>
      (EmbeddedDashboardEntryComponent, { embeddedDashboard: c })
      .then(column => {
        this.getDashboard();
      })
  }
  dashboardSelectedClicked(dash:EmbeddedDashboard) {
    this.selectedDashboard = dash;
    //this.url=this.sanitizer.bypassSecurityTrustHtml(this.selectedDashboard.link);
    this.url = this.sanitizer.bypassSecurityTrustResourceUrl(this.selectedDashboard.link);
  }
  getDashboard() {
    this.dashboardService.getEmbeddedDashboard()
      .then(data => {
        this.lstEmbeddedDashboard = data;
        if(data.length>0){
          this.selectedDashboard = data[0];
          this.url = this.sanitizer.bypassSecurityTrustResourceUrl(this.selectedDashboard.link);
        }
      })
  }

  // getUrl() {
  //   if (this.selectedDashboard.link)
  //     return this.sanitizer.bypassSecurityTrustUrl(this.selectedDashboard.link);
  // }
  deleteJrpParameter(c: number) {
    this.dashboardService.deleteJrpParameterInfo(c)
      .then(column => {
        this.getDashboard();

      })
  }
}
