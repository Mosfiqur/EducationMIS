package com.unicef.mis.util;

import android.content.Context;
import android.content.Intent;

import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.views.benificiary.BeneficiaryListActivity;
import com.unicef.mis.views.benificiary.BeneficiaryRecordsActivity;
import com.unicef.mis.views.benificiary.NewBeneficiaryOnlineActivity;
import com.unicef.mis.views.facility.FacilityRecordsActivity;
import com.unicef.mis.views.benificiary.NewBeneficiaryOfflineActivity;

public class NavigationHelper {

    public static void gotoOnlineBeneficiaryFacilityListPage(int instanceId, int operationMode, FacilityListDatum facility){
        Context appContext = UnicefApplication.getAppContext();
        Intent intent = new Intent(appContext, BeneficiaryListActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.putExtra(UIConstants.INTENT_EXTRA_FACILITY, facility);
        intent.putExtra(UIConstants.INTENT_EXTRA_INSTANCE_ID, instanceId);
        intent.putExtra(UIConstants.INTENT_EXTRA_OPERATION_MODE, operationMode);
        appContext.startActivity(intent);
    }

    public static void gotoFacilityRecordsPage(int instanceId, int operationMode, FacilityListDatum selectedFacility) {
        Context appContext = UnicefApplication.getAppContext();
        Intent intent = new Intent(appContext, FacilityRecordsActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.putExtra(UIConstants.INTENT_EXTRA_FACILITY, selectedFacility);
        intent.putExtra(UIConstants.INTENT_EXTRA_INSTANCE_ID, instanceId);
        intent.putExtra(UIConstants.INTENT_EXTRA_OPERATION_MODE, operationMode);
        appContext.startActivity(intent);
    }

    public static void gotoOnlineBeneficiaryRecordsPage(int instanceId, int operationMode,Beneficiary selectedBeneficiary){
        Context appContext = UnicefApplication.getAppContext();
        Intent intent = new Intent(appContext, BeneficiaryRecordsActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.putExtra(UIConstants.INTENT_EXTRA_OPERATION_MODE, operationMode);
        intent.putExtra(UIConstants.INTENT_EXTRA_BENEFICIARY, selectedBeneficiary);
        intent.putExtra(UIConstants.INTENT_EXTRA_INSTANCE_ID, instanceId);
        appContext.startActivity(intent);
    }

    public static void gotoBeneficiaryOfflineAdd(int operationMode, int instanceId, FacilityListDatum selectedFacility) {
        Context appContext = UnicefApplication.getAppContext();
        Intent intent = new Intent(appContext, NewBeneficiaryOfflineActivity.class);

        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.putExtra(UIConstants.INTENT_EXTRA_OPERATION_MODE, operationMode);
        intent.putExtra(UIConstants.INTENT_EXTRA_FACILITY, selectedFacility);
        intent.putExtra(UIConstants.INTENT_EXTRA_INSTANCE_ID, instanceId);

        appContext.startActivity(intent);
    }

    public static void gotoBeneficiaryOnlineAdd(int operationMode, int instanceId, FacilityListDatum selectedFacility) {
        Context appContext = UnicefApplication.getAppContext();
        Intent intent = new Intent(appContext, NewBeneficiaryOnlineActivity.class);

        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        intent.putExtra(UIConstants.INTENT_EXTRA_OPERATION_MODE, operationMode);
        intent.putExtra(UIConstants.INTENT_EXTRA_FACILITY, selectedFacility);
        intent.putExtra(UIConstants.INTENT_EXTRA_INSTANCE_ID, instanceId);

        appContext.startActivity(intent);
    }
}
