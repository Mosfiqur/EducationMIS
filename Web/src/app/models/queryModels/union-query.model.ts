import { IBaseQueryModel } from './base-query.model';

export class UnionQueryModel implements IBaseQueryModel {
    pageSize: number;
    pageNo: number;
    searchText?: string;
    upazilaId?: number;    
}