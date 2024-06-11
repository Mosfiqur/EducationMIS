import { Component, OnInit } from '@angular/core';
import { EntityType } from 'src/app/_enums/entityType';
import { ActivatedRoute } from '@angular/router';
import { FacilityRecordsDB } from 'src/app/localdb/FacilityRecordsDB';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { Facility, FacilityRecord, FacilityIndicator, FacilityDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { facilityDynamicCellAddViewModel } from 'src/app/models/viewModel/facilityDynamicCellAddViewModel';
import { filter } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { FacilityIndicatorDB } from 'src/app/localdb/FacilityIndicatorDB';
import { Location } from '@angular/common';
import { FacilityDataCollectionStatusDb } from 'src/app/localdb/FacilityDataCollectionStatusDb';
import { LoadingSpinnerService } from 'src/app/core/loading-spinner/loading-spinner.service';


@Component({
  selector: 'app-facility-list',
  templateUrl: './facility-list.component.html',
  styleUrls: ['./facility-list.component.scss']
})
export class FacilityListComponent implements OnInit {

  instanceId: number;
  collectFor: EntityType;
  listFacility: Facility[] = [];
  listFacilityIndicators: FacilityIndicator[];
  listFacilityRecords: FacilityRecord[];
  listFacilityDataCollectionStatus: FacilityDataCollectionStatus[] = [];

  constructor(private route: ActivatedRoute, private facilityRecordDbService: FacilityRecordsDB,
    private facilityDbService: FacilityDb, private toastrService: ToastrService,
    private dbService: IndexedDbService, private onlineFacilityService: OnlineFacilityService,
    private facilityIndicatorDbService: FacilityIndicatorDB, private location: Location,
    private facilityDataCollectionDbService: FacilityDataCollectionStatusDb,
    private loadingSpinnerService: LoadingSpinnerService) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.collectFor = parseInt(this.route.snapshot.paramMap.get('collectFor'));
    this.loadingSpinnerService.showLoadingScreen("collectedFacilityList");

    this.loadAllFacility().then((facilitiesResult) => {
      this.loadFacilityIndicators().then((facilityIndicatorResults) => {
        this.listFacilityIndicators = facilityIndicatorResults;
        facilitiesResult.map((facility) => {

          this.facilityDataCollectionStatusGet(facility.uniqueId).then((facilityDataCollection) => {
            if (facilityDataCollection.status === CollectionStatus.Collected) {
              this.listFacility.push(facility);
              this.listFacilityDataCollectionStatus.push(facilityDataCollection);
            }
          });
          // this.dbService.isRecordCollectedForFacility(facility.uniqueId, this.instanceId).then((recordStatus) => {
          //   if (recordStatus === true) {
          //     this.listFacility.push(facility);
          //   }
          // });
        });
        this.loadAllFacilityRecords().then((facilityRecordsResult) => {
          this.listFacilityRecords = facilityRecordsResult;

          this.loadingSpinnerService.hideLoadingScreen("collectedFacilityList");
        });
      });
    });
  }

  facilityDataCollectionStatusGet(facilityUniqueId) {
    return new Promise<FacilityDataCollectionStatus>((resolve, reject) => {
      this.facilityDataCollectionDbService.getFacilityDataCollectionStatus(facilityUniqueId, this.instanceId)
        .subscribe((result) => {
          resolve(result[0]);
        });
    });
  }

  loadAllFacilityRecords() {
    return new Promise<any>((resolve, reject) => {
      this.facilityRecordDbService.getAllFacilityRecords().subscribe((facilityRecordsResult) => {
        resolve(facilityRecordsResult);
      });
    });
  }

  async onClickUpload() {

    let uploadPromises = [];

    for (const facility of this.listFacility) {

      let filteredFacilityRecords = this.listFacilityRecords.filter(x => x.facilityId == facility.uniqueId
        && x.instanceId == this.instanceId);
      let facilityDynamicCell = new facilityDynamicCellAddViewModel();

      this.insertIntoFacilityDynamicCellModel(facility, filteredFacilityRecords, facilityDynamicCell);
      uploadPromises.push(await this.saveIntoFacilityCell(facilityDynamicCell, facility, filteredFacilityRecords));
    }
    Promise.all(uploadPromises).then(() => {
      this.location.back();
      this.toastrService.success(MessageConstant.UploadedSuccessfully);
    });
  }

  saveIntoFacilityCell(facilityDynamicCell, facility: Facility, filteredFacilityRecords: FacilityRecord[]) {
    return new Promise<any>(async (resolve, reject) => {

      await this.onlineFacilityService.saveFacilityCell(facilityDynamicCell).then(async (uploadedResponse) => {
        this.deleteAllFacilityInfo(filteredFacilityRecords, facility).then(() => {
          resolve();
        });
      }, (err) => {
        reject(err);
      });
    });
  }

  deleteAllFacilityInfo(filteredFacilityRecords: FacilityRecord[], facility: Facility) {
    return new Promise<any>((resolve, reject) => {

      let facilityRecordPromises = [];

      filteredFacilityRecords.map((record) => {
        facilityRecordPromises.push(this.facilityRecordDbService.deleteFacilityRecord(record.id));
      });

      Promise.all(facilityRecordPromises).then((data) => {
        var facilityDataCollection = this.listFacilityDataCollectionStatus.find(x => x.facilityId === facility.uniqueId && x.instanceId === this.instanceId);
        this.facilityDataCollectionDbService.deleteFacilityDataCollectionStatus(facilityDataCollection.id).subscribe(() => {
          resolve(data);
        });
      });
    });
  }

  insertIntoFacilityDynamicCellModel(facility, filteredFacilityRecords: FacilityRecord[], facilityDynamicCell) {

    facilityDynamicCell.dynamicCells = [];
    facilityDynamicCell.facilityId = facility.id;
    facilityDynamicCell.instanceId = this.instanceId;

    for(let filteredRecord of filteredFacilityRecords){
      let indicator = this.listFacilityIndicators.find(x => x.id === filteredRecord.columnId);

      if(filteredRecord.value === ''){continue;}

      let data = {
        entityDynamicColumnId: indicator.entityDynamicColumnId,
        value: filteredRecord.value.split(',')
      }
      facilityDynamicCell.dynamicCells.push(data);
    }
    
  }

  loadAllFacility() {
    return new Promise<Facility[]>((resolve, reject) => {
      this.facilityDbService.getAllFacility().subscribe((facilities) => {
        resolve(facilities);
      });
    });
  }

  loadFacilityIndicators() {
    return new Promise<FacilityIndicator[]>((resolve, reject) => {
      this.facilityIndicatorDbService.getFacilityIndicatorsByInstanceId(this.instanceId).subscribe((indicatorResult) => {
        resolve(indicatorResult);
      });
    });
  }

}
