import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityType } from 'src/app/_enums/entityType';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { Beneficiary, Facility, BeneficiaryRecord, BeneficiaryIndicator } from 'src/app/models/idbmodels/indexedDBModels';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';
import { BeneficiaryDynamicCellAddViewModel } from 'src/app/models/viewModel/facilityDynamicCellAddViewModel';
import { BeneficiaryIndicatorDb } from 'src/app/localdb/BeneficiaryIndicatorDb';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { FacilityDataCollectionStatusDb } from 'src/app/localdb/FacilityDataCollectionStatusDb';
import { BeneficiaryDataCollectionStatusDb } from 'src/app/localdb/BeneficiaryDataCollectionStatusDb';
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
  listBeneficiary: Beneficiary[] = [];
  listBeneficiaryIndicators: BeneficiaryIndicator[];

  constructor(private route: ActivatedRoute, private router: Router, private dbService: IndexedDbService,
    private beneficiaryDbService: BeneficiaryDb, private facilityDbService: FacilityDb,
    private beneficiaryRecordDbService: BeneficiaryRecordsDB, private beneficiaryIndicatorDbService: BeneficiaryIndicatorDb,
    private onlineFacilityService: OnlineFacilityService, private toastrService: ToastrService,
    private beneficiaryDataCollectionDbService:BeneficiaryDataCollectionStatusDb,
    private loadingSpinnerService:LoadingSpinnerService) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.collectFor = parseInt(this.route.snapshot.paramMap.get('collectFor'));

    this.loadAllFacility().then(facilitiesResults => {
      this.loadBeneficiaryIndicators().then((beneficiaryIndicatorResults) => {
        this.listBeneficiaryIndicators = beneficiaryIndicatorResults;
        this.loadingSpinnerService.showLoadingScreen("allFacilitiesListCollectedBeneficiary");

        this.loadFacilitiesOfBeneficiaryRecordCollected(facilitiesResults).then(() => {
          this.loadingSpinnerService.hideLoadingScreen("allFacilitiesListCollectedBeneficiary");
        });
      });
    });
  }

  private loadFacilitiesOfBeneficiaryRecordCollected(facilitiesResults: Facility[]) {
    return new Promise<any>((resolve,reject) => {
      let facilitiesPromises = [];

      facilitiesResults.forEach((facility) => {
        facilitiesPromises.push(this.loadBeneficiaryByFacility(facility.id).then(beneficiaries => {
  
          this.loadDisengagedBeneficiaryIfExists(beneficiaries, facility);
  
          this.loadBeneficiariesIfRecordCollected(beneficiaries, facility);
        }));
      });
      Promise.all(facilitiesPromises).then(() => {
        resolve();
      });
    });
    
  }

  loadDisengagedBeneficiaryIfExists(beneficiaries:Beneficiary[],facility){
    beneficiaries.forEach(beneficiary => {
      if(beneficiary.disengaged === true){
        if (!this.listFacility.includes(facility)) {
          this.listFacility.push(facility);
        }
      }
    });
  }

  loadBeneficiariesIfRecordCollected(beneficiaries, facility) {

    beneficiaries.forEach(beneficiary => {
      this.beneficiaryDataCollectionDbService.getBeneficiaryDataCollectionStatus(beneficiary.uniqueId,
        this.instanceId).subscribe((results) => {
          if(results && results.length>0){
            let beneficiaryDataCollection = results[0];
          
            if ((!this.listFacility.includes(facility)) && beneficiaryDataCollection.status === CollectionStatus.Collected) {
            this.listFacility.push(facility);
            }
          }
        });
      // this.dbService.isRecordCollectedForBeneficiary(beneficiary.uniqueId, this.instanceId).then((status) => {
      //   if (status == true) {
      //     if (!this.listFacility.includes(facility)) {
      //       this.listFacility.push(facility);
      //     }
      //   }
      // });
    });

  }

  navigateToLink(facility) {
    this.router.navigate(['/unicefpwa/upload-beneficiary-instance/beneficiary-status', this.instanceId, facility.id]);
  }

  loadBeneficiaryIndicators() {
    return new Promise<BeneficiaryIndicator[]>((resolve, reject) => {
      this.beneficiaryIndicatorDbService.getBeneficiaryIndicatorsByInstanceId(this.instanceId)
        .subscribe((indicatorResult) => {
          resolve(indicatorResult);
        });
    });
  }
  loadBeneficiaryByFacility(facilityId) {
    return new Promise<Beneficiary[]>((resolve, reject) => {
      this.beneficiaryDbService.getAllBeneficiary().subscribe((results) => {
        var beneficiariesByFacilityId = results.filter(x => x.facilityId == facilityId);

        resolve(beneficiariesByFacilityId);
      })
    })
  }

  loadAllFacility() {
    return new Promise<Facility[]>((resolve, reject) => {
      this.facilityDbService.getAllFacility().subscribe((data) => {
        resolve(data);
      })
    })
  }
  // loadAllBeneficiary(){
  //   return new Promise<Beneficiary[]>((resolve,reject) => {
  //     this.beneficiaryDbService.getAllBeneficiary().subscribe((dbResult) => {
  //       resolve(dbResult);
  //     });
  //   });
  // }

  // loadFacilityById(facilityId){
  //   return new Promise<any>((resolve,reject) => {
  //     this.facilityDbService.getFacilityById(facilityId).subscribe((facilityResult) => {
  //       resolve(facilityResult[0]);
  //     });
  //   });
  // }

}
