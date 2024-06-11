import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from 'src/app/models/user/user.model';
import { UserService } from 'src/app/services/user.service';
import { PartnerType } from 'src/app/_enums/partnerType';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';


@Component({
    selector:'app-profile-info-update',
    templateUrl: './profile-info-update.component.html',
    styleUrls: ['./profile-info-update.component.scss']
})
export class ProfileInfoUpdateComponent implements OnInit{
    
    user:any;
    userForm: FormGroup;
    isReadonlyHTMLElement:boolean = true;

    constructor(private authService: AuthService,private userService:UserService,
        private formBuilder: FormBuilder,private router:Router,private toastrService:ToastrService,
        ){

    }
    ngOnInit(){
        this.user = this.authService.userObject;
        this.buildForm();
        this.userForm.patchValue({...this.user});
    }
    buildForm() {
        this.userForm = this.formBuilder.group({
            fullName: ["", [Validators.required, Validators.minLength(3), Validators.maxLength(255)]],
            email: ["", [Validators.required, Validators.email]],
            phoneNumber: [""]
          });
    }

      async onSubmit() {
        if (!this.userForm.valid) {
          return;
        }
    
        let userModel = User.create(this.userForm.value);
        
          userModel.id = this.user.id;          
          await this.userService.updateProfileInfo(userModel);
          this.toastrService.success(MessageConstant.SaveSuccessful);
          this.isReadonlyHTMLElement = true;
          this.updateUserModel(userModel)
      }

    clickedEditButton(){
      this.isReadonlyHTMLElement = false;
    }

    clickedCancelButton(){
        this.isReadonlyHTMLElement = true;
    }
   
    updateUserModel(userVM:Partial<User>){
      this.user.fullName = userVM.fullName;
      this.user.phoneNumber = userVM.phoneNumber;
      localStorage.removeItem("userProfile");
      localStorage.setItem("userProfile",JSON.stringify(this.user));
    }
}