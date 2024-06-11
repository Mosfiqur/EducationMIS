package com.unicef.mis.constants;

import java.security.PublicKey;

public class APIConstants {
    public static final String AUTH = "Auth/Login";
    public static final String RUNNING_INSTANCES = "Schedule/GetRunningInstances";
    public static final String FACILITY_INDICATOR = "Indicator/GetFacilityIndicator";
    public static final String FACILITY_INDICATOR_POST = "DynamicProperties/SaveFacilityCell";
    public static final String GET_FACILITY_RECORDS = "Facility/GetAllForDevice";
    public static final String FACILITY_INDICATOR_BY_INSTANCE  ="Indicator/GetIndicatorsByInstance";
    public static final String FACILITY_GET_BY_ID = "Facility/GetById/{id}/{instanceId}";

    public static final String GET_ALL_FACILITIES = "Facility/GetAll";
    public static final String GET_ALL_FACILITIES_BENEFICIARIES = "Facility/GetAllByBeneficiaryInstance";
    public static final String GET_BENEFICIARY_RECORDS = "Beneficiary/GetByFacilityId";
    public static final String BENEFICIARY_INDICATOR = "Indicator/GetBeneficiaryIndicator";
    public static final String BENEFICIARY_INDICATOR_POST = "DynamicProperties/SaveBeneficiaryCell";
    public static final String DEACTIVE_BENEFICIARY = "Beneficiary/DeactivateBeneficiary";
    public static final String BENEFICIARY_GET_BY_ID = "Beneficiary/GetById/{id}/{instanceId}";

    public static final String GET_CAMPS = "Common/GetCamps";
    public static final String GET_BLOCKS = "Common/GetBlocks";
    public static final String GET_SUB_BLOCKS = "Common/GetSubBlocks";
    public static final String CREATE_BENEFICIARY = "beneficiary/add";

    public static final String GET_ALL_NOTIFICATIONS = "Notificaiton/GetAll";
    public static final String READ_NOTIFICATION = "Notificaiton/ReadNotification/{notificationId}";
    public static final String CLEAR_NOTIFICATION = "Notificaiton/ClearNotification";
}
