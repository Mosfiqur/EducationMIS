import { BrowserModule } from '@angular/platform-browser';
import { NgModule ,CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { MDBBootstrapModule } from 'angular-bootstrap-md';
import { HttpModule } from '@angular/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgxSmartModalModule } from 'ngx-smart-modal';
import {NgxPaginationModule} from 'ngx-pagination';

// import { AgGridModule } from 'ag-grid-angular';
// import { AgCustomCheckboxRenderer } from 'src/app/components/agGrid/ag-customCheckboxRenderer.component';
// import { AgCustomCheckboxEditor } from 'src/app/components/agGrid/ag-customCheckboxEditor.component';

import { ToastrModule } from "ngx-toastr";

import { SidebarComponent } from './components/sidebar/sidebar.component';
import { UnicefComponent } from './pages/unicef/unicef.component';

import { AuthGuard } from './security/auth.guard';
import { AuthService } from './services/auth.service';
import { HttpClientService } from './services/httpClientService';
import { SharedModule } from './shared/shared.module';
import { AuthInterceptor } from './_interceptors/auth.interceptor';
import { HttpErrorInterceptor } from './_interceptors/error.interceptor';
import { LoadingSpinnerInterceptor } from './core/loading-spinner/loading-spinner.interceptor';
import { CoreModule } from './core/core.module';
import {JQ_TOKEN} from './services/jQuery.service';
import { LoadingScreenComponent } from './components/loading-screen/loading-screen.component';

let jQuery = window['$'];
import { HighchartsChartModule } from 'highcharts-angular';

@NgModule({
    declarations: [
        AppComponent,
        SidebarComponent,
        UnicefComponent,
        LoadingScreenComponent
        
        // AgCustomCheckboxRenderer,
        // AgCustomCheckboxEditor,
    ],
    imports: [
        BrowserModule,
        CoreModule,
        SharedModule,
        HttpModule,
        AppRoutingModule,
        FormsModule,
        MDBBootstrapModule.forRoot(),
        HttpClientModule,
        ReactiveFormsModule,
        NgxSmartModalModule.forRoot(),
        ToastrModule.forRoot(),
        BrowserAnimationsModule,
        NgxPaginationModule,
        // AgGridModule.withComponents([AgCustomCheckboxRenderer, AgCustomCheckboxEditor])
      HighchartsChartModule
        
    ],
    providers:[
        AuthGuard,
        AuthService,
        HttpClientService,
        { provide: JQ_TOKEN, useValue: jQuery },
        {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
        {provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true},
        {provide: HTTP_INTERCEPTORS, useClass: LoadingSpinnerInterceptor, multi: true},
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
