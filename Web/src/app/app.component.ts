import { Component, OnInit } from '@angular/core';
import { AuthService } from './services/auth.service';
import { LoadingSpinnerService } from './core/loading-spinner/loading-spinner.service';
import { HttpClient } from '@angular/common/http';
import { Router, RouterEvent, RouteConfigLoadStart, RouteConfigLoadEnd } from '@angular/router';

@Component({
  selector: 'unicef-app',
  template: `  
  <router-outlet></router-outlet>
  <app-loading-spinner></app-loading-spinner>
  `,
})
export class AppComponent implements OnInit{
  constructor(
      private authService: AuthService, 
      private spinnerService: LoadingSpinnerService, 
      private router: Router
      ){
    
  }
  ngOnInit(): void {    
    this.authService.logUserInOnRefresh();
    this.router.events.subscribe((event: RouterEvent) => {
        if(event instanceof RouteConfigLoadStart){
          this.spinnerService.isLoading.next(true);
        }
        if(event instanceof RouteConfigLoadEnd){
          this.spinnerService.isLoading.next(false);
        }
    });
  }
  title = 'unicef'; 
}
