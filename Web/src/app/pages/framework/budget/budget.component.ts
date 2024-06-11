import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FrameworkDynamicColumnService } from 'src/app/services/framework-dynamic-column.service';
import { FrameworkType } from 'src/app/_enums/frameworkType';
import { IFrameworkDynamicColumn } from 'src/app/models/frameworks/framework-dynamic-column.model';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { Globals } from 'src/app/globals';

import { ModalService } from 'src/app/services/modal.service';
import { BudgetService } from 'src/app/services/budget.service';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';


import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { FwColumnEditorComponent } from 'src/app/components/fw-column-editor/fw-column-editor.component';
import { IColumnEditorResult } from 'src/app/models/helpers/column-editor-result';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { PaginationInstance } from 'ngx-pagination';
import { BudgetFrameworkDynamicCell, IBudgetFrameworkDynamicCell, IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { EntityType } from 'src/app/_enums/entityType';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { FrameworkBaseComponent } from '../framework-base.component';
import { IBudgetFramework } from 'src/app/models/frameworks/budget-framework.model';
import { IBudgetDynamicCellInsertModel } from 'src/app/models/frameworks/budget-dynamic-cell-insert.model';
import { ColumnMode, SelectionType } from 'src/app/lib/ngx-datatable';
import { ColumnDataType } from 'src/app/_enums/column-data-type';


@Component({
  selector: 'app-budget',
  templateUrl: './budget.component.html',
  styleUrls: ['./budget.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class BudgetComponent extends FrameworkBaseComponent implements OnInit {

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('budgetDate') budgetDate: any;
  @ViewChild('hdrTplWithContextMenu') hdrTplWithContextMenu: any;
  @ViewChild('customCellTpl') customCellTpl: any;
  @ViewChild('cellView') cellView: any;


  public frameworkType: any;

  public pagedBugets: IPagedResponse<IBudgetFramework>;

  public budgetsWithCells: IBudgetFramework[];


  public paginationConfig: PaginationInstance = {
    id: 'budget_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }


  get budgets(): IBudgetFramework[] {
    return this.pagedBugets ? this.pagedBugets.data : []
  }

  set budgets(budgets: IBudgetFramework[]) {
    this.pagedBugets.data = budgets;
  }

  public isAllSelected: boolean = false;
  public selectedBudgets: Map<number, boolean>;

  get numberOfBudgetsSelected(): number {
    return this.totalSelected.length;
  }

  get selectedBudget(): IBudgetFramework {
    return this.budgets.filter(x => x.isSelected)[0];
  }


  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 0, size: 10 };

  constructor(
    dynamicColumnService: DynamicColumnService,
    private toastrService: ToastrService,
    globals: Globals,
    modalService: ModalService,
    private budgetService: BudgetService
  ) {
    super(dynamicColumnService, globals, modalService)
    this.frameworkType = FrameworkType;
    this.selectedBudgets = new Map();

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
    this.loadDynamicColumns(EntityType.Budget).then(a => {
      this.loadGridData(1);
    });
    //await this.getAllBudgets(1);

  }


  async getAllBudgets(pageNo: number) {
    this.budgetService.getAll({ pageNo: pageNo, pageSize: this.paginationConfig.itemsPerPage })
      .then(x => {
        this.pagedBugets = x;
        this.paginationConfig.totalItems = this.pagedBugets.total;
        this.paginationConfig.currentPage = pageNo;
        this.arrangeCells();
        this.applySelection();
      });
  }

  getBudgetData(pageNo: number) {
    return new Promise<IPagedResponse<IBudgetFramework>>((resolve) => {

      this.budgetService.getAll({ pageNo: pageNo, pageSize: this.paginationConfig.itemsPerPage })
        .then(data => {
          this.pagedBugets = data;

          this.paginationConfig.totalItems = this.pagedBugets.total;
          this.paginationConfig.currentPage = pageNo;

          this.arrangeCells();
          this.applySelection();
          resolve(this.pagedBugets);
        })
    });
  }

  loadGridData(pageNo) {

    return new Promise((resolve) => {
      this.getBudgetData(pageNo).then(data => {
        this.rows = [];
        this.columns = [];

        this.columns = [
          {
            prop: 'startDate'
            , name: "Start Date"
            // , width: 200
            , headerCheckboxable: "true"
            , checkboxable: "true"
            , frozenLeft: "true"
            , cellTemplate: this.budgetDate, headerTemplate: this.hdrTpl
          },
          {
            prop: 'endDate',
            name: "End Date",
            frozenLeft: "true"
            , cellTemplate: this.budgetDate, headerTemplate: this.hdrTpl
          }
          , { prop: "donor", name: "Donor", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "project", name: 'Project', cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "amount", name: "Amount", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }

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
          data['startDate'] = a.startDate;
          data['endDate'] = a.endDate;
          data['donor'] = a.donor.name;
          data['project'] = a.project.name;
          data['amount'] = a.amount;

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
      (DynamicColumnComponent, { entityTypeId: EntityType.Budget, entityColumnId: entityColumnId, title: "Update column.", buttonText: "Update" })
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

      await this.dynamicColumnService.deleteDynamicColumn(entityColumnId,EntityType.Budget);
      this.loadDynamicColumns(EntityType.Budget).then(a => {
        this.loadGridData(1);
      });
      this.toastrService.success(MessageConstant.DeleteSuccess);
    }
  }

  applySelection() {
    this.budgets.forEach(x => {
      if (this.selectedBudgets.has(x.id)) {
        x.isSelected = true;
      }
    })
  }

  onPageSizeChanged(pageSize: number) {
    this.paginationConfig.itemsPerPage = pageSize;
    //this.getAllBudgets(1);
    this.loadDynamicColumns(EntityType.Budget).then(() => {
      this.loadGridData(1).then(() => {

        this.totalSelected = []
        this.selected = []
      });
    })
  }
  async getPage(pageNo: number) {
    await this.getAllBudgets(pageNo);
  }

  arrangeCells() {
    this.pagedBugets.data.forEach(budget => {
      let cells = new Array<IBudgetFrameworkDynamicCell>(this.dynamicColumns.length);
      this.dynamicColumns.forEach((column, index) => {

        let existing = null;
        existing = budget
          .dynamicCells
          .find(x => x.entityDynamicColumnId == column.id);

        if (existing == null) {
          cells[index] = new BudgetFrameworkDynamicCell(budget.id, column.id, [], column.columnName);
        } else {
          cells[index] = existing;
        }
        cells[index].listType = column.listObject;
        cells[index].dataType = column.columnDataType;
      })
      budget.dynamicCells = cells;
    });
  }

  pushEmptyCell(column: IDynamicColumn) {
    this.budgets.forEach(budget => {
      let cell = new BudgetFrameworkDynamicCell(budget.id, column.id, [], column.columnName) as IBudgetFrameworkDynamicCell;
      cell.listType = column.listObject;
      cell.dataType = column.columnDataType;
      budget.dynamicCells.push(cell)
    });
  }

  removeCells(id: number) {
    this.budgets.forEach(budget => {
      budget.dynamicCells.splice(budget.dynamicCells.findIndex(x => x.entityDynamicColumnId == id), 1);
    });
  }

  async onAddNewDynamicColumn() {
    super.addNewDynamicColumn(EntityType.Budget).then(a => {
      this.loadGridData(1);
    });
  }

  async onEditDynamicColumn(selectedColumn: IDynamicColumn) {
    throw new Error("Not yet implemented");
  }



  async deleteDynamicCell(cell: IBudgetFrameworkDynamicCell) {
    if (await this.modalService.confirm()) {
      await this.budgetService.deleteDynamicCell(cell).then(a => {
        this.loadGridData((this.page.pageNumber + 1)).then(() => {
          this.totalSelected = []
          this.selected = []
        });
        this.toastrService.success(MessageConstant.DeleteSuccess);
      })
      // this.getAllBudgets(1);
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

  async addDynamicCell(event, cell: IBudgetFrameworkDynamicCell) {
    this.editDynamicCell(event, cell);
  }
  async editDynamicCell(event, cell: IBudgetFrameworkDynamicCell) {
    var datatableBodyCell = this.getClosest(event.currentTarget, 'datatable-body-cell');
    datatableBodyCell.blur();

    let insertModel: IBudgetDynamicCellInsertModel;

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
            budgetFrameworkId: cell.budgetFrameworkId,
            dynamicCells: [{
              entityDynamicColumnId: cell.entityDynamicColumnId,
              value: dynamicCell.value,
            }]
          };
          this.budgetService.insertDynamicCell(insertModel).then(a => {
            this.toastrService.success(MessageConstant.SaveSuccessful);
            this.loadDynamicColumns(EntityType.Budget).then(() => {
              this.loadGridData((this.page.pageNumber + 1)).then(() => {
                this.totalSelected = []
                this.selected = []
              });
            })

          });
        }
      })
      .then(inserted => {
        // this.getAllBudgets(1);
        // this.toastrService.success(MessageConstant.SaveSuccessful);
      });
  }
  async editFixedCell(budget: IBudgetFramework, columnName: string, propertyName: string) {

    let cell = new BudgetFrameworkDynamicCell(budget.id, 0, budget[propertyName], columnName)
    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, { cell: cell })
      .then(async result => {
        if (result.cell != null && result.cell != undefined) {
          let updateModel = { ...budget }
          updateModel[propertyName] = result.cell.value;
          await this.budgetService.updateBudget(updateModel);
          budget[propertyName] = result.cell.value;
          this.toastrService.success(MessageConstant.SaveSuccessful);
        }
      });
  }


  toggleSelection(user: IBudgetFramework) {
    if (user.isSelected) {
      this.selectedBudgets.set(user.id, true);
    } else {
      this.selectedBudgets.delete(user.id);
    }

    if (this.selectedBudgets.size != this.budgets.length) {
      this.isAllSelected = false;
    } else {
      this.isAllSelected = true;
    }
  }



  selectAll(event) {
    this.budgets.forEach(x => x.isSelected = event);
    this.isAllSelected = event;

    if (event) {
      this.budgets.forEach(x => this.selectedBudgets.set(x.id, true));
    } else {
      this.budgets.forEach(x => this.selectedBudgets.delete(x.id));
    }
  }



  async deleteSelectedBudgets() {
    if (await this.modalService.confirm()) {
      // await this.budgets.filter(x => x.isSelected)
      //   .forEach(async (budget) => {
      //     this.budgetService.deleteBudget(budget.id)
      //       .then(res => {
      //         this.selectedBudgets.delete(budget.id);
      //         let idx = this.budgets.findIndex(x => x.id == budget.id);
      //         this.budgets.splice(idx, 1);
      //         this.toastrService.success(MessageConstant.DeleteSuccess);
      //       });
      //   });

      var ids = this.totalSelected.map(a => a.id);
      this.budgetService.deleteMultipleBudget(ids)
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
