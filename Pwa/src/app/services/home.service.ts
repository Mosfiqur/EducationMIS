import { Injectable } from '@angular/core';
import { User } from '../models/idbmodels/indexedDBModels';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  userInfo: User;

  constructor() { 

  }

  getUserInfo():Promise<User>{  
    return new Promise<User>((resolve, reject)=>{
      const user = new User(); 
      //TODO: 
      resolve(user);
    });
  }
}
