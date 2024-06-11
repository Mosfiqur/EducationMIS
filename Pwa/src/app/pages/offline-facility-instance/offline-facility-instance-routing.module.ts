import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AllFacilityComponent } from './facility/all-facility.component';
import { FacilityIndicatorComponent } from './indicator/facility-indicator.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
},
{ path: '', redirectTo: 'home', pathMatch: 'full' },
{
  path: 'facility/:collectFor/:id',
  component: AllFacilityComponent,
},
{
  path: 'indicator/:instanceId/:facilityId',
  component: FacilityIndicatorComponent,
},
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OfflineFacilityInstanceRoutingModule { }
