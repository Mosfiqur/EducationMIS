package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.unicef.mis.model.SubBlock;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.COL_BLOCK_ID;
import static com.unicef.mis.constants.DatabaseConstants.ID;
import static com.unicef.mis.constants.DatabaseConstants.NAME;
import static com.unicef.mis.constants.DatabaseConstants.TBL_SUB_BLOCK;

public class SubBlockDataAccess {

    private SQLiteDatabaseHelper dbHelper;

    public SubBlockDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public void saveOrUpdateSubBlock(List<SubBlock> subBlocks) {
        for (SubBlock subBlock : subBlocks) {
            saveOrUpdateSubBlockData(subBlock);
        }
    }

    private void saveOrUpdateSubBlockData(SubBlock subBlock) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_SUB_BLOCK + " where id = " + subBlock.getId().toString();
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            if (count > 0) {
                updateSubBlock(subBlock);
            } else {
                insertSubBlock(subBlock);
            }
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private void updateSubBlock(SubBlock subBlock) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        try {
            contentValues.put(NAME, subBlock.getName());
            database.update(TBL_SUB_BLOCK, contentValues, "id=" + subBlock.getId().toString(), null);
        } finally {
            database.close();
        }
    }

    public Boolean insertSubBlock(SubBlock subBlock){
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        contentValues.put(ID, subBlock.getId());
        contentValues.put(NAME, subBlock.getName());
        contentValues.put(COL_BLOCK_ID, subBlock.getBlockId());

        long result = db.insert(TBL_SUB_BLOCK, null, contentValues);

        if (result == -1)
            return false;
        else
            return true;
    }

    public List<SubBlock> getSubBlock(int BlockId){
        // Data model list in which we have to return the data
        List<SubBlock> data = new ArrayList<SubBlock>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_SUB_BLOCK + " WHERE " + COL_BLOCK_ID + "  =" + BlockId;
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    SubBlock data_model = new SubBlock(cursor.getInt(0), cursor.getString(1), cursor.getInt(2));
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
