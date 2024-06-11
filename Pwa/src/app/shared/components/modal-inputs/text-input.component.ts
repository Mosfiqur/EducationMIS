import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({    
    templateUrl: './text-input.component.html'    
})
export class TextInputComponent implements OnInit{
 
    public title: string = "";
    public inputLabel: string = "";    
    public paragraph: string = "";
    public paragraphForm: FormGroup;

    constructor( private fb: FormBuilder, private activeModal: NgbActiveModal){

    }

    ngOnInit(): void {
        this.paragraphForm = this.fb.group({
            paragraph: this.paragraph ? this.paragraph: ""
        })
    }

    close(){
        this.activeModal.close();
        //this.activeModal.close();
    }
    cancel(){
        this.close();
    }

    onSubmit(){
        this.activeModal.close(this.paragraphForm.get('paragraph').value);
    }
}