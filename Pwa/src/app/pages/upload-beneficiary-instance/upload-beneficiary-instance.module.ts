import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UploadBeneficiaryInstanceRoutingModule } from './upload-beneficiary-instance-routing.module';
import { HomeComponent } from './home/home.component';
import { FacilityListComponent } from './facility/facility-list.component';
import { BenStatusComponent } from './beneficiary-status/ben-status.component';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [HomeComponent, FacilityListComponent, BenStatusComponent],
  imports: [
    CommonModule,
    UploadBeneficiaryInstanceRoutingModule,
    SharedModule
  ]
})
export class UploadBeneficiaryInstanceModule { }
