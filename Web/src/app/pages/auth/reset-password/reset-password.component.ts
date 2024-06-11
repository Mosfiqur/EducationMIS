import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { IForgotPasswordModel } from 'src/app/models/auth/forgot-password.model';
import { ITokenValidationResult } from 'src/app/models/auth/token-validation-result.model';
import { AuthService } from 'src/app/services/auth.service';
import { HttpClientService } from 'src/app/services/httpClientService';
import { ModalService } from 'src/app/services/modal.service';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { ForgotPasswordComponent } from 'src/app/shared/components/forgot-password/forgot-password.component';
import { CustomValidators } from 'src/app/utility/CustomValidators';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';

@Component({
  selector: 'reset-password',
  templateUrl: './reset-password.component.html',
  providers: [HttpClientService],
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {  
  public form: FormGroup;
  public validationErrors = {};
  private subscriptions: Map<string, Subscription> = new Map();
  private token:string;
  public tokenValidation: ITokenValidationResult = {isValid : true};
  public resetSuccessful: boolean = false;
  constructor(
      private fb: FormBuilder,
      private activatedRoute: ActivatedRoute,
      private authService: AuthService
  ){

    this.activatedRoute.queryParamMap.subscribe(params => {
      const token = params.get('token');
      this.token = token;
    
      this.validateToken(token);
    })
  }

  validateToken(token: string){
      this.authService.validateToken(token)
      .then(result => this.tokenValidation = result)      
      .catch(x=> x);
  }
  ngOnDestroy(): void {
      this.subscriptions.forEach(x => x.unsubscribe());
  }
  ngOnInit(): void {
      this.buildForm();
  }


  buildForm(){       
      const confirm = this.fb.group({
          newPassword: ["", [
                  Validators.required, 
                  Validators.minLength(6), 
                  Validators.maxLength(16)]
              ],
          confirmPassword: ["", ]
      }, {validators: [CustomValidators.confirmPassword('newPassword', 'confirmPassword')]});
            
      this.form = this.fb.group({          
          confirm: confirm
      });


      let minMax = {
          minlength: ValidationMessage.minlength(6),
          maxlength: ValidationMessage.minlength(16)
      }
      this.subscriptions = new ValidationErrorBuilder()
      .withGroup(this.form)
      .withGroup(confirm)
      .useMessageContainer(this.validationErrors)
      .withMessages({
          newPassword: minMax            
      })
      .withCustomValidationMessages({
          confirmPasswordError: ValidationMessage.confirmPasswordError()
      })
      .build();
  }


  submit(){
      if(this.form.invalid){
          return;
      }
      let model = {
        ...this.form.value,
        ...this.form.get('confirm').value,
        token: this.token
    };

    this.authService.resetUserPassword(model)
    .then(()=> {
        this.resetSuccessful = true;
    })
    
  }

}
