import { Component } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: './confirm-modal.component.html'
})
export class ConfirmModalComponent{

    title: string;
    assertion: string;
    body: string;

    constructor(private activeModal: NgbActiveModal){        
    }

    handleCancel(){
        this.activeModal.close(false);
    }

    handleOkay(){        
        this.activeModal.close(true);        
    }
}