import { Component, OnInit, ViewChild, ViewEncapsulation, ChangeDetectorRef } from '@angular/core';

import { UserService } from 'src/app/services/user.service';
import { IUser, User } from 'src/app/models/user/user.model';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { PaginationInstance } from 'ngx-pagination/dist/pagination-instance';
import { ModalService } from 'src/app/services/modal.service';
import { ResetUserPasswordComponent } from 'src/app/shared/components/reset-password/reset-user-password.component';
import { IUserPasswordResetModel } from 'src/app/models/user/passwordResetModel';
import { IUserQueryModel } from 'src/app/models/queryModels/user-query.model';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { Globals } from 'src/app/globals';

import { EducationPartnerService } from 'src/app/services/educationPartner.service';
import { IDropdownSettings } from 'src/app/lib/multi-select';
import { EntityType } from 'src/app/_enums/entityType';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';

import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { IUserDynamicCell, IUserDynamicCellInsertModel } from 'src/app/models/frameworks/user-dynamic-cell.model';
import { ColumnMode, SelectionType } from 'src/app/lib/ngx-datatable';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { AuthService } from 'src/app/services/auth.service';
import { AppPermissions } from 'src/app/core/app-permissions';
import { IEducationSectorPartner } from 'src/app/models/user/educationSectorPartner';
import { PartnerType } from 'src/app/_enums/partnerType';
import { ColumnDataType } from 'src/app/_enums/column-data-type';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {
  public users: IUser[] = [];
  public pagedUsers: IPagedResponse<User>;


  @ViewChild('cellView') cellView: any;
  @ViewChild('userName') userNameView: any;

  @ViewChild('hdrTpl') hdrTpl: any;
  @ViewChild('hdrTplWithContextMenu') hdrTplWithContextMenu: any;
  @ViewChild('customCellTpl') customCellTpl: any;


  public dynamicColumns: IDynamicColumn[] = [];

  public newPassword: string;
  public newPasswordEmpty: boolean = false;

  private _userSearchText: string;
  private userSearchText$: Subject<string> = new Subject<string>();

  public pageConfig: PaginationInstance = {
    id: 'user_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }

  public isAllSelected: boolean = false;
  public selectedUsers: Map<number, boolean>;

  get numberOfUsersSelected(): number {
    return this.totalSelected.length;
  }

  get userSelected(): boolean {
    return this.totalSelected.length > 0;
  }

  get disableOnMultiSelection(): boolean {
    return this.totalSelected.length > 1;
  }

  get selectedUser(): IUser {
    return this.users.filter(x => x.isSelected)[0];
  }

  get userSearchText(): string {
    return this._userSearchText;
  }
  set userSearchText(val: string) {
    this.userSearchText$.next(val);
  }

  public dropdownSettings: IDropdownSettings = {};
  public espList: ISelectListItem[] = [];

  get canUpdateProfile(): boolean {
    return this.authService.hasPermission(AppPermissions.UpdateUserProfile);
  }

  public roles: ISelectListItem[] = [];
  @ViewChild('closeDeleteModal') closeDeleteModal;
  @ViewChild('closeResetPasswordModal') closeResetPasswordModal;


  private selectedEsps: ISelectListItem[] = [];
  private selectedRoles: ISelectListItem[] = [];
  public filters = {
    selectedRoles: this.selectedRoles,
    selectedEsps: this.selectedEsps
  }

  public entityTypeEnum: any;


  public totalSelected = [];

  public rows: any[];
  public columns: any[];
  public selected = [];
  public ColumnMode = ColumnMode;
  public SelectionType = SelectionType;

  public page = { totalElements: 0, pageNumber: 0, size: 10 };


  constructor(
    private userService: UserService,
    private toastrService: ToastrService,
    private modalService: ModalService,
    private globals: Globals,
    private espService: EducationPartnerService,
    private dynamicColumnService: DynamicColumnService,
    private authService: AuthService) {

    this.selectedUsers = new Map();
    this.entityTypeEnum = EntityType;
    this.userSearchText$.pipe(
      debounceTime(this.globals.searchDebounce),
      distinctUntilChanged()
    ).subscribe(text => {
      this._userSearchText = text;
      //this.getUsers(1);
      this.loadDynamicColumns().then(() => {
        this.loadGridData(1).then(() => {
          this.totalSelected = []
          this.selected = []
        });
      })

    });

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
    await this.loadDynamicColumns().then(a => {
      this.loadGridData(1);
    });
    //  await this.getUsers(this.pageConfig.currentPage);


    this.dropdownSettings = {
      ...this.globals.multiSelectSettings,
      enableCheckAll: false
    } as IDropdownSettings;

    this.userService.getRoles()
      .then(response => {
        this.roles = response.data.map(role => ({ id: role.id, text: role.roleName }));
      });
    this.espService.getAll().then(res => {
      this.espList = res.map(esp => ({ id: esp.id, text: esp.partnerName }))
    });


  }
  loadGridData(pageNo) {

    return new Promise((resolve) => {
      this.getUsersData(pageNo).then(data => {
        this.rows = [];
        this.columns = [];

        this.columns = [
          {
            prop: 'fullName'
            , name: "Name"
            , width: 200
            , headerCheckboxable: "true"
            , checkboxable: "true"
            , frozenLeft: "true"
            , cellTemplate: this.userNameView
            , headerTemplate: this.hdrTpl
          },
          {
            prop: 'designationName',
            name: "Designation",
            frozenLeft: "true"
            , cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl
          }
          // , { prop: "espNames", name: "ESP" }
          , { prop: "ppName", name: "Programming Partner", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "ipName", name: "Implementation Partner", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }

          , { prop: "email", name: 'Email', cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }
          , { prop: "roleName", name: "Role", cellTemplate: this.customCellTpl, headerTemplate: this.hdrTpl }

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
          data['fullName'] = a.fullName;
          data['designationName'] = a.designationName;
          data['espNames'] = a.espNames;
          data["ppName"] = this.getPartnerName(a.eduSectorPartners, PartnerType.ProgramPartner);
          data["ipName"] = this.getPartnerName(a.eduSectorPartners, PartnerType.ImplementationPartner);
          data['email'] = a.email;
          data['roleName'] = a.roleName;

          this.rows.push(data);
        })

        this.page.totalElements = data.total;
        this.page.pageNumber = (pageNo - 1);

        this.pageConfig.totalItems = data.total;
        resolve(true);

      })
    });
  }

  getPartnerName(espList: IEducationSectorPartner[], partnerType: PartnerType) {
    var name = "";

    if (espList.length > 0 && partnerType == PartnerType.ProgramPartner) {
      name = espList.filter(a => a.partnerType == partnerType || a.partnerType == PartnerType.EducationSectorPartner)[0].partnerName
    }
    else if (espList.length > 0 && partnerType == PartnerType.ImplementationPartner) {
      name = espList.filter(a => a.partnerType == partnerType || a.partnerType == PartnerType.EducationSectorPartner)[0].partnerName
    }

    return name;
  }

  async getUsers(pageNo: number) {
    let query = {
      ...this.getFilters(),
      pageNo: pageNo
    } as IUserQueryModel;

    this.pagedUsers = await this.userService.getAll(query);
    this.users = this.pagedUsers.data
    this.pageConfig.totalItems = this.pagedUsers.total;
    this.pageConfig.currentPage = pageNo;

    this.arrangeCells();

    this.applySelection();
  }

  getUsersData(pageNo: number) {
    return new Promise<IPagedResponse<User>>((resolve) => {
      let query = {
        ...this.getFilters(),
        pageNo: pageNo
      } as IUserQueryModel;

      //debugger;
      this.userService.getAll(query).then(data => {
        this.pagedUsers = data;
        this.users = this.pagedUsers.data
        this.pageConfig.totalItems = this.pagedUsers.total;
        this.pageConfig.currentPage = pageNo;

        this.arrangeCells();
        this.applySelection();
        resolve(this.pagedUsers);
      })
    });
  }
  openDynamicColumnModalForEdit(entityColumnId) {
    this.modalService.open<DynamicColumnComponent, DynamicColumnSaveViewModel>
      (DynamicColumnComponent, { entityTypeId: EntityType.User, entityColumnId: entityColumnId, title: "Update column.", buttonText: "Update" })
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

      await this.dynamicColumnService.deleteDynamicColumn(entityColumnId, EntityType.User);
      await this.loadDynamicColumns().then(a => {
        this.loadGridData(1);
      });
      await this.getUsers(this.pageConfig.currentPage);
      this.toastrService.success(MessageConstant.DeleteSuccess);
    }
  }

  getFilters() {
    return {
      pageNo: this.pageConfig.currentPage,
      pageSize: this.pageConfig.itemsPerPage,
      searchText: this.userSearchText,
      espIds: this.filters.selectedEsps.map(x => x.id),
      roleIds: this.filters.selectedRoles.map(x => x.id)
    };
  }
  applySelection() {
    this.users.forEach(user => user.isSelected = this.selectedUsers.has(user.id))
  }

  applyFilters() {
    // this.getUsers(1);
    this.loadGridData(1).then(() => {
      this.totalSelected = []
      this.selected = []
    });
  }

  clearFilters() {
    this.filters.selectedEsps = [];
    this.filters.selectedRoles = [];
    this.applyFilters();
  }

  onPageSizeChanged(pageSize: number) {
    this.pageConfig.itemsPerPage = pageSize;
    //this.getUsers(1);
    this.loadDynamicColumns().then(() => {
      this.loadGridData(1).then(() => {
        this.totalSelected = []
        this.selected = []
      });
    })
  }

  selectAll(event) {
    this.users.forEach(x => x.isSelected = event);
    this.isAllSelected = event;

    if (event) {
      this.users.forEach(x => this.selectedUsers.set(x.id, true));
    } else {
      this.users.forEach(x => this.selectedUsers.delete(x.id));
    }
  }

  toggleSelection(user: User) {
    if (user.isSelected) {
      this.selectedUsers.set(user.id, true);
    } else {
      this.selectedUsers.delete(user.id);
    }

    if (this.selectedUsers.size != this.users.length) {
      this.isAllSelected = false;
    } else {
      this.isAllSelected = true;
    }
  }

  async resetPassword(model: IUserPasswordResetModel) {
    return this.userService.resetPassword({
      userId: this.totalSelected[0].id,
      newPassword: model.newPassword
    }).then(() => {
    }).then(x => {
    }).then(() => {
      this.toastrService.success(MessageConstant.PasswordResetSuccess);
    })
  }

  deleteUser() {
    // this.users.filter(x => x.isSelected).forEach(x => {
    //   this.doDelete(x.id);
    //   this.selectedUsers.delete(x.id);
    // });
    var ids = this.totalSelected.map(a => a.id);
    this.userService.deleteUsers(ids)
      .then(() => {
        this.closeDeleteModal.nativeElement.click();
        this.loadDynamicColumns().then(() => {
          this.loadGridData(1).then(() => {
            this.toastrService.success(MessageConstant.DeleteSuccess);
            this.totalSelected = []
            this.selected = []
          });
        })

      })
      .catch(() => {
        this.closeDeleteModal.nativeElement.click();
      });

  }

  doDelete(id: number) {
    this.userService.deleteUser(id)
      .then(() => {
        this.closeDeleteModal.nativeElement.click();
        let idx = this.users.findIndex(x => x.id == this.selectedUser.id);
        this.users.splice(idx, 1);
        this.toastrService.success(MessageConstant.DeleteSuccess);
      })
      .catch(() => {
        this.closeDeleteModal.nativeElement.click();
      });

  }

  async getPage(pageNo: any) {
    await this.getUsers(pageNo);
  }

  onResetPassword() {
    this.modalService.open<ResetUserPasswordComponent, IUserPasswordResetModel>(ResetUserPasswordComponent)
      .then(async (model: IUserPasswordResetModel) => {
        if (model) {
          this.resetPassword(model);
        }
      })
      .catch(x => x);
  }

  async onColumnAdded(event) {
    // await this.loadDynamicColumns();
    // this.pushEmptyCell(column);
    this.loadDynamicColumns().then(() => {
      this.loadGridData(1).then(() => {
        this.totalSelected = []
        this.selected = []
      });
    })
  }

  async loadDynamicColumns() {

    return this.dynamicColumnService.getColumns({
      entityType: EntityType.User,
      pageNo: 1,
      pageSize: this.globals.maxPageSize
    })
      .then(response => {
        this.dynamicColumns = response.data.map(x => ({ ...x, columnName: x.name, columnNameInBangla: x.nameInBangla }));
      });
  }


  arrangeCells() {
    this.pagedUsers.data.forEach((user: IUser) => {
      let cells = new Array<IDynamicCell>(this.dynamicColumns.length);
      this.dynamicColumns.forEach((column, index) => {

        let existing = null;
        existing = user
          .dynamicCells
          .find(x => x.entityDynamicColumnId == column.id);
        if (existing == null) {
          cells[index] = {
            userId: user.id,
            entityDynamicColumnId: column.id,
            values: [],
            value: [],
            columnName: column.name
          } as IUserDynamicCell;

        } else {
          cells[index] = existing;
        }
        cells[index].listType = column.listObject;
        cells[index].dataType = column.columnDataType;
      })
      user.dynamicCells = cells;
    });
  }

  pushEmptyCell(column: IDynamicColumn) {
    this.users.forEach((user: IUser) => {
      let cell = {
        userId: user.id,
        entityDynamicColumnId: column.id,
        values: [],
        columnName: column.name
      } as IUserDynamicCell;
      cell.listType = column.listObject;
      cell.dataType = column.columnDataType;
      user.dynamicCells.push(cell)
    });
  }


  removeCells(id: number) {
    this.users.forEach(user => {
      user.dynamicCells.splice(user.dynamicCells.findIndex(x => x.entityDynamicColumnId == id), 1);
    });
  }


  async editDynamicCell(event, cell: IUserDynamicCell) {
    this.addDynamicCell(event, cell);

  }

  async deleteDynamicCell(cell: IUserDynamicCell) {
    if (await this.modalService.confirm()) {
      await this.userService.deleteDynamicCell(cell).then(a => {
        this.loadGridData((this.page.pageNumber + 1)).then(() => {
          this.totalSelected = []
          this.selected = []
        });
        this.toastrService.success(MessageConstant.DeleteSuccess);
      });
      // this.getUsers(1);

    }
  }
  getClosest(elem, selector) {

    // Get the closest matching element
    for (; elem && elem !== document; elem = elem.parentNode) {
      if (elem.matches(selector)) return elem;
    }
    return null;

  };

  async addDynamicCell(event, cell: IUserDynamicCell) {
    var datatableBodyCell = this.getClosest(event.currentTarget, 'datatable-body-cell');

    datatableBodyCell.blur();
    // event.currentTarget.parentElement.parentElement.blur()

    let insertModel: IUserDynamicCellInsertModel;

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
            userId: cell.userId,
            dynamicCells: [{
              entityDynamicColumnId: cell.entityDynamicColumnId,
              value: dynamicCell.value
            }]
          };

          this.userService.insertDynamicCell(insertModel).then(() => {
            this.toastrService.success(MessageConstant.SaveSuccessful);
            this.loadDynamicColumns().then(() => {
              this.loadGridData((this.page.pageNumber + 1)).then(() => {
                this.totalSelected = []
                this.selected = []
              });
            })
          });
        }
      })
      .then(() => {
        //this.getUsers(1);
        // this.toastrService.success(MessageConstant.SaveSuccessful);

        // this.loadDynamicColumns().then(a => {
        //   this.loadGridData(1).then(a => {
        //     this.totalSelected = []
        //     this.selected = []
        //   });
        // })

      });

  }

  confirmDeleteUsers() {
    this.modalService.confirm()
      .then(x => {
        if (x) {
          this.deleteUser();
        }
      })
  }

  deleselctEsp(esp: ISelectListItem) {
    this.filters.selectedEsps = this.filters.selectedEsps.filter(x => x.id != esp.id);
    // this.getUsers(1)
    this.loadGridData(1).then(() => {
      this.totalSelected = []
      this.selected = []
    });
  }

  deleselctRole(role: ISelectListItem) {
    this.filters.selectedRoles = this.filters.selectedRoles.filter(x => x.id != role.id);
    //this.getUsers(1)
    this.loadGridData(1).then(() => {
      this.totalSelected = []
      this.selected = []
    });
  }

  export() {
    this.userService.exportFiltered(this.getFilters());
  }
}
