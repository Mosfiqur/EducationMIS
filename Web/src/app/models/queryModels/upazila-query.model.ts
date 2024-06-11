import { IBaseQueryModel } from './base-query.model';

export class UpazilaQueryModel implements IBaseQueryModel {
    pageSize: number;
    pageNo: number;
    searchText?: string;
    districtId?: number;    
}