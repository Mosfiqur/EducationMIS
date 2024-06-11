import { Component, OnInit } from '@angular/core';
import { FacilityScheduleInstanceDB } from 'src/app/localdb/FacilityScheduleInstanceDB';
import { EntityType } from 'src/app/_enums/entityType';
import { Router } from '@angular/router';
import { BeneficiaryScheduleInstanceDb } from 'src/app/localdb/BeneficiaryScheduleInstanceDb';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  listInstance: any;
  collectFor: number = EntityType.Beneficiary;

  constructor(private scheduleInstanceDBService: BeneficiaryScheduleInstanceDb,private router:Router) { }

  ngOnInit() {
    this.loadInstance().then(data => {
      this.listInstance = data;
    });
  }

  navigateTo(instanceId) {
    this.router.navigate(["/unicefpwa/upload-beneficiary-instance/all-facility", this.collectFor, instanceId]);
  }

  loadInstance() {

    return new Promise<any>((resolve, reject) => {

      this.scheduleInstanceDBService.getAllScheduleInstances().subscribe(data => {
        var result = data.map(res => ({
          id: res.id, scheduleId: res.scheduleId, title: res.instanceTitle,
          instanceTitle: res.instanceTitle, dataCollectionDate: res.dataCollectionDate, status: res.status
        }));
        resolve(result);
      });
    });
  }

}
