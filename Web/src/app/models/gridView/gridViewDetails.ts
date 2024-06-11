import { DynamicColumn } from './DynamicColumn';

export class GridViewDetails {
    id: number;
    entityDynamicColumnId:number;
    columnOrder: number ;
    name:string;
    constructor(item: DynamicColumn) {
        this.entityDynamicColumnId = item.id;
        this.columnOrder=item.columnOrder;
    }
}