import { ApiConstant } from '../ApiConstant';

export class FrameworkDynamicColumnEndpoints {
      static add: string = ApiConstant.baseUrl + "FrameworkDynamicColumn/Add";    
      static update: string = ApiConstant.baseUrl + "FrameworkDynamicColumn/Update";    
      static getById: string = ApiConstant.baseUrl + "FrameworkDynamicColumn/GetById";    
      static delete: string = ApiConstant.baseUrl + "FrameworkDynamicColumn/Delete";    
      static getByFrameworkType: string = ApiConstant.baseUrl + "FrameworkDynamicColumn/GetByFrameworkType";                
}