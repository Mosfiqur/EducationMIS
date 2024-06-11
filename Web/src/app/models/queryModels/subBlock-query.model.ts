import { IBaseQueryModel } from './base-query.model';

export class SubBlockQueryModel implements IBaseQueryModel {
    pageSize: number;
    pageNo: number;
    searchText?: string;
    blockId?: number;    
}