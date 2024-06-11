import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { Globals } from 'src/app/globals';
import { AppUserAuth } from 'src/app/models/auth/AppUserAuth';
import { IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { IDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { IUserDynamicCell, IUserDynamicCellInsertModel } from 'src/app/models/frameworks/user-dynamic-cell.model';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { IRole } from 'src/app/models/role/role.model';
import { IEducationSectorPartner } from 'src/app/models/user/educationSectorPartner';
import { IUser, User } from 'src/app/models/user/user.model';
import { AuthService } from 'src/app/services/auth.service';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { ModalService } from 'src/app/services/modal.service';
import { UserService } from 'src/app/services/user.service';
import { ValidationErrorBuilder } from 'src/app/services/validation-error-builder';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ValidationMessage } from 'src/app/utility/ValidationMessage';
import { EntityType } from 'src/app/_enums/entityType';
import { LevelRank } from 'src/app/_enums/levelRank';
import { PartnerType } from 'src/app/_enums/partnerType';

@Component({
    selector: 'user-dynamic-columns',
    templateUrl: './user-profile.component.html'
})
export class UserProfileComponent implements OnInit{
     

    editing: boolean = false;
    user: IUser = {};
    userBasicInfo: any;
    get dynamicCells(): IDynamicCell[] {
        return this.user && this.user.dynamicCells ? this.user.dynamicCells : [];
    }
    dynamicColumns: IDynamicColumn[]= [];

    constructor(
        private route: ActivatedRoute, 
        private authService: AuthService,
        private userService: UserService,
        private dynamicColumnService: DynamicColumnService,
        private globals: Globals,
        private modalService: ModalService,
        private toastrService: ToastrService,
        public formBuilder: FormBuilder,
        private router: Router
        ){
  
          this.levelRank = LevelRank;
    }
 
    arrangeCells(){
        let cells = new Array<IDynamicCell>(this.dynamicColumns.length);
        this.dynamicColumns.forEach((column, index) => {
          
          let existing = null;        
          existing = this.user
           .dynamicCells
           .find(x => x.entityDynamicColumnId == column.id);             
           if(existing == null){
            cells[index] =  {
              userId: this.user.id,
              entityDynamicColumnId: column.id,
              values: [],
              value: [],
              columnName: column.name
            } as IUserDynamicCell;
  
           }else{
             cells[index] = existing;
           }
           cells[index].listType = column.listObject;
           cells[index].dataType = column.columnDataType;         
        })
        this.user.dynamicCells = cells;  
      }
    loadUser(id: number){        
        if(!id){
            setTimeout(()=> {
                this.userBasicInfo = JSON.parse(JSON.stringify(this.authService.userObject)); 
            },0);                  
        }
        this.userService.getAll({
            pageNo: 1,
            pageSize: 1,
            espIds: [],
            roleIds: [],
            searchText: null,
            userId: id
        }).then(response=> {
            this.user = response.data[0];                        
            this.loadDynamicColumns();            
            this.updateForm(this.user);
        });
    }


    async loadDynamicColumns(){
        return this.dynamicColumnService.getColumns({
            entityType: EntityType.User,
            pageNo: 1,
            pageSize: this.globals.maxPageSize
        })
        .then(response => {
            this.dynamicColumns = response.data.map(x => ({...x, columnName: x.name, columnNameInBangla: x.nameInBangla}));                
            this.arrangeCells();
            
        });
      }


    async deleteDynamicCell(cell: IUserDynamicCell){
        if(await this.modalService.confirm()){
          await this.userService.deleteDynamicCell(cell); 
          
          this.toastrService.success(MessageConstant.DeleteSuccess);
        }
    }

    async editDynamicCell(cell: IUserDynamicCell){        
        this.addDynamicCell(cell);
      }
    

    async addDynamicCell(cell: IUserDynamicCell){    
        let insertModel: IUserDynamicCellInsertModel;

        let column = this.dynamicColumns.find(x => x.id == cell.entityDynamicColumnId);
        this.modalService
        .open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, {cell: cell, column: column})
        .then(async result => {
            
            if(result.isDeleted){
            await this.deleteDynamicCell(cell);
            return;
            }            
            let dynamicCell = result.cell;
            
            if(dynamicCell != null && dynamicCell != undefined)  {                        
            insertModel = {
                userId : cell.userId,                    
                dynamicCells: [{
                entityDynamicColumnId: cell.entityDynamicColumnId,
                value: dynamicCell.value
                }]
            };

            return this.userService.insertDynamicCell(insertModel);
            }
        })
        .then(inserted => {      
            this.loadUser(this.user.id);
            this.toastrService.success(MessageConstant.SaveSuccessful); 
        });
    }



  userForm: FormGroup;
  
  public roles: IRole[] = [];
  public selectedRole: IRole;
  public levelRank: any;
  public espList: IEducationSectorPartner[];

  public validationErrors: any = {};
  public subscriptions : Map<string, Subscription> = new Map();



  ngOnInit() {
    const id = +this.route.snapshot.paramMap.get('id');
    
    this.userService.getRoles().then(response => {
      this.roles = response.data;
      if (this.user != null) {
        this.setSelectedRole(this.user.roleId);
      }
    });
    this.userService.getEspList().then(response => this.espList = response);


    if (id) {     
        
      this.loadUser(id);

      this.userService.getById(id).then(response => {
        
      });
    }

    this.userForm = this.formBuilder.group({
      fullName: ["", [Validators.required, Validators.minLength(3), Validators.maxLength(255)]],
      designationName: ["", [Validators.required]],
      roleId: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      phoneNumber: ["", [ Validators.minLength(11), Validators.maxLength(13)]],
      educationSectorPartnerId: [""],
      programPartnerId: [""],
      implementationPartnerId: [""]
    });

    this.setValidation();
  }

  setValidation(){
    
    let msg = {
      email: {
        email: ValidationMessage.invlaidEmail()
      },
      fullName: {
        minlength: ValidationMessage.minlength(3),
        maxlength: ValidationMessage.maxlength(255),          
      },
      phoneNumber: {
        minlength: ValidationMessage.minlength(11),
        maxlength: ValidationMessage.maxlength(13),
      }
    };    
    
    this.subscriptions = new ValidationErrorBuilder()
      .withGroup(this.userForm)
      .withMessages(msg)      
      .useMessageContainer(this.validationErrors)
      .build();
  }

  updateForm(user?: IUser) {
    this.userForm.patchValue({
      ...user
    });

    this.userForm.get('educationSectorPartnerId').setValue(user.educationSectorPartnerId);
    this.userForm.get('programPartnerId').setValue(user.programPartnerId);
    this.userForm.get('implementationPartnerId').setValue(user.implementationPartnerId);

    this.setSelectedRole(this.user.roleId);
  }


  async onSubmit() {
    if (!this.userForm.valid) {
      return;
    }

    let esp = this.userForm.get('educationSectorPartnerId').value;
    let pp = this.userForm.get('programPartnerId').value;
    let ip = this.userForm.get('implementationPartnerId').value;

    let userModel = User.create(this.userForm.value);
    userModel.eduSectorPartners = [];
    if (esp) {
      userModel.eduSectorPartners.push({ id: esp, partnerType: PartnerType.EducationSectorPartner });
    }
    if (pp) {
      userModel.eduSectorPartners.push({ id: pp, partnerType: PartnerType.ProgramPartner });
    }
    if (ip) {
      userModel.eduSectorPartners.push({ id: ip, partnerType: PartnerType.ImplementationPartner });
    }

    userModel.id = this.user.id;
    await this.userService.updateUser(userModel);
    this.toastrService.success(MessageConstant.SaveSuccessful);
    this.editing = false;
  }

  setSelectedRole(roleId: number) {
    if (this.roles.length) {
      this.onSelectRole(roleId);
    }
  }

  require(controlName: string){
    this.userForm.get(controlName).setValidators([Validators.required]);
    this.userForm.get(controlName).updateValueAndValidity();
  }
  unRequire(controlName: string){
    this.userForm.get(controlName).clearValidators();
    this.userForm.get(controlName).updateValueAndValidity();
    this.userForm.get(controlName).reset();
  }

  onSelectRole(roleId: number) {        
    if(!roleId){
      return;
    }
    this.selectedRole = this.roles.find(r => r.id == roleId);
    if (this.selectedRole.levelRank == LevelRank.ImplementationPartner || this.selectedRole.levelRank == LevelRank.ProgramPartner) {
      this.require('educationSectorPartnerId');
      this.unRequire('programPartnerId');
      this.unRequire('implementationPartnerId');
    }    
    else if (this.selectedRole.levelRank == LevelRank.Teacher) {
      this.unRequire('educationSectorPartnerId');
      this.require('programPartnerId');
      this.require('implementationPartnerId');

    }
    else{
      this.unRequire('educationSectorPartnerId');
      this.unRequire('programPartnerId');
      this.unRequire('implementationPartnerId');
    }
    this.userForm.updateValueAndValidity();
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());    
  }
}