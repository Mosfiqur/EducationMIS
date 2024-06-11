import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OfflineBeneficiaryInstanceRoutingModule } from './offline-beneficiary-instance-routing.module';
import { HomeComponent } from './home/home.component';
import { BeneficiaryInfoComponent } from './beneficiary/beneficiary-info.component';
import { BeneficiaryIndicatorComponent } from './indicator/beneficiary-indicator.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateBeneficiaryComponent } from './create-beneficiary/create-beneficiary.component';

@NgModule({
  declarations: [HomeComponent, BeneficiaryInfoComponent, BeneficiaryIndicatorComponent, CreateBeneficiaryComponent],
  imports: [
    CommonModule,
    OfflineBeneficiaryInstanceRoutingModule,
    SharedModule, FormsModule, ReactiveFormsModule
  ]
})
export class OfflineBeneficiaryInstanceModule { }
