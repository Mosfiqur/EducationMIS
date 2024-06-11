import { IBaseQueryModel } from './base-query.model';
import { FrameworkType } from 'src/app/_enums/frameworkType';

export interface FrameworkDynamicColumnQueryModel extends IBaseQueryModel{
    frameworkType: FrameworkType
}