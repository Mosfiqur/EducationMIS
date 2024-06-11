import { Component, OnInit } from '@angular/core';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { IScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { FacilityScheduleService } from 'src/app/services/facility-schedule.service';
import { Globals } from 'src/app/globals';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { InstanceStatus } from 'src/app/_enums/instance-status';
import { EntityType } from 'src/app/_enums/entityType';
import { BeneficiaryScheduleService } from 'src/app/services/beneficiary-schedule.service';

@Component({
  selector: 'app-scheduleinstance',
  templateUrl: './scheduleinstance.component.html',
  styleUrls: ['./scheduleinstance.component.scss']
})
export class ScheduleinstanceComponent implements OnInit {

  pagedInstances: IPagedResponse<IScheduleInstance>;
  entityType:EntityType;
  pageSize: number = 1;
  constructor(
    private scheduleService: FacilityScheduleService,
    private globals: Globals,
    private toastrService: ToastrService,
    private beneficiaryScheduleService: BeneficiaryScheduleService,
  ) { 
    this.pageSize = this.globals.defaultPageSize;
    this.pagedInstances={data:[],pageNo:0,pageSize:this.pageSize,total:0}
  }

  async ngOnInit() {
    this.entityType = EntityType.Facility;
    this.getInstances(1);
  }

  async getInstances(pageNo: number){
    this.pagedInstances = 
    await this.scheduleService
    .getCurrentScheduleInstances({pageNo: pageNo, pageSize: this.pageSize});
  }

  async getPage(pageNo: number){
    await this.getInstances(pageNo);   
  }

  onPageSizeChanged(pageSize: number){
    this.pageSize = pageSize;
    this.getInstances(1);
  }

  onStartCollection(selectedInstances: IScheduleInstance[]){
    debugger;
    selectedInstances
    .forEach(instance => {
      this.beneficiaryScheduleService.startCollection({instanceId: instance.id},this.entityType)
      .then(x => {
        this.toastrService.success(MessageConstant.SaveSuccessful);
        instance.status = InstanceStatus.Running;
      });
    });
  }

}
