import { Component, OnInit } from '@angular/core';
import { EntityType } from 'src/app/_enums/entityType';
import { ActivatedRoute, Router } from '@angular/router';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { Facility, FacilityRecord, FacilityDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { BeneficiaryScheduleInstanceDb } from 'src/app/localdb/BeneficiaryScheduleInstanceDb';
import { FacilityScheduleInstanceDB } from 'src/app/localdb/FacilityScheduleInstanceDB';
import { FacilityRecordsDB } from 'src/app/localdb/FacilityRecordsDB';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { PaginationInstance } from 'ngx-pagination';
import { FacilityDataCollectionStatusDb } from 'src/app/localdb/FacilityDataCollectionStatusDb';

@Component({
  selector: 'app-all-facility',
  templateUrl: './all-facility.component.html',
  styleUrls: ['./all-facility.component.scss']
})
export class AllFacilityComponent implements OnInit {

  instanceId: number;
  collectFor: EntityType;
  listFacility: Facility[] = [];
  status: OnlineOfflineStatus;
  collectionStatus: CollectionStatus;
  scheduleInstance: any;
  tempListFacilitySearching: Facility[];
  listFacilityRecords: FacilityRecord[] = [];
  offlinePageNo: number = 1;

  paginationConfig: PaginationInstance = {
    id: 'all_facility_table',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  constructor(private route: ActivatedRoute, private router: Router, private facilityDbService: FacilityDb,
    private dbService: IndexedDbService, private beneficiaryScheduleInstanceDbService: BeneficiaryScheduleInstanceDb,
    private facilityScheduleInstanceDbService: FacilityScheduleInstanceDB, private facilityRecordDbService: FacilityRecordsDB,
    private toastrService: ToastrService, private facilityDataCollectionDbService: FacilityDataCollectionStatusDb) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('id'));
    this.collectFor = parseInt(this.route.snapshot.paramMap.get('collectFor'));
    this.status = OnlineOfflineStatus.Offline;

    this.loadScheduleInstance().then((scheduleInstanceResult) => {
      this.scheduleInstance = scheduleInstanceResult;

      this.loadAllFacility().then(facilities => {

        facilities.map((facility) => {
          if (this.collectFor === EntityType.Facility) {
            this.loadFacilityDataCollectionStatus(facility.uniqueId).then((facilityDataCollection) => {
              if (facilityDataCollection) {
                facility.collectionStatus = facilityDataCollection.status;
                this.listFacility.push(facility);
              }
            });
            this.tempListFacilitySearching = this.listFacility;
          }
          else{
            this.listFacility = facilities;
            this.tempListFacilitySearching = this.listFacility;
          }
        });
        // this.loadFacilityRecords().then((facilityRecords) => {
        //   this.listFacilityRecords = facilityRecords;

        //   this.listFacility.map(facility => {
        //     this.computeCollectionStatus(facility).then((recordStatus) => {
        //       facility["collectionStatus"] = recordStatus;
        //     });
        //   });
        // });
      });
    })
  }

  loadFacilityDataCollectionStatus(facilityId) {
    return new Promise<FacilityDataCollectionStatus>((resolve, reject) => {
      this.facilityDataCollectionDbService.getFacilityDataCollectionStatus(facilityId, this.instanceId).subscribe((data) => {
        resolve(data[0]);
      });
    });
  }

  loadFacilityRecords() {
    return new Promise<any>((resolve, reject) => {
      this.facilityRecordDbService.getAllFacilityRecords().subscribe((data) => {
        resolve(data);
      });
    });
  }

  computeCollectionStatus(facility: Facility) {
    return new Promise<any>((resolve, reject) => {

      // let filteredFacilityRecords = this.listFacilityRecords.filter(x => x.facilityId == facility.uniqueId && x.instanceId == this.instanceId);
      // if(filteredFacilityRecords.length <= 0){
      //   resolve(CollectionStatus.Uploaded);
      // }
      this.dbService.isRecordCollectedForFacility(facility.uniqueId, this.instanceId).then((recordStatus) => {
        if (recordStatus === true) {

          resolve(CollectionStatus.Collected);
        }
        resolve(CollectionStatus.NotCollected);
      });
    });
  }

  //For Offline , when all facility list loaded
  searchFacilityonKeyup(event: KeyboardEvent) {
    let searchResult = (event.target as HTMLInputElement).value;

    this.listFacility = this.tempListFacilitySearching.filter(
      x => x.facilityName.toLowerCase().includes(searchResult.toLowerCase()) ||
        x.facilityCode.toLowerCase().includes(searchResult.toLowerCase()));
  }

  CollectionStatusText(id) {
    return CollectionStatus[id];
  }

  navigateToLink(facility) {
    if (this.collectFor == EntityType.Facility) {
      // if(facility.collectionStatus === CollectionStatus.Uploaded){
      //   this.toastrService.warning(MessageConstant.dataIsAlreadyUploaded);
      //   return;
      // }
      this.router.navigate(['/unicefpwa/offline-facility-instance/indicator', this.instanceId, facility.uniqueId]);
    }
    else {
      this.router.navigate(['/unicefpwa/offline-beneficiary-instance/beneficiary', this.instanceId, facility.id]);
    }
  }

  loadScheduleInstance() {
    return new Promise<any>((resolve, reject) => {
      if (this.collectFor === EntityType.Beneficiary) {
        this.beneficiaryScheduleInstanceDbService.getScheduleInstanceById(this.instanceId).subscribe((data) => {
          resolve(data);
        })
      }
      else {
        this.facilityScheduleInstanceDbService.getFacilityScheduleInstanceById(this.instanceId).subscribe((data) => {
          resolve(data);
        });
      }
    });
  }

  loadAllFacility() {
    return new Promise<Facility[]>((resolve, reject) => {
      this.facilityDbService.getAllFacility().subscribe((data) => {
        resolve(data);
      })
    })
  }

}
