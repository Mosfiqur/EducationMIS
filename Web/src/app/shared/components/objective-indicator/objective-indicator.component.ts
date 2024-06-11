import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { Globals } from 'src/app/globals';
import { Camp } from 'src/app/models/common/camp.model';
import { EducationSectorPartner } from 'src/app/models/educationSectorPartner/educationSectorPartner';
import { IObjectiveIndicator } from 'src/app/models/frameworks/objective-indicator.model';
import { IReportingFrequency } from 'src/app/models/frameworks/reporting-frequency.model';
import { CommonService } from 'src/app/services/common.service';
import { EducationPartnerService } from 'src/app/services/educationPartner.service';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { CustomValidators } from 'src/app/utility/CustomValidators';
import { DateFactory } from 'src/app/utility/DateFactory';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';

@Component({
    templateUrl: './objective-indicator.component.html'
})
export class ObjectiveIndicatorComponent implements OnInit, OnDestroy{
    
    public indicator: IObjectiveIndicator ;
    public form: FormGroup;
    public validationErrors: any = {};
    private subscriptions: Map<string, Subscription> = new Map();
    public organizations: EducationSectorPartner[] = [];
    public frequencies: IReportingFrequency[] = [];
    constructor(
        private fb: FormBuilder, 
        private global: Globals,
        private activeModal: NgbActiveModal,
        private commonService: CommonService,
        private espService: EducationPartnerService
        ){

    }

    get dateRange(): AbstractControl {
        return this.form.get('dateRange');
    }
    
    ngOnInit(): void {
        this.buildForm();
        // Here we use setTimeout to prevent the console error: Expression has changed after it was checked.
        setTimeout(() => {
            this.espService.getAll()
            .then(x => {
                this.organizations = x;
            });
            this.commonService.getAllReportingFrequencies()
            .then(x => {
                this.frequencies = x.data;
            });
        }, 0);        
    }

    buildForm(){
        let dateRange = this.fb.group({
            startDate: ["", Validators.required],
            endDate: ["", Validators.required], // Validate range
        },{validators: CustomValidators.dateRangeValidator('startDate', 'endDate')});

        this.form = this.fb.group({
            indicator: ["", [Validators.required, Validators.minLength(5), Validators.maxLength(512)]],
            unit: ["", [Validators.required, Validators.minLength(5), Validators.maxLength(512)]],
            baseLine: ["", [Validators.required, Validators.min(1), Validators.max(this.global.maxInt)]],
            target: ["", [Validators.required, Validators.min(1), Validators.max(this.global.maxInt)]],
            dateRange: dateRange,
            organizationId: ["", [Validators.required, Validators.min(1), Validators.max(this.global.maxInt)]],
            reportingFrequencyId: ["", [Validators.required, Validators.min(1), Validators.max(this.global.maxInt)]],
        });

        if(this.indicator && this.indicator.id){
            this.form.patchValue({...this.indicator});
            this.dateRange.patchValue({
                startDate: DateFactory.toNgbDateStruct(this.indicator.startDate.toString()),
                endDate: DateFactory.toNgbDateStruct(this.indicator.endDate.toString())
            })
        }
        this.subscriptions = 
        new ValidationErrorBuilder()
        .withGroup(this.form)
        .withGroup(dateRange)
        .withMessages(this.getValidationMessages())
        .withCustomValidationMessages({
            invalidDateRange: ValidationMessage.invalidDateRange()
          })
        .useMessageContainer(this.validationErrors)      
        .build();
    }
    

    getValidationMessages(){
        let minMax = {
            min: ValidationMessage.min(1),
            max: ValidationMessage.max(this.global.maxInt)
        };
        let minLmaxL = {
            minlength: ValidationMessage.minlength(5),
            maxlength: ValidationMessage.maxlength(512)
        };
        return {
            indicator: minLmaxL,
            unit: minLmaxL,
            baseLine: minMax,
            target: minMax,
            organizationId: minMax,
            frequencyId: minMax
        }        
    }

    onSubmit(){
        let result= {
            ...this.indicator || {},
            ...this.form.value,
            startDate:DateFactory.createFromNgbDateStruct(this.dateRange.get('startDate').value),
            endDate: DateFactory.createFromNgbDateStruct(this.dateRange.get('endDate').value)
        }
        this.activeModal.close(result);
    }
    close(){
        this.activeModal.close();
    }
    cancel(){
        this.activeModal.close();
    }
    ngOnDestroy(): void {
        this.subscriptions.forEach(x => x.unsubscribe());
    }

}