import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UnicefComponent } from './pages/unicef/unicef.component';
import { AuthGuard } from './security/auth.guard';
import { OnlineBeneficiaryComponent } from './pages/online-beneficiary-instance/online-beneficiary.component';
import { GetAllFacilityComponent } from './pages/online-beneficiary-instance/facility/get-all-facility.component';
import { BeneficiaryStatusComponent } from './pages/online-beneficiary-instance/beneficiary/beneficiary-status.component';
import { BeneficiaryIndicatorComponent } from './pages/online-beneficiary-instance/indicator/beneficiary-indicator.component';
import { CreateBeneficiaryComponent } from './pages/online-beneficiary-instance/create-beneficiary/create-beneficiary.component';

const routes: Routes = [
  {
    path: 'unicefpwa', component: UnicefComponent,
    children: [
      { path: 'dashboard', loadChildren: './pages/dashboard/dashboard.module#DashboardModule', canActivate: [AuthGuard] },
      { path: '', redirectTo: 'unicefpwa/dashboard', pathMatch: 'full', canActivate: [AuthGuard]  },
      { path: 'online-facility-instance', loadChildren: './pages/online-facility-instance/online-facility-instance.module#OnlineFacilityInstanceModule', canActivate: [AuthGuard] },
      { path: 'online-beneficiary-instance',component: OnlineBeneficiaryComponent, canActivate: [AuthGuard] },
      { path: 'facility/:collectFor/:id',component: GetAllFacilityComponent, canActivate: [AuthGuard] },
      { path: 'beneficiary/:instanceId/:facilityId',component: BeneficiaryStatusComponent, canActivate: [AuthGuard] },
      { path: 'online-beneficiary-instance/indicator/:instanceId/:beneficiaryId',component: BeneficiaryIndicatorComponent, canActivate: [AuthGuard] },
      { path: 'online-beneficiary-instance/beneficiary-create/:facilityId/:instanceId', component: CreateBeneficiaryComponent,canActivate:[AuthGuard]},
      { path: 'offline-facility-instance', loadChildren: './pages/offline-facility-instance/offline-facility-instance.module#OfflineFacilityInstanceModule', canActivate: [AuthGuard] },
      { path: 'offline-beneficiary-instance', loadChildren: './pages/offline-beneficiary-instance/offline-beneficiary-instance.module#OfflineBeneficiaryInstanceModule', canActivate: [AuthGuard] },
      { path: 'upload-facility-instance', loadChildren: './pages/upload-facility-instance/upload-facility-instance.module#UploadFacilityInstanceModule', canActivate: [AuthGuard] },
      { path: 'upload-beneficiary-instance', loadChildren: './pages/upload-beneficiary-instance/upload-beneficiary-instance.module#UploadBeneficiaryInstanceModule', canActivate: [AuthGuard] }
    ]
  },
  
  { path: 'unicefpwa/authentication', loadChildren: './pages/auth/auth.module#AuthModule' },
  {
    path: 'login',
    redirectTo: '/authentication/login',
    pathMatch: 'full',
  },
  { path: '', redirectTo: 'unicefpwa/dashboard', pathMatch: 'full' },
  {
    path: 'dashboards',
    redirectTo: 'unicefpwa/dashboard/home',
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
