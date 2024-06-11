  import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxPaginationModule, PaginationInstance } from 'ngx-pagination';
import { HttpClientService } from 'src/app/services/httpClientService';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiConstant } from 'src/app/utility/ApiConstant';
import { Instance } from 'src/app/models/scheduleinstance/instance';
import { instances } from 'chart.js';
import { FacilityViewModel } from 'src/app/models/facility/facilityViewModel';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { BeneficiaryCreateViewModel } from 'src/app/models/beneficiary/beneficiaryCreateViewModel';
import { BlockViewModel } from 'src/app/models/common/blockViewModel';
import { SubBlockViewModel } from 'src/app/models/common/subBlockViewModel';
import { CampViewModel } from 'src/app/models/common/campViewModel';
import { BeneficiaryService } from 'src/app/services/beneficiary.service';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { FacilityCreateViewModel } from 'src/app/models/facility/facilityCreateViewModel';
import { EducationPartnerService } from 'src/app/services/educationPartner.service';
import { EducationSectorPartner } from 'src/app/models/educationSectorPartner/educationSectorPartner';
import { CommonService } from 'src/app/services/common.service';
import { UpazilaViewModel } from 'src/app/models/common/upazilaViewModel';
import { UnionViewModel } from 'src/app/models/common/unionViewModel';
import { Camp } from 'src/app/models/common/camp.model';
import { FacilityService } from 'src/app/services/facility.service';
import { FacilityEditViewModel } from 'src/app/models/facility/facilityEditViewModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { Subscription } from 'rxjs';
import { ModalService } from 'src/app/services/modal.service';
import { EspSelectorComponent } from 'src/app/shared/components/esp-selector/esp-selector.component';
import { IEducationSectorPartner } from 'src/app/models/user/educationSectorPartner';
import { AssignTeacherComponent } from 'src/app/shared/components/assign-teacher/assign-teacher.component';
import { TeacherViewModel } from 'src/app/models/user/teacherViewModel';
@Component({
  selector: 'app-single-facility-edit',
  templateUrl: './single-facility-edit.component.html',
  styleUrls: ['./single-facility-edit.component.scss']
})
export class SingleFacilityEditComponent implements OnInit {
  pagedInstances: IPagedResponse<Instance>;
  pagedFacility: IPagedResponse<FacilityViewModel>;
  beneficiaryCreateViewModel: BeneficiaryCreateViewModel;

  facilityCreateViewModel: FacilityCreateViewModel;
  facilityEditViewModel: FacilityEditViewModel;

  @ViewChild('facilityModalClose') facilitySelectModalClose;
  @ViewChild('blockModalClose') blockSelectModalClose;
  @ViewChild('subBlockModalClose') subBlockSelectModalClose;
  @ViewChild('campModalClose') campSelectModalClose;

  @ViewChild('ppModalClose') ppSelectModalClose;
  @ViewChild('pipModalClose') ipSelectModalClose;

  public p: any;
  public hostSelected: boolean;
  private allRecords = 2147483647;

  public facilities: FacilityViewModel[];
  public facilityConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempFacilityViewModel: FacilityViewModel;
  public selectedFacilityViewModel: FacilityViewModel;

  public programPartner: EducationSectorPartner[];
  public ppConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempPpId: number;
  public selectedTempPpName: string;
  public selectedPpId: number;
  public selectedPpName: string;

  public implementaitonPartner: EducationSectorPartner[];
  public ipConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempIpId: number;
  public selectedTempIpName: string;
  public selectedIpId: number;
  public selectedIpName: string;

  public upazilas: UpazilaViewModel[];
  public selectedUpazilaId: number;
  public selectedUpazilaName: string;

  public unions: UnionViewModel[];
  public selectedUnionId: number;
  public selectedUnionName: string;

  public blocks: BlockViewModel[];
  public blockConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempBlockId: number;
  public selectedTempBlockName: string;
  public selectedBlockId: number;
  public selectedBlockName: string;
  public changeBlockId: boolean = false;

  public subBlocks: SubBlockViewModel[];
  public subBlockConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempSubBlockId: number;
  public selectedTempSubBlockName: string;
  public selectedSubBlockId: number;
  public selectedSubBlockName: string;


  public camps: Camp[];
  public campConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public campPaginationConfig: PaginationInstance = {
    id: 'camp_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }
  public selectedTempCampId: number;
  public selectedTempCampName: string;
  public selectedTempCampSSID: string;
  public selectedCampId: number;
  public selectedCampName: string;
  public selectedCampSSID: string;
  public changeCampId: boolean = false;

  public targetedPopulation = [
    { name: "Host Communities", value: 1 },
    { name: "Refugee Communities", value: 2 },
    { name: "Both Communities", value: 3 }
  ]
  public selectedtargetedPopulationValue: number;
  public selectedtargetedPopulationName: string;

  public facilityType = [
    { name: "Learning Center(LC)", value: 1 },
    { name: "Community Based Learning Facility", value: 2 },
    { name: "Cross Sectoral Shared Learning Facility (CSSLF)", value: 3 }
  ]
  public selectedfacilityTypeValue: number;
  public selectedfacilityTypeName: string;

  public facilityStatus = [
    { name: "Ongoing", value: 1 },
    { name: "Completed", value: 2 },
    { name: "Planned", value: 3 },
    { name: "Decommission", value: 4 }
  ]
  public selectedfacilityStatusValue: number;
  public selectedfacilityStatusName: string;

  public levelOfStudy = [
    { name: "Level 1", value: 1 },
    { name: "Level 2", value: 2 },
    { name: "Level 3", value: 3 },
    { name: "Level 4", value: 4 }
  ]
  public selectedlevelOfStudyValue: number;
  public selectedlevelOfStudyName: string;

  public gender = [
    { name: "Male", value: 1 },
    { name: "Female", value: 2 },
    { name: "Others", value: 3 }
  ]
  public selectedGenderValue: number;
  public selectedGenderName: string;

  public facilityForm: FormGroup;
  public validationErrors: any = {};
  subscriptions : Map<string, Subscription> = new Map();

  public instanceId: number;
  private facilityId: number;
  constructor(private httpClientService: HttpClientService, private toast: ToastrService
    , private router: Router, private route: ActivatedRoute, private beneficiaryService: BeneficiaryService
    , private educationPartnerService: EducationPartnerService
    , private commonService: CommonService
    , private facilityService: FacilityService
    , private fb: FormBuilder
    , private modalService: ModalService
    ) {
    this.selectedFacilityViewModel = new FacilityViewModel();
    this.beneficiaryCreateViewModel = new BeneficiaryCreateViewModel();

    this.facilityEditViewModel = new FacilityEditViewModel();
    this.facilityCreateViewModel = new FacilityCreateViewModel();
  }

  ngOnInit() {    
    this.loadUpazila();
    this.loadESP();
    this.facilityForm = this.fb.group({
      "id": null,
      "name": ['', [Validators.required]],
      "facilityCode": null,
      "targetedPopulation": ['', [Validators.required]],
      "facilityStatus": null,
      "latitude": null,
      "longitude": null,
      "donors": null,
      "nonEducationPartner": null,
      "programPartnerId": ['', [Validators.required]],
      "implementationPartnerId": ['', [Validators.required]],
      "upazilaId": ['', [Validators.required]],
      "unionId": ['', [Validators.required]],
      "campId": null,
      "selectedCampSSID": null,
      "paraName": null,
      "paraId": null,
      "blockId": null,
      "facilityType": null,
      "teacherId": null,
      "remarks": null,
      "teacherName": null,
      "teacherPhone": null,
      "teacherEmail": null
    });

    this.route.params.subscribe((params) => {
      this.facilityId = +params['id'];
      this.instanceId = +params['instanceId'];
      this.loadFacility(this.facilityId);
    });
    this.setValidation();
  }
  setValidation(){
    
    let msg = {
     
    };    
    
    this.subscriptions = new ValidationErrorBuilder()
      .withGroup(this.facilityForm)
      .withMessages(msg)      
      .useMessageContainer(this.validationErrors)
      .build();
  }
  loadFacility(id: number) {
    if(!id){
     return; 
    }
    this.facilityService.getById(id).then(a => {

      this.facilityForm.patchValue({ ...a });

      this.facilityEditViewModel = a;
      Object.assign(this.facilityCreateViewModel, a);

      this.selectedUpazilaId = this.facilityEditViewModel.upazilaId;
      this.selectedUpazilaName = this.facilityEditViewModel.upazilaName;
      this.selectedUnionId = this.facilityEditViewModel.unionId;
      this.selectedUnionName = this.facilityEditViewModel.unionName;
      this.selectedCampId = this.facilityEditViewModel.campId;
      this.selectedCampName = this.facilityEditViewModel.campName;
      this.selectedCampSSID = this.facilityEditViewModel.campSSID;
      this.selectedBlockId = this.facilityEditViewModel.blockId;
      this.selectedBlockName = this.facilityEditViewModel.blockName;


      this.selectedTempIpId = this.facilityEditViewModel.implementationPartnerId
      this.selectedTempIpName = this.facilityEditViewModel.implementationPartnerName;
      this.selectedIpId = this.facilityEditViewModel.implementationPartnerId;
      this.selectedIpName = this.facilityEditViewModel.implementationPartnerName;

      this.selectedTempPpId = this.facilityEditViewModel.programPartnerId
      this.selectedTempPpName = this.facilityEditViewModel.programPartnerName;
      this.selectedPpId = this.facilityEditViewModel.programPartnerId;
      this.selectedPpName = this.facilityEditViewModel.programPartnerName;


      this.selectedtargetedPopulationValue = this.facilityEditViewModel.targetedPopulation;
      this.targetedPopulation.filter(a => a.value == this.facilityEditViewModel.targetedPopulation).map(a => {
        this.selectedtargetedPopulationName = a.name
      });

      this.selectedfacilityTypeValue = this.facilityEditViewModel.facilityType;
      this.facilityType.filter(a => a.value == this.facilityEditViewModel.facilityType).map(a => {
        this.selectedfacilityTypeName = a.name
      });
      this.selectedfacilityStatusValue = this.facilityEditViewModel.facilityStatus;
      this.facilityStatus.filter(a => a.value == this.facilityEditViewModel.facilityStatus).map(a => {
        this.selectedfacilityStatusName = a.name
      });
      this.targetedPopulationSelect(this.selectedtargetedPopulationValue);
      this.loadUpazila();
      this.loadUnion(this.facilityEditViewModel.upazilaId);
      this.loadCamp(this.campConfig.currentPage, this.campConfig.itemsPerPage, this.facilityEditViewModel.unionId, this.campConfig.searchText);
      if (this.facilityEditViewModel.campId)
        this.loadBlock(this.blockConfig.currentPage, this.blockConfig.itemsPerPage, this.blockConfig.searchText, this.facilityEditViewModel.campId);
    });
  }

  loadUpazila() {
    this.commonService.getUpazilas({
      pageSize: this.allRecords,
      pageNo: 1
    }).then(data => {
      this.upazilas = data.data;
    })
  }
  selectUpazila(upazillaId: number) {     
    this.loadUnion(upazillaId);
    this.emptyCamp();
    this.emptyBlock();
  }
  loadUnion(upazilaId) {
    this.commonService.getUnions({
      pageSize: this.allRecords,
      pageNo: 1,
      upazilaId: upazilaId
    }).then(data => {
      this.unions = data.data;
     
    })
  }
  selectUnion(unionId: number) {
    
    
    this.emptyCamp();
    this.emptyBlock();
    this.loadCamp(
        this.campConfig.currentPage, 
        this.campConfig.itemsPerPage, 
        unionId, 
        this.campConfig.searchText);
  }
  loadESP() {
    this.educationPartnerService.getAll().then(data => {
      this.programPartner = data;
      this.implementaitonPartner = data;
    })
  }
  ppRadioOnChange(pp) {

    this.selectedTempPpId = pp.id;
    this.selectedTempPpName = pp.partnerName;
  }

  selectPpClicked() {
    // Todo: Use modal service to show esp selector modal here

    this.modalService.open<EspSelectorComponent, IEducationSectorPartner>(EspSelectorComponent, {
      modalTitle: "Select Programming Partner",
      fieldNameLabel: "Programming Partner Name",
      selectedEsp: {
        id:  this.facilityCreateViewModel.programPartnerId 
      }
    })
    .then((result: IEducationSectorPartner) => {      
      if(!result){
        return;
      }
      this.facilityCreateViewModel.programPartnerId = result.id;
      this.facilityCreateViewModel.programPartnerName =  result.partnerName;
      this.facilityForm.controls['programPartnerId'].patchValue(result.id);
    });    
  }

  selectIpClicked() {

    this.modalService.open<EspSelectorComponent, IEducationSectorPartner>(EspSelectorComponent, {
      modalTitle: "Select Implementing Partner",
      fieldNameLabel: "Implementing Partner Name",
      selectedEsp: {
        id:  this.facilityCreateViewModel.implementationPartnerId 
      }
    })
    .then((result: IEducationSectorPartner) => {      
      if(!result){
        return;
      }
      this.facilityCreateViewModel.implementationPartnerId = result.id;
      this.facilityCreateViewModel.implementationPartnerName =  result.partnerName;
      this.facilityForm.controls['implementationPartnerId'].patchValue(result.id);
    }); 

  }
  
  ipRadioOnChange(pp) {

    this.selectedTempIpId = pp.id;
    this.selectedTempIpName = pp.partnerName;
  }
  selectProgrammingPartner(esp) {
    this.facilityCreateViewModel.programPartnerId = esp.id;
    this.facilityCreateViewModel.programPartnerName = esp.partnerName;
  }
  selectImplementationPartner(esp) {
    this.facilityCreateViewModel.implementationPartnerId = esp.id;
    this.facilityCreateViewModel.implementationPartnerName = esp.partnerName;
  }
  targetedPopulationSelect(targetPopulation) {
    this.selectedtargetedPopulationValue = targetPopulation.value;
    this.selectedtargetedPopulationName = targetPopulation.name;
    this.facilityCreateViewModel.targetedPopulation = targetPopulation.value;

    if (targetPopulation == TargetPopulation.Host_Communities) {
      this.facilityForm.controls['campId'].patchValue(null);
      this.facilityForm.controls['selectedCampSSID'].patchValue(null);
      this.facilityForm.controls['facilityType'].patchValue(null);
      this.facilityForm.controls['facilityStatus'].patchValue(null);
      this.facilityForm.controls['blockId'].patchValue(null);

      this.selectedCampId = null;
      this.selectedCampName = null;
      this.facilityCreateViewModel.campId = null;
      this.selectedCampSSID = null;

      this.selectedBlockId = null;
      this.selectedBlockName = null;
      this.facilityCreateViewModel.blockId = null;

      this.selectedfacilityStatusValue = null;
      this.selectedfacilityStatusName = null;
      this.facilityCreateViewModel.facilityStatus = null;

      this.selectedfacilityTypeValue = null;
      this.selectedfacilityTypeName = null;
      this.facilityCreateViewModel.facilityType = null;

      this.hostSelected = true;

    }
    else {
      this.hostSelected = false;
    }
  }
  facilityTypeSelect(level) {
    this.selectedfacilityTypeValue = level.value;
    this.selectedfacilityTypeName = level.name;
    this.facilityCreateViewModel.facilityType = level.value;
  }
  facilityStatusSelect(level) {
    this.selectedfacilityStatusValue = level.value;
    this.selectedfacilityStatusName = level.name;
    this.facilityCreateViewModel.facilityStatus = level.value;
  }

  loadBlock(pageNumber, pageSize, searchText, campId) {
    var data = this.httpClientService.getAsync(ApiConstant.GetBlocks + '?PageSize=' + pageSize + '&PageNo=' + pageNumber + '&SearchText=' + searchText + '&CampId=' + campId)
      .then((data) => {
        //this.dynamicColumn = data.data;
        this.blockConfig.currentPage = pageNumber;
        this.blockConfig.total = data.total;

        this.blocks = data.data;
      })
  }

  closeBlockModalClicked() {
    this.changeBlockId = false;
    this.blockSelectModalClose.nativeElement.click();
  }

  pageChangedBlock(event) {
    this.loadBlock(event, this.blockConfig.itemsPerPage, this.blockConfig.searchText, this.selectedCampId);
  }
  blockRadioOnChange(block) {
    this.selectedTempBlockId = block.id;
    this.selectedTempBlockName = block.name;
    this.changeBlockId = true;

  }
  blockSearchChange() {
    this.loadBlock(1, this.blockConfig.itemsPerPage, this.blockConfig.searchText, this.selectedCampId);
  }
  selectBlockClicked() {
    this.changeBlockId = false;

    if (this.selectedTempBlockId > 0) {
      this.selectedBlockId = this.selectedTempBlockId;
      this.selectedBlockName = this.selectedTempBlockName;
      this.facilityCreateViewModel.blockId = this.selectedTempBlockId;

      this.facilityForm.controls['blockId'].patchValue(this.selectedBlockId);
      //this.loadSubBlock(this.subBlockConfig.currentPage, this.subBlockConfig.itemsPerPage, this.subBlockConfig.searchText);
      this.blockSelectModalClose.nativeElement.click();
    }
  }
  emptyBlock() {
    this.selectedBlockId = 0;
    this.selectedBlockName = '';
    this.blocks=[]
    delete this.facilityCreateViewModel.blockId
    this.facilityForm.controls['blockId'].patchValue(null);
  }

  loadSubBlock(pageNumber, pageSize, searchText) {
    var data = this.httpClientService.getAsync(ApiConstant.GetSubBlocks + '?PageSize=' + pageSize + '&PageNo=' + pageNumber + '&SearchText=' + searchText + '&BlockId=' + this.selectedBlockId)
      .then((data) => {
        //this.dynamicColumn = data.data;
        this.subBlockConfig.currentPage = pageNumber;
        this.subBlockConfig.total = data.total;

        this.subBlocks = data.data;
      })
  }
  pageChangedSubBlock(event) {
    this.loadSubBlock(event, this.subBlockConfig.itemsPerPage, this.subBlockConfig.searchText);
  }
  subBlockRadioOnChange(block) {
    this.selectedTempSubBlockId = block.id;
    this.selectedTempSubBlockName = block.name;

  }

  
  closeSubBlockModalClicked() {
    this.subBlockSelectModalClose.nativeElement.click();
  }

  subBlockSearchChange() {
    this.loadSubBlock(1, this.subBlockConfig.itemsPerPage, this.subBlockConfig.searchText);
  }
  selectSubBlockClicked() {

    if (this.selectedTempSubBlockId > 0) {
      this.selectedSubBlockId = this.selectedTempSubBlockId;
      this.selectedSubBlockName = this.selectedTempSubBlockName;
      this.beneficiaryCreateViewModel.subBlockId = this.selectedTempSubBlockId;
      this.subBlockSelectModalClose.nativeElement.click();
    }
  }
  loadCamp(pageNumber, pageSize, unionId, searchText) {
    var data = this.commonService.getCamps(
      {
        pageSize: pageSize,
        pageNo: pageNumber,
        unionId: unionId
      }
    )
      .then((data) => {
        //this.dynamicColumn = data.data;
        this.campConfig.currentPage = pageNumber;
        this.campConfig.total = data.total;
        this.campPaginationConfig.totalItems = data.total;
        //  this.campPaginationConfig.currentPage = data.pageNo;
        this.camps = data.data;
        
      })
  }
  pageChangedCamp(event) {
    this.campPaginationConfig.currentPage = event;
    this.loadCamp(event, this.campConfig.itemsPerPage, this.selectedUnionId, this.campConfig.searchText);
  }
  campRadioOnChange(camp) {
    this.selectedTempCampId = camp.id;
    this.selectedTempCampName = camp.name;
    this.selectedTempCampSSID = camp.ssid;
    this.changeCampId = true;

    //this.beneficiaryCreateViewModel.beneficiaryCampId = camp.id;
  }
  campSearchChange() {
    this.loadCamp(1, this.campConfig.itemsPerPage, this.selectedUnionId, this.campConfig.searchText);
  }
 
  closeCampModalClicked() {
    this.changeCampId = false;
    this.campSelectModalClose.nativeElement.click();
  }

  selectCampClicked() {
    this.changeCampId = false;

    if (this.selectedTempCampId > 0) {
      this.selectedCampId = this.selectedTempCampId;
      this.selectedCampName = this.selectedTempCampName;
      this.facilityCreateViewModel.campId = this.selectedTempCampId;
      this.selectedCampSSID = this.selectedTempCampSSID;

      this.facilityForm.controls['campId'].patchValue(this.selectedCampId);
      this.facilityForm.controls['selectedCampSSID'].patchValue(this.selectedCampSSID);
      this.emptyBlock();
      this.loadBlock(this.blockConfig.currentPage, this.blockConfig.itemsPerPage, this.blockConfig.searchText, this.selectedCampId);
      this.campSelectModalClose.nativeElement.click();
    }
  }
  emptyCamp() {
    this.selectedCampId = 0;
    this.selectedCampName = '';
    this.selectedCampSSID= '';
    this.camps=[];
    delete this.facilityCreateViewModel.campId
    this.facilityForm.controls['campId'].patchValue(null);
    this.facilityForm.controls['selectedCampSSID'].patchValue(null);
  }
  genderSelect(gender) {
    this.selectedGenderValue = gender.value;
    this.selectedGenderName = gender.name;
    this.beneficiaryCreateViewModel.sex = gender.value;
  }
  levelOfStudySelect(level) {
    this.selectedlevelOfStudyValue = level.value;
    this.selectedlevelOfStudyName = level.name;
    this.beneficiaryCreateViewModel.levelOfStudy = level.value;
  }
  setDisability(val) {
    this.beneficiaryCreateViewModel.disabled = val;
  }

  save() {    
    if(!this.instanceId){
      this.toast.error("Invalid instance");
      return;
    }

    let promise: Promise<any>;
    if(this.facilityId){
      promise = this.facilityService.update({
        ...this.facilityForm.value,
        instanceId: this.instanceId
      });
    }
    else{
      promise = this.facilityService.save({
        ...this.facilityForm.value,
        instanceId: this.instanceId
      });
    }
    
    promise.then(a => {
      this.toast.success(MessageConstant.SaveSuccessful)
      this.router.navigate(['unicef/facility/home']);
    });
  }


  selectTeacherClicked() {    
    this.modalService.open<AssignTeacherComponent, TeacherViewModel>(AssignTeacherComponent, {})
    .then((result: TeacherViewModel)=> {
      if(!result){
        return;
      }
      
      this.facilityForm.get('teacherId').setValue(result.id);
      this.facilityForm.get('teacherName').setValue(result.fullName);
      this.facilityForm.get('teacherEmail').setValue(result.email);
      this.facilityForm.get('teacherPhone').setValue(result.phoneNumber);
      this.facilityForm.updateValueAndValidity();
    })
  }
}
