import { Component, OnInit } from '@angular/core';
import { HttpClientService } from 'src/app/services/httpClientService';
import { ApiConstant } from 'src/app/utility/ApiConstant';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { EntityType } from 'src/app/_enums/entityType';
import { DynamicColumn } from 'src/app/models/gridView/DynamicColumn';
import { GridView } from 'src/app/models/gridView/gridView';
import { GridViewDetails } from 'src/app/models/gridView/gridViewDetails';

import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-column-view-create',
  templateUrl: './column-view-create.component.html',
  styleUrls: ['./column-view-create.component.scss']
})
export class ColumnViewCreateComponent implements OnInit {

  constructor(private httpClientService: HttpClientService,private toast:ToastrService
    ,private router: Router) { }
  dynamicColumn: DynamicColumn[] = [];
  dynamicColumnFilter: DynamicColumn[] = [];
  choosenColumn: DynamicColumn[] = [];
  name: string = '';
  searchText: string = '';
  gridView: GridView = new GridView();

  ngOnInit() {
    this.loadColumns();
  }
  loadColumns() {
    var data = this.httpClientService.getAsync(ApiConstant.GetAllFacilityColumn + '?EntityTypeId=' + EntityType.Facility)
      .then((data) => {
        //this.dynamicColumn = data.data;
        Object.assign(this.dynamicColumn,data.data)
        Object.assign(this.dynamicColumnFilter,data.data)
      })
  }
  columnChoosen_Clicked(data) {

    var index = this.dynamicColumn.indexOf(data);
    this.dynamicColumn.splice(index, 1);
    this.dynamicColumnFilter.splice(index, 1);
    this.choosenColumn.push(data);
  }
  choosenColumnDelete_Clicked(data) {

    var index = this.choosenColumn.indexOf(data);
    this.choosenColumn.splice(index, 1);
    this.dynamicColumn.push(data);
    this.dynamicColumnFilter.push(data);
  }

  get invalidViewName():boolean{
    return !this.name || this.name.length < 5;
  }
  saveDynamicView() {

    if(this.invalidViewName){
      return;
    }
    this.gridView.entityTypeId = EntityType.Facility;
    this.gridView.name = this.name;
    //var order=0;
    // this.choosenColumn.map((item)=>{
    //   item.columnOrder= (order+1);
    // })

    for (var i = 0; i < this.choosenColumn.length; i++) {
      this.choosenColumn[i].columnOrder = i + 1;
    }

    this.gridView.gridViewDetails = this.choosenColumn.map(item => new GridViewDetails(item));
    this.httpClientService.postAsync(ApiConstant.SaveFacilityGridView, this.gridView)
    .then(a=>
      {
        this.toast.success( MessageConstant.SaveSuccessful)
        this.router.navigate(['unicef/facility/home']);
      });
    
  }
  filterDynamicColumn(e) {
    var value=e.target.value;
    if (value) {
      this.dynamicColumn = Object.assign([], this.dynamicColumnFilter).filter(
        item => item.name.toLowerCase().indexOf(value.toLowerCase()) > -1
      )
    }
    else{
      this.dynamicColumn =Object.assign([], this.dynamicColumnFilter);
    }
  }
}
