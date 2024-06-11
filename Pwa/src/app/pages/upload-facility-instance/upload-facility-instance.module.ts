import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadFacilityInstanceRoutingModule } from './upload-facility-instance-routing.module';
import { HomeComponent } from './home/home.component';
import { FacilityListComponent } from './facility/facility-list.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [HomeComponent, FacilityListComponent,],
  imports: [
    CommonModule,
    UploadFacilityInstanceRoutingModule,
    SharedModule
  ]
})
export class UploadFacilityInstanceModule { }
