import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UnicefComponent } from './pages/unicef/unicef.component';
import { AuthGuard } from './security/auth.guard';

const routes: Routes = [
  {
    path: 'unicef', component: UnicefComponent,
    children: [
      { path: 'dashboard', loadChildren: './pages/dashboard/dashboard.module#DashboardModule', canActivate: [AuthGuard] },
      { path: '', redirectTo: 'unicef/dashboard', pathMatch: 'full' },
      { path: 'beneficiary', loadChildren: './pages/beneficiary/beneficiary.module#BeneficiaryModule', canActivate: [AuthGuard] },
      { path: 'facility', loadChildren: './pages/facility/facility.module#FacilityModule' , canActivate: [AuthGuard]},
      { path: 'framework', loadChildren: './pages/framework/framework.module#FrameworkModule', canActivate: [AuthGuard] },
      { path: 'reporting', loadChildren: './pages/reporting/reporting.module#ReportingModule', canActivate: [AuthGuard] },
      { path: 'users', loadChildren: './pages/users/users.module#UsersModule', canActivate: [AuthGuard] },
    ]
  },
  { path: '', redirectTo: 'unicef/dashboard', pathMatch: 'full' },
  { path: 'unicef/authentication', loadChildren: './pages/auth/auth.module#AuthModule' },
  {
    path: 'login',
    redirectTo: '/authentication/login',
    pathMatch: 'full',
  },
  {
    path: 'dashboards',
    redirectTo: 'unicef/dashboard/home',
    pathMatch: 'full',
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
