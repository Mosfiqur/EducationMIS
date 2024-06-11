import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { interval } from 'rxjs';
import { Subscription } from 'rxjs/internal/Subscription';
import { AuthService } from 'src/app/services/auth.service';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';

@Component({
    templateUrl: './forgot-password.component.html'
})
export class ForgotPasswordComponent implements OnInit{

    public form: FormGroup;
    public validationErrors = {};
    public successMessage = "";
    private subscriptions: Map<string, Subscription> = new Map();
    constructor(
        private fb: FormBuilder,
        private activeModal: NgbActiveModal,
        private authService: AuthService,
    ){

    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(x => x.unsubscribe());
    }

    ngOnInit(): void {
        this.buildForm();
    }

    buildForm(){       

        this.form = this.fb.group({
            email: ["", [Validators.required, Validators.email]]            
        });

        this.subscriptions = new ValidationErrorBuilder()
        .withGroup(this.form)
        .useMessageContainer(this.validationErrors)
        .withMessages({
            email: {
                email: ValidationMessage.invlaidEmail()
            }            
        })        
        .build();
    }

    close(){
        this.activeModal.close();
    }

    cancel(){
        this.close();
    }


    submit(){
        if(this.form.invalid){
            return;
        }

        this.authService.requestPasswordResetMail(this.form.value)
        .then(x => {
            
            this.successMessage = "We just sent an email to you with a link to reset your password!";            
        })
        .catch(x=> {
            this.activeModal.close();
        });
    }
}