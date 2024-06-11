// import { AccountCloserComponent } from './../../components/account/account-closer/account-closer.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from "./home/home.component";
import { EmbededComponent } from "./embeded/embeded.component";
import { PredefinedDashbordSelectedComponent } from './predefined-dashbord-selected/predefined-dashbord-selected.component';
import { GapMapComponent } from './gap-map/gap-map.component';
import { LcMapComponent } from './lc-map/lc-map.component';

// const routes: Routes = [{
//   path: '',
//   component: DashboardComponent,
//   children: [
//     {
//       path: 'home',
//       component: HomeComponent,
//     },
//     {path: '', redirectTo: 'home', pathMatch: 'full' },
//     {
//       path: 'embeded',
//       component: EmbededComponent,
//     }
//   ],
// }];
const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
    children: [
      {
        path: '', redirectTo: 'predefined-dashboard-selcted', pathMatch: 'full'
      },
      {
        path: 'predefined-dashboard-selcted',
        component: PredefinedDashbordSelectedComponent,
      },
      {
        path: 'gap-map', component: GapMapComponent
      },
      {
        path: 'lc-map', component: LcMapComponent
      }
    ]
  },
  // {
  //   path: 'predefined-dashboard-selcted',
  //   component: PredefinedDashbordSelectedComponent,
  // },  
  {
    path: 'embeded',
    component: EmbededComponent,
  },
  // {
  //   path: 'gap-map', component: GapMapComponent
  // },
  // {
  //   path: 'lc-map', component: LcMapComponent
  // },
  { path: '', redirectTo: 'home', pathMatch: 'full' },
];

@NgModule({
  imports: [
    RouterModule.forChild(routes),
  ],
  exports: [
    RouterModule,
  ],
})
export class DashboardRoutingModule {

}
