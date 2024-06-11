import { Component, OnInit } from '@angular/core';
import { IMonitoringFramework } from 'src/app/models/frameworks/monitoring-framework.model';
import { IObjectiveIndicator } from "src/app/models/frameworks/objective-indicator.model";
import { FrameworkDynamicColumnService } from 'src/app/services/framework-dynamic-column.service';
import { ToastrService } from 'ngx-toastr';
import { FrameworkType } from 'src/app/_enums/frameworkType';
import { IFrameworkDynamicColumn } from 'src/app/models/frameworks/framework-dynamic-column.model';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { Globals } from 'src/app/globals';
import { ModalService } from 'src/app/services/modal.service';
import { FwColumnEditorComponent } from 'src/app/components/fw-column-editor/fw-column-editor.component';
import { IColumnEditorResult } from 'src/app/models/helpers/column-editor-result';
import { MonitoringFrameworkService } from 'src/app/services/monitoring-framework.service';
import { IPagedResponse } from 'src/app/models/responseModels/pagedResponseModel';

import { FwCellEditorComponent } from 'src/app/components/fw-cell-editor/fw-cell-editor.component';
import { ICellEditorResult } from 'src/app/models/helpers/cell-editor-result';
import { IDynamicCell, IMonitoringDynamicCellInsertModel, IMonitoringFrameworkDynamicCell, MonitoringFrameworkDynamicCell } from 'src/app/models/frameworks/dynamic-cell.model';
import { ObjectiveIndicatorComponent } from 'src/app/shared/components/objective-indicator/objective-indicator.component';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { EntityType } from 'src/app/_enums/entityType';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { FrameworkBaseComponent } from '../framework-base.component';
import { IndicatorService } from 'src/app/services/indicator.service';
import { ColumnDataType } from 'src/app/_enums/column-data-type';



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent extends FrameworkBaseComponent implements OnInit {

  frameworks: IMonitoringFramework[]  = [];
  framework: IMonitoringFramework;
  
  frameworkType: any;

  dynamicColumns: IDynamicColumn[] = [];

  pagedFrameworks: IPagedResponse<IMonitoringFramework>;
  constructor(
    dynamicColumnService: DynamicColumnService,
    private toastrService: ToastrService,
    globals: Globals,
    modalService: ModalService,
    private monitoringService: MonitoringFrameworkService
    ) { 
    super(dynamicColumnService, globals, modalService)
      this.frameworkType = FrameworkType;
    
  }

  async ngOnInit() {
    this.loadDynamicColumns(EntityType.Monitoring);
    await this.getMonitoringFrameworks();
    
  }

  async getMonitoringFrameworks(){
    this.monitoringService.getAll({pageSize: 10, pageNo: 1})
    .then(x => {
      this.pagedFrameworks = x;
      this.frameworks = this.pagedFrameworks.data;
      
      this.arrangeCells();
    });
  }
  async arrangeCells(){
    this.frameworks.forEach(framework => {    
      
      framework.objectiveIndicators.forEach(indicator => {
        let cells = new Array<IMonitoringFrameworkDynamicCell>(this.dynamicColumns.length);
        this.dynamicColumns.forEach((column, index) => {
        
        let existing = null;
        existing = indicator
         .dynamicCells
         .find(x => x.entityDynamicColumnId == column.id);

         if(existing == null){
          cells[index] =  new MonitoringFrameworkDynamicCell(indicator.id, column.id, [], column.columnName);
         }else{
           cells[index] = existing;
         }
         cells[index].listType = column.listObject;
         cells[index].dataType = column.columnDataType;  
      })
      indicator.dynamicCells = cells;   
      })      
    });
  }

  async onAddNewDynamicColumn() {        
    super.addNewDynamicColumn(EntityType.Monitoring);      
  }

  pushEmptyCell(column: IDynamicColumn){        
    this.frameworks.forEach(fw => {      
      fw.objectiveIndicators.forEach(indicator => {
        let  cell =  new MonitoringFrameworkDynamicCell(indicator.id, column.id, [], column.columnName);        
        indicator.dynamicCells.push(cell);
      })
    });
  }


  async onEditDynamicColumn(selectedColumn: IFrameworkDynamicColumn) {        
    throw Error("Not implemented");
  }

  async editDynamicCell(indicator: IObjectiveIndicator, cell: IMonitoringFrameworkDynamicCell){        
    let insertModel: IMonitoringDynamicCellInsertModel;

    let column = this.dynamicColumns.find(x => x.id == cell.entityDynamicColumnId);

    this.modalService
    .open<FwCellEditorComponent, ICellEditorResult>(FwCellEditorComponent, {cell: cell, column: column})
    .then(async result => {    

      if(!result){
        return;
      }

      if(result.isDeleted){
        await this.deleteDynamicCell(cell);
        return;
      }
      
      insertModel = {
        objectiveIndicatorId : cell.objectiveIndicatorId,          
        dynamicCells: [{
          entityDynamicColumnId: cell.entityDynamicColumnId,
          value: result.cell.value
        }]
      };  
      
      return this.monitoringService.insertDynamicCell(insertModel)
      .then(x=> {
        this.toastrService.success(MessageConstant.SaveSuccessful); 
        this.getMonitoringFrameworks();
      });

    });

  }
  

  async deleteDynamicCell(cell: IMonitoringFrameworkDynamicCell){
    if(await this.modalService.confirm()){      
      await this.monitoringService.deleteDynamicCell(cell); 
      this.getMonitoringFrameworks();
      this.toastrService.success(MessageConstant.DeleteSuccess);
    }
}

async addDynamicCell(indicator: IObjectiveIndicator, cell: IMonitoringFrameworkDynamicCell){    
  this.editDynamicCell(indicator, cell);
}



editIndicator(indicator: IObjectiveIndicator){
  let updateModel:IObjectiveIndicator;
  this.modalService.open<ObjectiveIndicatorComponent, IObjectiveIndicator>(ObjectiveIndicatorComponent, {
    indicator: indicator
  })
  .then(result => {
    if(result){
      updateModel = result;
      return this.monitoringService.updateIndicator(result)
      .then(x=> {
        this.getMonitoringFrameworks();
        this.toastrService.success(MessageConstant.SaveSuccessful);
      }).catch(x=> x)
    }    
  })  
  .catch(x=> x)
}

async addIndicator(fw: IMonitoringFramework){
  this.modalService.open<ObjectiveIndicatorComponent, IObjectiveIndicator>(ObjectiveIndicatorComponent)
  .then((indicator: IObjectiveIndicator) => {
    indicator.monitoringFrameworkId = fw.id;
    return this.monitoringService.addIndicator(indicator);
    
  })
  .then((indicator: IObjectiveIndicator) => {
    this.addEmptyCells(indicator);
    // fw.objectiveIndicators.push(indicator);
    this.getMonitoringFrameworks();
    this.toastrService.success(MessageConstant.SaveSuccessful);
    this.arrangeCells()
  })
  .catch(x => x)
}

addEmptyCells(indicator: IObjectiveIndicator){
  this.dynamicColumns.forEach(column => {    
    indicator.dynamicCells.push(new MonitoringFrameworkDynamicCell(indicator.id, column.id, [], column.columnName))    
  })
}
addObjective(){  
  this.modalService.paragraphInput('Add Objective', 'Objective')
  .then( objective => {
    if(!objective)
      return;      
    return this.monitoringService.create({objective: objective});   
  })
  .then(x => {    
    if(x){
      this.frameworks.push(x);
      this.toastrService.success(MessageConstant.SaveSuccessful);
    }    
  })
  .catch(x =>x)
}
editObjective(fw?: IMonitoringFramework){    
  this.modalService.paragraphInput('Add Objective', 'Objective', fw.objective)
  .then( objective => {
    if(!objective){
      return;
    }    
    return this.monitoringService.update({objective: objective, id: fw.id})
    .then(x=> {
      fw.objective = objective;
      this.toastrService.success(MessageConstant.SaveSuccessful);
    });
  })
  .catch(x => x)
}

}
