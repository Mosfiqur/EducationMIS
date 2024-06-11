package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.unicef.mis.model.Block;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.CAMP_ID;
import static com.unicef.mis.constants.DatabaseConstants.CODE;
import static com.unicef.mis.constants.DatabaseConstants.ID;
import static com.unicef.mis.constants.DatabaseConstants.NAME;
import static com.unicef.mis.constants.DatabaseConstants.TBL_BLOCK;

public class BlockDataAccess {

    private SQLiteDatabaseHelper dbHelper;

    public BlockDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public void saveOrUpdateBlock(List<Block> blockList) {
        for (Block block : blockList) {
            saveOrUpdateBlockData(block);
        }
    }

    private void saveOrUpdateBlockData(Block block) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;

        try {
            String query = "select * from " + TBL_BLOCK + " where id = " + block.getId().toString();
            cursor = database.rawQuery(query, null);
            int count = cursor.getCount();
            if (count > 0) {
                updateBlock(block);
            } else {
                insertBlock(block);
            }
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }

    private void updateBlock(Block block) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        try {
            contentValues.put(CODE, block.getCode());
            contentValues.put(NAME, block.getName());
            database.update(TBL_BLOCK, contentValues, "id=" + block.getId().toString(), null);
        } finally {
            database.close();
        }
    }

    public boolean insertBlock(Block block){
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        ContentValues contentValues = new ContentValues();

        contentValues.put(ID, block.getId());
        contentValues.put(CODE, block.getCode());
        contentValues.put(NAME, block.getName());
        contentValues.put(CAMP_ID, block.getCampId());

        long result = db.insert(TBL_BLOCK, null, contentValues);

        if (result == -1)
            return false;
        else
            return true;
    }

    public List<Block> getBlock(int campId){
        // Data model list in which we have to return the data
        List<Block> data = new ArrayList<Block>();
        // Accessing database for reading data
        SQLiteDatabase db = dbHelper.getWritableDatabase();
        // Select query for selecting whole table data
        String select_query = "Select * from " + TBL_BLOCK + " WHERE campId = " + campId;
        // Cursor for traversing whole data into database
        Cursor cursor = db.rawQuery(select_query, null);
        try {
            // check if cursor move to first
            if (cursor.moveToFirst()) {
                // looping through all data and adding to arraylist
                do {
                    Block data_model = new Block(cursor.getInt(0), cursor.getString(1), cursor.getString(2),
                            cursor.getInt(3));
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
