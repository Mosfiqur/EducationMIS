import { Component, OnInit } from '@angular/core';
import { Beneficiary, Facility, BeneficiaryRecord, BeneficiaryIndicator, BeneficiaryDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { ActivatedRoute, Router } from '@angular/router';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';
import { BeneficiaryDynamicCellAddViewModel } from 'src/app/models/viewModel/facilityDynamicCellAddViewModel';
import { BeneficiaryIndicatorDb } from 'src/app/localdb/BeneficiaryIndicatorDb';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ToastrService } from 'ngx-toastr';
import { ThrowStmt } from '@angular/compiler';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { BeneficiaryFacilityViewModel } from 'src/app/models/viewModel/BeneficiaryFacilityViewModel';
import {Location} from '@angular/common';
import { BeneficiaryDataCollectionStatusDb } from 'src/app/localdb/BeneficiaryDataCollectionStatusDb';
import { LoadingSpinnerService } from 'src/app/core/loading-spinner/loading-spinner.service';

@Component({
  selector: 'app-ben-status',
  templateUrl: './ben-status.component.html',
  styleUrls: ['./ben-status.component.scss']
})
export class BenStatusComponent implements OnInit {

  instanceId: number;
  facilityId: number;
  listBeneficiary: Beneficiary[] = [];
  listDisengagedBeneficiary: Beneficiary[] = [];
  facility: Facility;
  listBeneficiaryIndicators: BeneficiaryIndicator[];
  listBeneficiaryRecords: BeneficiaryRecord[];
  listBeneficiaryDataCollectionStatus:BeneficiaryDataCollectionStatus[] = [];

  constructor(private route: ActivatedRoute, private beneficiaryDbService: BeneficiaryDb,
    private facilityDbService: FacilityDb, private dbService: IndexedDbService, private toastrService: ToastrService,
    private router: Router, private beneficiaryRecordDbService: BeneficiaryRecordsDB,
    private beneficiaryIndicatorDbService: BeneficiaryIndicatorDb, private onlineFacilityService: OnlineFacilityService,
    private onlineBeneficiaryService: OnlineBeneficiaryService,private location:Location,
    private beneficiaryDataCollectionDbService:BeneficiaryDataCollectionStatusDb,
    private loadingSpinnerService:LoadingSpinnerService) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));
    this.loadingSpinnerService.showLoadingScreen("beneficiaryAllData");
    
    this.loadFacilityById().then(facility => {
      this.facility = facility;

      this.loadBeneficiaryIndicators().then((beneficiaryIndicatorResults) => {
        this.listBeneficiaryIndicators = beneficiaryIndicatorResults;

        this.loadBeneficiaryByFacility(facility.id).then(beneficiaries => {
          this.loadDisengagedBeneficiaryIfExists(beneficiaries);

          this.loadBeneficiariesIfRecordCollected(beneficiaries);

          this.loadAllBeneficiaryRecords().then((beneficiaryRecordResults) => {
            this.listBeneficiaryRecords = beneficiaryRecordResults;

            this.loadingSpinnerService.hideLoadingScreen("beneficiaryAllData");
          });
        });
      });
    });
  }

  loadDisengagedBeneficiaryIfExists(beneficiaries: Beneficiary[]) {
    beneficiaries.forEach(beneficiary => {
      if (beneficiary.disengaged === true) {
        this.listDisengagedBeneficiary.push(beneficiary);
      }
    });
  }

  loadBeneficiariesIfRecordCollected(beneficiaries) {
    beneficiaries.forEach(beneficiary => {
      if(beneficiary.disengaged !== true){
        this.beneficiaryDataCollectionDbService.getBeneficiaryDataCollectionStatus(beneficiary.uniqueId,
          this.instanceId).subscribe((result) => {
            let beneficiaryDataCollection = result[0];
            
            if (beneficiaryDataCollection.status === CollectionStatus.Collected) {
              this.listBeneficiary.push(beneficiary);
              this.listBeneficiaryDataCollectionStatus.push(beneficiaryDataCollection);
            }
          });
      }
      // this.dbService.isRecordCollectedForBeneficiary(beneficiary.uniqueId, this.instanceId).then((status) => {
      //   if (status == true) {
      //     this.listBeneficiary.push(beneficiary);
      //   }
      // });
    });
  }

  // Upload Related All Method 

  loadAllBeneficiaryRecords() {
    return new Promise<BeneficiaryRecord[]>((resolve, reject) => {
      this.beneficiaryRecordDbService.getAllBeneficiaryRecords().subscribe((beneficiaryRecordResult) => {
        resolve(beneficiaryRecordResult);
      });
    });
  }

  newBeneficiaryUpdate(updatedBeneficiary:Beneficiary){
    return new Promise<any>((resolve,reject) => {
      this.beneficiaryDbService.updateBeneficiary(updatedBeneficiary).subscribe((updatedBeneficiaryDb) => {
        resolve(updatedBeneficiaryDb);
      })
    });
  }

  CreateForNewBeneficiary() {
    return new Promise<any>((resolve, reject) => {

      if(this.listBeneficiary.filter(x => x.id === 0).length <= 0){resolve();}
      
      this.listBeneficiary.filter(x => x.id === 0).forEach(beneficiary => {
        this.onlineBeneficiaryService.saveBeneficiary(
          {
            unhcrId: beneficiary.UnhcrId,
            name: beneficiary.beneficiaryName,
            fatherName: beneficiary.fatherName,
            motherName: beneficiary.motherName,
            fcnId: beneficiary.FCNId,
            dateOfBirth: beneficiary.dateOfBirth,
            sex: beneficiary.sex,
            disabled: beneficiary.disabled,
            levelOfStudy: beneficiary.levelOfStudy,
            enrollmentDate: beneficiary.enrollmentDate,
            facilityId: beneficiary.facilityId,
            facilityCampId: beneficiary.beneficiaryCampId,
            beneficiaryCampId: beneficiary.beneficiaryCampId,
            blockId: beneficiary.blockId,
            subBlockId: beneficiary.subBlockId,
            remarks: beneficiary.remarks,
            instanceId: this.instanceId
          }).then((responseData) => {
            beneficiary.id = responseData.id;
            this.newBeneficiaryUpdate(beneficiary).then(() => {
              resolve(responseData);
            });
          });
      });
    });
  }

  disengagedBeneficiaryDeactivate() {
    return new Promise<any>((resolve, reject) => {
      let disengagedBeneficiaryIds = [];

      this.listDisengagedBeneficiary.forEach(disengagedBeneficiary => {
        disengagedBeneficiaryIds.push(disengagedBeneficiary.id);
      });

      if (disengagedBeneficiaryIds.length <= 0) {
        resolve();
      }
      else {
        
        this.onlineBeneficiaryService.DeactivateBeneficiary({
          beneficiaryIds: disengagedBeneficiaryIds,
          instanceId: this.instanceId
        }).then((data) => {
          this.listDisengagedBeneficiary.forEach(disengagedBeneficiary => {
            this.beneficiaryDbService.deleteBeneficiary(disengagedBeneficiary.uniqueId);
          });
          resolve(data);
        });
      }
    });
  }

  insertBeneficiaryDynamicCellModel(beneficiary: Beneficiary, filteredBeneficiaryRecords: BeneficiaryRecord[], beneficiaryDynamicCell) {
    return new Promise<any>((resolve, reject) => {
      beneficiaryDynamicCell.dynamicCells = [];
      beneficiaryDynamicCell.beneficiaryId = beneficiary.id;
      beneficiaryDynamicCell.instanceId = this.instanceId;

      for(let filteredRecord of filteredBeneficiaryRecords){
        let indicator = this.listBeneficiaryIndicators.find(x => x.id === filteredRecord.columnId);
 
        if(filteredRecord.value === ''){continue;}
        
        let data = {
          entityDynamicColumnId: indicator.entityDynamicColumnId,
          value: filteredRecord.value.split(',')
        }
        beneficiaryDynamicCell.dynamicCells.push(data);
    
      resolve(beneficiaryDynamicCell);
      }
    });
    //   filteredBeneficiaryRecords.map((filteredRecord) => {
    //     let indicator = this.listBeneficiaryIndicators.find(x => x.id === filteredRecord.columnId);

    //     let data = {
    //       entityDynamicColumnId: indicator.entityDynamicColumnId,
    //       value: filteredRecord.value.split(',')
    //     }
    //     beneficiaryDynamicCell.dynamicCells.push(data);
    //   });

    //   resolve(beneficiaryDynamicCell);
    // });
  }

  async saveIntoBeneficiaryCell(beneficiaryDynamicCell, beneficiary: Beneficiary, filteredBeneficiaryRecords: BeneficiaryRecord[]) {
    return new Promise<any>(async (resolve, reject) => {
      debugger;
      await this.onlineFacilityService.saveBeneficiaryCell(beneficiaryDynamicCell).then(async (uploadedResponse) => {
        await this.deleteAllBeneficiaryInfo(filteredBeneficiaryRecords,beneficiary).then(() => {
          resolve();
        });
      }, (err) => {
        reject(err);
      });
    });
  }

  deleteAllBeneficiaryInfo(filteredBeneficiaryRecords: BeneficiaryRecord[], beneficiary: Beneficiary) {
    return new Promise<any>((resolve, reject) => {

      let beneficiaryRecordPromises = [];

      filteredBeneficiaryRecords.map((filteredRecord) => {
        beneficiaryRecordPromises.push(this.beneficiaryRecordDbService.deleteBeneficiaryRecord(filteredRecord.id));
      });

      Promise.all(beneficiaryRecordPromises).then((data) => {
        let beneficiaryDataCollection = this.listBeneficiaryDataCollectionStatus.find(x => x.beneficiaryId === beneficiary.uniqueId && x.instanceId === this.instanceId)
        this.beneficiaryDataCollectionDbService.deleteBeneficiaryDataCollectionStatus(beneficiaryDataCollection.id).subscribe(() => {
          resolve(data);
        });
        // this.beneficiaryDbService.deleteBeneficiary(beneficiary.uniqueId).subscribe((data) => {
        //   resolve(data);
        // });
      });
    });
  }


  onClickUpload() {

    this.CreateForNewBeneficiary().then(() => {
      this.disengagedBeneficiaryDeactivate().then(async () => {

        const uploadedPromises = [];

        for (const beneficiary of this.listBeneficiary) {

          let filteredBeneficiaryRecords = this.listBeneficiaryRecords.filter(x => x.beneficiaryId ==
            beneficiary.uniqueId && x.instanceId == this.instanceId);
          let beneficiaryDynamicCell = new BeneficiaryDynamicCellAddViewModel();

          this.insertBeneficiaryDynamicCellModel(beneficiary, filteredBeneficiaryRecords, beneficiaryDynamicCell);

          uploadedPromises.push(await this.saveIntoBeneficiaryCell(beneficiaryDynamicCell, beneficiary, filteredBeneficiaryRecords));
        }

        Promise.all(uploadedPromises).then((uploadedData) => {
          console.log(uploadedData);
          this.toastrService.success(MessageConstant.UploadedSuccessfully);
          this.location.back();
        }, (err) => {
          console.log(err);
        });
      });
    });

    // let uploadPromises = [];

    // this.CreateForNewBeneficiary().then(() => {
    //   this.disengagedBeneficiaryDeactivate().then(() => {

    //     for (let beneficiary of this.listBeneficiary) {
    //       this.getFilteredBeneficiaryRecords(beneficiary).then((filteredBeneficiaryRecords) => {
    //         let beneficiaryDynamicCell = new BeneficiaryDynamicCellAddViewModel();

    //         this.insertBeneficiaryDynamicCellModel(beneficiary, filteredBeneficiaryRecords, beneficiaryDynamicCell);

    //         uploadPromises.push(this.saveIntoBeneficiaryCell(beneficiaryDynamicCell, beneficiary, filteredBeneficiaryRecords));
    //       });
    //     }

    //   });
    // });
    // Promise.all(uploadPromises).then((uploadedData) => {
    //   console.log(uploadedData);
    //   this.toastrService.success(MessageConstant.UploadedSuccessfully);
    // });
    // this.listBeneficiary.map((beneficiary) => {
    //   this.beneficiaryRecordDbService.getAllBeneficiaryRecords().subscribe((beneficiaryRecordResult) => {

    //     let filteredBeneficiaryRecords = beneficiaryRecordResult.filter(x => x.beneficiaryId == beneficiary.uniqueId
    //       && x.instanceId == this.instanceId);

    //     let beneficiaryDynamicCell = new BeneficiaryDynamicCellAddViewModel();
    //     this.insertIntoBeneficiaryDynamicCellModel(beneficiary, filteredBeneficiaryRecords, beneficiaryDynamicCell).then(() => {
    //       let uploadPromises = [];

    //       uploadPromises.push(this.saveIntoBeneficiaryCell(beneficiaryDynamicCell, beneficiary, filteredBeneficiaryRecords));

    //       Promise.all(uploadPromises).then((uploadedResponse) => {
    //         console.log(uploadedResponse);
    //         this.toastrService.success(MessageConstant.UploadedSuccessfully);
    //       });
    //     });
    //     // this.onlineFacilityService.saveBeneficiaryCell(beneficiaryDynamicCell).then((uploadedData) => {
    //     //   console.log(uploadedData);

    //     //   this.toastrService.success(MessageConstant.UploadedSuccessfully);

    //     //   filteredBeneficiaryRecords.map((record) => {
    //     //     record.status = CollectionStatus.NotCollected;
    //     //     record.value = "";

    //     //     this.dbService.SaveBeneficiaryRecord(record, false);
    //     //   });
    //     // });
    //   });
    // });
  }

  // insertIntoBeneficiaryDynamicCellModel(beneficiary: Beneficiary, filteredBeneficiaryRecords: BeneficiaryRecord[], beneficiaryDynamicCell) {
  //   return new Promise<any>((resolve, reject) => {
  //     if (beneficiary.id === 0) {
  //       this.createBeneficiaryIfNotExists(beneficiary).then((responseData) => {
  //         beneficiary.id = responseData.id;

  //         resolve(this.insertBeneficiaryDynamicCellModel(beneficiary, filteredBeneficiaryRecords, beneficiaryDynamicCell));
  //       });
  //     }
  //     else {
  //       resolve(this.insertBeneficiaryDynamicCellModel(beneficiary, filteredBeneficiaryRecords, beneficiaryDynamicCell));
  //     }
  //   });
  // }

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


}
