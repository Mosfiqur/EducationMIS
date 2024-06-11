import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { TargetService } from 'src/app/services/target.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { CommonService } from 'src/app/services/common.service';
import { Camp } from 'src/app/models/common/camp.model';
import { Globals } from 'src/app/globals';
import { Gender } from 'src/app/_enums/gender';
import { IAgeGroup } from 'src/app/models/frameworks/age-group.model';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { Convert } from 'src/app/utility/Convert';
import { Utility } from 'src/app/utility/Utility';
import { Month } from 'src/app/_enums/month';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { Subscription } from 'rxjs';
import { ITargetFramework } from 'src/app/models/frameworks/target-framework.model';
import { CustomValidators } from 'src/app/utility/CustomValidators';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { UnionViewModel } from 'src/app/models/common/unionViewModel';

@Component({
  selector: 'app-new-target',
  templateUrl: './new-target.component.html',
  styleUrls: ['./new-target.component.scss']
})
export class NewTargetComponent implements OnInit, OnDestroy {

  public target: ITargetFramework
  public validationErrors: any = {};
  public targetForm: FormGroup;
  public camps: Camp[] = [];
  public ageGroupList: IAgeGroup[] = [];
  public genderList: ISelectListItem[] = [];
  public yearList: number[] = [];
  public monthList: ISelectListItem[] = [];
  public targetPopulationList : ISelectListItem[] = [];
  public upazilaList: ISelectListItem[] = [];
  public filteredUnions: ISelectListItem[] = [];

  private unions: UnionViewModel[] = [];
  private subs: Map<string, Subscription> = new Map();
  private targetId: number = 0;
  constructor(
    private targetService: TargetService,
    private fb: FormBuilder,
    private toastrService: ToastrService,
    private router: Router,
    private commonService: CommonService,
    private globals: Globals,
    private route: ActivatedRoute
    ) {
      this.route.paramMap.subscribe(params => {
        this.targetId = +params.get('id');
        ;
      });
      
     }
  
  get startDate():AbstractControl{
    return this.targetForm.get('dateRange').get('startDate');
  }
  get endDate():AbstractControl{
    return this.targetForm.get('dateRange').get('endDate');
  }

  get isCampRequired():boolean{    
    return this.targetForm.get('targetedPopulation').value != TargetPopulation.Host_Communities;
  }

  get filteredCamps():ISelectListItem[] {
    const unionId = this.targetForm.get('unionId').value;
    return this.camps.filter(x=> x.unionId == unionId)
      .map(x=> ({id: x.id, text: x.name} as ISelectListItem));
  }

  private getAllQuery = {
    pageNo: 1, pageSize: this.globals.maxPageSize
  };

  ngOnInit() {


    let promises:any[] = [            
      this.commonService.getAllCamps(),
      this.commonService.getUpazilas(this.getAllQuery),
      this.commonService.getUnions(this.getAllQuery),
      this.commonService.getAllAgeGroups(),
      this.targetService.getById(this.targetId)
    ];

    Promise.all(promises)
    .then(result => {
      this.camps = result[0].data;
      this.upazilaList = result[1].data.map(x => ({id: x.id, text: x.name}));
      this.unions= result[2].data;
      this.ageGroupList = result[3].data;
      this.target = result[4]
      if(this.targetId){
        this.patchForm();      
      }
    });

    this.targetPopulationList = Convert.enumToSelectList(TargetPopulation);
    this.genderList = Convert.enumToSelectList(Gender);    
    this.yearList = Utility.range(2000, 2030).sort((a, b) => -1);
    this.monthList = Convert.enumToSelectList(Month);
    this.buildForm();    
  }

  patchForm(){
    this.targetForm.patchValue({...this.target});
    this.startDate.patchValue({
      startYear: this.target.startYear,
      startMonth: this.target.startMonth,
    });
    this.endDate.patchValue({
      endYear: this.target.endYear,
      endMonth: this.target.endMonth
    });
    this.onChangeUpazila();    
    this.onTargetPopulationChange();
  }
  onChangeUpazila(){    
    const upazilaId = this.targetForm.get('upazilaId').value;    
    this.filteredUnions = this.unions.filter(x => x.upazilaId == upazilaId)
    .map(x => ({id: x.id, text: x.name}));
  }

  getTargetById(){
    this.targetService.getById(this.targetId).then(x => this.target = x);
    this.patchForm();
  }


  buildForm(){
    const startDate = this.fb.group({
      startYear: ["", [Validators.required]],
      startMonth: ["", [Validators.required]],
    });

    const endDate = this.fb.group({
      endYear: ["", [Validators.required]],
      endMonth: ["", [Validators.required]],
    });

    const dateRange = this.fb.group({
      startDate: startDate,
      endDate: endDate}, {validators: CustomValidators.dateRangeValidatorForTarget('startDate', 'endDate')});
      
    this.targetForm = this.fb.group({
      campId: ["", [Validators.required]],
      gender: ["", [Validators.required]],
      ageGroupId: ["", [Validators.required]],
      peopleInNeed: ["", [Validators.required]],
      target: ["", [Validators.required]],            
      dateRange: dateRange,
      targetedPopulation: ["", [Validators.required]],            
      upazilaId: ["", [Validators.required]],            
      unionId: ["", [Validators.required]],            
    });

    this.subs = 
    new ValidationErrorBuilder()
    .withGroup(this.targetForm)
    .withGroup(startDate)
    .withGroup(endDate)
    .withGroup(dateRange)
    .useMessageContainer(this.validationErrors)
    .withCustomValidationMessages({
      invalidDateRange: ValidationMessage.invalidDateRange()
    })
    .build();
  }

  onTargetPopulationChange(){
    let c = this.targetForm.get('campId');
    if(this.isCampRequired){
      c.setValidators([Validators.required]);
    }else{
      c.clearValidators();
    }
    c.updateValueAndValidity();
  }

  async onSubmit(){    
    if(this.targetForm.invalid){
      return;
    }

    let dateRange = {
      ...this.startDate.value,
      ...this.endDate.value
    };

    if(this.target.id){      

     
      await this.targetService.updateTarget({
        ...this.target,
        ...this.targetForm.value,   
        ...dateRange        
      })
    }else{
      await this.targetService.addTarget({
        ...this.targetForm.value,
        ...dateRange
      });
    }
    
    const toast = this.toastrService.success(MessageConstant.SaveSuccessful);    
    this.router.navigate(['unicef/framework/target']);    
  }

  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }



}
