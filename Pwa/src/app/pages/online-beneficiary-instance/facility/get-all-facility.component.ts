import { Component, OnInit } from '@angular/core';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityType } from 'src/app/_enums/entityType';
import { Camp, Block, SubBlock, BeneficiaryScheduleInstance, Facility, Beneficiary, BeneficiaryIndicator, BeneficiaryRecord, FacilityScheduleInstance, FacilityIndicator, FacilityRecord, ListItem, List, BeneficiaryDataCollectionStatus, FacilityDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { CommonService } from 'src/app/services/common.service';
import { CampBlockSubBlockViewModel } from 'src/app/models/viewModel/CampBlockSubBlockViewModel';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { BeneficiaryViewModel } from 'src/app/models/viewModel/beneficiaryViewModel';
import { EntityDynamicColumn } from 'src/app/models/indicator/EntityDynamicColumn';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { IndicatorViewModel } from 'src/app/models/indicator/IndicatorViewModel';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { indicatorGetViewModel } from 'src/app/models/viewModel/indicatorGetViewModel';
import { ListDataTypeViewModel } from 'src/app/models/viewModel/ListDataTypeViewModel';
import { BeneficiaryFacilityViewModel } from 'src/app/models/viewModel/BeneficiaryFacilityViewModel';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { LoadingScreenService } from 'src/app/services/loading-screen.service';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { PaginationInstance } from 'ngx-pagination';
import { LoadingSpinnerService } from 'src/app/core/loading-spinner/loading-spinner.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-get-all-facility',
  templateUrl: './get-all-facility.component.html',
  styleUrls: ['./get-all-facility.component.scss']
})
export class GetAllFacilityComponent implements OnInit {
  private allRecords = 2147483647;
  listFacility: BeneficiaryFacilityViewModel[];
  listScheduleInstances: InstanceViewModel[];
  scheduleInstance: InstanceViewModel;
  listBeneficiary: BeneficiaryViewModel[] = [];
  instanceId: number;
  collectFor: EntityType;
  campsAll: Camp[];
  blocksAll: Block[];
  subBlocksAll: SubBlock[];
  beneficiaryScheduleInstancesAll: BeneficiaryScheduleInstance[];
  beneficiaryAll: Beneficiary[];
  beneficiaryIndicatorAll: BeneficiaryIndicator[];
  beneficiaryRecordsAll: BeneficiaryRecord[];
  indicatorAll: EntityDynamicColumn[];
  facilityIndicatorAll: FacilityIndicator[];

  private _facilitySearchText: string = "";
  private facilitySearchText$: Subject<string> = new Subject<string>();

  get facilitySearchText(): string {
    return this._facilitySearchText;
  }
  set facilitySearchText(val: string) {
    this.facilitySearchText$.next(val);
  }


  paginationConfig: PaginationInstance = {
    id: 'all_facility_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  constructor(private route: ActivatedRoute, private router: Router,
    private onlineBeneficiaryService: OnlineBeneficiaryService, private commonService: CommonService,
    private onlineFacilityService: OnlineFacilityService, private dbService: IndexedDbService,
    private toastrService: ToastrService, private loadingSpinnerService: LoadingSpinnerService) {

    this.facilitySearchText$.pipe(
      debounceTime(800),
      distinctUntilChanged()
    ).subscribe(text => {
      this._facilitySearchText = text.trim();
      if (this._facilitySearchText === "") {
        this.loadFacility(1, this.paginationConfig.itemsPerPage, "")
      }
      else {
        this.loadFacility(1, this.allRecords, this.facilitySearchText);
      }
    });
  }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('id'));
    this.collectFor = parseInt(this.route.snapshot.paramMap.get('collectFor'));
    this.loadFacility(this.paginationConfig.currentPage, this.paginationConfig.itemsPerPage, "").then(result => {
      this.listFacility = result;

      this.loadScheduleInstance().then(scheduleInstance => {
        this.listScheduleInstances = scheduleInstance;

        this.scheduleInstance = this.listScheduleInstances.find(x => x.id == this.instanceId);
      })
    })
  }
  CollectionStatusText(id) {
    return CollectionStatus[id];
  }

  navigateToLink(facility) {
    if (facility.collectionStatus === CollectionStatus.Approved) {
      this.toastrService.info(MessageConstant.facilityDataAlreadyApproved);
      return;
    }
    if (this.collectFor == EntityType.Facility) {
      this.router.navigate(['/unicefpwa/online-facility-instance/indicator', this.instanceId, facility.id]);
    }
    else {
      this.router.navigate(['/unicefpwa/beneficiary', this.instanceId, facility.id]);
    }

  }

  OnClickAvailableOffline() {
    this.loadingSpinnerService.showLoadingScreen("OnClickAvailableOffline");
    this.loadCampWithBlockSubBlock().then(results => {
      let promises = [];
      promises = this.saveOrUpdateCamps(results, promises);
    }).catch((error) => {
      this.toastrService.error("Something went Wrong - ", error);
    });
  }


  private saveOrUpdateCamps(results: CampBlockSubBlockViewModel[], promises: any[]) {
    let campPromises = [];
    results.forEach(result => {
      campPromises.push(this.dbService.SaveCamp(result.camp));
    });

    Promise.all(campPromises).then(() => {
      console.log("All Camps Offline data Upload done");
      this.saveOrUpdateBlocks(results, promises);
    });
    return promises;
  }

  private saveOrUpdateBlocks(results: CampBlockSubBlockViewModel[], promises: any[]) {
    let blockPromises = [];
    results.forEach(result => {
      result.blocks.forEach(block => {
        blockPromises.push(this.dbService.SaveBlock(block));
      });
    });

    Promise.all(blockPromises).then(() => {
      console.log("All Block Offline data Upload done");
      this.saveOrUpdateSubBlocks(results, promises);
    });
  }

  private saveOrUpdateSubBlocks(results: CampBlockSubBlockViewModel[], promises: any[]) {
    let subBlockPromises = [];
    results.forEach(result => {
      result.subBlocks.forEach(subBlock => {
        subBlockPromises.push(this.dbService.SaveSubBlock(subBlock));
      });
    });

    Promise.all(subBlockPromises).then(() => {
      console.log("All SubBlock Offline data Upload done");
      this.saveOrUpdateInstances(promises);
    });
  }

  private saveOrUpdateInstances(promises: any[]) {
    let instancePromises = [];

    var scheduleInstance = this.listScheduleInstances.filter(si => si.id == this.instanceId)[0];
    instancePromises.push(this.InsertScheduleInstance(scheduleInstance));

    // this.listScheduleInstances.forEach(scheduleInstance => {
    //   instancePromises.push(this.InsertScheduleInstance(scheduleInstance));
    // });

    Promise.all(instancePromises).then(() => {
      console.log("Schedule Instance Data Upload Done");
      this.saveOrUpdateLists(promises);
    });
  }

  private saveOrUpdateLists(promises: any[]) {
    let listPromises = [];
    this.loadListTypeItem().then((lists) => {
      lists.forEach(list => {
        listPromises.push(this.InsertListItem(list));
      });

      Promise.all(listPromises).then((listItems) => {

        console.log("List Item Inserted into IndexedDb Done of ListItems - ", listItems);
        this.saveOrUpdateFacilities(promises);
      });
    });
  }

  private saveOrUpdateFacilities(promises: any[]) {
    let facilityPromises = [];

    this.loadFacility(1, this.allRecords, "").then((facilitiesResult) => {
      this.listFacility = facilitiesResult;

      this.listFacility.forEach(facility => {
        facilityPromises.push(this.InsertFacility(facility));
      });

      Promise.all(facilityPromises).then((facilities) => {
        console.log("All Facility Offline Data Upload done of ID - ", facilities);

        if (this.collectFor == EntityType.Beneficiary) {
          let beneficiaryAllPromises = [];
          this.pushIntoLoadBeneficiary(facilities, beneficiaryAllPromises);
        }
        else if (this.collectFor == EntityType.Facility) {
          this.saveOrUpdateFacilityDataCollectionStatus(facilities);
        }
      });
    });
  }

  saveOrUpdateFacilityDataCollectionStatus(facilities: Facility[]) {
    let facilityDataCollectionPromises = [];
    facilities.forEach(facility => {
      facilityDataCollectionPromises.push(this.InsertFacilityDataCollectionStatus(facility));
    });

    Promise.all(facilityDataCollectionPromises).then((facilityDataCollectionResult) => {
      console.log("All Facility Data Collection Offline Download done. ", facilityDataCollectionResult);
      this.saveOrUpdateFacilityRecord(facilities);
    });
  }

  private saveOrUpdateFacilityRecord(facilities: any[]) {
    let indicatorAllPromises = [];

    this.loadIndicatorByInstance().then((facilityIndicators => {
      facilityIndicators.forEach(indicator => {
        indicatorAllPromises.push(this.InsertFacilityIndicator(indicator));
      });

      Promise.all(indicatorAllPromises).then((indicatorsResult) => {
        console.log("All Facility Offline Data Upload of Indicators - ", indicatorsResult);
        let facilityRecordPromises = [];

        facilities.forEach(eachFacility => {
          indicatorsResult.forEach(indicator => {

            let filteredFacility = this.listFacility.find(x => x.id === eachFacility.id);

            let facilityIndicator = filteredFacility.properties.find(x => x.entityColumnId === indicator.entityDynamicColumnId);
            facilityRecordPromises.push(this.InsertFacilityRecord(eachFacility.uniqueId,
              indicator.id, facilityIndicator, facilityIndicator.status));
          });
        });

        Promise.all(facilityRecordPromises).then((facilityRecordsResult) => {
          console.log("All Facility Offline Data Upload Done of All Facility Records.", facilityRecordsResult);

          this.loadingSpinnerService.hideLoadingScreen("OnClickAvailableOffline");
          this.toastrService.success(MessageConstant.AvailableOffline);
        });
      });
    }));
  }

  private pushIntoLoadBeneficiary(facilities: any[], beneficiaryAllPromises: any[]) {
    for (let facility of facilities) {
      beneficiaryAllPromises.push(this.loadBeneficiaryByFacility(facility.id).then((beneficiaries) => {
        if (beneficiaries && beneficiaries.length > 0) {
          beneficiaries.map(beneficiary => {
            this.listBeneficiary.push(beneficiary);
          });
        }
      }));
    }

    Promise.all(beneficiaryAllPromises).then(() => {
      if (this.listBeneficiary.length > 0) {
        this.saveOrUpdateBeneficiaries();
      }
      else {
        this.toastrService.error(MessageConstant.noBeneficiaryToDownload);
        this.loadingSpinnerService.hideLoadingScreen("OnClickAvailableOffline");
      }
    });
  }

  private saveOrUpdateBeneficiaries() {
    let beneficiaryPromises = [];
    this.listBeneficiary.forEach(beneficiary => {
      beneficiaryPromises.push(this.InsertBeneficiary(beneficiary));
    });

    Promise.all(beneficiaryPromises).then((beneficiaryResult) => {
      console.log("All Beneficiary  Offline Data Upload done. ", beneficiaryResult);
      //this.saveOrUpdateBeneficiaryIndicators(beneficiaryResult);
      this.saveOrUpdateBeneficiaryDataCollectionStatus(beneficiaryResult);
    });
  }

  saveOrUpdateBeneficiaryDataCollectionStatus(beneficiaryResult: Beneficiary[]) {
    let beneficiaryDataCollectionPromises = [];

    beneficiaryResult.map(beneficiary => {
      beneficiaryDataCollectionPromises.push(this.InsertBeneficiaryDataCollectionStatus(beneficiary));
    });

    Promise.all(beneficiaryDataCollectionPromises).then((beneficiaryDataCollectionResult) => {
      console.log("All Beneficiary  Data collection status Download done. ", beneficiaryDataCollectionResult);
      this.saveOrUpdateBeneficiaryIndicators(beneficiaryResult);
    }, (reject) => {
      this.loadingSpinnerService.hideLoadingScreen("OnClickAvailableOffline");
    });
  }

  private saveOrUpdateBeneficiaryIndicators(beneficiaryResult: Beneficiary[]) {
    let indicatorsPromises = [];
    this.listBeneficiary[0].properties.map(indicator => {
      indicatorsPromises.push(this.InsertBeneficiaryIndicator(indicator));
    });

    Promise.all(indicatorsPromises).then((indicatorsResult) => {
      console.log("All Beneficiary Indicator Offline Data Upload done. ", indicatorsResult);

      this.saveBeneficiaryRecords(beneficiaryResult, indicatorsResult);
    });
  }

  private saveBeneficiaryRecords(beneficiaryResult: Beneficiary[], indicatorsResult: BeneficiaryIndicator[]) {
    let beneficiaryRecordPromises = [];

    beneficiaryResult.map(eachBeneficiary => {
      indicatorsResult.map(eachIndicator => {

        var filteredBeneficiary = this.listBeneficiary.find(x => x.entityId == eachBeneficiary.id);
        let filteredIndicator = filteredBeneficiary.properties.find(x => x.entityColumnId == eachIndicator.entityDynamicColumnId);

        beneficiaryRecordPromises.push(this.InsertBeneficiaryRecord(eachBeneficiary.uniqueId, eachIndicator.id, filteredIndicator));
      });
    });

    Promise.all(beneficiaryRecordPromises).then((recordIds) => {
      console.log("Beneficiary Records Saved of Id - ", recordIds);

      this.loadingSpinnerService.hideLoadingScreen("OnClickAvailableOffline");
      this.toastrService.success(MessageConstant.AvailableOffline);
    });

    // beneficiary.properties.forEach(indicator => {
    //   var uniqueId = beneficiaryResult.filter(x => x.id == beneficiary.entityId)[0].uniqueId;
    //  var beneficiaryIndicatorId = indicatorsResult.filter(x => x.entityDynamicColumnId == indicator.entityColumnId && x.instanceId == this.instanceId)[0].id;

    //   promises.push(this.InsertBeneficiaryRecord(uniqueId,
    //     beneficiary.collectionStatus, beneficiaryIndicatorId, indicator));
    // });

    // Promise.all(promises).then((recordIds) => {
    //   console.log("Beneficiary Records Saved of Id - ", recordIds);
    // });
  }

  InsertBeneficiaryDataCollectionStatus(beneficiary: Beneficiary) {
    return new Promise<any>((resolve, reject) => {
      var beneficiaryDataCollectionStatus = new BeneficiaryDataCollectionStatus();
      beneficiaryDataCollectionStatus.beneficiaryId = beneficiary.uniqueId;
      beneficiaryDataCollectionStatus.instanceId = this.instanceId;
      beneficiaryDataCollectionStatus.status = beneficiary.collectionStatus;

      resolve(this.dbService.SaveBeneficiaryDataCollectionStatus(beneficiaryDataCollectionStatus, true));
    });
  }

  InsertFacilityDataCollectionStatus(facility: Facility) {
    return new Promise<any>((resolve, reject) => {
      var facilityDataCollectionStatus = new FacilityDataCollectionStatus();
      facilityDataCollectionStatus.facilityId = facility.uniqueId;
      facilityDataCollectionStatus.instanceId = this.instanceId;
      facilityDataCollectionStatus.status = facility.collectionStatus;

      resolve(this.dbService.SaveFacilityDataCollectionStatus(facilityDataCollectionStatus, true));
    });
  }

  InsertListItem(listParam: ListDataTypeViewModel) {
    return new Promise<any>((resolve) => {

      let list = new List();
      list.id = listParam.id;
      list.name = listParam.name;
      this.dbService.SaveList(list).then((listId) => {

        for (var listItemParam of listParam.listItems) {

          var listItem = new ListItem();
          listItem.id = listItemParam.id;
          listItem.listId = listId;
          listItem.title = listItemParam.title;
          listItem.value = listItemParam.value;

          this.dbService.SaveListItem(listItem);
        }

        resolve(listId);

      });
    })
  }


  InsertFacilityRecord(uniqueId: number, columnId: number, indicator: EntityDynamicColumn, collectionStatus: CollectionStatus) {
    return new Promise<number>((resolve) => {
      var facilityRecord = new FacilityRecord();
      facilityRecord.facilityId = uniqueId;
      facilityRecord.instanceId = this.instanceId;
      facilityRecord.columnId = columnId;
      facilityRecord.value = indicator.values.join(',').toString();
      facilityRecord.status = collectionStatus;

      resolve(this.dbService.SaveFacilityRecord(facilityRecord, true));
    })
  }

  InsertFacilityIndicator(indicator: IndicatorViewModel) {
    return new Promise<FacilityIndicator>((resolve) => {

      var facilityIndicator = new FacilityIndicator();
      facilityIndicator.columnDataType = indicator.columnDataType;
      facilityIndicator.columnName = indicator.indicatorName;
      facilityIndicator.columnNameInBangla = indicator.indicatorNameInBangla ? indicator.indicatorNameInBangla : '';
      facilityIndicator.entityDynamicColumnId = indicator.entityDynamicColumnId;
      facilityIndicator.instanceId = this.instanceId;
      facilityIndicator.isMultiValued = indicator.isMultivalued;
      facilityIndicator.listId = indicator.listObject == null ? null : indicator.listObject.id;

      resolve(this.dbService.SaveFacilityIndicator(facilityIndicator));
    })
  }

  InsertBeneficiaryRecord(uniqueId: number, columnId: number, indicator: EntityDynamicColumn) {
    return new Promise<number>((resolve) => {
      var beneficiaryRecord = new BeneficiaryRecord();
      beneficiaryRecord.beneficiaryId = uniqueId;
      beneficiaryRecord.instanceId = this.instanceId;
      beneficiaryRecord.columnId = columnId;
      beneficiaryRecord.status = indicator.status;
      beneficiaryRecord.value = indicator.values.join(',').toString();

      resolve(this.dbService.SaveBeneficiaryRecord(beneficiaryRecord, true));
    })
  }

  InsertBeneficiaryIndicator(indicator: EntityDynamicColumn) {
    return new Promise<BeneficiaryIndicator>((resolve) => {

      var beneficiaryIndicator = new BeneficiaryIndicator();
      beneficiaryIndicator.entityDynamicColumnId = indicator.entityColumnId;
      beneficiaryIndicator.columnName = indicator.properties;
      beneficiaryIndicator.columnNameInBangla = indicator.columnNameInBangla ? indicator.columnNameInBangla : '';
      beneficiaryIndicator.columnDataType = indicator.dataType;
      beneficiaryIndicator.instanceId = this.instanceId;
      beneficiaryIndicator.isMultiValued = indicator.isMultiValued;
      beneficiaryIndicator.listId = indicator.columnListId;

      resolve(this.dbService.SaveBenefeciaryIndicator(beneficiaryIndicator));
    })
  }

  InsertBeneficiary(element: BeneficiaryViewModel) {
    return new Promise<Beneficiary>((resolve) => {
      var beneficiary = new Beneficiary();
      beneficiary.id = element.entityId;
      beneficiary.UnhcrId = element.unhcrId;
      beneficiary.beneficiaryName = element.beneficiaryName;
      beneficiary.fatherName = element.fatherName;
      beneficiary.motherName = element.motherName;
      beneficiary.FCNId = element.fcnId;
      beneficiary.dateOfBirth = element.dateOfBirth;
      beneficiary.sex = element.sex;
      beneficiary.disengaged = false;
      beneficiary.disabled = element.disabled;
      beneficiary.levelOfStudy = element.levelOfStudy;
      beneficiary.enrollmentDate = element.enrollmentDate;
      beneficiary.facilityId = element.facilityId;
      beneficiary.beneficiaryCampId = element.beneficiaryCampId;
      beneficiary.blockId = element.blockId;
      beneficiary.subBlockId = element.subBlockId;
      beneficiary.facilityName = element.facilityName;
      beneficiary.beneficiaryCampName = element.beneficiaryCampName;
      beneficiary.blockName = element.blockName;
      beneficiary.subBlockName = element.subBlockName;
      beneficiary.collectionStatus = element.collectionStatus;

      resolve(this.dbService.SaveBeneficiary(beneficiary));
    })
  }

  InsertFacility(facilityElement: BeneficiaryFacilityViewModel) {
    return new Promise<any>((resolve) => {
      let facility = new Facility();
      facility.id = facilityElement.id;
      facility.facilityName = facilityElement.facilityName;
      facility.facilityCode = facilityElement.facilityCode;
      facility.campName = facilityElement.campName;
      facility.blockName = facilityElement.blockName;
      facility.upazilaName = facilityElement.upazilaName;
      facility.unionName = facilityElement.unionName;
      facility.teacherName = facilityElement.teacherName;
      facility.implementationPartnerName = facilityElement.implemantationPartnerName;
      facility.programmingPartnerName = facilityElement.programmingPartnerName;
      facility.collectionStatus = facilityElement.collectionStatus;

      resolve(this.dbService.SaveFacility(facility));
    })
  }

  InsertScheduleInstance(scheduleInstance: InstanceViewModel) {
    return new Promise<any>((resolve) => {

      if (this.collectFor == EntityType.Beneficiary) {
        var beneficiaryScheduleInstance = new BeneficiaryScheduleInstance();

        beneficiaryScheduleInstance.id = scheduleInstance.id;
        beneficiaryScheduleInstance.dataCollectionDate = scheduleInstance.dataCollectionDate;
        beneficiaryScheduleInstance.endDate = scheduleInstance.endDate;
        beneficiaryScheduleInstance.instanceTitle = scheduleInstance.title;
        beneficiaryScheduleInstance.status = scheduleInstance.status;
        beneficiaryScheduleInstance.scheduleId = scheduleInstance.scheduleId;

        resolve(this.dbService.SaveScheduleInstance(beneficiaryScheduleInstance));
      }
      else if (this.collectFor == EntityType.Facility) {
        var facilityScheduleInstance = new FacilityScheduleInstance();
        facilityScheduleInstance.id = scheduleInstance.id;
        facilityScheduleInstance.dataCollectionDate = scheduleInstance.dataCollectionDate;
        facilityScheduleInstance.endDate = scheduleInstance.endDate;
        facilityScheduleInstance.instanceTitle = scheduleInstance.title;
        facilityScheduleInstance.scheduleId = scheduleInstance.scheduleId;
        facilityScheduleInstance.status = scheduleInstance.status;

        resolve(this.dbService.SaveFacilityScheduleInstance(facilityScheduleInstance));
      }

    });
  }

  loadBeneficiaryIndicator(beneficiaryId) {
    return new Promise<indicatorGetViewModel[]>((resolve) => {
      this.onlineFacilityService.getBeneficiaryIndicator(this.instanceId, beneficiaryId, this.allRecords, 1)
        .then((data) => {
          resolve(data.data[0].indicators);
        });
    })
  }

  loadFacilityIndicator(facilityId) {
    return new Promise<indicatorGetViewModel[]>((resolve) => {

      this.onlineFacilityService.getFacilityIndicator(this.instanceId, facilityId, this.allRecords, 1)
        .then((data) => {
          resolve(data.data[0].indicators);
        });
    });
  }

  loadBeneficiaryByFacility(facilityId) {
    return new Promise<BeneficiaryViewModel[]>((resolve, reject) => {
      var data = this.onlineBeneficiaryService
        .GetByFacilityId(this.instanceId, facilityId, this.allRecords, 1,"")
        .then((data) => {
          resolve(data.data);
        });
    });
  }

  async getPage(pageNo: number) {
    await this.loadFacility(pageNo, this.paginationConfig.itemsPerPage, this._facilitySearchText);
  }

  // async searchFacilityonKeyup(event:KeyboardEvent){
  //   let searchResult = (event.target as HTMLInputElement).value;
  //   this.loadFacility(this.paginationConfig.currentPage,this.allRecords,searchResult);
  // }

  loadFacility(pageNo, pageSize, searchText) {
    return new Promise<BeneficiaryFacilityViewModel[]>((resolve) => {
      if (this.collectFor == 1) {
        this.onlineBeneficiaryService.getAllFacility(pageSize, pageNo,this.instanceId, searchText).then((data) => {
          this.paginationConfig.currentPage = pageNo;
          this.paginationConfig.totalItems = data.total;
          this.listFacility = data.data;

          resolve(data.data);
        });
      }
      else {
        this.onlineBeneficiaryService
          .GetAllForDevice(this.instanceId, pageSize, pageNo, searchText)
          .then((data) => {
            this.paginationConfig.currentPage = pageNo;
            this.paginationConfig.totalItems = data.total;
            this.listFacility = data.data;
            resolve(data.data);
          });
      }
    });
  }

  loadCampWithBlockSubBlock() {
    return new Promise<CampBlockSubBlockViewModel[]>((resolve) => {
      this.commonService.getCampWithBlockSubBlock({
        pageNo: 1,
        pageSize: this.allRecords
      }).then((data) => {
        resolve(data.data);
      })
    });
  }

  loadScheduleInstance() {

    return new Promise<InstanceViewModel[]>((resolve, reject) => {
      var data = this.onlineBeneficiaryService
        .getRunningInstance(this.collectFor, this.allRecords, 1)
        .then((data) => {
          resolve(data.data);
        });
    });
  }


  loadIndicatorByInstance() {
    return new Promise<IndicatorViewModel[]>((resolve) => {
      this.onlineBeneficiaryService.GetIndicatorByInstance(this.instanceId, this.allRecords, 1)
        .then((data) => {
          resolve(data.data);
        })
    })
  }

  loadListTypeItem() {
    return new Promise<ListDataTypeViewModel[]>((resolve) => {
      this.onlineBeneficiaryService.GetAllListTypeData(1, this.allRecords).then((data) => {
        resolve(data.data);
      })
    })
  }

}


