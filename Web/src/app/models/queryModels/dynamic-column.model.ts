import { EntityType } from 'src/app/_enums/entityType';
import { IBaseQueryModel } from './base-query.model';

export interface IDynamicColumnQueryModel extends IBaseQueryModel{
    entityType: EntityType
}