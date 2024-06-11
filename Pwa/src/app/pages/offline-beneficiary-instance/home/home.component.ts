import { Component, OnInit } from '@angular/core';
import { EntityType } from 'src/app/_enums/entityType';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { BeneficiaryScheduleInstance } from 'src/app/models/idbmodels/indexedDBModels';
import { BeneficiaryScheduleInstanceDb } from 'src/app/localdb/BeneficiaryScheduleInstanceDb';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  listInstance: BeneficiaryScheduleInstance[];
  collectFor:number=EntityType.Beneficiary;


  constructor(private scheduleInstanceDBService:BeneficiaryScheduleInstanceDb) { }

  ngOnInit() {
    this.loadInstance().then(data => {
      this.listInstance = data;
    });
  }

  loadInstance() {
    return new Promise<BeneficiaryScheduleInstance[]>((resolve, reject) => {
      
      this.scheduleInstanceDBService.getAllScheduleInstances().subscribe(data => {
        resolve(data.reverse());
      })

    })
  }
}
