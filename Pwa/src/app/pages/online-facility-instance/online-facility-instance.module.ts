import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OnlineFacilityInstanceRoutingModule } from './online-facility-instance-routing.module';
import { HomeComponent } from './home/home.component';
import { FacilityIndicatorComponent } from './indicator/facility-indicator.component';
import { ColumnDataTypeComponent } from 'src/app/components/columnDataType/column-data-type/column-data-type.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [HomeComponent, FacilityIndicatorComponent,ColumnDataTypeComponent],
  imports: [
    CommonModule,
    OnlineFacilityInstanceRoutingModule,
    SharedModule, FormsModule,ReactiveFormsModule
  ]
})
export class OnlineFacilityInstanceModule { }
