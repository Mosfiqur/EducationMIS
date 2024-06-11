import { Component, OnDestroy, OnInit } from '@angular/core';
import { BudgetService } from 'src/app/services/budget.service';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { FrameworkDynamicColumnService } from 'src/app/services/framework-dynamic-column.service';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { DateFactory } from 'src/app/utility/DateFactory';
import { IBudgetFramework } from 'src/app/models/frameworks/budget-framework.model';
import { CommonService } from 'src/app/services/common.service';
import { Observable, of, Subscription } from 'rxjs';
import { catchError, debounceTime, distinctUntilChanged, switchMap, tap } from 'rxjs/operators';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { IProject } from 'src/app/models/frameworks/project.model';
import { IDonor } from 'src/app/models/frameworks/donor.model';
import { NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { CustomValidators } from 'src/app/utility/CustomValidators';
import { Globals } from 'src/app/globals';



@Component({
  selector: 'app-new-budget',
  templateUrl: './new-budget.component.html',
  styleUrls: ['./new-budget.component.scss']
})
export class NewBudgetComponent implements OnInit, OnDestroy {

  private subscriptions: Map<string, Subscription> = new Map();

  public budget: IBudgetFramework = null;
  public budgetForm: FormGroup;
  public validationErrors: any = {};  
  public dpConfig;

  constructor(
    private budgetService: BudgetService,
    private fb: FormBuilder,
    private toastrService: ToastrService,
    private router: Router,
    private route: ActivatedRoute,
    private globals: Globals
    ) { 
      this.route.paramMap.subscribe(params =>{
        const id = +params.get('id');
        this.getBudget(id);
      });

      this.dpConfig = {
        ...this.globals.bsDatepickerConfig        
      };       
    }
  
  get dateRange(): AbstractControl{
    return this.budgetForm.get('dateRange');
  }

  get project(): IProject{
    let proj = this.budgetForm.get('project').value;
    if(proj.id){
      return proj;
    }
    return {
      name: proj
    }
  }
  get donor(): IDonor {
    let don = this.budgetForm.get('donor').value;
    if(don.id){
      return don;      
    }
    return {
      name: don
    }
  }
  

  ngOnInit() {
    
  }

  getBudget(id: number){

    if(id == 0){
      this.budget = {
        startDate: null,
        endDate: null,        
        donor: null,
        project: null,
        amount: 0,
      }
      this.buildForm();
      return;
    }
    
    this.budgetService.getById(id)
    .subscribe(x => {
      this.budget = x;
      this.buildForm();
    });
  }


  buildForm(){   
    let dateRange  = this.fb.group({
      startDate: ["", Validators.required],
      endDate: ["", Validators.required], // Validate range
    }, {validators: CustomValidators.dateRangeValidator('startDate', 'endDate')});
    
    this.budgetForm = this.fb.group({
      dateRange: dateRange,
      donor: ["", [Validators.required]],
      project: ["", [Validators.required]],
      amount: ["", [Validators.required, Validators.min(1)]]
    });

    this.subscriptions = new ValidationErrorBuilder()
    .withGroup(this.budgetForm)
    .withGroup(dateRange)
    .withMessages({
      amount: {
        min: ValidationMessage.min(1)
      }
    })
    .withCustomValidationMessages({
      invalidDateRange: ValidationMessage.invalidDateRange()
    })
    .useMessageContainer(this.validationErrors)
    .build();  

    if(this.budget.id){
      this.patchForm();
    }
  }

  patchForm(){
    this.budgetForm.patchValue({
      ...this.budget
    });

    this.dateRange.patchValue({
      startDate: DateFactory.toModel(this.budget.startDate.toString()),
      endDate: DateFactory.toModel(this.budget.endDate.toString()),
    });
  }
  
  async onSubmit(){
    if(this.budgetForm.invalid){
      return;
    }

    let b = {
      ...this.budget ? this.budget : {},
      ...this.budgetForm.value      
    } as IBudgetFramework;
    
    this.addProject(this.project)
    .then((project: IProject) => {
      b.projectId = project.id;
      return this.addDonor(this.donor)
    })
    .then((donor: IDonor) => {
      b.donorId = donor.id;      
      b.startDate = DateFactory.fromModel(this.dateRange.get('startDate').value);
      b.endDate = DateFactory.fromModel(this.dateRange.get('endDate').value);
      if(this.budget.id){
        return this.budgetService.updateBudget(b);
      }
      return this.budgetService.addBudget(b);
    })
    .then(x => {
      this.toastrService.success(MessageConstant.SaveSuccessful);    
      this.router.navigate(['unicef/framework/budget']);
    });    
  }

  updateBudget(){   
    
    let b = {
      ...this.budget,
      ...this.budgetForm.value
    }

    this.addProject(this.project)
    .then(x => {
      b.projectId = x.id;
      return this.addDonor(this.donor);
    })
    .then(x => {
      b.donorId = x.id;
      return this.budgetService.updateBudget(b);
    })
  }

  addProject(project: IProject): Promise<IProject>{
    if(project.id){
      return Promise.resolve(project);
    }
    return new Promise((resolve, reject) => {
      this.budgetService.addProject(project)
      .then(resolve)
      .catch(reject)
    });
  }

  addDonor(donor: IDonor): Promise<IDonor> {
    if(donor.id){
      return Promise.resolve(donor);
    }    
    return new Promise((resolve, reject) => {
      this.budgetService.addDonor(donor)
      .then(resolve)
      .catch(reject)
    });
  }

  public donorSearchFailed: boolean;
  searchDonor = (text$: Observable<string>) =>
      text$.pipe(
        debounceTime(300),
        distinctUntilChanged(),        
        switchMap(term =>
          this.budgetService.searchDonor(term).pipe(
            tap(() => this.donorSearchFailed = false),
            catchError(() => {
              this.donorSearchFailed = true;
              return of([]);
            }))
        )
      )

  formatter = (x: {name: string}) => x.name;

  public projectSearchFailed: boolean;
  searchProject = (text$: Observable<string>) =>  
      text$.pipe(
        debounceTime(300),
        distinctUntilChanged(),        
        switchMap(term =>
          this.budgetService.searchProject(term).pipe(
            tap(() => this.projectSearchFailed = false),
            catchError(() => {
              this.projectSearchFailed = true;
              return of([]);
            }))
        )
      );


ngOnDestroy(): void {
  this.subscriptions.forEach( x=> x.unsubscribe());
}

}
