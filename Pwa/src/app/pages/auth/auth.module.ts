import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AuthRoutingModule } from './auth-routing.module';
import { AuthComponent } from './auth.component';
import { LoginComponent } from './login/login.component';
import { ArchwizardModule } from 'angular-archwizard';
//https://www.npmjs.com/package/angular-archwizard
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [AuthComponent, LoginComponent],
  imports: [
    CommonModule,
    AuthRoutingModule,
    ArchwizardModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class AuthModule { }
