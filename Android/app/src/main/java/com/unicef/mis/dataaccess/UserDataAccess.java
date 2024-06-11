package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.unicef.mis.model.auth.UserProfile;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.COL_USER_DESIGNATIONID;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_DESIGNATIONNAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_EMAIL;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_FULLNAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_LEVELID;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_LEVELNAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_LEVELRANK;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_PHONENUMBER;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_ROLEID;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_ROLENAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_USER_TOKEN;
import static com.unicef.mis.constants.DatabaseConstants.TBL_USER;

public class UserDataAccess {
    private SQLiteDatabaseHelper dbHelper;

    public UserDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public boolean insertUser(Integer id, String fullname, Integer levelid, String levelname,
                              Integer levelrank, Integer designationid, String designationname, int roleid, String rolename, String email,
                              String phonenumber, String token) {

        SQLiteDatabase db = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        contentValues.put(COL_USER_ID, id);
        contentValues.put(COL_USER_FULLNAME, fullname);
        contentValues.put(COL_USER_LEVELID, levelid);
        contentValues.put(COL_USER_LEVELNAME, levelname);
        contentValues.put(COL_USER_LEVELRANK, levelrank);
        contentValues.put(COL_USER_DESIGNATIONID, designationid);
        contentValues.put(COL_USER_DESIGNATIONNAME, designationname);
        contentValues.put(COL_USER_ROLEID, roleid);
        contentValues.put(COL_USER_ROLENAME, rolename);
        contentValues.put(COL_USER_EMAIL, email);
        contentValues.put(COL_USER_PHONENUMBER, phonenumber);
        contentValues.put(COL_USER_TOKEN, token);
//        contentValues.put(EDUSECTORPARTNERS, edusectorpartners);

        long result = db.insert(TBL_USER, null, contentValues);

        if (result == -1)
            return false;
        else
            return true;
    }

    //get the all Profile
    public List<UserProfile> getProfile() {
        // Data model list in which we have to return the data
        List<UserProfile> data = new ArrayList<UserProfile>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_USER;
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    UserProfile data_model = new UserProfile(cursor.getInt(0), cursor.getString(1), cursor.getInt(2),
                            cursor.getString(3), cursor.getInt(4), cursor.getInt(5),
                            cursor.getString(6), cursor.getInt(7), cursor.getString(8),
                            cursor.getString(9), cursor.getString(10));
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

    public List<UserProfile> deleteProfile() {
        List<UserProfile> data = new ArrayList<>();

        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data

//        DELETE FROM Customers;

        String select_query = "DELETE FROM " + TBL_USER;
        System.out.println(select_query);
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    UserProfile data_model = new UserProfile();
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
}
