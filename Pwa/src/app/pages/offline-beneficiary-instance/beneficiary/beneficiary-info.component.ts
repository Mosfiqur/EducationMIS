import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { Beneficiary, Facility } from 'src/app/models/idbmodels/indexedDBModels';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { FacilityDb } from 'src/app/localdb/FacilityDb';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-beneficiary-info',
  templateUrl: './beneficiary-info.component.html',
  styleUrls: ['./beneficiary-info.component.scss']
})
export class BeneficiaryInfoComponent implements OnInit {

  instanceId:number;
  facilityId:number;
  listBeneficiary:Beneficiary[];
  facility:Facility;
  status:OnlineOfflineStatus;

  constructor(private route:ActivatedRoute,private beneficiaryDbService:BeneficiaryDb,private facilityDbService:FacilityDb,
    private router:Router) { }

  ngOnInit() {
    this.status = OnlineOfflineStatus.Offline;
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));

    // this.loadFacilityById().then(facilityResult => {
    //   this.facility = facilityResult;

    //   this.loadBeneficiaryByFacility().then(results => {
    //     this.listBeneficiary = results;
    //   });
    // })
  }

  loadBeneficiaryByFacility(){
    return new Promise<Beneficiary[]>((resolve,reject) => {
      this.beneficiaryDbService.getAllBeneficiary().subscribe((results)=> {
        var beneficiariesByFacilityId = results.filter(x => x.facilityId == this.facilityId);
        
        resolve(beneficiariesByFacilityId);
      })
    })
  }

  loadFacilityById(){
    return new Promise<Facility>((resolve,reject) => {
      this.facilityDbService.getFacilityByUniqueId(this.facilityId).subscribe((data) => {
        resolve(data);
      })
    })
  }

  CollectionStatusText(id) {
    return CollectionStatus[id];
  }

  navigateToBeneficiaryIndicator(beneficiary:Beneficiary){
    this.router.navigate(['/unicefpwa/offline-beneficiary-instance/indicator',this.instanceId,beneficiary.uniqueId]);
  }

}
