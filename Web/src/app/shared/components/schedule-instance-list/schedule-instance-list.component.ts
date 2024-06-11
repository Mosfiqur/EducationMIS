import { Component, OnInit, Output, EventEmitter, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ISchedule } from 'src/app/models/scheduling/schedule.model';
import { BeneficiaryScheduleService } from 'src/app/services/beneficiary-schedule.service';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { IScheduleInstance, IScheduleInstanceUpdateModel } from 'src/app/models/scheduling/schedule-instance.model';
import { Globals } from 'src/app/globals';
import { PaginationInstance } from 'ngx-pagination';
import { InstanceStatus } from 'src/app/_enums/instance-status';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ModalService } from 'src/app/services/modal.service';
import { ScheduleInstanceService } from 'src/app/services/schedule-instance.service';
import { EntityType } from 'src/app/_enums/entityType';


@Component({
  selector: 'schedule-instance-list',
  templateUrl: './schedule-instance-list.component.html'
})
export class ScheduleinstanceListComponent implements OnChanges {


  paginationConfig: PaginationInstance = {
    id: 'instance_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  instanceStatusEnum: any;

  @Input('pagedData')
  pagedInstances: IPagedResponse<IScheduleInstance>;
  
  @Input('entityType')
  entityType: EntityType;
  

  @Output('startCollection') 
  startCollection: EventEmitter<IScheduleInstance[]> = new EventEmitter();
  @Output('getPage') 
  getPage: EventEmitter<number> = new EventEmitter();
  @Output('onPageSizeChanged') 
  pageSizeChangedEvent: EventEmitter<any> = new EventEmitter();
  @Output('onAllCompleted')
  allCompleted: EventEmitter<any> = new EventEmitter();


  get instances(): IScheduleInstance[] {
    return this.pagedInstances ? this.pagedInstances.data : [];
  }


  isAllSelected: boolean = false;
  selectedInstances: Map<number, boolean>;

  get numberOfInstanceSelected(): number {
    return this.selectedInstances.size;
  }

  get selectedInstance(): IScheduleInstance {
    return this.instances.filter(x => x.isSelected)[0];
  }

  get selectedItemsHaveSameStatus(): boolean {
    if (this.numberOfInstanceSelected == 0)
      return false;;
    return this.instances.filter(x => x.isSelected)
      .every(x => x.status == this.selectedInstance.status);
  }

  get collectVisible(): boolean {
    if (this.numberOfInstanceSelected == 0)
      return false;
    return this.selectedInstance ? this.selectedInstance.status == InstanceStatus.Pending : false;
  }
  get showComplete(): boolean {
    if (this.numberOfInstanceSelected == 0)
      return false;
    return this.selectedInstance ? this.selectedInstance.status == InstanceStatus.Running : false;
  }
  constructor(
    private scheduleService: BeneficiaryScheduleService,
    private toastrService: ToastrService,
    private modalService: ModalService,
    private scheduleInstanceService: ScheduleInstanceService
  ) {
    this.instanceStatusEnum = InstanceStatus
    this.selectedInstances = new Map();
  }
  ngOnChanges(changes: SimpleChanges): void {    
    if (changes.pagedInstances.currentValue) {
      this.paginationConfig.totalItems = changes.pagedInstances.currentValue.total;
      this.applySelection();
    }

  }
  
  applySelection(){
    this.instances.forEach(x=> {
      if(this.selectedInstances.has(x.id)){
        x.isSelected = true;
      }
    })
  }

  async onPageChange(pageNo: number) {
    this.getPage.emit(pageNo);
    this.paginationConfig.currentPage = pageNo;
  }

  onPageSizeChanged(pageSize: number){
    this.paginationConfig.itemsPerPage = pageSize;
    this.pageSizeChangedEvent.emit(pageSize);
  }


  selectAll(event) {
    this.instances.forEach(x => x.isSelected = event);
    this.isAllSelected = event;

    if (event) {
      this.instances.forEach(x => this.selectedInstances.set(x.id, true));
    } else {
      this.instances.forEach(x => this.selectedInstances.delete(x.id));
    }
  }

  toggleSelection(instance: IScheduleInstance) {
    if (instance.isSelected) {
      this.selectedInstances.set(instance.id, true);
    } else {
      this.selectedInstances.delete(instance.id);
    }
    if (this.selectedInstances.size != this.instances.length) {
      this.isAllSelected = false;
    } else {
      this.isAllSelected = true;
    }
  }

  onStartCollection() {
    this.instances.filter(x => x.isSelected)
      .forEach(instance => {
        this.scheduleService.startCollection({ instanceId: instance.id },this.entityType)
          .then(x => {
            this.toastrService.success(MessageConstant.SaveSuccessful);
            instance.status = InstanceStatus.Running;
            this.deselectAll();
          });
      });
  }
  
  deselectAll(){
    this.instances.filter(x=> x.isSelected)
    .forEach(x=> x.isSelected = false);
    this.selectedInstances = new Map();
  }

  editInstance() {
    let updateModel: IScheduleInstanceUpdateModel;
    this.modalService.textInput("Edit Reporting cycle", "Title", this.selectedInstance.title)
      .then((title: string) => {
        if (!title)
          return;
        updateModel = {
          id: this.selectedInstance.id,
          scheduleId: this.selectedInstance.scheduleId,
          title: title
        };
        return this.scheduleInstanceService.updateInstance(updateModel,this.entityType);
      })
      .then(x => {
        this.selectedInstance.title = updateModel.title;
        this.toastrService.success(MessageConstant.SaveSuccessful);
      })
      .catch(x => x);
  }

  completeInstance() {
    this.scheduleService.completeInstance(this.selectedInstance.id)
      .then(x => {
        this.toastrService.success(MessageConstant.InstanceComplete);
        this.instances.filter(x => x.id == this.selectedInstance.id).map(a => a.status = InstanceStatus.Completed);
        
        if(this.instances.filter(x=> x.status != InstanceStatus.Completed).length == 0){
          this.allCompleted.emit();          
        }
        this.deselectAll();
      });

  }
}
