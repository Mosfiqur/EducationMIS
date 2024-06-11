import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Camp } from 'src/app/models/common/camp.model';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { CommonService } from 'src/app/services/common.service';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'camp-selector-modal',
  templateUrl: './camp-selector.component.html',
  styleUrls: ['./camp-selector.component.scss']
})
export class CampSelectorComponent implements OnInit {

  selectedCampIds: Camp[];
  isMultivalued: string;

  public returnCamp: Camp[] = [];
  public lstCamp: any[];
  public campPaginationConfig: PaginationInstance = {
    id: 'camp_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  private searchTerms = new Subject<string>();
  public baseCampQuery: IBaseQueryModel = {
    pageNo: 1,
    pageSize: 10,
    searchText:""
  };

  constructor(private commonService: CommonService, private activeModal: NgbActiveModal) { 
    this.searchTerms.pipe(
			// wait 400ms after each keystroke before considering the term
			debounceTime(400),

			// ignore new term if same as previous term
			distinctUntilChanged(),

		).subscribe(value => {
      this.baseCampQuery.searchText=value;
			this.loadCamp(this.baseCampQuery);
		});


  }

  ngOnInit() {
    if (this.selectedCampIds)
      this.returnCamp = this.selectedCampIds;
    setTimeout(() => {
      this.loadCamp(this.baseCampQuery);
    });
    
  }

	search(term: string): void {
		this.searchTerms.next(term);
	}
  loadCamp(queryModel: IBaseQueryModel) {
    this.commonService
      .getPaginatedCamps(queryModel)
      .then(a => {
        this.lstCamp = a.data;
        this.lstCamp.map(a => {

          a.Selected = this.checkSelected(a.id);
        })
        this.campPaginationConfig.totalItems = a.total;
        this.campPaginationConfig.currentPage = queryModel.pageNo;
      })
  }



  checkSelected(id) {

    for (var i = 0; i < this.returnCamp.length; i++) {
      if (this.returnCamp[i].id == id) {
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


      for (var i = 0; i < this.returnCamp.length; i++) {
        if (this.returnCamp[i].id == camp.id) {
          this.returnCamp.splice(i, 1)
          this.reloadCheck();
          break;

        }
      }
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnCamp.push(camp);
      this.reloadCheck();
    }
    else {
      this.returnCamp = [camp];
      this.reloadCheck();

    }
  }

  reloadCheck() {
    this.lstCamp.map(a => {

      a.Selected = this.checkSelected(a.id);
    })
  }

  pageChangedInstance(event) {
    this.baseCampQuery.pageNo = event;
    this.loadCamp(this.baseCampQuery);
  }

  closeModal() {
    this.activeModal.close(this.returnCamp);
  }
  confirm() {

    this.closeModal();
  }
}
