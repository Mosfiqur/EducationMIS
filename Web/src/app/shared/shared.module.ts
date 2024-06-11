import { NgModule } from '@angular/core';
import { ConfirmModalComponent } from '../components/confirm-modal/confirm-modal.component';
import { ModalService } from '../services/modal.service';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { InstanceStatusPipe } from '../_pipe/instance-status.pipe';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { NgxPaginationModule } from 'ngx-pagination';
import { ScheduleCreationComponent } from './components/schedule-creation/schedule-creation.component';
import { ScheduleinstanceListComponent } from './components/schedule-instance-list/schedule-instance-list.component';
import { AgGridModule } from 'ag-grid-angular';
import { AgCustomCheckboxRenderer } from '../components/agGrid/ag-customCheckboxRenderer.component';
import { InstanceStatusComponent } from './components/instance-status/instance-status.component';

import { ListDataTypeViewer } from './components/list-datattype-viewer/list-datatype-viewer.component';
import { ListEditorComponent } from './components/list-datatype-editor/list-editor.component';
import { DynamicColumnComponent } from './components/dynamic-column/dynamic-column.component';
import { InstanceSelectorComponent } from './components/instance-selector/instance-selector.component';

import { AgCustomCheckboxEditor } from '../components/agGrid/ag-customCheckboxEditor.component';
import { AgCustomRadioButtonRenderer } from '../components/agGrid/ag-customRadioButtonRenderer.component';
import { AgCustomRadioButtonEditor } from '../components/agGrid/ag-customRadioButtonEditor.component';
import { ParagraphInputComponent } from './components/modal-inputs/paragraph-input.component';
import { ObjectiveIndicatorComponent } from './components/objective-indicator/objective-indicator.component';
import { AutoCompleteComponent } from './components/ngb-type-ahead/auto-complete.component';
import { CalenderIcon } from './components/svg-icons/calender-icon.component';
import { BooleanPipe } from '../_pipe/boolean.pipe';
import { GenderPipe } from '../_pipe/gender.pipe';
import { MonthPipe } from '../_pipe/month.pipe';
import { InstanceModalTriggerDirective } from './components/instance-selector/instanceModalTrigger.directive';

import { DragDropModule } from "@angular/cdk/drag-drop";
import { NgxDaterangepickerMd } from 'ngx-daterangepicker-material';
import { CampSelectorComponent } from './components/camp-selector/camp-selector.component';
import { FacilitySelectorComponent } from './components/facility-selector/facility-selector.component';

import { IMportResultComponent } from './components/import-result/import-result.component';
import { EditIconComponent } from './components/svg-icons/edit-icon.component';
import { TextInputComponent } from './components/modal-inputs/text-input.component';
import { PartnerSelectorComponent } from './components/partner-selector/partner-selector.component';
import { ObjectiveIndicatorSelectorComponent } from './components/objective-indicator-selector/objective-indicator-selector.component';
import { EntityIndicatorSelectorComponent } from './components/entity-indicator-selector/entity-indicator-selector.component';
import { ChangePasswordComponent } from './components/change-password/change-password.component';
import { ForgotPasswordComponent } from './components/forgot-password/forgot-password.component';
import { ResetUserPasswordComponent } from './components/reset-password/reset-user-password.component';

import {HighchartsChartModule, HighchartsChartComponent } from 'highcharts-angular';
import { JrpParameterInfoComponent } from '../components/jrpParameterInfo/jrpParameterInfo.component';
import { EmbeddedDashboardEntryComponent } from '../components/embeddedDashboard/embeddedDashboard.component';
import { PageSizeSelectorComponent } from './components/page-size-selector/page-size-selector.component';
import { TeacherSelectorComponent } from './components/teacher-selector/teacher-selector.component';
import { NgMultiSelectDropDownModule } from '../lib/multi-select';
import { EspSelectorComponent } from './components/esp-selector/esp-selector.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { dateFormatPipe } from '../_pipe/dateFormat.pipe';

import { FwColumnEditorComponent } from '../components/fw-column-editor/fw-column-editor.component';
import { FwCellEditorComponent } from '../components/fw-cell-editor/fw-cell-editor.component';
import { FwCellViewerComponent } from '../components/fw-cell-viewer/fw-cell-viewer.component';


import { NgxListTypeCellRenderer } from '../components/ngxDataGrid/list-type-cell-renderer.component';
import { ListTypeCellRendererDirective } from '../components/ngxDataGrid/list-type-cell-renderer.directive';
import { NgxDatatableModule } from '../lib/ngx-datatable';
import { AddDynamicColumnComponent } from './components/add-dynamic-column/add-dynamic-column.component';
import { AssignTeacherComponent } from './components/assign-teacher/assign-teacher.component';
import { GridLegendComponent } from './components/grid-legend/grid-legend.component';


let icons = [
    EditIconComponent,
    CalenderIcon
];

@NgModule({
    declarations: [
        ConfirmModalComponent,
        InstanceStatusPipe,
        ScheduleCreationComponent,
        ScheduleinstanceListComponent,
        AgCustomCheckboxRenderer,
        AgCustomCheckboxEditor,
        AgCustomRadioButtonEditor,
        AgCustomRadioButtonRenderer,
        InstanceStatusComponent,
        ListEditorComponent,
        ListDataTypeViewer,
        DynamicColumnComponent,
        ParagraphInputComponent,
        ObjectiveIndicatorComponent,
        AutoCompleteComponent,     
        BooleanPipe,
        GenderPipe,
        MonthPipe,     
        dateFormatPipe,   
        DynamicColumnComponent,
        InstanceSelectorComponent,
        InstanceModalTriggerDirective,
        IMportResultComponent,
		TextInputComponent,
		CampSelectorComponent,
        FacilitySelectorComponent,      
        ...icons,
        PartnerSelectorComponent,
        ObjectiveIndicatorSelectorComponent,
        EntityIndicatorSelectorComponent,
        JrpParameterInfoComponent,
        
        ChangePasswordComponent,
        ForgotPasswordComponent,
        ResetUserPasswordComponent
        ,EmbeddedDashboardEntryComponent
        ,PageSizeSelectorComponent
        ,TeacherSelectorComponent,
        EspSelectorComponent,
        AssignTeacherComponent,
        AddDynamicColumnComponent,
        FwColumnEditorComponent,        
        FwCellEditorComponent,
        FwCellViewerComponent,
             
        NgxListTypeCellRenderer,
        ListTypeCellRendererDirective,
        GridLegendComponent
    	],
    imports: [
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        NgxPaginationModule,
        AgGridModule.withComponents([AgCustomCheckboxRenderer, AgCustomCheckboxEditor, AgCustomRadioButtonEditor,
            AgCustomRadioButtonRenderer]),
        DragDropModule,
        NgxDaterangepickerMd.forRoot(),
        NgMultiSelectDropDownModule.forRoot(),
        HighchartsChartModule,
        BsDatepickerModule.forRoot(),
        NgxDatatableModule,
        
    ],
    providers:[ModalService, DatePipe, DecimalPipe, BooleanPipe, dateFormatPipe],        
    bootstrap: [],
    entryComponents: [
        ConfirmModalComponent,
        ScheduleCreationComponent,
        DynamicColumnComponent,
        ParagraphInputComponent,
        ObjectiveIndicatorComponent,
        IMportResultComponent,  
        TextInputComponent,        
        CampSelectorComponent,
        FacilitySelectorComponent,
        PartnerSelectorComponent,
        ObjectiveIndicatorSelectorComponent,
        EntityIndicatorSelectorComponent,
        JrpParameterInfoComponent,
       
        ChangePasswordComponent,
        ForgotPasswordComponent,
        ResetUserPasswordComponent
        ,EmbeddedDashboardEntryComponent
        ,TeacherSelectorComponent,
        EspSelectorComponent,
        AssignTeacherComponent,
        FwColumnEditorComponent,        
        FwCellEditorComponent
    ],
    exports: [
        InstanceStatusPipe,
        NgbModule,
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        NgxPaginationModule,
        ScheduleinstanceListComponent,
        AgGridModule,
        AgCustomCheckboxRenderer,
        AgCustomCheckboxEditor,
        AgCustomRadioButtonEditor,
        AgCustomRadioButtonRenderer,
        InstanceStatusComponent,
        ListEditorComponent,
        ListDataTypeViewer,
        DynamicColumnComponent,
        ParagraphInputComponent,
        ObjectiveIndicatorComponent,
        AutoCompleteComponent,       
        BooleanPipe,
        GenderPipe,
        MonthPipe,
        dateFormatPipe,
        InstanceSelectorComponent,
        InstanceModalTriggerDirective,
        DragDropModule,
        NgMultiSelectDropDownModule,
        IMportResultComponent,
        TextInputComponent,
        ...icons,
        NgxDaterangepickerMd,
        CampSelectorComponent,
        FacilitySelectorComponent,
        PartnerSelectorComponent,
        HighchartsChartModule,        
        JrpParameterInfoComponent,
   
        ChangePasswordComponent,
        ForgotPasswordComponent,
        ResetUserPasswordComponent
        ,EmbeddedDashboardEntryComponent
        ,PageSizeSelectorComponent
        ,TeacherSelectorComponent,
        EspSelectorComponent,
        BsDatepickerModule,
        AssignTeacherComponent,
        AddDynamicColumnComponent,
        FwColumnEditorComponent,        
        FwCellEditorComponent,
        FwCellViewerComponent,
 
        NgxDatatableModule,
        
        NgxListTypeCellRenderer,
        ListTypeCellRendererDirective,
        GridLegendComponent
    ]
})
export class SharedModule { }
