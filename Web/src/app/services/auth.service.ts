import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './httpClientService';
import { AppUserAuth } from '../models/auth/AppUserAuth';
import { AppUser } from '../models/auth/AppUser';
import { ApiConstant } from '../utility/ApiConstant';

import { Router } from '@angular/router';
import { ILoginResponse } from '../models/auth/login-response.model';
import { IPasswordChangeModel } from '../models/user/passwordResetModel';
import { IForgotPasswordModel } from '../models/auth/forgot-password.model';
import { ITokenValidationResult } from '../models/auth/token-validation-result.model';
import { IPasswordResetModel } from '../models/auth/password-reset.model';
import { CacheConstant } from '../utility/CacheConstant';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userObject: AppUserAuth = new AppUserAuth();

  constructor(private httpClientService: HttpClientService, private router: Router) { }


  login(entity: AppUser): Promise<AppUserAuth> {
    // Initialize security object
    this.resetSecurityObject();
    return new Promise((resolve, reject) => {
      this.httpClientService.postAsync(ApiConstant.Login, entity)
        .then((data: ILoginResponse) => {

          var responseData={
            bearerToken:data.token,
            isAuthenticated:true,
            fullName: data.userProfile.fullName,
            roleId: data.userProfile.roleId,
            roleName: data.userProfile.roleName,
            id: data.userProfile.id,
            email:data.userProfile.email,
            phoneNumber: data.userProfile.phoneNumber,
            designationName:data.userProfile.designationName            
          }
          Object.assign(this.userObject, responseData);
          // Store into local storage
          localStorage.setItem("bearerToken",this.userObject.bearerToken);
          // todo: Fix this mess after release !!
          localStorage.setItem("userProfile", JSON.stringify(this.userObject));
          resolve(this.userObject);

          this.getPermissions();
        }, (err) => {
          reject(err);
        });
    });
  }
 
  logout(): void {
    this.resetSecurityObject();
    this.router.navigate(['unicef/authentication']);
  }

  logUserInOnRefresh(){
    const token = localStorage.getItem('bearerToken');
    if(token){
      const token = localStorage.getItem('bearerToken');      
      if(token){
        let obj = localStorage.getItem('userProfile');
        if(obj != null &&  obj != undefined){
          this.userObject = JSON.parse(obj);
        }
        
        this.userObject.isAuthenticated = true;
        this.userObject.bearerToken = token;      
        this.getPermissions();
      }
    }
  }

  getPermissions(){
    this.httpClientService.getAsync(ApiConstant.baseUrl + "Auth/GetOwnPermissions")
    .then(permissions => this.userObject.permissions = permissions);
  }

  resetSecurityObject(): void {
    this.userObject.bearerToken = "";
    this.userObject.isAuthenticated = false;
    this.userObject.fullName = "";
    this.userObject.levelName = "";
    this.userObject.roleName = "";
    this.userObject.email = "";
    this.userObject.phoneNumber = "";

    localStorage.removeItem("bearerToken");    
    localStorage.removeItem(CacheConstant.BeneficiaryGrid);    
    localStorage.removeItem(CacheConstant.BeneficiaryVersionDataGrid);    
    localStorage.removeItem(CacheConstant.FacilityGrid);    
    localStorage.removeItem(CacheConstant.FacilityVersionDataGrid);    

  }

  
  getToken(){
    const token = localStorage.getItem('bearerToken');
    return token;
  }


  async changePassword(model: IPasswordChangeModel) {           
    return this.httpClientService.postAsync(ApiConstant.changePassword, model);
  }

  async requestPasswordResetMail(model: IForgotPasswordModel){
    return this.httpClientService.postAsync(ApiConstant.requestPasswordResetMail, model);
  }

  async validateToken(token: string): Promise<ITokenValidationResult>{    
    return this.httpClientService.getAsync(ApiConstant.validatePasswordResetToken + "/?token="+token);
  }

  async resetUserPassword(model: IPasswordResetModel):Promise<any> {
    return this.httpClientService.postAsync(ApiConstant.resetOwnPassword, model);
  }

  hasPermission(permission: string): boolean{
    return !!this.userObject.permissions.find(x=> x == permission);
  }
}
