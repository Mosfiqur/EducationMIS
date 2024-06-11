import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbDateParserFormatter, NgbActiveModal, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ScheduleType } from 'src/app/_enums/schedule-type';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { Convert } from 'src/app/utility/Convert';
import { ISchedule, Schedule } from 'src/app/models/scheduling/schedule.model';
import { DateFactory } from 'src/app/utility/DateFactory';
import { Frequency } from 'src/app/models/scheduling/frequency.model';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { CustomValidators } from 'src/app/utility/CustomValidators';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { Subscription } from 'rxjs';
import { Globals } from 'src/app/globals';


@Component({
    templateUrl: './schedule-creation.component.html'
})
export class ScheduleCreationComponent implements OnInit, OnDestroy{

    public scheduleForm: FormGroup;    
    public frequencies: ISelectListItem[];

    public schedule: ISchedule;
    public isEditing: boolean = false;
    public validationErrors:any = {};
    private subscriptions: Map<string, Subscription> = new Map();    
    
    get scheduleType(): ScheduleType{
        return this.scheduleForm.get('scheduleType').value;
    }
    
    get dateRange(): AbstractControl {    
        return this.scheduleForm.get('dateRange');
    }

    get startDate(): NgbDateStruct {
        return this.dateRange.get('startDate').value;
    } 

    get endDate(): NgbDateStruct {
        return this.dateRange.get('endDate').value;
    } 
   
    public dpConfig;
    constructor(
        private fb: FormBuilder,
        private formatter: NgbDateParserFormatter,
        private activeModal: NgbActiveModal,
        private globals: Globals
    ){        
        this.frequencies = Convert.enumToSelectList(ScheduleType);
        this.dpConfig = {
            ...this.globals.bsDatepickerConfig
        };        
    }
  

    ngOnInit(): void {
        let dateRange = this.buildDateRange();
        this.scheduleForm = this.fb.group({
            scheduleName: ["", [Validators.required]],
            scheduleType: ["", [Validators.required]],
            dateRange: dateRange,
            description: [""]
        });

        if(this.schedule && this.schedule.id){
            this.isEditing = true;            

            this.patchValues();
        };
        this.setupValidationMessages();
    }

    buildDateRange() : FormGroup{
        return this.fb.group({
            startDate: ["", Validators.required],
            endDate: ["", Validators.required], 
          },{validators: CustomValidators.dateRangeValidator('startDate', 'endDate')});
    }

    patchValues(){
        this.scheduleForm.patchValue({
            ...this.schedule,
            scheduleType: ScheduleType[this.schedule.scheduleType]            
        });
        this.dateRange.patchValue({
            startDate: new Date(this.schedule.startDate.toString()),
            endDate:  new Date(this.schedule.endDate.toString())
        });
    }   

    setupValidationMessages(){
        this.subscriptions = new ValidationErrorBuilder()
        .withGroup(this.scheduleForm)
        .withGroup(this.dateRange as FormGroup)
        .useMessageContainer(this.validationErrors)
        .withCustomValidationMessages({
            invalidDateRange: ValidationMessage.invalidDateRange()
        })
        .build();
    }

    onClose(){
        this.activeModal.close();
    }
    onCancel(){
        this.activeModal.close();
    }
    onSubmit(){
        if(!this.scheduleForm.valid){
            return ;
        }        

        let d= this.dateRange.get('startDate').value;
        let v = DateFactory.fromModel(this.dateRange.get('startDate').value);

        let s = {
            ...this.schedule || {},
            ...this.scheduleForm.value,     
            startDate: DateFactory.fromModel(this.dateRange.get('startDate').value),
            endDate: DateFactory.fromModel(this.dateRange.get('endDate').value),
            frequency: Frequency.createNew(this.scheduleType.toString(), DateFactory.fromModel(this.dateRange.get('startDate').value))
        } as ISchedule;   
        this.activeModal.close(s);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(x => x.unsubscribe());
    }
}