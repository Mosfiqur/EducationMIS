import { Injectable } from '@angular/core';
import { HttpClientService } from './httpClientService';
import { ApiConstant } from '../utility/ApiConstant';
import { IPagedResponse } from '../models/responseModels/pagedResponseModel';
import { IUser, User } from '../models/user/user.model';
import { IDesignation } from '../models/user/designation';
import { IRole } from '../models/role/role.model';
import { IEducationSectorPartner } from '../models/user/educationSectorPartner';
import { IPasswordResetModel } from '../models/user/passwordResetModel';
import { IUserQueryModel } from '../models/queryModels/user-query.model';
import { IUserDynamicCell, IUserDynamicCellInsertModel } from '../models/frameworks/user-dynamic-cell.model';
import { UserEndpoints } from '../utility/endpoints/user';

@Injectable({
    providedIn: 'root'
})

export class UserService {
    
    api: string = "user/"
    pagedUsers: IPagedResponse<User>;
    constructor(
        private httpClientService: HttpClientService,
        private endpoints: UserEndpoints
        ){

    }

    async getAll(query: IUserQueryModel): Promise<IPagedResponse<User>> {
        return this.httpClientService.postAsync(ApiConstant.getUsers, query)
        .then(res => {            
            res.data = res.data.map(
                    (x: IUser) => new User(
                        x.fullName, 
                        x.designationId, 
                        x.designationName, 
                        x.roleId, 
                        x.roleName, 
                        x.email, 
                        x.phoneNumber, 
                        x.eduSectorPartners, 
                        x.id, 
                        x.dynamicCells)
                        );
            this.pagedUsers = res;    
            return res;
        });        
    }

    async getById(id: number): Promise<User> {
        return this.httpClientService.getAsync(`${ApiConstant.getUserById}?userId=${id}`)        
        .then(x => new User(x.fullName, x.designationId, x.designationName, x.roleId, x.roleName, x.email, x.phoneNumber, x.eduSectorPartners, x.id));        
    }

    async getDesignations(): Promise<IPagedResponse<IDesignation>> {
        return this.httpClientService.getAsync(ApiConstant.getDesignations);
    }

    async getRoles(): Promise<IPagedResponse<IRole>> {
        return this.httpClientService.getAsync(ApiConstant.getRoles);
    }

    async createUser(model: Partial<User>) {           
        return this.httpClientService.postAsync(ApiConstant.createUser, model);
    }

    async resetPassword(model: IPasswordResetModel) {           
        return this.httpClientService.postAsync(ApiConstant.resetPassword, model);
    }

    async updateUser(model: Partial<User>) {           
        return this.httpClientService.putAsync(ApiConstant.updateUser, model);
    }

    async updateProfileInfo(model: Partial<User>) {           
        return this.httpClientService.putAsync(ApiConstant.updateProfileInfo, model);
    }

    async deleteUser(userId: number) {           
        return this.httpClientService.deleteAsync(`${ApiConstant.deleteUser}?userId=${userId}`);        
    }
    async deleteUsers(userIds: number[]) {           
        return this.httpClientService.postAsync(`${ApiConstant.deleteUsers}`,userIds);        
    }

    async getEspList(): Promise<IEducationSectorPartner[]> {
        return this.httpClientService.getAsync(ApiConstant.getEspList);
    }

    async deleteDynamicCell(cell: IUserDynamicCell): Promise<any> {
        return this.httpClientService.postAsync(this.endpoints.deleteDynamicCell, cell);
    }
    async insertDynamicCell(insertModel: IUserDynamicCellInsertModel): Promise<any> {        
        return this.httpClientService.postAsync(this.endpoints.insertDynamicCell, insertModel);
    }

    exportFiltered (query: IUserQueryModel) {
        return this.httpClientService.download(this.endpoints.exportFiltered, query);
    }
      
}