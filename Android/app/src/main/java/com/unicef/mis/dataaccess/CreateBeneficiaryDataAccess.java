package com.unicef.mis.dataaccess;

import com.unicef.mis.util.UnicefApplication;

public class CreateBeneficiaryDataAccess {
    private SQLiteDatabaseHelper dbHelper;

    public CreateBeneficiaryDataAccess() {
        this.dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }
}
