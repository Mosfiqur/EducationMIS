import { Component, Input, OnInit, ViewEncapsulation } from "@angular/core";
import { EntityType } from "src/app/_enums/entityType";


@Component({
    selector: 'grid-legend',
    templateUrl: './grid-legend.component.html',
    styleUrls: ['./grid-legend.component.scss'],
    encapsulation: ViewEncapsulation.None,
})
export class GridLegendComponent {

    @Input('entityType') entityType: EntityType;

    
}