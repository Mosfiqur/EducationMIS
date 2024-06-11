import { Component, OnInit } from '@angular/core';
import { DynamicColumn } from 'src/app/models/gridView/DynamicColumn';
import { GridView } from 'src/app/models/gridView/gridView';
import { Router, ActivatedRoute } from '@angular/router';
import { HttpClientService } from 'src/app/services/httpClientService';
import { ToastrService } from 'ngx-toastr';
import { ApiConstant } from 'src/app/utility/ApiConstant';
import { EntityType } from 'src/app/_enums/entityType';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { GridViewDetails } from 'src/app/models/gridView/gridViewDetails';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';

@Component({
  selector: 'app-column-view-edit',
  templateUrl: './column-view-edit.component.html',
  styleUrls: ['./column-view-edit.component.scss']
})
export class ColumnViewEditComponent implements OnInit {

  private allRecords = 2147483647;
  public paginationPageSize = 10;

  dynamicColumn: DynamicColumn[] = [];
  dynamicColumnFilter: DynamicColumn[] = [];

  choosenColumn: DynamicColumn[] = [];
  name: string = '';
  searchText: string = '';
  gridView: GridView = new GridView();

  constructor(private httpClientService: HttpClientService, private toast: ToastrService,
    private dynamicColumnService:DynamicColumnService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      var id = params['id'];
      this.loadGridView(id);
    });
  }
  loadColumns(searchText) {
    console.log('use service')
    var data =this.dynamicColumnService.getAllColumns(this.allRecords,1,EntityType.Facility,searchText) //this.httpClientService.getAsync(ApiConstant.GetAllColumn + '?EntityTypeId=' + EntityType.Beneficiary)
      .then((data) => {
        //this.dynamicColumn = data.data;
        Object.assign(this.dynamicColumn, data.data);
        Object.assign(this.dynamicColumnFilter, data.data);


        for (var i = this.dynamicColumn.length - 1; i >= 0; i--) {
          for (var j = 0; j < this.choosenColumn.length; j++) {
            if (this.dynamicColumn[i] && (this.dynamicColumn[i].id === this.choosenColumn[j].id)) {
              this.dynamicColumn.splice(i, 1);
            }
          }
        }

      })
  }
  loadGridView(id) {
    var data = this.httpClientService.getAsync(ApiConstant.GetByIdFacility + '?id=' + id)
      .then((data) => {
        //this.dynamicColumn = data.data;

        Object.assign(this.gridView, data);
        Object.assign(this.choosenColumn, this.gridView.gridViewDetails.map(obj=>({id: obj.entityDynamicColumnId, name: obj.name})));
        this.loadColumns("");
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

  saveDynamicView() {

    this.gridView.entityTypeId = EntityType.Facility;
   // this.gridView.name = this.name;

    for (var i = 0; i < this.choosenColumn.length; i++) {
      this.choosenColumn[i].columnOrder = i + 1;
    }

    this.gridView.gridViewDetails = this.choosenColumn.map(item => new GridViewDetails(item));
    this.httpClientService.postAsync(ApiConstant.UpdateFacilityGridView, this.gridView)
      .then(a => {
        this.toast.success(MessageConstant.SaveSuccessful)
        this.router.navigate(['unicef/beneficiary/home']);
      });

  }
  filterDynamicColumn(e) {
    var value = e.target.value;
    if (value) {
      this.dynamicColumn = Object.assign([], this.dynamicColumnFilter).filter(
        item => item.name.toLowerCase().indexOf(value.toLowerCase()) > -1
      )
    }
    else {
      this.dynamicColumn = Object.assign([], this.dynamicColumnFilter);
    }
  }
}
