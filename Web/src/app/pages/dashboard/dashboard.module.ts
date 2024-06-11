import { NgxSmartModalService, NgxSmartModalModule } from 'ngx-smart-modal';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from "./dashboard-routing.module";

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HomeComponent } from "./home/home.component";
import { EmbededComponent } from "./embeded/embeded.component";
import { PredefinedDashbordSelectedComponent } from './predefined-dashbord-selected/predefined-dashbord-selected.component';
import { GapMapComponent } from './gap-map/gap-map.component';
import { LcMapComponent } from './lc-map/lc-map.component';

import { SharedModule } from 'src/app/shared/shared.module';
import { JrpFilterComponent } from 'src/app/components/dashboard/jrpFilter.component';
import { NgMultiSelectDropDownModule } from 'src/app/lib/multi-select';
// import { JrpFilterComponent } from 'src/app/components/dashboard/jrpFilter.component';

// import { DashboardComponent } from "../dashboard/dashboard.component";
// import {SidebarComponent} from '../../components/sidebar/sidebar.component';


export const DASHBOARD_COMPONENTS = [

  HomeComponent,
  EmbededComponent,
  GapMapComponent,
  LcMapComponent
];
@NgModule({
  declarations: [
    DASHBOARD_COMPONENTS,
    PredefinedDashbordSelectedComponent,
    JrpFilterComponent
  ],
  providers: [NgxSmartModalService],
  imports: [FormsModule, ReactiveFormsModule, NgxSmartModalModule.forRoot(),
    DashboardRoutingModule, CommonModule,
    NgMultiSelectDropDownModule.forRoot(),
    SharedModule

  ]
})
export class DashboardModule { }
