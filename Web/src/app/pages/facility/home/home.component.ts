import { AfterViewInit, Component, ComponentFactoryResolver, ContentChild, OnInit, TemplateRef, ViewChild, ViewEncapsulation } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { EntityType } from 'src/app/_enums/entityType';
import { GridView } from 'src/app/models/gridView/gridView';

import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { BeneficiaryViewModel } from 'src/app/models/beneficiary/beneficiaryViewModel';
import { GridColumnDef } from 'src/app/models/dynamicColumn/gridColumnDef';
import { GridRowDef } from 'src/app/models/dynamicColumn/gridRowDef';
import { InstanceService } from 'src/app/services/instance.service';
import { GridviewService } from 'src/app/services/gridView.service';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';

import { MessageConstant } from 'src/app/utility/MessageConstant';
import { Router } from '@angular/router';

import { FacilityViewModel } from 'src/app/models/facility/facilityViewModel';
import { FacilityService } from 'src/app/services/facility.service';
import { TargetPopulation } from 'src/app/_enums/targetedPopulation';
import { FacilityType } from 'src/app/_enums/facilityType';
import { FacilityStatus } from 'src/app/_enums/facilityStatus';
import { TeacherViewModel } from 'src/app/models/user/teacherViewModel';
import { PaginationInstance } from 'ngx-pagination';
import { InstanceStatus } from 'src/app/_enums/instance-status';
import { IScheduleInstance, ScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { ModalService } from 'src/app/services/modal.service';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';

import { FacilityFilter } from 'src/app/models/facility/facilityFilterModel';
import { IMportResultComponent } from 'src/app/shared/components/import-result/import-result.component';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';
import { Column } from 'ag-grid-community';
import { NgxListTypeCellRenderer } from 'src/app/components/ngxDataGrid/list-type-cell-renderer.component';
import { NgxGridColumnDef } from 'src/app/models/dynamicColumn/ngxGridColumnDef';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { CacheConstant } from 'src/app/utility/CacheConstant';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { Convert } from 'src/app/utility/Convert';
import { AssignTeacherComponent } from 'src/app/shared/components/assign-teacher/assign-teacher.component';
import { FacilityEditViewModel } from 'src/app/models/facility/facilityEditViewModel';
import { Utility } from 'src/app/utility/Utility';
import { DataApprovalService } from 'src/app/services/approval.service';
import { FacilityActionButton } from './action-buttons';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { Globals } from 'src/app/globals';
import { IFacilityDynamicCellAddModel } from 'src/app/models/facility/dynamic-cell-add.model';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { IFacilityDynamicCell } from 'src/app/models/facility/facility-dynamic-cell.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit {
  public p: any;
  @ViewChild('deleteBeneficiaryModalClose') deleteBeneficiaryModalClose;
  @ViewChild('assignTeacherClose') assignTeacherClose;
  @ViewChild('facilityVersionDataImportFileInput') facilityVersionDataImportFileInput: any;
  @ViewChild('versionDataModalClose') versionDataModalClose: any;
  @ViewChild('filter') facilityFilter: any;
  @ViewChild('multiInstanceSelector') multiInstanceSelector: any;

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('hdrTplWithContextMenu') hdrTplWithContextMenu: any;
  @ViewChild('listCellTpl') listTypeCellRenderer: any;
  @ViewChild('customCellTpl') customCellTpl: any;
  @ViewChild('cellView') cellView: any;
  // @ContentChild('listtemplate') listTypeCellRenderer: TemplateRef<any>;


  public facilityFilterModel: FacilityFilter;
  public getFacilityFilterModel: FacilityFilter;

  public teachers: TeacherViewModel[];
  public selectedTempTeacherId: number;
  public selectedTempTeacherName: string;
  public selectedTeacherId: number;
  public selectedTeacherName: string;

  public gridView: GridView[] = [];
  public selectedView: string = "";
  public selectedViewId: number;

  public lstInstanceForImportVersionData: InstanceViewModel[];
  public selectInstanceForImportVersionData: string = '';
  public selectInstanceIdForImportVersionData: number;

  public currentInstanceForImportVersionData: IScheduleInstance;
  public lstInstance: InstanceViewModel[];

  public currentInstance: IScheduleInstance;

  public lstSelectedInstance: InstanceViewModel[] = [];
  public selectedInstanceId: number;
  public loadInstanceSelectorModal: boolean;

  public beneficiarySelectedRow: any[] = [];
  public facilitySelectedRow: any[] = [];
  public instanceStatusEnum: any;
  public cellOldValue: any;
  public dataType = [
    { name: "Int", value: 1 },
    { name: "Text", value: 2 },
    { name: "Decimal", value: 3 },
    { name: "Datetime", value: 4 },
    { name: "Boolean", value: 5 },
    { name: "List", value: 6 }
  ];

  public frameworkComponents: any;
  public pagedBeneficiaries: IPagedResponse<BeneficiaryViewModel>;
  public pagedFacilities: IPagedResponse<FacilityViewModel>;

  private allRecords = 2147483647;

  public overlayNoRowsTemplate: any;

  public teacherPaginationConfig: PaginationInstance = {
    id: 'teacher_pagination',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }


  public searchTerms = new Subject<string>();

  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 1, size: 10 };

  public collectionStatusList: ISelectListItem[] = [];
  public collectionStatus: CollectionStatus = 0;

  public dynamicColumns: IDynamicColumn[] = [];
  
  get facilities(): FacilityViewModel[]{

    return this.pagedFacilities ? this.pagedFacilities.data : [];
  }
  
  get isEditButtonVisible():boolean{
    if(this.facilitySelectedSize != 1)
      return false;
    // let id = this.totalSelected.map(x=> x.id)[0];           
    // let status = 
    // this.facilities.find(x => x.id == id).collectionStatus;
    let status= this.totalSelected[0].collectionStatus;
    return this.visibleActionButtons(status)
      .indexOf(FacilityActionButton.Edit) != -1;
  }

  get isDeleteButtonVisible():boolean{
    return this.isActionButtonVisible(FacilityActionButton.Delete);
  }

  get isApproveButtonVisible():boolean{
   return this.isActionButtonVisible(FacilityActionButton.Approve);
  }

  get isRecollectButtonVisible():boolean{
    return this.isActionButtonVisible(FacilityActionButton.Recollect);
  }

  get isAssignTeacherButtonVisible():boolean{
    return this.isActionButtonVisible(FacilityActionButton.AssignTeacher);
  }


  constructor(
    private router: Router,
    private toast: ToastrService,
    private instanceService: InstanceService,
    private gridViewService: GridviewService,
    private facilityService: FacilityService,
    private modalService: ModalService,
    private approvalService: DataApprovalService,
    private localStorage: LocalStorageService,
    private dynamicColumnService: DynamicColumnService,
    private globals: Globals
  ) {
    this.currentInstance = new ScheduleInstance();
    this.facilityFilterModel = new FacilityFilter();
    this.facilityFilterModel.programPartner = [];
    this.facilityFilterModel.implementationPartner = [];
    this.facilityFilterModel.teachers = [];


    this.instanceStatusEnum = InstanceStatus

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
      })
    });

    this.rows = [];
    this.columns = [];
  }



  isActionButtonVisible(button: FacilityActionButton){
   
    if(!this.facilitySelectedSize){
      return false;
    }        

    // let ids = this.totalSelected.map(x=> x.id);

    // let statusList =
    // this.facilities.filter(x => ids.indexOf(x.id) > -1)
    // .map(x => x.collectionStatus);

    let statusList =
    this.totalSelected.map(x => x.collectionStatus);

    if(statusList.length == 0){
      return false;
    }
    if(Utility.hasDuplicate(statusList)){
      return false;
    }
    console.log(statusList);
    return this.visibleActionButtons(statusList[0])
      .indexOf(button) > -1;
  }

  visibleActionButtons(st: CollectionStatus): FacilityActionButton[]{
    switch(st){
      case CollectionStatus.NotCollected:
        return [FacilityActionButton.Edit,
                FacilityActionButton.Delete,
                FacilityActionButton.Approve,
                FacilityActionButton.AssignTeacher];
      case CollectionStatus.Collected:
        return [FacilityActionButton.Edit,
                FacilityActionButton.Delete,
                FacilityActionButton.Approve,
                FacilityActionButton.Recollect,
                FacilityActionButton.AssignTeacher];
      case CollectionStatus.Deleted:
        return [];
      case CollectionStatus.Approved:
        return [FacilityActionButton.Edit,
                FacilityActionButton.Delete,                
                FacilityActionButton.AssignTeacher];
      case CollectionStatus.Recollect:
        return [FacilityActionButton.Edit,
                FacilityActionButton.Delete,          
                FacilityActionButton.AssignTeacher];
    }
  }

  search(term: string): void {
    this.searchTerms.next(term);
  }

  openDynamicColumnModal() {
    this.modalService.open<DynamicColumnComponent, DynamicColumnSaveViewModel>
      (DynamicColumnComponent, { entityTypeId: EntityType.Facility })
      .then(column => {
        if (column) {
          this.afterDynamicColumnSave(column);
        }
      })
  }
  openDynamicColumnModalForEdit(entityColumnId) {
    this.modalService.open<DynamicColumnComponent, DynamicColumnSaveViewModel>
      (DynamicColumnComponent, { entityTypeId: EntityType.Facility, entityColumnId: entityColumnId, title: "Update column.", buttonText: "Update" })
      .then(column => {
        if (column) {
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        }
      })
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
      }
    ] as ISelectListItem[];
  }

  async openDynamicColumnModalForDelete(entityColumnId) {
    if (this.selectedViewId > 0 && await this.modalService.confirm()) {
      await this.gridViewService.deleteFacilityColumnToView(this.selectedViewId, entityColumnId);
      this.loadGridView().then(gridData => {
        this.loadGridData(1);
      });
      this.toast.success(MessageConstant.DeleteSuccess)
    }
  }

  changeGridByCollectionStatus() {

    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }

  afterDynamicColumnSave(column: DynamicColumnSaveViewModel) {
    if (this.selectedViewId > 0) {
      this.gridViewService.addFacilityColumnToGridView({
        "GridViewId": this.selectedViewId,
        "EntityDynamicColumnId": column.id
      }).then(() => {
        this.toast.success(MessageConstant.SaveSuccessful);
        this.loadGridData(1).then(a => {
          this.totalSelected = []
          this.selected = []
        });
      });
    }
    else {
      this.toast.success("Save successfull, Unable to add column to gridview.");
    }
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
        if(value[i] != null){
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

    this.loadCollectionStatus();
    this.loadInstanceForVersionData();
    this.loadInstance().then(instanceData => {
      this.loadDynamicColumns(EntityType.Facility).then(a => {
        this.loadGridView().then(gridData => {
          var facilityCache = this.localStorage.get(CacheConstant.FacilityVersionDataGrid);
          if (facilityCache != null) {
            this.loadCacheData(facilityCache);
          }  
          this.loadGridData(this.page.pageNumber);  
        });
      });

      
    })

  }

  loadCacheData(facilityCache) {

    let searchText = this.facilityFilterModel.searchText;
    this.facilityFilterModel = new FacilityFilter()
    this.facilityFilterModel.programPartner = [];
    this.facilityFilterModel.implementationPartner = [];

    Object.assign(this.facilityFilterModel, facilityCache.filter);
    Object.assign(this.page, facilityCache.page)
    this.facilityFilterModel.searchText = searchText;



    if (facilityCache.viewId != null) {
      this.selectedViewId = facilityCache.viewId;
      this.gridView.filter(a => a.id == facilityCache.viewId).map(a => {
        this.selectedView = a.name;

      });
    }
    if (facilityCache.instanceId != null) {
      this.selectedInstanceId = facilityCache.instanceId;
      this.lstInstance.filter(a => a.id == facilityCache.instanceId).map(a => {
        this.lstSelectedInstance = [];
        this.lstSelectedInstance.push(a);
        this.currentInstance = a as unknown as IScheduleInstance;
      });

    }
    this.facilityFilter.loadData(this.facilityFilterModel);
  }

  async editDynamicCell(event, cell: IFacilityDynamicCell) {
    this.addDynamicCell(event, cell);
  }
  getClosest(elem, selector) {

    // Get the closest matching element
    for (; elem && elem !== document; elem = elem.parentNode) {
      if (elem.matches(selector)) return elem;
    }
    return null;

  };
  async addDynamicCell(event, cell: IFacilityDynamicCell) {
    var datatableBodyCell = this.getClosest(event.currentTarget, 'datatable-body-cell');

    datatableBodyCell.blur();


    
    let insertModel: IFacilityDynamicCellAddModel;

    let column = this.dynamicColumns.find(x => x.id == cell.entityDynamicColumnId);

    this.modalService
    .open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, { cell: cell, column: column })
    .then(async result => {
      if (result == null) {
        return;
      }
      if (result.isDeleted) {
        this.deleteFacilityCell(cell);
        return;
      }
      debugger;
      let dynamicCell = result.cell;

      if (dynamicCell != null && dynamicCell != undefined) {
        insertModel = {
          facilityId: cell.facilityId,
          instanceId: this.currentInstance.id,
          dynamicCells: [{
            entityDynamicColumnId: cell.entityDynamicColumnId,
            value: dynamicCell.value
          }]
        };

        this.facilityService.facilityCellSave(insertModel).then(() => {
          this.toast.success(MessageConstant.SaveSuccessful);
          this.reload();
        });
      }
    }).catch(x=> x);
  }
reload(){
  this.loadDynamicColumns(EntityType.Facility).then(() => {
    this.loadGridData((this.page.pageNumber + 1)).then(() => {
      this.totalSelected = []
      this.selected = []
    });
  });
}

deleteFacilityCell(cell: IFacilityDynamicCell){
  this.facilityService.deleteFacilityCell({
    entityDynamicColumnId: cell.entityDynamicColumnId,
    facilityId: cell.facilityId,
    instanceId: this.currentInstance.id
  }).then( x=> {
    this.toast.success(MessageConstant.DeleteSuccess);
    this.reload();
  });
}

  loadGridData(pageNo) {

    return new Promise((resolve) => {
      if (this.selectedInstanceId == null) {
        resolve(false);
        return
      }
      let facilityQueryModel: any = {
        instanceId: this.selectedInstanceId,
        collectionStatus: this.collectionStatus,
        viewId: this.selectedViewId,
        pageNo: pageNo,
        pageSize: this.page.size,
        filter: this.facilityFilterModel
      }

      var getDataFromApi = this.selectedViewId > 0 ?
        this.facilityService.getAllByViewId(facilityQueryModel)
        : this.facilityService.getAllByInstanceId(facilityQueryModel);

      getDataFromApi.then(response => {
        this.columns = []
        this.rows = []
        this.page.totalElements = 0;
        this.page.pageNumber = (pageNo - 1);

        this.columns = [
          { 
            prop: 'checkboxColumn', 
            name: "", 
            width: 10,
            headerCheckboxable: "true", 
            checkboxable: "true"
          }
        ];
        if (response.data.length > 0) {

          response.data[0].properties.map(prop => {
            var cData = new NgxGridColumnDef(prop);
            cData["headerTemplate"] = this.hdrTplWithContextMenu;
            if(prop.isFixed){
              if (prop.dataType == ColumnDataType.List) {
                cData["cellTemplate"] = this.listTypeCellRenderer;
              }            
              else {
                cData["cellTemplate"] = this.customCellTpl;
              }
              
            }else{              
              cData["cellTemplate"] = this.cellView;
            }
            this.columns.push(cData);
          });

          response.data.forEach(facilityVM => {
            let row = {
              id: facilityVM.id,
              collectionStatus: facilityVM.collectionStatus
            };
            let props = facilityVM.properties.map(prop=> {
              return {
                entityDynamicColumnId: prop.entityColumnId,
                value: prop.values,
                values: prop.values,
                columnName: prop.columnName,
                dataType: prop.dataType,  
                facilityId: facilityVM.id,
                listType: {
                  listItems: prop.listItem,
                  name: ""                 
                }
                
              } as IFacilityDynamicCell
            });

            
            props.forEach(cell => {
              let fieldName = "field" + cell.entityDynamicColumnId.toString();
              row[fieldName] = cell
            });            
            this.rows.push(row);
          });

          this.page.totalElements = response.total;
          this.page.pageNumber = (pageNo - 1);

        }
        this.pagedFacilities = response;
        this.setCacheData(pageNo);
        resolve(true);


      })
    });
  }


  loadGridView() {
    return new Promise((resolve, reject) => {
      var data = this.gridViewService.getFacilityView(this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;
          this.gridView = data.data;

          this.selectedView = "Complete View";
          this.selectedViewId = 0;
          resolve(true);
        }, () => {
          this.selectedView = "Complete View";
          this.selectedViewId = 0;
          resolve(false);
        })
    })
  }


  loadInstance() {
    return new Promise((resolve, reject) => {
      var data = this.instanceService.getNotPendingInstances(EntityType.Facility, this.allRecords, 1)
        .then((data) => {
          //this.dynamicColumn = data.data;
         // this.lstInstanceForImportVersionData = data.data;

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
      var data = this.instanceService.getRunningInstance(EntityType.Facility, this.allRecords, 1)
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
    //this.refreshGrid();
    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = [];
    });
  }

  onPageSizeChanged(pageSize: number) {
    this.page.size = Number(pageSize);
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }

  dynamicCellOldValue(params) {
    console.log('old', params);
    this.cellOldValue = params.value;
  }
  
  editFacility() {
    
    debugger;
    

    this.router.navigate(['/unicef/facility/edit', this.totalSelected[0].id, this.selectedInstanceId]);
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
      "facilityId": params.data.id,
      "instanceId": this.selectedInstanceId,
      "dynamicCells": [
        {
          "value": val,
          "entityDynamicColumnId": params.colDef.columnId
        }
      ]
    }
    this.facilityService.facilityCellSave(dynamicCell).then(() => {
      this.toast.success(MessageConstant.UpdateSuccessful);
      // this.refreshGrid();
      //this.uncheckAllSelectedRow();
    })

  }


  get facilitySelected(): boolean {
    return this.totalSelected.length > 0 && this.currentInstance.status!= InstanceStatus.Completed;
  }
  get facilitySelectedSize(): number {
    return this.totalSelected.length;
  }


  openFacilityVersionDataImportFile() {
    if (this.selectInstanceIdForImportVersionData > 0) {
      var fileCtrl = document.getElementById("ctrlImportFacilityVersionData");
      if (fileCtrl) {
        fileCtrl.click();
      }
    }
  }

  selectInstanceForImportVersionData_Click(instance) {
    this.selectInstanceForImportVersionData = instance.title;
    this.selectInstanceIdForImportVersionData = instance.id;
    this.currentInstanceForImportVersionData = instance;
  }
  versionedFileUploadData: any;
  versionDataFileName: string = "";
  onFacilityVersionDataImportFileSelected(files: FileList) {
    this.versionedFileUploadData = files.item(0);
    this.versionDataFileName = this.versionedFileUploadData.name;

  }
  uploadVersionData() {
    //console.log('file Data', this.versionedFileUploadData)
    if (this.versionedFileUploadData) {
      var formData = new FormData();
      formData.append("file", this.versionedFileUploadData);
      formData.append("instanceId", this.selectInstanceIdForImportVersionData.toString());
      this.facilityService.importFacilityVersionData(formData).then(result => {

        this.modalService.open<IMportResultComponent, any>(IMportResultComponent, { importResult: result })
          .then(x => x)
          .catch(x => x);
        this.loadGridData(1).then(a => {
          this.totalSelected = []
          this.selected = []
        });
      });
      this.versionDataModalClose.nativeElement.click();
      this.facilityVersionDataImportFileInput.nativeElement.value = '';
    }
  }

  instanceValue_changed(data) {
    this.lstSelectedInstance = data;
    Object.assign(this.currentInstance, data[0]);
    this.selectedInstanceId = data[0].id;
    this.loadGridData(1).then(a => {
      this.totalSelected = []
      this.selected = []
    });
  }
  setCacheData(pageNo) {

    // return new Promise((resolve, reject) => {
    let pageInfo = { totalElements: 0, pageNumber: 1, size: 10 };
    pageInfo.pageNumber = pageNo;
    pageInfo.size = this.page.size;

    let facilityCacheData = {
      filter: this.facilityFilterModel,
      page: pageInfo,
      viewId: this.selectedViewId,
      instanceId: this.selectedInstanceId
    }

    this.localStorage.set(CacheConstant.FacilityVersionDataGrid, facilityCacheData);
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

  downloadTemplate() {
    this.facilityService.downloadVersionedImportTemplate(this.selectedInstanceId);
  }

  exportSelectedInstance() {
    this.facilityService.exportByInstance(this.selectedInstanceId);
  }

  openMultiInstanceSelector() {
    this.multiInstanceSelector.nativeElement.click();
  }

  exportAggregatedData(instances: IScheduleInstance[]) {
    this.facilityService.exportAggregated(instances.map(x => x.id));
  }

  deleteFacility(){    
    this.modalService.confirm()
    .then(isYes => {
      if(isYes){
        this.facilityService.delete({
          facilityIds: this.totalSelected.map(x=> x.id),
          instanceId: this.currentInstance.id
        }).then(response => {
          this.toast.success(MessageConstant.DeleteSuccess);
          this.loadGridData(1).then(a => {
            this.totalSelected = []
            this.selected = []
          });
        })
        .catch(x=> x);
      }
    });    
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
           teacherId: this.selectedTeacherId,
           instanceId: this.selectedInstanceId
         }).then(a => {
           this.toast.success(MessageConstant.SaveSuccessful);
           this.loadGridData(1).then(a => {
             this.totalSelected = []
             this.selected = []
           });
         })
       }

     });
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

  async loadDynamicColumns(entityType: EntityType) {
    return this.dynamicColumnService.getColumns({
        entityType: entityType,
        pageNo: 1,
        pageSize: this.globals.maxPageSize
    }).then(response => {
          this.dynamicColumns = response.data.map(x => ({ ...x, columnName: x.name, columnNameInBangla: x.nameInBangla }));
      });
}
}
