
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersRoutingModule } from "./users-routing.modules";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HomeComponent } from "./home/home.component";
import { RolesComponent } from "./roles/roles.component";
import { UserCreateComponent } from './user-create/user-create.component';
import { RolesNewComponent } from './roles-new/roles-new.component';
import { UserEditComponent } from './user-edit/user-edit.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/shared/shared.module';
import { ProfileInfoUpdateComponent } from './profile-info-update/profile-info-update.component';
import { UserProfileComponent } from './user-profile/user-profile.component';

//import {SidebarComponent} from '../../components/sidebar/sidebar.component';

export const REPORTING_COMPONENTS = [
  
  HomeComponent,
  RolesComponent,
  
];
@NgModule({
  declarations: [
    REPORTING_COMPONENTS,
    UserCreateComponent,
    RolesNewComponent,
    UserEditComponent,   
    ProfileInfoUpdateComponent,
    UserProfileComponent
  ],
  providers: [],
  imports: [FormsModule, ReactiveFormsModule, 
    UsersRoutingModule, CommonModule,
    NgxPaginationModule,
    SharedModule
     ]
})
export class UsersModule { }

