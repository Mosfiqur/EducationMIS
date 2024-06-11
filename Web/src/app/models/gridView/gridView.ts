import { EntityType } from 'src/app/_enums/entityType';
import { GridViewDetails } from './gridViewDetails';

export class GridView {
    id: number ;
    name: string ;
    entityTypeId:EntityType;
    gridViewDetails:GridViewDetails[]
}