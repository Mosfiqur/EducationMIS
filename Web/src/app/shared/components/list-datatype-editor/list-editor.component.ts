import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { IListDataType } from 'src/app/models/dynamicColumn/list-datatype';
import { ListItem } from 'src/app/models/dynamicColumn/listItem';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { ListDataTypeService } from '../../services/list-datatype.service';


function duplicateListItemValidator(controlName: string): ValidatorFn {

    return function duplicateValidator(control: AbstractControl): { [key: string]: Boolean } | null {

        if (control.parent != undefined && control.parent.parent != undefined) {
            let dict = {};
            let formArray = control.parent.parent as FormArray;
            for (let i = 0; i < formArray.length; i++) {
                let val = formArray.controls[i].get(controlName).value;
                if (dict[val]) {
                    return { duplicate: true }
                }
                dict[val] = true;
            }
        }
        return null;
    }
}

@Component({
    selector: 'list-editor',
    templateUrl: './list-editor.component.html'
})
export class ListEditorComponent implements OnChanges, OnInit, OnDestroy {


    validationErrors = {};
    skipValidationFor: string[] = ['listItems'];

    list: IListDataType;
    listEditorForm: FormGroup;

    isEditing: boolean = false;
    subscriptions: Map<string, Subscription> = new Map();

    get listItems(): FormArray {
        return this.listEditorForm.get('listItems') as FormArray;
    }

    @Input('listId') listId?: number;
    @Output('done') done: EventEmitter<IListDataType> = new EventEmitter();
    @Output('cancel') cancel: EventEmitter<void> = new EventEmitter();

    constructor(
        private listDataTypeService: ListDataTypeService,
        private fb: FormBuilder,
        private toastrService: ToastrService
    ) {

    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes.listId.currentValue) {
            setTimeout(() => {
                this.listDataTypeService.getById(+this.listId)
                    .then(x => {
                        this.list = x;
                        this.isEditing = false;
                        this.buildForm();
                    });
            });

        } else {
            this.isEditing = true;
            this.list = null;
            this.buildForm();
        }
    }
    ngOnInit(): void {

    }


    buildForm() {
        let listItems = this.list ? [...this.list.listItems.map(x => this.buildListItem(x))] : [this.buildListItem()];
        this.listEditorForm = this.fb.group({
            name: [this.list ? this.list.name : "", [Validators.required, Validators.minLength(5), Validators.maxLength(255)]],
            listItems: this.fb.array(listItems)
        });
        this.setUpValidation();
    }

    setUpValidation() {
        let msg = {
            name: {
                minlength: ValidationMessage.minlength(5),
                maxlength: ValidationMessage.maxlength(255)
            }
        };

        let builder = new ValidationErrorBuilder()
            .withGroup(this.listEditorForm);
        this.listItems.controls.forEach((group: FormGroup, index) => {
            builder.withGroup(group, 'listItems' + index);
        });

        this.subscriptions =
            builder.skip(this.skipValidationFor)
                .useMessageContainer(this.validationErrors)
                .withCustomValidationMessages({ duplicate: ValidationMessage.duplicateNotAllowed() })
                .withMessages(msg)
                .build();
    }


    buildListItem(aListItem?: ListItem): FormGroup {
        return this.fb.group({
            id: [aListItem ? aListItem.id : 0, []],
            title: [aListItem ? aListItem.title : "", [Validators.required, duplicateListItemValidator('title')]],
            value: [aListItem ? aListItem.value : "", [Validators.required, duplicateListItemValidator('value')]]
        });
    }

    addListItem() {
        let group = this.buildListItem();
        let subs = new ValidationErrorBuilder()
            .withGroup(group, 'listItems' + this.listItems.length)
            .withCustomValidationMessages({ duplicate: ValidationMessage.duplicateNotAllowed() })
            .useMessageContainer(this.validationErrors)
            .build();

        subs.forEach((sub, key) => this.subscriptions.set(key, sub));
        this.listItems.push(group);
    }

    removeItemAt(index: number) {
        this.listItems.removeAt(index);
        if (this.listItems.length == 0) {
            this.addListItem();
        }
        this.subscriptions[index].unsubscribe();
    }

    onSubmit() {
        if (this.listEditorForm.invalid) {
            return;
        }

        if (this.list && this.list.id > 0) {
            let list = {
                ...this.list,
                ...this.listEditorForm.value
            }
            this.listDataTypeService.update(list)
                .then(x => {
                    this.toastrService.success(MessageConstant.SaveSuccessful);
                    this.isEditing = false;
                });
        }
        else {
            this.listDataTypeService.add(this.listEditorForm.value)
                .then(async aListDataType => {
                    this.list = aListDataType;
                    this.toastrService.success(MessageConstant.SaveSuccessful);
                    this.isEditing = false;
                });
        }
    }


    onCancel() {
        this.cancel.emit();
    }

    startEditing() {
        this.isEditing = true;
    }

    doneEditing() {
        this.done.emit(this.list);
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(x => x.unsubscribe());
    }
}