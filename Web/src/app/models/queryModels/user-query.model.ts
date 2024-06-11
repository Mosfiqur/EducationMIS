import { IBaseQueryModel } from './base-query.model';

export interface IUserQueryModel extends IBaseQueryModel{
    roleIds?: number[];
    espIds?: number[];
    userId?: number;
}