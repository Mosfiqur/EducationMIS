package com.unicef.mis.dataaccess;

import android.content.ContentValues;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;

import com.unicef.mis.model.ListItem;
import com.unicef.mis.model.ListObject;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.constants.DatabaseConstants.COL_FACILITY_TITLE;
import static com.unicef.mis.constants.DatabaseConstants.COL_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_LIST_ID;
import static com.unicef.mis.constants.DatabaseConstants.COL_NAME;
import static com.unicef.mis.constants.DatabaseConstants.COL_VALUE;
import static com.unicef.mis.constants.DatabaseConstants.TBL_LIST_ITEMS;
import static com.unicef.mis.constants.DatabaseConstants.TBL_LIST_OBJECT;

public class ListDataAccess {
    private SQLiteDatabaseHelper dbHelper;

    public ListDataAccess(SQLiteDatabaseHelper dbHelper) {

        this.dbHelper = dbHelper;
    }


    public void saveOrUpdateLists(List<ListObject> lists) {
        for (ListObject list : lists) {
            saveOrUpdateList(list);
        }
    }

    public void saveOrUpdateList(ListObject list) {
        ListObject dbList = getListById(list.getId());
        if (dbList == null) {
            saveList(list);
        }
    }

    public void saveList(ListObject list) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        try {
            ContentValues cv = new ContentValues();
            cv.put(COL_ID, list.getId());
            cv.put(COL_NAME, list.getName());
            long rowId = database.insert(TBL_LIST_OBJECT, null, cv);
            for (ListItem li : list.getItems()) {
                ContentValues contentValue = new ContentValues();
                contentValue.put(COL_ID, li.getId());
                contentValue.put(COL_LIST_ID, list.getId());
                contentValue.put(COL_FACILITY_TITLE, li.getTitle());
                contentValue.put(COL_VALUE, li.getValue());
                database.insert(TBL_LIST_ITEMS, null, contentValue);
            }
        } finally {
            database.close();
        }
    }

    public ListObject getListById(long listId) {
        SQLiteDatabase database = dbHelper.getReadableDatabase();
        Cursor cursor = null;
        try {
            ListObject listObject = null;
            List<ListItem> items = new ArrayList<ListItem>();
            String query = "select a.id as list_id, a.name as list_name, b.id as list_item_id, " +
                    "b.title, b.value from " + TBL_LIST_OBJECT + " a  " +
                    "inner join " + TBL_LIST_ITEMS + " b on b.listId = a.id where a.id=?";
            cursor = database.rawQuery(query, new String[]{String.valueOf(listId)});
            if (cursor.moveToFirst()) {
                do {
                    if (listObject == null) {
                        listObject = new ListObject();
                        listObject.setId(cursor.getInt(0));
                        listObject.setName(cursor.getString(1));
                    }
                    ListItem li = new ListItem(cursor.getInt(2), cursor.getString(3), cursor.getInt(4));
                    items.add(li);
                }
                while (cursor.moveToNext());
                listObject.setItems(items);
            }
            return listObject;
        } finally {
            if (cursor != null) {
                cursor.close();
            }
            database.close();
        }
    }
}
