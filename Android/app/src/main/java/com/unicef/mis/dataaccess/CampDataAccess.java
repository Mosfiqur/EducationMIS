package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.unicef.mis.model.Camp;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.ID;
import static com.unicef.mis.constants.DatabaseConstants.SSID;
import static com.unicef.mis.constants.DatabaseConstants.NAME;
import static com.unicef.mis.constants.DatabaseConstants.NAME_ALIAS;
import static com.unicef.mis.constants.DatabaseConstants.TBL_CAMP;
import static com.unicef.mis.constants.DatabaseConstants.UNIONID;

public class CampDataAccess {

    private SQLiteDatabaseHelper dbHelper;

    public CampDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }


    public boolean saveOrUpdateCamp(List<Camp> campList) {
        for (Camp camp : campList) {
            saveOrUpdateCampData(camp);
        }
        return true;
    }

    private void saveOrUpdateCampData(Camp camp) {

        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_CAMP + " where id = " + camp.getId().toString();
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            if (count > 0) {
                updateCamp(camp);
            } else {
                insertCamp(camp);
            }
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }

    }

    private void updateCamp(Camp camp) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        try {
            contentValues.put(SSID, camp.getSsid());
            contentValues.put(NAME, camp.getName());
            contentValues.put(NAME_ALIAS, camp.getNameAlias());
            contentValues.put(UNIONID, camp.getUnionId());
            database.update(TBL_CAMP, contentValues, "id=" + camp.getId().toString(), null);
        } finally {
            database.close();
        }
    }

    public boolean insertCamp(Camp camp) {
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        contentValues.put(ID, camp.getId());
        contentValues.put(SSID, camp.getSsid());
        contentValues.put(NAME, camp.getName());
        contentValues.put(NAME_ALIAS, camp.getNameAlias());
        contentValues.put(UNIONID, camp.getUnionId());

        long result = db.insert(TBL_CAMP, null, contentValues);

        if (result == -1)
            return false;
        else
            return true;

    }


    public List<Camp> getCamp() {
        // Data model list in which we have to return the data
        List<Camp> data = new ArrayList<Camp>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_CAMP;
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    Camp data_model = new Camp(cursor.getInt(0), cursor.getString(1), cursor.getString(2),
                            cursor.getString(3), cursor.getInt(4));
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

}
