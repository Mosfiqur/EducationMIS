import { Component, OnInit } from '@angular/core';
import { Role, IRole } from 'src/app/models/role/role.model';
import { IDesignation } from 'src/app/models/user/designation';
import { IEducationSectorPartner } from 'src/app/models/user/educationSectorPartner';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LevelRank } from 'src/app/_enums/levelRank';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { RoleService } from 'src/app/services/role.service';
import { FormGroup, FormBuilder, Validators, FormArray, FormControl } from '@angular/forms';
import { IPermissionPreset } from 'src/app/models/role/permission-preset.model';
import { IPermission } from 'src/app/models/role/permission.model';
import { ILevel } from 'src/app/models/role/level.model';

@Component({
  selector: 'app-roles-new',
  templateUrl: './roles-new.component.html',
  styleUrls: ['./roles-new.component.scss']
})
export class RolesNewComponent implements OnInit {
  isGroupWiseSelected: boolean = false;
  permissionsWithGroups: IPermission[] = [];
  roleForm: FormGroup;
  role: Role = null;

  levels: ILevel[] = [];
  presets: IPermissionPreset[] = [];
  permissions: IPermission[] = [];
  currentPreset: IPermission[];
  editState: boolean = false;


  isAllSelected: boolean = false;

  get permissionsFormArray() {
    return this.roleForm.controls.permissions as FormArray;
  }
  set permissionsFormArray(array: FormArray) {
    this.roleForm.controls.permissions = array;
  }


  get isAllPermissionSelected(): boolean {
    return this.permissionsFormArray.controls.filter(x => x.value).length == this.permissions.length;
  }


  constructor(
    private roleService: RoleService,
    private formBuilder: FormBuilder,
    private router: Router,
    private toastrService: ToastrService,
    private route: ActivatedRoute
  ) {
  }

  async ngOnInit() {
    this.buildForm();
    await this.getAllPermissions();

    const id = +this.route.snapshot.paramMap.get('id');
    this.roleService.getLevels().then(res => {
      this.levels = res;
      console.log(this.levels);
    });
    this.roleService.getPermissionPresets().then(res => {
      this.presets = res.data
      console.log(this.presets);
    });

    this.permissionsFormArray = new FormArray([]);
    this.permissions.forEach(x => this.permissionsFormArray.push(new FormControl(false)));

    if (id) {
      this.editState = true;
      this.roleService.getById(id).then(res => {
        this.role = res;
        this.roleForm.patchValue({
          ...this.role
        });
        return this.roleService.getPermissionsByPresetId(this.role.permissionPresetId);
      })
        .then(res => {
          this.currentPreset = res;
          this.selectPermissions(this.role.permissions);
          this.roleForm.get('isAllSelected').setValue(this.isAllPermissionSelected);
        })
    }


  }

  buildForm() {
    this.roleForm = this.formBuilder.group({
      id: [""],
      roleName: ["", [Validators.required, Validators.minLength(3)]],
      levelId: ["", [Validators.required]],
      permissionPresetId: ["", Validators.required],
      isAllSelected: [false],
      permissions: new FormArray([])
    });
  }

  updateForm(role?: Role) {
    this.roleForm.patchValue({
      ...role
    });
  }

  async getAllPermissions() {
    this.permissions = await (await this.roleService.getAllPermissions()).data;

    var allGroup = new Set(this.permissions.map(item => item.group));
    // console.log(allGroup);
    var permissionOfGroup : IPermission[] = [];
    
    allGroup.forEach(eachGroup => {
      permissionOfGroup = this.permissions.filter(x => x.group === eachGroup);
      permissionOfGroup.forEach(x => {x.isGroup = false; x.isSelected = false; });
      permissionOfGroup.unshift({id:1,group:eachGroup,description:eachGroup,isSelected:false,permissionName:"",isGroup:true});

      this.permissionsWithGroups.push(...permissionOfGroup);
    });
    this.permissions = this.permissionsWithGroups;
    console.log(this.permissions);
  }

  async onSubmit() {
    if (!this.roleForm.valid) {
      return;
    }

    let permissions = this.permissionsFormArray.controls
      .map((control, index) => control.value ? this.permissions[index] : null)
      .filter(x => x != null && x.isGroup != true);

    let roleModel = Role.createFrom(this.roleForm.value);
    roleModel.permissions = permissions;
    await this.roleService.saveRole(roleModel);
    this.toastrService.success(MessageConstant.SaveSuccessful);
    this.router.navigate(['unicef/users/roles']);
  }

  async onSelectPreset() {
    let id = +this.roleForm.get('permissionPresetId').value;
    await this.roleService.getPermissionsByPresetId(id).then(res => {
      this.currentPreset = res;
      this.selectPermissions(this.currentPreset);
    });
  }

  onSelectAllPermission() {
    let value = this.roleForm.get('isAllSelected').value;
    this.permissionsFormArray = new FormArray([]);
    this.permissions.forEach(x => this.permissionsFormArray.push(new FormControl(!value)))
  }

  onSelectPermissionAllGroupWiseSet(groupSelected:IPermission) {

    let isGroupSelect = !groupSelected.isSelected;
    this.permissions.filter(x => x.group === groupSelected.group).forEach(x => {
      x.isSelected = isGroupSelect;
    });
    console.log(this.permissions);

    this.permissionsFormArray = new FormArray([]);
    this.permissions.forEach(x => {
      this.permissionsFormArray.push(new FormControl(x.isSelected));
    });
  }

  selectPermissions(currentPreset: IPermission[]) {
    
    this.permissionsFormArray = new FormArray([])
    this.permissions.forEach(x => {
      let inPreset = currentPreset.findIndex(p => p.id == x.id) != -1;
      x.isSelected = inPreset ? true : false;
      if(x.isGroup === true){
        let groupPresetPermission = currentPreset.filter(preset => preset.group === x.group);
        let groupPermission = this.permissions.filter(elem => elem.group === x.group && elem.isGroup === false);
        let presetOfPermission = groupPresetPermission.length === groupPermission.length;
        this.permissionsFormArray.push(presetOfPermission ? new FormControl(true) : new FormControl(false));
      }
      else{
        this.permissionsFormArray.push(inPreset ? new FormControl(true) : new FormControl(false))
      }
    });
    
    this.toggleSelection();
  }

  toggleSelection() {
    this.roleForm.get('isAllSelected').setValue(this.isAllPermissionSelected);
  }
}
