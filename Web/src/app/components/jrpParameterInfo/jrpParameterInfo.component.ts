import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { ToastrService } from 'ngx-toastr';
import { IJrpParameterInfo } from 'src/app/models/dashboard/jrp-parameter-info.model';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { IObjectiveIndicator } from 'src/app/models/frameworks/objective-indicator.model';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { DashboardService } from 'src/app/services/dashboard.service';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { InstanceService } from 'src/app/services/instance.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';
import { ModalService } from 'src/app/services/modal.service';
import { EntityIndicatorSelectorComponent } from 'src/app/shared/components/entity-indicator-selector/entity-indicator-selector.component';
import { ObjectiveIndicatorSelectorComponent } from 'src/app/shared/components/objective-indicator-selector/objective-indicator-selector.component';
import { ObjectiveIndicatorComponent } from 'src/app/shared/components/objective-indicator/objective-indicator.component';
import { Convert } from 'src/app/utility/Convert';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceStatus } from 'src/app/_enums/instance-status';

@Component({
  selector: 'jrp-parameter-info',
  templateUrl: './jrpParameterInfo.component.html',
  styleUrls: ['./jrpParameterInfo.component.scss']
})
export class JrpParameterInfoComponent implements OnInit {

  jrpParameterInfo: IJrpParameterInfo;

  target: IObjectiveIndicator[] = [];
  indicator: IDynamicColumn[] = [];
  public formulaList: ISelectListItem[] = [];
  parameterForm: FormGroup;
  constructor(private activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private dashboardService: DashboardService,
    private modalService: ModalService,
    private dynamicColumnService: DynamicColumnService,
    private toast: ToastrService) {

    this.parameterForm = this.fb.group({
      "id": null,
      "name": ['', [Validators.required]],

      "targetId": [null, [Validators.required]],
      "indicatorId": [null, [Validators.required]],
    });
  }

  ngOnInit() {
    this.loadParameterInfo();
  }
  loadParameterInfo() {
    if (this.jrpParameterInfo) {
      this.parameterForm.patchValue({
        id: this.jrpParameterInfo.id,
        name: this.jrpParameterInfo.name,
        targetId: this.jrpParameterInfo.targetId,
        indicatorId: this.jrpParameterInfo.indicatorId
      });
      this.target.push(<IObjectiveIndicator>{});
      this.indicator.push(<IDynamicColumn>{});
      Object.assign(this.target[0], { id: this.jrpParameterInfo.targetId, indicator: this.jrpParameterInfo.targetName });
      Object.assign(this.indicator[0], { id: this.jrpParameterInfo.indicatorId, name: this.jrpParameterInfo.indicatorName });

    }
  }
  selectTarget() {
    let selectedId: IObjectiveIndicator[] = [];
    Object.assign(selectedId, this.target);
    this.modalService.open<ObjectiveIndicatorSelectorComponent, IObjectiveIndicator[]>
      (ObjectiveIndicatorSelectorComponent, { selectedIndicatorIds: selectedId, isMultivalued: 'false' })
      .then(column => {
        this.target = column;
        this.parameterForm.controls['targetId'].patchValue(this.target[0].id);
      })
  }
  selectIndicator() {
    let selectedId: IDynamicColumn[] = [];
    Object.assign(selectedId, this.indicator);
    this.modalService.open<EntityIndicatorSelectorComponent, IDynamicColumn[]>
      (EntityIndicatorSelectorComponent, { selectedIndicatorIds: selectedId, isMultivalued: 'false' })
      .then(column => {
        this.indicator = column
        this.parameterForm.controls['indicatorId'].patchValue(this.indicator[0].id);
      })



  }
  setFormula(value) {
    this.parameterForm.controls['formula'].patchValue(value);

  }
  getTargetText() {
    if (this.target.length > 0) {
      return this.target[0].indicator;
    }
    else {
      return "No target selected";
    }
  }
  getIndicatorText() {
    if (this.indicator.length > 0) {
      return this.indicator[0].name;
    }
    else {
      return "No indicator selected";
    }
  }
  save() {
    if(this.parameterForm.get('id').value){
      this.dashboardService.updateJrpParameterInfo(this.parameterForm.value).then(a => {
        this.toast.success(MessageConstant.UpdateSuccessful)
        this.closeModal();
      });
    }
    else{
      this.dashboardService.saveJrpParameterInfo(this.parameterForm.value).then(a => {
        this.toast.success(MessageConstant.SaveSuccessful)
        this.closeModal();
      });
    }
    
  }


  closeModal() {
    this.activeModal.close();
  }


}
