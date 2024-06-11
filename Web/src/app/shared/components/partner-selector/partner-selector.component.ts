import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { EducationSectorPartner } from 'src/app/models/educationSectorPartner/educationSectorPartner';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { EducationPartnerService } from 'src/app/services/educationPartner.service';

@Component({
  selector: 'partner-selector-modal',
  templateUrl: './partner-selector.component.html',
  styleUrls: ['./partner-selector.component.scss']
})
export class PartnerSelectorComponent implements OnInit {

  selectedIpIds: EducationSectorPartner[];
  isMultivalued: string;
  title: string = "Education Sector Partner";

  public returnIp: EducationSectorPartner[] = [];
  public lstIp: any[];
  public ipPaginationConfig: PaginationInstance = {
    id: 'ip_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  public baseCampQuery: IBaseQueryModel = {
    pageNo: 1,
    pageSize: 10
  };

  constructor(private educationPartnerService: EducationPartnerService, private activeModal: NgbActiveModal) { }

  ngOnInit() {
    if (this.selectedIpIds)
      this.returnIp = this.selectedIpIds;
    setTimeout(() => {
      this.loadESP();
    });

  }

  loadESP() {
    this.educationPartnerService.getAll().then(data => {
      this.lstIp = data;
      this.lstIp.map(a => {
        a.Selected = this.checkSelected(a.id);
      })
      this.ipPaginationConfig.totalItems = data.length;

    })
  }

  checkSelected(id) {

    for (var i = 0; i < this.returnIp.length; i++) {
      if (this.returnIp[i].id == id) {
        return true;
      }
    }
    return false;
  }
  checkbox_changed(event, partner) {
    event.preventDefault();
    if (!event.target.checked) {
      for (var i = 0; i < this.returnIp.length; i++) {
        if (this.returnIp[i].id == partner.id) {
          this.returnIp.splice(i, 1)
          this.reloadCheck();
          break;

        }
      }
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnIp.push(partner);
      this.reloadCheck();
    }
    else {
      this.returnIp = [partner];
      this.reloadCheck();

    }
  }

  reloadCheck() {
    this.lstIp.map(a => {
      a.Selected = this.checkSelected(a.id);
    })
  }
  pageChangedInstance(event) {
    this.ipPaginationConfig.currentPage = event;

  }

  closeModal() {
    this.activeModal.close(this.returnIp);
  }
  confirm() {

    this.closeModal();
  }
}
