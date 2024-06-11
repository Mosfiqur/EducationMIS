import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
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
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { Gender } from 'src/app/_enums/gender';
import { BeneficiaryFilter, DateRange } from 'src/app/models/beneficiary/beneficiaryFilterModel';
import { analyzeFileForInjectables } from '@angular/compiler';
import { IMportResultComponent } from 'src/app/shared/components/import-result/import-result.component';
import { ModalService } from 'src/app/services/modal.service';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ColumnMode, SelectionType } from 'src/app/lib/ngx-datatable';
import * as moment from 'moment';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { CacheConstant } from 'src/app/utility/CacheConstant';



@Component({
  selector: 'app-home',
  templateUrl: './listOfBeneficiaries.component.html',
  styleUrls: ['./listOfBeneficiaries.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ListOfBeneficiariesComponent implements OnInit {
  @ViewChild('deleteBeneficiaryModalClose') deleteBeneficiaryModalClose;
  @ViewChild('saveDynamicColumnClose') saveDynamicColumnClose;
  @ViewChild('beneficiaryImportFileInput') beneficiaryImportFileInput: any;
  @ViewChild('filter') beneficiaryFilter: any;

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('customCellTpl') customCellTpl: any;

  public rowClassRules;
  public beneficiaryFilterModel: BeneficiaryFilter;
  public getBeneficiaryFilterModel: BeneficiaryFilter;

  public gridView: GridView[] = [];
  public selectedView: string = "";
  public selectedViewId: number;

  public lstInstance: InstanceViewModel[];
  public selectedInstance: string = "";
  public selectedInstanceId: number;

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
  ];

  public selectedDataType: string = "";
  public selectedDataTypeValue: number;

  public frameworkComponents: any;
  public pagedBeneficiaries: IPagedResponse<BeneficiaryViewModel>;
  private allRecords = 2147483647;

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
    private modalService: ModalService,
    private localStorage: LocalStorageService
  ) {


    this.beneficiaryFilterModel = new BeneficiaryFilter()
    this.beneficiaryFilterModel.camps = [];
    this.beneficiaryFilterModel.facilities = [];
    this.beneficiaryFilterModel.dateOfBirth = new DateRange();
    this.beneficiaryFilterModel.enrolmentDate = new DateRange();

    // console.log('column data type',Object.keys(ColumnDataType).filter(key => !isNaN(Number(ColumnDataType[key]))));
    this.entityDynamicColumn = new DynamicColumnSaveViewModel();

    this.searchTerms.pipe(
      // wait 400ms after each keystroke before considering the term
      debounceTime(400),
      // ignore new term if same as previous term
      distinctUntilChanged(),

    ).subscribe(value => {
      this.beneficiaryFilterModel.searchText = value;
      this.loadGridData(1).then(a => {
        this.totalSelected = [];
        this.selected = []
      });
    });



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
      'inactive-not-approve-beneficiary-data': row.isActive === false && row.isApprove === false,
      'inactive-approve-beneficiary-data': row.isActive === false && row.isApprove === true,
      'not-approved-beneficiary-data': row.isActive === true && row.isApprove === false,
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
        return item.id === data.id
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

    let returnText = "";
    for (let i = 0; i < value.length; i++) {
      returnText = returnText + column.filter(a => a.value == value[i])[0].title + ':' + value[i] + ','
    }
    return returnText;//`Name ${row["name"]}, Gender is : ${row["company"]}`;
  }


  ngOnInit() {
    // this.loadGridView();
    // this.loadInstance();
    var beneficiaryCache = this.localStorage.get(CacheConstant.BeneficiaryGrid);
    if (beneficiaryCache != null) {
      this.loadCacheData(beneficiaryCache);
    }

    this.loadGridData(this.page.pageNumber)

  }
  loadCacheData(beneficiaryCache) {
    let searchText = this.beneficiaryFilterModel.searchText;
    this.beneficiaryFilterModel = new BeneficiaryFilter()
    this.beneficiaryFilterModel.camps = [];
    this.beneficiaryFilterModel.facilities = [];
    this.beneficiaryFilterModel.dateOfBirth = new DateRange();
    this.beneficiaryFilterModel.enrolmentDate = new DateRange();
    Object.assign(this.beneficiaryFilterModel, beneficiaryCache.filter);
    Object.assign(this.page, beneficiaryCache.page)
    this.beneficiaryFilterModel.searchText = searchText;

    this.beneficiaryFilter.loadData(this.beneficiaryFilterModel);
  }

  loadGridData(pageNo) {

    return new Promise((resolve, reject) => {
      let beneficiaryQueryModel: any = {
        pageNo: pageNo,
        pageSize: this.page.size,
        filter: this.beneficiaryFilterModel
      }

      this.beneficiaryService.getAll(beneficiaryQueryModel).then(data => {
        this.rows = [];
        this.columns = [];

        this.columns = [
          {
            prop: 'unhcrId'
            , name: "proGres ID"
            , width: 170
            , headerCheckboxable: "true"
            , checkboxable: "true"
            , frozenLeft: "true"
            ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl 
          },
          {
            prop: 'beneficiaryName',
            width: 200,
            name: "Beneficiary Name",
            frozenLeft: "true"
            ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl 
          }
          , { prop: "fatherName", name: "Father Name" ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl }
          , { prop: "motherName", name: 'Mother Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "fcnId", name: "FCN Id" ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl }
          , { prop: "dateOfBirth", name: 'Date of Birth',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "sex", name: "Sex",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "disabled", name: 'Disabled',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "levelOfStudy", name: "Level of study",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "enrollmentDate", name: 'Enrollment Date',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "beneficiaryCamp", name: "Beneficiary Camp",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "facilityName", name: 'Facility Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "blockName", name: "Block name",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "subBlockName", name: 'Sub Block name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
        ];

        data.data.map(a => {
          // var data = new GridRowDef(a.properties).row;
          var data = {};
          data['id'] = a.entityId;
          data['unhcrId'] = a.unhcrId;
          data['beneficiaryName'] = a.beneficiaryName;
          data['beneficiaryCamp'] = a.beneficiaryCampName;
          data['facilityName'] = a.facilityName;

          data['fatherName'] = a.fatherName;
          data['motherName'] = a.motherName;
          data['fcnId'] = a.fcnId;
          data['dateOfBirth'] = moment(a.dateOfBirth).format('DD-MMM-YYYY');
          data['sex'] = Gender[a.sex];
          data['disabled'] = a.disabled ? 'Yes' : 'No';
          data['levelOfStudy'] = LevelOfStudy[a.levelOfStudy];
          data['enrollmentDate'] = moment(a.enrollmentDate).format('DD-MMM-YYYY');
          data['blockName'] = a.blockName;
          data['subBlockName'] = a.subBlockName;
          data['isActive'] = a.isActive;
          data['isApprove'] = a.isApproved;

          this.rows.push(data);
        })

        this.page.totalElements = data.total;
        this.setCacheData(pageNo);
        this.page.pageNumber = (pageNo - 1);

        this.pagedBeneficiaries = data;
        // console.log('col',this.columnDefs,this.rowData);
        resolve(this.pagedBeneficiaries);

      })
    });
  }

  onPageSizeChanged(pageSize: number) {
    this.page.size = Number(pageSize);
    this.page.pageNumber = 1;

    this.loadGridData(this.page.pageNumber).then(a => {
      this.totalSelected = [];
      this.selected = []
    });

  }

  dataTypeSelect(data) {
    this.selectedDataType = data.name;
    this.selectedDataTypeValue = data.value;
  }

  dynamicCellOldValue(params) {
    console.log('old', params);
    this.cellOldValue = params.value;
  }
  dynamicCellUpdate(params) {
    console.log('cell data', params);
    if (params.value == null) {
      return;
    }

    var newValue = params.value;

    if (this.cellOldValue == newValue) {
      return;
    }

    var val = [];
    if (typeof params.value === "string") {
      val.push(params.value);
    }
    else {
      val = params.value;
    }
    var dynamicCell = {
      "beneficiaryId": params.data.id,
      "instanceId": this.selectedInstanceId,
      "dynamicCells": [
        {
          "value": val,
          "entityDynamicColumnId": params.colDef.columnId
        }
      ]
    }
    this.beneficiaryService.beneficiaryCellSave(dynamicCell).then(a => {
      this.toast.success(MessageConstant.UpdateSuccessful);
      this.loadGridData(1).then(a => {
        this.totalSelected = [];
        this.selected = []
      });
    })

  }


  get beneficiarySelected(): boolean {

    return this.totalSelected.length > 0;
  }
  get beneficiarySelectedSize(): number {
    return this.totalSelected.length;
  }
  editBeneficiary() {
    if (this.beneficiarySelectedSize == 1) {
      this.router.navigate(['/unicef/beneficiary/edit/' + this.totalSelected[0].id]);
    }
  }
  async activeBeneficiary() {
    if (await this.modalService.confirm("Confirm Action", "", "Selected beneficairy will active.", "Confirm")) {
      this.beneficiaryService.active(this.totalSelected[0].id).then(a => {
        this.toast.success(MessageConstant.ActiveSuccess);
        this.loadGridData(1).then(a => {
          this.totalSelected = [];
          this.selected = []
        });
      })
    }

  }
  async deleteBeneficiarys() {

    if (this.beneficiarySelected) {
      var ids = this.totalSelected.map(a => a.id);
      //  if (await this.modalService.confirm()) {
      this.beneficiaryService.delete(ids).then(a => {
        this.deleteBeneficiaryModalClose.nativeElement.click();
        this.toast.success(MessageConstant.DeleteSuccess);
        this.loadGridData(1).then(a => {
          this.totalSelected = [];
          this.selected = []
        });
      })
      //  }
    }
  }

  openBeneficiaryImportFile() {
    var fileCtrl = document.getElementById("ctrlImportBeneficiary");
    if (fileCtrl) {
      fileCtrl.click();
    }
  }

  onBeneficiaryImportFileSelected(files: FileList) {
    var fileToUpload = files.item(0);
    if (fileToUpload) {
      let formData = new FormData();
      formData.append("file", fileToUpload);
      this.beneficiaryService.importBeneficiaries(formData).then(result => {
        this.loadGridData(1).then(a => {
          this.totalSelected = [];
          this.selected = []
        });
        return this.modalService.open<IMportResultComponent, void>(IMportResultComponent, { importResult: result });
      }).catch(x => x);
      this.beneficiaryImportFileInput.nativeElement.value = '';
    }
  }

  setCacheData(pageNo) {

    // return new Promise((resolve, reject) => {
    let pageInfo = { totalElements: 0, pageNumber: 1, size: 10 };
    pageInfo.pageNumber = pageNo;
    pageInfo.size = this.page.size;

    let beneficiaryCacheData = {
      filter: this.beneficiaryFilterModel,
      page: pageInfo
    }
    this.localStorage.set(CacheConstant.BeneficiaryGrid, beneficiaryCacheData);
    //   resolve()
    // })

  }

  filterComplete(data) {
    this.getBeneficiaryFilterModel = data;
    let searchText = this.beneficiaryFilterModel.searchText;
    this.beneficiaryFilterModel = new BeneficiaryFilter()
    this.beneficiaryFilterModel.camps = [];
    this.beneficiaryFilterModel.facilities = [];
    this.beneficiaryFilterModel.dateOfBirth = new DateRange();
    this.beneficiaryFilterModel.enrolmentDate = new DateRange();
    Object.assign(this.beneficiaryFilterModel, this.getBeneficiaryFilterModel);
    this.beneficiaryFilterModel.searchText = searchText;

    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = []
    });


  }

  beneficiaryFilterRemove(prop) {
    this.beneficiaryFilter.removeFilter(prop)
    if (prop === "camps" || prop === "facilities") {
      this.beneficiaryFilterModel[prop] = [];
    }
    else if (prop === "sex" || prop === "disable" || prop === "levelOfStudy") {
      delete this.beneficiaryFilterModel[prop];
    }
    else if (prop === "dateOfBirth" || prop === "enrolmentDate") {
      this.beneficiaryFilterModel[prop] = new DateRange();
    }

    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = []
    });

  }

  downloadImportTemplate() {
    this.beneficiaryService.downloadBeneficiaryImportTemplate()
  }

  export() {
    this.beneficiaryService.exportAllBeneficiaries();
  }

}
