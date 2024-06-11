import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './httpClientService';
import { AppUserAuth } from '../models/auth/AppUserAuth';
import { AppUser } from '../models/auth/AppUser';
import { ApiConstant } from '../utility/ApiConstant';

import { Router } from '@angular/router';
import { ILoginResponse } from '../models/auth/login-response.model';
import { NgxIndexedDBService } from 'ngx-indexed-db';
import { User } from '../models/idbmodels/indexedDBModels';
import { UserDB } from '../localdb/UserDB';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  userObject: User;

  constructor(private httpClientService: HttpClientService, private router: Router, private userDb: UserDB) {
    this.userObject = new User();
  }


  login(entity: AppUser): Promise<User> {
    // Initialize security object
    // this.resetSecurityObject();
    return new Promise((resolve, reject) => {
      this.httpClientService.postAsync(ApiConstant.Login, entity)
        .then((data: ILoginResponse) => {
          const userProfile = data.userProfile;
          var user = new User();
          user.id = userProfile.id;
          user.token = data.token;
          user.designationName = userProfile.designationName;
          user.email = userProfile.email;
          user.fullName = userProfile.fullName;
          user.levelName = userProfile.levelName;
          user.phoneNumber = userProfile.phoneNumber;
          user.roleName = userProfile.roleName;
          userProfile.bearerToken = data.token;
          Object.assign(this.userObject, user);
          localStorage.setItem("bearerToken",this.userObject.token);
          localStorage.setItem("userProfile", JSON.stringify(this.userObject));
          resolve(user);
        }, (err) => {
          reject(err);
        });
    });
  }

  logUserInOnRefresh() {
    const token = localStorage.getItem('bearerToken');
    if (token) {
      const token = localStorage.getItem('bearerToken');
      if (token) {
        let obj = localStorage.getItem('userProfile');
        if (obj != null && obj != undefined) {
          this.userObject = JSON.parse(obj);
        }
        this.userObject.token = token;
      }
    }
  }

  logout(): void {
    localStorage.removeItem("bearerToken");
    localStorage.removeItem("userProfile");

    this.userDb.deleteUser().subscribe((res) => {
      if (res) {
        this.router.navigate(['unicefpwa/authentication']);
      }
    }, (err) => {
      console.log('Failed to delete user: ', err);
    })
  }
}
