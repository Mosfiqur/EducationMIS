import { Component, OnInit, ViewChild, Inject, ViewEncapsulation } from '@angular/core';
import { UserDB } from 'src/app/localdb/UserDB';
import { User, Facility, FacilityDataCollectionStatus, FacilityRecord, FacilityIndicator, Beneficiary, BeneficiaryDataCollectionStatus, BeneficiaryIndicator, BeneficiaryRecord } from 'src/app/models/idbmodels/indexedDBModels';
import { AuthService } from 'src/app/services/auth.service';
import { PaginationInstance } from 'ngx-pagination';
import { ModalService } from 'src/app/services/modal.service';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/services/notification.service';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { IndexedDbService } from 'src/app/localdb/IndexedDbService';
import { NotificationTypeEnum } from 'src/app/_enums/notificationTypeEnum';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { resolve } from 'url';
import { EntityDynamicColumn } from 'src/app/models/indicator/EntityDynamicColumn';
import { IndicatorViewModel } from 'src/app/models/indicator/IndicatorViewModel';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { JQ_TOKEN } from 'src/app/services/jQuery.service';

@Component({
  selector: 'app-unicef',
  templateUrl: './unicef.component.html',
  styleUrls: ['./unicef.component.scss'],
})

export class UnicefComponent implements OnInit {
  allRecords:number = 2147483647;
  user: User;
  @ViewChild('sidebarToggle') sidebarToggle: HTMLButtonElement;
  public notActedTotal: number = 0;
  public listNotification: Notification[] = [];
  public show: boolean = true;
  public buttonName: any = true;
  instanceId:number;

  paginationConfig: PaginationInstance = {
    id: 'notification_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  };

  public sidebarCollapsed: boolean = false;
  constructor(
    @Inject(JQ_TOKEN) private $: any,
    private userDb: UserDB,
    private authService: AuthService,
    private modalService: ModalService,
    private router: Router,
    private notification: NotificationService,
    private facilityDb:FacilityDb,
    private dbService:IndexedDbService,
    private onlineFacilityService:OnlineFacilityService,
    private beneficiaryDbService:BeneficiaryDb,
    private onlineBeneficiaryService:OnlineBeneficiaryService

  ) {
    this.user = new User();
  }

  ngOnInit() {
    // console.log(this.user);
    this.userDb.getUser().subscribe((res) => {
      if (res && res.length > 0) {
        this.user = res[0];
      }
    });

    this.getNotification(1);
    setInterval(a => {
      this.getNotification(this.paginationConfig.currentPage);
    }, 30000);

    this.$('.notification-bottom').on('click', function (event) {
      setTimeout(() => {
        document.getElementById('notificationContainer').classList.add("show")
        document.getElementById('notificationBody').classList.add("show")
        document.getElementById('notificationContainer').setAttribute("aria-expanded", "true");
      });
  });

  }

  getPage(pageNo: number) {
    if (!pageNo)
      return;
    this.getNotification(pageNo);
  }

  getNotification(pageNo) {
    let baseQueryModel = {
      pageNo: pageNo,
      pageSize: this.paginationConfig.itemsPerPage
    }

    this.notification.getAll(baseQueryModel).subscribe(a => {
      let getData: any = a;
      let data = JSON.parse(getData._body);

      this.notActedTotal = data.notActedTotal;
      this.listNotification = data.data;
      this.paginationConfig.currentPage = pageNo;
      this.paginationConfig.totalItems = data.notActedTotal;
    });
  }

  async readAndRedirectToUri(id, uri, data, notificationTypeId) {
    this.notification.readNotification(id).then(async a => {
      
      if(notificationTypeId == NotificationTypeEnum.Beneficiary_Instance_Start){
        uri = 'unicefpwa/online-beneficiary-instance';
        var url = "/" + uri;
        this.router.navigate([url]);
      }
      else if(notificationTypeId == NotificationTypeEnum.Facility_Instance_Start){
        uri = 'unicefpwa/online-facility-instance/home';
        var url = "/" + uri;
        this.router.navigate([url]);
      }
      else if(notificationTypeId == NotificationTypeEnum.Recollect_Facility){
        uri = 'unicefpwa/offline-facility-instance/indicator';
        var jsonData = data;
        let facilityId = JSON.parse(jsonData).FacilityId;
        this.instanceId = JSON.parse(jsonData).InstanceId;

        await this.getFacilitybyFacilityId(facilityId).then(async (facility) => {
          uri = uri+ "/"+this.instanceId+"/"+facility.uniqueId;

          await this.getFacilityIndicator(this.instanceId, facilityId).then(async (indicatorResults) => {
            let indicatorPromises = [];
            
            for(let eachIndicator of indicatorResults){
              indicatorPromises.push(await this.InsertFacilityIndicator(eachIndicator));
            }
            Promise.all(indicatorPromises).then(async (facilityIndicatorResults) => {
              let facilityRecordPromises = [];
              
              for(let indicator of indicatorResults){
                let filteredFacilityIndicator = facilityIndicatorResults.find(x => x.entityDynamicColumnId === indicator.entityDynamicColumnId && x.instanceId === this.instanceId);
                let columnId = filteredFacilityIndicator.id;
                facilityRecordPromises.push(await this.InsertFacilityRecord(facility.uniqueId,columnId,
                  indicator,facility.collectionStatus));
              }

              Promise.all(facilityRecordPromises).then((facilityRecordResults) => {
                this.InsertFacilityDataCollectionStatus(facility,this.instanceId).then(()=>{
                  var url = "/" + uri;
                  this.router.navigate([url]);
                });
              });
            });
          });
        });
      }
      else if(notificationTypeId == NotificationTypeEnum.Recollect_Beneficiary){
        uri = 'unicefpwa/offline-beneficiary-instance/indicator';
        var jsonData = data;
        let beneficiaryId = JSON.parse(jsonData).BeneficiaryId;
        this.instanceId = JSON.parse(jsonData).InstanceId;
        await this.getBeneficiaryById(beneficiaryId).then(async (beneficiary) => {
          uri = uri+ "/"+this.instanceId+"/"+beneficiary.uniqueId;

          await this.getBeneficiaryIndicator(this.instanceId,beneficiaryId).then(async (indicatorResults) => {
            let indicatorPromises = [];
            debugger;
            for(let eachIndicator of indicatorResults){
              indicatorPromises.push(await this.InsertBeneficiaryIndicator(eachIndicator));
            }
            Promise.all(indicatorPromises).then(async (beneficiaryIndicatorResults) => {
              let beneficiaryRecordPromises = [];

              console.log(beneficiaryIndicatorResults);
              for(let indicator of indicatorResults){
                let filteredBeneficiaryIndicator = beneficiaryIndicatorResults.find(x => x.entityDynamicColumnId === indicator.entityDynamicColumnId && x.instanceId === this.instanceId);
                let columnId = filteredBeneficiaryIndicator.id;

                beneficiaryRecordPromises.push(await this.InsertBeneficiaryRecord(beneficiary.uniqueId,columnId,
                  indicator));
              }

              Promise.all(beneficiaryRecordPromises).then((beneficiaryRecordResults) => {
                this.InsertBeneficiaryDataCollectionStatus(beneficiary).then(()=>{
                  var url = "/" + uri;
                  this.router.navigate([url]);
                });
              });
            });
          });
        });
      }
    });
  }

  private async getBeneficiaryById(beneficiaryId: number) {
    return new Promise<any>((resolve,reject) => {
      this.beneficiaryDbService.getBeneficiaryById(beneficiaryId).subscribe((beneficiaryResults) => {
        resolve(beneficiaryResults[0]);
      });
    });
  }

  private getBeneficiaryIndicator(instanceId: any, beneficiaryId: any) {
    return new Promise<any>((resolve,reject) => {
      this.onlineFacilityService.getBeneficiaryIndicator(instanceId, beneficiaryId, this.allRecords, 1).then((data) => {
        resolve(data.data[0].indicators);
      });
    });
  }

  private getFacilityIndicator(instanceId: any, facilityId: any) {
    return new Promise<any>((resolve,reject) => {
      this.onlineFacilityService.getFacilityIndicator(instanceId, facilityId, this.allRecords, 1).then((data) => {
        resolve(data.data[0].indicators);
      });
    });
  }

  private async getFacilitybyFacilityId(facilityId: any) {
    return new Promise<any>((resolve,reject) => {
      this.facilityDb.getFacilityById(facilityId).subscribe((facilityResult) => {
        resolve(facilityResult[0]);
      });
    });
  }

  clearAll() {
    this.notification.clearNotification().then(a => {
      this.getNotification(1);
    });
  }

  toggleSidebar() {
    let sidebar = document.getElementById('sidebar');
    let content = document.getElementById('content');
    if (!this.sidebarCollapsed) {
      sidebar.style.width = "0px";
      content.style.width = "100%";
    } else {
      sidebar.style.width = "260px";
      content.style.width = "calc(100% - 260px)";
    }
    this.sidebarCollapsed = !this.sidebarCollapsed;
  }

  logout() {
    this.authService.logout();
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

  InsertBeneficiaryIndicator(indicator: any) {
    return new Promise<BeneficiaryIndicator>((resolve) => {

      var beneficiaryIndicator = new BeneficiaryIndicator();
      beneficiaryIndicator.entityDynamicColumnId = indicator.entityDynamicColumnId;
      beneficiaryIndicator.columnName = indicator.columnName;
      beneficiaryIndicator.columnNameInBangla = indicator.columnNameInBangla;
      beneficiaryIndicator.columnDataType = indicator.columnDataType;
      beneficiaryIndicator.instanceId = this.instanceId;
      beneficiaryIndicator.isMultiValued = indicator.isMultiValued;
      beneficiaryIndicator.listId = indicator.listObjectId;

      resolve(this.dbService.SaveBenefeciaryIndicator(beneficiaryIndicator));
    })
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

  

  InsertFacilityIndicator(indicator: any) {
    return new Promise<FacilityIndicator>((resolve) => {

      var facilityIndicator = new FacilityIndicator();
      facilityIndicator.columnDataType = indicator.columnDataType;
      facilityIndicator.columnName = indicator.columnName;
      facilityIndicator.columnNameInBangla = indicator.columnNameInBangla;
      facilityIndicator.entityDynamicColumnId = indicator.entityDynamicColumnId;
      facilityIndicator.instanceId = this.instanceId;
      facilityIndicator.isMultiValued = indicator.isMultivalued == null ? null : indicator.isMultiValued;
      facilityIndicator.listId = indicator.listObject == null ? null : indicator.listObject.id;
      
      resolve(this.dbService.SaveFacilityIndicator(facilityIndicator));
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

  InsertFacilityDataCollectionStatus(facility: Facility,instanceId) {
    return new Promise<any>((resolve, reject) => {
      var facilityDataCollectionStatus = new FacilityDataCollectionStatus();
      facilityDataCollectionStatus.facilityId = facility.uniqueId;
      facilityDataCollectionStatus.instanceId = instanceId;
      facilityDataCollectionStatus.status = facility.collectionStatus;

      resolve(this.dbService.SaveFacilityDataCollectionStatus(facilityDataCollectionStatus, true));
    });
  }

}

