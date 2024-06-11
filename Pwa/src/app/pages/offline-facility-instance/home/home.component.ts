import { Component, OnInit } from '@angular/core';
import { FacilityScheduleInstanceDB } from 'src/app/localdb/FacilityScheduleInstanceDB';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { EntityType } from 'src/app/_enums/entityType';
import {  OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  // listInstance: InstanceViewModel[];
  // collectFor:number=EntityType.Facility;
   status:OnlineOfflineStatus;


  constructor(private scheduleInstanceDBService:FacilityScheduleInstanceDB) { }

  ngOnInit() {
    this.status=OnlineOfflineStatus.Offline;
    // this.loadInstance().then(data => {
    //   this.listInstance = data;
    // });
  }

  // loadInstance() {
    
  //   return new Promise<InstanceViewModel[]>((resolve, reject) => {
      
  //     this.scheduleInstanceDBService.getAllFacilityScheduleInstance().subscribe(data => {
  //       resolve(data);
  //     })

  //   })
  // }

}
