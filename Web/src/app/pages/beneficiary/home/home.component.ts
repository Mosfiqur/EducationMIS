import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { EntityType } from 'src/app/_enums/entityType';
import { GridView } from 'src/app/models/gridView/gridView';



import { IDatasource, IGetRowsParams } from 'ag-grid-community';
import { BeneficiaryService } from 'src/app/services/beneficiary.service';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from 'src/app/models/beneficiary/beneficiaryViewModel';
import { GridColumnDef } from 'src/app/models/dynamicColumn/gridColumnDef';
import { GridRowDef } from 'src/app/models/dynamicColumn/gridRowDef';
import { GridviewService } from 'src/app/services/gridView.service';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { Router } from '@angular/router';

import { IScheduleInstance, ScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { ModalService } from 'src/app/services/modal.service';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { BeneficiaryFilter, DateRange } from 'src/app/models/beneficiary/beneficiaryFilterModel';
import { BeneficiaryGridQueryModel } from 'src/app/models/beneficiary/beneficiaryGridQueryModel';
import { IMportResultComponent } from 'src/app/shared/components/import-result/import-result.component';
import { InstanceService } from 'src/app/services/instance.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { NgxGridRowDef } from 'src/app/models/dynamicColumn/ngxGridRowDef';
import { NgxGridColumnDef } from 'src/app/models/dynamicColumn/ngxGridColumnDef';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { CacheConstant } from 'src/app/utility/CacheConstant';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { Convert } from 'src/app/utility/Convert';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { Globals } from 'src/app/globals';

import { DataApprovalService } from 'src/app/services/approval.service';
import { BeneficiaryActionButton } from './action-button';
import { Utility } from 'src/app/utility/Utility';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { IBeneficiaryDynamicCell, IBeneficiaryDynamicCellInsertModel } from 'src/app/models/beneficiary/beneficiary-dynamic-cell.model';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { InstanceStatus } from 'src/app/_enums/instance-status';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit {
  @ViewChild('deleteBeneficiaryModalClose') deleteBeneficiaryModalClose;

  @ViewChild('beneficiaryImportFileInput') beneficiaryImportFileInput: any;
  @ViewChild('beneficiaryVersionDataImportFileInput') beneficiaryVersionDataImportFileInput: any;
  @ViewChild('versionDataImportModalClose') versionDataImportModalClose: any;
  @ViewChild('filter') beneficiaryFilter: any;


  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('hdrTplWithContextMenu') hdrTplWithContextMenu: any;
  @ViewChild('listCellTpl') listTypeCellRenderer: any;
  @ViewChild('customCellTpl') customCellTpl: any;
  @ViewChild('cellView') cellView: any;

  public collectionStatusList: ISelectListItem[] = [];
  public frameworkComponents: any;

  @ViewChild('multiInstanceSelector') multiInstanceSelector: any;


  public allRecords: number;
  public beneficiaryFilterModel: BeneficiaryFilter;
  public getBeneficiaryFilterModel: BeneficiaryFilter;

  public selectedView: string = "";
  public gridView: GridView[] = [];
  public selectedViewId: number;
  //InstanceForImportVersionData
  public lstInstanceForImportVersionData: InstanceViewModel[];
  public selectInstanceForImportVersionData: string = '';
  public selectInstanceIdForImportVersionData: number;

  public lstInstance: InstanceViewModel[];

  public currentInstance: IScheduleInstance;
  public currentInstanceForVersionData: IScheduleInstance;

  public lstSelectedInstance: InstanceViewModel[] = [];
  public lstSelectedInstanceForExport: InstanceViewModel[] = [];

  public selectedInstanceId: number;
  public loadInstanceSelectorModal: boolean = false;
  //instanceStatusEnum: any;

  public beneficiarySelectedRow: any[] = [];
  public cellOldValue: any;

  public pagedBeneficiaries: IPagedResponse<BeneficiaryViewModel>;

  public searchTerms = new Subject<string>();

  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 1, size: 10 };
  public collectionStatus: CollectionStatus = 0;

  public dynamicColumns: IDynamicColumn[] = [];

  constructor(
    private router: Router,
    private toast: ToastrService,
    private beneficiaryService: BeneficiaryService,
    private gridViewService: GridviewService,
    private modalService: ModalService,
    private instanceService: InstanceService,
    private localStorage: LocalStorageService,
    private globals: Globals,
    private approvalService: DataApprovalService,
    private dynamicColumnService: DynamicColumnService
  ) {
    this.allRecords = this.globals.maxPageSize;
    this.currentInstance = new ScheduleInstance();
    this.beneficiaryFilterModel = new BeneficiaryFilter()
    this.beneficiaryFilterModel.camps = [];
    this.beneficiaryFilterModel.facilities = [];
    this.beneficiaryFilterModel.dateOfBirth = new DateRange();
    this.beneficiaryFilterModel.enrolmentDate = new DateRange();

    this.loadCollectionStatus();
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
    this.rows = [];
    this.columns = [];
  }

  get beneficiaries(): BeneficiaryViewModel[] {
    return this.pagedBeneficiaries ? this.pagedBeneficiaries.data : [];
  }
  get isEditButtonVisible(): boolean {
    if (this.beneficiarySelectedSize != 1)
      return false;
   // let id = this.totalSelected.map(x => x.id)[0];
    // let status =
    //   this.beneficiaries.find(x => x.entityId == id).collectionStatus;
    let status= this.totalSelected[0].collectionStatus;
    return this.visibleActionButtons(status)
      .indexOf(BeneficiaryActionButton.Edit) != -1;
  }
  get isDeleteButtonVisible(): boolean {
    return this.isActionButtonVisible(BeneficiaryActionButton.Delete);
  }

  get isApproveButtonVisible(): boolean {
    return this.isActionButtonVisible(BeneficiaryActionButton.Approve);
  }

  get isRecollectButtonVisible(): boolean {
    return this.isActionButtonVisible(BeneficiaryActionButton.Recollect);
  }
  get isActiveButtonVisible(): boolean {
    return this.isActionButtonVisible(BeneficiaryActionButton.Active);
  }
  get isInactiveApproveButtonVisible(): boolean {
    return this.isActionButtonVisible(BeneficiaryActionButton.InactiveApprove);
  }

  search(term: string): void {

    this.searchTerms.next(term);
  }

  rowIdentity = (row: any) => {
    return row.id;
  }
  loadCollectionStatus() {
    this.collectionStatusList = [
      {
        id: 0,
        text: "All"
      },
      {
        id: CollectionStatus.NotCollected,
        text: "Not Collected"
      },
      {
        id: CollectionStatus.Collected,
        text: "Collected"
      },
      {
        id: CollectionStatus.Approved,
        text: "Approved"
      },
      {
        id: CollectionStatus.Recollect,
        text: "Recollecting"
      },
      {
        id: CollectionStatus.Deleted,
        text: "Deleted"
      },
      {
        id: CollectionStatus.Requested_Inactive,
        text: "Requested Inactive"
      },
      {
        id: CollectionStatus.Inactivated,
        text: "Inactivated"
      }] as ISelectListItem[];
  }
  changeGridByCollectionStatus() {

    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }
  onActivate(event) {
    if (event.type == 'click') {
      console.log(event.row);
    }
  }

  getRowClass = (row) => {
    return {
      'not-collected-row': row.collectionStatus === CollectionStatus.NotCollected,
      'collected-row': row.collectionStatus === CollectionStatus.Collected,
      'recollect-row': row.collectionStatus === CollectionStatus.Recollect,
      'deleted-row': row.collectionStatus === CollectionStatus.Deleted,
      'requested-inactive-row': row.collectionStatus === CollectionStatus.Requested_Inactive,
      'inactivated-row': row.collectionStatus === CollectionStatus.Inactivated,

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

  getListCellText(val) {

    let returnText = "";
    var col = this.dynamicColumns.find(x => x.id == val.entityDynamicColumnId);
    if (col.columnDataType == ColumnDataType.List) {
      var value = val.values;
      var column = col.listItems;
      for (let i = 0; i < value.length; i++) {
        if (value[i] != null) {
          returnText = returnText + column.filter(a => a.value == value[i])[0].title + ':' + value[i] + ','
        }
      }
    }
    else {
      if (val.values != null)
        returnText = val.values.length > 0 ? val.values[0] : "";
    }
    return returnText;//`Name ${row["name"]}, Gender is : ${row["company"]}`;
  }

  ngOnInit() {
    // this.loadGridView();
    // this.loadInstance();
     this.loadInstanceForVersionData();


    this.loadInstance().then(instanceData => {
      this.loadDynamicColumns().then(() => {
        this.loadGridView().then(gridData => {
          var beneficiaryCache = this.localStorage.get(CacheConstant.BeneficiaryVersionDataGrid);
          if (beneficiaryCache != null) {
            this.loadCacheData(beneficiaryCache);
          }
          this.loadGridData(this.page.pageNumber);

        })
      });
    });

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

    if (beneficiaryCache.viewId != null) {
      this.selectedViewId = beneficiaryCache.viewId;
      this.gridView.filter(a => a.id == beneficiaryCache.viewId).map(a => {
        this.selectedView = a.name;

      });
    }
    if (beneficiaryCache.instanceId != null) {
      this.selectedInstanceId = beneficiaryCache.instanceId;
      this.lstInstance.filter(a => a.id == beneficiaryCache.instanceId).map(a => {
        this.lstSelectedInstance = [];        
        this.lstSelectedInstance.push(a);
        this.currentInstance = a as unknown as IScheduleInstance;
      });

    }
    this.beneficiaryFilter.loadData(this.beneficiaryFilterModel);
  }

  loadGridData(pageNo) {


    return new Promise((resolve) => {
      if (this.selectedInstanceId == null) {
        resolve(false);
        return
      }
      let beneficiaryQueryModel: BeneficiaryGridQueryModel = {
        instanceId: this.selectedInstanceId,
        viewId: this.selectedViewId,
        collectionStatus: this.collectionStatus,
        pageNo: pageNo,
        pageSize: this.page.size,
        filter: this.beneficiaryFilterModel
      }
      //this.beneficiaryService.getAllByViewId(this.selectedViewId, this.selectedInstanceId, this.paginationPageSize, pageNo).then(data => {
      var getDataFromApi = this.selectedViewId > 0 ?
        this.beneficiaryService.getAllByViewId(beneficiaryQueryModel)
        : this.beneficiaryService.getAllByInstanceId(beneficiaryQueryModel);


      getDataFromApi.then(data => {
        this.columns = []
        this.rows = []
        this.page.totalElements = 0;
        this.page.pageNumber = (pageNo - 1);

        this.columns = [
          {
            prop: 'checkboxColumn'
            , name: ""
            , width: 10
            , headerCheckboxable: "true"
            , checkboxable: "true"

          }
        ];


        if (data.data.length > 0) {

          data.data[0].properties.map(a => {
            var cData = new NgxGridColumnDef(a);
            cData["headerTemplate"] = this.hdrTplWithContextMenu;
            if (a.isFixed) {
              if (a.dataType == ColumnDataType.List) {
                cData["cellTemplate"] = this.listTypeCellRenderer;
              }
              else {
                cData["cellTemplate"] = this.customCellTpl;
              }

            }
            else {
              cData["cellTemplate"] = this.cellView;
            }
            this.columns.push(cData);

          });

          data.data.forEach(beneficiaryVM => {
            let row = {
              id: beneficiaryVM.entityId,
              collectionStatus: beneficiaryVM.collectionStatus
            };
            let props = beneficiaryVM.properties.map(prop => {
              return {
                entityDynamicColumnId: prop.entityColumnId,
                value: prop.values,
                values: prop.values,
                columnName: prop.columnName,
                dataType: prop.dataType,
                beneficiaryId: beneficiaryVM.entityId,
                listType: {
                  listItems: prop.listItem,
                  name: ""
                }

              } as IBeneficiaryDynamicCell
            });


            props.forEach(cell => {
              let fieldName = "field" + cell.entityDynamicColumnId.toString();
              row[fieldName] = cell
            });
            this.rows.push(row);
          });

          this.page.totalElements = data.total;

          this.page.pageNumber = (pageNo - 1);

        }
        this.pagedBeneficiaries = data;
        this.setCacheData(pageNo);
        resolve(true);
      });
    });
  }

  loadGridView() {
    return new Promise((resolve, reject) => {
      var data = this.gridViewService.getBeneficiaryView(this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;
          this.gridView = data.data;
          //var maxid = 0;
          // this.gridView.map(function (obj) {
          //   if (obj.id > maxid) maxid = obj.id;
          // });
          // this.gridView.filter(a => a.id == maxid).map(a => {
          //   this.selectedView = a.name;
          //   this.selectedViewId = a.id;
          // });
          this.selectedView = "Complete View";
          this.selectedViewId = 0;
          resolve(true);
        }, () => {
          this.selectedView = "Complete View";
          this.selectedViewId = 0
          resolve(false);
        })
    })
  }

  loadInstance() {
    return new Promise((resolve, reject) => {
      var data = this.instanceService.getNotPendingInstances(EntityType.Beneficiary, this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;

        //  this.lstInstanceForImportVersionData = data.data;

          this.lstInstance = data.data;
          var maxid = 0;
          this.lstInstance.map(function (obj) {
            if (obj.id > maxid) maxid = obj.id;
          });
          this.lstInstance.filter(a => a.id == maxid).map(a => {

            Object.assign(this.currentInstance, a);
            this.lstSelectedInstance.push(a);
            this.selectedInstanceId = a.id;
            this.loadInstanceSelectorModal = true;
          });
          resolve(true)
        })
    });
  }

  loadInstanceForVersionData() {
    return new Promise((resolve, reject) => {
      var data = this.instanceService.getRunningInstance(EntityType.Beneficiary, this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;
          this.lstInstanceForImportVersionData = data.data;

          resolve(true)
        })
    });
  }

  selectView_Clicked(grid) {

    this.selectedView = grid.name;
    this.selectedViewId = grid.id;
    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = []
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

  dynamicCellOldValue(params) {
    this.cellOldValue = params.value;
  }
  dynamicCellUpdate(params) {
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
    this.beneficiaryService.beneficiaryCellSave(dynamicCell).then(() => {
      this.toast.success(MessageConstant.UpdateSuccessful);
      this.loadGridData(1).then(a => {
        this.totalSelected = [];
        this.selected = []
      });
    })

  }

  get beneficiarySelected(): boolean {
    return this.totalSelected.length > 0 && this.currentInstance.status!=InstanceStatus.Completed;
  }
  get beneficiarySelectedSize(): number {
    return this.totalSelected.length;
  }
  editBeneficiary() {
    if (this.beneficiarySelectedSize == 1) {
      this.router.navigate(['/unicef/beneficiary/edit/' + this.totalSelected[0].id + '/' + this.selectedInstanceId]);
    }
  }

  deleteBeneficiarys() {
    this.modalService.confirm("Confirm Action", "", "Selected beneficairy will be deleted.", "Confirm").then(a => {

      if (a) {
        var ids = this.totalSelected.map(a => a.id);
        var data = { beneficiaryIds: ids, InstanceId: this.selectedInstanceId, status: CollectionStatus.Deleted };
        this.beneficiaryService.delete(data).then(() => {

          this.toast.success(MessageConstant.DeleteSuccess);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        })
        console.log(data);
      }

    })
  }
  activeBeneficiarys() {
    this.modalService.confirm("Confirm Action", "", "Selected beneficairy will be activated.", "Confirm").then(a => {

      if (a) {
        var ids = this.totalSelected.map(a => a.id);
        var data = { beneficiaryIds: ids, InstanceId: this.selectedInstanceId, status: CollectionStatus.NotCollected };
        this.beneficiaryService.active(data).then(() => {

          this.toast.success(MessageConstant.ActiveSuccess);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        })
        console.log(data);
      }

    })
  }
  approveInctiveBeneficiarys() {
    this.modalService.confirm("Confirm Action", "", "Selected beneficairy will be inactivated.", "Confirm").then(a => {

      if (a) {
        var ids = this.totalSelected.map(a => a.id);
        var data = { beneficiaryIds: ids, InstanceId: this.selectedInstanceId };
        this.approvalService.approveInactiveBeneficiaries(data).then(() => {

          this.toast.success(MessageConstant.InactiveSuccessfull);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        })
        console.log(data);
      }

    })
  }

  recollectBeneficiarys() {
    this.modalService.confirm("Confirm Action", "", "Selected beneficairy will change to recollect state.", "Confirm").then(a => {

      if (a) {
        var ids = this.totalSelected.map(a => a.id);
        var beneficiaryForRecollect = {
          "beneficiaryIds": ids,
          "instanceId": this.selectedInstanceId
        }
        this.approvalService.recollectBeneficiaries(beneficiaryForRecollect).then(a => {
          this.toast.success(MessageConstant.RecollectData);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        })

      }

    })
  }
  approveBeneficiarys() {
    this.modalService.confirm("Confirm Action", "", "Selected beneficairy will be approved.", "Confirm").then(a => {

      if (a) {
        var ids = this.totalSelected.map(a => a.id);
        var beneficiaryForApproval = {
          "beneficiaryIds": ids,
          "instanceId": this.selectedInstanceId
        }
        this.approvalService.approveBeneficiaries(beneficiaryForApproval).then(a => {
          this.toast.success(MessageConstant.ApproveSuccessfull);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        })

      }

    })
  }
  openBeneficiaryImportFile() {
    var fileCtrl = document.getElementById("ctrlImportBeneficiary");
    if (fileCtrl) {
      fileCtrl.click();
    }
  }
  openBeneficiaryVersionDataImportFile() {
    if (this.selectInstanceIdForImportVersionData > 0) {
      var fileCtrl = document.getElementById("ctrlImportBeneficiaryVersionData");
      if (fileCtrl) {
        fileCtrl.click();
      }
    }
  }

  onBeneficiaryImportFileSelected(files: FileList) {
    var fileToUpload = files.item(0);
    if (fileToUpload) {
      console.log("File found");
      var formData = new FormData();
      formData.append("file", fileToUpload);
      this.beneficiaryService.importBeneficiaries(formData).then(result => {

        this.toast.success("Total " + result.totalImported + " beneficiaries imported");
      });
      this.beneficiaryImportFileInput.nativeElement.value = '';
      console.log("Emptying...")
    }
  }
  selectInstanceForImportVersionData_Click(instance) {
    this.selectInstanceForImportVersionData = instance.title;
    this.selectInstanceIdForImportVersionData = instance.id;
    this.currentInstanceForVersionData = instance;
  }
  versionedFileUploadData: any;
  versionDataFileName: string = "";
  onBeneficiaryVersionDataImportFileSelected(files: FileList) {
    this.versionedFileUploadData = files.item(0);
    this.versionDataFileName = this.versionedFileUploadData.name;
    // if (fileToUpload) {
    //   console.log("File found");
    //   var formData = new FormData();
    //   formData.append("file", fileToUpload);
    //   formData.append("instanceId", this.selectInstanceIdForImportVersionData.toString());
    //   this.beneficiaryService.importBeneficiariesVersionData(formData).then(result => {

    //     this.toast.success("Total " + result.totalImported + " beneficiaries imported");
    //   });
    //   this.beneficiaryVersionDataImportFileInput.nativeElement.value = '';
    //   console.log("Emptying...")
    // }
  }
  uploadVersionData() {
    console.log('file Data', this.versionedFileUploadData)
    if (this.versionedFileUploadData) {
      // console.log("File found");
      var formData = new FormData();
      formData.append("file", this.versionedFileUploadData);
      formData.append("instanceId", this.selectInstanceIdForImportVersionData.toString());
      this.beneficiaryService.importBeneficiariesVersionData(formData).then(result => {
        this.modalService.open<IMportResultComponent, any>(IMportResultComponent, { importResult: result })
          .then(x => x)
          .catch(x => x);
        this.loadGridData(1).then(a => {
          this.totalSelected = [];
          this.selected = []
        });
      });
      this.beneficiaryVersionDataImportFileInput.nativeElement.value = '';
      this.versionDataImportModalClose.nativeElement.click();
      console.log("Emptying...")
    }
  }


  openDynamicColumnModal() {
    this.modalService.open<DynamicColumnComponent, DynamicColumnSaveViewModel>
      (DynamicColumnComponent, { entityTypeId: EntityType.Beneficiary })
      .then(column => {
        if (column) {
          this.afterDynamicColumnSave(column);
        }
      })
  }
  openDynamicColumnModalForEdit(entityColumnId) {
    this.modalService.open<DynamicColumnComponent, DynamicColumnSaveViewModel>
      (DynamicColumnComponent, { entityTypeId: EntityType.Beneficiary, entityColumnId: entityColumnId, title: "Update column.", buttonText: "Update" })
      .then(column => {
        if (column) {
          this.afterDynamicColumnSave(column);
        }
      })
  }

  async openDynamicColumnModalForDelete(entityColumnId) {
    if (this.selectedViewId > 0 && await this.modalService.confirm()) {
      await this.gridViewService.deleteBeneficiaryColumnToView(this.selectedViewId, entityColumnId);
      this.loadGridView().then(gridData => {
        this.loadGridData(1);
      });
      this.toast.success(MessageConstant.DeleteSuccess)
    }
  }
  afterDynamicColumnSave(column: DynamicColumnSaveViewModel) {
    if (this.selectedViewId > 0) {
      this.gridViewService.addBeneficiaryColumnToGridView({
        "GridViewId": this.selectedViewId,
        "EntityDynamicColumnId": column.id
      }).then(() => {
        this.toast.success(MessageConstant.SaveSuccessful);
        this.loadDynamicColumns().then(() => {
          this.loadGridData(1).then(a => {
            this.totalSelected = [];
            this.selected = []
          });
        })

      });
    }
    else {
      this.toast.success("Save successfull, Unable to add column to gridview.");
    }
  }
  instanceValue_changed(data) {
    this.lstSelectedInstance = data;
    Object.assign(this.currentInstance, data[0]);
    this.selectedInstanceId = data[0].id;
    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = []
    });
  }
  setCacheData(pageNo) {

    // return new Promise((resolve, reject) => {
    let pageInfo = { totalElements: 0, pageNumber: 1, size: 10 };
    pageInfo.pageNumber = pageNo;
    pageInfo.size = this.page.size;

    let beneficiaryCacheData = {
      filter: this.beneficiaryFilterModel,
      page: pageInfo,
      viewId: this.selectedViewId,
      instanceId: this.selectedInstanceId
    }
    this.localStorage.set(CacheConstant.BeneficiaryVersionDataGrid, beneficiaryCacheData);
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

  isActionButtonVisible(button: BeneficiaryActionButton) {
    if (!this.beneficiarySelectedSize) {
      return false;
    }

    // let ids = this.totalSelected.map(x => x.id);

    // let statusList =
    //   this.beneficiaries.filter(x => ids.indexOf(x.entityId) > -1)
    //     .map(x => x.collectionStatus);
    let statusList =
    this.totalSelected.map(x => x.collectionStatus);

    if(statusList.length == 0){
      return false;
    }
    if (Utility.hasDuplicate(statusList)) {
      return false;
    }
    return this.visibleActionButtons(statusList[0])
      .indexOf(button) > -1;
  }

  visibleActionButtons(st: CollectionStatus): BeneficiaryActionButton[] {
    switch (st) {
      case CollectionStatus.NotCollected:
        return [
          BeneficiaryActionButton.Edit,
          BeneficiaryActionButton.Delete,
          BeneficiaryActionButton.Approve
        ];
      case CollectionStatus.Collected:
        return [
          BeneficiaryActionButton.Edit,
          BeneficiaryActionButton.Delete,
          BeneficiaryActionButton.Approve,
          BeneficiaryActionButton.Recollect
        ];
      case CollectionStatus.Deleted:
        return [];
      case CollectionStatus.Approved:
        return [
          BeneficiaryActionButton.Edit,
          BeneficiaryActionButton.Delete
        ];
      case CollectionStatus.Recollect:
        return [
          BeneficiaryActionButton.Edit,
          BeneficiaryActionButton.Delete
        ];
      case CollectionStatus.Requested_Inactive:
        return [
          BeneficiaryActionButton.Edit,
          BeneficiaryActionButton.InactiveApprove,
          BeneficiaryActionButton.Delete,
          BeneficiaryActionButton.Recollect
        ];
      case CollectionStatus.Inactivated:
        return [
          BeneficiaryActionButton.Active,
          BeneficiaryActionButton.Delete
        ];
    }
  }
  async loadDynamicColumns() {

    return this.dynamicColumnService.getColumns({
      entityType: EntityType.Beneficiary,
      pageNo: 1,
      pageSize: this.globals.maxPageSize
    })
      .then(response => {
        this.dynamicColumns = response.data.map(x => ({ ...x, columnName: x.name, columnNameInBangla: x.nameInBangla }));
      });
  }
  getClosest(elem, selector) {

    // Get the closest matching element
    for (; elem && elem !== document; elem = elem.parentNode) {
      if (elem.matches(selector)) return elem;
    }
    return null;

  };
  async editDynamicCell(event, cell: IBeneficiaryDynamicCell) {
    this.addDynamicCell(event, cell);
  }
  async addDynamicCell(event, cell: IBeneficiaryDynamicCell) {
    var datatableBodyCell = this.getClosest(event.currentTarget, 'datatable-body-cell');

    datatableBodyCell.blur();
    // event.currentTarget.parentElement.parentElement.blur()

    let insertModel: IBeneficiaryDynamicCellInsertModel;

    let column = this.dynamicColumns.find(x => x.id == cell.entityDynamicColumnId);

    this.modalService
      .open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, { cell: cell, column: column })
      .then(async result => {

        if (result == null) {
          return;
        }
        if (result.isDeleted) {
          // await this.deleteDynamicCell(cell);
          return;
        }

        let dynamicCell = result.cell;

        if (dynamicCell != null && dynamicCell != undefined) {
          insertModel = {
            beneficiaryId: cell.beneficiaryId,
            instanceId: this.selectedInstanceId,
            dynamicCells: [{
              entityDynamicColumnId: cell.entityDynamicColumnId,
              value: dynamicCell.value
            }]
          };

          this.beneficiaryService.beneficiaryCellSave(insertModel).then(() => {
            this.toast.success(MessageConstant.SaveSuccessful);
            this.loadDynamicColumns().then(() => {
              this.loadGridData((this.page.pageNumber + 1)).then(() => {
                this.totalSelected = []
                this.selected = []
              });
            })
          });
        }
      })
  }

  downloadImportTemplate() {
    if (!this.selectedInstanceId || this.selectedInstanceId === 0) {
      this.toast.info('There is no scheduled instance selected');
      return;
    }
    this.beneficiaryService.downloadVersionDataImportTemplate(this.selectedInstanceId).then(res => {
      var fileName = "BeneficiaryVersionData.xlsx";
      var blob = new Blob([res],
        { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=UTF-8' });
      if (window.navigator && window.navigator.msSaveOrOpenBlob) { // for IE
        window.navigator.msSaveBlob(blob, fileName);
      } else {
        var objectUrl = window.URL.createObjectURL(blob);
        var link = document.createElement('a');
        link.setAttribute('download', fileName);
        link.setAttribute('href', objectUrl);
        link.click();
      }
    });
  }

  exportSelectedInstance() {
    this.beneficiaryService.exportByInstance(this.selectedInstanceId);
  }

  openMultiInstanceSelector() {
    this.multiInstanceSelector.nativeElement.click();
  }

  exportAggregatedData(instances: IScheduleInstance[]) {
    this.beneficiaryService.exportAggregated(instances.map(x => x.id));
  }


}
