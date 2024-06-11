import { Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormGroup, FormControl, Validators, AbstractControl, ValidationErrors, FormBuilder } from "@angular/forms";
import { UserService } from "src/app/services/user.service";
import { IRole } from 'src/app/models/role/role.model';
import { LevelRank } from 'src/app/_enums/levelRank';
import { IUser, User } from 'src/app/models/user/user.model';
import { IEducationSectorPartner } from 'src/app/models/user/educationSectorPartner';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { PartnerType } from 'src/app/_enums/partnerType';
import { Router, ActivatedRoute } from '@angular/router';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { Subscription } from 'rxjs';
@Component({
  selector: "app-user-create",
  templateUrl: "./user-create.component.html",
  styleUrls: ["./user-create.component.scss"],
})
export class UserCreateComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  public user: User = null;
  
  public roles: IRole[] = [];
  public selectedRole: IRole;
  public levelRank: any;
  public espList: IEducationSectorPartner[];

  public editState: boolean = false;

  public validationErrors: any = {};
  public subscriptions : Map<string, Subscription> = new Map();

  constructor(
    public userService: UserService,
    public formBuilder: FormBuilder,
    public router: Router,
    public toastrService: ToastrService,
    public route: ActivatedRoute    
  ) {
    this.levelRank = LevelRank;
  }


  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');
    
    this.userService.getRoles().then(response => {
      this.roles = response.data;
      if (this.user != null) {
        this.setSelectedRole(this.user.roleId);
      }
    });
    this.userService.getEspList().then(response => this.espList = response);


    if (id) {
      this.editState = true;
      this.userService.getById(id).then(response => {
        this.user = response;
        this.updateForm(this.user);
      });
    }

    this.userForm = this.formBuilder.group({
      fullName: ["", [Validators.required, Validators.minLength(3), Validators.maxLength(255)]],
      designationName: ["", [Validators.required]],
      roleId: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      phoneNumber: ["", [ Validators.minLength(11), Validators.maxLength(13)]],
      educationSectorPartnerId: [""],
      programPartnerId: [""],
      implementationPartnerId: [""]
    });

    this.setValidation();
  }

  setValidation(){
    
    let msg = {
      email: {
        email: ValidationMessage.invlaidEmail()
      },
      fullName: {
        minlength: ValidationMessage.minlength(3),
        maxlength: ValidationMessage.maxlength(255),          
      },
      phoneNumber: {
        minlength: ValidationMessage.minlength(11),
        maxlength: ValidationMessage.maxlength(13),
      }
    };    
    
    this.subscriptions = new ValidationErrorBuilder()
      .withGroup(this.userForm)
      .withMessages(msg)      
      .useMessageContainer(this.validationErrors)
      .build();
  }

  updateForm(user?: User) {
    this.userForm.patchValue({
      ...user
    });

    this.userForm.get('educationSectorPartnerId').setValue(user.educationSectorPartnerId);
    this.userForm.get('programPartnerId').setValue(user.programPartnerId);
    this.userForm.get('implementationPartnerId').setValue(user.implementationPartnerId);

    this.setSelectedRole(this.user.roleId);
  }


  async onSubmit() {
    if (!this.userForm.valid) {
      return;
    }

    let esp = this.userForm.get('educationSectorPartnerId').value;
    let pp = this.userForm.get('programPartnerId').value;
    let ip = this.userForm.get('implementationPartnerId').value;

    let userModel = User.create(this.userForm.value);
    userModel.eduSectorPartners = [];
    if (esp) {
      userModel.eduSectorPartners.push({ id: esp, partnerType: PartnerType.EducationSectorPartner });
    }
    if (pp) {
      userModel.eduSectorPartners.push({ id: pp, partnerType: PartnerType.ProgramPartner });
    }
    if (ip) {
      userModel.eduSectorPartners.push({ id: ip, partnerType: PartnerType.ImplementationPartner });
    }

    if (!this.editState) {
      await this.userService.createUser(userModel);
      this.toastrService.success(MessageConstant.SaveSuccessful);
      this.router.navigate(['unicef/users/home']);
    } else {
      userModel.id = this.user.id;
      await this.userService.updateUser(userModel);
      this.toastrService.success(MessageConstant.SaveSuccessful);
      this.router.navigate(['unicef/users/home']);
    }
  }

  setSelectedRole(roleId: number) {
    if (this.roles.length) {
      this.onSelectRole(roleId);
    }
  }

  require(controlName: string){
    this.userForm.get(controlName).setValidators([Validators.required]);
    this.userForm.get(controlName).updateValueAndValidity();
  }
  unRequire(controlName: string){
    this.userForm.get(controlName).clearValidators();
    this.userForm.get(controlName).updateValueAndValidity();
    this.userForm.get(controlName).reset();
  }

  onSelectRole(roleId: number) {        
    if(!roleId){
      return;
    }
    this.selectedRole = this.roles.find(r => r.id == roleId);
    if (this.selectedRole.levelRank == LevelRank.ImplementationPartner || this.selectedRole.levelRank == LevelRank.ProgramPartner) {
      this.require('educationSectorPartnerId');
      this.unRequire('programPartnerId');
      this.unRequire('implementationPartnerId');
    }    
    else if (this.selectedRole.levelRank == LevelRank.Teacher) {
      this.unRequire('educationSectorPartnerId');
      this.require('programPartnerId');
      this.require('implementationPartnerId');

    }
    else{
      this.unRequire('educationSectorPartnerId');
      this.unRequire('programPartnerId');
      this.unRequire('implementationPartnerId');
    }
    this.userForm.updateValueAndValidity();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());    
  }
}
