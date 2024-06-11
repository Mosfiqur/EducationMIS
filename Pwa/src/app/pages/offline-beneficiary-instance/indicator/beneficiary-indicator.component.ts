import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { BeneficiaryIndicatorDb } from 'src/app/localdb/BeneficiaryIndicatorDb';
import { ListItemDB } from 'src/app/localdb/ListItemDB';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { Beneficiary, BeneficiaryIndicator, BeneficiaryRecord, ListItem } from 'src/app/models/idbmodels/indexedDBModels';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';
import { IDynamicColumn } from 'src/app/models/cellEditorModels/IDynamicColumn';
import { ModalService } from 'src/app/services/modal.service';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/helpers/cell-editor-result';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { Subscription } from 'rxjs';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { stat } from 'fs';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';

@Component({
  selector: 'app-beneficiary-indicator',
  templateUrl: './beneficiary-indicator.component.html',
  styleUrls: ['./beneficiary-indicator.component.scss']
})
export class BeneficiaryIndicatorComponent implements OnInit {
  private allRecords = 2147483647;

  status:OnlineOfflineStatus;
  instanceId: number;
  beneficiaryUniqueId: number;
  beneficiary: Beneficiary;
  public indicatorForm:FormGroup;
  listbeneficiaryIndicator: BeneficiaryIndicator[];
  listBeneficiaryRecords: BeneficiaryRecord[];
  listSelectedListItems:ListItem[] = [];
  public validationErrors = {};
  private subs: Map<string, Subscription> = new Map();
  
  constructor(private route: ActivatedRoute, private beneficiaryDbService: BeneficiaryDb, private listItemDbService: ListItemDB,
    private beneficiaryIndicatorDbService: BeneficiaryIndicatorDb, private beneficiaryRecordDbService: BeneficiaryRecordsDB,
    private modalService: ModalService, private dbService: IndexedDbService,private formBuilder:FormBuilder,private onlineFacilityService:OnlineFacilityService,
    private onlineBeneficiaryService:OnlineBeneficiaryService,) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.beneficiaryUniqueId = parseInt(this.route.snapshot.paramMap.get('beneficiaryUniqueId'));

    this.status = OnlineOfflineStatus.Offline;

    // this.loadBeneficiary().then((beneficiary) => {
    //   this.beneficiary = beneficiary;

    //   this.loadBeneficiaryIndicators().then((beneficiaryIndicators) => {
    //     this.listbeneficiaryIndicator = beneficiaryIndicators;

    //     this.listbeneficiaryIndicator.map(indicator => {
    //       if (indicator.listId != null)
    //         this.loadListTypeItems(indicator.listId, indicator);
    //     });

    //     this.loadBeneficiaryRecords().then((beneficiaryRecordsResult) => {
    //       this.listBeneficiaryRecords = beneficiaryRecordsResult;
    //     });
    //   });

    // });
    //this.buildForm();
  }


  //Online 


  loadBeneficiaryOnline() {
    return new Promise<any>((resolve, reject) => {
      this.onlineBeneficiaryService.GetBeneficiaryById(this.beneficiaryUniqueId,this.instanceId).then((data) => {
        resolve(data);
      });
    });
  }

  loadBenenficiaryIndicators() {
    return new Promise<any>((resolve, reject) => {
      var data = this.onlineFacilityService.getBeneficiaryIndicator(this.instanceId, this.beneficiaryUniqueId, this.allRecords, 1)
        .then((data) => {
          resolve(data.data[0].indicators);
          // this.indicatorList = data.data;
          // this.indicatorList.map(x => this.indicators = x.indicators);
          // console.log(this.indicatorList);
        });
    });
  }





  buildForm(){
    this.indicatorForm = this.formBuilder.group({
      value:["",[Validators.required]]
    });

    this.subs = new ValidationErrorBuilder().withGroup(this.indicatorForm)
    .useMessageContainer(this.validationErrors)
    .build();
  }

  onClickRecordSave(beneficiaryIndicatorId: number) {
    
    var beneficiaryRecord = this.listBeneficiaryRecords.filter(x => x.columnId == beneficiaryIndicatorId).pop();// for last change record
    if (beneficiaryRecord.value === "") { return; }
    this.dbService.SaveBeneficiaryRecord(beneficiaryRecord,false).then((recordId) => {
      console.log("Saved Or Updated Beneficiary Record Id - ", recordId);
    });
  }

  onChangeRecordSave(columnId: number, value: string) {
    
    console.log("Beneficiary Id - " + this.beneficiaryUniqueId + " Instance Id - ", this.instanceId, " ColumnId - ", columnId, " - Value - ", value);
    var beneficiaryRecord = new BeneficiaryRecord();
    beneficiaryRecord.instanceId = this.instanceId;
    beneficiaryRecord.columnId = columnId;
    beneficiaryRecord.beneficiaryId = this.beneficiaryUniqueId;
    beneficiaryRecord.status = CollectionStatus.Collected;
    beneficiaryRecord.value = value;

    this.listBeneficiaryRecords.push(beneficiaryRecord);

  }

  onClickListTypeSave(column: IDynamicColumn) {
    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(
      FwCellEditorComponent, { cell: { entityDynamicColumnId: column.entityDynamicColumnId, value: ["1"] }, column: column })
      .then((cellResult) => {
        cellResult.cell.value.map((eachValue) => {
          this.listItemDbService.getListItemById(eachValue).subscribe((data) => {
            this.listSelectedListItems.push(data);
          });
        });
        this.onChangeRecordSave(column.id, cellResult.cell.value.join(','));
      });
  }

  loadBeneficiary() {
    return new Promise<any>((resolve, reject) => {
      this.beneficiaryDbService.getBeneficiaryByUniqueId(this.beneficiaryUniqueId).subscribe((result) => {
        resolve(result);
      });
    });
  }

  loadBeneficiaryIndicators() {
    return new Promise<any>((resolve, reject) => {
      this.beneficiaryIndicatorDbService.getBeneficiaryIndicatorsByInstanceId(this.instanceId).subscribe((data) => {
        resolve(data);
      });
    });
  }


  loadBeneficiaryRecords() {
    return new Promise<any>((resolve, reject) => {
      this.beneficiaryRecordDbService.getAllBeneficiaryRecords().subscribe((data) => {
        var dataResult = data.filter(x => x.instanceId == this.instanceId && x.beneficiaryId == this.beneficiaryUniqueId);
        resolve(dataResult);
      });
    });
  }

  loadListTypeItems(listId, indicator) {

    this.listItemDbService.getListItemByListId(listId).subscribe((results) => {
      indicator['listItems'] = results;
    });

  }

  ColumnDataTypeText(id) {
    return ColumnDataType[id];
  }

  OnSubmit() {

  }

  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }

}
