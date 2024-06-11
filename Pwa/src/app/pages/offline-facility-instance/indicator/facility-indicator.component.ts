import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FacilityIndicatorDB } from 'src/app/localdb/FacilityIndicatorDB';
import { FacilityIndicator, BeneficiaryRecord, Facility, ListItem, FacilityRecord } from 'src/app/models/idbmodels/indexedDBModels';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { indicatorGetViewModel } from 'src/app/models/viewModel/indicatorGetViewModel';
import { ListItemDB } from 'src/app/localdb/ListItemDB';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { ModalService } from 'src/app/services/modal.service';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/helpers/cell-editor-result';
import { IDynamicColumn } from 'src/app/models/cellEditorModels/IDynamicColumn';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { FacilityRecordsDB } from 'src/app/localdb/FacilityRecordsDB';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-facility-indicator',
  templateUrl: './facility-indicator.component.html',
  styleUrls: ['./facility-indicator.component.scss']
})
export class FacilityIndicatorComponent implements OnInit {

  status:OnlineOfflineStatus;
  listFacilityIndicators: FacilityIndicator[];
  listFacilityRecords:FacilityRecord[] = [];
  facility: Facility;
  instanceId: number;
  facilityId: number;
  testValue: any[];
  public indicatorForm:FormGroup;
  public validationErrors = {};
  private subs: Map<string, Subscription> = new Map();


  constructor(private route: ActivatedRoute, private router: Router, private facilityIndicatorDbService: FacilityIndicatorDB,
    private facilityDbService: FacilityDb, private listItemDbService: ListItemDB,private modalService: ModalService,
    private formBuilder:FormBuilder,private dbService:IndexedDbService,private facilityRecordDbService:FacilityRecordsDB) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));

    this.status = OnlineOfflineStatus.Offline;
    // this.loadFacility().then((facilitiesResult) => {
    //   this.facility = facilitiesResult;

    //   this.loadFacilityIndicators().then((indicatorsResult) => {
    //     this.listFacilityIndicators = indicatorsResult;

    //     this.listFacilityIndicators.map(indicator => {
    //       if (indicator.listId != null)
    //         this.loadListTypeItems(indicator.listId, indicator);
    //     });

    //     this.loadFacilityRecords().then((facilityRecordsResult) => {
    //       this.listFacilityRecords = facilityRecordsResult;
    //     });
    //   });

    //   this.buildForm();

    // })
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

  loadFacilityRecords(){
    return new Promise<any>((resolve,reject) => {
      this.facilityRecordDbService.getAllFacilityRecords().subscribe((data) => {
        var dataResult = data.filter(x => x.instanceId == this.instanceId && x.facilityId == this.facilityId);
        resolve(dataResult);
      });
      // this.facilityRecordDbService.getAllFacilityRecords(this.facilityId,this.instanceId).subscribe((data) => {
      //   debugger;
      //   resolve(data);
      // });
    });
  }

  loadListTypeItems(listId, indicator) {

    this.listItemDbService.getListItemByListId(listId).subscribe((results) => {
      indicator['listItems'] = results;
    })

  }

  ColumnDataTypeText(id) {
    return ColumnDataType[id];
  }

  buildForm(){
    this.indicatorForm = this.formBuilder.group({
      value:["",[Validators.required]]
    });

    this.subs = new ValidationErrorBuilder().withGroup(this.indicatorForm)
    .useMessageContainer(this.validationErrors)
    .build();
  }

  onClickRecordSave(facilityIndicatorId:number){
    
    var facilityRecord = this.listFacilityRecords.filter(x => x.columnId == facilityIndicatorId).pop();// for last change record
    if(facilityRecord.value === ""){return;}
     this.dbService.SaveFacilityRecord(facilityRecord,false).then((recordId) => {
      console.log("Saved Or Updated Facilty Record Id - ",recordId);
    });
  }

  onChangeRecordSave(columnId:number,value:string) {
console.log("Facility Id - "+this.facilityId+" Instance Id - ",this.instanceId," ColumnId - ",columnId," - Value - ",value);
    var facilityRecord = new FacilityRecord();
    facilityRecord.instanceId = this.instanceId;
    facilityRecord.columnId = columnId;
    facilityRecord.facilityId = this.facilityId;
    facilityRecord.status = CollectionStatus.Collected;
    facilityRecord.value = value;

    this.listFacilityRecords.push(facilityRecord);

    // this.dbService.SaveFacilityRecord(facilityRecord).then((recordId) => {
    //   console.log("Saved Or Updated Facilty Record Id - ",recordId);
    // });
  }

  onSubmit() {

  }

  onClickListTypeSave(column:IDynamicColumn){
    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(
      FwCellEditorComponent, {cell: {entityDynamicColumnId:column.entityDynamicColumnId,value:["1"]}, column: column})
    .then((cellResult)=> {
      this.onChangeRecordSave(column.id,cellResult.cell.value.join(','));
    });
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

}
