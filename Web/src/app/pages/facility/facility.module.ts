import { NgModule } from '@angular/core';
import { FacilityRoutingModule } from "./facility-routing.modules";

import { HomeComponent } from "./home/home.component";
import { IndicatorsComponent } from "./indicators/indicators.component";
import { ScheduleComponent } from "./schedule/schedule.component";
import { ScheduleinstanceComponent } from "./scheduleinstance/scheduleinstance.component";
import { ApproveComponent } from "./approve/approve.component";

import { SingleFacilityEditComponent } from './single-facility-edit/single-facility-edit.component';
import { ChangeIndicatorsOrderComponent } from './change-indicators-order/change-indicators-order.component';

import { SharedModule } from 'src/app/shared/shared.module';

import { ColumnViewCreateComponent } from './column-view-create/column-view-create.component';
import { ColumnViewEditComponent } from './column-view-edit/column-view-edit.component';
import { ListOfFacilityComponent } from './list-of-facility/listOfFacility.component';

import { FacilityFilterTextComponent } from 'src/app/components/facility/facilityFilterText.component';
import { FacilityFilterComponent } from 'src/app/components/facility/facilityFilter.component';




export const FACILITY_COMPONENTS = [

  HomeComponent,
  IndicatorsComponent,
  ScheduleComponent,
  ScheduleinstanceComponent,
  ApproveComponent,
  ColumnViewCreateComponent,
  ColumnViewEditComponent,
  ListOfFacilityComponent
];
@NgModule({
  declarations: [
    FACILITY_COMPONENTS,    
    SingleFacilityEditComponent,    
    ChangeIndicatorsOrderComponent,    
    FacilityFilterComponent,
    FacilityFilterTextComponent    
  ],
  providers: [],
  imports: [   
    SharedModule,
    FacilityRoutingModule, 
    //AgGridModule        
     ] 
})
export class FacilityModule { }
