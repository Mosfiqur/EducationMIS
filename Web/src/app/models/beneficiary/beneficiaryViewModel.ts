import { CollectionStatus } from 'src/app/_enums/collectionStatus';
import { EntityDynamicColumn } from '../dynamicColumn/entityDynamicColumn';
import { Gender } from 'src/app/_enums/gender';
import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';

export class BeneficiaryViewModel{
      entityId: number;
      beneficiaryName: string;
      unhcrId: string;
      fcnId: string;

      fatherName:string;
      motherName:string;
      dateOfBirth:string;
      sex:Gender;
      disabled:boolean;
      levelOfStudy:LevelOfStudy;
      enrollmentDate :string;


      facilityId: number;
      facilityName: string;
      facilityCampId: number;
      facilityCampName: string;
      beneficiaryCampId: number;
      beneficiaryCampName: string;
      blockId: number;
      blockName: string;
      subBlockId: number;
      subBlockName: string;
      programmingPartnerId: number;
      programmingPartnerName: string;
      implemantationPartnerId: number;
      implemantationPartnerName: string;
      collectionStatus: CollectionStatus;

      remarks:string;
      isActive:boolean;
      isApproved:boolean;
      
      properties:EntityDynamicColumn[];
}