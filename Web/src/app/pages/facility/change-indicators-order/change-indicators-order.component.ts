import { Component, OnInit } from '@angular/core';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { moveItemInArray, CdkDragDrop } from '@angular/cdk/drag-drop';
import { ToastrService } from 'ngx-toastr';
import { IndicatorService } from 'src/app/services/indicator.service';
import { ActivatedRoute, Router } from '@angular/router';
import { IndicatorViewModel } from 'src/app/models/indicator/indicatorViewModel';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';

@Component({
  selector: 'app-change-indicators-order',
  templateUrl: './change-indicators-order.component.html',
  styleUrls: ['./change-indicators-order.component.scss']
})
export class ChangeIndicatorsOrderComponent implements OnInit {

  private allRecords = 2147483647;
  public pageNo = 1;

  pagedIndicators: IPagedResponse<IndicatorViewModel>;
  lstIndicator: IndicatorViewModel[];
  instanceId:number;

  constructor(private indicatorService: IndicatorService, private route: ActivatedRoute
    , private router: Router, private toast: ToastrService) { }

  ngOnInit() {
    this.route.params.subscribe((params) => {
      var id = params['instanceId'];
      this.instanceId=id;
      this.loadIndicator(id);
    });
  }

  loadIndicator(instanceId) {
    this.indicatorService.getFacilityIndicator(this.allRecords, this.pageNo, instanceId).then(data => {
      this.lstIndicator = data.data;
    });
  }

  onDrop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.lstIndicator, event.previousIndex, event.currentIndex);
    this.lstIndicator.forEach((indicator, idx) => {

      indicator.columnOrder = idx + 1;
    });
  }

  get getIndicator() {
    return this.lstIndicator.map((a) => {
      return ({ entityDynamicColumnId: a.entityDynamicColumnId, columnOrder: a.columnOrder, instanceId: this.instanceId })
    })
  }

  save(){
    this.indicatorService.UpdateFacility(this.getIndicator).then(a => {
      this.toast.success(MessageConstant.UpdateSuccessful);
      this.router.navigate(['unicef/facility/indicators/'+this.instanceId]);
    });
  }


}
