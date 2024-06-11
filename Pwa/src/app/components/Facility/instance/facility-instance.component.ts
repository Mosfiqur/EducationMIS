import { Component, OnInit, Input } from '@angular/core';
import { FacilityScheduleInstanceDB } from 'src/app/localdb/FacilityScheduleInstanceDB';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { EntityType } from 'src/app/_enums/entityType';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { InstanceStatus } from 'src/app/_enums/instanceStatus';
import { Router } from '@angular/router';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { FacilityScheduleInstance } from 'src/app/models/idbmodels/indexedDBModels';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'facility-instance',
  templateUrl: './facility-instance.component.html',
  styleUrls: ['./facility-instance.component.scss']
})
export class FacilityInstanceComponent implements OnInit {

  @Input() onlineOfflineStatus: OnlineOfflineStatus;
  @Input() redirectTo: string;

  private allRecords = 2147483647;
  listInstance: any;
  collectFor: number = EntityType.Facility;

  constructor(private scheduleInstanceDBService: FacilityScheduleInstanceDB, private toastrService:ToastrService,
    private router: Router, private onlineBeneficiaryService: OnlineBeneficiaryService,
  ) { }

  ngOnInit() {
    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {

      this.loadInstance().then(data => {
        this.listInstance = data;
      });
    }
    else {

      this.loadInstanceOnline().then(data => {
        this.listInstance = data;
      });
    }

  }

  navigateTo(instanceId) {

    this.router.navigate([this.redirectTo, this.collectFor, instanceId]);
  }

  loadInstance() {

    return new Promise<any>((resolve, reject) => {

      this.scheduleInstanceDBService.getAllFacilityScheduleInstance().subscribe(data => {
        var result = data.map(res => ({
          id: res.id, scheduleId: res.scheduleId, title: res.instanceTitle,
          instanceTitle: res.instanceTitle, dataCollectionDate: res.dataCollectionDate, status: res.status,
          endDate: res.endDate
        }));
        resolve(result.reverse());
      });
    });
  }

  loadInstanceOnline() {

    return new Promise<InstanceViewModel[]>((resolve, reject) => {
      var data = this.onlineBeneficiaryService
        .getRunningInstance(EntityType.Facility, this.allRecords, 1)
        .then((data) => {
          var result = data.data.map(res => (
            { id: res.id, scheduleId: res.scheduleId, status: res.status, title: res.title, dataCollectionDate: res.dataCollectionDate, endDate: res.endDate }))
          resolve(result);
          // this.listInstance = data.data;
        });
    });
    // let data:InstanceViewModel[]=[{id:1,scheduleId:1,dataCollectionDate:'2020/01/01',status:InstanceStatus.Pending,title:'This is test Instance'}]
    //     resolve(data);
    // })
  }

}
