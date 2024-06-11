package com.unicef.mis.constants;

public class DatabaseConstants {
    // Database Version
    public static final int DATABASE_VERSION = 5;
    // Database Name
    public static final String DATABASE_NAME = "unicef_education_mis_db";
    //table name
    public static final String TBL_USER = "tbl_user";

    //User Table Coloumn Name
    public static final String COL_USER_ID = "id";
    public static final String COL_USER_FULLNAME = "fullname";
    public static final String COL_USER_LEVELID = "levelId";
    public static final String COL_USER_LEVELNAME = "levelname";
    public static final String COL_USER_LEVELRANK = "levelRank";
    public static final String COL_USER_DESIGNATIONID = "designationId";
    public static final String COL_USER_DESIGNATIONNAME = "designationname";
    public static final String COL_USER_ROLEID = "roleId";
    public static final String COL_USER_ROLENAME = "rolename";
    public static final String COL_USER_EMAIL = "email";
    public static final String COL_USER_PHONENUMBER = "phonenumber";
    public static final String COL_USER_EDUSECTORPARTNERS = "edusectorpartners";
    public static final String COL_USER_TOKEN = "token";
    public static final String COL_BENEFICIARY_SCHEDULEID = "scheduleId";
    public static final String COL_BENEFICIARY_TITLE = "title";
    public static final String COL_BENEFICIARY_DATACOLLECTIONDATE = "dataCollectionDate";
    public static final String COL_STATUS = "status";
    //GET Facility Schedule TABLE
    public static final String COL_ID = "id";
    public static final String COL_FACILITY_SCHEDULEID = "scheduleId";
    public static final String COL_FACILITY_TITLE = "title";
    public static final String COL_FACILITY_DATACOLLECTIONDATE = "dataCollectionDate";
    public static final String COL_END_DATE = "endDate";
    //facility get all column name
    public static final String COL_FACILITYGALL_ID = "id";
    public static final String COL_FACILITY_NAME = "facilityName";
    public static final String COL_FACILITY_CODE = "facilityCode";
    public static final String COL_CAMP_ID = "camp_id";
    public static final String COL_CAMP_NAME = "camp_name";
    public static final String COL_PROGRAMMING_PARTNER_NAME = "programPartnerName";
    public static final String COL_IMPLEMENTATION_PARTNER_NAME = "implementationPartnerName";
    //facility indicator list
    public static final String COL_ENTITY_DYNAMIC_COLUMN_ID = "entityDynamicColumnId";
    public static final String COL_SCHEDULE_ID_INDICATOR = "instanceId";
    public static final String COL_COLUMNORDER = "columnOrder";
    public static final String COL_COLUMN_NAME = "columnName";
    public static final String COL_COLUMN_NAME_BANGLA = "columnNameBangla";
    public static final String COL_COLUMN_DATA_TYPE = "columnDataType";
    public static final String COL_LISTOBJECTID = "listObjectId";
    public static final String COL_LISTOBJECTNAME = "listObjectName";
    //beneficiary Name List
    public static final String COL_ENTITY_ID = "entityId";
    public static final String COL_INSTANCE_ID = "instanceId";
    public static final String COL_FACILITY_ID = "facilityId";
    public static final String COL_BENEFICIARY_NAME = "beneficiaryName";
    public static final String COL_COLLECTION_STATUS = "collectionStatus";
    public static final String COL_IS_MULTI_VALUED = "isMultiValued";
    public static final String COL_LIST_ID = "listId";
    public static final String COL_NAME = "name";
    public static final String COL_VALUE = "value";
    public static final String COL_PROGRESS_ID = "progressId";
    public static final String COL_BENEFICIARY_ID = "beneficiaryId";
    public static final String COL_COLUMN_ID = "columnId";

    //camp
    public static final String ID = "id";
    public static final String SSID = "ssid";
    public static final String NAME = "name";
    public static final String NAME_ALIAS = "nameAlias";
    public static final String UNIONID = "unionId";

    //blocks
    public static final String CODE = "code";
    public static final String CAMP_ID ="campId";

    //subblocks
    public static final String COL_BLOCK_ID = "blockId";
    public static final String COL_SUB_BLOCK_ID = "subBlockId";

    //create bneficiary
    public static final String COL_UNHCR_ID = "unhcrId";
    public static final String COL_FATHER_NAME = "fatherName";
    public static final String COL_MOTHER_NAME = "motherName";
    public static final String COL_FCNID = "fcnId";
    public static final String COL_DATE_OF_BIRTH = "dateOfBirth";
    public static final String COL_SEX = "sex";
    public static final String COL_DISABLED = "disabled";
    public static final String COL_LEVEL_OF_STUDY = "levelOfStudy";
    public static final String COL_ENROLLMENT_DATE = "enrollmentDate";
    public static final String FACILITY_ID = "facilityId";
    public static final String COL_FACILITY_CAMP_ID = "facilityCampId";
    public static final String COL_BENEFICIARY_CAMP_ID = "beneficiaryCampId";
    public static final String COL_REMARKS = "remarks";
    public static final String COL_ISACTIVE = "isActive";

    //Facility Get By ID
    public static final String COL_PROGRAM_PARTNER_NAME = "programPartnerName";
    public static final String IMPLEMENTATION_PARTNER_NAME = "implementationPartnerName";
    public static final String COL_UNION_NAME = "unionName";
    public static final String COL_UPZILLA_NAME = "upazilaName";
    public static final String CAMP_NAME = "campName";
    public static final String COL_BLOCK_NAME = "blockName";
    public static final String COL_TEACHER_NAME = "teacherName";

    //Benefixiary Get BY ID
    public static final String FACILITY_NAME = "facilityName";
    public static final String BENEFICARY_CAMP_NAME = "beneficiaryCampName";
    public static final String SUB_BLOCK_NAME = "subBlockName";

    //Database Query
    public static final String CREATE_USER_TABLE = "create table " + TBL_USER + " ( " +
            "id INTEGER PRIMARY KEY, " +
            "fullname TEXT, " +
            "levelid Integer, " +
            "levelname TEXT, " +
            "levelrank Integer, " +
            "designationid Integer, " +
            "designationname TEXT, " +
            "roleid Integer, " +
            "email TEXT, " +
            "rolename TEXT, " +
            "phonenumber TEXT, " +
            "token TEXT)";

    public static final String TBL_CAMP= "tbl_camp";
    public static final String CREATE_CAMP = " create table " + TBL_CAMP + "(" +
            " id INTEGER PRIMARY KEY, " +
            " ssid TEXT NOT NULL," +
            " name TEXT NOT NULL," +
            " nameAlias TEXT NOT NULL," +
            " unionId TEXT NOT NULL)";

    public static final String TBL_BLOCK ="tbl_block";
    public static final String CREATE_BLOCK= " create table " + TBL_BLOCK + " ( " +
            " id INTEGER PRIMARY KEY, " +
            " code TEXT NOT NULL, " +
            " name TEXT NOT NULL, "+
            " campId TEXT NOT NULL," +
            " FOREIGN KEY (campId) REFERENCES " + TBL_CAMP +"(id))";

    public static final String TBL_SUB_BLOCK =" tbl_sub_block";
    public static final String CREATE_SUB_BLOCK = " create table " +TBL_SUB_BLOCK + "(" +
            " id INTEGER PRIMARY KEY, " +
            " name TEXT NOT NULL," +
            " blockId TEXT NOT NULL," +
            " FOREIGN KEY (blockId) REFERENCES " + TBL_BLOCK + "(id))";



    public static final String TBL_BENIFICARY_SCHEDULE = "tbl_beneficiary_schedule";
    public static final String CREATE_BENIFICARY_SCHEDULE_TABLE = "create table " + TBL_BENIFICARY_SCHEDULE + " ( " +
            "id         INTEGER PRIMARY KEY,  " +
            "scheduleId INTEGER NOT NULL,  " +
            "title TEXT NOT NULL, " +
            "dataCollectionDate TEXT,  " +
            "endDate TEXT," +
            "status Integer)";
    public static final String TBL_FACILITY_SCHEDULE = "tbl_facility_schedule";
    public static final String CREATE_FACILITY_SCHEDULE_TABLE = "create table " + TBL_FACILITY_SCHEDULE + " ( " +
            "id INTEGER PRIMARY KEY, " +
            "scheduleId INTEGER, " +
            "title TEXT, " +
            "dataCollectionDate TEXT, " +
            "endDate TEXT," +
            "status Integer)";
    public static final String TBL_FACILITY = "tbl_facility";
    public static final String CREATE_FACILITY_TABLE = "create table " + TBL_FACILITY + " ( " +
            "id INTEGER PRIMARY KEY, " +
            "facilityName TEXT, " +
            "facilityCode TEXT, " +
            "camp_id Integer, " +
            "camp_name TEXT, " +
            "programPartnerName TEXT, " +
            "implementationPartnerName TEXT, " +
            "unionName      TEXT," +
            "upazilaName    TEXT," +
            "campName   TEXT," +
            "blockName      TEXT," +
            "teacherName    TEXT)";

    public static final String TBL_BENEFICIARY = "tbl_beneficiary";
    //progress id will be unchr id
    public static final String CREATE_BENEFICIARY_TABLE = "create table " + TBL_BENEFICIARY + " ( " +
            " id INTEGER PRIMARY KEY AUTOINCREMENT," +
            " entityId INTEGER, " +
            " isActive INTEGER NOT NULL, " +
            " facilityId INTEGER NOT NULL, " +
            " beneficiaryName TEXT NOT NULL, " +
            " unhcrId TEXT, " +
            " fatherName TEXT NOT NULL, " +
            " motherName TEXT NOT NULL, " +
            " fcnId TEXT, " +
            " dateOfBirth TEXT NOT NULL, " +
            " sex INTEGER NOT NULL, " +
            " disabled INTEGER NOT NULL, " +
            " levelOfStudy INTEGER NOT NULL, " +
            " enrollmentDate TEXT NOT NULL, " +
            " facilityCampId INTEGER, " +
            " beneficiaryCampId INTEGER NOT NULL, " +
            " blockId INTEGER NOT NULL, " +
            " subBlockId INTEGER NOT NULL, " +
            " remarks TEXT, " +
            " facilityName       TEXT," +
            " beneficiaryCampName    TEXT," +
            " blockName      TEXT," +
            " subBlockName   TEXT," +
            " FOREIGN KEY (beneficiaryCampId) REFERENCES " + TBL_CAMP + "(id)," +
            " FOREIGN KEY (blockId) REFERENCES " + TBL_BLOCK + "(id)," +
            " FOREIGN KEY (subBlockId) REFERENCES " + TBL_SUB_BLOCK  + "(id)," +
            " FOREIGN KEY(facilityId) REFERENCES " + TBL_FACILITY + "(id))";
    public static final String TBL_BENEFICIARY_INDICATOR = "tbl_beneficiary_indicator";
    public static final String TBL_FACILITY_RECORDS = "tbl_facility_records";
    public static final String CREATE_FACILITY_RECORDS_TABLE = "create table " + TBL_FACILITY_RECORDS + "( " +
            "id             INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "facilityId     INTEGER, " +
            "instanceId    INTEGER, " +
            "columnId      INTEGER, " +
            "value          TEXT, " +
            "status         INTEGER, " +
            "FOREIGN KEY(facilityId) REFERENCES " + TBL_FACILITY + "(id), " +
            "FOREIGN KEY(instanceId) REFERENCES " + TBL_FACILITY_SCHEDULE + "(id))";
//            "FOREIGN KEY(columnId) REFERENCES " + TBL_FACILITY_INDICATOR + "(id))";
    public static final String TBL_BENEFICIARY_RECORDS = "tbl_beneficiary_records";
    public static final String CREATE_BENEFICIARY_RECORDS_TABLE = "create table " + TBL_BENEFICIARY_RECORDS + "( " +
            "id             INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "beneficiaryId  INTEGER, " +
            "instanceId     INTEGER, " +
            "columnId       INTEGER, " +
            "value          TEXT, " +
            "status         INTEGER, " +
            "FOREIGN KEY(beneficiaryId) REFERENCES " + TBL_BENEFICIARY + "(id), " +
            "FOREIGN KEY(instanceId) REFERENCES " + TBL_BENIFICARY_SCHEDULE + "(id))";
    public static final String TBL_LIST_OBJECT = "tbl_list_object";
    public static final String CREATE_BENEFICIARY_INDICATOR_TABLE = "create table " + TBL_BENEFICIARY_INDICATOR + " ( " +
            "id  INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "entityDynamicColumnId INTEGER, " +
            "instanceId INTEGER, " +
            "columnName TEXT, " +
            "columnNameBangla TEXT, " +
            "columnDataType INTEGER, " +
            "isMultiValued  INTEGER, " +
            "listId    INTEGER, " +
            "FOREIGN KEY (listId) REFERENCES " + TBL_LIST_OBJECT + "(id), " +
            "FOREIGN KEY(instanceId) REFERENCES " + TBL_BENIFICARY_SCHEDULE + "(id))";
    public static final String TBL_FACILITY_INDICATOR = "tbl_facility_indicator";
    public static final String CREATE_FACILITY_INDICATOR_TABLE = "create table " + TBL_FACILITY_INDICATOR + " ( " +
            "id  INTEGER PRIMARY KEY AUTOINCREMENT, " +
            "entityDynamicColumnId  INTEGER, " +
            "instanceId INTEGER, " +
            "columnName TEXT, " +
            "columnNameBangla TEXT, " +
            "columnDataType INTEGER, " +
            "isMultiValued  INTEGER, " +
            "listId    INTEGER, " +
            "FOREIGN KEY (listId) REFERENCES " + TBL_LIST_OBJECT + "(id), " +
            "FOREIGN KEY (instanceId) REFERENCES " + TBL_FACILITY_SCHEDULE + "(id))";
    public static final String CREATE_LIST_OBJECT_TABLE = "create table " + TBL_LIST_OBJECT + " ( " +
            "id INTEGER PRIMARY KEY, " +
            "name TEXT NOT NULL)";

    public static final String TBL_LIST_ITEMS = "tbl_list_items";
    public static final String CREATE_LIST_ITEMS_TABLE = "create table " + TBL_LIST_ITEMS + " ( " +
            "id INTEGER PRIMARY KEY, " +
            "listId INTEGER NOT NULL, " +
            "title   TEXT NOT NULL, " +
            "value   INTEGER NOT NULL, " +
            "FOREIGN KEY(listId) REFERENCES " + TBL_LIST_OBJECT + "(id))";
    public static final String TBL_BENEFICIARY_DATA_COLLECTION_STATUS = "tbl_beneficiary_data_collection_status";
    public static final String CREATE_BENEFCIARY_DATA_COLLECTION_STATUS_TABLE = "create table " + TBL_BENEFICIARY_DATA_COLLECTION_STATUS + "( " +
            "beneficiaryId      INTEGER, " +
            "instanceId         INTEGER, " +
            "status             INTEGER,  " +
            "FOREIGN KEY(beneficiaryId) REFERENCES " + TBL_BENEFICIARY + "(id), " +
            "FOREIGN KEY(instanceId) REFERENCES " + TBL_BENIFICARY_SCHEDULE + "(id))";
    public static final String TBL_FACILITY_DATA_COLLECTION_STATUS = "tbl_facility_data_collection_status";
    public static final String CREATE_FACILITY_DATA_COLLECTION_STATUS_TABLE = "create table " + TBL_FACILITY_DATA_COLLECTION_STATUS + "( " +
            "facilityId         INTEGER, " +
            "instanceId         INTEGER, " +
            "status             INTEGER, " +
            "FOREIGN KEY(facilityId) REFERENCES " + TBL_FACILITY + "(id), " +
            "FOREIGN KEY(instanceId) REFERENCES " + TBL_FACILITY_SCHEDULE + "(id))";

//    public static final String TBL_FACILITY_GET_BY_ID ="tbl_facility_get_by_id";
//    public static final String CREATE_TBL_FACILITY_GET_BY_ID  = " create table " + TBL_FACILITY_GET_BY_ID +"(" +
//            "id     INTEGER," +
//            "programPartnerName     TEXT," +
//            "implementationPartnerName      TEXT," +
//            "unionName      TEXT," +
//            "upazilaName    TEXT," +
//            "campName   TEXT," +
//            "blockName      TEXT," +
//            "teacherName    TEXT," +
//            "FOREIGN KEY(id) REFERENCES " + TBL_FACILITY + "(id))";
//
//    public static final String TBL_BENEFICIARY_GET_BY_ID = "tbl_beneficiary_get_by_id";
//    public static final String CREATE_TBL_BENEFICIARY_GET_BY_ID  = " create table " + TBL_BENEFICIARY_GET_BY_ID + "(" +
//            "beneficiaryId      INTEGER," +
//            "id     INTEGER," +
//            "facilityName       TEXT," +
//            "beneficiaryCampName    TEXT," +
//            "blockName      TEXT," +
//            "subBlockName   TEXT," +
//            "FOREIGN KEY(id) REFERENCES " + TBL_BENIFICARY_SCHEDULE + "(id), " +
//            "FOREIGN KEY(beneficiaryId) REFERENCES " + TBL_FACILITY_SCHEDULE + "(entityId))";

}
