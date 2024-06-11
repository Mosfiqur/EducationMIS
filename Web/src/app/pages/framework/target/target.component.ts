import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FrameworkDynamicColumnService } from 'src/app/services/framework-dynamic-column.service';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { FrameworkType } from 'src/app/_enums/frameworkType';
import { Globals } from 'src/app/globals';

import { ModalService } from 'src/app/services/modal.service';
import { FwColumnEditorComponent } from 'src/app/components/fw-column-editor/fw-column-editor.component';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { ITargetFramework } from 'src/app/models/frameworks/target-framework.model';
import { PaginationInstance } from 'ngx-pagination';
import { TargetService } from 'src/app/services/target.service';
import { ITargetFrameworkDynamicCell, ITargetFrameworkInsertModel, TargetFrameworkDynamicCell } from 'src/app/models/frameworks/target-framework-dynamic-cell.model';
import { IColumnEditorResult } from 'src/app/models/helpers/column-editor-result';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { Camp } from 'src/app/models/common/camp.model';
import { CommonService } from 'src/app/services/common.service';
import { debounceTime } from 'rxjs/operators';
import { FrameworkBaseComponent } from '../framework-base.component';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { EntityType } from 'src/app/_enums/entityType';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { NgbTypeaheadWindow } from '@ng-bootstrap/ng-bootstrap/typeahead/typeahead-window';
import { ColumnMode, SelectionType } from 'src/app/lib/ngx-datatable';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { Gender } from 'src/app/_enums/gender';

@Component({
  selector: 'app-target',
  templateUrl: './target.component.html',
  styleUrls: ['./target.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class TargetComponent extends FrameworkBaseComponent implements OnInit {

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('hdrTplWithContextMenu') hdrTplWithContextMenu: any;
  @ViewChild('customCellTpl') customCellTpl: any;
  @ViewChild('cellView') cellView: any;

  frameworkType: any;
  camps: Camp[] = [];
  pagedTargets: IPagedResponse<ITargetFramework>;

  targetsWithCells: ITargetFramework[];


  paginationConfig: PaginationInstance = {
    id: 'target_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  get targets(): ITargetFramework[] {
    return this.pagedTargets ? this.pagedTargets.data : []
  }

  set targets(targets: ITargetFramework[]) {
    this.pagedTargets.data = targets;
  }

  isAllSelected: boolean = false;
  selectedTargets: Map<number, boolean>;

  get numberOfTargetsSelected(): number {
    return this.totalSelected.length;
  }

  get selectedTarget(): ITargetFramework {
    return this.targets.filter(x => x.isSelected)[0];
  }



  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 0, size: 10 };


  constructor(
    private toastrService: ToastrService,
    protected globals: Globals,
    protected modalService: ModalService,
    private targetService: TargetService,
    private commonService: CommonService,
    protected dynamicColumnService: DynamicColumnService
  ) {
    super(dynamicColumnService, globals, modalService)
    this.frameworkType = FrameworkType;
    this.selectedTargets = new Map();

    this.rows = [];
    this.columns = [];

  }


  rowIdentity = (row: any) => {
    return row.id;
  }

  onActivate(event) {
    if (event.type === 'click') {
      event.cellElement.blur()
    };
  }

  setPage(pageInfo) {
    console.log('pagination change', pageInfo);
    this.page.pageNumber = pageInfo.page;
    this.loadGridData(this.page.pageNumber).then(() => {
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
    this.rows.forEach(data => {
      this.totalSelected = this.totalSelected.filter(function (item) {
        return item.id !== data.id
      })
    })
    if (this.selected.length > 0) {
      this.totalSelected.push(...this.selected);
    }

  }

  selectNgxDataRow(value) {
    console.log('selectCheck event', value);
  }

  getListCellText(val) {

    let returnText = "";
    var col = this.dynamicColumns.find(x => x.id == val.entityDynamicColumnId);
    if (col.columnDataType == ColumnDataType.List) {
      var value = val.values;
      var column = col.listItems;
      for (let i = 0; i < value.length; i++) {
        returnText = returnText + column.filter(a => a.value == value[i])[0].title + ':' + value[i] + ','
      }
    }
    else {
      if (val.values != null)
        returnText = val.values.length > 0 ? val.values[0] : "";
    }
    return returnText;//`Name ${row["name"]}, Gender is : ${row["company"]}`;
  }


  async ngOnInit() {
    this.loadDynamicColumns(EntityType.Target).then(a => {
      this.loadGridData(1)
    });
    await this.getAllTargets(1);
    this.camps = (await this.commonService.getCamps({ pageNo: 1, pageSize: this.globals.maxPageSize })).data;
  }

  async getAllTargets(pageNo: number) {
    this.targetService.getAll({ pageNo: pageNo, pageSize: this.paginationConfig.itemsPerPage })
      .then(x => {

        this.pagedTargets = x;
        this.paginationConfig.totalItems = this.pagedTargets.total;
        this.paginationConfig.currentPage = pageNo;
        this.arrangeCells();

        this.applySelection();
      });
  }

  getTargetData(pageNo: number) {
    return new Promise<IPagedResponse<ITargetFramework>>((resolve) => {

      this.targetService.getAll({ pageNo: pageNo, pageSize: this.paginationConfig.itemsPerPage })
        .then(data => {
          this.pagedTargets = data;

          this.paginationConfig.totalItems = this.pagedTargets.total;
          this.paginationConfig.currentPage = pageNo;

          this.arrangeCells();
          this.applySelection();
          resolve(this.pagedTargets);
        })
    });
  }
  loadGridData(pageNo) {

    return new Promise((resolve) => {
      this.getTargetData(pageNo).then(data => {
        this.rows = [];
        this.columns = [];

        this.columns = [
          {
            prop: 'campName'
            , name: "Camp Name"
            // , width: 200
            , headerCheckboxable: "true"
            , checkboxable: "true"
            , frozenLeft: "true"
            , cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl
          },
          {
            prop: 'gender',
            name: "Gender"
            , cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl
          }
          , { prop: "ageGroupName", name: "Age group", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "peopleInNeed", name: 'PIN', cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "target", name: "Target", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "startDate", name: "Start Date", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "endDate", name: "End Date", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }

        ];

        if (data.data.length > 0) {

          data.data[0].dynamicCells.map(val => {
            var cData = {};
            cData["prop"] = "field" + val.entityDynamicColumnId.toString();
            cData["name"] = val.columnName;
            cData["headerTemplate"] = this.hdrTplWithContextMenu;
            cData["cellTemplate"] = this.cellView;
            cData["columnId"] = val.entityDynamicColumnId;
            this.columns.push(cData);

          });
        }


        data.data.map(a => {

          var data = {}
          a.dynamicCells.forEach(a => {
            var fieldName = "field" + a.entityDynamicColumnId.toString();
            data[fieldName] = a
          })
          data['id'] = a.id;
          data['campName'] = a.campName;
          data['gender'] =Gender[a.gender];
          data['ageGroupName'] = a.ageGroupName;
          data['peopleInNeed'] = a.peopleInNeed;
          data['target'] = a.target;
          data['startDate'] = a.startMonth + '/' + a.startYear;
          data['endDate'] = a.endMonth + '/' + a.endYear;
          this.rows.push(data);
        })

        this.page.totalElements = data.total;
        this.page.pageNumber = (pageNo - 1);

        this.paginationConfig.totalItems = data.total;
        resolve(true);

      })
    });
  }
  openDynamicColumnModalForEdit(entityColumnId) {
    this.modalService.open<DynamicColumnComponent, DynamicColumnSaveViewModel>
      (DynamicColumnComponent, { entityTypeId: EntityType.Target, entityColumnId: entityColumnId, title: "Update column.", buttonText: "Update" })
      .then(column => {
        if (column) {
          this.loadGridData(1).then(() => {
            this.totalSelected = []
            this.selected = []
          });
        }
      })
  }

  async openDynamicColumnModalForDelete(entityColumnId) {

    if (await this.modalService.confirm()) {

      await this.dynamicColumnService.deleteDynamicColumn(entityColumnId,EntityType.Target);
      this.loadDynamicColumns(EntityType.Target).then(a => {
        this.loadGridData(1);
      });
      this.toastrService.success(MessageConstant.DeleteSuccess);
    }
  }

  applySelection() {
    this.targets.forEach(x => {
      if (this.selectedTargets.has(x.id)) {
        x.isSelected = true;
      }
    })
  }

  onPageSizeChanged(pageSize: number) {
    this.paginationConfig.itemsPerPage = pageSize;
    //this.getAllTargets(1);
    this.loadDynamicColumns(EntityType.Target).then(() => {
      this.loadGridData(1).then(() => {

        this.totalSelected = []
        this.selected = []
      });
    })
  }

  arrangeCells() {
    this.pagedTargets.data.forEach(target => {
      let cells = new Array<ITargetFrameworkDynamicCell>(this.dynamicColumns.length);
      this.dynamicColumns.forEach((column, index) => {

        let existing = null;
        existing = target
          .dynamicCells
          .find(x => x.entityDynamicColumnId == column.id);

        if (existing == null) {
          cells[index] = new TargetFrameworkDynamicCell(target.id, column.id, [], column.columnName);
        } else {
          cells[index] = existing;
        }
        cells[index].listType = column.listObject;
        cells[index].dataType = column.columnDataType;
      })
      target.dynamicCells = cells;
    });
  }
  async getPage(pageNo: number) {
    await this.getAllTargets(pageNo);
  }

  pushEmptyCell(column: IDynamicColumn) {
    this.targets.forEach(target => {
      let cell = {
        targetFrameworkId: target.id,
        entityDynamicColumnId: column.id,
        values: [],
        columnName: column.name
      } as ITargetFrameworkDynamicCell;
      cell.listType = column.listObject;
      cell.dataType = column.columnDataType;
      target.dynamicCells.push(cell)
    });
  }

  removeCells(id: number) {
    this.targets.forEach(target => {
      target.dynamicCells.splice(target.dynamicCells.findIndex(x => x.entityDynamicColumnId == id), 1);
    });
  }

  async onAddNewDynamicColumn() {
    super.addNewDynamicColumn(EntityType.Target).then(a => {
      this.loadGridData(1);
    });
  }

  async onEditDynamicColumn(selectedColumn: IDynamicColumn) {
    throw Error("Not implemented");
  }

  async editDynamicCell(event, cell: ITargetFrameworkDynamicCell) {
    this.addDynamicCell(event, cell);
  }

  async deleteDynamicCell(cell: ITargetFrameworkDynamicCell) {
    if (await this.modalService.confirm()) {
      await this.targetService.deleteDynamicCell(cell).then(a => {
        this.loadGridData((this.page.pageNumber + 1)).then(() => {
          this.totalSelected = []
          this.selected = []
        });
        this.toastrService.success(MessageConstant.DeleteSuccess);
      })
      // this.getAllTargets(1);
      // this.toastrService.success(MessageConstant.DeleteSuccess);
    }
  }
  getClosest(elem, selector) {

    // Get the closest matching element
    for (; elem && elem !== document; elem = elem.parentNode) {
      if (elem.matches(selector)) return elem;
    }
    return null;

  };
  async addDynamicCell(event, cell: ITargetFrameworkDynamicCell) {
    var datatableBodyCell = this.getClosest(event.currentTarget, 'datatable-body-cell');
    datatableBodyCell.blur();

    let insertModel: ITargetFrameworkInsertModel;

    let column = this.dynamicColumns.find(x => x.id == cell.entityDynamicColumnId);
    this.modalService
      .open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, { cell: cell, column: column })
      .then(async result => {
        if (result == null) {
          return;
        }

        if (result.isDeleted) {
          await this.deleteDynamicCell(cell);
          return;
        }
        let dynamicCell = result.cell;


        if (dynamicCell != null && dynamicCell != undefined) {
          insertModel = {
            targetFrameworkId: cell.targetFrameworkId,
            dynamicCells: [{
              entityDynamicColumnId: cell.entityDynamicColumnId,
              value: dynamicCell.value
            }]
          };

          return this.targetService.insertDynamicCell(insertModel).then(a => {
            this.loadDynamicColumns(EntityType.Target).then(() => {
              this.loadGridData((this.page.pageNumber + 1)).then(() => {
                this.totalSelected = []
                this.selected = []
              });
            })
          });
        }
      })
      .then(inserted => {
        // this.getAllTargets(1);    
        // this.toastrService.success(MessageConstant.SaveSuccessful); 
      });
  }

  async editFixedCell(target: ITargetFramework, columnName: string, propertyName: string) {

    let cell = new TargetFrameworkDynamicCell(target.id, 0, target[propertyName], columnName)
    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, { cell: cell, inputType: "number" })
      .then(async result => {
        if (result.cell != null && result.cell != undefined) {

          let updateModel = { ...target } as ITargetFramework;
          updateModel[propertyName] = result.cell.value;
          await this.targetService.updateTarget(updateModel);
          target[propertyName] = result.cell.value;
          this.toastrService.success(MessageConstant.SaveSuccessful);
        }
      });
  }

  toggleSelection(target: ITargetFramework) {
    if (target.isSelected) {
      this.selectedTargets.set(target.id, true);
    } else {
      this.selectedTargets.delete(target.id);
    }

    if (this.selectedTargets.size != this.targets.length) {
      this.isAllSelected = false;
    } else {
      this.isAllSelected = true;
    }
  }

  selectAll(event) {
    this.targets.forEach(x => x.isSelected = event);
    this.isAllSelected = event;

    if (event) {
      this.targets.forEach(x => this.selectedTargets.set(x.id, true));
    } else {
      this.targets.forEach(x => this.selectedTargets.delete(x.id));
    }
  }

  async deleteSelectedTargets() {
    if (await this.modalService.confirm()) {
      // await this.targets.filter(x => x.isSelected)
      //   .forEach(async (target) => {
      //     this.targetService.deleteTarget(target.id)
      //       .then(res => {
      //         this.selectedTargets.delete(target.id);
      //         let idx = this.targets.findIndex(x => x.id == target.id);
      //         this.targets.splice(idx, 1);
      //         this.toastrService.success(MessageConstant.DeleteSuccess);
      //       });
      //   });
      var ids = this.totalSelected.map(a => a.id);
      this.targetService.deleteMultipleTarget(ids)
      .then(res => {
        this.loadGridData(1).then(() => {
          this.toastrService.success(MessageConstant.DeleteSuccess);
          this.totalSelected = []
          this.selected = []
        });
      });

        
    }
  }
}
