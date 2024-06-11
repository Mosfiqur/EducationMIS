// import { AccountCloserComponent } from './../../components/account/account-closer/account-closer.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from "./home/home.component";
import { IndicatorsComponent } from "./indicators/indicators.component";
import { ScheduleComponent } from "./schedule/schedule.component";
import { ScheduleinstanceComponent } from "./scheduleinstance/scheduleinstance.component";
import { ApproveComponent } from "./approve/approve.component";


import { BeneficiaryEditComponent } from './beneficiary-edit/beneficiary-edit.component';
import { ColumnViewCreateComponent } from './column-view-create/column-view-create.component';
import { ColumnViewEditComponent } from './column-view-edit/column-view-edit.component';
import { SingleFacilityAllBeneficiaryComponent } from './single-facility-all-beneficiary/single-facility-all-beneficiary.component';
import { ChangeIndicatorsOrderComponent } from './change-indicators-order/change-indicators-order.component';

import {ListOfBeneficiariesComponent} from './list-of-beneficiaries/listOfBeneficiaries.component';

const routes: Routes = [{
    path: 'home',
    component: HomeComponent,
},
{ path: '', redirectTo: 'home', pathMatch: 'full' },
{
    path: 'all',
    component: ListOfBeneficiariesComponent,
},
{
    path: 'indicators',
    component: IndicatorsComponent,
},
{
    path: 'indicators/:instanceId',
    component: IndicatorsComponent,
},
{
    path: 'schedule',
    component: ScheduleComponent,
},
{
    path: 'scheduleinstance',
    component: ScheduleinstanceComponent,
},
{
    path: 'approve',
    component: ApproveComponent,
},
{
    path: 'edit/:id/:instanceId',
    component: BeneficiaryEditComponent,
},
{
    path: 'column-new',
    component: ColumnViewCreateComponent,
},
{
    path: 'column-edit/:id',
    component: ColumnViewEditComponent,
}
,
{
    path: 'aprove-single-facility',
    component: SingleFacilityAllBeneficiaryComponent,
}
,
{
    path: 'change-indicators-order/:instanceId',
    component: ChangeIndicatorsOrderComponent,
}
];
@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ],
    exports: [
        RouterModule,
    ],
})
export class BenefeciaryRoutingModule {

}
