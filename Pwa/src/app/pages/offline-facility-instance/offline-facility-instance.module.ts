import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OfflineFacilityInstanceRoutingModule } from './offline-facility-instance-routing.module';
import { HomeComponent } from './home/home.component';
import { AllFacilityComponent } from './facility/all-facility.component';
import { FacilityIndicatorComponent } from './indicator/facility-indicator.component';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgxPaginationModule } from 'ngx-pagination';


@NgModule({
  declarations: [HomeComponent, AllFacilityComponent, FacilityIndicatorComponent],
  imports: [
    CommonModule,
    OfflineFacilityInstanceRoutingModule,
    SharedModule, FormsModule,
    ReactiveFormsModule,NgxPaginationModule
  ]
})
export class OfflineFacilityInstanceModule { }
