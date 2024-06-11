import { NgxSmartModalService, NgxSmartModalModule } from 'ngx-smart-modal';

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DashboardRoutingModule } from "./dashboard-routing.module";

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HomeComponent } from "./home/home.component";


export const DASHBOARD_COMPONENTS = [

  HomeComponent,
];
@NgModule({
  declarations: [
    DASHBOARD_COMPONENTS,
  ],
  providers: [NgxSmartModalService],
  imports: [FormsModule, ReactiveFormsModule, NgxSmartModalModule.forRoot(),
    DashboardRoutingModule, CommonModule
  ]
})
export class DashboardModule { }
