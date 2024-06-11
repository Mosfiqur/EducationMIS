import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { IDatasource, IGetRowsParams, Module } from 'ag-grid-community';

import { IndicatorService } from 'src/app/services/indicator.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { GridRowDef } from 'src/app/models/dynamicColumn/gridRowDef';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { IndicatorViewModel } from 'src/app/models/indicator/indicatorViewModel';
import { EntityType } from 'src/app/_enums/entityType';
import { GridColumnDef } from 'src/app/models/dynamicColumn/gridColumnDef';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { ModalService } from 'src/app/services/modal.service';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { InstanceViewModel } from 'src/app/models/instance/instanceViewModel';
import { IScheduleInstance, ScheduleInstance } from 'src/app/models/scheduling/schedule-instance.model';
import { InstanceService } from 'src/app/services/instance.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { Subject } from 'rxjs/internal/Subject';
import { ColumnMode, SelectionType } from '@swimlane/ngx-datatable';

@Component({
  selector: 'app-indicators',
  templateUrl: './indicators.component.html',
  styleUrls: ['./indicators.component.scss'],
  encapsulation: ViewEncapsulation.None

})
export class IndicatorsComponent implements OnInit {

  @ViewChild('addIndicatorModalClose') addIndicatorModalClose;
  @ViewChild('customCellTpl') customCellTpl: any;

  public gridApi;
  public gridColumnApi;

  public columnDefs;
  public lstGridColumnDef = [];
  public rowData;
  public lstGridData = [];
  private allRecords = 2147483647;
  public cacheBlockSize = 10;
  public paginationPageSize = 10;
  public pagedIndicators: IPagedResponse<IndicatorViewModel>;

  public lstInstance: InstanceViewModel[];
  public currentInstance: IScheduleInstance;
  public lstSelectedInstance: InstanceViewModel[] = [];
  public selectedInstanceId: number=0;
  public instanceLoaded: boolean = false;


  public searchToAddIndicatorText = new Subject<string>();

  public totalSelected = [];
  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 0, size: 10 };

  //for dynamic column grid (dcg)
  public dcgtotalSelected = [];
  public dcgrows: any[];
  public dcgcolumns: any[];
  public dcgSelected = [];
  public dcgColumnMode = ColumnMode;
  public dcgSelectionType = SelectionType;

  public dcgPage = { totalElements: 0, pageNumber: 0, size: 10 };

  constructor(

    private router: Router,
    private dynamicColumnService: DynamicColumnService,
    private toast: ToastrService,
    private indicatorService: IndicatorService,
    private modalService: ModalService,
    private instanceService: InstanceService,
    private route: ActivatedRoute
  ) {
 

    this.currentInstance = new ScheduleInstance();


    this.searchToAddIndicatorText.pipe(
      // wait 400ms after each keystroke before considering the term
      debounceTime(400),
      // ignore new term if same as previous term
      distinctUntilChanged(),

    ).subscribe(value => {
      this.dynamicColumnSearchText = value;
      this.loadDynamicolumnGridData(1, this.dynamicColumnSearchText).then(a => {
        this.dcgtotalSelected = [];
        this.dcgSelected = [];
      });
    });

    this.rows = [];
    this.columns = [];
    this.dcgrows = [];
    this.dcgcolumns = [];
  }



  rowIdentity = (row: any) => {
    return row.entityDynamicColumnId;
  }

  onActivate(event) {
    if (event.type == 'click') {
      console.log(event.row);
    }
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
        return item.entityDynamicColumnId === data.entityDynamicColumnId
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
          return item.entityDynamicColumnId !== data.entityDynamicColumnId
        })

      })

      this.totalSelected.push(...this.selected);
    }
    else {
      this.rows.forEach(data => {
        this.totalSelected = this.totalSelected.filter(function (item) {
          return item.entityDynamicColumnId !== data.entityDynamicColumnId
        })

      })

    }
  }


  ngOnInit() {
    this.loadInstance().then(a => {


      this.route.params.subscribe((params) => {
        var id = params['instanceId'];
        this.lstInstance.filter(a => a.id == id).map(a => {

          Object.assign(this.currentInstance, a);
          this.lstSelectedInstance.push(a);
          this.selectedInstanceId = a.id;
          this.instanceLoaded = true;
        });
        if (this.selectedInstanceId > 0) {
          this.loadGridData(1);
          this.loadDynamicolumnGridData(1, "");
        }
      });
    });



  }

  searchToAddIndicator(term: string): void {
    this.searchToAddIndicatorText.next(term);
  }

  loadGridData(pageNo) {

    return new Promise((resolve, reject) => {
      this.lstGridData = [];
      if (this.selectedInstanceId) {
        this.indicatorService.getBeneficiaryIndicator(this.page.size, pageNo, this.selectedInstanceId).then(data => {
          this.columns = []
          this.rows = []

          if (data.data.length > 0) {
            this.columns = [
              //{ prop: 'facilityId', name: "Id" },
              {
                prop: 'indicatorName',
                name: 'English',
                width: 250,
                headerCheckboxable: "true",
                checkboxable: "true",
                cellTemplate: this.customCellTpl
              }
              , { 'prop': 'indicatorNameInBangla', 'name': 'Bangla',cellTemplate: this.customCellTpl }
              , { 'prop': 'type', 'name': 'Type',cellTemplate: this.customCellTpl }
              , { 'prop': 'listItemName', 'name': 'List Values',cellTemplate: this.customCellTpl }


            ];

            data.data.map(a => {
              var data = {};
              data['id'] = a.id;
              data['entityDynamicColumnId'] = a.entityDynamicColumnId;
              data['indicatorName'] = a.indicatorName;
              data['indicatorNameInBangla'] = a.indicatorNameInBangla;
              data['type'] = ColumnDataType[a.columnDataType];
              data['listItemName'] = a.listItems.map(o => o.title).join(', ');
              this.rows.push(data);
            })
            this.page.totalElements = data.total;
            this.page.pageNumber = (pageNo - 1);
            this.pagedIndicators = data;
            // console.log('col',this.columnDefs,this.rowData);
            resolve(true);
          }
          else{
            resolve(false);
          }
          

        })
      }
     
    });
  }

  onPageSizeChanged(pageSize: number) {
    this.page.size = pageSize;
    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = []
    })
  }


  get indicatorSelected(): boolean {
    return this.totalSelected.length > 0;
  }
  get indicatorSelectedSize(): number {
    return this.totalSelected.length;
  }

  //start load modal data





  dcgRowIdentity = (row: any) => {
    return row.id;
  }

  dcgOnActivate(event) {
    if (event.type == 'click') {
      console.log(event.row);
    }
  }

  dcgSetPage(pageInfo) {
    console.log('pagination change', pageInfo);
    this.page.pageNumber = pageInfo.page;
    this.loadDynamicolumnGridData(this.page.pageNumber, this.dynamicColumnSearchText).then(a => {
      this.dcgGetSelectedData();
    });
  }

  dcgOnNgxDataTableSelect({ selected }) {

    this.dcgSelected.splice(0, this.dcgSelected.length);
    this.dcgSelected.push(...selected);
    this.dcgAddToTotalSelect();
  }
  dcgGetSelectedData() {
    this.dcgSelected = [];
    this.dcgrows.forEach(data => {

      this.dcgSelected.push(...this.dcgtotalSelected.filter(function (item) {
        return item.id === data.id
      }))

    })

  }
  get dcgGetSelected() {
    return this.dcgtotalSelected.length;
  }
  dcgAddToTotalSelect() {

    if (this.dcgSelected.length > 0) {
      this.dcgrows.forEach(data => {

        this.dcgtotalSelected = this.dcgtotalSelected.filter(function (item) {
          return item.id !== data.id
        })

      })

      this.dcgtotalSelected.push(...this.dcgSelected);
    }
    else {
      this.dcgrows.forEach(data => {
        this.dcgtotalSelected = this.dcgtotalSelected.filter(function (item) {
          return item.id !== data.id
        })

      })

    }
  }



  private dynamicColumnSearchText = "";

  loadInstance() {
    return new Promise((resolve, reject) => {
      var data = this.instanceService
        .getInstanceStatusWise(EntityType.Beneficiary, 2, this.allRecords, 1)
        //.getNotPendingInstances(EntityType.Beneficiary, this.allRecords, 1)
        .then((data) => {

          //this.dynamicColumn = data.data;
          this.lstInstance = data.data;
          var maxid = 0;
          this.lstInstance.map(function (obj) {
            if (obj.id > maxid) maxid = obj.id;
          });
          this.lstInstance.filter(a => a.id == maxid).map(a => {

            Object.assign(this.currentInstance, a);
            this.lstSelectedInstance.push(a);
            this.selectedInstanceId = a.id;
            this.instanceLoaded = true;
          });
          resolve(true)
        })
    });
  }

  loadDynamicolumnGridData(pageNo, searchText) {

    return new Promise((resolve, reject) => {

      this.dynamicColumnService.getColumnsForIndicator(this.paginationPageSize, pageNo, EntityType.Beneficiary, this.selectedInstanceId, searchText).then(data => {
        this.dcgcolumns = []
        this.dcgrows = []


        this.dcgcolumns = [
          //{ prop: 'facilityId', name: "Id" },
          {
            prop: 'dynamicColumnName',
            name: 'English',
            width: 250,
            headerCheckboxable: "true",
            checkboxable: "true",
            cellTemplate: this.customCellTpl
          }
          , { 'prop': 'dynamicColumnNameInBangla', 'name': 'Bangla',cellTemplate: this.customCellTpl }
          , { 'prop': 'type', 'name': 'Type',cellTemplate: this.customCellTpl }
          , { 'prop': 'listItemName', 'name': 'List Values',cellTemplate: this.customCellTpl }


        ];

        data.data.map(a => {
          var data = {};
          data['id'] = a.id;
          data['dynamicColumnName'] = a.name;
          data['dynamicColumnNameInBangla'] = a.nameInBangla;
          data['type'] = ColumnDataType[a.columnDataType];
          data['listItemName'] = a.listItems.map(o => o.title).join(', ');
          this.dcgrows.push(data);
        })
        this.dcgPage.totalElements = data.total;
        this.dcgPage.pageNumber = (pageNo - 1);
        resolve(true);


      })
    });
  }

  addIndicator() {
    if (this.dcgtotalSelected.length > 0) {
      let lstForIndicator: any[] = [];
      this.dcgtotalSelected.map(a => {

        let data = {
          "entityDynamicColumnId": a.id,
          "entityTypeId": EntityType.Beneficiary,
          "instanceId": this.selectedInstanceId
        };
        lstForIndicator.push(data);
      });

      this.indicatorService.saveBeneficiary(lstForIndicator).then(a => {
        this.loadDynamicolumnGridData(1, this.dynamicColumnSearchText).then(a => {
          this.dcgtotalSelected = [];
          this.dcgSelected = [];
        });

        this.loadGridData(1).then(a => {
          this.totalSelected = [];
          this.selected = []
        })


        this.addIndicatorModalClose.nativeElement.click();

      });


    }
  }

  async removeIndicators() {

    if (await this.modalService.confirm()) {

      // this.indicatorSelectedRow.forEach(indicator => {
      //   this.indicatorService.remove(indicator.id)
      //   .then(response => {
      //     this.toast.success(MessageConstant.DeleteSuccess);
      //   })
      // });
      let lstForIndicator: any[] = [];
      this.totalSelected.map(a => {
        let data = {
          "entityDynamicColumnId": a.entityDynamicColumnId,
          "instanceId": this.selectedInstanceId
        };
        lstForIndicator.push(data);
      });
      this.indicatorService.removeBeneficiary(lstForIndicator)
        .then(response => {
          this.toast.success(MessageConstant.DeleteSuccess);
          this.loadDynamicolumnGridData(1, this.dynamicColumnSearchText).then(a => {
            this.dcgtotalSelected = [];
            this.dcgSelected = [];
          });

          this.loadGridData(1).then(a => {
            this.totalSelected = [];
            this.selected = []
          })
        })
    }
  }
  instanceValue_changed(data) {
    this.lstSelectedInstance = data;
    Object.assign(this.currentInstance, data[0]);
    this.selectedInstanceId = data[0].id;
    this.loadDynamicolumnGridData(1, this.dynamicColumnSearchText).then(a => {
      this.dcgtotalSelected = [];
      this.dcgSelected = [];
    });

    this.loadGridData(1).then(a => {
      this.totalSelected = [];
      this.selected = []
    })

  }
}
