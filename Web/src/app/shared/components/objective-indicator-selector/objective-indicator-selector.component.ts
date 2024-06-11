import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { Camp } from 'src/app/models/common/camp.model';
import { IObjectiveIndicator } from 'src/app/models/frameworks/objective-indicator.model';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { CommonService } from 'src/app/services/common.service';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { MonitoringFrameworkService } from 'src/app/services/monitoring-framework.service';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'objective-indicator-selector-modal',
  templateUrl: './objective-indicator-selector.component.html',
  styleUrls: ['./objective-indicator-selector.component.scss']
})
export class ObjectiveIndicatorSelectorComponent implements OnInit {

  selectedIndicatorIds: IObjectiveIndicator[];
  isMultivalued: string;

  public searchText:string;
  public returnIndicator: IObjectiveIndicator[] = [];
  public lstIndicator: any[];
  public indicatorPaginationConfig: PaginationInstance = {
    id: 'indicator_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  public baseCampQuery: IBaseQueryModel = {
    pageNo: 1,
    pageSize: 10,
    searchText:''
  };

  constructor(private monitoringService: MonitoringFrameworkService, private activeModal: NgbActiveModal) { }

  ngOnInit() {
    if (this.selectedIndicatorIds)
      this.returnIndicator = this.selectedIndicatorIds;
    this.loadIndicator();
  }

  loadIndicator() {
    this.monitoringService
      .getObjectiveIndicator(this.baseCampQuery)
      .then(a => {
        this.lstIndicator = a.data;
        this.lstIndicator.map(a => {

          a.Selected = this.checkSelected(a.id);
        })
        this.indicatorPaginationConfig.totalItems = a.total;
        this.indicatorPaginationConfig.currentPage = this.baseCampQuery.pageNo;
      })
  }

  setSearchText(){
    this.baseCampQuery.searchText="";
    Object.assign(this.baseCampQuery.searchText,this.searchText);
    this.loadIndicator();
  }

  checkSelected(id) {

    for (var i = 0; i < this.returnIndicator.length; i++) {
      if (this.returnIndicator[i].id == id) {
        return true;
      }
    }
    return false;
  }
  checkbox_changed(event, camp) {
    event.preventDefault();
    if (!event.target.checked) {
      // var ind = this.returnCamp.indexOf(camp);
      // this.returnCamp.splice(ind, 1)
      // this.reloadCheck();
      // return;


      for (var i = 0; i < this.returnIndicator.length; i++) {
        if (this.returnIndicator[i].id == camp.id) {
          this.returnIndicator.splice(i, 1)
          this.reloadCheck();
          break;

        }
      }
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnIndicator.push(camp);
      this.reloadCheck();
    }
    else {
      
      this.returnIndicator = [camp];
      this.reloadCheck();

    }
  }

  reloadCheck() {
    this.lstIndicator.map(a => {

      a.Selected = this.checkSelected(a.id);
    })
  }

  pageChangedInstance(event) {
    this.baseCampQuery.pageNo = event;
    this.loadIndicator();
  }

  closeModal() {
    this.activeModal.close(this.returnIndicator);
  }
  confirm() {

    this.closeModal();
  }
}
