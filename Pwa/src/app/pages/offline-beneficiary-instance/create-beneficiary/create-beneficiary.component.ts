import { Component, OnInit } from '@angular/core';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { ActivatedRoute, Router } from '@angular/router';
import { Facility } from 'src/app/models/idbmodels/indexedDBModels';
import { FacilityDb } from 'src/app/localdb/FacilityDb';

@Component({
  selector: 'app-create-beneficiary',
  templateUrl: './create-beneficiary.component.html',
  styleUrls: ['./create-beneficiary.component.scss']
})
export class CreateBeneficiaryComponent implements OnInit {

  status:OnlineOfflineStatus;
  
  facilityId:number;
  facilityCode:String;
  facilityName:String;
  programmingPartnerName:String;
  implementationPartnerName:String;

  constructor(private route:ActivatedRoute,private facilityDbService:FacilityDb) { }

  ngOnInit() {
    this.status = OnlineOfflineStatus.Offline;
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));


    this.loadFacilityById().then(facilityResult => {
      this.facilityName = facilityResult.facilityName;
      this.implementationPartnerName = facilityResult.implementationPartnerName;
      this.programmingPartnerName = facilityResult.programmingPartnerName;
      this.facilityCode = facilityResult.facilityCode;
    });
  }

  loadFacilityById() {
    return new Promise<Facility>((resolve, reject) => {
      this.facilityDbService.getFacilityById(this.facilityId).subscribe((data) => {
        resolve(data[0]);
      })
    })
  }

}
