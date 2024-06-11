import { Injectable, Type } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmModalComponent } from '../components/confirm-modal/confirm-modal.component';
import { ParagraphInputComponent } from '../shared/components/modal-inputs/paragraph-input.component';
import { TextInputComponent } from '../shared/components/modal-inputs/text-input.component';

@Injectable({
    providedIn: 'root'
})
export class ModalService {

    private options: NgbModalOptions;

    constructor(private ngbModal: NgbModal) {
        this.options = {
            backdrop: 'static',
            keyboard: false,
            size: 'lg'
        }
    }

    async open<T, R>(component: Type<T>, config?: Partial<T>, options?: NgbModalOptions): Promise<R> {
        let opt = {
            ...this.options,
            ...options
        } as NgbModalOptions;

        const modal = this.ngbModal.open(component, opt);
        Object.assign(modal.componentInstance, config);
        return modal.result;
    }

    async confirm(title?: string, assertion?: string, body?: string, confirmText?: string): Promise<boolean> {
        return this.open<ConfirmModalComponent, boolean>(ConfirmModalComponent, {
            title: title || "Confirm Action",
            assertion: assertion = "Are you sure you want to do this?",
            body: body || "You may lose data by performing this action.",
            confirmText: confirmText|| "Delete"
        });
    }

    async paragraphInput(modalTitle: string, inputLabel: string, paragraph?: string) {
        return this.open<ParagraphInputComponent, string>(ParagraphInputComponent, { inputLabel: inputLabel, title: modalTitle, paragraph: paragraph });
    }

    async textInput(modalTitle: string, inputLabel: string, value?: string) {
        return this.open<TextInputComponent, string>(TextInputComponent, { inputLabel: inputLabel, title: modalTitle, paragraph: value });
    }
}
