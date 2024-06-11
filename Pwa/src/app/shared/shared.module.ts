import { CalenderIcon } from './svg-icons/calender-icon.component';
import { EditIconComponent } from './svg-icons/edit-icon.component';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FwCellEditorComponent } from '../components/fw-cell-editor/fw-cell-editor.component';
import { CommonModule } from '@angular/common';

import { RouterModule } from '@angular/router';
import { FacilityInstanceComponent } from '../components/Facility/instance/facility-instance.component';
import { FacilityListComponent } from '../components/Facility/all-facility/facility-list.component';
import { FacilityIndComponent } from '../components/Facility/indicator/facility-indicator.component';
import { BeneficiaryIndicatComponent } from '../components/Benefciary/indicator/beneficiary-indicator.component';
import { BeneficiaryStatComponent } from '../components/Benefciary/status-table/beneficiary-status.component';
import { LoadingScreenComponent } from '../components/loading-screen/loading-screen.component';
import { ConfirmModalComponent } from '../components/confirm-modal/confirm-modal.component';
import { ParagraphInputComponent } from './components/modal-inputs/paragraph-input.component';
import { TextInputComponent } from './components/modal-inputs/text-input.component';
import { BackButtonComponent } from './back-button/back-button.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { CreateBenComponent } from '../components/Benefciary/create-beneficiary/create-beneficiary.component';

@NgModule({
    declarations: [
        CalenderIcon,EditIconComponent,FwCellEditorComponent,FacilityInstanceComponent,FacilityListComponent,FacilityIndComponent,
        FacilityInstanceComponent,BeneficiaryIndicatComponent,BeneficiaryStatComponent,LoadingScreenComponent,ConfirmModalComponent,
        ParagraphInputComponent,TextInputComponent, BackButtonComponent,CreateBenComponent
    ],
    imports: [
        FormsModule,NgbModule,ReactiveFormsModule,CommonModule,RouterModule,NgxPaginationModule
    ],
    providers:[
       
    ],
    exports:[
        FwCellEditorComponent,FacilityInstanceComponent,FacilityListComponent,FacilityIndComponent,FacilityInstanceComponent,
        BeneficiaryIndicatComponent,BeneficiaryStatComponent,LoadingScreenComponent,ConfirmModalComponent,ParagraphInputComponent,
        TextInputComponent,BackButtonComponent,CreateBenComponent


    ],
    entryComponents:[FwCellEditorComponent,ConfirmModalComponent]
    
})
export class SharedModule { }
