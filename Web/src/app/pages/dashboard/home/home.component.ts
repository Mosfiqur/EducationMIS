declare var $: any;
import { Component, OnInit } from '@angular/core';

import { HttpClientService } from '../../../services/httpClientService'

import { Router } from '@angular/router';

@Component({
  selector: 'ngx-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  
  referralCapital: number = 72000;

  cgMessage: string = "Hellow World";

  dashboards = [
    {name: 'JRP -2020', path: 'predefined-dashboard-selcted'},
    {name: 'Gap Map', path: 'gap-map'},
    {name: 'LC Map', path: 'lc-map'},

  ]
  selectedRoute: any;
  constructor(private router:Router) {



  }

  onRouteSelected(route: any){
    if(!route)
      this.selectFirstDashboard();      
    this.selectedRoute = route;
  }

  



  ngOnInit() {
    this.selectFirstDashboard();
  }

  selectFirstDashboard(){
    this.onRouteSelected(this.dashboards[0]);
  }

}
