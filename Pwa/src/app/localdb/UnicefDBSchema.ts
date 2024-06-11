import { ObjectStoreMeta } from 'ngx-indexed-db';

export class UnicefDBSchema{ 
    static TableNames={
        tbl_user:'user', 
        tbl_list:'list', 
        tbl_list_item:'listItem', 
        tbl_beneficiary_records:'beneficiaryRecords',
        tbl_camps:'camps',
        tbl_blocks:'blocks',
        tbl_subBlocks:'subBlocks',
        tbl_beneficiary_schedule_instance: 'beneficiaryScheduleInstance',
        tbl_facility_schedule_instance : 'facilityScheduleInstance',
        tbl_facility : 'facility',
        tbl_beneficiary : 'beneficiary',
        tbl_facility_indicator : 'facilityIndicator',
        tbl_beneficiary_indicator : 'beneficiaryIndicator',
        tbl_facility_records:'facilityRecords',
        tbl_beneficiary_data_collection_status:'beneficiary_data_collection_status',
        tbl_facility_data_collection_status:'facility_data_collection_status'
    }; 
    static ColumnNames = {
        col_id:'id', 
        col_full_name:'fullName', 
        col_designation_name:'designationName', 
        col_role_name:'roleName', 
        col_email:'email', 
        col_phone_number:'phoneNumber', 
        col_token:'token', 
        col_name:'name', 
        col_list_id:'listId', 
        col_title:'title', 
        col_value:'value', 
        col_beneficiary_id:'beneficiaryId', 
        col_instance_id:'instanceId', 
        col_column_id:'columnId', 
        col_status:'status',
        col_SSID:'SSID',
        col_nameAlias:'nameAlias',
        col_code:'code',
        col_camp_id: 'campId',
        col_block_id:'blockId',
        col_scheduleId:'scheduleId',
        col_scheduleTitle:'scheduleTitle',
        col_instanceTitle:'instanceTitle',
        col_dataCollectionDate:'dataCollectionDate',
        col_endDate: 'endDate',
        col_facilityName:'facilityName',
        col_facilityCode:'facilityCode',
        col_campName:'campName',
        col_programmingPartnerName:'programmingPartnerName',
        col_implementationPartnerName:'implementationPartnerName',
        col_uniqueId:'uniqueId',
        col_UnhcrId:'UnhcrId',
        col_disengaged:'disengaged',
        col_beneficiaryName:'beneficiaryName',
        col_entityDynamicColumnId:'entityDynamicColumnId',
        col_columnName:'columnName',
        col_columnDataType:'columnDataType',
        col_isMultiValued:'isMultiValued',
        col_fatherName:'fatherName',
        col_motherName:'motherName',
        col_FCNId:'FCNId',
        col_dateOfBirth:'dateOfBirth',
        col_sex:'sex',
        col_disabled:'disabled',
        col_levelOfStudy:'levelOfStudy',
        col_enrollmentDate:'enrollmentDate',
        col_facilityId:'facilityId',
        col_beneficiaryCampId:'beneficiaryCampId',
        col_remarks:'remarks',
        col_subBlockId:'subBlockId',
        col_collectionStatus:'collectionStatus',
        col_columnNameInBangla:'columnNameInBangla',
        col_blockName: 'blockName',
        col_subBlockName: 'subBlockName', 
        col_upazilaName: 'upazilaName',
        col_unionName: 'unionName',
        col_teacherName: 'teacherName'
    }; 

    static IndexNames = {
        beneficiary_instance:'beneficiary_instance', 
        beneficiary_instance_column:'beneficiary_instance_column',
        beneficiary_indicator_index:'beneficiary_indicator',
        facility_indicator_index:'facility_indicator',
        facility_record_index:'facility_records',
        facility_all_record_index:'facility_all_records',
        facility_data_collection_index: 'facility_data_collection_index',
        beneficiary_data_collection_index:'beneficiary_data_collection_index'
    };

    
    static UserTable:ObjectStoreMeta =
    {
        store: UnicefDBSchema.TableNames.tbl_user,
        storeConfig: { keyPath: UnicefDBSchema.ColumnNames.col_id, autoIncrement: false },
        storeSchema: [
            { name: UnicefDBSchema.ColumnNames.col_full_name, keypath: UnicefDBSchema.ColumnNames.col_full_name, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_designation_name, keypath: UnicefDBSchema.ColumnNames.col_designation_name, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_role_name, keypath: UnicefDBSchema.ColumnNames.col_role_name, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_email, keypath: UnicefDBSchema.ColumnNames.col_email, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_phone_number, keypath: UnicefDBSchema.ColumnNames.col_phone_number, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_token, keypath: UnicefDBSchema.ColumnNames.col_token, options: { unique: false } }
        ]
    };  

    static ListTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_list, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_name, keypath: UnicefDBSchema.ColumnNames.col_name, options: { unique: false } }
        ]
    };
    static ListItemTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_list_item, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_list_id, keypath: UnicefDBSchema.ColumnNames.col_list_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_title, keypath: UnicefDBSchema.ColumnNames.col_title, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_value, keypath: UnicefDBSchema.ColumnNames.col_value, options: { unique: false } }
        ]
    };

    static BeneficiaryRecordsTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_beneficiary_records, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_beneficiary_id, keypath: UnicefDBSchema.ColumnNames.col_beneficiary_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instance_id, keypath: UnicefDBSchema.ColumnNames.col_instance_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_column_id, keypath: UnicefDBSchema.ColumnNames.col_column_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_value, keypath: UnicefDBSchema.ColumnNames.col_value, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_status, keypath: UnicefDBSchema.ColumnNames.col_status, options: { unique: false } } 
        ]
    };

    static CampsTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_camps, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_SSID, keypath: UnicefDBSchema.ColumnNames.col_SSID, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_name, keypath: UnicefDBSchema.ColumnNames.col_name, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_nameAlias, keypath: UnicefDBSchema.ColumnNames.col_nameAlias, options: { unique: false } }, 
        ]
    };

    static BlocksTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_blocks, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_code, keypath: UnicefDBSchema.ColumnNames.col_code, options: { unique: false } },
            { name: UnicefDBSchema.ColumnNames.col_name, keypath: UnicefDBSchema.ColumnNames.col_name, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_camp_id, keypath: UnicefDBSchema.ColumnNames.col_camp_id, options: { unique: false } }, 
        ]
    };

    static SubBlocksTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_subBlocks, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_name, keypath: UnicefDBSchema.ColumnNames.col_name, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_block_id, keypath: UnicefDBSchema.ColumnNames.col_block_id, options: { unique: false } }, 
        ]
    };

    static BeneficiaryScheduleInstanceTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_beneficiary_schedule_instance, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_scheduleId, keypath: UnicefDBSchema.ColumnNames.col_scheduleId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_scheduleTitle, keypath: UnicefDBSchema.ColumnNames.col_scheduleTitle, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instanceTitle, keypath: UnicefDBSchema.ColumnNames.col_instanceTitle, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_dataCollectionDate, keypath: UnicefDBSchema.ColumnNames.col_dataCollectionDate, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_endDate, keypath: UnicefDBSchema.ColumnNames.col_endDate, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_status, keypath: UnicefDBSchema.ColumnNames.col_status, options: { unique: false } }, 
        ]
    };

    static FacilityScheduleInstanceTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_facility_schedule_instance, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:false}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_scheduleId, keypath: UnicefDBSchema.ColumnNames.col_scheduleId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_scheduleTitle, keypath: UnicefDBSchema.ColumnNames.col_scheduleTitle, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instanceTitle, keypath: UnicefDBSchema.ColumnNames.col_instanceTitle, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_dataCollectionDate, keypath: UnicefDBSchema.ColumnNames.col_dataCollectionDate, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_endDate, keypath: UnicefDBSchema.ColumnNames.col_endDate, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_status, keypath: UnicefDBSchema.ColumnNames.col_status, options: { unique: false } }, 
        ]
    };

    static FacilityTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_facility, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_uniqueId, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_id, keypath: UnicefDBSchema.ColumnNames.col_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_facilityName, keypath: UnicefDBSchema.ColumnNames.col_facilityName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_facilityCode, keypath: UnicefDBSchema.ColumnNames.col_facilityCode, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_campName, keypath: UnicefDBSchema.ColumnNames.col_campName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_blockName, keypath: UnicefDBSchema.ColumnNames.col_blockName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_upazilaName, keypath: UnicefDBSchema.ColumnNames.col_upazilaName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_unionName, keypath: UnicefDBSchema.ColumnNames.col_unionName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_teacherName, keypath: UnicefDBSchema.ColumnNames.col_teacherName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_programmingPartnerName, keypath: UnicefDBSchema.ColumnNames.col_programmingPartnerName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_implementationPartnerName, keypath: UnicefDBSchema.ColumnNames.col_implementationPartnerName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_collectionStatus, keypath: UnicefDBSchema.ColumnNames.col_collectionStatus, options: { unique: false } }, 
        ]
    };

    static BeneficiaryTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_beneficiary, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_uniqueId, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_id, keypath: UnicefDBSchema.ColumnNames.col_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_UnhcrId, keypath: UnicefDBSchema.ColumnNames.col_UnhcrId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_fatherName, keypath: UnicefDBSchema.ColumnNames.col_fatherName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_motherName, keypath: UnicefDBSchema.ColumnNames.col_motherName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_FCNId, keypath: UnicefDBSchema.ColumnNames.col_FCNId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_dateOfBirth, keypath: UnicefDBSchema.ColumnNames.col_dateOfBirth, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_sex, keypath: UnicefDBSchema.ColumnNames.col_sex, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_disengaged, keypath: UnicefDBSchema.ColumnNames.col_disengaged, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_disabled, keypath: UnicefDBSchema.ColumnNames.col_disabled, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_levelOfStudy, keypath: UnicefDBSchema.ColumnNames.col_levelOfStudy, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_enrollmentDate, keypath: UnicefDBSchema.ColumnNames.col_enrollmentDate, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_facilityId, keypath: UnicefDBSchema.ColumnNames.col_facilityId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_beneficiaryCampId, keypath: UnicefDBSchema.ColumnNames.col_beneficiaryCampId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_block_id, keypath: UnicefDBSchema.ColumnNames.col_block_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_subBlockId, keypath: UnicefDBSchema.ColumnNames.col_subBlockId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_facilityName, keypath: UnicefDBSchema.ColumnNames.col_facilityName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_campName, keypath: UnicefDBSchema.ColumnNames.col_campName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_blockName, keypath: UnicefDBSchema.ColumnNames.col_blockName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_subBlockName, keypath: UnicefDBSchema.ColumnNames.col_subBlockName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_remarks, keypath: UnicefDBSchema.ColumnNames.col_remarks, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_collectionStatus, keypath: UnicefDBSchema.ColumnNames.col_collectionStatus, options: { unique: false } }, 
        ]
    };

    static FacilityIndicatorTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_facility_indicator, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_entityDynamicColumnId, keypath: UnicefDBSchema.ColumnNames.col_entityDynamicColumnId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instance_id, keypath: UnicefDBSchema.ColumnNames.col_instance_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_columnName, keypath: UnicefDBSchema.ColumnNames.col_columnName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_columnNameInBangla, keypath: UnicefDBSchema.ColumnNames.col_columnNameInBangla, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_columnDataType, keypath: UnicefDBSchema.ColumnNames.col_columnDataType, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_isMultiValued, keypath: UnicefDBSchema.ColumnNames.col_isMultiValued, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_list_id, keypath: UnicefDBSchema.ColumnNames.col_list_id, options: { unique: false } }, 

        ]
    };

    static BeneficiaryIndicatorTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_beneficiary_indicator, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_entityDynamicColumnId, keypath: UnicefDBSchema.ColumnNames.col_entityDynamicColumnId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instance_id, keypath: UnicefDBSchema.ColumnNames.col_instance_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_columnName, keypath: UnicefDBSchema.ColumnNames.col_columnName, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_columnNameInBangla, keypath: UnicefDBSchema.ColumnNames.col_columnNameInBangla, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_columnDataType, keypath: UnicefDBSchema.ColumnNames.col_columnDataType, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_isMultiValued, keypath: UnicefDBSchema.ColumnNames.col_isMultiValued, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_list_id, keypath: UnicefDBSchema.ColumnNames.col_list_id, options: { unique: false } }, 
        ]
    };

    static FacilityRecordsTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_facility_records, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_facilityId, keypath: UnicefDBSchema.ColumnNames.col_facilityId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instance_id, keypath: UnicefDBSchema.ColumnNames.col_instance_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_column_id, keypath: UnicefDBSchema.ColumnNames.col_column_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_value, keypath: UnicefDBSchema.ColumnNames.col_value, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_status, keypath: UnicefDBSchema.ColumnNames.col_status, options: { unique: false } }, 
        ]
    };

    static BeneficiaryDataCollectionStatusTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_beneficiary_data_collection_status, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_beneficiary_id, keypath: UnicefDBSchema.ColumnNames.col_beneficiary_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instance_id, keypath: UnicefDBSchema.ColumnNames.col_instance_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_status, keypath: UnicefDBSchema.ColumnNames.col_status, options: { unique: false } }, 
        ]
    };


    static FacilityDataCollectionStatusTable:ObjectStoreMeta = {
        store:UnicefDBSchema.TableNames.tbl_facility_data_collection_status, 
        storeConfig:{keyPath:UnicefDBSchema.ColumnNames.col_id, autoIncrement:true}, 
        storeSchema:[
            { name: UnicefDBSchema.ColumnNames.col_facilityId, keypath: UnicefDBSchema.ColumnNames.col_facilityId, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_instance_id, keypath: UnicefDBSchema.ColumnNames.col_instance_id, options: { unique: false } }, 
            { name: UnicefDBSchema.ColumnNames.col_status, keypath: UnicefDBSchema.ColumnNames.col_status, options: { unique: false } },  
        ]
    };
}

