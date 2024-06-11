package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.text.TextUtils;

import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.enumtype.DataTypes;
import com.unicef.mis.enumtype.MultiValuedType;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.CreateBeneficiaryModel;
import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.ListItem;
import com.unicef.mis.model.ListObject;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.PropertiesInfoModel;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.indicator.BeneficiaryIndicatorModel;
import com.unicef.mis.model.benificiary.indicator.Datum;
import com.unicef.mis.model.benificiary.indicator.Indicator;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryDynamicCell;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.FormatUtils;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.BENEFICARY_CAMP_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_BENEFICIARY_CAMP_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_BLOCK_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_BENEFICIARY_DATACOLLECTIONDATE;
import static com.unicef.mis.constants.DatabaseConstants.COL_BENEFICIARY_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_BENEFICIARY_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_BENEFICIARY_SCHEDULEID;
import static com.unicef.mis.constants.DatabaseConstants.COL_BLOCK_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_NAME_BANGLA;
import static com.unicef.mis.constants.DatabaseConstants.COL_END_DATE;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_STATUS;
import static com.unicef.mis.constants.DatabaseConstants.COL_BENEFICIARY_TITLE;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLLECTION_STATUS;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_DATA_TYPE;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_ENTITY_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_ENTITY_DYNAMIC_COLUMN_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_INSTANCE_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_IS_MULTI_VALUED;
import static com.unicef.mis.constants.DatabaseConstants.COL_LIST_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_VALUE;
import static com.unicef.mis.constants.DatabaseConstants.COL_DATE_OF_BIRTH;
import static com.unicef.mis.constants.DatabaseConstants.COL_DISABLED;
import static com.unicef.mis.constants.DatabaseConstants.COL_ENROLLMENT_DATE;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_CAMP_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_FATHER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_FCNID;
import static com.unicef.mis.constants.DatabaseConstants.COL_ISACTIVE;
import static com.unicef.mis.constants.DatabaseConstants.COL_LEVEL_OF_STUDY;
import static com.unicef.mis.constants.DatabaseConstants.COL_MOTHER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_REMARKS;
import static com.unicef.mis.constants.DatabaseConstants.COL_SEX;
import static com.unicef.mis.constants.DatabaseConstants.COL_SUB_BLOCK_ID;
import static com.unicef.mis.constants.DatabaseConstants.FACILITY_NAME;
import static com.unicef.mis.constants.DatabaseConstants.ID;
import static com.unicef.mis.constants.DatabaseConstants.SUB_BLOCK_NAME;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BENEFICIARY;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BENEFICIARY_DATA_COLLECTION_STATUS;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BENEFICIARY_INDICATOR;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BENEFICIARY_RECORDS;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BENIFICARY_SCHEDULE;
import static com.unicef.mis.constants.DatabaseConstants.COL_UNHCR_ID;


public class BeneficiaryDataAccess {
    private static String TAG = BeneficiaryDataAccess.class.getSimpleName();
    private SQLiteDatabaseHelper dbHelper;
    private String serverDateFormat = "yyyy/MM/dd";

    public BeneficiaryDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public void saveBeneficiaryIndicators(Integer instanceId, List<IndicatorNew> indicators) {
        for (IndicatorNew indicator : indicators) {
            saveOrUpdateBeneficiaryIndicator(instanceId, indicator);
        }
    }

    public long saveOrUpdateBeneficiaryIndicator(Integer instanceId, IndicatorNew indicator) {
        long indicatorId = getBeneficiaryIndicatorId(instanceId, indicator);
        if (indicatorId > 0) {
            return indicatorId;
        }
        return saveBeneficiaryIndicator(instanceId, indicator);
    }

    public long getBeneficiaryIndicatorId(Integer instanceId, IndicatorNew indicator) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select id from " + TBL_BENEFICIARY_INDICATOR +
                    " where entityDynamicColumnId=" + indicator.getEntityDynamicColumnId() +
                    " and instanceId=" + instanceId.toString();
            cursor = database.rawQuery(query, null);
            long indicatorId = 0;
            if (cursor.moveToFirst()) {
                do {
                    indicatorId = cursor.getInt(0);
                }
                while (cursor.moveToNext());
            }
            return indicatorId;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public long saveBeneficiaryIndicator(Integer instanceId, IndicatorNew indicator) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_ENTITY_DYNAMIC_COLUMN_ID, indicator.getEntityDynamicColumnId());
            cv.put(COL_INSTANCE_ID, instanceId);
            cv.put(COL_COLUMN_NAME, indicator.getColumnName());
            cv.put(COL_COLUMN_NAME_BANGLA, indicator.getColumnNameInBangla());
            cv.put(COL_COLUMN_DATA_TYPE, indicator.getColumnDataType());
            if (indicator.getMultiValued() == true) {
                cv.put(COL_IS_MULTI_VALUED, MultiValuedType.mutiValueType.getIntValue());
            } else {
                cv.put(COL_IS_MULTI_VALUED, MultiValuedType.singleValueType.getIntValue());
            }

            if (indicator.getListObject() != null) {
                cv.put(COL_LIST_ID, indicator.getListObject().getId());
            }
            long indicatorId = database.insert(TBL_BENEFICIARY_INDICATOR, null, cv);
            return indicatorId;
        } finally {
            database.close();
        }
    }

    public boolean insertBenificarySchedule(ScheduledInstance instance) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        try {
            ContentValues contentValues = new ContentValues();

            contentValues.put(COL_ID, instance.getId());
            contentValues.put(COL_BENEFICIARY_SCHEDULEID, instance.getScheduleId());
            contentValues.put(COL_BENEFICIARY_TITLE, instance.getTitle());
            contentValues.put(COL_BENEFICIARY_DATACOLLECTIONDATE, instance.getDataCollectionDate());
            contentValues.put(COL_END_DATE, instance.getEndDate());
            contentValues.put(COL_STATUS, instance.getStatus());

            long result = db.insert(TBL_BENIFICARY_SCHEDULE, null, contentValues);

            if (result == -1)
                return false;
            else
                return true;
        } finally {
            db.close();
        }
    }

    public void saveOrUpdateBeneficiarySchedule(Schedule schedule) {
        for (ScheduledInstance instance : schedule.getData()) {
            if (instance.getId() == Singleton.getInstance().getIdInstance()) {
                saveOrUpdateInstance(instance);
            }
        }
    }

    public void saveOrUpdateInstance(ScheduledInstance instance) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_BENIFICARY_SCHEDULE + " where id = " + instance.getId().toString();
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            if (count > 0) {
                updateInstance(instance);
            } else {
                insertBenificarySchedule(instance);
            }
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public void updateInstance(ScheduledInstance instance) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_BENEFICIARY_TITLE, instance.getTitle());
            cv.put(COL_BENEFICIARY_DATACOLLECTIONDATE, instance.getDataCollectionDate());
            cv.put(COL_STATUS, instance.getStatus());
            database.update(TBL_BENIFICARY_SCHEDULE, cv, "id=" + instance.getId().toString(), null);
        } finally {
            database.close();
        }
    }

    public List<ScheduledInstance> getBeneficiarySchedule() {
        // Data model list in which we have to return the data
        List<ScheduledInstance> data = new ArrayList<ScheduledInstance>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_BENIFICARY_SCHEDULE +" ORDER By id DESC";
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    ScheduledInstance data_model = new ScheduledInstance(cursor.getInt(0), cursor.getInt(1), cursor.getString(2),
                            cursor.getString(3), cursor.getString(4), cursor.getInt(5));
                    data.add(data_model);
                } while (cursor.moveToNext());
            }
        } finally {
            // After using cursor we have to close it
            cursor.close();
        }

        // Closing database
        db.close();

        // returning list
        return data;

    }

    public List<Beneficiary> getBeneficiaryList(int facilityId, int instanceId) {
        // Data model list in which we have to return the data
        List<Beneficiary> data = new ArrayList<Beneficiary>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_BENEFICIARY + " where " + COL_FACILITY_ID + "=?";
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, new String[]{String.valueOf(facilityId)});
        Beneficiary benificiary = null;


        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    Beneficiary beneficiary = readBeneficiaryFromCursor(cursor);
                    boolean allDataCollected = isAllDataCollected(beneficiary.getEntityId(), instanceId);
                    int collectionStatus = allDataCollected ? CollectionStatus.Collected.getIntValue() : CollectionStatus.NotCollected.getIntValue();
//                    beneficiary.setCollectionStatus(collectionStatus);
                    beneficiary.setCollectionStatus(beneficiary.getCollectionStatus());
                    data.add(beneficiary);

                } while (cursor.moveToNext());
            }

        } finally {
            // After using cursor we have to close it
            if (cursor != null) {
                cursor.close();
            }
            // Closing database
            db.close();
        }


        // returning list
        return data;

    }

    public List<Beneficiary> getBeneficiaryListUpload(int facilityId) {
        // Data model list in which we have to return the data
        List<Beneficiary> data = new ArrayList<Beneficiary>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_BENEFICIARY + " where " + COL_FACILITY_ID + "=?" + " and " + COL_COLLECTION_STATUS + " = 2";
        System.out.println(select_query);
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, new String[]{String.valueOf(facilityId)});
        Beneficiary benificiary = null;
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    Beneficiary beneficiary = readBeneficiaryFromCursor(cursor);
                    data.add(beneficiary);
                } while (cursor.moveToNext());
            }

//            for (Benificiary b:data){
//                b.setCollectionStatus(isAllDataCollected(b.getEntityId(), instanceId)?CollectionStatus.Collected.getIntValue():CollectionStatus.NotCollected.getIntValue());
//            }
        } finally {
            // After using cursor we have to close it
            if (cursor != null) {
                cursor.close();
            }
            // Closing database
            db.close();
        }


        // returning list
        return data;

    }


    public Beneficiary getBenificiary(int benificiaryId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_BENEFICIARY + " where " + COL_ENTITY_ID + "=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(benificiaryId)});
            Beneficiary benificiary = null;
            if (cursor.moveToFirst()) {
                do {
                    benificiary = readBeneficiaryFromCursor(cursor);
                } while (cursor.moveToNext());
            }
            return benificiary;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private Beneficiary readBeneficiaryFromCursor(Cursor cursor) {
        Beneficiary beneficiary = new Beneficiary();

        beneficiary.setId(cursor.getInt(0));
        beneficiary.setEntityId(cursor.getInt(1));
        beneficiary.setFacilityId(cursor.getInt(3));
        beneficiary.setBeneficiaryName(cursor.getString(4));
        beneficiary.setUnhcrId(cursor.getString(5));

        return beneficiary;
    }

    public void saveBeneficiaries(int instanceId, List<Beneficiary> beneficiaries) {
        for (Beneficiary beneficiary : beneficiaries) {
            int beneficiaryId = saveOrUpdateBeneficiary(beneficiary);
            beneficiary.setId(beneficiaryId);
            saveOrUpdateDataCollectionStatus(instanceId, beneficiary);
            saveBeneficiaryRecords(instanceId, beneficiary);
        }
    }

    private void saveOrUpdateDataCollectionStatus(int instanceId, Beneficiary beneficiary) {
        if (!hasBenificiaryDataCollectionStatus(instanceId, beneficiary.getId())) {
            saveBeneficiaryCollectionStatus(instanceId, beneficiary);
        }
    }

    private boolean hasBenificiaryDataCollectionStatus(int instanceId, int beneficiaryId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_BENEFICIARY_DATA_COLLECTION_STATUS + " where " + COL_INSTANCE_ID + "=? AND " +
                    COL_BENEFICIARY_ID + "=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(instanceId), String.valueOf(beneficiaryId)});
            return cursor.getCount() > 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private int saveBeneficiaryCollectionStatus(int instanceId, Beneficiary beneficiary) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_BENEFICIARY_ID, beneficiary.getId());
            cv.put(COL_INSTANCE_ID, instanceId);
            cv.put(COL_STATUS, beneficiary.getCollectionStatus());

            long newStatus = database.insert(TBL_BENEFICIARY_DATA_COLLECTION_STATUS, null, cv);
            return (int) newStatus;
        } finally {
            database.close();
        }
    }

    private void saveBeneficiaryRecords(Integer instanceId, Beneficiary beneficiary) {
        if (doesAnyRecordExists(beneficiary.getId(), instanceId)) {
            return;
        }

        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            database.beginTransaction();
            for (PropertiesInfoModel indicator : beneficiary.getIndicators()) {
                String recordValue = "";
                List<String> values = indicator.getValues();
                if (values != null && values.size() > 0) {
                    recordValue = android.text.TextUtils.join(",", values);
                }
                ContentValues cv = new ContentValues();
                cv.put(COL_BENEFICIARY_ID, beneficiary.getId());
                cv.put(COL_INSTANCE_ID, instanceId);
                long columnId = indicator.getEntityColumnId();
                cv.put(COL_COLUMN_ID, columnId);
                cv.put(COL_STATUS, indicator.getStatus());
                cv.put(COL_VALUE, recordValue);
                database.insert(TBL_BENEFICIARY_RECORDS, null, cv);
            }
            database.setTransactionSuccessful();
        } finally {
            database.endTransaction();
            database.close();
        }
    }

    public ArrayList<ListObject> getListsByInstance(int instanceId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "WITH A as  " +
                    "(" +
                    "select distinct listId " +
                    "from tbl_beneficiary_indicator " +
                    "where instanceId=? " +
                    "and columnDataType = 6 " +
                    ") " +
                    "select * " +
                    "from tbl_list_object l " +
                    "inner join A on l.id = A.listId " +
                    "inner join tbl_list_items li on li.listId = l.id; ";
            cursor = database.rawQuery(query, new String[]{String.valueOf(instanceId)});
            ArrayList<ListObject> listObjects = new ArrayList<ListObject>();
            //TODO: Implement reading
            if (cursor.moveToFirst()) {
                do {
//
//                    BeneficiaryIndicator indicator = new BeneficiaryIndicator();
//                    indicator.setId(cursor.getInt(0));
//                    indicator.setDynamicColumnId(cursor.getInt(1));
//                    indicator.setInstanceId(cursor.getInt(2));
//                    indicator.setColumnName(cursor.getString(3));
//                    indicator.setColumnDataType(cursor.getInt(4));
//                    indicator.setIsMultiValued(cursor.getInt(5));
//                    indicator.setListId(cursor.getInt(6));
//                    listObjects.add(indicator);
                } while (cursor.moveToNext());
                return null;
            }
            return null;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public ArrayList<PropertiesInfoModel> getIndicators(int instanceId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select * " +
                    "from tbl_beneficiary_indicator a " +
                    "left join tbl_list_object l on a.listId = l.id " +
                    "where a.instanceId=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(instanceId)});
            ArrayList<PropertiesInfoModel> indicators = new ArrayList<PropertiesInfoModel>();
            if (cursor.moveToFirst()) {
                do {
                    PropertiesInfoModel indicator = new PropertiesInfoModel();
                    indicator.setId(cursor.getInt(0));
                    indicator.setEntityColumnId(cursor.getInt(1));
                    indicator.setInstanceId(cursor.getInt(2));
                    indicator.setColumnName(cursor.getString(3));
                    indicator.setDataType(cursor.getInt(4));
                    boolean isMultiValued = cursor.getInt(5) > 0;
                    indicator.setIsMultiValued(isMultiValued);
                    indicator.setColumnListId(cursor.getInt(6));
                    indicators.add(indicator);
                } while (cursor.moveToNext());
                return indicators;
            }
            return indicators;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public boolean doesAnyRecordExists(Integer beneficiaryId, Integer instanceId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select id from " + TBL_BENEFICIARY_RECORDS +
                    " where " + COL_BENEFICIARY_ID + "=? and " +
                    COL_INSTANCE_ID + "=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(beneficiaryId), String.valueOf(instanceId)});
            int count = cursor.getCount();
            return count > 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private int saveOrUpdateBeneficiary(Beneficiary benificiary) {
        Beneficiary dbBeneficiary = getBenificiary(benificiary.getEntityId());
        if (dbBeneficiary == null) {
            return saveBeneficiary(benificiary);
        } else {
            updateBeneficiary(benificiary);
            return dbBeneficiary.getId();
        }
    }

    private void updateBeneficiary(Beneficiary beneficiary) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();

        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_BENEFICIARY_NAME, beneficiary.getBeneficiaryName());
            cv.put(COL_UNHCR_ID, beneficiary.getUnhcrId());
            database.update(TBL_BENEFICIARY, cv, "entityId=?", new String[]{String.valueOf(beneficiary.getEntityId())});
        } finally {
            database.close();
        }
    }


    public int saveBeneficiary(Beneficiary beneficiary) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();

            cv.put(COL_ENTITY_ID, beneficiary.getEntityId());
            cv.put(COL_ISACTIVE, beneficiary.getActive() ? 1 : 0);
            cv.put(COL_FACILITY_ID, beneficiary.getFacilityId());
            cv.put(COL_BENEFICIARY_NAME, beneficiary.getBeneficiaryName());
            if (beneficiary.getUnhcrId() != null) {
                cv.put(COL_UNHCR_ID, beneficiary.getUnhcrId());
            }
            cv.put(COL_FATHER_NAME, beneficiary.getFatherName());
            cv.put(COL_MOTHER_NAME, beneficiary.getMotherName());
            if (beneficiary.getFcnId() != null) {
                cv.put(COL_FCNID, beneficiary.getFcnId());
            }
            cv.put(COL_DATE_OF_BIRTH, beneficiary.getDateOfBirth());
            cv.put(COL_SEX, beneficiary.getSex());
            cv.put(COL_DISABLED, beneficiary.getDisabled() ? 1 : 0);
            cv.put(COL_LEVEL_OF_STUDY, beneficiary.getLevelOfStudy());
            cv.put(COL_ENROLLMENT_DATE, beneficiary.getEnrollmentDate());
            cv.put(COL_FACILITY_CAMP_ID, beneficiary.getFacilityCampId());
            cv.put(COL_BENEFICIARY_CAMP_ID, beneficiary.getBeneficiaryCampId());
            cv.put(COL_BLOCK_ID, beneficiary.getBlockId());
            cv.put(COL_SUB_BLOCK_ID, beneficiary.getSubBlockId());
            if (beneficiary.getRemarks() != null) {
                cv.put(COL_REMARKS, beneficiary.getRemarks());
            }

            long newId = database.insert(TBL_BENEFICIARY, null, cv);
            return (int) newId;
        } finally {
            database.close();
        }
    }

    public void saveBeneficiaryIndicatorData(PropertiesInfoModel indicator, IGenericApiCallBack callBack) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            String recordValue = "";
            List<String> values = indicator.getValues();
            if (values != null && values.size() > 0) {
                recordValue = android.text.TextUtils.join(",", values);
            }

            ContentValues cv = new ContentValues();
            cv.put(COL_VALUE, recordValue);
            cv.put(COL_STATUS, CollectionStatus.Collected.getIntValue());

            String whereClause = COL_ID + "=" + indicator.getId();
            database.update(TBL_BENEFICIARY_RECORDS, cv, whereClause, null);

            callBack.apiCallSuccessful(null, null);
        } catch (Exception e) {
            e.printStackTrace();
            callBack.apiCallFailed(true, e.getMessage());
        } finally {
            database.close();
        }
    }

    private boolean isAllDataCollected(long beneficiaryId, long instanceId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select 1 from " + TBL_BENEFICIARY_RECORDS +
                    " where " + COL_BENEFICIARY_ID + "=?  " +
                    "and " + COL_INSTANCE_ID + "=?  " +
                    "and " + COL_STATUS + "=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(beneficiaryId), String.valueOf(instanceId),
                    String.valueOf(CollectionStatus.NotCollected.getIntValue())});
            int count = cursor.getCount();
            return count == 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public Beneficiary getBeneficiary(int instanceId, int beneficiaryId) {
        Beneficiary benificiary = getBenificiary(beneficiaryId);
        if (benificiary != null) {
            ArrayList<PropertiesInfoModel> indicators = getBeneficiaryIndicators(instanceId, beneficiaryId);
            benificiary.setIndicators(indicators);
        }
        return benificiary;
    }

    public ArrayList<PropertiesInfoModel> getBeneficiaryIndicators(int instanceId, int beneficiaryId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select a.id, b.entityDynamicColumnId, b.columnName, b.columnNameBangla, b.columnDataType,  " +
                    "b.isMultiValued, b.listId, a.status, a.value " +
                    "from tbl_beneficiary_records a " +
                    "inner join tbl_beneficiary_indicator b on a.columnId = b.id  " +
                    "where a.beneficiaryId=? " +
                    "and a.instanceId=? ";
            cursor = database.rawQuery(query, new String[]{String.valueOf(beneficiaryId), String.valueOf(instanceId)});
            ArrayList<PropertiesInfoModel> benIndicators = new ArrayList<PropertiesInfoModel>();
            if (cursor.moveToFirst()) {
                do {
                    PropertiesInfoModel indicator = new PropertiesInfoModel();
                    indicator.setId(cursor.getInt(0));
                    indicator.setEntityColumnId(cursor.getInt(1));
                    indicator.setColumnName(cursor.getString(2));
                    indicator.setColumnNameInBangla(cursor.getString(3));
                    indicator.setDataType(cursor.getInt(4));
                    boolean isMultiValued = cursor.getInt(5) > 0;
                    indicator.setIsMultiValued(isMultiValued);
                    indicator.setColumnListId(cursor.getInt(6));
                    indicator.setStatus(cursor.getInt(7));
                    List<String> values = new ArrayList<String>();
                    String strValue = cursor.getString(8);
                    if (strValue != null && strValue.length() > 0) {
                        values.add(strValue);
                    }
                    indicator.setValues(values);
                    benIndicators.add(indicator);
                } while (cursor.moveToNext());
            }
            return benIndicators;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    //delete tables
    public boolean deleteBeneficiaryRecords(int beneficiaryId, int instanceId) {

        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "DELETE FROM " + TBL_BENEFICIARY_RECORDS +
                    " where " + COL_BENEFICIARY_ID + "=? and " +
                    COL_INSTANCE_ID + "=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(beneficiaryId), String.valueOf(instanceId), String.valueOf(CollectionStatus.NotCollected.getIntValue())});
            int count = cursor.getCount();
            return count == 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public void deleteBeneficiary(int beneficiaryId, int instanceId) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            database.beginTransaction();
            String whereClause = COL_BENEFICIARY_ID + "=? and " + COL_INSTANCE_ID + "=?";
            database.delete(TBL_BENEFICIARY_RECORDS, whereClause, new String[]{String.valueOf(beneficiaryId), String.valueOf(instanceId)});
            whereClause = COL_ENTITY_ID + "=?";
            database.delete(TBL_BENEFICIARY, whereClause, new String[]{String.valueOf(beneficiaryId)});
            database.setTransactionSuccessful();
        } finally {
            database.endTransaction();
            database.close();
        }
    }

    public List<Beneficiary> updateCollectionStatus(int status) {
        List<Beneficiary> data = new ArrayList<>();

        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data

        String select_query = "Update " + TBL_BENEFICIARY + " set " + COL_COLLECTION_STATUS + "='" + 2 + "'" + " where " + COL_ENTITY_ID + "='" + status + "'";
        System.out.println(select_query);
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    Beneficiary data_model = new Beneficiary(cursor.getInt(0));
                    data.add(data_model);
                } while (cursor.moveToNext());
            }
        } finally {
            // After using cursor we have to close it
            cursor.close();
        }

        // Closing database
        db.close();
        return data;
    }

    private int getBeneficiaryCount(int facilityId, String searchText) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String countQuery = "select count(1) from " + TBL_BENEFICIARY + " " +
                "where " + COL_FACILITY_ID + " = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            countQuery = countQuery + " and ( " + COL_BENEFICIARY_NAME + " like '%" + searchText + "%' or " +
                    COL_UNHCR_ID + " like '%" + searchText + "%' ) ";
        }
        Cursor countCursor = null;
        int totalCount = 0;
        try {
            countCursor = database.rawQuery(countQuery, new String[]{String.valueOf(facilityId)});
            if (countCursor.moveToFirst()) {
                do {
                    totalCount = countCursor.getInt(0);
                } while (countCursor.moveToNext());
            }
            return totalCount;
        } finally {
            if (countCursor != null) {
                countCursor.close();
            }
        }
    }

    public PagedResponse<Beneficiary> getBeneficiariesByFacility(int facilityId, int instanceId, QueryParamModel queryParam) {
        String searchText = queryParam.getSearchText();
        PagedResponse<Beneficiary> response = new PagedResponse<>();
        response.setTotal(getBeneficiaryCount(facilityId, searchText));
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        int offset = (queryParam.getPageNumber() - 1) * queryParam.getPageSize();
        String query = "select b.id, b.entityId, b.isActive, b.facilityId, b.beneficiaryName, b.unhcrId, s.status " +
                "from " + TBL_BENEFICIARY + " as b " +
                "inner join " + TBL_BENEFICIARY_DATA_COLLECTION_STATUS + " as s on s.beneficiaryId = b.id   " +
                "where s.instanceId = " + instanceId + " and b.facilityId = " + facilityId;
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " and ( b." + COL_BENEFICIARY_NAME + " like '%" + searchText + "%' or " +
                    "b." + COL_UNHCR_ID + " like '%" + searchText + "%' ) ";
        }
        query = query + " order by b." + COL_BENEFICIARY_NAME + " limit " + queryParam.getPageSize() + " offset " + offset;

        Cursor cursor = null;
        List<Beneficiary> beneficiaryList = new ArrayList<Beneficiary>();
        try {
            cursor = database.rawQuery(query, null);
            if (cursor.moveToFirst()) {
                do {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary.setId(cursor.getInt(0));
                    beneficiary.setEntityId(cursor.getInt(1));
                    int isActiveInt = cursor.getInt(2);
                    beneficiary.setActive(isActiveInt > 0);
                    beneficiary.setFacilityId(cursor.getInt(3));
                    beneficiary.setBeneficiaryName(cursor.getString(4));
                    beneficiary.setUnhcrId(cursor.getString(5));
                    beneficiary.setCollectionStatus(cursor.getInt(6));
                    beneficiaryList.add(beneficiary);
                } while (cursor.moveToNext());
            }
            response.setData(beneficiaryList);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
        return response;
    }

    public BeneficiaryIndicatorModel getBeneficiaryRecords(int instanceId, Integer beneficiaryId) {
        BeneficiaryIndicatorModel model = new BeneficiaryIndicatorModel();
        Datum indicator = new Datum();
        HashMap<Integer, ListObject> listDictionary = getListsForInstance(instanceId);
        indicator.setInstanceId(instanceId);
        indicator.setBeneficiaryId(beneficiaryId);
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select r.id, r.beneficiaryId, r.instanceId, r.columnId, r.value, r.status," +
                    "i.entityDynamicColumnId, i.columnName, i.columnNameBangla, i.columnDataType, i.isMultiValued, i.listId " +
                    "from tbl_beneficiary_records r " +
                    "left join tbl_beneficiary_indicator i on r.columnId = i.entityDynamicColumnId and i.instanceId = ?  " +
                    "where r.instanceId = ? " +
                    "and r.beneficiaryId = ?";

            cursor = db.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(instanceId),
                    String.valueOf(beneficiaryId)
            });
            if (cursor.moveToFirst()) {
                do {
                    Indicator ind = new Indicator();
                    ind.setRecordId(cursor.getInt(0));
                    ind.setEntityDynamicColumnId(cursor.getInt(3));
                    String value = cursor.getString(4);
                    int status = cursor.getInt(5);
                    ind.setStatus(status == 0 ? CollectionStatus.NotCollected.getIntValue() : status);
                    ind.setColumnName(cursor.getString(7));
                    ind.setColumnNameInBangla(cursor.getString(8));
                    ind.setColumnDataType(cursor.getInt(9));
                    ind.setMultiValued(cursor.getInt(10) > 0);
                    ind.setListId(cursor.getInt(11));
                    if (ind.getColumnDataType() == DataTypes.List.getIntValue()) {
                        ListObject list = listDictionary.get(Integer.valueOf((int) ind.getListId()));
                        ind.setListItems(list.getItems());
                        if (value != null && !value.isEmpty()) {
                            String[] valuesArray = value.split(",");
                            for (int i = 0; i < valuesArray.length; i++) {
                                ind.getValues().add(valuesArray[i].trim());
                            }
                        }
                    } else {
                        if (value != null && !value.isEmpty()) {
                            ind.getValues().add(value);
                        }
                    }
                    indicator.getIndicators().add(ind);
                } while (cursor.moveToNext());
            }

            // end loop
            model.getData().add(indicator);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            db.close();
        }
        return model;
    }

    private HashMap<Integer, ListObject> getListsForInstance(int instanceId) {
        HashMap<Integer, ListObject> result = new HashMap<>();
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select lo.*, li.*  " +
                "from tbl_list_items li   " +
                "join tbl_list_object lo on li.listId = lo.id   " +
                "where lo.id in ( " +
                "select listId  " +
                "from tbl_beneficiary_indicator  " +
                "where instanceId = ?   " +
                "and columnDataType = " + DataTypes.List.getIntValue() + ")";
        Cursor cursor = null;
        try {
            cursor = database.rawQuery(query, new String[]{String.valueOf(instanceId)});
            if (cursor.moveToFirst()) {
                do {
                    int listId = cursor.getInt(0);
                    ListObject list = new ListObject();
                    if (result.containsKey(listId)) {
                        list = result.get(listId);
                    } else {
                        list.setId(listId);
                        list.setName(cursor.getString(1));
                        result.put(listId, list);
                    }
                    ListItem li = new ListItem();
                    li.setId(cursor.getInt(2));
                    li.setTitle(cursor.getString(4));
                    li.setValue(cursor.getInt(5));
                    list.getItems().add(li);
                } while (cursor.moveToNext());
            }
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
        return result;
    }

    public void saveRecords(BeneficiaryPost records) {
        for (BeneficiaryDynamicCell record : records.getDynamicCells()) {
            updateRecord(records.getInstanceId(), records.getId(), record);
        }
    }

     private void updateRecord(Integer instanceId, Integer beneficiaryId, BeneficiaryDynamicCell record) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            String value = null;
            if (record.getDataType() == DataTypes.List.getIntValue()) {
                value = TextUtils.join(",", record.getValue());
            } else {
                value = record.getValue().get(0);
            }
            ContentValues cv = new ContentValues();
            cv.put(COL_VALUE, value);
            cv.put(COL_STATUS, CollectionStatus.Collected.getIntValue());
            String whereClause = COL_ID + "=?";
            database.update(TBL_BENEFICIARY_RECORDS, cv, whereClause, new String[]{String.valueOf(record.getRecordId())});
        } finally {
            database.close();
        }

        updateDataCollectionStatus(instanceId, beneficiaryId);
    }

    private void updateDataCollectionStatus(Integer instanceId, Integer beneficiaryId) {
        int presentStatus = getDataCollectionStatus(instanceId, beneficiaryId);
        if (presentStatus == CollectionStatus.Collected.getIntValue()) {
            return;
        }
        boolean isAllCollected = isAllRecordsCollected(instanceId, beneficiaryId);
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_STATUS, isAllCollected ? CollectionStatus.Collected.getIntValue() : CollectionStatus.NotCollected.getIntValue());
            String whereClause = COL_INSTANCE_ID + " = ? AND " + COL_BENEFICIARY_ID + " = ?";
            database.update(TBL_BENEFICIARY_DATA_COLLECTION_STATUS, cv, whereClause, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(beneficiaryId)
            });
        } finally {
            database.close();
        }
    }

    private boolean isAllRecordsCollected(Integer instanceId, Integer beneficiaryId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select 1   " +
                "from " + TBL_BENEFICIARY_RECORDS + " " +
                "where " + COL_INSTANCE_ID + " = ? " +
                "and " + COL_BENEFICIARY_ID + " = ? " +
                "and " + COL_STATUS + " <> ? " +
                "and columnId not in ( " + ApplicationConstants.BENEFICIARY_REOCRDS_EXCLUDEED +" )";
        Cursor cursor = null;
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(beneficiaryId),
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            int count = cursor.getCount();
            return count == 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private int getDataCollectionStatus(Integer instanceId, Integer beneficiaryId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select status  " +
                "from  " + TBL_BENEFICIARY_DATA_COLLECTION_STATUS + " " +
                "where " + COL_INSTANCE_ID + " = ? " +
                "and " + COL_BENEFICIARY_ID + " = ?";
        Cursor cursor = null;
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(beneficiaryId)
            });
            int status = CollectionStatus.NotCollected.getIntValue();
            if (cursor.moveToFirst()) {
                do {
                    status = cursor.getInt(0);
                }
                while (cursor.moveToNext());
            }
            return status;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public PagedResponse<Beneficiary> getBeneficiariesByFacilityForUpload(int facilityId, int instanceId, QueryParamModel queryParam) {
        String searchText = queryParam.getSearchText();
        PagedResponse<Beneficiary> response = new PagedResponse<>();
        response.setTotal(getBeneficiaryCountForUpload(facilityId, instanceId, searchText));
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        int offset = (queryParam.getPageNumber() - 1) * queryParam.getPageSize();
        String query = "select b.id, b.entityId, b.isActive, b.facilityId, b.beneficiaryName, b.unhcrId, s.status " +
                "from tbl_beneficiary_data_collection_status s  " +
                "inner join tbl_beneficiary b on s.beneficiaryId = b.id  " +
                "where ( s.status = 2  " +
                "or b.isActive = 0 ) " +
                "and s.instanceId = ?  " +
                "and s.status =? " +
                "and b.facilityId = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " and ( b." + COL_BENEFICIARY_NAME + " like '%" + searchText + "%' or " +
                    "b." + COL_UNHCR_ID + " like '%" + searchText + "%' ) ";
        }
        query = query + " order by b." + COL_BENEFICIARY_NAME + " limit " + queryParam.getPageSize() + " offset " + offset;

        Cursor cursor = null;
        List<Beneficiary> beneficiaryList = new ArrayList<Beneficiary>();
        try {
//
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(CollectionStatus.Collected.getIntValue()),
                    String.valueOf(facilityId)
            });
            if (cursor.moveToFirst()) {
                System.out.println(TAG +": "+ query);
                do {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary.setId(cursor.getInt(0));
                    beneficiary.setEntityId(cursor.getInt(1));
                    int isActiveInt = cursor.getInt(2);
                    beneficiary.setActive(isActiveInt > 0);
                    beneficiary.setFacilityId(cursor.getInt(3));
                    beneficiary.setBeneficiaryName(cursor.getString(4));
                    beneficiary.setUnhcrId(cursor.getString(5));
                    beneficiary.setCollectionStatus(cursor.getInt(6));
                    beneficiaryList.add(beneficiary);
                } while (cursor.moveToNext());
            }
            response.setData(beneficiaryList);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
        return response;
    }

    private Integer getBeneficiaryCountForUpload(int facilityId, int instanceId, String searchText) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String countQuery = "select count(s.beneficiaryId)   " +
                "from tbl_beneficiary_data_collection_status s  " +
                "inner join tbl_beneficiary b on s.beneficiaryId = b.id  " +
                "where ( s.status = ?   " +
                "or b.isActive = 0 ) " +
                "and s.instanceId = ?  " +
                "and b.facilityId = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            countQuery = countQuery + " and ( b." + COL_BENEFICIARY_NAME + " like '%" + searchText + "%' or " +
                    "b." + COL_UNHCR_ID + " like '%" + searchText + "%' ) ";
        }
        Cursor countCursor = null;
        int totalCount = 0;
        try {
            countCursor = database.rawQuery(countQuery, new String[]{
                    String.valueOf(CollectionStatus.Collected.getIntValue()),
                    String.valueOf(instanceId),
                    String.valueOf(facilityId)
            });
            if (countCursor.moveToFirst()) {
                do {
                    totalCount = countCursor.getInt(0);
                } while (countCursor.moveToNext());
            }
            return totalCount;
        } finally {
            if (countCursor != null) {
                countCursor.close();
            }
        }
    }

    public void createNewBeneficiary(int instanceId, Beneficiary beneficiary) {
        ArrayList<Indicator> indicators = getInstanceIndicators(instanceId);
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            database.beginTransaction();
            int beneficiaryId = insertBeneficiary(beneficiary, database);
            insertRecords(instanceId, beneficiaryId, beneficiary, database, indicators);
            insertDataCollectionStatus(instanceId, beneficiaryId, database);
            database.setTransactionSuccessful();
        } finally {
            database.endTransaction();
            database.close();
        }
    }

    private ArrayList<Indicator> getInstanceIndicators(int instanceId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        ArrayList<Indicator> indicators = new ArrayList<>();
        String query = "select " + COL_ENTITY_DYNAMIC_COLUMN_ID + ", " + COL_COLUMN_DATA_TYPE + " " +
                "from " + TBL_BENEFICIARY_INDICATOR + " " +
                "where " + COL_INSTANCE_ID + " = ?";
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId)
            });
            if (cursor.moveToFirst()) {
                do {
                    Indicator indicator = new Indicator();
                    indicator.setEntityDynamicColumnId(cursor.getInt(0));
                    indicator.setColumnDataType(cursor.getInt(1));
                    indicators.add(indicator);
                } while (cursor.moveToNext());
            }
            return indicators;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private void insertDataCollectionStatus(int instanceId, int beneficiaryId, SQLiteDatabase database) {
        ContentValues cv = new ContentValues();
        cv.put(COL_BENEFICIARY_ID, beneficiaryId);
        cv.put(COL_INSTANCE_ID, instanceId);
        cv.put(COL_STATUS, CollectionStatus.NotCollected.getIntValue());
        database.insert(TBL_BENEFICIARY_DATA_COLLECTION_STATUS, null, cv);
    }

    private void insertRecords(int instanceId, int beneficiaryId, Beneficiary beneficiary, SQLiteDatabase database, ArrayList<Indicator> indicators) {
        for (Indicator indicator : indicators) {
            ContentValues cv = new ContentValues();
            cv.put(COL_BENEFICIARY_ID, beneficiaryId);
            cv.put(COL_INSTANCE_ID, instanceId);
            cv.put(COL_COLUMN_ID, indicator.getEntityDynamicColumnId());
            if (indicator.getEntityDynamicColumnId() ==122){
                cv.put(COL_VALUE, beneficiary.getUnhcrId());
            } if (indicator.getEntityDynamicColumnId() ==123){
                cv.put(COL_VALUE, beneficiary.getBeneficiaryName());
            }if (indicator.getEntityDynamicColumnId() ==124){
                cv.put(COL_VALUE, beneficiary.getFatherName());
            }if (indicator.getEntityDynamicColumnId() ==125){
                cv.put(COL_VALUE, beneficiary.getMotherName());
            }if (indicator.getEntityDynamicColumnId() ==126){
                cv.put(COL_VALUE, beneficiary.getFcnId());
            }if (indicator.getEntityDynamicColumnId() ==127){
                cv.put(COL_VALUE, beneficiary.getDateOfBirth());
            }if (indicator.getEntityDynamicColumnId() ==128){
                cv.put(COL_VALUE, beneficiary.getSex());
            }if (indicator.getEntityDynamicColumnId() ==129){
                cv.put(COL_VALUE, beneficiary.getDisabled());
            }if (indicator.getEntityDynamicColumnId() ==130){
                cv.put(COL_VALUE, beneficiary.getLevelOfStudy());
            }if (indicator.getEntityDynamicColumnId() ==131){
                cv.put(COL_VALUE, beneficiary.getEnrollmentDate());
            }if (indicator.getEntityDynamicColumnId() ==132){
                cv.put(COL_VALUE, beneficiary.getFacilityId());
            }if (indicator.getEntityDynamicColumnId() ==133){
                cv.put(COL_VALUE, beneficiary.getBeneficiaryCampId());
            }if (indicator.getEntityDynamicColumnId() ==134){
                cv.put(COL_VALUE, beneficiary.getBlockId());
            }if (indicator.getEntityDynamicColumnId() ==135){
                cv.put(COL_VALUE, beneficiary.getSubBlockId());
            }if (indicator.getEntityDynamicColumnId() ==136){
                cv.put(COL_VALUE, beneficiary.getRemarks());
            }
            cv.put(COL_STATUS, CollectionStatus.NotCollected.getIntValue());
            database.insert(TBL_BENEFICIARY_RECORDS, null, cv);

        }
    }

    private int insertBeneficiary(Beneficiary beneficiary, SQLiteDatabase database) {
        ContentValues cv = new ContentValues();
        cv.put(COL_ISACTIVE, 1);
        cv.put(COL_FACILITY_ID, beneficiary.getFacilityId());
        cv.put(COL_BENEFICIARY_NAME, beneficiary.getBeneficiaryName());
        String unhcrId = beneficiary.getUnhcrId();
        if (unhcrId != null && !unhcrId.isEmpty()) {
            cv.put(COL_UNHCR_ID, unhcrId);
        }
        cv.put(COL_FATHER_NAME, beneficiary.getFatherName());
        cv.put(COL_MOTHER_NAME, beneficiary.getMotherName());
        if (beneficiary.getFcnId() != null) {
            cv.put(COL_FCNID, beneficiary.getFcnId());
        }
        cv.put(COL_DATE_OF_BIRTH, beneficiary.getDateOfBirth());
        cv.put(COL_SEX, beneficiary.getSex());
        cv.put(COL_DISABLED, beneficiary.getDisabled() ? 1 : 0);
        cv.put(COL_LEVEL_OF_STUDY, beneficiary.getLevelOfStudy());
        cv.put(COL_ENROLLMENT_DATE, beneficiary.getEnrollmentDate());
        cv.put(COL_FACILITY_CAMP_ID, beneficiary.getFacilityCampId());
        cv.put(COL_BENEFICIARY_CAMP_ID, beneficiary.getBeneficiaryCampId());
        cv.put(COL_BLOCK_ID, beneficiary.getBlockId());
        cv.put(COL_SUB_BLOCK_ID, beneficiary.getSubBlockId());
        if (beneficiary.getRemarks() != null) {
            cv.put(COL_REMARKS, beneficiary.getRemarks());
        }
        cv.put(FACILITY_NAME, beneficiary.getFacilityName());
        cv.put(BENEFICARY_CAMP_NAME, beneficiary.getBeneficiaryCampName());
        cv.put(COL_BLOCK_NAME, beneficiary.getBlockName());
        cv.put(SUB_BLOCK_NAME, beneficiary.getSubBlockName());

        long newId = database.insert(TBL_BENEFICIARY, null, cv);
        return (int) newId;
    }

    public void changeActiveStatus(boolean activeStatus, int beneficiaryId) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_ISACTIVE, activeStatus ? 1 : 0);
            String whereClause = COL_ID + "=?";
            database.update(TBL_BENEFICIARY, cv, whereClause, new String[]{
                    String.valueOf(beneficiaryId)
            });
        } finally {
            database.close();
        }
    }

    public ArrayList<CreateBeneficiaryModel> getNewBeneficiaries(int facilityId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select id, entityId, isActive, facilityId, beneficiaryName, unhcrId, fatherName, motherName, fcnId, dateOfBirth,  " +
                "sex, disabled, levelOfStudy, enrollmentDate, facilityCampId, beneficiaryCampId, blockId, subBlockId, remarks  " +
                "from tbl_beneficiary  " +
                "where entityId is null   " +
                "and facilityId = ?";
        Cursor cursor = null;
        ArrayList<CreateBeneficiaryModel> beneficiaries = new ArrayList<CreateBeneficiaryModel>();
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(facilityId)
            });
            if (cursor.moveToFirst()) {
                do {
                    CreateBeneficiaryModel beneficiary = new CreateBeneficiaryModel();
                    beneficiary.setDatabaseId(cursor.getInt(0));
                    beneficiary.setFacilityId(Integer.valueOf(cursor.getInt(3)));
                    beneficiary.setName(cursor.getString(4));
                    beneficiary.setUnhcrId(cursor.getString(5));
                    beneficiary.setFatherName(cursor.getString(6));
                    beneficiary.setMotherName(cursor.getString(7));
                    beneficiary.setFcnId(cursor.getString(8));
                    String dob = cursor.getString(9);
                    dob = FormatUtils.formatDate(dob, serverDateFormat);
                    beneficiary.setDateOfBirth(dob);
                    beneficiary.setSex(Integer.valueOf(cursor.getInt(10)));
                    beneficiary.setDisabled(cursor.getInt(11) > 0);
                    beneficiary.setLevelOfStudy(cursor.getString(12));
                    beneficiary.setEnrollmentDate(FormatUtils.formatDate(cursor.getString(13), serverDateFormat));
                    beneficiary.setBeneficiaryCampId(cursor.getInt(15));
                    beneficiary.setBlockId(cursor.getInt(16));
                    beneficiary.setSubBlockId(cursor.getInt(17));
                    beneficiary.setRemarks(cursor.getString(18));
                    beneficiary.setInstanceId(Singleton.getInstance().getIdInstance());
                    beneficiaries.add(beneficiary);
                } while (cursor.moveToNext());
            }
            return beneficiaries;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public void updateBeneficiariesEntityId(ArrayList<CreateBeneficiaryModel> beneficiaries) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            for (CreateBeneficiaryModel beneficiary : beneficiaries) {
                ContentValues cv = new ContentValues();
                cv.put(COL_ENTITY_ID, beneficiary.getId());
                String whereClause = COL_ID + "=?";
                database.update(TBL_BENEFICIARY, cv, whereClause, new String[]{
                        String.valueOf(beneficiary.getDatabaseId())
                });
            }
        } finally {
            database.close();
        }
    }

    public ArrayList<Beneficiary> getInactiveBeneficiaries(int facilityId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select " + COL_ID + ", " + COL_ENTITY_ID + " " +
                "from " + TBL_BENEFICIARY + "  " +
                "where " + COL_FACILITY_ID + " = ? " +
                "and " + COL_ISACTIVE + " = 1";
        Cursor cursor = null;
        ArrayList<Beneficiary> beneficiaries = new ArrayList<>();
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(facilityId)
            });
            if (cursor.moveToFirst()) {
                do {
                    Beneficiary beneficiary = new Beneficiary();
                    beneficiary.setId(cursor.getInt(0));
                    beneficiary.setEntityId(cursor.getInt(1));
                    beneficiaries.add(beneficiary);
                } while (cursor.moveToNext());
            }
            return beneficiaries;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public void deleteInactiveBeneficiaries(ArrayList<Beneficiary> inactiveBeneficiaries) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            for (Beneficiary beneficiary : inactiveBeneficiaries) {
                String whereClause = COL_BENEFICIARY_ID + "=?";
                database.delete(TBL_BENEFICIARY_RECORDS, whereClause, new String[]{String.valueOf(beneficiary.getId())});

                database.delete(TBL_BENEFICIARY_DATA_COLLECTION_STATUS, whereClause, new String[]{String.valueOf(beneficiary.getId())});

                whereClause = COL_ENTITY_ID + "=?";
                database.delete(TBL_BENEFICIARY, whereClause, new String[]{String.valueOf(beneficiary.getId())});
            }
        } finally {
            database.close();
        }
    }

    public ArrayList<BeneficiaryPost> getBeneficiariesToUpload(int instanceId) {
        ArrayList<Indicator> instanceIndicators = getInstanceIndicators(instanceId);
        HashMap<Integer, Indicator> indicatorDictionary = new HashMap<Integer, Indicator>();
        for (Indicator indicator : instanceIndicators) {
            indicatorDictionary.put(indicator.getEntityDynamicColumnId(), indicator);
        }
        HashMap<Integer, BeneficiaryPost> resultDictionary = new HashMap<Integer, BeneficiaryPost>();
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "SELECT r.id, r.beneficiaryId, b.entityId, r.columnId, r.value  " +
                "from tbl_beneficiary_records r  " +
                "join tbl_beneficiary b on r.beneficiaryId = b.id and r.instanceId = ? " +
                "join tbl_beneficiary_data_collection_status s on s.beneficiaryId = b.id and s.instanceId = ?  " +
                "where s.status = ?  " +
                "order by r.beneficiaryId, r.columnId";
        Cursor cursor = null;
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(instanceId),
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            if (cursor.moveToFirst()) {
                do {
                    Integer beneficiaryId = Integer.valueOf(cursor.getInt(1));
                    int entityId = Integer.valueOf(cursor.getInt(2));
                    BeneficiaryPost beneficiary = null;
                    if (!resultDictionary.containsKey(beneficiaryId)) {
                        beneficiary = new BeneficiaryPost();
                        beneficiary.setId(beneficiaryId);
                        beneficiary.setBeneficiaryId(entityId);
                        beneficiary.setInstanceId(instanceId);
                        resultDictionary.put(beneficiaryId, beneficiary);
                    } else {
                        beneficiary = resultDictionary.get(beneficiaryId);
                    }

                    BeneficiaryDynamicCell cell = new BeneficiaryDynamicCell();
                    int columnId = cursor.getInt(3);
                    cell.setEntityDynamicColumnId(columnId);
                    cell.setDataType(indicatorDictionary.get(Integer.valueOf(columnId)).getColumnDataType());
                    cell.setRecordId(cursor.getInt(0));
                    String value = cursor.getString(4);
                    if (value != null && !value.isEmpty()) {
                        if (cell.getDataType() == DataTypes.List.getIntValue()) {
                            String[] listValues = value.split(",");
                            for (int i = 0; i < listValues.length; i++) {
                                cell.getValue().add(listValues[i].trim());
                            }
                        } else {
                            cell.getValue().add(value.trim());
                        }
                        beneficiary.getDynamicCells().add(cell);
                    }
                } while (cursor.moveToNext());
            }

            ArrayList<BeneficiaryPost> beneficiaryPosts = new ArrayList<BeneficiaryPost>(resultDictionary.values());
            return beneficiaryPosts;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public void deleteRecords(ArrayList<BeneficiaryPost> beneficiaryPosts) {
        for (BeneficiaryPost beneficiaryPost:beneficiaryPosts){
            deleteRecord(beneficiaryPost);
        }
    }

    private void deleteRecord(BeneficiaryPost beneficiaryPost) {
        boolean hasOtherInstanceRecords = hasOtherInstanceRecords(beneficiaryPost.getId(), beneficiaryPost.getInstanceId());
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try{
            String whereClause = COL_BENEFICIARY_ID + " = ? AND " + COL_INSTANCE_ID + " = ?";
            database.delete(TBL_BENEFICIARY_RECORDS, whereClause, new String[]{
                    String.valueOf(beneficiaryPost.getId()),
                    String.valueOf(beneficiaryPost.getInstanceId())
            });

            database.delete(TBL_BENEFICIARY_DATA_COLLECTION_STATUS, whereClause, new String[]{
                    String.valueOf(beneficiaryPost.getId()),
                    String.valueOf(beneficiaryPost.getInstanceId())
            });

            if(!hasOtherInstanceRecords) {
                whereClause = COL_ID + "=?";
                database.delete(TBL_BENEFICIARY, whereClause, new String[]{String.valueOf(beneficiaryPost.getId())});
            }
        }
        finally {
            database.close();
        }
    }

    private boolean hasOtherInstanceRecords(int beneficiaryId, int instanceId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "SELECT count(1)    " +
                "from tbl_beneficiary_records  " +
                "where instanceId <> ? " +
                "and beneficiaryId = ? ";
        Cursor cursor = null;
        int count = 0;
        try{
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(beneficiaryId)
            });
            if(cursor.moveToFirst()){
                do{
                    count = cursor.getInt(0);
                } while (cursor.moveToNext());
            }

            return count > 0;
        }
        finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    public void saveBeneficiaryGetById(Beneficiary beneficiary, int instanceId){
        SQLiteDatabase database = dbHelper.getWritableDatabase();

        try {
            ContentValues cv = new ContentValues();
//            cv.put(COL_ENTITY_ID, beneficiary.getId());
//            cv.put(COL_INSTANCE_ID, instanceId);
            cv.put(FACILITY_NAME, beneficiary.getFacilityName());
            cv.put(BENEFICARY_CAMP_NAME, beneficiary.getBeneficiaryCampName());
            cv.put(COL_BLOCK_NAME, beneficiary.getBlockName());
            cv.put(SUB_BLOCK_NAME, beneficiary.getSubBlockName());

            database.update(TBL_BENEFICIARY, cv, "entityId=" + beneficiary.getId(), null);


        } catch (Exception e){
            e.printStackTrace();
        }

        finally {
            database.close();
        }
    }

    public Beneficiary getBeneficiaryGetByIdModel(int id, int instanceId){
        List <Beneficiary> data = new ArrayList<>();
        Beneficiary data_model = new Beneficiary();
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String select_query = "select * from " + TBL_BENEFICIARY +" where id= "+ id;
            cursor = db.rawQuery(select_query, null);
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {


                    data_model.setFacilityName(cursor.getString(19));
                    data_model.setBeneficiaryCampName(cursor.getString(20));
                    data_model.setBlockName(cursor.getString(21));
                    data_model.setSubBlockName(cursor.getString(22));

                } while (cursor.moveToNext());
            }

            data.add(data_model);

        } finally {
            // After using cursor we have to close it
            if (cursor != null) {
                cursor.close();
            }
            db.close();
        }

        return data_model;
    }
}
