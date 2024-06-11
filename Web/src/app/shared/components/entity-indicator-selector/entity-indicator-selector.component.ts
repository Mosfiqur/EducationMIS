import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Camp } from 'src/app/models/common/camp.model';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { IObjectiveIndicator } from 'src/app/models/frameworks/objective-indicator.model';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { IDynamicColumnQueryModel } from 'src/app/models/queryModels/dynamic-column.model';
import { CommonService } from 'src/app/services/common.service';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { MonitoringFrameworkService } from 'src/app/services/monitoring-framework.service';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'entity-indicator-selector-modal',
  templateUrl: './entity-indicator-selector.component.html',
  styleUrls: ['./entity-indicator-selector.component.scss']
})
export class EntityIndicatorSelectorComponent implements OnInit {

  selectedIndicatorIds: IDynamicColumn[];
  isMultivalued: string;

  public searchText: string;
  public returnIndicator: IDynamicColumn[] = [];
  public lstIndicator: any[];
  public indicatorPaginationConfig: PaginationInstance = {
    id: 'indicator_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  public baseQuery: IDynamicColumnQueryModel = {
    pageNo: 1,
    pageSize: 10,
    searchText: '',
    entityType:EntityType.Beneficiary
  };
  private searchTerms = new Subject<string>();
  
  constructor(private monitoringService: MonitoringFrameworkService,
    private activeModal: NgbActiveModal,
    private dynamicColumnService: DynamicColumnService) { 

      this.baseQuery.entityType=EntityType.Beneficiary

      this.searchTerms.pipe(
        // wait 400ms after each keystroke before considering the term
        debounceTime(400),
  
        // ignore new term if same as previous term
        distinctUntilChanged(),
  
      ).subscribe(value => {
        this.baseQuery.searchText=value;
        this.loadIndicator();
      });
  
    }

  ngOnInit() {
    if (this.selectedIndicatorIds)
      this.returnIndicator = this.selectedIndicatorIds;
    this.loadIndicator();
  }

	search(term: string): void {
		this.searchTerms.next(term);
	}
  loadIndicator() {
    this.dynamicColumnService
      .getNumericColumns(this.baseQuery)
      .then(a => {
        this.lstIndicator = a.data;
        this.lstIndicator.map(a => {

          a.Selected = this.checkSelected(a.id);
        })
        this.indicatorPaginationConfig.totalItems = a.total;
        this.indicatorPaginationConfig.currentPage = this.baseQuery.pageNo;
      })
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
    this.baseQuery.pageNo = event;
    this.loadIndicator();
  }

  closeModal() {
    this.activeModal.close(this.returnIndicator);
  }
  confirm() {

    this.closeModal();
  }
}
