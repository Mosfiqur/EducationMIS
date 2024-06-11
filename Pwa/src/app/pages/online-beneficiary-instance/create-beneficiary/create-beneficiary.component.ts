import { Component, OnInit } from '@angular/core';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { FacilityViewModel } from 'src/app/models/viewModel/facilityViewModel';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-create-beneficiary',
  templateUrl: './create-beneficiary.component.html',
  styleUrls: ['./create-beneficiary.component.scss']
})
export class CreateBeneficiaryComponent implements OnInit {

  status:OnlineOfflineStatus;

  facilityCode:String;
  facilityId:number;
  facilityName:String;
  programmingPartnerName:String;
  implementationPartnerName:String;

  constructor(private onlineFacilityService:OnlineFacilityService,private route:ActivatedRoute) { }

  ngOnInit() {
    this.status = OnlineOfflineStatus.Online;
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));


    this.loadFacilityByIdOnline().then(facilityResult => {
      this.facilityName = facilityResult.name;
      this.programmingPartnerName = facilityResult.programPartnerName;
      this.implementationPartnerName = facilityResult.implementationPartnerName;
      this.facilityCode = facilityResult.facilityCode;
    });
  }

  loadFacilityByIdOnline() {
    return new Promise<FacilityViewModel>((resolve, reject) => {

      var data = this.onlineFacilityService
        .getFacilityById(this.facilityId)
        .then((data) => {
          resolve(data);
        });
    });
  }

}
