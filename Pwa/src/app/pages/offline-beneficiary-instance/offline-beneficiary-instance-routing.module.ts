import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BeneficiaryInfoComponent } from './beneficiary/beneficiary-info.component';
import { BeneficiaryIndicatorComponent } from './indicator/beneficiary-indicator.component';
import { CreateBeneficiaryComponent } from './create-beneficiary/create-beneficiary.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
},
{ path: '', redirectTo: 'home', pathMatch: 'full' },
{
  path:'beneficiary/:instanceId/:facilityId',
  component: BeneficiaryInfoComponent
},
{
  path:'indicator/:instanceId/:beneficiaryUniqueId',
  component: BeneficiaryIndicatorComponent
},
{
  path:'beneficiary-create/:facilityId/:instanceId',
  component: CreateBeneficiaryComponent
},
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfflineBeneficiaryInstanceRoutingModule { }
