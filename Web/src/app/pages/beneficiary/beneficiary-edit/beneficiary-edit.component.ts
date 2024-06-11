import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxPaginationModule } from 'ngx-pagination';
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
import { BeneficiaryEditViewModel } from 'src/app/models/beneficiary/beneficiaryEditViewModel';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Globals } from 'src/app/globals';
import { DateFactory } from 'src/app/utility/DateFactory';
@Component({
  selector: 'app-beneficiary-edit',
  templateUrl: './beneficiary-edit.component.html',
  styleUrls: ['./beneficiary-edit.component.scss']
})
export class BeneficiaryEditComponent implements OnInit {
  public p: any;
  public pagedInstances: IPagedResponse<Instance>;
  public pagedFacility: IPagedResponse<FacilityViewModel>;
  public beneficiaryCreateViewModel: BeneficiaryCreateViewModel;
  public beneficiaryEditViewModel: BeneficiaryEditViewModel;
  public dpConfig;
  @ViewChild('facilityModalClose') facilitySelectModalClose;
  @ViewChild('blockModalClose') blockSelectModalClose;
  @ViewChild('subBlockModalClose') subBlockSelectModalClose;
  @ViewChild('campModalClose') campSelectModalClose;

  public dataHolderName: string;
  public dataHolderPhone: string;

  public facilities: FacilityViewModel[];
  public facilityConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempFacilityViewModel: FacilityViewModel;
  public selectedFacilityViewModel: FacilityViewModel;


  public blocks: BlockViewModel[];
  public blockConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempBlockId: number;
  public changeBlockId: boolean = false;
  public selectedTempBlockName: string;
  public selectedBlockId: number = 0;
  public selectedBlockName: string;

  public subBlocks: SubBlockViewModel[];
  public subBlockConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempSubBlockId: number;
  public changeSubBlockId: boolean = false;
  public selectedTempSubBlockName: string;
  public selectedSubBlockId: number = 0;
  public selectedSubBlockName: string;

  public camps: CampViewModel[];
  public campConfig = {
    itemsPerPage: 10,
    currentPage: 1,
    total: 0,
    searchText: ""
  };
  public selectedTempCampId: number;
  public changeCampId: boolean = false;
  public selectedTempCampName: string;
  public selectedCampId: number = 0;
  public selectedCampName: string;

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
  ];
  public selectedGenderValue: number;
  public selectedGenderName: string;

  public bForm: FormGroup;
  constructor(
    private httpClientService: HttpClientService,
    private toast: ToastrService,
    private router: Router,
    private beneficiaryService: BeneficiaryService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private globals: Globals
  ) {
    this.selectedFacilityViewModel = new FacilityViewModel();
    this.beneficiaryCreateViewModel = new BeneficiaryCreateViewModel();
    this.dpConfig = {
      ...this.globals.bsDatepickerConfig
    }
  }
  public editState: boolean = false;

  ngOnInit() {

    this.loadCamp(this.campConfig.currentPage, this.campConfig.itemsPerPage, this.campConfig.searchText);
    this.route.params.subscribe((params) => {
      var id = params['id'];
      this.editState = id != 0 ? true : false;
      var instanceId = params['instanceId'];
      this.loadBeneficiary(id, instanceId);
      this.loadFacility(instanceId, this.facilityConfig.currentPage, this.facilityConfig.itemsPerPage, this.facilityConfig.searchText);
    });
    this.loadDataHolder();

  }

  buildForm() {
    this.bForm = this.fb.group({

    })
  }

  loadDataHolder() {
    var data = JSON.parse(localStorage.getItem('userProfile'));
    this.dataHolderName = data.fullName;
    this.dataHolderPhone = data.phoneNumber;
  }


  loadBeneficiary(id, instanceId) {
    if (id == 0) {
      this.beneficiaryCreateViewModel = {
        id: 0,
        beneficiaryCampId: null,
        blockId: null,
        dateOfBirth: null,
        disabled: false,
        enrollmentDate: null,
        facilityCampId: null,
        facilityCode: null,
        facilityId: null,
        fatherName: null,
        fcnId: null,
        isActive: false,
        isApprove: false,
        levelOfStudy: null,
        motherName: null,
        name: null,
        remarks: null,
        sex: null,
        subBlockId: null,
        unhcrId: null,
        instanceId: instanceId
      };
      return;
    }

    this.beneficiaryService.getById(id, instanceId).then(a => {
      this.beneficiaryEditViewModel = a;
      Object.assign(this.beneficiaryCreateViewModel, a);
      this.beneficiaryCreateViewModel.instanceId = instanceId;
      this.beneficiaryCreateViewModel.dateOfBirth =
        DateFactory.toModel(this.beneficiaryCreateViewModel.dateOfBirth.toString());
      this.beneficiaryCreateViewModel.enrollmentDate =
        DateFactory.toModel(this.beneficiaryCreateViewModel.enrollmentDate.toString());

      this.selectedGenderValue = this.beneficiaryEditViewModel.sex;
      this.gender.filter(a => a.value == this.beneficiaryEditViewModel.sex).map(a => {
        this.selectedGenderName = a.name
      });
      this.selectedlevelOfStudyValue = this.beneficiaryEditViewModel.levelOfStudy;
      this.levelOfStudy.filter(a => a.value == this.beneficiaryEditViewModel.levelOfStudy).map(a => {
        this.selectedlevelOfStudyName = a.name
      });
      this.selectedCampId = this.beneficiaryEditViewModel.beneficiaryCampId;
      this.selectedCampName = this.beneficiaryEditViewModel.beneficiaryCampName;
      this.selectedSubBlockId = this.beneficiaryEditViewModel.subBlockId;
      this.selectedSubBlockName = this.beneficiaryEditViewModel.subBlockName;
      this.selectedBlockId = this.beneficiaryEditViewModel.blockId;
      this.selectedBlockName = this.beneficiaryEditViewModel.blockName;

      this.selectedFacilityViewModel.id = this.beneficiaryEditViewModel.facilityId;
      this.selectedFacilityViewModel.programmingPartnerName = this.beneficiaryEditViewModel.programmingPartnerName;
      this.selectedFacilityViewModel.implemantationPartnerName = this.beneficiaryEditViewModel.implemantationPartnerName;
      this.selectedFacilityViewModel.campName = this.beneficiaryEditViewModel.facilityCampName;
    });
  }
  loadFacility(instanceId, pageNumber, pageSize, searchText) {
    var data = this.httpClientService.getAsync(ApiConstant.GetAllFacilityObjectByBeneficiaryInstance + '?PageSize=' + pageSize + '&PageNo=' + pageNumber + '&SearchText=' + searchText + '&InstanceId=' + instanceId)
      .then((data) => {
        //this.dynamicColumn = data.data;
        this.facilityConfig.currentPage = pageNumber;
        this.facilityConfig.total = data.total;

        this.facilities = data.data;
      })
  }
  pageChangedFacility(event) {
    this.route.params.subscribe((params) => {
      var instanceId = params['instanceId'];
      this.loadFacility(instanceId, event, this.facilityConfig.itemsPerPage, this.facilityConfig.searchText);
    });
  }
  facilityRadioOnChange(facility) {
    this.selectedTempFacilityViewModel = facility;
    this.selectedFacilityViewModel = facility;
    //this.beneficiaryCreateViewModel.facilityId=facility.id;
  }
  facilitySearchChange() {
    this.route.params.subscribe((params) => {
      var instanceId = params['instanceId'];
      this.loadFacility(instanceId, 1, this.facilityConfig.itemsPerPage, this.facilityConfig.searchText);
    });
  }
  selectFacilityClicked() {
    if (this.selectedTempFacilityViewModel.id > 0) {
      this.selectedFacilityViewModel = this.selectedTempFacilityViewModel;
      this.beneficiaryCreateViewModel.facilityId = this.selectedTempFacilityViewModel.id;
      this.beneficiaryCreateViewModel.facilityCode = this.selectedTempFacilityViewModel.facilityCode;
      this.facilitySelectModalClose.nativeElement.click();
    }
  }
  loadBlock(pageNumber, pageSize, searchText) {
    var data = this.httpClientService.getAsync(ApiConstant.GetBlocks + '?PageSize=' + pageSize + '&PageNo=' + pageNumber + '&SearchText=' + searchText + '&CampId=' + this.selectedCampId)
      .then((data) => {
        //this.dynamicColumn = data.data;
        this.blockConfig.currentPage = pageNumber;
        this.blockConfig.total = data.total;

        this.blocks = data.data;
      })
  }
  pageChangedBlock(event) {
    this.loadBlock(event, this.blockConfig.itemsPerPage, this.blockConfig.searchText);
  }

  closeBlockModalClicked() {
    this.changeBlockId = false;
    this.blockSelectModalClose.nativeElement.click();
  }

  blockRadioOnChange(block) {
    this.selectedTempBlockId = block.id;
    this.selectedTempBlockName = block.name;
    this.changeBlockId = true;
    //this.selectedBlockId = block.id;
  }
  blockSearchChange() {
    this.loadBlock(1, this.blockConfig.itemsPerPage, this.blockConfig.searchText);
  }
  selectBlockClicked() {
    this.changeBlockId = false;
    if (this.selectedTempBlockId > 0) {
      this.selectedBlockId = this.selectedTempBlockId;
      this.selectedBlockName = this.selectedTempBlockName;
      this.beneficiaryCreateViewModel.blockId = this.selectedTempBlockId;
      this.emptySubBlock();
      this.loadSubBlock(this.subBlockConfig.currentPage, this.subBlockConfig.itemsPerPage, this.subBlockConfig.searchText);
      this.blockSelectModalClose.nativeElement.click();
    }
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

  closeSubBlockModalClicked() {
    this.changeSubBlockId = false;
    this.subBlockSelectModalClose.nativeElement.click();
  }

  subBlockRadioOnChange(subBlock) {
    this.selectedTempSubBlockId = subBlock.id;
    this.selectedTempSubBlockName = subBlock.name;
    // this.selectedSubBlockId = subBlock.id;
    this.changeSubBlockId = true;
  }
  subBlockSearchChange() {
    this.loadSubBlock(1, this.subBlockConfig.itemsPerPage, this.subBlockConfig.searchText);
  }
  selectSubBlockClicked() {
    this.changeSubBlockId = false;
    if (this.selectedTempSubBlockId > 0) {
      this.selectedSubBlockId = this.selectedTempSubBlockId;
      this.selectedSubBlockName = this.selectedTempSubBlockName;
      this.beneficiaryCreateViewModel.subBlockId = this.selectedTempSubBlockId;
      this.subBlockSelectModalClose.nativeElement.click();
    }
  }
  loadCamp(pageNumber, pageSize, searchText) {
    var data = this.httpClientService.getAsync(ApiConstant.GetCapms + '?PageSize=' + pageSize + '&PageNo=' + pageNumber + '&SearchText=' + searchText)
      .then((data) => {
        //this.dynamicColumn = data.data;
        this.campConfig.currentPage = pageNumber;
        this.campConfig.total = data.total;

        this.camps = data.data;
      })
  }
  pageChangedCamp(event) {
    this.loadCamp(event, this.campConfig.itemsPerPage, this.campConfig.searchText);
  }

  closeCampModalClicked() {
    this.changeCampId = false;
    this.campSelectModalClose.nativeElement.click();
  }

  campRadioOnChange(camp) {
    this.selectedTempCampId = camp.id;
    this.selectedTempCampName = camp.name;
    // this.selectedCampId = camp.id;
    this.changeCampId = true;
    //this.beneficiaryCreateViewModel.beneficiaryCampId = camp.id;
  }
  campSearchChange() {
    this.loadCamp(1, this.campConfig.itemsPerPage, this.campConfig.searchText);
  }
  selectCampClicked() {
    this.changeCampId = false;
    if (this.selectedTempCampId > 0) {
      this.selectedCampId = this.selectedTempCampId;
      this.selectedCampName = this.selectedTempCampName;
      this.beneficiaryCreateViewModel.beneficiaryCampId = this.selectedTempCampId;
      this.emptyBlock();
      this.emptySubBlock();
      this.loadBlock(this.blockConfig.currentPage, this.blockConfig.itemsPerPage, this.blockConfig.searchText);
      this.campSelectModalClose.nativeElement.click();
    }
  }
  emptyBlock() {
    this.selectedBlockId = 0;
    this.selectedBlockName = '';
    this.blocks = [];
    delete this.beneficiaryCreateViewModel.blockId
  }
  emptySubBlock() {
    this.selectedSubBlockId = 0;
    this.selectedSubBlockName = '';
    this.subBlocks = [];
    delete this.beneficiaryCreateViewModel.subBlockId;
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

    if (this.beneficiaryCreateViewModel.id != 0) {
      this.beneficiaryCreateViewModel.dateOfBirth =
        DateFactory.fromModel(this.beneficiaryCreateViewModel.dateOfBirth.toString());

      this.beneficiaryCreateViewModel.enrollmentDate =
        DateFactory.fromModel(this.beneficiaryCreateViewModel.enrollmentDate.toString());

      this.beneficiaryService.update(this.beneficiaryCreateViewModel).then(a => {
        this.toast.success(MessageConstant.UpdateSuccessful)
        this.router.navigate(['unicef/beneficiary/home']);
      });
    } else {
      this.beneficiaryService.save(this.beneficiaryCreateViewModel).then(a => {
        this.toast.success(MessageConstant.SaveSuccessful)
        this.router.navigate(['unicef/beneficiary/home']);
      }, (err) => {

      });
    }

  }
}
