import { IBaseQueryModel } from './base-query.model';

export class BlockQueryModel implements IBaseQueryModel {
    pageSize: number;
    pageNo: number;
    searchText?: string;
    campId?: number;    
}