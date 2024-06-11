import { IndicatorViewModel } from './indicatorViewModel';
import { EntityType } from 'src/app/_enums/entityType';

export class IAddIndicatorModel {
    indicators: IndicatorViewModel[];
    entityType: EntityType
}