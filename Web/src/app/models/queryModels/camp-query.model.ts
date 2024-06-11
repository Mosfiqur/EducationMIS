import { IBaseQueryModel } from './base-query.model';

export class CampQueryModel implements IBaseQueryModel {
    pageSize: number;
    pageNo: number;
    searchText?: string;
    unionId?: number;    
}