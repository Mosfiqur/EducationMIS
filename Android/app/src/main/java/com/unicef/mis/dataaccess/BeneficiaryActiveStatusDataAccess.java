package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.COL_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_ISACTIVE;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BENEFICIARY;

public class BeneficiaryActiveStatusDataAccess {

    private SQLiteDatabaseHelper dbHelper;

    public BeneficiaryActiveStatusDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public List<Beneficiary> isActiveStatus(int entityId){
        ArrayList<Beneficiary> data = new ArrayList<Beneficiary>();
        SQLiteDatabase db = dbHelper.getWritableDatabase();

        String select_query = "Select * from " + TBL_BENEFICIARY + " WHERE entityId = " + entityId;
        System.out.println(select_query);
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    boolean isActive = cursor.getInt(2) > 0;
                    Beneficiary data_model = new Beneficiary(cursor.getInt(1), isActive);
                    System.out.println("IsActive Value: "+data_model.getActive());
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

    public void updateBeneficiaryStatus(int beneficiaryId, boolean isActive, IGenericApiCallBack callBack) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {

            ContentValues cv = new ContentValues();
            cv.put(COL_ISACTIVE, isActive? 1 : 0);
            String whereClause = COL_ID + "=" + beneficiaryId;
            database.update(TBL_BENEFICIARY, cv, whereClause, null);

            callBack.apiCallSuccessful(null, null);
        } catch (Exception e) {
            e.printStackTrace();
            callBack.apiCallFailed(true, e.getMessage());
        } finally {
            database.close();
        }
    }
}
