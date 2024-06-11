import { Component, OnInit } from '@angular/core';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { EntityType } from "src/app/_enums/entityType";
import { InstanceViewModel } from "src/app/models/instance/instanceViewModel";
import { AuthService } from 'src/app/services/auth.service';


@Component({
  selector: 'app-online-beneficiary',
  templateUrl: './online-beneficiary.component.html',
  styleUrls: ['./online-beneficiary.component.scss']
})
export class OnlineBeneficiaryComponent implements OnInit {
  private allRecords = 2147483647;
  lstInstance: InstanceViewModel[];
  selectedInstance: string = "";
  selectedInstanceId: number = 0;
  collectFor:number = EntityType.Beneficiary;

  constructor(private onlineBeneficiaryService: OnlineBeneficiaryService,private authService:AuthService) { }

  ngOnInit() {
    this.loadInstance();
  }
   loadInstance() {
    
    return new Promise((resolve, reject) => {
      
      var data = this.onlineBeneficiaryService
        .getRunningInstance(EntityType.Beneficiary, this.allRecords, 1)
        .then((data) => {
          this.lstInstance = data.data;

          var maxid = 0;
          this.lstInstance.map(function (obj) {
            if (obj.id > maxid) maxid = obj.id;
          });
          this.lstInstance
            .filter((a) => a.id == maxid)
            .map((a) => {
              this.selectedInstance = a.title;
              this.selectedInstanceId = a.id;
            });
          resolve(data);
        });
    });
  }
}
