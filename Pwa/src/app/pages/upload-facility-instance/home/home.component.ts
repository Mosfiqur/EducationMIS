import { Component, OnInit } from '@angular/core';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { EntityType } from 'src/app/_enums/entityType';
import { Router } from '@angular/router';
import { FacilityScheduleInstanceDB } from 'src/app/localdb/FacilityScheduleInstanceDB';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  listInstance: any;
  collectFor: number = EntityType.Facility;

  constructor(private router:Router,private scheduleInstanceDbService:FacilityScheduleInstanceDB){

  }
  
  ngOnInit() {
    this.loadInstance().then(data => {
      this.listInstance = data;
    });
  }

  navigateTo(instanceId) {
    this.router.navigate(["/unicefpwa/upload-facility-instance/all-facility", this.collectFor, instanceId]);
  }

  loadInstance() {

    return new Promise<any>((resolve, reject) => {

      this.scheduleInstanceDbService.getAllFacilityScheduleInstance().subscribe(data => {
        var result = data.map(res => ({
          id: res.id, scheduleId: res.scheduleId, title: res.instanceTitle,
          instanceTitle: res.instanceTitle, dataCollectionDate: res.dataCollectionDate, status: res.status
        }));
        resolve(result);
      });
    });
  }

}
