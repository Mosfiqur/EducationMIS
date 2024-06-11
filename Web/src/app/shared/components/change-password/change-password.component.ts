import { Component, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { CustomValidators } from 'src/app/utility/CustomValidators';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';


function passWord(c: AbstractControl){
    
    const pass:string = c.value;

    
}


@Component({
    templateUrl: './change-password.component.html'
})
export class ChangePasswordComponent implements OnInit, OnDestroy{
    
    public form: FormGroup;
    public validationErrors: any = {};
    private subscriptions: Map<string, Subscription> = new Map();
    constructor(
        private fb: FormBuilder,
        private activeModal: NgbActiveModal
    ){

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
            oldPassword: ["", [Validators.required]],
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
        this.activeModal.close({
            ...this.form.value,
            ...this.form.get('confirm').value
        });
    }

}