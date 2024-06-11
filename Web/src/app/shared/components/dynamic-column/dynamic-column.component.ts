import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { IListDataType } from 'src/app/models/dynamicColumn/list-datatype';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { Convert } from 'src/app/utility/Convert';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { EntityType } from 'src/app/_enums/entityType';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { ListDataTypeService } from '../../services/list-datatype.service';


@Component({
  templateUrl: './dynamic-column.component.html'
})
export class DynamicColumnComponent implements OnInit, OnDestroy {

  title:string;
  buttonText:string;

  validationErrors = {};
  entityTypeId: EntityType;
  column: DynamicColumnSaveViewModel;
  entityColumnId: number;

  facilityEntityType = EntityType.Facility;
  targetPopulation: ISelectListItem[];
  selectedTargetPopulation: TargetPopulation;

  dataType: any;
  dataTypeList: ISelectListItem[];
  selectedDataType: ColumnDataType;

  listDataTypes: IListDataType[] = [];
  selectedListDataType: IListDataType;

  openListEditor = false;
  subscriptions: Map<string, Subscription> = new Map();

  dynamicColumnForm: FormGroup;

  get selectedList(): AbstractControl {
    return this.dynamicColumnForm.get('columnListId');
  }
  constructor(
    private dynamicColumnService: DynamicColumnService,
    private listDataTypeService: ListDataTypeService,
    private activeModal: NgbActiveModal,
    private fb: FormBuilder
  ) {
    //this.column = new DynamicColumnSaveViewModel();  
    this.dataType = ColumnDataType;
    this.dataTypeList = Convert.enumToSelectList(ColumnDataType);
    this.targetPopulation = Convert.enumToSelectList(TargetPopulation);
  }

  ngOnInit(): void {

    this.title= this.title==null?"Add New Column":this.title
    this.buttonText= this.buttonText==null?"Add New Column":this.buttonText
    //this.loadListTypes();
    this.buildForm();
    this.setValidationForTargetPopulation();
    setTimeout(() => {
      this.loadData();
    }, 100);
    
  }

  loadListTypes() {
    this.listDataTypeService.getAll().then(listDataTypes => {
      this.listDataTypes.length = 0;
      listDataTypes.forEach(item => this.listDataTypes.push(item));
      this.listDataTypes.unshift({
        id: 0,
        name: "New List",
        listItems: []
      })
    });
  }

  loadData() {
    if (this.entityColumnId > 0) {
      this.dynamicColumnService.getById(this.entityColumnId,this.entityTypeId)
      .then((column: any) => {
        
        this.dynamicColumnForm.patchValue({ ...column });
        
        if(column.columnDataType==ColumnDataType.List){
          this.selectedListDataType=column.columnList
          this.selectedDataType=column.columnDataType;
          this.openListEditor = true;
          this.loadListTypes();
        }
      });
    }
  }

  buildForm() {
    this.dynamicColumnForm = this.fb.group({
      columnName: [
        this.column ? this.column.columnName : "", [
          Validators.required,
          Validators.minLength(3),
          Validators.maxLength(255)
        ]
      ],
      columnNameInBangla: [
        this.column ? this.column.columnNameInBangla : "",
        [Validators.required,
        Validators.minLength(3),
        Validators.maxLength(255)]
      ],
      columnDataType: [
        this.column ? this.column.columnDataType : "",
        [Validators.required]
      ],
      columnListId: [this.column && this.column.columnListId ? this.column.columnListId : ""],
      isMultiValued: [this.column && this.column.columnListId ? this.column.isMultiValued : ""],
      description: [this.column ? this.column.description : ""],
      targetedPopulation: [this.column ? this.column.targetedPopulation : ""]
    });

    let msg = {
      columnName: {
        minlength: ValidationMessage.minlength(3),
        maxlength: ValidationMessage.maxlength(255)
      },
      columnNameInBangla: {
        minlength: ValidationMessage.minlength(3),
        maxlength: ValidationMessage.maxlength(255)
      }
    };

    this.subscriptions = new ValidationErrorBuilder()
      .withGroup(this.dynamicColumnForm)
      .withMessages(msg)
      .useMessageContainer(this.validationErrors)
      .build();

  }

  setValidationForTargetPopulation(): void {
    const targetedPopulation = this.dynamicColumnForm.get('targetedPopulation');
    if (this.entityTypeId === EntityType.Facility) {
      targetedPopulation.setValidators(Validators.required);
    } else {
      targetedPopulation.clearValidators();
    }
    targetedPopulation.updateValueAndValidity();
  }

  requireList() {
    let control =
      this.dynamicColumnForm.get('columnListId');
    control.setValidators([Validators.required, Validators.min(1)]);
    control.updateValueAndValidity();
  }

  unRequireList() {
    let control =
      this.dynamicColumnForm.get('columnListId');
    control.clearValidators();
    control.updateValueAndValidity();
  }


  dataTypeSelect() {
    this.selectedDataType = this.dynamicColumnForm.get('columnDataType').value;
    if (this.selectedDataType == ColumnDataType.List) {
      this.requireList();
      this.loadListTypes();
    }
    else {
      this.unRequireList();
    }

  }

  onSubmit() {
    if (this.dynamicColumnForm.invalid) {
      return;
    }

    if (this.column && this.column.id) {
      // update

    } else {
      this.create();
    }

  }

  create() {

    let columnToSave = {
      ...this.dynamicColumnForm.value,
      entityTypeId: this.entityTypeId,
      entityColumnId: this.entityColumnId
    }
    this.dynamicColumnService.save(columnToSave)
      .then((column: DynamicColumnSaveViewModel) => {
        this.activeModal.close({
          ...columnToSave,
          id: column.id,
          columnDataType: parseInt(columnToSave.columnDataType)
        });
      });
  }
  onSelectListType() {
    this.openListEditor = true;
    let id = this.selectedList.value;
    if (!id) {
      this.closeEditor();
      return;
    }
    this.selectedListDataType = this.listDataTypes.find(x => x.id == id);
    this.openListEditor = true;
  }

  onSaveListDataType(aListDataType: IListDataType) {
    if (!this.listDataTypes.map(x => x.id).includes(aListDataType.id)) {
      this.listDataTypes.push(aListDataType);
    }
    this.openListEditor = false;
    this.selectedList.setValue(aListDataType.id);
    this.selectedList.updateValueAndValidity();
  }
  onCancelListDataType() {
    this.closeEditor();
    this.selectedList.setValue('');
    this.selectedList.updateValueAndValidity();
  }

  closeEditor() {
    this.openListEditor = false;
    this.selectedListDataType = null;
  }

  cancel() {
    this.close();
  }

  close() {
    this.activeModal.close();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(x => x.unsubscribe());
  }
}