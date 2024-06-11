package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.text.TextUtils;
import android.util.Log;
import android.widget.Toast;

import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.enumtype.DataTypes;
import com.unicef.mis.enumtype.MultiValuedType;
import com.unicef.mis.model.BeneficiaryIndicator;
import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.ListItem;
import com.unicef.mis.model.ListObject;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.PropertiesInfoModel;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.model.facility.indicator.FacilityIndicator;
import com.unicef.mis.model.facility.indicator.FacilityIndicatorModel;
import com.unicef.mis.model.facility.indicator.Indicator;
import com.unicef.mis.model.facility.indicator.post.FacilityDynamicCell;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.CAMP_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_BLOCK_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_CAMP_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_CAMP_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_DATA_TYPE;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_COLUMN_NAME_BANGLA;
import static com.unicef.mis.constants.DatabaseConstants.COL_END_DATE;
import static com.unicef.mis.constants.DatabaseConstants.COL_ENTITY_DYNAMIC_COLUMN_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITYGALL_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_CODE;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_DATACOLLECTIONDATE;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_SCHEDULEID;
import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_TITLE;
import static com.unicef.mis.constants.DatabaseConstants.COL_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_IMPLEMENTATION_PARTNER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_INSTANCE_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_IS_MULTI_VALUED;
import static com.unicef.mis.constants.DatabaseConstants.COL_LIST_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_PROGRAMMING_PARTNER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_PROGRAM_PARTNER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_STATUS;
import static com.unicef.mis.constants.DatabaseConstants.COL_TEACHER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_UNION_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_UPZILLA_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_VALUE;
import static com.unicef.mis.constants.DatabaseConstants.IMPLEMENTATION_PARTNER_NAME;
import static com.unicef.mis.constants.DatabaseConstants.TBL_FACILITY;
import static com.unicef.mis.constants.DatabaseConstants.TBL_FACILITY_DATA_COLLECTION_STATUS;
import static com.unicef.mis.constants.DatabaseConstants.TBL_FACILITY_INDICATOR;
import static com.unicef.mis.constants.DatabaseConstants.TBL_FACILITY_RECORDS;
import static com.unicef.mis.constants.DatabaseConstants.TBL_FACILITY_SCHEDULE;

public class FacilityDataAccess {
    private static String TAG = FacilityDataAccess.class.getSimpleName();
    private final SQLiteDatabaseHelper dbHelper;

    public FacilityDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public void saveFacilityIndicators(Integer instanceId, List<IndicatorNew> indicators, IGenericApiCallBack callBack) {
        for (IndicatorNew indicator : indicators) {
            saveOrUpdateFacilityIndicator(instanceId, indicator, callBack);
        }
        callBack.apiCallSuccessful(null, null);
    }

    private long saveOrUpdateFacilityIndicator(Integer instanceId, IndicatorNew indicator, IGenericApiCallBack callBack) {
        long indicatorId = getFacilityIndicatorId(instanceId, indicator, callBack);
        if (indicatorId > 0) {
            return indicatorId;
        }
        return saveFacilityIndicator(instanceId, indicator, callBack);
    }

    private long saveFacilityIndicator(Integer id, IndicatorNew indicator, IGenericApiCallBack callBack) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_ENTITY_DYNAMIC_COLUMN_ID, indicator.getEntityDynamicColumnId());
            cv.put(COL_INSTANCE_ID, id);
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
            long indicatorId = database.insert(TBL_FACILITY_INDICATOR, null, cv);
            return indicatorId;
        } catch (Exception e) {
            callBack.apiCallFailed(true, e.getMessage());
            return 0;
        } finally {
            database.close();
        }
    }

    private long getFacilityIndicatorId(Integer instanceId, IndicatorNew indicator, IGenericApiCallBack callBack) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select id from " + TBL_FACILITY_INDICATOR +
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
        } catch (Exception e) {
            callBack.apiCallFailed(true, e.getMessage());
            return 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }


    public List<ScheduledInstance> getFacilitySchedule() {
        // Data model list in which we have to return the data
        List<ScheduledInstance> data = new ArrayList<ScheduledInstance>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        // Select query for selecting whole table data
        String select_query = "select * from " + TBL_FACILITY_SCHEDULE +" ORDER By id DESC";
        // Cursor for traversing whole data into database
        Cursor cursor = null;

        try {
            cursor = db.rawQuery(select_query, null);
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    ScheduledInstance data_model = new ScheduledInstance(cursor.getInt(0), cursor.getInt(1), cursor.getString(2),
                            cursor.getString(3), cursor.getString(4), cursor.getInt(5));
                    data.add(data_model);
                } while (cursor.moveToNext());
            }
            return data;
        } finally {
            // After using cursor we have to close it
            if (cursor != null) {
                cursor.close();
            }
            db.close();
        }
    }

    public List<BeneficiaryIndicator> getFacilityIndicatorList(int instanceId, int facilityId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            String query = "select a.id, b.entityDynamicColumnId, b.columnName, b.columnNameBangla, b.columnDataType,  " +
                    "b.isMultiValued, b.listId, a.status, a.value " +
                    "from tbl_facility_records a " +
                    "inner join tbl_facility_indicator b on a.columnId = b.entityDynamicColumnId  " +
                    "where a.facilityId=? " +
                    "and a.instanceId=? ";
            cursor = database.rawQuery(query, new String[]{String.valueOf(facilityId), String.valueOf(instanceId)});
            ArrayList<BeneficiaryIndicator> benIndicators = new ArrayList<BeneficiaryIndicator>();
            if (cursor.moveToFirst()) {
                do {
                    BeneficiaryIndicator indicator = new BeneficiaryIndicator();
                    indicator.setId(cursor.getInt(0));
                    indicator.setDynamicColumnId(cursor.getInt(1));
                    indicator.setColumnName(cursor.getString(2));
                    indicator.setColumnNameInBangla(cursor.getString(3));
                    indicator.setColumnDataType(cursor.getInt(4));
                    boolean isMultiValued = cursor.getInt(5) > 0;
                    indicator.setIsMultiValued(isMultiValued);
                    indicator.setListId(cursor.getInt(6));
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


    public List<FacilityListDatum> getFacilityList() {
        // Data model list in which we have to return the facilityList
        List<FacilityListDatum> facilityList = new ArrayList<FacilityListDatum>();
        // Accessing database for reading facilityList
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table facilityList
        String select_query = "Select * from " + TBL_FACILITY;
        // Cursor for traversing whole facilityList into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all facilityList and adding to arraylist
                do {
                    FacilityListDatum facility = new FacilityListDatum();
                    facility.setId(cursor.getInt(0));
                    facility.setFacilityName(cursor.getString(1));
                    facility.setFacilityCode(cursor.getString(2));
                    facility.setCampId(cursor.getInt(3));
                    facility.setCampName(cursor.getString(4));
                    facility.setProgrammingPartnerName(cursor.getString(5));
                    facility.setImplemantationPartnerName(cursor.getString(6));
                    facilityList.add(facility);
                } while (cursor.moveToNext());
            }
        } finally {
            // After using cursor we have to close it
            cursor.close();
            // Closing database
            db.close();
        }

        // returning list
        return facilityList;
    }

    public void saveOrUpdateFacilitySchedule(Schedule schedule, IGenericApiCallBack callBack) {
        for (ScheduledInstance instance : schedule.getData()) {
            if (instance.getId() == Singleton.getInstance().getIdInstance()) {
                saveOrUpdateInstance(instance, callBack);
            }
        }


    }

    public void saveOrUpdateFacilities(List<FacilityListDatum> facilityList, IGenericApiCallBack callBack) {
        for (FacilityListDatum facility : facilityList) {
            saveOrUpdateFacility(facility);
        }
        callBack.apiCallSuccessful(null, null);
    }


    private void saveOrUpdateFacility(FacilityListDatum facility) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_FACILITY + " where id = " + facility.getId();
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            if (count > 0) {
                updateFacility(facility);
            } else {
                insertFacility(facility);
            }
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private void updateFacility(FacilityListDatum facility) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_FACILITY_NAME, facility.getFacilityName());
            cv.put(COL_FACILITY_CODE, facility.getFacilityCode());
            cv.put(COL_CAMP_ID, facility.getCampId());
            cv.put(COL_CAMP_NAME, facility.getCampName());
            cv.put(COL_PROGRAMMING_PARTNER_NAME, facility.getProgrammingPartnerName());
            cv.put(COL_IMPLEMENTATION_PARTNER_NAME, facility.getImplemantationPartnerName());
            database.update(TBL_FACILITY, cv, "id=" + facility.getId(), null);
        } finally {
            database.close();
        }
    }

    public boolean insertFacility(FacilityListDatum facility) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        try {
            ContentValues contentValues = new ContentValues();

            contentValues.put(COL_FACILITYGALL_ID, facility.getId());
            contentValues.put(COL_FACILITY_NAME, facility.getFacilityName());
            contentValues.put(COL_FACILITY_CODE, facility.getFacilityCode());
            contentValues.put(COL_CAMP_ID, facility.getCampId());
            contentValues.put(COL_CAMP_NAME, facility.getCampName());
            contentValues.put(COL_PROGRAMMING_PARTNER_NAME, facility.getProgrammingPartnerName());
            contentValues.put(COL_IMPLEMENTATION_PARTNER_NAME, facility.getImplemantationPartnerName());

            long result = db.insert(TBL_FACILITY, null, contentValues);

            return result != -1;
        } finally {
            db.close();
        }
    }

    public void saveOrUpdateInstance(ScheduledInstance instance, IGenericApiCallBack callBack) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_FACILITY_SCHEDULE + " where id = " + instance.getId().toString();
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            if (count > 0) {
                updateInstance(instance);
            } else {
                insertFacilitySchedule(instance);
            }
            callBack.apiCallSuccessful(null, null);
        } catch (Exception e) {
            e.printStackTrace();
            callBack.apiCallFailed(true, e.getMessage());
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }


    private void updateInstance(ScheduledInstance instance) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_FACILITY_TITLE, instance.getTitle());
            cv.put(COL_FACILITY_DATACOLLECTIONDATE, instance.getDataCollectionDate());
            cv.put(COL_STATUS, instance.getStatus());
            database.update(TBL_FACILITY_SCHEDULE, cv, "id=" + instance.getId().toString(), null);
        } finally {
            database.close();
        }
    }

    public boolean insertFacilitySchedule(ScheduledInstance instance) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        try {
            ContentValues contentValues = new ContentValues();

            contentValues.put(COL_ID, instance.getId());
            contentValues.put(COL_FACILITY_SCHEDULEID, instance.getScheduleId());
            contentValues.put(COL_FACILITY_TITLE, instance.getTitle());
            contentValues.put(COL_FACILITY_DATACOLLECTIONDATE, instance.getDataCollectionDate());
            contentValues.put(COL_END_DATE, instance.getEndDate());
            contentValues.put(COL_STATUS, instance.getStatus());

            long result = db.insert(TBL_FACILITY_SCHEDULE, null, contentValues);

            return result != -1;
        } finally {
            db.close();
        }
    }

    public void saveOrUpdateFacilityList(List<FacilityListDatum> facilities) {
        for (FacilityListDatum facility : facilities) {
            saveOrUpdateFacility(facility);
        }
    }

    public Object saveRecords(int instanceId, List<FacilityListDatum> facilities) {
        for (FacilityListDatum facility : facilities) {
            saveOrUpdateFacilityRecords(instanceId, facility);
            saveOrUpdateFacilityDataCollectionStatus(instanceId, facility);
        }
        return null;
    }

    private void saveOrUpdateFacilityDataCollectionStatus(int instanceId, FacilityListDatum facility) {
        if (!hasFacilityDataCollectionStatus(instanceId, facility.getId())) {
            saveFacilityCollectionStatus(instanceId, facility);
        }
    }

    private int saveFacilityCollectionStatus(int instanceId, FacilityListDatum facility) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_FACILITY_ID, facility.getId());
            cv.put(COL_INSTANCE_ID, instanceId);
            cv.put(COL_STATUS, facility.getCollectionStatus());

            long newStatus = database.insert(TBL_FACILITY_DATA_COLLECTION_STATUS, null, cv);
            return (int) newStatus;
        } finally {
            database.close();
        }
    }

    private boolean hasFacilityDataCollectionStatus(int instanceId, int facilityId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_FACILITY_DATA_COLLECTION_STATUS + " where " + COL_INSTANCE_ID + "=? AND " +
                    COL_FACILITY_ID + "=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(instanceId), String.valueOf(facilityId)});
            return cursor.getCount() > 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private void saveOrUpdateFacilityRecords(int instanceId, FacilityListDatum facility) {
        for (PropertiesInfoModel record : facility.getProperties()) {
            saveOrUpdateFacilityRecord(instanceId, facility.getId(), record);
        }
    }

    private ArrayList<BeneficiaryIndicator> getInstanceIndicators(int instanceId) {
        ArrayList<BeneficiaryIndicator> indicators = new ArrayList<BeneficiaryIndicator>();
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        String selectQuery = "select * from " + TBL_FACILITY_INDICATOR + " where instanceId = " + instanceId;
        Cursor cursor = db.rawQuery(selectQuery, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                do {
                    BeneficiaryIndicator indicator = new BeneficiaryIndicator();
                    indicator.setId(cursor.getInt(0));
                    indicator.setDynamicColumnId(cursor.getInt(1));
                    indicator.setColumnName(cursor.getString(3));
                    indicator.setColumnDataType(cursor.getInt(5));
                    boolean isMultiValued = cursor.getInt(6) > 0;
                    indicator.setIsMultiValued(isMultiValued);
                    indicator.setListId(cursor.getInt(7));

                    indicators.add(indicator);
                } while (cursor.moveToNext());
            }
        } finally {
            // After using cursor we have to close it
            cursor.close();
            // Closing database
            db.close();
        }
        return indicators;
    }

    private boolean doesRecordExists(int instanceId, int facilityId, long columnId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_FACILITY_RECORDS + " where facilityId = " + facilityId + " and instanceId = " + instanceId + " and columnId = " + columnId;
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            return count > 0;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private void saveOrUpdateFacilityRecord(int instanceId, Integer facilityId, PropertiesInfoModel record) {
        if (!doesRecordExists(instanceId, facilityId, record.getEntityColumnId())) {
            insertFacilityRecord(instanceId, facilityId, record, record.getEntityColumnId());
        }
    }

    public void updateFacilityRecord(BeneficiaryIndicator indicator, IGenericApiCallBack callBack) {
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
            database.update(TBL_FACILITY_RECORDS, cv, whereClause, null);

            callBack.apiCallSuccessful(null, null);
        } catch (Exception e) {
            e.printStackTrace();
            callBack.apiCallFailed(true, e.getMessage());
        } finally {
            database.close();
        }
    }

    private void insertFacilityRecord(int instanceId, Integer facilityId, PropertiesInfoModel record, int columnId) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        try {
            String recordValue = "";

            List<String> values = record.getValues();
            if (values != null && values.size() >= 0) {
                recordValue = android.text.TextUtils.join(",", values);
            }

            ContentValues contentValues = new ContentValues();
            contentValues.put(COL_FACILITY_ID, facilityId);
            contentValues.put(COL_INSTANCE_ID, instanceId);
            contentValues.put(COL_COLUMN_ID, columnId);
            contentValues.put(COL_VALUE, recordValue);
            contentValues.put(COL_STATUS, record.getStatus());
            db.insert(TBL_FACILITY_RECORDS, null, contentValues);
        } finally {
            db.close();
        }
    }

    public List<FacilityListDatum> getFacilityListUpload(int instanceId) {
        List<FacilityListDatum> data = new ArrayList<FacilityListDatum>();
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        String select_query = "Select a.id, a.facilityName, a.facilityCode, a.camp_id, a.camp_name," +
                " a.programming_partner_name, a.implementation_partner_name," +
                "b.status from tbl_facility a INNER JOIN tbl_facility_records b on   " +
                "b.facilityId = a.id where b.instanceId = ? AND b.status = 2";
        System.out.println(select_query);
        Cursor cursor = db.rawQuery(select_query, new String[]{String.valueOf(instanceId)});
        try {
            if (cursor.moveToFirst()) {
                do {
                    FacilityListDatum facilities = readFacilityFromCursor(cursor);
                    data.add(facilities);
                } while (cursor.moveToNext());
            }

        } finally {
            if (cursor != null) {
                cursor.close();
            }
            db.close();
        }
        return data;

    }

    private FacilityListDatum readFacilityFromCursor(Cursor cursor) {

        FacilityListDatum facilities = new FacilityListDatum();

        facilities.setId(cursor.getInt(0));
        facilities.setFacilityName(cursor.getString(1));
        facilities.setFacilityCode(cursor.getString(2));
        facilities.setCampId(cursor.getInt(3));
        facilities.setCampName(cursor.getString(4));
        facilities.setProgrammingPartnerName(cursor.getString(5));
        facilities.setImplemantationPartnerName(cursor.getString(6));

        return facilities;
    }

    public PagedResponse<FacilityListDatum> searchFacilities(QueryParamModel queryParam) {
        String searchText = queryParam.getSearchText();
        PagedResponse<FacilityListDatum> response = new PagedResponse<>();
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        String query = "select * from " + TBL_FACILITY;
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " where " + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    COL_FACILITY_CODE + " like '%" + searchText + "%' ";
        }
        Cursor countCursor = null;
        try {
            countCursor = db.rawQuery(query, null);
            response.setTotal(countCursor.getCount());
        } finally {
            if (countCursor != null) {
                countCursor.close();
            }
        }

        int offset = (queryParam.getPageNumber() - 1) * queryParam.getPageSize();
        query = "select * from " + TBL_FACILITY;
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " where " + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    COL_FACILITY_CODE + " like '%" + searchText + "%' ";
        }
        query = query + " order by " + COL_FACILITY_CODE + " limit " + queryParam.getPageSize() + " offset " + offset;
        Cursor cursor = null;
        List<FacilityListDatum> facilityList = new ArrayList<FacilityListDatum>();
        try {
            cursor = db.rawQuery(query, null);
            if (cursor.moveToFirst()) {
                // looping through all facilityList and adding to arraylist
                do {
                    FacilityListDatum facility = new FacilityListDatum();
                    facility.setId(cursor.getInt(0));
                    facility.setFacilityName(cursor.getString(1));
                    facility.setFacilityCode(cursor.getString(2));
                    facility.setCampId(cursor.getInt(3));
                    facility.setCampName(cursor.getString(4));
                    facility.setProgrammingPartnerName(cursor.getString(5));
                    facility.setImplemantationPartnerName(cursor.getString(6));
                    facilityList.add(facility);
                } while (cursor.moveToNext());
            }
            response.setData(facilityList);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            db.close();
        }
        return response;
    }

    public PagedResponse<FacilityListDatum> searchFacilitiesByInstance(int instanceId, QueryParamModel queryParam) {
        String searchText = queryParam.getSearchText();
        PagedResponse<FacilityListDatum> response = new PagedResponse<>();
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        String query = "select * from " + TBL_FACILITY + " f  " +
                "join tbl_facility_data_collection_status s on s.facilityId = f.id and s.instanceId = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " where f." + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    "f." + COL_FACILITY_CODE + " like '%" + searchText + "%'";
        }
        Cursor countCursor = null;
        try {
            countCursor = db.rawQuery(query, new String[]{String.valueOf(instanceId)});
            response.setTotal(countCursor.getCount());
        } finally {
            if (countCursor != null) {
                countCursor.close();
            }
        }

        int offset = (queryParam.getPageNumber() - 1) * queryParam.getPageSize();
        query = "select f.*, s.status from " + TBL_FACILITY + " as f " +
                "inner join " + TBL_FACILITY_DATA_COLLECTION_STATUS + " as s on s.facilityId = f.id   " +
                "where s.instanceId = " + instanceId + " ";
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " and ( f." + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    "f." + COL_FACILITY_CODE + " like '%" + searchText + "%' ) ";
        }
        query = query + " order by " + COL_FACILITY_CODE + " limit " + queryParam.getPageSize() + " offset " + offset;
        Cursor cursor = null;
        List<FacilityListDatum> facilityList = new ArrayList<FacilityListDatum>();
        try {
            cursor = db.rawQuery(query, null);
            if (cursor.moveToFirst()) {
                // looping through all facilityList and adding to arraylist
                do {
                    FacilityListDatum facility = new FacilityListDatum();
                    facility.setId(cursor.getInt(0));
                    facility.setFacilityName(cursor.getString(1));
                    facility.setFacilityCode(cursor.getString(2));
                    facility.setCampId(cursor.getInt(3));
                    facility.setCampName(cursor.getString(4));
                    facility.setProgrammingPartnerName(cursor.getString(5));
                    facility.setImplemantationPartnerName(cursor.getString(6));
                    facility.setCollectionStatus(cursor.getInt(12));
                    facilityList.add(facility);
                } while (cursor.moveToNext());
            }
            response.setData(facilityList);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            db.close();
        }
        return response;
    }

    public FacilityIndicatorModel getFacilityRecords(int instanceId, int facilityId) {
        FacilityIndicatorModel model = new FacilityIndicatorModel();
        FacilityIndicator indicator = new FacilityIndicator();
        HashMap<Integer, ListObject> listDictionary = getListsForInstance(instanceId);
        indicator.setFacilityId(facilityId);
        indicator.setInstanceId(instanceId);
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            // in loop

            String query = "select r.id, r.facilityId, r.instanceId, r.columnId, r.value, r.status," +
                    "i.entityDynamicColumnId, i.columnName, i.columnNameBangla, i.columnDataType, i.isMultiValued, i.listId " +
                    "from tbl_facility_records r " +
                    "join tbl_facility_indicator i on r.columnId = i.entityDynamicColumnId  and i.instanceId = ? " +
                    "where r.instanceId = ? " +
                    "and r.facilityId = ?";

            cursor = db.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(instanceId),
                    String.valueOf(facilityId)});
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
                "from tbl_facility_indicator  " +
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

    public boolean saveFacilityRecords(FacilityPost records) {
        for (FacilityDynamicCell record : records.getDynamicCells()) {
            updateRecord(records.getInstanceId(), records.getFacilityId(), record);
        }
        return true;
    }

    private void updateRecord(Integer instanceId, Integer facilityId, FacilityDynamicCell record) {
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
            database.update(TBL_FACILITY_RECORDS, cv, whereClause, new String[]{String.valueOf(record.getRecordId())});
        } finally {
            database.close();
        }

        updateDataCollectionStatus(instanceId, facilityId);
    }

    private void updateDataCollectionStatus(Integer instanceId, Integer facilityId) {
        int presentStatus = getDataCollectionStatus(instanceId, facilityId);
        if (presentStatus == CollectionStatus.Collected.getIntValue()) {
            return;
        }
        boolean isAllCollected = isAllRecordsCollected(instanceId, facilityId);
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_STATUS, isAllCollected ? CollectionStatus.Collected.getIntValue() : CollectionStatus.NotCollected.getIntValue());
            String whereClause = COL_INSTANCE_ID + " = ? AND " + COL_FACILITY_ID + " = ?";
            database.update(TBL_FACILITY_DATA_COLLECTION_STATUS, cv, whereClause, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(facilityId)
            });
        } finally {
            database.close();
        }
    }

    private int getDataCollectionStatus(Integer instanceId, Integer facilityId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select status  " +
                "from  " + TBL_FACILITY_DATA_COLLECTION_STATUS + " " +
                "where instanceId = ?  " +
                "and facilityId = ?";
        Cursor cursor = null;
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(facilityId)
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

    private boolean isAllRecordsCollected(Integer instanceId, Integer facilityId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "select 1   " +
                "from " + TBL_FACILITY_RECORDS + " " +
                "where " + COL_INSTANCE_ID + " = ? " +
                "and " + COL_FACILITY_ID + " = ? " +
                "and " + COL_STATUS + " <> ? " +
                "and columnId not in ( " + ApplicationConstants.DAMAGE_INDICATORS + " ) ";
        Cursor cursor = null;
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(facilityId),
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


    public PagedResponse<FacilityListDatum> getFacilitiesForUpload(int instanceId, QueryParamModel queryParam) {
        String searchText = queryParam.getSearchText();
        PagedResponse<FacilityListDatum> response = new PagedResponse<>();
        Integer result;
        SQLiteDatabase database1 = dbHelper.getReadableDatabase();
        String countQuery = "select count(1)   " +
                "from tbl_facility_data_collection_status  " +
                "where instanceId = ?  " +
                "and status = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            countQuery = countQuery + " and ( f." + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    "f." + COL_FACILITY_CODE + " like '%" + searchText + "%' ) ";
        }

        int count = 0;
        Cursor cursor1 = null;
        try {
            cursor1 = database1.rawQuery(countQuery, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            if (cursor1.moveToFirst()) {
                do {
                    count = cursor1.getInt(0);
                } while (cursor1.moveToNext());
            }
            result = count;
        } finally {
            if (cursor1 != null) {
                cursor1.close();
            }
            database1.close();
        }
        response.setTotal(result);
        SQLiteDatabase database = dbHelper.getReadableDatabase();

        int offset = (queryParam.getPageNumber() - 1) * queryParam.getPageSize();
        String query = "select f.*, s.status from " + TBL_FACILITY + " as f " +
                "inner join " + TBL_FACILITY_DATA_COLLECTION_STATUS +
                " as s on s.facilityId = f.id and s.instanceId = ? " +
                "where s.status = ?  ";
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " and ( f." + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    "f." + COL_FACILITY_CODE + " like '%" + searchText + "%' ) ";
        }
        query = query + " order by f." + COL_FACILITY_CODE + " limit " + queryParam.getPageSize() + " offset " + offset;
        Cursor cursor = null;
        List<FacilityListDatum> facilityList = new ArrayList<FacilityListDatum>();
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            if (cursor.moveToFirst()) {
                // looping through all facilityList and adding to arraylist
                do {
                    FacilityListDatum facility = new FacilityListDatum();
                    facility.setId(cursor.getInt(0));
                    facility.setFacilityName(cursor.getString(1));
                    facility.setFacilityCode(cursor.getString(2));
                    facility.setCampId(cursor.getInt(3));
                    facility.setCampName(cursor.getString(4));
                    facility.setProgrammingPartnerName(cursor.getString(5));
                    facility.setImplemantationPartnerName(cursor.getString(6));
                    facility.setCollectionStatus(cursor.getInt(12));
                    facilityList.add(facility);
                } while (cursor.moveToNext());
            }
            response.setData(facilityList);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
        return response;
    }

    public PagedResponse<FacilityListDatum> getFacilitiesContainsBeneficiaryToUpload(QueryParamModel queryParam) {
        String searchText = queryParam.getSearchText();
        PagedResponse<FacilityListDatum> response = new PagedResponse<>();
        Integer result;
        SQLiteDatabase database1 = dbHelper.getReadableDatabase();
        String countQuery = "select count(DISTINCT f.id)   " +
                "from tbl_beneficiary_data_collection_status s  " +
                "inner join tbl_beneficiary b on s.beneficiaryId = b.id  " +
                "inner join tbl_facility f on b.facilityId = f.id  " +
                "where s.status = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            countQuery = countQuery + " and ( f." + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    "f." + COL_FACILITY_CODE + " like '%" + searchText + "%' ) ";
        }

        int count = 0;
        Cursor cursor1 = null;
        try {
            cursor1 = database1.rawQuery(countQuery, new String[]{
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            if (cursor1.moveToFirst()) {
                do {
                    count = cursor1.getInt(0);
                } while (cursor1.moveToNext());
            }
            result = count;
        } finally {
            if (cursor1 != null) {
                cursor1.close();
            }
            database1.close();
        }
        response.setTotal(result);
        SQLiteDatabase database = dbHelper.getReadableDatabase();

        int offset = (queryParam.getPageNumber() - 1) * queryParam.getPageSize();
        String query = "select DISTINCT f.*, s.status  " +
                "from tbl_beneficiary_data_collection_status s  " +
                "inner join tbl_beneficiary b on s.beneficiaryId = b.id  " +
                "inner join tbl_facility f on b.facilityId = f.id  " +
                "where s.status = ? ";
        if (searchText != null && !searchText.isEmpty()) {
            query = query + " and ( f." + COL_FACILITY_NAME + " like '%" + searchText + "%' or " +
                    "f." + COL_FACILITY_CODE + " like '%" + searchText + "%' ) ";
        }
        query = query + " order by f." + COL_FACILITY_CODE + " limit " + queryParam.getPageSize() + " offset " + offset;
        Cursor cursor = null;
        List<FacilityListDatum> facilityList = new ArrayList<FacilityListDatum>();
        try {
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            if (cursor.moveToFirst()) {
                // looping through all facilityList and adding to arraylist
                do {
                    FacilityListDatum facility = new FacilityListDatum();
                    facility.setId(cursor.getInt(0));
                    facility.setFacilityName(cursor.getString(1));
                    facility.setFacilityCode(cursor.getString(2));
                    facility.setCampId(cursor.getInt(3));
                    facility.setCampName(cursor.getString(4));
                    facility.setProgrammingPartnerName(cursor.getString(5));
                    facility.setImplemantationPartnerName(cursor.getString(6));
                    facility.setCollectionStatus(cursor.getInt(12));
                    facilityList.add(facility);
                } while (cursor.moveToNext());
            }
            response.setData(facilityList);
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
        return response;
    }

    public ArrayList<FacilityPost> getFacilitiesToUpload(int instanceId) {
        ArrayList<BeneficiaryIndicator> instanceIndicators = getInstanceIndicators(instanceId);
        HashMap<Integer, BeneficiaryIndicator> indicatorDictionary = new HashMap<Integer, BeneficiaryIndicator>();
        for (BeneficiaryIndicator indicator:instanceIndicators){
            indicatorDictionary.put((int) indicator.getDynamicColumnId(), indicator);
        }
        HashMap<Integer, FacilityPost> resultDictionary = new HashMap<Integer, FacilityPost>();
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        String query = "SELECT r.id, r.facilityId, r.columnId, r.value  " +
                "from tbl_facility_records r  " +
                "join tbl_facility f on r.facilityId = f.id and r.instanceId = ?  " +
                "join tbl_facility_data_collection_status s on s.facilityId = f.id and s.instanceId = ?  " +
                "where s.status = ?  " +
                "order by r.facilityId, r.columnId";
        Cursor cursor = null;
        try{
            cursor = database.rawQuery(query, new String[]{
                    String.valueOf(instanceId),
                    String.valueOf(instanceId),
                    String.valueOf(CollectionStatus.Collected.getIntValue())
            });
            if(cursor.moveToFirst()){
                do{
                    Integer facilityId = Integer.valueOf(cursor.getInt(1));
                    FacilityPost facility = null;
                    if(!resultDictionary.containsKey(facilityId)){
                        facility = new FacilityPost();
                        facility.setFacilityId(facilityId);
                        facility.setInstanceId(instanceId);
                        resultDictionary.put(facilityId, facility);
                    }
                    else{
                        facility = resultDictionary.get(facilityId);
                    }
                    FacilityDynamicCell cell = new FacilityDynamicCell();
                    int columnId = cursor.getInt(2);
                    cell.setEntityDynamicColumnId(columnId);
                    if (indicatorDictionary.containsKey(columnId)){
                        cell.setDataType(indicatorDictionary.get(columnId).getColumnDataType());
                    }

                    cell.setRecordId(cursor.getInt(0));
                    String value = cursor.getString(3);
                    if(value != null && !value.isEmpty()){
                        if(cell.getDataType() == DataTypes.List.getIntValue()){
                            String[] listValues = value.split(",");
                            for (int i = 0; i < listValues.length; i++){
                                cell.getValue().add(listValues[i].trim());
                            }
                        }
                        else{
                            cell.getValue().add(value.trim());
                        }
                        facility.getDynamicCells().add(cell);
                    }
                } while (cursor.moveToNext());
            }

            ArrayList<FacilityPost> facilityPosts = new ArrayList<FacilityPost>(resultDictionary.values());
            return facilityPosts;
        }
        finally {
            if(cursor != null){
                cursor.close();
            }
            database.close();
        }
    }

    public void deleteRecords(ArrayList<FacilityPost> facilityData) {
//        if (facilityData !=null){
//            for (int value =0; value<facilityData.size(); value++){
//                if (facilityData.get(value) !=null){
//                    deleteRecord(facilityData.get(value));
//                } else {
//                    Log.d("Null Position", String.valueOf(value));
//                }
//
//            }
//
//        }
        for (FacilityPost post:facilityData){
            if (post == null){
                Log.d("Null Value", "nulto");
            } else {
                deleteRecord(post);
            }

        }

    }

    private void deleteRecord(FacilityPost facilityPost) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try{
            String whereClause = COL_FACILITY_ID + " = ? AND " + COL_INSTANCE_ID + " = ?";
            database.delete(TBL_FACILITY_RECORDS, whereClause, new String[]{
                    String.valueOf(facilityPost.getFacilityId()),
                    String.valueOf(facilityPost.getInstanceId())
            });

            database.delete(TBL_FACILITY_DATA_COLLECTION_STATUS, whereClause, new String[]{
                    String.valueOf(facilityPost.getFacilityId()),
                    String.valueOf(facilityPost.getInstanceId())
            });
        }
        finally {
            database.close();
        }
    }


    public void saveFacilityGetById( FacilityListDatum facility){
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_ID, facility.getId());
            cv.put(COL_PROGRAM_PARTNER_NAME, facility.getProgramPartnerName());
            cv.put(IMPLEMENTATION_PARTNER_NAME, facility.getImplementationPartnerName());
            cv.put(COL_UNION_NAME, facility.getUnionName());
            cv.put(COL_UPZILLA_NAME, facility.getUpazilaName());
            cv.put(CAMP_NAME, facility.getCampName());
            cv.put(COL_BLOCK_NAME, facility.getBlockName());
            cv.put(COL_TEACHER_NAME,facility.getTeacherName());

            database.update(TBL_FACILITY, cv, "id=" + facility.getId(), null);


        }  finally {
            database.close();
        }
    }

    public FacilityListDatum getFacilityGetById(int id) {
        List<FacilityListDatum> data = new ArrayList<>();
//        FacilityGetByIdModel data = new FacilityGetByIdModel();
        FacilityListDatum data_model = new FacilityListDatum();
        SQLiteDatabase db = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String select_query = "select * from " + TBL_FACILITY +" where id =" +id;
            cursor = db.rawQuery(select_query, null);
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {

                    data_model.setId(cursor.getInt(0));
                    data_model.setProgramPartnerName(cursor.getString(5));
                    data_model.setImplementationPartnerName(cursor.getString(6));
                    data_model.setUnionName(cursor.getString(7));
                    data_model.setUpazilaName(cursor.getString(8));
                    data_model.setCampName(cursor.getString(9));
                    data_model.setBlockName(cursor.getString(10));
                    data_model.setTeacherName(cursor.getString(11));

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
