import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from "./home/home.component";
import { IndicatorsComponent } from "./indicators/indicators.component";
import { ScheduleComponent } from "./schedule/schedule.component";
import { ScheduleinstanceComponent } from "./scheduleinstance/scheduleinstance.component";
import { ApproveComponent } from "./approve/approve.component";
import { SingleFacilityEditComponent } from './single-facility-edit/single-facility-edit.component';
import { ChangeIndicatorsOrderComponent } from './change-indicators-order/change-indicators-order.component';
import { ColumnViewCreateComponent } from './column-view-create/column-view-create.component';
import { ColumnViewEditComponent } from './column-view-edit/column-view-edit.component';


const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
    },
    { path: '', redirectTo: 'home', pathMatch: 'full' },    
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
    }
    ,{
        path: 'edit/:id/:instanceId',
        component: SingleFacilityEditComponent,
    }
    ,{
        path: 'new/:instanceId',
        component: SingleFacilityEditComponent,
    }
    , 
    {
        path: 'change-indicators-order/:instanceId',
        component: ChangeIndicatorsOrderComponent,
    },
    {
        path: 'column-new',
        component: ColumnViewCreateComponent,
    },
    {
        path: 'column-edit/:id',
        component: ColumnViewEditComponent,
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
export class FacilityRoutingModule {

}
