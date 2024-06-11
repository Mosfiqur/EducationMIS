declare var $: any;
import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { HomeService } from 'src/app/services/home.service';
import { BeneficiaryRecord, User } from 'src/app/models/idbmodels/indexedDBModels';
import { UserDB } from 'src/app/localdb/UserDB';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';

@Component({
  selector: 'ngx-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  testResult: String = 'Hello World!!';
  user: User;

  constructor(private router: Router, private homeService: HomeService, private userDb: UserDB, private beneficiaryRecordsDb:BeneficiaryRecordsDB) {
    this.user = new User();
  }

  ngOnInit() { 
    this.userDb.getUser().subscribe(res => {
      if (res && res.length > 0) {
        this.user = res[0];
      }
    })
  }

  getRandomString(lengthOfCode: number, possible: string) {
    let text = "";
    for (let i = 0; i < lengthOfCode; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  } 

  getRandomInt(max) {
    return Math.floor(Math.random() * Math.floor(max));
  }

  possible = 'Aliquam sed dapibus lacus. Nullam ac ante ac arcu commodo gravida. Nam bibendum augue ac nulla condimentum, eu rutrum magna luctus. Nam quis felis pellentesque nisi tincidunt interdum ut vitae turpis. Nulla aliquet hendrerit tortor nec efficitur. Donec accumsan ultrices mi quis pulvinar. Fusce hendrerit laoreet sapien in convallis. Nulla consequat nunc vitae sapien luctus vehicula. Praesent ex nisi, tempor eu sem id, condimentum facilisis nisl. Praesent eleifend sollicitudin elit. Fusce ut ipsum at dolor condimentum aliquet sed ut nulla. Sed non suscipit purus, at sodales ligula. Suspendisse potenti. Aenean ultrices, dolor ultricies aliquam pellentesque, diam augue tristique magna, ac efficitur diam neque eget sem. Pellentesque quis diam bibendum, finibus nibh ac, tincidunt dolor. Praesent sit amet augue ut mauris mollis sodales eget sed magna.';
  beneficiaryIds = [1, 2];
  instanceIds = [101, 102];
  columnIds = [1001, 1002, 1003];

  onClickTest1() { 
    for(let beneficiaryId of this.beneficiaryIds){
      for(let instanceId of this.instanceIds){
        for(let columnId of this.columnIds){
          let record = new BeneficiaryRecord();
          record.beneficiaryId = beneficiaryId; 
          record.instanceId = instanceId; 
          record.columnId = columnId; 
          record.status = 1; 
          record.value = this.getRandomString(128, this.possible); 
          this.beneficiaryRecordsDb.saveBeneficiaryRecord(record) 
          .subscribe((key)=>{
            console.log('Record id: ', key);
          })
        }
      }
    }
  } 

  onClickTest2(){ 
    for(let beneficiaryId of this.beneficiaryIds){
      for(let instanceId of this.instanceIds){
        this.beneficiaryRecordsDb.getBeneficiaryInstanceRecords(beneficiaryId, instanceId,1) 
        .subscribe((records)=>{
          console.log('Records for beneficiary ' + beneficiaryId, records);
        });
      }
    }
  }
}
