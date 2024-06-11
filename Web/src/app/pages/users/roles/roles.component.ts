import { Component, OnInit } from '@angular/core';
import { IRole, Role } from 'src/app/models/role/role.model';
import { RoleService } from 'src/app/services/role.service';
import { PaginationInstance } from 'ngx-pagination';
import { getRtlScrollAxisType } from '@angular/cdk/platform';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';

@Component({
  selector: 'app-roles',
  templateUrl: './roles.component.html',
  styleUrls: ['./roles.component.scss']
})
export class RolesComponent implements OnInit {

  roles : Role[] = [];
  public pagedResponse: IPagedResponse<Role>;
  isAllSelected: boolean = false;
  selectedRoles : Map<number, boolean>;

  get selectedRole(): Role{    
    return this.roles.filter(x => x.isSelected)[0];
  }
  
  get roleSelected():boolean{
    return this.selectedRoles.size > 0;
  }
  
  get numberOfRolesSelected():number {    
    return this.selectedRoles.size;
  }

  
  get disableOnMultiSelection():boolean{
    return this.selectedRoles.size > 1;
  }

  
  pageConfig: PaginationInstance  = {
    id: 'role_list',
    itemsPerPage: 10,
    currentPage: 1,
    totalItems: 0
  }



  constructor(private roleService: RoleService) { 
    this.selectedRoles = new Map();
  }

  async ngOnInit() {
    await this.getRoles(this.pageConfig.currentPage);    
  }

  async getRoles(pageNo: number){
    this.pagedResponse = await (await this.roleService
      .getAll({pageNo: pageNo, pageSize: this.pageConfig.itemsPerPage}));
    this.roles = this.pagedResponse.data;
    this.pageConfig.currentPage = pageNo;
    this.pageConfig.totalItems = this.pagedResponse.total;
    this.applySelection();
  }

  applySelection(){
    this.roles.forEach(x=> {
      if(this.selectedRoles.has(x.id)){
        x.isSelected = true;
      }
    })
  }

  onPageSizeChanged(pageSize: number){
    this.pageConfig.itemsPerPage = pageSize;
    this.getRoles(1);
  }
  
  selectAll(event){        
    this.roles.forEach(x=> x.isSelected = event);
    this.isAllSelected = event;
        
    if(event){
      this.roles.forEach(x => this.selectedRoles.set(x.id, true));
    }else{
      this.roles.forEach(x => this.selectedRoles.delete(x.id));
    }    
  } 

  toggleSelection(user: Role){
    if(user.isSelected){
      this.selectedRoles.set(user.id, true);
    }else{
      this.selectedRoles.delete(user.id);
    }    

    if(this.selectedRoles.size != this.roles.length){
      this.isAllSelected = false;
    }else{
      this.isAllSelected = true;
    }
  }

  async getPage(pageNo: any){
    await this.getRoles(pageNo);        
  }
}
