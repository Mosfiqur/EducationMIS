import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { InstanceService } from 'src/app/services/instance.service';
import { GridRowDef } from 'src/app/models/dynamicColumn/gridRowDef';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from 'src/app/models/beneficiary/beneficiaryViewModel';
import { GridColumnDef } from 'src/app/models/dynamicColumn/gridColumnDef';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IDatasource, IGetRowsParams } from 'ag-grid-community';
import { DataApprovalService } from 'src/app/services/approval.service';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { EntityType } from 'src/app/_enums/entityType';
import { AgCustomCheckboxRenderer } from 'src/app/components/agGrid/ag-customCheckboxRenderer.component';
import { NonEditableGridColumnDef } from 'src/app/models/dynamicColumn/nonEditableGridColumnDef';
import { AgCustomCheckboxEditor } from 'src/app/components/agGrid/ag-customCheckboxEditor.component';
import { AgCustomRadioButtonEditor } from 'src/app/components/agGrid/ag-customRadioButtonEditor.component';
import { AgCustomRadioButtonRenderer } from 'src/app/components/agGrid/ag-customRadioButtonRenderer.component';
import { BeneficiaryFilter, DateRange } from 'src/app/models/beneficiary/beneficiaryFilterModel';

import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { BeneficiaryApprovalGridQueryModel } from 'src/app/models/beneficiary/beneficiaryApprovalGridQueryModel';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { Convert } from 'src/app/utility/Convert';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { NgxGridColumnDef } from 'src/app/models/dynamicColumn/ngxGridColumnDef';
import { ColumnDataType } from 'src/app/_enums/column-data-type';

@Component({
  selector: 'app-approve',
  templateUrl: './approve.component.html',
  styleUrls: ['./approve.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class ApproveComponent implements OnInit {

  @ViewChild('filter') beneficiaryFilter: any;

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('listCellTpl') listTypeCellRenderer: any;
  @ViewChild('customCellTpl') customCellTpl: any;

  public allRecords = 2147483647;

  public lstInstance: InstanceViewModel[];
  public selectedInstance: string = "";
  public selectedInstanceId: number = 0;

  public pagedBeneficiaries: IPagedResponse<BeneficiaryViewModel>;
  public beneficiarySelectedRow: any[] = [];

  public beneficiaryFilterModel: BeneficiaryFilter;
  public getBeneficiaryFilterModel: BeneficiaryFilter;

  public searchTerms = new Subject<string>();

  public collectionStatusList: ISelectListItem[] = [];
  public collectionStatus: CollectionStatus = 2;

  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 0, size: 10 };

  constructor(private router: Router,
    private toast: ToastrService,
    private instanceService: InstanceService,
    private approvalService: DataApprovalService) {


    this.beneficiaryFilterModel = new BeneficiaryFilter()
    this.beneficiaryFilterModel.camps = [];
    this.beneficiaryFilterModel.facilities = [];
    this.beneficiaryFilterModel.dateOfBirth = new DateRange();
    this.beneficiaryFilterModel.enrolmentDate = new DateRange();

    this.searchTerms.pipe(
      // wait 400ms after each keystroke before considering the term
      debounceTime(400),
      // ignore new term if same as previous term
      distinctUntilChanged(),

    ).subscribe(value => {
      this.beneficiaryFilterModel.searchText = value;
      this.loadGridData(1).then(a => {
        this.totalSelected = []
        this.selected=[]
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
      'unapprove-beneficiary-data': row.type === 'collection' && row.isApprove === false && row.status === CollectionStatus.Collected,
      'unapprove-recollect-beneficiary-data': row.type === 'collection' && row.isApprove === false && row.status === CollectionStatus.Recollect,
      'approve-beneficiary-data': row.type === 'collection' && row.status === CollectionStatus.Approved,
      'recollect-beneficiary-data': row.type === 'collection' && row.isApprove === true && row.status === CollectionStatus.Recollect,
      'deactivate-beneficiary-data': row.type === 'deactive'
    }
  };

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
    this.loadInstance().then(a => {
      this.loadGridData(1)
    });
  }

  search(term: string): void {
    this.searchTerms.next(term);
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
      this.totalSelected = []
      this.selected=[]
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
      this.totalSelected = []
      this.selected=[]
    });
  }

  changeGridByCollectionStatus() {
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected=[]
    });
  }

  loadGridData(pageNo) {

    return new Promise((resolve, reject) => {

      let beneficiaryQueryModel: BeneficiaryApprovalGridQueryModel = {
        collectionStatus: this.collectionStatus > 0 ? this.collectionStatus : null,
        instanceId: this.selectedInstanceId,
        pageNo: pageNo,
        pageSize: this.page.size,
        filter: this.beneficiaryFilterModel
      }

      this.approvalService.getSubmittedBeneficiaries(beneficiaryQueryModel).then(data => {
        this.columns = []
        this.rows = []

        this.columns = [
          { prop: 'unhcrId'
          , name: "proGres ID"
          , headerCheckboxable: "true"
          , checkboxable: "true"
          , frozenLeft: "true" 
          ,width: 170
          ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl 
        },
          {
            prop: 'beneficiaryName',
            width: 200,
            name: "Beneficiary Name",
            frozenLeft: "true"
            ,cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl 
          }
          , { prop: "beneficiaryCamp", name: "Beneficiary Camp",cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }
          , { prop: "facilityName", name: 'Facility Name',cellTemplate:this.customCellTpl,headerTemplate:this.hdrTpl  }

        ];
        var a=data.data[0].collectedData;

        if (data.data[0].collectedData.length > 0) {
          data.data[0].collectedData[0].properties.map(a => {
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
        }
        data.data.map(d => {
          d["collectedData"].map(a => {
            var data = new GridRowDef(a.properties).row;
            data['id'] = a.entityId;
            data['unhcrId'] = a.unhcrId;
            data['beneficiaryName'] = a.beneficiaryName;
            data['beneficiaryCamp'] = a.beneficiaryCampName;
            data['facilityName'] = a.facilityName;
            data['isActive'] = a.isActive;
            data['isApprove'] = a.isApproved;
            data['type'] = 'collection';
            data['status'] = a.collectionStatus;
            this.rows.push(data);
          })
          d["deactivateData"].map(a => {
            var data = {};
            data['id'] = a.entityId;
            data['unhcrId'] = a.unhcrId;
            data['beneficiaryName'] = a.beneficiaryName;
            data['beneficiaryCamp'] = a.beneficiaryCampName;
            data['facilityName'] = a.facilityName;
            data['type'] = 'deactive'

            this.rows.push(data);
          })
        })




        this.page.totalElements = data.total;
        this.page.pageNumber = (pageNo - 1);

        this.pagedBeneficiaries = data;
        resolve(this.pagedBeneficiaries);

      })
    });
  }


  loadInstance() {
    return new Promise((resolve, reject) => {
      var data = this.instanceService.getRunningInstance(EntityType.Beneficiary, this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;
          this.lstInstance = data.data;
          var maxid = 0;
          this.lstInstance.map(function (obj) {
            if (obj.id > maxid) maxid = obj.id;
          });
          this.lstInstance.filter(a => a.id == maxid).map(a => {
            this.selectedInstance = a.title;
            this.selectedInstanceId = a.id;
          });
          resolve(true)
        })
    });
  }

  onPageSizeChanged(pageSize: number) {
    this.page.size = Number(pageSize);
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected=[]
    });
  }

  instance_Clicked(instance) {
    this.selectedInstance = instance.title;
    this.selectedInstanceId = instance.id;
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected=[]
    });
  }


  get beneficiarySelected(): boolean {
    return this.totalSelected.length > 0;
  }
  get showContextMenu(): boolean {

    return this.totalSelected.length > 0 && this.collectionStatus != 0;
  }
  get beneficiarySelectedSize(): number {
    return this.totalSelected.length;
  }

  approveBeneficiary() {
    if (this.beneficiarySelected) {

      var approvalIds: number[] = [];
      var deactivateIds: number[] = [];

      this.totalSelected.map(a => {
        if (a.type == 'collection') {
          approvalIds.push(a.id);
        }
        else if (a.type == 'deactive') {
          deactivateIds.push(a.id);
        }
      });
      var beneficiaryForApproval = {
        "beneficiaryIds": approvalIds,
        "instanceId": this.selectedInstanceId
      }
      this.approvalService.approveBeneficiaries(beneficiaryForApproval).then(a => {
        this.toast.success(MessageConstant.ApproveSuccessfull);
        this.loadGridData(1).then(a => {
          this.totalSelected = []
          this.selected=[]
        });
      })
      if (deactivateIds.length > 0) {
        var deactivateBeneficiary = {
          "beneficiaryIds": deactivateIds,
          "instanceId": this.selectedInstanceId
        }
        this.approvalService.approveInactiveBeneficiaries(deactivateBeneficiary).then(a => {
          this.toast.success(MessageConstant.InactiveSuccessfull);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected=[]
          });
        })
      }

    }
  }
  recollectBeneficiary() {
    if (this.beneficiarySelected) {
      var ids = this.totalSelected.map(a => a.id);
      var beneficiaryForRecollect = {
        "beneficiaryIds": ids,
        "instanceId": this.selectedInstanceId
      }
      this.approvalService.recollectBeneficiaries(beneficiaryForRecollect).then(a => {
        this.toast.success(MessageConstant.RecollectData);
        this.loadGridData(1).then(a => {
          this.totalSelected = []
          this.selected=[]
        });
      })
    }
  }
}
