import { FrameworkType } from 'src/app/_enums/frameworkType';


export interface IFrameworkDynamicColumn{
    id?: number;
    columnName?: string;
    isFixed: boolean;
    hasMultiValue: boolean;
    frameworkType: FrameworkType
}

export class FrameworkDynamicColumn implements IFrameworkDynamicColumn{
    id?: number;
    columnName?: string;
    isFixed: boolean;
    hasMultiValue: boolean;
    frameworkType: FrameworkType;

}