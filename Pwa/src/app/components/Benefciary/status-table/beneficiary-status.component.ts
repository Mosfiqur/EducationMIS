import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { Beneficiary, Facility, BeneficiaryRecord, BeneficiaryDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { ToastrService } from 'ngx-toastr';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';
import { PaginationInstance } from 'ngx-pagination';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { BeneficiaryDataCollectionStatusDb } from 'src/app/localdb/BeneficiaryDataCollectionStatusDb';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'beneficiary-status',
  templateUrl: './beneficiary-status.component.html',
  styleUrls: ['./beneficiary-status.component.scss']
})
export class BeneficiaryStatComponent implements OnInit {
  private allRecords = 2147483647;
  @Input() onlineOfflineStatus: OnlineOfflineStatus;
  @Input() redirectTo: string;
  instanceId: number;
  facilityId: number;
  listBeneficiary: any[] = [];
  listBeneficiaryRecords:BeneficiaryRecord[] = [];
  tempListBeneficiarySearching: any[];
  facility: Facility;
  beneficiaryCollectionStatus: CollectionStatus;
  offlinePageNo:number = 1;

  private _beneficiarySearchText: string = "";
  private beneficiarySearchText$: Subject<string> = new Subject<string>();

  get beneficiarySearchText(): string {
    return this._beneficiarySearchText;
  }
  set beneficiarySearchText(val: string){
    this.beneficiarySearchText$.next(val);
  }

  
  paginationConfig: PaginationInstance = {
    id: 'beneficiary_status_table',
    itemsPerPage: 5,
    currentPage: 1,
    totalItems: 0
  }

  constructor(private route: ActivatedRoute, private beneficiaryDbService: BeneficiaryDb, private facilityDbService: FacilityDb,
    private router: Router, private onlineBeneficiaryService: OnlineBeneficiaryService,
    private onlineFacilityService: OnlineFacilityService, private tosterService: ToastrService,
    private dbService: IndexedDbService,private beneficiaryRecordDbService:BeneficiaryRecordsDB,
    private beneficiaryDataCollectionDbService:BeneficiaryDataCollectionStatusDb) { 

      this.beneficiarySearchText$.pipe(
        debounceTime(800),
        distinctUntilChanged()
      ).subscribe(text => {
        this._beneficiarySearchText = text;
        if(this._beneficiarySearchText === ""){
          this.loadBeneficiaryByFacilityOnline(1,this.paginationConfig.itemsPerPage,"");
        }
        else{
          this.loadBeneficiaryByFacilityOnline(1,this.allRecords,this.beneficiarySearchText);
        }
      });    
    }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));

    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {

      this.loadFacilityById().then(facilityResult => {
        this.facility = facilityResult;

        this.loadBeneficiaryByFacility().then(beneficiaries => {
          // this.listBeneficiary = results;
          // this.tempListBeneficiarySearching = this.listBeneficiary;

          beneficiaries.map(beneficiary => {
            this.loadBeneficiaryDataCollectionStatus(beneficiary.uniqueId).then((beneficiaryDataCollection) => {
              if (beneficiaryDataCollection) {
                if(beneficiary.disengaged === true){
                  beneficiary.collectionStatus = CollectionStatus.Disengaged;
                }
                else{
                  beneficiary.collectionStatus = beneficiaryDataCollection.status;
                }
                this.listBeneficiary.push(beneficiary);
              }
            });
          });
          this.tempListBeneficiarySearching = this.listBeneficiary;

          // this.loadBeneficiaryRecords().then((recordResult) =>{
          //   this.listBeneficiaryRecords = recordResult;

          //   this.computeCollectionStatus(results);
          // });
        });
      });
    }
    else {

      this.loadFacilityByIdOnline().then(facilityResult => {
        this.facility = facilityResult;

        this.loadBeneficiaryByFacilityOnline(1,this.allRecords,"").then(results => {
          this.listBeneficiary = results;

        });
      });
    }
  }

  searchBeneficiaryOfflineonKeyup(event: KeyboardEvent){
    let searchResult = (event.target as HTMLInputElement).value;

    this.listBeneficiary = this.tempListBeneficiarySearching.filter(
      x => x.UnhcrId.toLowerCase().includes(searchResult.toLowerCase()) || 
           x.beneficiaryName.toLowerCase().match(searchResult.toLowerCase()));
  }



  //Offline

  loadBeneficiaryRecords(){
    return new Promise<any>((resolve,reject) => {
      this.beneficiaryRecordDbService.getAllBeneficiaryRecords().subscribe((data) => {
        resolve(data);
      });
    });
  }

  loadBeneficiaryDataCollectionStatus(beneficiaryId){
    return new Promise<BeneficiaryDataCollectionStatus>((resolve,reject) => {
      this.beneficiaryDataCollectionDbService.getBeneficiaryDataCollectionStatus(beneficiaryId,this.instanceId)
        .subscribe((data) => {
          resolve(data[0]);
        });
    });
  }

  computeCollectionStatus(results) {
    results.map(beneficiary => {

      if(beneficiary.disengaged === true){
        beneficiary.collectionStatus = CollectionStatus.Disengaged;
        return;
      }

      // let filteredBeneficiaryRecords = this.listBeneficiaryRecords.filter(x => x.beneficiaryId == beneficiary.uniqueId && x.instanceId == this.instanceId);
      // if(filteredBeneficiaryRecords.length <= 0){
      //   beneficiary.collectionStatus = CollectionStatus.Uploaded;
      //   return;
      // }

      this.dbService.isRecordCollectedForBeneficiary(beneficiary.uniqueId, this.instanceId).then((recordStatus) => {
        if (recordStatus === true) {
          beneficiary.collectionStatus = CollectionStatus.Collected;
        }
        else {
          beneficiary.collectionStatus = CollectionStatus.NotCollected;
        }
      });
    });
  }

  loadBeneficiaryByFacility() {
    return new Promise<Beneficiary[]>((resolve, reject) => {
      this.beneficiaryDbService.getAllBeneficiary().subscribe((results) => {
        var beneficiariesByFacilityId = results.filter(x => x.facilityId == this.facilityId);

        resolve(beneficiariesByFacilityId);
      })
    })
  }

  loadFacilityById() {
    return new Promise<Facility>((resolve, reject) => {
      this.facilityDbService.getFacilityById(this.facilityId).subscribe((data) => {
        resolve(data[0]);
      })
    })
  }

  CollectionStatusText(id) {
    return CollectionStatus[id];
  }

  navigateToBeneficiaryCreate(facilityId){
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {
      this.router.navigate(['/unicefpwa/offline-beneficiary-instance/beneficiary-create', facilityId,this.instanceId]);
    }
    else {
      this.router.navigate(['/unicefpwa/online-beneficiary-instance/beneficiary-create',facilityId,this.instanceId]);
    }
  }

  navigateToBeneficiaryIndicator(beneficiary) {
    if(beneficiary.collectionStatus === CollectionStatus.Requested_Inactive){
      this.tosterService.warning(MessageConstant.requestForInactive);
      return;
    }
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {
      this.router.navigate(['/unicefpwa/offline-beneficiary-instance/indicator', this.instanceId, beneficiary.uniqueId]);
    }
    else {
      this.router.navigate(['/unicefpwa/online-beneficiary-instance/indicator', this.instanceId, beneficiary.entityId]);
    }
  }


  // Online

  loadFacilityByIdOnline() {
    return new Promise<any>((resolve, reject) => {

      var data = this.onlineFacilityService
        .getFacilityById(this.facilityId)
        .then((data) => {
          resolve(data);
        });
    });
  }

  async getPage(pageNo: number) {
    if (this.onlineOfflineStatus === OnlineOfflineStatus.Online){
      await this.loadBeneficiaryByFacilityOnline(pageNo,this.paginationConfig.itemsPerPage,this._beneficiarySearchText);
    }
  }

  loadBeneficiaryByFacilityOnline(pageNo: number,pageSize,searchText) {
    return new Promise<any>((resolve, reject) => {
      this.onlineBeneficiaryService.GetByFacilityId(this.instanceId, this.facilityId, this.paginationConfig.itemsPerPage, pageNo,searchText).then((data) => {

          this.paginationConfig.currentPage = pageNo;
          this.paginationConfig.totalItems = data.total;
          this.listBeneficiary = data.data;
          resolve(data.data);
      });
    });
  }
}
