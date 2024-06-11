import { Component, OnInit } from '@angular/core';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { Schedule, ISchedule } from 'src/app/models/scheduling/schedule.model';
import { BeneficiaryScheduleService } from 'src/app/services/beneficiary-schedule.service';
import { ToastrService } from 'ngx-toastr';
import { ModalService } from 'src/app/services/modal.service';
import { MessageConstant } from 'src/app/utility/MessageConstant';

import { PaginationInstance } from 'ngx-pagination';
import { Globals } from 'src/app/globals';
import { ScheduleCreationComponent } from 'src/app/shared/components/schedule-creation/schedule-creation.component';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {

  pagedSchedules: IPagedResponse<Schedule>;
  
  get schedules(): Schedule[]{
    return this.pagedSchedules ? this.pagedSchedules.data : [];
  }

  paginationConfig: PaginationInstance  = {
    id: 'schedule_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }
    
  isAllSelected : boolean =  false;
  selectedSchedules: Map<number, boolean>;

  get numberOfSchedulesSelected():number {
    return this.selectedSchedules.size;
  }

  get selectedSchedule(): ISchedule{
    return this.schedules.filter(x => x.isSelected)[0];
  }

  constructor(
    private scheduleServie: BeneficiaryScheduleService,
    private toastrService: ToastrService,
    private modalService: ModalService,
    private globals: Globals
  ) { 
    this.selectedSchedules = new Map();
  }

  ngOnInit() {
    this.getAllSchedules();
  }

  getAllSchedules(pageNo: number = 1){
    this.scheduleServie.getSchedules({pageNo: pageNo, pageSize: this.paginationConfig.itemsPerPage})
    .then(response => {
      this.pagedSchedules = response;
      this.paginationConfig.currentPage = pageNo;
      this.paginationConfig.totalItems = this.pagedSchedules.total;
    });
  }
  applySelection(){
    this.schedules.forEach(x=> {
      if(this.selectedSchedules.has(x.id)){
        x.isSelected = true;
      }
    })
  }
  onPageSizeChanged(pageSize: number){
    this.paginationConfig.itemsPerPage = pageSize;
    this.getAllSchedules(1);
  }

  async deleteSelectedSchedules(){    
    if(await this.modalService.confirm()){
      await this.schedules.filter(x => x.isSelected)
      .forEach(async schedule => {
        this.scheduleServie.deleteSchedule(schedule.id)
        .then(res => {        
          this.selectedSchedules.delete(schedule.id);        
          let idx = this.schedules.findIndex(x => x.id == schedule.id);
          this.schedules.splice(idx, 1);
          this.toastrService.success(MessageConstant.DeleteSuccess);
      });
    });    
    }      
  }

  createNewSchedule(){
    
    this.modalService.open<ScheduleCreationComponent, ISchedule>(ScheduleCreationComponent, {})
    .then(async schedule => {      
      await this.scheduleServie.createSchedule(schedule)
      this.toastrService.success(MessageConstant.SaveSuccessful);
      this.getAllSchedules();
    }).catch(x => x);
  }

  editSchedule(){
    this.modalService.open<ScheduleCreationComponent, ISchedule>(ScheduleCreationComponent, {schedule: this.selectedSchedule})
    .then(async schedule => {      
      this.scheduleServie.updateSchedule(schedule)
      .then(x => {        
        this.selectedSchedule.scheduleName = schedule.scheduleName;
        this.selectedSchedule.description = schedule.description;
        this.toastrService.success(MessageConstant.SaveSuccessful);      
      });      
    })
    .catch( x=> x)
  }
  
  toggleSelection(schedule: Schedule){
    if(schedule.isSelected){
      this.selectedSchedules.set(schedule.id, true);
    }else{
      this.selectedSchedules.delete(schedule.id);
    }   
    
    if(this.selectedSchedules.size != this.schedules.length){
      this.isAllSelected = false;
    }else{
      this.isAllSelected = true;
    }
  }

  getPage(pageNo: number){
      if(!pageNo)
      return;
      this.getAllSchedules(pageNo);
  }

  selectAll(event){    
    this.schedules.forEach(x=> x.isSelected = event);
    this.isAllSelected = event;
        
    if(event){
      this.schedules.forEach(x => this.selectedSchedules.set(x.id, true));
    }else{
      this.schedules.forEach(x => this.selectedSchedules.delete(x.id));
    }    
  } 
}
