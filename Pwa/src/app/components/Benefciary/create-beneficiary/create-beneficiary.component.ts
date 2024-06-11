import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from 'src/app/services/common.service';
import { CampDb } from 'src/app/localdb/CampDb';
import { BlockDb } from 'src/app/localdb/BlockDb';
import { SubBlockDb } from 'src/app/localdb/SubBlockDb';
import { Camp, Block, SubBlock, User, Beneficiary, BeneficiaryIndicator, BeneficiaryRecord, BeneficiaryDataCollectionStatus } from 'src/app/models/idbmodels/indexedDBModels';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { Subscription } from 'rxjs';
import { ISelectListItem } from 'src/app/helpers/select-list.model';
import { Convert } from 'src/app/utility/Convert';
import { Gender } from 'src/app/_enums/gender';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { UserDB } from 'src/app/localdb/UserDB';
import { Toast, ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { OnlineBeneficiaryService } from 'src/app/services/online-beneficiary.service';
import { BeneficiaryDb } from 'src/app/localdb/BeneficiaryDb';
import { BeneficiaryRecordsDB } from 'src/app/localdb/BeneficiaryRecordsDB';
import { BeneficiaryIndicatorDb } from 'src/app/localdb/BeneficiaryIndicatorDb';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import {Location} from '@angular/common';
import { BeneficiaryDataCollectionStatusDb } from 'src/app/localdb/BeneficiaryDataCollectionStatusDb';
import { EntityDynamicColumn } from 'src/app/models/indicator/EntityDynamicColumn';


@Component({
  selector: 'create-beneficiary',
  templateUrl: './create-beneficiary.component.html',
  styleUrls: ['./create-beneficiary.component.scss']
})
export class CreateBenComponent implements OnInit {

  @ViewChild('blockModalClose') blockSelectModalClose;
  @ViewChild('subBlockModalClose') subBlockSelectModalClose;
  @ViewChild('campModalClose') campSelectModalClose;
  
  @Input() onlineOfflineStatus: OnlineOfflineStatus;
  @Input() facilityCode: String;
  @Input() facilityName: String;
  @Input() programmingPartnerName: String;
  @Input() implementationPartnerName: String;
  listCamps: Camp[] = [];
  listBlocks: Block[] = [];
  listSubBlocks: SubBlock[] = [];
  public beneficiaryForm: FormGroup;
  public validationErrors = {};
  private subs: Map<string, Subscription> = new Map();
  public genderList: ISelectListItem[] = [];
  public levelOfStudyList: ISelectListItem[] = [];
  user = new User();

  selectedCampName: string; selectedCampId: number;
  selectedBlockName: string; selectedBlockId: number;
  selectedSubBlockName: string; selectedSubBlockId: number;

  onChangeSelectedCampName: string; onChangeSelectedCampId: number;
  onChangeSelectedBlockName: string; onChangeSelectedBlockId: number;
  onChangeSelectedSubBlockName: string; onChangeSelectedSubBlockId: number;

  public changeCampId:boolean = false;
  public changeBlockId:boolean = false;
  public changeSubBlockId:boolean = false;

  facilityId: number;
  instanceId: number;
  beneficiaryCreateEntityDynamicIds:number[] = [122,123,124,125,126,127,128,129,130,131,132,133,134,135,136];

  constructor(private route: ActivatedRoute, private commonService: CommonService, private campServiceDb: CampDb,
    private blockServiceDb: BlockDb, private subBlockServiceDb: SubBlockDb, private formBuilder: FormBuilder,
    private userServiceDb: UserDB, private toastrService: ToastrService, private onlineBeneficiaryService: OnlineBeneficiaryService,
    private beneficiaryDb: BeneficiaryDb, private beneficiaryRecordDb: BeneficiaryRecordsDB,
    private beneficiaryIndicatorDb: BeneficiaryIndicatorDb,private location:Location, 
    private beneficiaryDataCollectionDbService:BeneficiaryDataCollectionStatusDb) { }

  ngOnInit() {
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.genderList = Convert.enumToSelectList(Gender);
    this.levelOfStudyList = Convert.enumToSelectList(LevelOfStudy);

    this.loadUserFromDb();

    this.buildForm();
    this.loadAllCamps().then((campsAllResult) => {
      this.listCamps = campsAllResult;
    });
  }

  buildForm() {
    
    this.beneficiaryForm = this.formBuilder.group({
      unhcrId: ["", Validators.required],
      name: ["", Validators.required],
      fatherName: ["", Validators.required],
      motherName: ["", Validators.required],
      fcnId: ["", Validators.required],
      dateOfBirth: ["", Validators.required],
      sex: ["", Validators.required],
      disabled: ["", Validators.required],
      levelOfStudy: ["", Validators.required],
      enrollmentDate: ["", Validators.required],
      beneficiaryCampId: ["", Validators.required],
      blockId: ["", Validators.required],
      subBlockId: ["", Validators.required],
      beneficiaryCampName: [""],
      blockName:[""],
      subBlockName:[""],
      remarks: [""],
      facilityId: [this.facilityId],
      instanceId: [this.instanceId]
    });

    this.subs = new ValidationErrorBuilder()
      .withGroup(this.beneficiaryForm)
      .useMessageContainer(this.validationErrors)
      .build();
  }




  onSubmit() {
    // this.beneficiaryForm.patchValue({ facilityId: this.facilityId });
    var submittedValue = this.beneficiaryForm.value;
    console.log(submittedValue);
    if (this.beneficiaryForm.invalid) {
      this.toastrService.error(MessageConstant.fillUpAllValue,MessageConstant.recordNotSaved);
      return;
    }

    if (this.onlineOfflineStatus == OnlineOfflineStatus.Offline) {
      var beneficiary = new Beneficiary();

      this.setBeneficiaryInModel(beneficiary, submittedValue);

      this.loadBeneficiaryIndicators().then((indicatorsResult) => {

        this.beneficiaryDb.saveBeneficiary(beneficiary).subscribe((beneficiaryUniqueId) => {
            
          var beneficiaryDataCollection = new BeneficiaryDataCollectionStatus();
            beneficiaryDataCollection.beneficiaryId = beneficiaryUniqueId;
            beneficiaryDataCollection.instanceId = this.instanceId;
            beneficiaryDataCollection.status = CollectionStatus.NotCollected;
            
           this.beneficiaryDataCollectionDbService.saveBeneficiaryDataCollectionStatus(beneficiaryDataCollection)
          .subscribe(() => {
            let recordPromises = [];

            indicatorsResult.map((indicator) => {
              var beneficiaryRecord = new BeneficiaryRecord();

              if(this.findBeneficiaryCreateOfflineIndicatorId(indicator) === true){
                beneficiaryRecord.beneficiaryId = beneficiaryUniqueId;
                beneficiaryRecord.instanceId = indicator.instanceId;
                beneficiaryRecord.columnId = indicator.id;
                beneficiaryRecord.status = CollectionStatus.Collected;
                beneficiaryRecord.value = this.offlineCreateBeneficiaryRecordFind(indicator.entityDynamicColumnId,submittedValue);
              }
              else{
                beneficiaryRecord.beneficiaryId = beneficiaryUniqueId;
                beneficiaryRecord.instanceId = indicator.instanceId;
                beneficiaryRecord.columnId = indicator.id;
                beneficiaryRecord.status = CollectionStatus.NotCollected;
                beneficiaryRecord.value = "";
              }              
  
              recordPromises.push(this.saveRecordsForBeneficiary(beneficiaryRecord));
            });
  
            Promise.all(recordPromises).then((recordResults) => {
              console.log("Beneficiary Record Inserted for All Indicator - ", recordResults);
              this.toastrService.success(MessageConstant.SaveSuccessful);

            });
          });
        })
      });
    }
    else {
      debugger;
      this.onlineBeneficiaryService.saveBeneficiary(submittedValue).then((result) => {
        console.log(result);
        this.toastrService.success(MessageConstant.SaveSuccessful);
      });
    }

    this.location.back();
  }
findBeneficiaryCreateOfflineIndicatorId(indicator){
  return this.beneficiaryCreateEntityDynamicIds.find(x => x === indicator.entityDynamicColumnId) ? true : false
}
  offlineCreateBeneficiaryRecordFind(entityDynamicColumnId,submittedValue){
    if(entityDynamicColumnId === 122){return submittedValue['unhcrId'];}
    else if(entityDynamicColumnId === 123){return submittedValue['name']}
    else if(entityDynamicColumnId === 124){return submittedValue['fatherName']}
    else if(entityDynamicColumnId === 125){return submittedValue['motherName']}
    else if(entityDynamicColumnId === 126){return submittedValue['fcnId']}
    else if(entityDynamicColumnId === 127){return submittedValue['dateOfBirth']}
    else if(entityDynamicColumnId === 128){return submittedValue['sex']}
    else if(entityDynamicColumnId === 129){return submittedValue['disabled']}
    else if(entityDynamicColumnId === 130){return submittedValue['levelOfStudy']}
    else if(entityDynamicColumnId === 131){return submittedValue['enrollmentDate']}
    else if(entityDynamicColumnId === 132){return this.facilityId;}
    else if(entityDynamicColumnId === 133){return submittedValue['beneficiaryCampId']}
    else if(entityDynamicColumnId === 134){return submittedValue['blockId']}
    else if(entityDynamicColumnId === 135){return submittedValue['subBlockId']}
    else if(entityDynamicColumnId === 136){return submittedValue['remarks']}
  }

  saveRecordsForBeneficiary(beneficiaryRecord: BeneficiaryRecord) {
    return new Promise<number>((resolve, reject) => {
      this.beneficiaryRecordDb.saveBeneficiaryRecord(beneficiaryRecord).subscribe((recordResult) => {
        resolve(recordResult);
      });
    });
  }

  setBeneficiaryInModel(beneficiary: Beneficiary, submittedValue) {
    beneficiary.id = 0;
    beneficiary.UnhcrId = submittedValue['unhcrId'];
    beneficiary.beneficiaryName = submittedValue['name'];
    beneficiary.fatherName = submittedValue['fatherName'];
    beneficiary.motherName = submittedValue['motherName'];
    beneficiary.FCNId = submittedValue['fcnId'];
    beneficiary.facilityId = this.facilityId;
    beneficiary.beneficiaryCampId = submittedValue['beneficiaryCampId'];
    beneficiary.blockId = submittedValue['blockId'];
    beneficiary.subBlockId = submittedValue['subBlockId'];
    beneficiary.facilityName = this.facilityName;
    beneficiary.beneficiaryCampName = submittedValue['beneficiaryCampName'];
    beneficiary.blockName = submittedValue['blockName'];
    beneficiary.subBlockName = submittedValue['subBlockName'];
    beneficiary.dateOfBirth = submittedValue['dateOfBirth'];
    beneficiary.disabled = submittedValue['disabled'];
    beneficiary.disengaged = false;
    beneficiary.enrollmentDate = submittedValue['enrollmentDate'];
    beneficiary.levelOfStudy = submittedValue['levelOfStudy'];
    beneficiary.remarks = submittedValue['remarks'];
    beneficiary.sex = submittedValue['sex'];
    debugger;
  }

  loadBeneficiaryIndicators() {
    return new Promise<BeneficiaryIndicator[]>((resolve, reject) => {
      this.beneficiaryIndicatorDb.getAllBeneficiaryIndicators().subscribe((indicatorResults) => {
        resolve(indicatorResults);
      });
    });
  }

  loadUserFromDb() {
    this.userServiceDb.getUser().subscribe((data) => {
      this.user = data[0];
    })
  }

  loadAllCamps() {
    return new Promise<Camp[]>((resolve, reject) => {
      if (this.onlineOfflineStatus === OnlineOfflineStatus.Online) {
        this.commonService.getCamps({ pageNo: 1, pageSize: 1000 }).then((data) => {
          resolve(data.data);
        })
      }
      else {
        this.campServiceDb.getAllCamps().subscribe((campsAllResult) => {
          resolve(campsAllResult);
        });
      }
    });
  }

  loadBlocksWithCampId(campId: number) {
    return new Promise<Block[]>((resolve, reject) => {
      if (this.onlineOfflineStatus === OnlineOfflineStatus.Online) {
        this.commonService.getBlocks({ pageNo: 1, pageSize: 1000, campId: campId }).then((data) => {
          resolve(data.data);
        })
      }
      else {
        this.blockServiceDb.getBlocksByCampId(campId).subscribe((blocksAllResult) => {
          resolve(blocksAllResult);
        });
      }
    });
  }

  loadSubBlocksWithBlockId(blockId) {
    return new Promise<SubBlock[]>((resolve, reject) => {
      if (this.onlineOfflineStatus === OnlineOfflineStatus.Online) {
        this.commonService.getSubBlocks({ pageNo: 1, pageSize: 1000, blockId: blockId }).then((data) => {
          resolve(data.data);
        })
      }
      else {
        this.subBlockServiceDb.getSubBlocksByBlockId(blockId).subscribe((subBlocksAllResult) => {
          resolve(subBlocksAllResult);
        });
      }
    });
  }

  onClickCamp() {
    this.changeCampId = false;
    this.beneficiaryForm.patchValue({ beneficiaryCampId: this.onChangeSelectedCampId });
    this.beneficiaryForm.patchValue({ beneficiaryCampName: this.onChangeSelectedCampName });
    this.selectedCampName = this.onChangeSelectedCampName;
    this.selectedCampId = this.onChangeSelectedCampId;

    this.loadBlocksWithCampId(this.selectedCampId).then((blocksAllResult) => {
      this.listBlocks = blocksAllResult;
    });
  }

  onClickBlock() {
    this.changeBlockId = false;
    this.beneficiaryForm.patchValue({ blockId: this.onChangeSelectedBlockId });
    this.beneficiaryForm.patchValue({ blockName: this.onChangeSelectedBlockName });
    this.selectedBlockId = this.onChangeSelectedBlockId;
    this.selectedBlockName = this.onChangeSelectedBlockName;

    this.loadSubBlocksWithBlockId(this.selectedBlockId).then((subBlocksAllResult) => {
      this.listSubBlocks = subBlocksAllResult;
    });
  }

  onClickSubBlock() {
    this.changeSubBlockId = false;
    this.beneficiaryForm.patchValue({ subBlockId: this.onChangeSelectedSubBlockId });
    this.beneficiaryForm.patchValue({ subBlockName: this.onChangeSelectedSubBlockName });
    this.selectedSubBlockId = this.onChangeSelectedSubBlockId;
    this.selectedSubBlockName = this.onChangeSelectedSubBlockName;
  }

  closeBlockModalClicked(){
    this.changeBlockId = false;
    this.blockSelectModalClose.nativeElement.click();
  }

  closeCampModalClicked(){
    this.changeCampId = false;
    this.campSelectModalClose.nativeElement.click();
  }

  closeSubBlockModalClicked(){
    this.changeSubBlockId = false;
    this.subBlockSelectModalClose.nativeElement.click();
  }

  campRadioOnChange(camp: Camp) {
    this.onChangeSelectedCampId = camp.id;
    this.onChangeSelectedCampName = camp.name;
    this.changeCampId = true;
  }

  subBlockRadioOnChange(subBlock: SubBlock) {
    this.onChangeSelectedSubBlockId = subBlock.id;
    this.onChangeSelectedSubBlockName = subBlock.name;
    this.changeSubBlockId = true;
  }

  blockRadioOnChange(block: Block) {
    this.onChangeSelectedBlockId = block.id;
    this.onChangeSelectedBlockName = block.name;
    this.changeBlockId = true;
  }

  ngOnDestroy(): void {
    this.subs.forEach(x => x.unsubscribe());
  }
}
