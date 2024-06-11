import { Component, OnInit, ViewChild, ViewEncapsulation } from "@angular/core";
import { Router } from "@angular/router";
import { ToastrService } from "ngx-toastr";
import { InstanceService } from "src/app/services/instance.service";
import { GridRowDef } from "src/app/models/dynamicColumn/gridRowDef";
import { IPagedResponse } from "src/app/models/responseModels/pagedResponseModel";
import { BeneficiaryViewModel } from "src/app/models/beneficiary/beneficiaryViewModel";
import { GridColumnDef } from "src/app/models/dynamicColumn/gridColumnDef";
import { InstanceViewModel } from "src/app/models/instance/instanceViewModel";
import { IDatasource, IGetRowsParams } from "ag-grid-community";
import { DataApprovalService } from "src/app/services/approval.service";
import { MessageConstant } from "src/app/utility/MessageConstant";
import { FacilityViewModel } from "src/app/models/facility/facilityViewModel";
import { EntityType } from "src/app/_enums/entityType";
import { AgCustomCheckboxRenderer } from "src/app/components/agGrid/ag-customCheckboxRenderer.component";
import { AgCustomCheckboxEditor } from "src/app/components/agGrid/ag-customCheckboxEditor.component";
import { NonEditableGridColumnDef } from "src/app/models/dynamicColumn/nonEditableGridColumnDef";
import { AgCustomRadioButtonEditor } from 'src/app/components/agGrid/ag-customRadioButtonEditor.component';
import { AgCustomRadioButtonRenderer } from 'src/app/components/agGrid/ag-customRadioButtonRenderer.component';
import { FacilityFilter } from 'src/app/models/facility/facilityFilterModel';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { Convert } from 'src/app/utility/Convert';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { NgxGridColumnDef } from 'src/app/models/dynamicColumn/ngxGridColumnDef';
import { ColumnDataType } from 'src/app/_enums/column-data-type';

@Component({
  selector: "app-approve",
  templateUrl: "./approve.component.html",
  styleUrls: ["./approve.component.scss"],
  encapsulation: ViewEncapsulation.None,
})
export class ApproveComponent implements OnInit {

  @ViewChild('filter') facilityFilter: any;

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('listCellTpl') listTypeCellRenderer: any;
  @ViewChild('customCellTpl') customCellTpl: any;

  public facilityFilterModel: FacilityFilter;
  public getFacilityFilterModel: FacilityFilter;


  public lstInstance: InstanceViewModel[];
  public selectedInstance: string = "";
  public selectedInstanceId: number = 0;

  public pagedBeneficiaries: IPagedResponse<BeneficiaryViewModel>;
  public pagedFacilities: IPagedResponse<FacilityViewModel>;

  public facilitySelectedRow: any[] = [];


  public searchTerms = new Subject<string>();

  public collectionStatusList: ISelectListItem[] = [];
  public collectionStatus: CollectionStatus = 2;
  private allRecords = 2147483647;

  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;


  public page = { totalElements: 0, pageNumber: 0, size: 10 };


  constructor(
    private router: Router,
    private toast: ToastrService,
    private instanceService: InstanceService,
    private approvalService: DataApprovalService
  ) {

    this.facilityFilterModel = new FacilityFilter();
    this.facilityFilterModel.programPartner = [];
    this.facilityFilterModel.implementationPartner = [];
    this.facilityFilterModel.teachers = [];

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

    this.loadCollectionStatus();

    this.rows = [];
    this.columns = [];
  }

  loadCollectionStatus() {
    this.collectionStatusList = Convert.enumToSelectList(CollectionStatus);
    this.collectionStatusList = this.collectionStatusList.filter(a => a.id != 1);
    this.collectionStatusList.splice(0, 0, { id: 0, text: "All" });
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
      'approve-facility-data': row.status === CollectionStatus.Approved,
      'recollect-facility-data': row.status === CollectionStatus.Recollect,
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
    this.loadInstance().then(a => {
      this.loadGridData(1)
    });
  }
  changeGridByCollectionStatus() {

    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }
  loadGridData(pageNo) {
    return new Promise((resolve, reject) => {

      let facilityQueryModel: any = {
        collectionStatus: this.collectionStatus > 0 ? this.collectionStatus : null,
        instanceId: this.selectedInstanceId,
        pageNo: pageNo,
        pageSize: this.page.size,
        filter: this.facilityFilterModel
      }

      this.approvalService
        .getSubmittedFacilities(
          facilityQueryModel
        )
        .then((data) => {

          this.columns = []
          this.rows = []

          if (data.data.length > 0) {

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
              , { 'prop': 'status', 'name': 'Status',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }


            ];
            data.data[0].properties.map(a => {
              var cData = new NgxGridColumnDef(a);
              cData["headerTemplate"] = this.hdrTpl;
              if (a.dataType == ColumnDataType.List) {
                cData["cellTemplate"] = this.listTypeCellRenderer
              }
              else{
                cData["cellTemplate"] = this.customCellTpl
              }

              this.columns.push(cData);

            });

            data.data.map((a) => {
              var data = new GridRowDef(a.properties).row;
              data["id"] = a.id;
              data["name"] = a.facilityName;
              data["facilityCode"] = a.facilityCode;
              data["camp"] = a.campName;
              data['status'] = a.collectionStatus;
              this.rows.push(data);
            });
            this.page.totalElements = data.total;
            this.page.pageNumber = (pageNo - 1);

            this.pagedFacilities = data;
            // console.log('col',this.columnDefs,this.rowData);
            resolve(this.pagedFacilities);
          }
          resolve(this.pagedFacilities);
        });
    });
  }
  onPageSizeChanged(pageSize: number) {
    this.page.size = Number(pageSize);
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }
  loadInstance() {
    return new Promise((resolve, reject) => {
      var data = this.instanceService
        .getRunningInstance(EntityType.Facility, this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;
          this.lstInstance = data.data;
          var maxid = 0;
          this.lstInstance.map(function (obj) {
            if (obj.id > maxid) maxid = obj.id;
          });
          this.lstInstance
            .filter((a) => a.id == maxid)
            .map((a) => {
              this.selectedInstance = a.title;
              this.selectedInstanceId = a.id;
            });
          resolve(true);
        });
    });
  }

  instance_Clicked(instance) {
    this.selectedInstance = instance.title;
    this.selectedInstanceId = instance.id;
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }

  get facilitySelected(): boolean {
    return this.totalSelected.length > 0;
  }
  get showContextMenu(): boolean {
    return this.totalSelected.length > 0 && this.collectionStatus != 0;
  }

  get facilitySelectedSize(): number {
    return this.totalSelected.length;
  }

  approveFacility() {
    if (this.totalSelected) {
      var ids = this.totalSelected.map((a) => a.id);
      var facilityForApproval = {
        facilityIds: ids,
        instanceId: this.selectedInstanceId,
      };
      this.approvalService
        .approveFacilities(facilityForApproval)
        .then((a) => {
          this.toast.success(MessageConstant.ApproveSuccessfull);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });

        });
    }
  }
  recollectFacility() {
    if (this.totalSelected) {
      var ids = this.totalSelected.map((a) => a.id);
      var facilityForRecollect = {
        facilityIds: ids,
        instanceId: this.selectedInstanceId,
      };
      this.approvalService
        .recollectFacility(facilityForRecollect)
        .then((a) => {
          this.toast.success(MessageConstant.RecollectData);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });

        });
    }
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

}
