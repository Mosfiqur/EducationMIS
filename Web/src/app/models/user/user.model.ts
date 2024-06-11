import { IEducationSectorPartner } from './educationSectorPartner';
import { PartnerType } from 'src/app/_enums/partnerType';
import { IDynamicCell } from '../frameworks/dynamic-cell.model';

export interface IUser {
  fullName?: string;
  designationId?: number;
  designationName?: string;
  roleId?: number;
  roleName?: string;
  email?: string;
  phoneNumber?: string;
  eduSectorPartners?: IEducationSectorPartner[];
  id?: number;
  dynamicCells?: IDynamicCell[];
  isSelected?: boolean;
  educationSectorPartnerId?: number;
  programPartnerId?: number;
  implementationPartnerId?: number;
}

export class User implements IUser{
  static create(value: Partial<User>){
    return Object.assign({}, value);
  }
  constructor(
    fullName: string,
    designationId: number,
    designationName: string,
    roleId: number,
    roleName: string,
    email: string,
    phoneNumber: string,
    educationPartners: IEducationSectorPartner[],
    id: number,
    dynamicCells?: IDynamicCell[],
    ){
   
      this.id = id;
      this.fullName = fullName;
      this.designationId = designationId;
      this.designationName = designationName;
      this.roleId = roleId;
      this.roleName = roleName;
      this.email = email;
      this.phoneNumber = phoneNumber;      
      this.eduSectorPartners = educationPartners || [];
      this.isSelected = false;
      this.dynamicCells = dynamicCells;
    } 
  id?: number;
  fullName: string;
  designationId: number;
  designationName: string;
  roleId: number;
  roleName: string;
  email: string;
  phoneNumber: string;
  eduSectorPartners: IEducationSectorPartner[];  
  dynamicCells?: IDynamicCell[];
  isSelected: boolean;
  get espNames():string{    
    if(this.eduSectorPartners.length){
      return this.eduSectorPartners.map(p => p.partnerName).join(', ');
    }
    return "";
  }  

  get educationSectorPartnerId():number{
    let partnerIds = 
     this.eduSectorPartners.filter( x => x.partnerType == PartnerType.EducationSectorPartner)
    .map(x => x.id)
    if(partnerIds.length){
      return partnerIds[0];
    }
    return 0;
  }

  get programPartnerId():number{
    let partnerIds = 
     this.eduSectorPartners.filter( x => x.partnerType == PartnerType.ProgramPartner)
    .map(x => x.id)
    if(partnerIds.length){
      return partnerIds[0];
    }
    return 0;
  }

  get implementationPartnerId():number{
    let partnerIds = 
     this.eduSectorPartners.filter( x => x.partnerType == PartnerType.ImplementationPartner)
    .map(x => x.id)
    if(partnerIds.length){
      debugger;
      return partnerIds[0];
    }
    return 0;
  }
}