import { Component, Input, Output, EventEmitter, OnInit, ViewChild } from '@angular/core';
import { IFrameworkDynamicColumn, FrameworkDynamicColumn } from 'src/app/models/frameworks/framework-dynamic-column.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FrameworkType } from 'src/app/_enums/frameworkType';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalService } from 'src/app/services/modal.service';


@Component({
    selector: 'fwdynamic',
    templateUrl: './fw-column-editor.component.html'
})
export class FwColumnEditorComponent implements OnInit{
    fwdynamicColumnForm: FormGroup;
    dynamicColumn: IFrameworkDynamicColumn; 
    frameWorkType: FrameworkType;   
    isMultiValueSelectorVisible: boolean = true;
    constructor(private fb: FormBuilder, 
        private activeModal: NgbActiveModal,
        private modalService: ModalService
        ){
        
    }

    ngOnInit(): void {     
        this.fwdynamicColumnForm = this.fb.group({
            columnName: [this.dynamicColumn ? this.dynamicColumn.columnName: "", [Validators.required]],
            hasMultiValue: [this.dynamicColumn ? this.dynamicColumn.hasMultiValue : false],
            frameworkType: this.frameWorkType,
            isFixed: false,
            id: [this.dynamicColumn ? this.dynamicColumn.id : null]
        });
    }
    handleCancel(){                    
        this.activeModal.close();        
    }

    handleDelete(){
        this.activeModal.close({isDeleted: true});        
    }

    onSubmit(){        
        if(!this.fwdynamicColumnForm.valid){            
            return;
        }              
        this.activeModal.close({column: this.fwdynamicColumnForm.value});
    }
}