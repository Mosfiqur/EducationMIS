
import { NgModule, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReportingRoutingModule } from "./reporting-routing.modules";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HomeComponent } from "./home/home.component";
import { FiveWReportComponent } from './five-w-report/five-w-report.component';

import { CampWiseReportComponent } from './camp-wise-report/camp-wise-report.component';
import { DuplicationReportComponent } from './duplication-report/duplication-report.component';
import { GapAnalysisReportComponent } from './gap-analysis-report/gap-analysis-report.component';
import { SummaryReportComponent } from './summary-report/summary-report.component';
import { DamageReportComponent } from './damage-report/damage-report.component';
import { SharedModule } from 'src/app/shared/shared.module';

//import {SidebarComponent} from '../../components/sidebar/sidebar.component';

export const REPORTING_COMPONENTS = [
  
  HomeComponent,
  
];
@NgModule({
  declarations: [
    REPORTING_COMPONENTS,
    FiveWReportComponent,

    CampWiseReportComponent,
    DuplicationReportComponent,
    GapAnalysisReportComponent,
    SummaryReportComponent,
    DamageReportComponent
  ],
  providers: [],
  imports: [FormsModule, ReactiveFormsModule, 
    ReportingRoutingModule, CommonModule,
    SharedModule
     ]
})
export class ReportingModule implements OnInit{

  constructor(){

  }
  ngOnInit() {

  }

  


 }

