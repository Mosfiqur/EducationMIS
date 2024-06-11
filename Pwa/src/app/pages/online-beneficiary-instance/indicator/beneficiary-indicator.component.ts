import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BeneficiaryViewModel } from 'src/app/models/viewModel/beneficiaryViewModel';
import { resolve } from 'url';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { BeneficiaryWiseIndicatorViewModel } from 'src/app/models/viewModel/facilityWiseIndicatorViewModel';
import { indicatorGetViewModel } from 'src/app/models/viewModel/indicatorGetViewModel';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-beneficiary-indicator',
  templateUrl: './beneficiary-indicator.component.html',
  styleUrls: ['./beneficiary-indicator.component.scss']
})
export class BeneficiaryIndicatorComponent implements OnInit {
  status:OnlineOfflineStatus;
  instanceId: number;
  beneficiaryId: number;
  beneficiary: BeneficiaryViewModel;
  private allRecords = 2147483647;
  public indicatorList: BeneficiaryWiseIndicatorViewModel[];
  public indicators: indicatorGetViewModel[];



  constructor(private route: ActivatedRoute, private onlineFacilityService: OnlineFacilityService,private onlineBeneficiaryService:OnlineBeneficiaryService,
    ) { }

  ngOnInit() {
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.beneficiaryId = parseInt(this.route.snapshot.paramMap.get('beneficiaryId'));
    this.status = OnlineOfflineStatus.Online;

    // this.loadBeneficiary().then((beneficiary) => {
    //   console.log(beneficiary);
    //   this.beneficiary = beneficiary;

    //   this.loadBenenficiaryIndicators().then((indicatorsResult)=> {
    //     this.indicatorList = indicatorsResult;
    //   });
    // });
  }

  loadBeneficiary() {
    return new Promise<any>((resolve, reject) => {
      this.onlineBeneficiaryService.GetBeneficiaryById(this.beneficiaryId,this.instanceId).then((data) => {
        resolve(data);
      });
    });
  }

  loadBenenficiaryIndicators() {
    return new Promise<any>((resolve, reject) => {
      var data = this.onlineFacilityService.getBeneficiaryIndicator(this.instanceId, this.beneficiaryId, this.allRecords, 1)
        .then((data) => {
          resolve(data.data[0].indicators);
          // this.indicatorList = data.data;
          // this.indicatorList.map(x => this.indicators = x.indicators);
          // console.log(this.indicatorList);
        });
    });
  }

}
