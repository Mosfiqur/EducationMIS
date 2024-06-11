import { IRole, Role } from '../models/role/role.model';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { Injectable } from '@angular/core';
import { ILevel } from '../models/role/level.model';
import { IPermissionPreset } from '../models/role/permission-preset.model';
import { IPermission } from '../models/role/permission.model';
import { IBaseQueryModel } from '../models/queryModels/base-query.model';
import { Globals } from '../globals';

@Injectable({
    providedIn: 'root'
})

export class RoleService {
    api: string = "Role/"    
    constructor(private httpClientService: HttpClientService,
        private globals: Globals
        ){

    }

    async getAll(query: IBaseQueryModel): Promise<IPagedResponse<Role>> {
        return this.httpClientService
        .getAsync(`${ApiConstant.getRoles}?pageNo=${query.pageNo}&pageSize=${query.pageSize}`)
        .then(response => {
            return {
                data: response.data.map(x => new Role(x.roleName, x.levelRank,x.levelId, x.levelName, x.id)),
                pageNo: response.pageNo,
                pageSize: response.pageSize,
                total: response.total
            }
        })        
    }

    async getById(id: number): Promise<Role> {
        return this.httpClientService
        .getAsync(`${ApiConstant.getRoleById}?roleId=${id}`)
        .then(x => new Role(x.roleName, x.levelRank,x.levelId, x.levelName, x.id, x.permissionPresetId, x.permissions));
    }

    async saveRole(role: Partial<Role>) {
        if(role.id)        
            return this.updateRole(role);
        return this.httpClientService.postAsync(ApiConstant.createRole, role);
    }

    async updateRole(role: Partial<Role>) {
        return this.httpClientService.putAsync(ApiConstant.updateRole, role);
    }

    async getLevels(): Promise<ILevel[]> {
        return this.httpClientService.getAsync(ApiConstant.getLevels);
    }

    async getPermissionPresets(): Promise<IPagedResponse<IPermissionPreset>> {
        return this.httpClientService.getAsync(ApiConstant.getPermissionPersets);
    }

    async getPermissionsByPresetId(id: number): Promise<IPermission[]> {
        return this.httpClientService.getAsync(`${ApiConstant.getPermissionsByPersetId}?id=${id}`);
    }

    async getAllPermissions(): Promise<IPagedResponse<IPermission>> {
        return this.httpClientService.getAsync(`${ApiConstant.getPermissions}?pageNo=1&pageSize=${this.globals.maxPageSize}`);
    }
    
}