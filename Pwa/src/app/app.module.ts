import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { ToastrModule } from "ngx-toastr";


import { SidebarComponent } from './components/sidebar/sidebar.component';
import { UnicefComponent } from './pages/unicef/unicef.component';

import { AuthGuard } from './security/auth.guard';
import { AuthService } from './services/auth.service';
import { HttpClientService } from './services/httpClientService';
import { AuthInterceptor } from './_interceptors/auth.interceptor';
import { HttpErrorInterceptor } from './_interceptors/error.interceptor';
import { LoadingSpinnerInterceptor } from './core/loading-spinner/loading-spinner.interceptor';
import { CoreModule } from './core/core.module';
import { ServiceWorkerModule } from '@angular/service-worker';
import { environment } from '../environments/environment';
import { OnlineBeneficiaryComponent } from './pages/online-beneficiary-instance/online-beneficiary.component';
import { GetAllFacilityComponent } from './pages/online-beneficiary-instance/facility/get-all-facility.component';
import { BeneficiaryStatusComponent } from './pages/online-beneficiary-instance/beneficiary/beneficiary-status.component';
import { NgxIndexedDBModule } from 'ngx-indexed-db';
import { UserDB } from './localdb/UserDB';
import { DBConfiguration } from './localdb/DbConfiguration';
import { BeneficiaryRecordsDB } from './localdb/BeneficiaryRecordsDB';
import { CampDb } from './localdb/CampDb';
import { BlockDb } from './localdb/BlockDb';
import { SubBlockDb } from './localdb/SubBlockDb';
import { BeneficiaryScheduleInstanceDb } from './localdb/BeneficiaryScheduleInstanceDb';
import { FacilityDb } from './localdb/FacilityDb';
import { BeneficiaryDb } from './localdb/BeneficiaryDb';
import { BeneficiaryIndicatorDb } from './localdb/BeneficiaryIndicatorDb';
import { FacilityScheduleInstanceDB } from './localdb/FacilityScheduleInstanceDB';
import { FacilityIndicatorDB } from './localdb/FacilityIndicatorDB';
import { FacilityRecordsDB } from './localdb/FacilityRecordsDB';
import { ListDb } from './localdb/ListDb';
import { ListItemDB } from './localdb/ListItemDB';
import { EditIconComponent } from './shared/svg-icons/edit-icon.component';
import { CalenderIcon } from './shared/svg-icons/calender-icon.component';
import { SharedModule } from './shared/shared.module';
import { IndexedDbService } from './localdb/IndexedDbService';
import { BeneficiaryIndicatorComponent } from './pages/online-beneficiary-instance/indicator/beneficiary-indicator.component';
import { CreateBeneficiaryComponent } from './pages/online-beneficiary-instance/create-beneficiary/create-beneficiary.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { BeneficiaryDataCollectionStatusDb } from './localdb/BeneficiaryDataCollectionStatusDb';
import { FacilityDataCollectionStatusDb } from './localdb/FacilityDataCollectionStatusDb';
import { HttpModule } from '@angular/http';
import { JQ_TOKEN } from './services/jQuery.service';
let jQuery = window['$'];

@NgModule({
    declarations: [
        AppComponent,
        SidebarComponent,
        UnicefComponent,
        OnlineBeneficiaryComponent,
        GetAllFacilityComponent,
        BeneficiaryStatusComponent,
        BeneficiaryIndicatorComponent,
        CreateBeneficiaryComponent
    ],
    imports: [
        BrowserModule,
        CoreModule,
        AppRoutingModule,
        FormsModule,
        HttpModule,
        HttpClientModule,
        ReactiveFormsModule,
        ToastrModule.forRoot(),
        BrowserAnimationsModule, 
        NgxIndexedDBModule.forRoot(DBConfiguration.UnicefDbConfig),   
        ServiceWorkerModule.register('ngsw-worker.js', { enabled: environment.production }),
        SharedModule,
        NgxPaginationModule
    ],
    providers:[
        AuthGuard,
        AuthService,
        HttpClientService, 
        UserDB,  
        BeneficiaryRecordsDB, BeneficiaryScheduleInstanceDb, BeneficiaryIndicatorDb, FacilityScheduleInstanceDB,
        CampDb,BlockDb,SubBlockDb,FacilityDb,BeneficiaryDb,FacilityIndicatorDB,FacilityRecordsDB,ListDb,ListItemDB,
        IndexedDbService,BeneficiaryDataCollectionStatusDb,FacilityDataCollectionStatusDb,
        { provide: JQ_TOKEN, useValue: jQuery },
        {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true},
        {provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true},
        {provide: HTTP_INTERCEPTORS, useClass: LoadingSpinnerInterceptor, multi: true},
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
