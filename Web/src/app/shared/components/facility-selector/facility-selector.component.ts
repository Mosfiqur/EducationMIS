import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Camp } from 'src/app/models/common/camp.model';
import { FacilityViewModel } from 'src/app/models/facility/facilityViewModel';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { CommonService } from 'src/app/services/common.service';
import { FacilityService } from 'src/app/services/facility.service';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'facility-selector-modal',
  templateUrl: './facility-selector.component.html',
  styleUrls: ['./facility-selector.component.scss']
})
export class FacilitySelectorComponent implements OnInit {

  selectedFacilityIds: FacilityViewModel[];
  isMultivalued: string;

  public returnFacility: FacilityViewModel[] = [];
  public lstFacility: any[];
  public facilityPaginationConfig: PaginationInstance = {
    id: 'facility_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  private searchTerms = new Subject<string>();
  query: IBaseQueryModel={
    pageNo:1,
    pageSize:10,
    searchText:""
  }

  constructor(private facilityService: FacilityService, private activeModal: NgbActiveModal) {
    this.searchTerms.pipe(
			// wait 400ms after each keystroke before considering the term
			debounceTime(400),

			// ignore new term if same as previous term
			distinctUntilChanged(),

		).subscribe(value => {
      this.query.searchText=value;
			this.loadFacility(this.facilityPaginationConfig.itemsPerPage,this.facilityPaginationConfig.currentPage,this.query.searchText);
		});
   }

  ngOnInit() {
    if (this.selectedFacilityIds)
      this.returnFacility = this.selectedFacilityIds;
    setTimeout(() => {
      this.loadFacility(this.facilityPaginationConfig.itemsPerPage,this.facilityPaginationConfig.currentPage,this.query.searchText);
    });
    
  }

	search(term: string): void {
		this.searchTerms.next(term);
	}
  loadFacility(pageSize,pageNo,searchText) {
    this.query={
      pageNo:pageNo,
      pageSize:pageSize,
      searchText:searchText      
    }
    this.facilityService
      .getAllLatest(this.query)
      .then(a => {
        this.lstFacility = a.data;
        this.lstFacility.map(a => {

          a.Selected = this.checkSelected(a.id);
        })
        this.facilityPaginationConfig.totalItems = a.total;
        this.facilityPaginationConfig.currentPage = pageNo;
      })
  }



  checkSelected(id) {

    for (var i = 0; i < this.returnFacility.length; i++) {
      if (this.returnFacility[i].id == id) {
        return true;
      }
    }
    return false;
  }
  checkbox_changed(event, facility) {
    event.preventDefault();
    if (!event.target.checked) {
      // var ind = this.returnFacility.indexOf(facility);
      // this.returnFacility.splice(ind, 1)
      // this.reloadCheck();
      // return;

      
      for (var i = 0; i < this.returnFacility.length; i++) {
        if (this.returnFacility[i].id == facility.id) {
          this.returnFacility.splice(i, 1)
          this.reloadCheck();
          break;

        }
      }
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnFacility.push(facility);
      this.reloadCheck();
    }
    else {
      this.returnFacility = [facility];
      this.reloadCheck();

    }
  }

  reloadCheck() {
    this.lstFacility.map(a => {

      a.Selected = this.checkSelected(a.id);
    })
  }

  pageChangedInstance(event) {
    this.facilityPaginationConfig.currentPage = event;
    this.loadFacility(this.facilityPaginationConfig.itemsPerPage,this.facilityPaginationConfig.currentPage,this.query.searchText);
  }

  closeModal() {
    this.activeModal.close(this.returnFacility);
  }
  confirm() {

    this.closeModal();
  }
}
