import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpRequest } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LoadingSpinnerService {
  private requests: Map<any, any> = new Map();  
  public isLoading = new BehaviorSubject(false);
  constructor() { }

  showLoadingScreen(key: any){
    this.requests.set(key, 1);    
    this.isLoading.next(true);    
  }

  hideLoadingScreen(key: any){    
    if(this.requests.has(key)){
      this.requests.delete(key);
      this.isLoading.next(this.requests.size > 0);  
    }    
  }
}