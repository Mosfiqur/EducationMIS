// import { AccountCloserComponent } from './../../components/account/account-closer/account-closer.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CampWiseReportComponent } from './camp-wise-report/camp-wise-report.component';
import { DamageReportComponent } from './damage-report/damage-report.component';
import { DuplicationReportComponent } from './duplication-report/duplication-report.component';
import { FiveWReportComponent } from './five-w-report/five-w-report.component';
import { GapAnalysisReportComponent } from './gap-analysis-report/gap-analysis-report.component';
import { HomeComponent } from "./home/home.component";
import { SummaryReportComponent } from './summary-report/summary-report.component';

// const routes: Routes = [{
//     path: '',
//     component: DashboardComponent,
//     children: [
//         {
//             path: 'home',
//             component: HomeComponent,
//         },
//         { path: '', redirectTo: 'home', pathMatch: 'full' }
//     ],
// }];
const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
        
        children: [
            {
                path: 'five-w-report', component: FiveWReportComponent
            },
            {
                path: 'camp-wise-report', component: CampWiseReportComponent
            },
            {
                path: 'duplication-report', component: DuplicationReportComponent
            },
            {
                path: 'gap-analysis-report', component: GapAnalysisReportComponent
            },
            {
                path: 'damage-report', component: DamageReportComponent
            },
            {
                path: 'summary-report', component: SummaryReportComponent
            },
        ]
    },
    
    { path: '', redirectTo: 'home/five-w-report', pathMatch: 'full' }
    
];
@NgModule({
    imports: [
        RouterModule.forChild(routes),
    ],
    exports: [
        RouterModule,
    ],
})
export class ReportingRoutingModule {

}
