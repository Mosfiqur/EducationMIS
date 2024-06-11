import { LevelOfStudy } from 'src/app/_enums/levelOfStudy';
import { CollectionStatus } from 'src/app/_enums/collectionStatus';

export class List{
    id:number; 
    name:string; 
} 

export class ListItem{
    id:number; 
    listId: number; 
    title:string; 
    value:number; 
} 

export class User{ 
    id:number; 
    token: string = ""; 
	fullName: string = "";
	levelName: string = "";
	roleName: string = "";
	email: string = "";
	phoneNumber: string= "";
	designationName:string="";     
} 

export class Camp{
    id:number;
    name:string;
    SSID:string;
    nameAlias:string;
}

export class Block{
    id:number;
    code:string;
    name:string;
    campId:number;
}

export class SubBlock{
    id:number;
    name:string;
    blockId:number;
} 

export class BeneficiaryScheduleInstance{
    id:number; 
    scheduleId:number;
    scheduleTitle:String; 
    instanceTitle:String; 
    dataCollectionDate:String; 
    endDate: String;
    status:number; 
}  

export class FacilityScheduleInstance{
    id:number; // storecofig autoincrement false 
    scheduleId:number;
    scheduleTitle:String; 
    instanceTitle:String; 
    dataCollectionDate:String; 
    endDate: String;
    status:number; 
}  

export class Facility{
    uniqueId:number;
    id:number; 
    facilityName:String;
    facilityCode:String; 
    campName:String; 
    blockName:String; 
    upazilaName: String;
    unionName: String;
    teacherName: String;
    programmingPartnerName:String; 
    implementationPartnerName:String; 
    collectionStatus:CollectionStatus;
} 

/**
 * While uploading data, at first get new beneficiaries with following query: 
 * select * from Beneficiary where id is null or id = 0; 
 */
export class Beneficiary{
    uniqueId:number;  // StoreConfig key 
    id:number; // can be zero or null for new beneficiary 
    UnhcrId:String;  
    beneficiaryName:String; 
    fatherName:string;
    motherName:string;
    FCNId:string;
    dateOfBirth:string;
    sex:number;
    disengaged:boolean;
    disabled:boolean;
    levelOfStudy:LevelOfStudy;
    enrollmentDate:string;
    facilityId:number;
    facilityName:String;
    beneficiaryCampId:number;
    beneficiaryCampName: String;
    blockName: String;
    subBlockName: String;
    blockId:number;
    subBlockId:number;
    remarks:string;
    collectionStatus:CollectionStatus;
    // Other fields used in beneficiary create 
} 

export class FacilityIndicator{
    id:number; // storeconfig key autoincrement 
    entityDynamicColumnId:number; 
    instanceId:number; 
    columnName:String; 
    columnNameInBangla:String;
    columnDataType:number; 
    isMultiValued:boolean; 
    listId:number; 
} 


export class BeneficiaryIndicator{
    id:number; // storeconfig key autoincrement 
    entityDynamicColumnId:number; 
    instanceId:number; 
    columnName:String; 
    columnNameInBangla:String;
    columnDataType:number; 
    isMultiValued:boolean; 
    listId:number; 
}


export class BeneficiaryRecord{
    id:number; // storeconfig key autoincrement true
    beneficiaryId:number; // unique id of beneficiary table
    instanceId:number;
    columnId:number; // id of beneficiary indicator table
    value:String;
    status:number;
}

export class FacilityRecord{
    id:number; // storeconfig key autoincrement true
    facilityId:number; // unique id of facility table
    instanceId:number;
    columnId:number; // id of facility indicator table
    value:String;
    status:number;
}

export class BeneficiaryDataCollectionStatus{
    id:number;
    beneficiaryId:number;
    instanceId:number;
    status:CollectionStatus;
}

export class FacilityDataCollectionStatus{
    id:number;
    facilityId:number;
    instanceId:number;
    status:CollectionStatus;
}