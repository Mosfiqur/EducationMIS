package com.unicef.mis.factory;

import android.content.Context;

import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.interfaces.IBeneficiaryRepository;
import com.unicef.mis.interfaces.IFacilityRepository;
import com.unicef.mis.interfaces.IScheduleRepository;
import com.unicef.mis.repository.BeneficiaryAddRepository;
import com.unicef.mis.repository.BeneficiaryOfflineRepository;
import com.unicef.mis.repository.BeneficiaryOnlineRepository;
import com.unicef.mis.repository.BeneficiaryUploadRepository;
import com.unicef.mis.repository.FacilityOfflineRepository;
import com.unicef.mis.repository.FacilityOnlineRepository;
import com.unicef.mis.repository.BeneficiaryRepository;
import com.unicef.mis.repository.FacilityUploadRepository;
import com.unicef.mis.repository.NotificationRepository;
import com.unicef.mis.repository.ScheduleLocalRepository;
import com.unicef.mis.repository.ScheduleOnlineRepository;
import com.unicef.mis.repository.FacilityScheduleRepository;
import com.unicef.mis.repository.UserRepository;
import com.unicef.mis.util.UnicefApplication;

public class RepositoryFactory {

    public static UserRepository getUserRepository() {
        return new UserRepository(UnicefApplication.getAppContext());
    }

    //Repository for beneficiary Schedule
    public static ScheduleOnlineRepository getBeneficiaryScheduleRepository() {
        return new ScheduleOnlineRepository(UnicefApplication.getAppContext());
    }

    //Repository for facility Schedule
    public static FacilityScheduleRepository getFacilityScheduleRepository() {
        return new FacilityScheduleRepository(UnicefApplication.getAppContext());
    }

    //Repository for beneficiary list
    public static BeneficiaryRepository getBeneficiaryRepository(){
        return new BeneficiaryRepository(UnicefApplication.getAppContext());
    }

    //Repository for facility list - Beneficiary
    public static FacilityOnlineRepository getBeneficiaryFacilityRepository(){
        return new FacilityOnlineRepository(UnicefApplication.getAppContext());
    }

    public static IScheduleRepository getScheduleRepository(int operationMode) {
        if(operationMode == OperationMode.Online.getIntValue()){
            return new ScheduleOnlineRepository(UnicefApplication.getAppContext());
        }
        return new ScheduleLocalRepository();
    }

    public static BeneficiaryAddRepository getBeneficiaryAddRepository(){
        return new BeneficiaryAddRepository(UnicefApplication.getAppContext());
    }

    public static NotificationRepository getNotificationRepository(){
        return new NotificationRepository(UnicefApplication.getAppContext());
    }

    public static IFacilityRepository getFacilityRepository(int operationMode, Context context) {
        OperationMode mode = OperationMode.fromInt(operationMode);
        switch (mode){
            case Online:
                return new FacilityOnlineRepository(context);
            case Offline:
                return new FacilityOfflineRepository(context);
            case Upload:
                return new FacilityUploadRepository(context);
        }

        return null;
    }

    public static IBeneficiaryRepository getBeneficiaryRepository(int operationMode, Context context){
        OperationMode mode = OperationMode.fromInt(operationMode);
        switch (mode){
            case Online:
                return new BeneficiaryOnlineRepository(context);
            case Offline:
                return new BeneficiaryOfflineRepository(context);
            case Upload:
                return new BeneficiaryUploadRepository(context);
        }

        return null;
    }
}
