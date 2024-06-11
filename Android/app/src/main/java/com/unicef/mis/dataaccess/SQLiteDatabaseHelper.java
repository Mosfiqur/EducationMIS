package com.unicef.mis.dataaccess;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.constants.DatabaseConstants;

public class SQLiteDatabaseHelper extends SQLiteOpenHelper {
    private ListDataAccess listDataAccess;
    private BeneficiaryDataAccess beneficiaryDataAccess;
    private FacilityDataAccess facilityDataAccess;
    private UserDataAccess userAccess;
    private CampDataAccess campDataAccess;
    private BlockDataAccess blockDataAccess;
    private SubBlockDataAccess subBlockDataAccess;
    private BeneficiaryActiveStatusDataAccess activeDataAccess;

    private SQLiteDatabaseHelper(Context context) {
        super(context, DatabaseConstants.DATABASE_NAME, null, DatabaseConstants.DATABASE_VERSION);
    }

    public static SQLiteDatabaseHelper getInstance(Context context){
        return new SQLiteDatabaseHelper(context);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL(DatabaseConstants.CREATE_LIST_OBJECT_TABLE);
        db.execSQL(DatabaseConstants.CREATE_LIST_ITEMS_TABLE);
        db.execSQL(DatabaseConstants.CREATE_USER_TABLE);
        db.execSQL(DatabaseConstants.CREATE_BENIFICARY_SCHEDULE_TABLE);
        db.execSQL(DatabaseConstants.CREATE_FACILITY_SCHEDULE_TABLE);
        db.execSQL(DatabaseConstants.CREATE_FACILITY_TABLE);
        db.execSQL(DatabaseConstants.CREATE_FACILITY_INDICATOR_TABLE);
        db.execSQL(DatabaseConstants.CREATE_BENEFICIARY_TABLE);
        db.execSQL(DatabaseConstants.CREATE_BENEFICIARY_INDICATOR_TABLE);
        db.execSQL(DatabaseConstants.CREATE_FACILITY_RECORDS_TABLE);
        db.execSQL(DatabaseConstants.CREATE_BENEFICIARY_RECORDS_TABLE);
        db.execSQL(DatabaseConstants.CREATE_BENEFCIARY_DATA_COLLECTION_STATUS_TABLE);
        db.execSQL(DatabaseConstants.CREATE_FACILITY_DATA_COLLECTION_STATUS_TABLE);
        db.execSQL(DatabaseConstants.CREATE_CAMP);
        db.execSQL(DatabaseConstants.CREATE_BLOCK);
        db.execSQL(DatabaseConstants.CREATE_SUB_BLOCK);
//        db.execSQL(DatabaseConstants.CREATE_TBL_FACILITY_GET_BY_ID);
//        db.execSQL(DatabaseConstants.CREATE_TBL_BENEFICIARY_GET_BY_ID);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        // on upgrade drop older tables
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENEFICIARY_DATA_COLLECTION_STATUS);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_FACILITY_DATA_COLLECTION_STATUS);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENEFICIARY_RECORDS);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_FACILITY_RECORDS);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENEFICIARY_INDICATOR);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENEFICIARY);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_FACILITY_INDICATOR);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_FACILITY);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_FACILITY_SCHEDULE);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENIFICARY_SCHEDULE);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_USER);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_LIST_ITEMS);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_LIST_OBJECT);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_CAMP);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BLOCK);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_SUB_BLOCK);
        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENEFICIARY);
//        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_FACILITY_GET_BY_ID);
//        db.execSQL(" DROP TABLE IF EXISTS " + DatabaseConstants.TBL_BENEFICIARY_GET_BY_ID);


        // create new tables
        onCreate(db);
    }

    public ListDataAccess getListDataAccess(){
        if(listDataAccess == null){
            listDataAccess = new ListDataAccess(this);
        }
        return listDataAccess;
    }

    public BeneficiaryDataAccess getBeneficiaryDataAccess(){
        if(beneficiaryDataAccess == null){
            beneficiaryDataAccess = new BeneficiaryDataAccess();
        }
        return beneficiaryDataAccess;
    }

    public FacilityDataAccess getFacilityDataAccess(){
        if (facilityDataAccess == null){
            facilityDataAccess = DataAccessFactory.getFacilityDataAccess();
        }

        return facilityDataAccess;
    }

    public CampDataAccess getCampDataAccess(){
        if (campDataAccess == null){
            campDataAccess = DataAccessFactory.getCampDataAccess();
        }

        return campDataAccess;
    }

    public BlockDataAccess getBlockDataAccess(){
        if (blockDataAccess == null){
            blockDataAccess = DataAccessFactory.getBlockDataAccess();
        }

        return blockDataAccess;
    }

    public SubBlockDataAccess getSubBlockDataAccess(){
        if (subBlockDataAccess == null){
            subBlockDataAccess = DataAccessFactory.getSubBlockDataAccess();
        }

        return subBlockDataAccess;
    }

    public BeneficiaryActiveStatusDataAccess getActiveDataAccess(){
      if (activeDataAccess == null){
          activeDataAccess = DataAccessFactory.getActiveDataAccess();
      }

      return activeDataAccess;
    }

    public UserDataAccess getUserDataAccess(){
        if (userAccess == null){
            userAccess = DataAccessFactory.getUserDataAccess();
        }

        return userAccess;
    }


}
