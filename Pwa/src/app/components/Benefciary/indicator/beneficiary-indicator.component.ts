import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { BeneficiaryIndicatorDb } from 'src/app/localdb/BeneficiaryIndicatorDb';
import { ListItemDB } from 'src/app/localdb/ListItemDB';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { Beneficiary, BeneficiaryIndicator, BeneficiaryRecord, ListItem, BeneficiaryDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';
import { IDynamicColumn } from 'src/app/models/cellEditorModels/IDynamicColumn';
import { ModalService } from 'src/app/services/modal.service';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/helpers/cell-editor-result';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { FormGroup, Validators, FormBuilder, FormControl } from '@angular/forms';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { Subscription } from 'rxjs';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { indicatorGetViewModel } from 'src/app/models/viewModel/indicatorGetViewModel';
import { BeneficiaryViewModel } from 'src/app/models/viewModel/beneficiaryViewModel';
import { BeneficiaryDynamicCellAddViewModel } from 'src/app/models/viewModel/facilityDynamicCellAddViewModel';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import * as moment from 'moment';
import {Location} from '@angular/common';
import { BeneficiaryDataCollectionStatusDb } from 'src/app/localdb/BeneficiaryDataCollectionStatusDb';
import { listItem } from 'src/app/models/indicator/listObject';
import { BeneficiaryWiseIndicatorViewModel } from 'src/app/models/viewModel/facilityWiseIndicatorViewModel';
import { IndicatorViewModel } from 'src/app/models/indicator/IndicatorViewModel';

@Component({
  selector: 'beneficiary-indicator',
  templateUrl: './beneficiary-indicator.component.html',
  styleUrls: ['./beneficiary-indicator.component.scss']
})
export class BeneficiaryIndicatComponent implements OnInit {
  //online
  status: any;
  @Input() onlineOfflineStatus: OnlineOfflineStatus;
  instanceId: number;
  beneficiaryUniqueId: number;
  beneficiary: any;
  public indicatorForm: FormGroup;
  listbeneficiaryIndicator: any[] = [];
  listBeneficiaryRecords: BeneficiaryRecord[] = [];
  listSelectedListItems: ListItem[] = [];
  public validationErrors = {};
  private subs: Map<string, Subscription> = new Map();
  private allRecords = 2147483647;
  public beneficiaryDynamicCell: BeneficiaryDynamicCellAddViewModel;
  listBeneficiaryDynamicCell: BeneficiaryDynamicCellAddViewModel[] = [];
  listSelectedListItem: Map<number,ListItem[]> = new Map(); 
  disabledEntityDynamicColumnIds= [ 122, 132, 133, 134, 135];

  constructor(private route: ActivatedRoute, private beneficiaryDbService: BeneficiaryDb, private listItemDbService: ListItemDB,
    private beneficiaryIndicatorDbService: BeneficiaryIndicatorDb, private beneficiaryRecordDbService: BeneficiaryRecordsDB,
    private modalService: ModalService, private dbService: IndexedDbService, private formBuilder: FormBuilder, private onlineBeneficiaryService: OnlineBeneficiaryService,
    private onlineFacilityService: OnlineFacilityService, private toastrService: ToastrService,
    private location:Location,private beneficiaryDataCollectionDbService:BeneficiaryDataCollectionStatusDb) {

  }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.buildForm();

    this.status = this.onlineOfflineStatus;
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Online) {
      this.beneficiaryUniqueId = parseInt(this.route.snapshot.paramMap.get('beneficiaryId')); // beneficiary id

      this.loadBeneficiaryOnline().then((beneficiary) => {
        this.beneficiary = beneficiary;

        this.loadBenenficiaryIndicatorsOnline().then((beneficiaryIndicators) => {
          this.listbeneficiaryIndicator = beneficiaryIndicators;
          this.buildForm();
          this.populatePatchForm();
          
          // this.listbeneficiaryIndicator.map(indicator => {
          //   if (indicator.listId != null){
          //     this.loadListTypeItemsOnline(indicator.listId, indicator);
          //   }
          // });
        });

      });
    }
    else {
      this.beneficiaryUniqueId = parseInt(this.route.snapshot.paramMap.get('beneficiaryUniqueId'));

      this.loadBeneficiary().then((beneficiary) => {
        this.beneficiary = beneficiary;

        this.loadBeneficiaryIndicators().then((beneficiaryIndicators) => {
          this.listbeneficiaryIndicator = beneficiaryIndicators;
          this.buildForm();

          this.listbeneficiaryIndicator.map(indicator => {
            if (indicator.listId != null)
              this.loadListTypeItems(indicator.listId, indicator);
          });

          this.loadBeneficiaryRecords().then((beneficiaryRecordsResult) => {
            this.listBeneficiaryRecords = beneficiaryRecordsResult;

            this.populatePatchForm();
          });
        });

      });
    }
  }
  

  populatePatchForm() {
    if (this.onlineOfflineStatus === OnlineOfflineStatus.Offline) {
      this.listBeneficiaryRecords.forEach(x => {
        let beneficiaryIndicator = this.listbeneficiaryIndicator.find(indicator => indicator.id === x.columnId);
        
        if (beneficiaryIndicator.columnDataType === 4 && x.value !== '') {
          x.value = moment(x.value.toString()).format('YYYY-MM-DD');
        }
        this.indicatorForm.get(x.columnId + '').patchValue(x.value);

        if(beneficiaryIndicator.columnDataType === ColumnDataType.List){
          if(x.value !== ''){
          let listItemValues = <String[]> x.value.split(',');
          let listSavedListItem = [];

        if (beneficiaryIndicator.listId != null)
            this.loadListTypeItems(beneficiaryIndicator.listId, beneficiaryIndicator).then((beneficiaryIndicator) => {
              
              listItemValues.map(listItemValue => {
                var selectedListItem = beneficiaryIndicator.listItems.find(x => x.value == listItemValue);
                selectedListItem.isSelected = true;
                listSavedListItem.push(selectedListItem);
              });
    
              this.ListTypeItemsChoosedShow({
                entityDynamicColumnId:beneficiaryIndicator.entityDynamicColumnId,
                columnDataType:beneficiaryIndicator.columnDataType,
                columnName:beneficiaryIndicator.columnName,
                columnNameInBangla: beneficiaryIndicator.columnNameInBangla,
                entityTypeId: beneficiaryIndicator.entityTypeId,
                listItems: listSavedListItem,
                values: beneficiaryIndicator.values
              });
              console.log(listItemValues);
              console.log(listSavedListItem);
            });
        }
      }
      });
    }

    else {
      this.listbeneficiaryIndicator.forEach(x => {
        let value = x.values.join(',');

        if (x.columnDataType === ColumnDataType.Datetime && value !== '') {
          let formattedValue = moment(value).format('YYYY-MM-DD');
          this.indicatorForm.get(x.entityDynamicColumnId + '').patchValue(formattedValue);
        }
        else {
          // if(this.isDisabledIndicator(x.entityDynamicColumnId) === true){
          //   value = this.disabledIndicatorNameFind(x.entityDynamicColumnId,value);
          // }
          this.indicatorForm.get(x.entityDynamicColumnId + '').patchValue(value);
        }
        
        if(x.columnDataType === ColumnDataType.List){
          if(value !== ''){
            let listItemValues = <String[]> value.split(',');

            let listSavedListItem = [];
            listItemValues.map(listItemValue => {
              var selectedListItem = x.listItems.find(x => x.value == listItemValue);
              selectedListItem.isSelected = true;
              listSavedListItem.push(selectedListItem);
            });
  
            this.ListTypeItemsChoosedShow({
              entityDynamicColumnId:x.entityDynamicColumnId,
              columnDataType:x.columnDataType,
              columnName:x.columnName,
              columnNameInBangla: x.columnNameInBangla,
              entityTypeId: x.entityTypeId,
              listItems: listSavedListItem,
              values: x.values
            });  
          }         
        }
      });
    }
  }

  isDisabledIndicator(entityDynamicColumnId:number){   
    return this.disabledEntityDynamicColumnIds.find(x => x === entityDynamicColumnId) ? true : false;
  }


  disabledIndicatorNameFind(entityDynamicColumnId:number, value:string){
    if(entityDynamicColumnId === 132) {
      return this.beneficiary.facilityName;
    }
    else if(entityDynamicColumnId === 133){
      return this.beneficiary.beneficiaryCampName;
    }
    else if(entityDynamicColumnId === 134){
      return this.beneficiary.blockName;
    }
    else if(entityDynamicColumnId === 135){
      return this.beneficiary.subBlockName;
    }
    else if(entityDynamicColumnId === 122){
      return this.onlineOfflineStatus === OnlineOfflineStatus.Offline ? this.beneficiary.UnhcrId : this.beneficiary.unhcrId;
    }
    else{
      return value;
    }
  }


  buildForm() {

    let group = {}
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Online) {
      this.listbeneficiaryIndicator.forEach(input_template => {
        group[input_template.entityDynamicColumnId] = new FormControl('');
      });
    }
    else {
      this.listbeneficiaryIndicator.forEach(input_template => {
        group[input_template.id] = new FormControl('', Validators.required);
      });
    }
    this.indicatorForm = new FormGroup(group);

    this.subs = new ValidationErrorBuilder().withGroup(this.indicatorForm)
      .useMessageContainer(this.validationErrors)
      .build();
  }

  onChangeOnlineRecordSave(columnId: number, value: string) {
    console.log("Beneficiary  Id - " + this.beneficiaryUniqueId + " Instance Id - ", this.instanceId, " ColumnId - ", columnId, " - Value - ", value);

    this.beneficiaryDynamicCell = new BeneficiaryDynamicCellAddViewModel();
    this.beneficiaryDynamicCell.dynamicCells = [];

    this.beneficiaryDynamicCell.instanceId = this.instanceId;
    this.beneficiaryDynamicCell.beneficiaryId = this.beneficiaryUniqueId;

    let data = {
      entityDynamicColumnId: columnId,
      value: value.split(',')
    }
    this.beneficiaryDynamicCell.dynamicCells.push(data);

    this.listBeneficiaryDynamicCell.push(this.beneficiaryDynamicCell);

  }

  onClickRecordSave(beneficiaryIndicatorId: number, columnDataType) {

    let submittedValue = this.indicatorForm.value;
    let recordValue = (submittedValue[beneficiaryIndicatorId]).toString();
    if (columnDataType === ColumnDataType.Datetime) {
      recordValue = moment(recordValue).format('DD-MMM-YYYY');
    }

    if (!recordValue) { this.toastrService.error(MessageConstant.fieldEmpty); return; }

    if (this.onlineOfflineStatus === OnlineOfflineStatus.Online) {

      var beneficiaryIndicator = this.listbeneficiaryIndicator.find(x => x.entityDynamicColumnId === beneficiaryIndicatorId);

      var beneficiaryCell = new BeneficiaryDynamicCellAddViewModel();
      beneficiaryCell.dynamicCells = [];
      beneficiaryCell.instanceId = this.instanceId;
      beneficiaryCell.beneficiaryId = this.beneficiaryUniqueId;

      let data = {
        entityDynamicColumnId: beneficiaryIndicator.entityDynamicColumnId,
        value: recordValue.split(',')
      }
      beneficiaryCell.dynamicCells.push(data);

      this.onlineFacilityService.saveBeneficiaryCell(beneficiaryCell).then(() => {
        this.toastrService.success(MessageConstant.SaveSuccessful);
      });
    }
    else {

      var beneficiaryRecord = this.listBeneficiaryRecords.find(x => x.columnId == beneficiaryIndicatorId &&
        x.beneficiaryId == this.beneficiaryUniqueId && x.instanceId == this.instanceId);
      if (recordValue === "") { return; }

      beneficiaryRecord.value = recordValue;
      beneficiaryRecord.status = CollectionStatus.Collected;

      this.dbService.SaveBeneficiaryRecord(beneficiaryRecord, false).then((recordId) => {
        this.toastrService.success(MessageConstant.SaveSuccessful);
        //console.log("Saved Or Updated Facilty Record Id - ", recordId);
      });
    }
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
    let value = [];

    if(this.onlineOfflineStatus === OnlineOfflineStatus.Offline){
      var selectedListItem = this.indicatorForm.get(column.id + '').value;
      value = selectedListItem.split(',');
    }

    else{
      var selectedListItem = this.indicatorForm.get(column.entityDynamicColumnId + '').value;
      value = selectedListItem.split(',');
    }
    console.log(column);
    
    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(
      FwCellEditorComponent, { cell: { entityDynamicColumnId: column.entityDynamicColumnId, value: value,values:value }, column: column })
      .then((cellResult) => {
        if (cellResult.isCanceled) {
          return;
        }
        this.ListTypeItemsChoosedShow(column);

        if (this.onlineOfflineStatus == OnlineOfflineStatus.Online) {
          this.indicatorForm.patchValue({ [column.entityDynamicColumnId]: cellResult.cell.value.join(',') });
        }

        else {
          this.indicatorForm.patchValue({ [column.id]: cellResult.cell.value.join(',') });
        }
      });
  }

  private ListTypeItemsChoosedShow(column: IDynamicColumn) {
    let indicator = <IndicatorViewModel>this.listbeneficiaryIndicator.find(x => x.entityDynamicColumnId === column.entityDynamicColumnId);
    
    // When indicator loads -> listItem will not null
    let filteredListItems: any[] = [];
    if(column.listItems != null){
      filteredListItems = column.listItems.filter(x => x.isSelected === true);
    }
    else{
      filteredListItems = indicator.listItems.filter(x => x.isSelected === true);
    }
    
    this.listSelectedListItem.set(column.entityDynamicColumnId,filteredListItems);
    //console.log(this.listSelectedListItem.get(column.entityDynamicColumnId));
    // filteredListItems.map(filteredListItem => {
    //   this.listSelectedListItem.push(filteredListItem.title);
    // });
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
    return new Promise<any>((resolve,reject) => {
      this.listItemDbService.getListItemByListId(listId).subscribe((results) => {
        indicator['listItems'] = results;
        resolve(indicator);
      });
    });
  }

  ColumnDataTypeText(id) {
    return ColumnDataType[id];
  }

  beneficiaryDataCollectionStatusUpdateOffline(){
    return new Promise<any>((resolve,reject) => {
      this.dbService.isRecordCollectedForBeneficiary(this.beneficiaryUniqueId,this.instanceId).then(async (status) => {
        var beneficiaryDataCollection = new BeneficiaryDataCollectionStatus();
        beneficiaryDataCollection.beneficiaryId = this.beneficiaryUniqueId;
        beneficiaryDataCollection.instanceId = this.instanceId;

        if (status == true) {
          beneficiaryDataCollection.status = CollectionStatus.Collected;
          resolve(this.dbService.SaveBeneficiaryDataCollectionStatus(beneficiaryDataCollection,false));
        }
        else{
          beneficiaryDataCollection.status = CollectionStatus.NotCollected;
          resolve(this.dbService.SaveBeneficiaryDataCollectionStatus(beneficiaryDataCollection,false));
        }
      });
    });
  }

  onSubmit() {
    var submittedValue = this.indicatorForm.value;

    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {
      // if (!this.indicatorForm.valid) {
      //   this.toastrService.error(MessageConstant.fillUpAllValue);
      //   return;
      // }

      let promisesSaveAll = [];
      for (var key in submittedValue) {

        if(submittedValue[key] === '' || submittedValue[key] === null){continue;}
        var value = (submittedValue[key]).toString();
        var columnId = parseInt(key);

        let beneficiaryIndicator = this.listbeneficiaryIndicator.find(indicator => indicator.id === columnId);
       
        if (beneficiaryIndicator.columnDataType === 4) {
          value = moment(value.toString()).format('DD-MMM-YYYY');
        }
        
        var beneficiaryRecord = new BeneficiaryRecord();
        beneficiaryRecord.beneficiaryId = this.beneficiaryUniqueId;
        beneficiaryRecord.instanceId = this.instanceId;
        beneficiaryRecord.columnId = columnId;
        beneficiaryRecord.status = CollectionStatus.Collected;
        beneficiaryRecord.value = value;

        promisesSaveAll.push(this.dbService.SaveBeneficiaryRecord(beneficiaryRecord, false));
      }
      Promise.all(promisesSaveAll).then((dbResult) => {
        this.beneficiaryDataCollectionStatusUpdateOffline().then(() => {
          let total= this.listbeneficiaryIndicator.length;
          let savedCount = dbResult.length;
          this.toastrService.success(" (" + savedCount + " Out of " + total + ")",MessageConstant.SaveSuccessful);
        });
      });
    }
    else {
      let beneficiaryDynamicCell = new BeneficiaryDynamicCellAddViewModel();
      this.insertIntoBeneficiaryDynamicCellModel(submittedValue, beneficiaryDynamicCell);

      this.onlineFacilityService.saveBeneficiaryCell(beneficiaryDynamicCell).then(() => {
        let total= this.listbeneficiaryIndicator.length;
        let savedCount = beneficiaryDynamicCell.dynamicCells.length;
        this.toastrService.success(" (" + savedCount + " Out of " + total + ")",MessageConstant.SaveSuccessful);
      });
    }

    //console.log(this.indicatorForm.value['101']);
  }

  loadBeneficiaryOnline() {
    return new Promise<any>((resolve, reject) => {
      this.onlineBeneficiaryService.GetBeneficiaryById(this.beneficiaryUniqueId,this.instanceId).then((data) => {
        resolve(data);
      });
    });
  }

  loadBenenficiaryIndicatorsOnline() {
    return new Promise<any>((resolve, reject) => {
      var data = this.onlineFacilityService.getBeneficiaryIndicator(this.instanceId, this.beneficiaryUniqueId, this.allRecords, 1)
        .then((data) => {
          resolve(data.data[0].indicators);
        });
    });
  }

  loadListTypeItemsOnline(listId, indicator) {
    return new Promise<any>((resolve, reject) => {
      this.onlineBeneficiaryService.GetAllListTypeData(1, this.allRecords).then((data) => {
        let listItemFiltered = data.data.filter(x => x.id == listId);
        indicator['listItems'] = listItemFiltered;
      });
    });
  }
  insertIntoBeneficiaryDynamicCellModel(submittedValue, beneficiaryDynamiCell) {
    beneficiaryDynamiCell.dynamicCells = [];

    beneficiaryDynamiCell.instanceId = this.instanceId;
    beneficiaryDynamiCell.beneficiaryId = this.beneficiaryUniqueId;

    for (var key in submittedValue) {
      var value = (submittedValue[key]).toString();
      var columnId = parseInt(key);

      if(value === ''){continue;}

      let beneficiaryIndicator = this.listbeneficiaryIndicator.find(indicator => indicator.entityDynamicColumnId === columnId);

      if (beneficiaryIndicator.columnDataType === ColumnDataType.Datetime) {
        if (value !== "") {
          value = moment(value.toString()).format('DD-MMM-YYYY');
        }
      }

      let data = {
        entityDynamicColumnId: columnId,
        value: value.split(',')
      }
      beneficiaryDynamiCell.dynamicCells.push(data);
    }
  }

  makeBeneficiaryInactiveOnline(beneficiaryId){
    this.onlineBeneficiaryService.DeactivateBeneficiary({
      beneficiaryIds: [beneficiaryId],
      instanceId: this.instanceId
    }).then(() => {
      this.toastrService.success(MessageConstant.inactiveSuccessful);
      this.location.back();
    });
  }

  makeBeneficiaryInactiveToggle(disengaged: boolean) {
    this.beneficiary.disengaged = !disengaged;
    console.log(this.beneficiary.disengaged);
    this.beneficiaryDbService.updateBeneficiary(this.beneficiary);
  }

  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }
}