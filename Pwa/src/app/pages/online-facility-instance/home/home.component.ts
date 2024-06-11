import { Component, OnInit } from '@angular/core';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { EntityType } from "src/app/_enums/entityType";
import { InstanceViewModel } from "src/app/models/instance/instanceViewModel";
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  // private allRecords = 2147483647;
  // lstInstance: InstanceViewModel[];
  // collectFor:number=EntityType.Facility;

  status: OnlineOfflineStatus;

  constructor(private onlineBeneficiaryService: OnlineBeneficiaryService) { }

  ngOnInit() {
    this.status=OnlineOfflineStatus.Online
    //this.loadInstance();
  }
  // loadInstance() {

  //   return new Promise((resolve, reject) => {

  //     var data = this.onlineBeneficiaryService
  //       .getRunningInstance(EntityType.Facility, this.allRecords, 1)
  //       .then((data) => {
  //         this.lstInstance = data.data;
  //       });
  //   });
  // }


}
