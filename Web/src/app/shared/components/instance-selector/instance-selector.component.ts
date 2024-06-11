import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter, ViewEncapsulation, OnChanges, SimpleChanges } from '@angular/core';
import { PaginationInstance } from 'ngx-pagination';
import { ToastrService } from 'ngx-toastr';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'instance-selector-modal',
  templateUrl: './instance-selector.component.html',
  styleUrls: ['./instance-selector.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class InstanceSelectorComponent implements OnInit, OnChanges {

  ngOnChanges(changes: SimpleChanges): void {
    if (changes.selectedInstanceIds) {
      this.returnInstanceIds = changes.selectedInstanceIds.currentValue || [];

      this.lstInstance.map(a => {
        a.Selected = this.checkSelected(a.id);
      })
    }
  }

  @Input() title: string;
  @Input() elementId: string;

  @Input() selectedInstanceIds: InstanceViewModel[];
  @Input() instanceFor: EntityType;
  @Input() instanceStatus: number;
  @Input() isMultivalued: string;
  @Input() value:string;
  @ViewChild('modalcontainer') containerEl: ElementRef;

  @Output() instanceValueChanged = new EventEmitter();
  public p: any;
  public returnInstanceIds: InstanceViewModel[] = [];
  public lstInstance: any[]=[];
  public instancePaginationConfig: PaginationInstance = {
    id: 'instance_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  constructor(@Inject(JQ_TOKEN) private $: any, private instanceService: InstanceService,
  private toast: ToastrService) { }

  ngOnInit() {
    if (this.selectedInstanceIds)
      this.returnInstanceIds = this.selectedInstanceIds;
    this.loadInstance(this.instancePaginationConfig.currentPage, this.instancePaginationConfig.itemsPerPage);

    this.$(this.elementId).modal({
      backdrop: 'static',
      keyboard: false
    });

  }

  loadInstance(pageNumber, pageSize) {
    this.instanceService
      .getInstanceStatusWise(this.instanceFor, this.instanceStatus, pageSize, pageNumber)
      .then(a => {
        this.lstInstance = a.data;
        this.lstInstance.map(a => {

          a.Selected = this.checkSelected(a.id);
        })
        this.instancePaginationConfig.totalItems = a.total;
        this.instancePaginationConfig.currentPage = pageNumber;
      })
  }
  checkSelected(id) {

    for (var i = 0; i < this.returnInstanceIds.length; i++) {
      if (this.returnInstanceIds[i].id == id) {
        return true;
      }
    }
    return false;
  }
  isInstanceRequired(instance){
    let checkedValues:InstanceViewModel[]=[]
    Object.assign(checkedValues,this.returnInstanceIds);
    let ind= checkedValues.indexOf(instance);
    checkedValues.splice(ind,1);
    if(this.value==="required" && checkedValues.length==0){
     return true;
    }
    else{
      return false;
    }
  }
  checkbox_clicked(row, instance) {
    // console.log(row.currentTarget.getElementsByTagName('input')[0]);
    var event = row.currentTarget.getElementsByTagName('input')[0];
    event.checked = !event.checked;

    if (!event.checked) {
     
      if(this.isInstanceRequired(instance)){
        this.toast.error("Must have a value");
        event.checked=true;
        return;
      }

      var ind = this.returnInstanceIds.indexOf(instance);
      this.returnInstanceIds.splice(ind, 1)
      //this.lstInstance[this.lstInstance.indexOf(instance)].Selected=false;
      this.reloadCheck();
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnInstanceIds.push(instance);
      //this.lstInstance[this.lstInstance.indexOf(instance)].Selected=true;
      this.reloadCheck();
    }
    else {
      this.returnInstanceIds = [instance];
      this.reloadCheck();
      // this.lstInstance[this.lstInstance.indexOf(instance)].Selected=true;
    }
  }

  checkbox_changed(event, instance) {
    event.preventDefault();
    if (!event.target.checked) {
      var ind = this.returnInstanceIds.indexOf(instance);
      this.returnInstanceIds.splice(ind, 1)
      //this.lstInstance[this.lstInstance.indexOf(instance)].Selected=false;
      this.reloadCheck();
      return;
    }
    if (this.isMultivalued === "true") {
      this.returnInstanceIds.push(instance);
      //this.lstInstance[this.lstInstance.indexOf(instance)].Selected=true;
      this.reloadCheck();
    }
    else {
      this.returnInstanceIds = [instance];
      this.reloadCheck();
      // this.lstInstance[this.lstInstance.indexOf(instance)].Selected=true;
    }
  }

  reloadCheck() {
    this.lstInstance.map(a => {

      a.Selected = this.checkSelected(a.id);
    })
  }

  InstanceStatusText(id) {
    return InstanceStatus[id];
  }

  pageChangedInstance(event) {
    this.loadInstance(event, this.instancePaginationConfig.itemsPerPage);
  }

  closeModal() {
    this.$(this.containerEl.nativeElement).modal('hide');
  }
  confirm() {
    this.instanceValueChanged.emit(this.returnInstanceIds);
    this.closeModal();
  }
}
