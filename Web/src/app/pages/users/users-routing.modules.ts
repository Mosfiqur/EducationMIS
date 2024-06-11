// import { AccountCloserComponent } from './../../components/account/account-closer/account-closer.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from "./home/home.component";
import { RolesComponent } from "./roles/roles.component";
import { UserCreateComponent } from './user-create/user-create.component';
import { RolesNewComponent } from './roles-new/roles-new.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { ProfileInfoUpdateComponent } from './profile-info-update/profile-info-update.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

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
//             path: 'roles',
//             component: RolesComponent,
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
        path: 'roles',
        component: RolesComponent,
    },
    {
        path: 'user-new',
        component: UserCreateComponent,
    },
    {
        path: 'user/:id/edit',
        component: UserCreateComponent,
    },

    {
        path: 'roles-new',
        component: RolesNewComponent,
    } ,
    {
        path: 'roles/:id/edit',
        component: RolesNewComponent,
    },
    {
        path: 'my-profile',
        component: ProfileInfoUpdateComponent
    },
    {
        path: 'profile/:id',
        component: UserProfileComponent
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
export class UsersRoutingModule {

}
