import { Component, OnInit, Input } from '@angular/core';
import { OnlineFacilityService } from 'src/app/services/online-facility.service';
import { facilityWiseIndicatorViewModel } from 'src/app/models/viewModel/facilityWiseIndicatorViewModel';
import { ActivatedRoute } from '@angular/router';
import { indicatorGetViewModel } from 'src/app/models/viewModel/indicatorGetViewModel';
import { ColumnDataType } from 'src/app/_enums/column-data-type';
import { facilityDynamicCellAddViewModel } from 'src/app/models/viewModel/facilityDynamicCellAddViewModel';
import { dynamicCellViewModel } from 'src/app/models/viewModel/dynamicCellViewModel';
import { ToastrService } from 'ngx-toastr';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { FacilityViewModel } from 'src/app/models/viewModel/facilityViewModel';
import { IDynamicColumn } from 'src/app/models/cellEditorModels/IDynamicColumn';
import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ModalService } from 'src/app/services/modal.service';
import { FormBuilder } from '@angular/forms';
import { ICellEditorResult } from 'src/app/helpers/cell-editor-result';
import { OnlineOfflineStatus } from 'src/app/_enums/onlineOfflineStatus';

@Component({
  selector: 'app-facility-indicator',
  templateUrl: './facility-indicator.component.html',
  styleUrls: ['./facility-indicator.component.scss']
})
export class FacilityIndicatorComponent implements OnInit {
  private allRecords = 2147483647;
  public indicatorList: facilityWiseIndicatorViewModel[];
  facilityId: number;
  instanceId: number;
  public indicators:indicatorGetViewModel[];
  public facilityDynamicCell : facilityDynamicCellAddViewModel;
  public facilityViewModel:FacilityViewModel;
  status: OnlineOfflineStatus;

  constructor(private route: ActivatedRoute, private onlineFacilityService: OnlineFacilityService, 
    private toastrService: ToastrService,private modalService: ModalService,private formBuilder:FormBuilder) {
    this.facilityDynamicCell=new facilityDynamicCellAddViewModel();
    this.facilityDynamicCell.dynamicCells=[];
  }

  ngOnInit() {
    this.facilityId = parseInt(this.route.snapshot.paramMap.get('facilityId'));
    this.instanceId = parseInt(this.route.snapshot.paramMap.get('instanceId'));
    this.status = OnlineOfflineStatus.Online;
    // this.loadFacilityInfo();
    // this.loadFacilityIndicator();
  }

  loadFacilityInfo(){
    this.onlineFacilityService.getFacilityById(this.facilityId).then((data)=>{
      this.facilityViewModel = data;
    });
  }

  loadFacilityIndicator() {
    return new Promise((resolve, reject) => {

      var data = this.onlineFacilityService.getFacilityIndicator(this.instanceId, this.facilityId, this.allRecords, 1)
        .then((data) => {
          this.indicatorList = data.data;
          this.indicatorList.map(x => this.indicators = x.indicators);
          resolve(this.indicatorList);
        });
    });
  }

  ColumnDataTypeText(id) {
    return ColumnDataType[id];
  }

  onSubmit(){
    this.indicators.map(a=>{
      var data=({entityDynamicColumnId:a.entityDynamicColumnId,value:a.values})
      let ent:dynamicCellViewModel;
      ent=data;
      this.facilityDynamicCell.facilityId=this.facilityId;
      this.facilityDynamicCell.instanceId=this.instanceId;
      this.facilityDynamicCell.dynamicCells.push(ent);
    });
    console.log('facility',this.facilityDynamicCell);
    this.onlineFacilityService.saveFacilityCell(this.facilityDynamicCell).then(a => {
      this.toastrService.success(MessageConstant.SaveSuccessful);
    });
  }

  onClickListTypeSave(column:IDynamicColumn){
    this.modalService.open<FwCellEditorComponent, ICellEditorResult>(
      FwCellEditorComponent, {cell: {entityDynamicColumnId:column.entityDynamicColumnId,value:["1"]}, column: column})
    .then((cellResult)=> {
      //this.onChangeRecordSave(column.id,cellResult.cell.value.join(','));
    });
  }
  
  addIndicatorValue(value:string,indicator:indicatorGetViewModel){
    if(indicator.columnDataType!=ColumnDataType.List){
      indicator.values=[value];
    }
    else{
      indicator.values.push(value);
    }
  }
}

