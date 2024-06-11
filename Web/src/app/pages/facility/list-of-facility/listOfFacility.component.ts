import { Component, OnInit, ViewChild } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { HttpClientService } from 'src/app/services/httpClientService';
import { ApiConstant } from 'src/app/utility/ApiConstant';
import { EntityType } from 'src/app/_enums/entityType';
import { GridView } from 'src/app/models/gridView/gridView';


import { AgCustomCheckboxRenderer } from 'src/app/components/agGrid/ag-customCheckboxRenderer.component';
import { AgCustomCheckboxEditor } from 'src/app/components/agGrid/ag-customCheckboxEditor.component';

import { GridOptions, IDatasource, IGetRowsParams, GridApi } from 'ag-grid-community';
import { BeneficiaryService } from 'src/app/services/beneficiary.service';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from 'src/app/models/beneficiary/beneficiaryViewModel';
import { GridColumnDef } from 'src/app/models/dynamicColumn/gridColumnDef';
import { GridRowDef } from 'src/app/models/dynamicColumn/gridRowDef';
import { InstanceService } from 'src/app/services/instance.service';
import { GridviewService } from 'src/app/services/gridView.service';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { DynamicColumnSaveViewModel } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { Router } from '@angular/router';
import { EntityDynamicColumn } from 'src/app/models/dynamicColumn/entityDynamicColumn';
import { FacilityViewModel } from 'src/app/models/facility/facilityViewModel';
import { FacilityService } from 'src/app/services/facility.service';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { FacilityType } from 'src/app/_enums/facilityType';
import { FacilityStatus } from 'src/app/_enums/facilityStatus';
import { TeacherViewModel } from 'src/app/models/user/teacherViewModel';
import { PaginationInstance } from 'ngx-pagination';
import { ModalService } from 'src/app/services/modal.service';
import { IMportResultComponent } from 'src/app/shared/components/import-result/import-result.component';
import { FacilityFilter } from 'src/app/models/facility/facilityFilterModel';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AssignTeacherComponent } from 'src/app/shared/components/assign-teacher/assign-teacher.component';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { CacheConstant } from 'src/app/utility/CacheConstant';


@Component({
  selector: 'list-of-component-home',
  templateUrl: './listOfFacility.component.html',
  styleUrls: ['./listOfFacility.component.scss']
})
export class ListOfFacilityComponent implements OnInit {
  @ViewChild('deleteBeneficiaryModalClose') deleteBeneficiaryModalClose;
  @ViewChild('saveDynamicColumnClose') saveDynamicColumnClose;
  @ViewChild('assignTeacherClose') assignTeacherClose;
  @ViewChild('facilityImportInput') facilityImportInput;
  @ViewChild('filter') facilityFilter: any;

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('customCellTpl') customCellTpl: any;

  public p: any;
  public facilityFilterModel: FacilityFilter;
  public getFacilityFilterModel: FacilityFilter;

  public teachers: TeacherViewModel[];
  public selectedTempTeacherId: number;
  public selectedTempTeacherName: string;
  public selectedTeacherId: number;
  public selectedTeacherName: string;


  public entityDynamicColumn: DynamicColumnSaveViewModel;
  public beneficiarySelectedRow: any[] = [];

  public cellOldValue: any;
  public dataType = [
    { name: "Int", value: 1 },
    { name: "Text", value: 2 },
    { name: "Decimal", value: 3 },
    { name: "Datetime", value: 4 },
    { name: "Boolean", value: 5 },
    { name: "List", value: 6 }
  ]

  public selectedDataType: string = "";
  public selectedDataTypeValue: number;

  public frameworkComponents;
  public pagedBeneficiaries: IPagedResponse<BeneficiaryViewModel>;
  public pagedFacilities: IPagedResponse<FacilityViewModel>;


  public teacherPaginationConfig = {
    id: 'teacher_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0,
    searchText: ""
  }


  public searchTerms = new Subject<string>();

  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;


  public page = { totalElements: 0, pageNumber: 1, size: 10 };

  constructor(
    private router: Router,
    private toast: ToastrService,
    private beneficiaryService: BeneficiaryService,
    private instanceService: InstanceService,
    private gridViewService: GridviewService,
    private dynamicColumnService: DynamicColumnService,
    private facilityService: FacilityService,
    private modalService: ModalService,
    private localStorage: LocalStorageService
  ) {
    this.facilityFilterModel = new FacilityFilter();
    this.facilityFilterModel.programPartner = [];
    this.facilityFilterModel.implementationPartner = [];
    this.facilityFilterModel.teachers = [];
    // console.log('column data type',Object.keys(ColumnDataType).filter(key => !isNaN(Number(ColumnDataType[key]))));
    this.entityDynamicColumn = new DynamicColumnSaveViewModel();

    this.searchTerms.pipe(
      // wait 400ms after each keystroke before considering the term
      debounceTime(400),
      // ignore new term if same as previous term
      distinctUntilChanged(),

    ).subscribe(value => {
      this.facilityFilterModel.searchText = value;
      this.loadGridData(1).then(a => {
        this.totalSelected = []
        this.selected = []
      });
    });
    this.rows = [];
    this.columns = [];
  }
  search(term: string): void {
    this.searchTerms.next(term);
  }

  rowIdentity = (row: any) => {
    return row.id;
  }

  onActivate(event) {
    if (event.type == 'click') {
      console.log(event.row);
    }
  }

  getRowClass = (row) => {
    return {
      'row-color1': row.gender == "Male",
      'row-color2': row.gender == "Female",
    };
  }

  setPage(pageInfo) {
    console.log('pagination change', pageInfo);
    this.page.pageNumber = pageInfo.page;
    this.loadGridData(this.page.pageNumber).then(a => {
      this.getSelectedData();
    });
  }

  onNgxDataTableSelect({ selected }) {
    this.selected.splice(0, this.selected.length);
    this.selected.push(...selected);
    this.addToTotalSelect();
  }
  getSelectedData() {
    this.selected = [];
    this.rows.forEach(data => {

      this.selected.push(...this.totalSelected.filter(function (item) {
        return item.facilityCode === data.facilityCode
      }))

    })

  }
  get getSelected() {
    return this.totalSelected.length;
  }
  addToTotalSelect() {

    if (this.selected.length > 0) {
      this.rows.forEach(data => {

        this.totalSelected = this.totalSelected.filter(function (item) {
          return item.id !== data.id
        })

      })

      this.totalSelected.push(...this.selected);
    }
    else {
      this.rows.forEach(data => {
        this.totalSelected = this.totalSelected.filter(function (item) {
          return item.id !== data.id
        })

      })

    }
  }

  selectNgxDataRow(row, column, value) {
    console.log('selectCheck event', value);
  }

  getListCellText(value, col) {
    var column = col.cellEditorParams;
    console.log('data', value, column)
    let returnText = "";
    for (let i = 0; i < value.length; i++) {
      returnText = returnText + column.filter(a => a.value == value[i])[0].title + ':' + value[i] + ','
    }
    return returnText;//`Name ${row["name"]}, Gender is : ${row["company"]}`;
  }
  ngOnInit() {
    // this.loadInstance();
    this.assignTeacher(1, this.teacherPaginationConfig.itemsPerPage, this.teacherPaginationConfig.searchText);
    var facilityCache = this.localStorage.get(CacheConstant.FacilityGrid);
    if (facilityCache != null) {
      this.loadCacheData(facilityCache);
    }
    this.loadGridData(this.page.pageNumber)
  }

  loadCacheData(facilityCache) {
    
    let searchText = this.facilityFilterModel.searchText;
    this.facilityFilterModel = new FacilityFilter()
    this.facilityFilterModel.programPartner = [];
    this.facilityFilterModel.implementationPartner = [];

    Object.assign(this.facilityFilterModel, facilityCache.filter);
    Object.assign(this.page, facilityCache.page)
    this.facilityFilterModel.searchText = searchText;

    this.facilityFilter.loadData(this.facilityFilterModel);
  }

  loadGridData(pageNo) {

    return new Promise((resolve, reject) => {

      let facilityQueryModel: any = {
        pageNo: pageNo,
        pageSize: this.page.size,
        filter: this.facilityFilterModel
      }

      this.facilityService.getAllFilterData(facilityQueryModel).then(data => {
        this.columns = []
        this.rows = []

          this.columns = [
            //{ prop: 'facilityId', name: "Id" },
            {
              prop: 'name',
              name: 'Facility Name',
              width: 250,
              headerCheckboxable: "true",
              checkboxable: "true",
              frozenLeft: "true"
              ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl 
            }
            , { 'prop': 'facilityCode', 'name': 'Facility Id', frozenLeft: "true",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'fCamp', 'name': 'Camp', frozenLeft: "true",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'teacherName', 'name': 'Teacher Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'paraName', 'name': 'Para Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'blockName', 'name': 'Block Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'targetedPopulation', 'name': 'Targeted Polulation',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'type', 'name': 'Type',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'status', 'name': 'Status',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'donors', 'name': 'Donors',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'programmingPartnerName', 'name': 'Programming Partner Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'implemantationPartnerName', 'name': 'Implemantation Partner Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
            , { 'prop': 'remarks', 'name': 'Remarks',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }

          ];

          data.data.map(a => {
            var data = {};
            data['id'] = a.id;
            data['name'] = a.facilityName;
            data['facilityCode'] = a.facilityCode;
            data['fCamp'] = a.campName;
            data['teacherName'] = a.teacherName;
            data['paraName'] = a.paraName;
            data['blockName'] = a.blockName;
            data['targetedPopulation'] = TargetPopulation[a.targetedPopulation];
            data['type'] = FacilityType[a.facilityType];
            data['status'] = FacilityStatus[a.facilityStatus];


            data['donors'] = a.donors;
            data['programmingPartnerName'] = a.programmingPartnerName;
            data['implemantationPartnerName'] = a.implemantationPartnerName
            data['remarks'] = a.remarks
            this.rows.push(data);
          })

          this.page.totalElements = data.total;
          this.page.pageNumber = (pageNo - 1);
          this.pagedFacilities = data;
          this.setCacheData(pageNo);
          resolve(this.pagedFacilities);



      })
    });
  }
  onPageSizeChanged(pageSize: number) {
    this.page.size = Number(pageSize);
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }
  dataTypeSelect(data) {
    this.selectedDataType = data.name;
    this.selectedDataTypeValue = data.value;
  }


  get facilitySelected(): boolean {
    return this.totalSelected.length > 0;
  }
  get facilitySelectedSize(): number {
    return this.totalSelected.length;
  }

  editFacility() {
    if (this.facilitySelectedSize == 1) {
      this.router.navigate(['/unicef/facility/single-facility-edit/' + this.totalSelected[0].id]);
    }
  }


  assignTeacher(pageNo, pageSize, searchText) {
    this.facilityService.getTeachers(pageSize, pageNo, searchText).then(data => {
      this.teachers = data.data;
      this.teacherPaginationConfig.currentPage = pageNo;
      this.teacherPaginationConfig.totalItems = data.total;
    })
  }

  teacherRadioOnChange(teacher) {
    this.selectedTempTeacherId = teacher.id;
    this.selectedTempTeacherName = teacher.fullName;
  }

  selectTeacherClicked() {
 
    this.modalService.open<AssignTeacherComponent, TeacherViewModel>(AssignTeacherComponent , {})
     .then((result: TeacherViewModel) => {
       if (!result) {
         return;
       }
     
       this.selectedTeacherId = result.id;
       this.selectedTeacherName = result.fullName;

       if (this.selectedTeacherId > 0) {
         var ids = this.totalSelected.map(a => a.id);
         this.facilityService.assignTeacher({
           facilityIds: ids,
           teacherId: this.selectedTeacherId
         }).then(a => {
           this.toast.success(MessageConstant.SaveSuccessful);



           this.loadGridData(1).then(a => {
             this.totalSelected = []
             this.selected = []
           });
         })
       }

     })
   }


  pageChangedTeacher(event) {
    this.assignTeacher(event, this.teacherPaginationConfig.itemsPerPage, this.teacherPaginationConfig.searchText);
  }
  searchTeacher(event) {
    this.assignTeacher(event, this.teacherPaginationConfig.itemsPerPage, this.teacherPaginationConfig.searchText);
  }

  openFacilityFileBrowser() {
    this.facilityImportInput.nativeElement.click();
  }

  onImportFacilityDataSelected(files: FileList) {
    let formData = new FormData();
    formData.append('file', files.item(0));
    this.facilityService.importFacilities(formData)
      .then(result => {
        // if(result.rowErrors.length == 0){
        //   this.toast.success(`Total ${result.totalImported} facilities imported`);
        // }else{
        //   this.toast.error(result.rowErrors[0].errorMessage);
        // }
        
        this.modalService.open<IMportResultComponent, void>(IMportResultComponent, {
          importResult: result
        })
          .then(x => x)
          .catch(x => x);
        this.loadGridData(1).then(a => {
          this.totalSelected = []
          this.selected = []
        });
      });
    this.facilityImportInput.nativeElement.value = null;
  }

  downloadTemplate() {
    this.facilityService.downloadImportTemplate();
  }
  setCacheData(pageNo) {

    // return new Promise((resolve, reject) => {
    let pageInfo = { totalElements: 0, pageNumber: 1, size: 10 };
    pageInfo.pageNumber = pageNo;
    pageInfo.size = this.page.size;

    let facilityCacheData = {
      filter: this.facilityFilterModel,
      page: pageInfo
    }

    this.localStorage.set(CacheConstant.FacilityGrid, facilityCacheData);
    //   resolve()
    // })

  }

  filterComplete(data) {
    this.getFacilityFilterModel = data;
    let searchText = this.facilityFilterModel.searchText;
    this.facilityFilterModel = new FacilityFilter()
    this.facilityFilterModel.programPartner = [];
    this.facilityFilterModel.implementationPartner = [];

    Object.assign(this.facilityFilterModel, this.getFacilityFilterModel);
    this.facilityFilterModel.searchText = searchText;

    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }
  facilityFilterRemove(prop) {

    this.facilityFilter.removeFilter(prop)
    if (prop === "programPartner" || prop === "implementationPartner" || prop === "teachers") {
      this.facilityFilterModel[prop] = [];
    }
    else if (prop === "upazila" || prop === "union") {
      delete this.facilityFilterModel[prop + "Id"];
      delete this.facilityFilterModel[prop + "Name"];
    }
    else if (prop === "targetedPopulation" || prop === "facilityType" || prop === "facilityStatus") {
      delete this.facilityFilterModel[prop];
    }

    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }

  export() {
    this.facilityService.exportAll();
  }
}
