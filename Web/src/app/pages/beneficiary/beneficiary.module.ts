import { NgModule } from '@angular/core';
import { BenefeciaryRoutingModule } from "./beneficiary-routing.modules";
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
import { DragDropModule } from '@angular/cdk/drag-drop';
import { SharedModule } from 'src/app/shared/shared.module';
import { ListOfBeneficiariesComponent } from './list-of-beneficiaries/listOfBeneficiaries.component';
import { BeneficiaryFilterComponent } from 'src/app/components/beneficiary/beneficiaryFilter.component';
import { BeneficiaryFilterTextComponent } from 'src/app/components/beneficiary/beneficiaryFilterText.component';

export const BENEFICIARY_COMPONENTS = [

  HomeComponent,
  IndicatorsComponent,
  ScheduleComponent,
  ScheduleinstanceComponent,
  ApproveComponent,  
  ListOfBeneficiariesComponent
];

@NgModule({
  declarations: [
    BENEFICIARY_COMPONENTS,
    BeneficiaryEditComponent,
    ColumnViewCreateComponent,
    ColumnViewEditComponent,
    SingleFacilityAllBeneficiaryComponent,
    ChangeIndicatorsOrderComponent,    
    BeneficiaryFilterComponent,
    BeneficiaryFilterTextComponent
  ],
  providers: [],
  imports: [
    SharedModule,
    BenefeciaryRoutingModule,
    DragDropModule
  ],
  entryComponents: [

  ]
})
export class BeneficiaryModule { }
