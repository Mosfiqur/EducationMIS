import { Component, OnInit } from '@angular/core';
import { ISchedule } from 'src/app/models/scheduling/schedule.model';
import { BeneficiaryScheduleService } from 'src/app/services/beneficiary-schedule.service';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { IScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { Globals } from 'src/app/globals';
import { PaginationInstance } from 'ngx-pagination';
import { InstanceStatus } from 'src/app/_enums/instance-status';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { EntityType } from 'src/app/_enums/entityType';

@Component({
  selector: 'app-scheduleinstance',
  templateUrl: './scheduleinstance.component.html',
  styleUrls: ['./scheduleinstance.component.scss']
})
export class ScheduleinstanceComponent implements OnInit {

  pagedInstances: IPagedResponse<IScheduleInstance>;
  entityType:EntityType;
  pageSize: number = 10;

  constructor(
    private scheduleService: BeneficiaryScheduleService,
    private globals: Globals,
    private toastrService: ToastrService
  ) { 
    this.pagedInstances={data:[],pageNo:0,pageSize:this.pageSize,total:0}
  }

  async ngOnInit() {
    this.entityType = EntityType.Beneficiary;
    this.getInstances(1);
    
  }

  async getInstances(pageNo: number){
    this.pagedInstances = 
    await this.scheduleService
    .getCurrentScheduleInstances({pageNo: pageNo, pageSize: this.pageSize});
  }

  async onGetPage(pageNo: number){
    await this.getInstances(pageNo);    
  }

  
  onPageSizeChanged(pageSize: number){
    this.pageSize = pageSize;
    this.getInstances(1);
  }
 
  onStartCollection(selectedInstances: IScheduleInstance[]){
    selectedInstances
    .forEach(instance => {
      this.scheduleService.startCollection({instanceId: instance.id},this.entityType)
      .then(x => {
        this.toastrService.success(MessageConstant.SaveSuccessful);
        instance.status = InstanceStatus.Running;
      });
    });
  }

  onAllCompleted(){
    this.getInstances(1);
  }

  
}
