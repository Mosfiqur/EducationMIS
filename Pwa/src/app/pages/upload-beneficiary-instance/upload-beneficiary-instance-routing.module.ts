import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { BenStatusComponent } from './beneficiary-status/ben-status.component';
import { FacilityListComponent } from './facility/facility-list.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
  },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  {
    path: 'all-facility/:collectFor/:instanceId',
    component: FacilityListComponent,
  },
  {
    path: 'beneficiary-status/:instanceId/:facilityId',
    component: BenStatusComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UploadBeneficiaryInstanceRoutingModule { }
