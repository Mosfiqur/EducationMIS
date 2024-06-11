import { Component, ElementRef, Inject, Input, ViewChild, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginationInstance } from 'ngx-pagination';
import { ToastrService } from 'ngx-toastr';
import { EmbeddedDashboard } from 'src/app/models/dashboard/embedded-dashboard.model';
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
  selector: 'embedded-dashboard-entry',
  templateUrl: './embeddedDashboard.component.html',
  styleUrls: ['./embeddedDashboard.component.scss']
})
export class EmbeddedDashboardEntryComponent implements OnInit {

  embeddedDashboard: EmbeddedDashboard;

  parameterForm: FormGroup;
  constructor(private activeModal: NgbActiveModal,
    private fb: FormBuilder,
    private dashboardService: DashboardService,
    private toast: ToastrService) {

    this.parameterForm = this.fb.group({
      "id": null,
      "name": ['', [Validators.required]],
      "link": [null, [Validators.required]],
      "isEmbedded":false
    });
  }

  ngOnInit() {
    this.loadParameterInfo();
  }
  
  loadParameterInfo() {
    if (this.embeddedDashboard) {
      this.parameterForm.patchValue({
        id: this.embeddedDashboard.id,
        name: this.embeddedDashboard.name,
        link: this.embeddedDashboard.link,
        isEmbedded: this.embeddedDashboard.isEmbedded
      });
    }
  }

  
    save() {
    if(this.parameterForm.get('id').value){
      this.dashboardService.updateEmbeddedDashboard(this.parameterForm.value).then(a => {
        this.toast.success(MessageConstant.UpdateSuccessful)
        this.closeModal();
      });
    }
    else{
      this.dashboardService.saveEmbeddedDashboard(this.parameterForm.value).then(a => {
        this.toast.success(MessageConstant.SaveSuccessful)
        this.closeModal();
      });
    }
    
  }


  closeModal() {
    this.activeModal.close();
  }


}
