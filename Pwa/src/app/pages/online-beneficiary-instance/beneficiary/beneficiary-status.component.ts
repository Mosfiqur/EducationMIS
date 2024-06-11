import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { BeneficiaryViewModel } from 'src/app/models/viewModel/beneficiaryViewModel';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-beneficiary-status',
  templateUrl: './beneficiary-status.component.html',
  styleUrls: ['./beneficiary-status.component.scss']
})
export class BeneficiaryStatusComponent implements OnInit {
  facilityId:number;
  instanceId:number;
  private allRecords = 2147483647;
  listBeneficiary:BeneficiaryViewModel[];
  status:OnlineOfflineStatus;
  constructor(private route:ActivatedRoute,private onlineBeneficiaryService:OnlineBeneficiaryService,private router:Router) { }

  ngOnInit() {
    this.status = OnlineOfflineStatus.Online;
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    
    // this.loadBeneficiary().then(result => {
    //   this.listBeneficiary = result;
    // })
  }

  // loadBeneficiary(){
  //   return new Promise<any>((resolve, reject) => {
      
  //     var data = this.onlineBeneficiaryService
  //       .GetByFacilityId(this.instanceId,this.facilityId,this.allRecords,1)
  //       .then((data) => {
  //         resolve(data.data);
  //   });
  // });
  // }

  navigateToBeneficiaryIndicator(beneficiary:BeneficiaryViewModel){
    this.router.navigate(['/unicefpwa/online-beneficiary-instance/indicator',this.instanceId,beneficiary.entityId]);
  }

  loadBeneficiaryIndicator(){

  }

  CollectionStatusText(id) {
    return CollectionStatus[id];
  }

}
