import { Component, OnInit } from '@angular/core';
import { DashboardService } from 'src/app/services/dashboard.service';
import { ITotalCountsModel } from 'src/app/models/dashboard/total-counts.model';
import { ModalService } from 'src/app/services/modal.service';
import { DynamicColumnComponent } from 'src/app/shared/components/dynamic-column/dynamic-column.component';
import { DynamicColumnSaveViewModel, IDynamicColumn } from 'src/app/models/dynamicColumn/dynamicColumnSaveViewModel';
import { EntityType } from 'src/app/_enums/entityType';
import { ObjectiveIndicatorSelectorComponent } from 'src/app/shared/components/objective-indicator-selector/objective-indicator-selector.component';
import { IObjectiveIndicator } from 'src/app/models/frameworks/objective-indicator.model';
import { Formula } from 'src/app/_enums/formula';
import { Convert } from 'src/app/utility/Convert';
import { ISelectListItem } from 'src/app/models/helpers/select-list.model';
import { DynamicColumnService } from 'src/app/services/dynamicColumn.service';
import { EntityIndicatorSelectorComponent } from 'src/app/shared/components/entity-indicator-selector/entity-indicator-selector.component';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageConstant } from 'src/app/utility/MessageConstant';
import { ToastrService } from 'ngx-toastr';
import { IBaseQueryModel } from 'src/app/models/queryModels/base-query.model';
import { IJrpParameterInfo } from 'src/app/models/dashboard/jrp-parameter-info.model';

import * as Highcharts from 'highcharts';

import addMore from "highcharts/highcharts-more";
import noData from 'highcharts/modules/no-data-to-display';
import { ObjectiveIndicatorComponent } from 'src/app/shared/components/objective-indicator/objective-indicator.component';
import { JrpParameterInfoComponent } from 'src/app/components/jrpParameterInfo/jrpParameterInfo.component';
import { JrpFilter } from 'src/app/models/dashboard/jrp-filter.model';

addMore(Highcharts)
noData(Highcharts);


@Component({
  selector: 'app-predefined-dashbord-selected',
  templateUrl: './predefined-dashbord-selected.component.html',
  styleUrls: ['./predefined-dashbord-selected.component.scss']
})
export class PredefinedDashbordSelectedComponent implements OnInit {

  totalCounts: ITotalCountsModel;

  public jrpFilterModel: JrpFilter;
  jrpParameterInfoList: IJrpParameterInfo[];
  target: IObjectiveIndicator[] = [];
  indicator: IDynamicColumn[] = [];
  public formulaList: ISelectListItem[] = [];
  parameterForm: FormGroup;

  public Highcharts: typeof Highcharts = Highcharts;

  public jrpChartType: string = "chart";
  public jrpChartFilter: JrpFilter;

  public jrpChart: any = {
    chart: {
      type: 'column'
    },
    credits: {
      enabled: false
    },
    title: {
      text: 'JRP'
    },

    xAxis: {
      categories: []
    },

    yAxis: {
      allowDecimals: false,
      min: 0,
      title: {
        display: false,
        text: ''
      }
    },

    // tooltip: {
    //   formatter: function () {
    //     return '<b>' + this.x + '</b><br/>' +
    //       this.series.name + ': ' + this.y + '<br/>' +
    //       'Total: ' + this.point.name;
    //   }
    // },

    series: [{}, {}]
  };


  constructor(
    private fb: FormBuilder,
    private dashboardService: DashboardService,
    private modalService: ModalService,
    private dynamicColumnService: DynamicColumnService,
    private toast: ToastrService
  ) {
    //this.jrpChartFilter=new JrpFilter();
    this.jrpFilterModel = new JrpFilter();
    this.parameterForm = this.fb.group({
      "id": null,
      "name": ['', [Validators.required]],
      "formula": ['', [Validators.required]],
      "targetId": [null, [Validators.required]],
      "indicatorId": [null, [Validators.required]],
    });

  }

  async ngOnInit() {
    this.totalCounts = await this.dashboardService.getTotalCounts();
    this.formulaList = Convert.enumToSelectList(Formula);

    this.getJrpData();
    this.getJrpChartData();
  }
  selectInstance() {
    let selectedId: IObjectiveIndicator[] = [];
    Object.assign(selectedId, this.target);
    this.modalService.open<ObjectiveIndicatorSelectorComponent, IObjectiveIndicator[]>
      (ObjectiveIndicatorSelectorComponent, { selectedIndicatorIds: selectedId, isMultivalued: 'false' })
      .then(column => {
        this.target = column;
        this.parameterForm.controls['targetId'].patchValue(this.target[0].id);
      })
  }

  openJrpParameter() {

    this.modalService.open<JrpParameterInfoComponent, IObjectiveIndicator[]>
      (JrpParameterInfoComponent, {})
      .then(column => {
        this.getJrpData();
        this.getJrpChartData();
      })
  }
  editJrpParameter(c: IJrpParameterInfo) {
    this.modalService.open<JrpParameterInfoComponent, IObjectiveIndicator[]>
      (JrpParameterInfoComponent, { jrpParameterInfo: c })
      .then(column => {
        this.getJrpData();
        this.getJrpChartData();
      })
  }
  deleteJrpParameter(c: number) {
    this.dashboardService.deleteJrpParameterInfo(c)
      .then(column => {
        this.getJrpData();
        this.getJrpChartData();
      })
  }
  filterComplete(data) {
    this.jrpFilterModel = data;
    this.getJrpChartData();
  }
  getJrpData() {
    let query: IBaseQueryModel = {
      pageNo: 1,
      pageSize: 2147483647,
      searchText: ''
    }
    this.dashboardService.getJrpData(query).then(a => {
      this.jrpParameterInfoList = a;
    });

  }
  getJrpChartData() {
    let query = {

    }

    this.dashboardService.getJrpChartData(this.jrpFilterModel).then(a => {
      let cat = [];
      let tar = [];
      let ach = [];
      var maxAchievement = Math.max.apply(Math, a.achievement.map(function (o) { return o; }))
      var maxTarget = Math.max.apply(Math, a.target.map(function (o) { return o; }))
      var maxVal = maxAchievement > maxTarget ? maxAchievement : maxTarget;
      if (maxVal == null)
        maxVal = 0;
      console.log('max achievement', maxVal);
      a.categories.map(a => {
        cat.push(a);
      })
      a.target.map(a => {
        tar.push(a);
      })
      a.achievement.map(a => {
        ach.push(a);
      })

      this.jrpChart = {
        chart: {
          type: 'column'
        },
        credits: {
          enabled: false
        },
        title: {
          text: 'JRP'
        },

        xAxis: {
          categories: cat
        },

        yAxis: {
          allowDecimals: true,
          min: 0,
          title: {
            display: false,
            text: ''
          }
        },
        plotOptions: {

          series: {
            dataLabels: {
              enabled: true,
              formatter: function () {
                
                let px = maxVal / 150;
                return this.y > px || this.y == 0 ? "" : this.y;
              }
            }
          }
        },

        series: [{
          name: 'Target',
          data: tar
        }, {
          name: 'Achievement',
          data: ach
        }]
      };




    });

  }
}
