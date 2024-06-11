import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Camp } from 'src/app/models/common/camp.model';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { TeacherViewModel } from 'src/app/models/user/teacherViewModel';
import { CommonService } from 'src/app/services/common.service';
import { FacilityService } from 'src/app/services/facility.service';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'teacher-selector-modal',
  templateUrl: './teacher-selector.component.html',
  styleUrls: ['./teacher-selector.component.scss']
})
export class TeacherSelectorComponent implements OnInit {

  selectedTeacherIds: TeacherViewModel[];
  isMultivalued: string;

  public returnTeacher: TeacherViewModel[] = [];
  public lstTeacher: any[];
  public teacherPaginationConfig: PaginationInstance = {
    id: 'teacher_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  private searchTerms = new Subject<string>();
  public baseCampQuery: IBaseQueryModel = {
    pageNo: 1,
    pageSize: 10,
    searchText: ""
  };

  constructor(private commonService: CommonService, private activeModal: NgbActiveModal
    , private facilityService: FacilityService) {
    this.searchTerms.pipe(
      // wait 400ms after each keystroke before considering the term
      debounceTime(400),

      // ignore new term if same as previous term
      distinctUntilChanged(),

    ).subscribe(value => {
      this.baseCampQuery.searchText = value;
      this.loadTeacehr(this.baseCampQuery);
    });


  }

  ngOnInit() {
    if (this.selectedTeacherIds)
      this.returnTeacher = this.selectedTeacherIds;
    setTimeout(() => {
      this.loadTeacehr(this.baseCampQuery);
    });

  }

  search(term: string): void {
    this.searchTerms.next(term);
  }
  loadTeacehr(queryModel: IBaseQueryModel) {

    this.facilityService.getTeachers(queryModel.pageSize, queryModel.pageNo, queryModel.searchText).then(data => {
      this.lstTeacher = data.data;
      this.lstTeacher.map(a => {

        a.Selected = this.checkSelected(a.id);
      })
      this.teacherPaginationConfig.currentPage = queryModel.pageNo;
      this.teacherPaginationConfig.totalItems = data.total;
    })
  }



  checkSelected(id) {

    for (var i = 0; i < this.returnTeacher.length; i++) {
      if (this.returnTeacher[i].id == id) {
        return true;
      }
    }
    return false;
  }
  checkbox_changed(event, teacher) {
    event.preventDefault();
    if (!event.target.checked) {
      // var ind = this.returnCamp.indexOf(camp);
      // this.returnCamp.splice(ind, 1)
      // this.reloadCheck();
      // return;


      for (var i = 0; i < this.returnTeacher.length; i++) {
        if (this.returnTeacher[i].id == teacher.id) {
          this.returnTeacher.splice(i, 1)
          this.reloadCheck();
          break;

        }
      }
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnTeacher.push(teacher);
      this.reloadCheck();
    }
    else {
      this.returnTeacher = [teacher];
      this.reloadCheck();

    }
  }

  reloadCheck() {
    this.lstTeacher.map(a => {
      a.Selected = this.checkSelected(a.id);
    })
  }

  pageChangedInstance(event) {
    this.baseCampQuery.pageNo = event;
    this.loadTeacehr(this.baseCampQuery);
  }

  closeModal() {
    this.activeModal.close(this.returnTeacher);
  }
  confirm() {

    this.closeModal();
  }
}
