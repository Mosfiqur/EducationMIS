
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FrameworkRoutingModule } from "./framework-routing.modules";

import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { HomeComponent } from "./home/home.component";
import { BudgetComponent } from "./budget/budget.component";
import { TargetComponent } from "./target/target.component";
import { NewBudgetComponent } from './new-budget/new-budget.component';
import { NewTargetComponent } from './new-target/new-target.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalService } from 'src/app/services/modal.service';
import { ConfirmModalComponent } from 'src/app/components/confirm-modal/confirm-modal.component';
import { FwCellViewerComponent } from 'src/app/components/fw-cell-viewer/fw-cell-viewer.component';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { FwColumnEditorComponent } from 'src/app/components/fw-column-editor/fw-column-editor.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from 'src/app/shared/shared.module';


export const FRAMEWORK_COMPONENTS = [
    
    HomeComponent,
    BudgetComponent,
    TargetComponent    
];
@NgModule({
    declarations: [
        FRAMEWORK_COMPONENTS,
        NewBudgetComponent,
        NewTargetComponent,                
    ],
    providers: [ModalService],
    imports: [FormsModule, 
        ReactiveFormsModule,
        FrameworkRoutingModule, 
        CommonModule,
        SharedModule,
        NgxPaginationModule,
        NgbModule.forRoot()

    ],
    entryComponents: [
 
    ]
})
export class FrameworkModule { }
