// import { AccountCloserComponent } from './../../components/account/account-closer/account-closer.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from "./home/home.component";
import { BudgetComponent } from "./budget/budget.component";
import { TargetComponent } from "./target/target.component";
import { NewBudgetComponent } from './new-budget/new-budget.component';
import { NewTargetComponent } from './new-target/new-target.component';


// const routes: Routes = [{
//     path: '',
//     component: DashboardComponent,
//     children: [
//         {
//             path: 'home',
//             component: HomeComponent,
//         },
//         { path: '', redirectTo: 'home', pathMatch: 'full' },
//         {
//             path: 'budget',
//             component: BudgetComponent,
//         },
//         {
//             path: 'target',
//             component: TargetComponent,
//         }
//     ],
// }];

const routes: Routes = [
    {
        path: 'home',
        component: HomeComponent,
    },
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    {
        path: 'budget',
        component: BudgetComponent,
    },
    {
        path: 'target',
        component: TargetComponent,
    }
    ,
    {
        path: 'new-budget',
        component: NewBudgetComponent,
    }
    ,
    {
        path: 'budget/:id/edit',
        component: NewBudgetComponent,
    }
    ,
    {
        path: 'new-target',
        component: NewTargetComponent,
    },
    {
        path: 'target/:id/edit',
        component: NewTargetComponent,
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
export class FrameworkRoutingModule {

}
