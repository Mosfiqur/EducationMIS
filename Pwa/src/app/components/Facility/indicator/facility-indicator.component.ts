import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FacilityIndicatorDB } from 'src/app/localdb/FacilityIndicatorDB';
import { FacilityIndicator, BeneficiaryRecord, Facility, ListItem, FacilityRecord, FacilityDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { indicatorGetViewModel } from 'src/app/models/viewModel/indicatorGetViewModel';
import { ListItemDB } from 'src/app/localdb/ListItemDB';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { ICellEditorResult } from 'src/app/helpers/cell-editor-result';
import { IDynamicColumn } from 'src/app/models/cellEditorModels/IDynamicColumn';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';
import { Subscription } from 'rxjs';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { FacilityRecordsDB } from 'src/app/localdb/FacilityRecordsDB';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { FacilityViewModel } from 'src/app/models/viewModel/facilityViewModel';
import { facilityWiseIndicatorViewModel } from 'src/app/models/viewModel/facilityWiseIndicatorViewModel';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { ListDataTypeViewModel } from 'src/app/models/viewModel/ListDataTypeViewModel';
import { facilityDynamicCellAddViewModel } from 'src/app/models/viewModel/facilityDynamicCellAddViewModel';
import { dynamicCellViewModel } from 'src/app/models/viewModel/dynamicCellViewModel';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ToastrService } from 'ngx-toastr';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { FwCellEditorComponent } from '../../fw-cell-editor/fw-cell-editor.component';
import { ModalService } from 'src/app/services/modal.service';
import * as moment from 'moment';
import { IndicatorViewModel } from 'src/app/models/indicator/IndicatorViewModel';

@Component({
  selector: 'facility-indicator',
  templateUrl: './facility-indicator.component.html',
  styleUrls: ['./facility-indicator.component.scss']
})
export class FacilityIndComponent implements OnInit {

  @Input() onlineOfflineStatus: OnlineOfflineStatus;
  @Input() redirectTo: string;
  public facilityViewModel: FacilityViewModel;
  private allRecords = 2147483647;
  listItems: ListDataTypeViewModel[];
  public facilityDynamicCell: facilityDynamicCellAddViewModel;
  listFacilityDynamicCell: facilityDynamicCellAddViewModel[] = [];

  public test = '';
  public indicatorList: facilityWiseIndicatorViewModel[];
  listFacilityIndicators: any[] = []; // can be indicatorGetViewModel or FacilityIndicator[]
  listFacilityRecords: FacilityRecord[] = [];
  facility: any;
  instanceId: number;
  facilityId: number;
  testValue: any[];
  public indicatorForm: FormGroup;
  public validationErrors = {};
  private subs: Map<string, Subscription> = new Map();
  listSelectedListItem: Map<number, ListItem[]> = new Map(); 
  disabledEntityDynamicColumnIds= [1,9,10,12,13,14,16,17];

  constructor(private route: ActivatedRoute, private router: Router, private facilityIndicatorDbService: FacilityIndicatorDB,
    private facilityDbService: FacilityDb, private listItemDbService: ListItemDB, private modalService: ModalService,
    private formBuilder: FormBuilder, private dbService: IndexedDbService, private facilityRecordDbService: FacilityRecordsDB,
    private onlineFacilityService: OnlineFacilityService, private onlineBeneficiaryService: OnlineBeneficiaryService, private toastrService: ToastrService,
  ) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));
    this.buildForm();

    if (this.onlineOfflineStatus == OnlineOfflineStatus.Online) {
      this.test = 'Online';
      this.loadFacilityOnline().then((facilitiesResult) => {
        this.facility = facilitiesResult;

        this.loadFacilityIndicatorsOnline().then((indicatorsResult) => {
          this.listFacilityIndicators = indicatorsResult;
          this.buildForm();
          this.populatePatchForm();

          this.listFacilityIndicators.map(indicator => {
            if (indicator.listId != null)
              this.loadListTypeItemsOnline(indicator.listId, indicator);
          });
        });
      });

    }
    else {
      this.test = 'Offline';
      this.loadFacility().then((facilitiesResult) => {
        this.facility = facilitiesResult;

        this.loadFacilityIndicators().then((indicatorsResult) => {
          this.listFacilityIndicators = indicatorsResult;
          this.buildForm();

          this.listFacilityIndicators.map(indicator => {
            if (indicator.listId != null)
              this.loadListTypeItems(indicator.listId, indicator);
          });

          this.loadFacilityRecords().then((facilityRecordsResult) => {
            this.listFacilityRecords = facilityRecordsResult;

            this.populatePatchForm();
          });
        });
      });
    }
  }



  loadFacility() {
    return new Promise<any>((resolve, reject) => {
      this.facilityDbService.getFacilityByUniqueId(this.facilityId).subscribe((result) => {
        resolve(result);
      })
    })
  }

  loadFacilityIndicators() {
    return new Promise<any>((resolve, reject) => {
      this.facilityIndicatorDbService.getFacilityIndicatorsByInstanceId(this.instanceId).subscribe((data) => {
        resolve(data);
      })
    })
  }

  loadFacilityRecords() {
    return new Promise<any>((resolve, reject) => {
      this.facilityRecordDbService.getAllFacilityRecords().subscribe((data) => {
        var dataResult = data.filter(x => x.instanceId == this.instanceId && x.facilityId == this.facilityId);
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

  populatePatchForm() {
    console.log(this.facility);
    if (this.onlineOfflineStatus === OnlineOfflineStatus.Offline) {
      
      this.listFacilityRecords.forEach(x => {
        let facilityIndicator = this.listFacilityIndicators.find(indicator => indicator.id === x.columnId);
        if (facilityIndicator.columnDataType === 4 && x.value !== '') {
          x.value = moment(x.value.toString()).format('YYYY-MM-DD');
        }

        this.indicatorForm.get(x.columnId + '').patchValue(x.value);

        if(facilityIndicator.columnDataType === ColumnDataType.List){
          
          if(x.value !== ''){
          let listItemValues = <String[]> x.value.split(',');
          let listSavedListItem = [];

        if (facilityIndicator.listId != null){
          
            this.loadListTypeItems(facilityIndicator.listId, facilityIndicator).then((facilityIndicator) => {
              
              listItemValues.map(listItemValue => {
                var selectedListItem = facilityIndicator.listItems.find(x => x.value == listItemValue);
                selectedListItem.isSelected = true;
                listSavedListItem.push(selectedListItem);
              });
              
              this.ListTypeItemsChoosedShow({
                entityDynamicColumnId:facilityIndicator.entityDynamicColumnId,
                columnDataType:facilityIndicator.columnDataType,
                columnName:facilityIndicator.columnName,
                columnNameInBangla: facilityIndicator.columnNameInBangla,
                entityTypeId: facilityIndicator.entityTypeId,
                listItems: listSavedListItem,
                values: facilityIndicator.values
              });
            });
          }
        }
      }
      });
    }

    else {
      this.listFacilityIndicators.forEach(x => {
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


  disabledIndicatorNameFind(entityDynamicColumnId:number):string{
    if(entityDynamicColumnId === 9) {
      return (this.onlineOfflineStatus === OnlineOfflineStatus.Online) ?
       this.facility.programPartnerName : this.facility.programmingPartnerName;
    }
    
    else if(entityDynamicColumnId === 10){
      return this.facility.implementationPartnerName;
    }
    else if(entityDynamicColumnId === 12){
      return this.facility.upazilaName;
    }
    else if(entityDynamicColumnId === 13){
      return this.facility.unionName;
    }
    else if(entityDynamicColumnId === 14){
      return this.facility.campName;
    }
    else if(entityDynamicColumnId === 16){
      return this.facility.blockName;
    }
    else if(entityDynamicColumnId === 17){
      return this.facility.teacherName;
    }
    else if(entityDynamicColumnId === 1){
      return this.facility.facilityCode;
    }
  }

  buildForm() {
    let group = {};
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Online) {
      this.listFacilityIndicators.forEach(input_template => {
        group[input_template.entityDynamicColumnId] = new FormControl('');
      });
    }
    else {
      this.listFacilityIndicators.forEach(input_template => {
        group[input_template.id] = new FormControl('', Validators.required);
      });
    }
    this.indicatorForm = new FormGroup(group);

    this.subs = new ValidationErrorBuilder().withGroup(this.indicatorForm)
      .useMessageContainer(this.validationErrors)
      .build();
  }

  onClickRecordSave(facilityIndicatorId: number, columnDataType) {
    let submittedValue = this.indicatorForm.value;
    let recordValue = (submittedValue[facilityIndicatorId]).toString();

    if (columnDataType === ColumnDataType.Datetime) {
      recordValue = moment(recordValue).format('DD-MMM-YYYY');
    }

    if (!recordValue) { return; }

    if (this.onlineOfflineStatus == OnlineOfflineStatus.Online) {
      var facilityIndicator = this.listFacilityIndicators.find(x => x.entityDynamicColumnId === facilityIndicatorId);

      var facilityCell = new facilityDynamicCellAddViewModel();
      facilityCell.dynamicCells = [];
      facilityCell.instanceId = this.instanceId;
      facilityCell.facilityId = this.facilityId;

      let data = {
        entityDynamicColumnId: facilityIndicator.entityDynamicColumnId,
        value: recordValue.split(',')
      }
      facilityCell.dynamicCells.push(data);

      this.onlineFacilityService.saveFacilityCell(facilityCell).then(() => {
        this.toastrService.success(MessageConstant.SaveSuccessful);
      });
    }
    else {

      var facilityRecord = this.listFacilityRecords.find(x => x.columnId == facilityIndicatorId &&
        x.facilityId == this.facilityId && x.instanceId == this.instanceId);
      if (recordValue === "") { return; }

      facilityRecord.value = recordValue;
      facilityRecord.status = CollectionStatus.Collected;

      this.dbService.SaveFacilityRecord(facilityRecord, false).then((recordId) => {
        this.toastrService.success(MessageConstant.SaveSuccessful);
        //console.log("Saved Or Updated Facilty Record Id - ", recordId);
      });
    }
  }

  facilityDataCollectionStatusUpdateOffline(){
    return new Promise<any>((resolve,reject) => {
      this.dbService.isRecordCollectedForFacility(this.facilityId,this.instanceId).then(async (status) => {
        var facilityDataCollection = new FacilityDataCollectionStatus();
        facilityDataCollection.facilityId = this.facilityId;
        facilityDataCollection.instanceId = this.instanceId;

        if (status == true) {
          facilityDataCollection.status = CollectionStatus.Collected;
          resolve(this.dbService.SaveFacilityDataCollectionStatus(facilityDataCollection,false));
        }
        else{
          facilityDataCollection.status = CollectionStatus.NotCollected;
          resolve(this.dbService.SaveFacilityDataCollectionStatus(facilityDataCollection,false));
        }
      });
    });
  }

  onSubmit() {
    var submittedValue = this.indicatorForm.value;
    console.log(submittedValue);
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {
      let promisesSaveAll = [];

      // if (!this.indicatorForm.valid) {
      //   this.toastrService.error(MessageConstant.fillUpAllValue);
      //   return;
      // }

      for (var key in submittedValue) {
        if(submittedValue[key] === '' || submittedValue[key] === null){continue;}

        var value = (submittedValue[key]).toString();
        var columnId = parseInt(key);

        let facilityIndicator = this.listFacilityIndicators.find(indicator => indicator.id === columnId);
        if (facilityIndicator.columnDataType === 4) {
          value = moment(value.toString()).format('DD-MMM-YYYY');
        }

        var facilityRecord = new FacilityRecord();
        facilityRecord.facilityId = this.facilityId;
        facilityRecord.instanceId = this.instanceId;
        facilityRecord.columnId = columnId;
        facilityRecord.status = CollectionStatus.Collected;
        facilityRecord.value = value;

        promisesSaveAll.push(this.dbService.SaveFacilityRecord(facilityRecord, false));
      }
      Promise.all(promisesSaveAll).then((dbResult) => {
        this.facilityDataCollectionStatusUpdateOffline().then(() => {
        let total= this.listFacilityIndicators.length;
        let savedCount = dbResult.length;
        this.toastrService.success(" (" + savedCount + " Out of " + total + ")",MessageConstant.SaveSuccessful);
      });
    });
    }

    else {
      let facilityDynamicCell = new facilityDynamicCellAddViewModel();
      this.insertIntoFacilityDynamicCellModel(submittedValue, facilityDynamicCell);

      this.onlineFacilityService.saveFacilityCell(facilityDynamicCell).then(() => {
        let total = this.listFacilityIndicators.length;
        let savedCount = facilityDynamicCell.dynamicCells.length;
        //this.toastrService.success('success', 'Saved<br/>it!');
        this.toastrService.success(" (" + savedCount + " Out of " + total + ")",MessageConstant.SaveSuccessful);
      });
    }
    // console.log(this.indicatorForm.value['101']);
  }

  onClickListTypeSave(column: IDynamicColumn) {
    let value = [];

    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {
      var selectedListItem = this.indicatorForm.get(column.id + '').value;
      value = selectedListItem.split(',');
    }
    else{
      var selectedListItem = this.indicatorForm.get(column.entityDynamicColumnId + '').value;
      value = selectedListItem.split(',');
    }
    console.log(column);

    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(
      FwCellEditorComponent, { cell: { entityDynamicColumnId: column.entityDynamicColumnId, value: value, values: value }, column: column })
      .then((cellResult) => {
        console.log(cellResult);
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
    let indicator = <IndicatorViewModel>this.listFacilityIndicators.find(x => x.entityDynamicColumnId === column.entityDynamicColumnId);
    
    // When indicator loads -> listItem will not null
    let filteredListItems: any[] = [];
    if(column.listItems != null){
      filteredListItems = column.listItems.filter(x => x.isSelected === true);
    }
    else{
      filteredListItems = indicator.listItems.filter(x => x.isSelected === true);
    }
    
    this.listSelectedListItem.set(column.entityDynamicColumnId,filteredListItems);
  }

  addIndicatorValue(value: string, indicator: indicatorGetViewModel) {

    if (indicator.columnDataType != ColumnDataType.List) {
      indicator.values = [value];
    }
    else {
      indicator.values.push(value);
    }
  }

  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }


  //Online

  insertIntoFacilityDynamicCellModel(submittedValue, facilityDynamicCell) {
    facilityDynamicCell.dynamicCells = [];

    facilityDynamicCell.instanceId = this.instanceId;
    facilityDynamicCell.facilityId = this.facilityId;

    for (var key in submittedValue) {
      var value = (submittedValue[key]).toString();
      var columnId = parseInt(key);

      if (value === '') { continue; }

      let facilityIndicator = this.listFacilityIndicators.find(indicator => indicator.entityDynamicColumnId === columnId);
      if (facilityIndicator.columnDataType === 4 && value !== '') {
        value = moment(value.toString()).format('DD-MMM-YYYY');
      }

      let data = {
        entityDynamicColumnId: columnId,
        value: value.split(',')
      }
      facilityDynamicCell.dynamicCells.push(data);
    }
  }

  onChangeOnlineRecordSave(columnId: number, value: string) {

    console.log("Facility Id - " + this.facilityId + " Instance Id - ", this.instanceId, " ColumnId - ", columnId, " - Value - ", value);
    this.facilityDynamicCell = new facilityDynamicCellAddViewModel();
    this.facilityDynamicCell.dynamicCells = [];

    this.facilityDynamicCell.instanceId = this.instanceId;
    this.facilityDynamicCell.facilityId = this.facilityId;

    let data = {
      entityDynamicColumnId: columnId,
      value: value.split(',')
    }
    this.facilityDynamicCell.dynamicCells.push(data);

    this.listFacilityDynamicCell.push(this.facilityDynamicCell);
  }

  loadFacilityOnline() {
    return new Promise<any>((resolve, reject) => {
      this.onlineFacilityService.getFacilityById(this.facilityId).then((data) => {
        resolve(data);
      });
    });
  }


  loadFacilityIndicatorsOnline() {
    return new Promise<indicatorGetViewModel[]>((resolve, reject) => {
      var data = this.onlineFacilityService.getFacilityIndicator(this.instanceId, this.facilityId, this.allRecords, 1)
        .then((data) => {

          this.indicatorList = data.data;
          this.indicatorList.map(x => {
            return x.indicators.map(y => {
              return {
                ColumnName: y.ColumnName,
                columnDataType: y.columnDataType,
                dataCollectionDate: y.dataCollectionDate,
                entityDynamicColumnId: y.entityDynamicColumnId,
                isMultiValued: y.isMultiValued,
                listObjectId: y.listObjectId,
                values: y.values,
                listItem: y.listItem
              } as indicatorGetViewModel
            })
          });
          resolve(this.indicatorList[0].indicators);
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
}