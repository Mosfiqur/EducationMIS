package com.unicef.mis.repository;

import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.interfaces.IScheduleRepository;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.util.Promise;

import java.util.ArrayList;
import java.util.List;

public class ScheduleLocalRepository implements IScheduleRepository {
    private BeneficiaryDataAccess beneficiaryDB;
    private FacilityDataAccess facilityDB;

    public ScheduleLocalRepository(){
        beneficiaryDB = DataAccessFactory.getBeneficiaryDataAccess();
        facilityDB = DataAccessFactory.getFacilityDataAccess();
    }
    @Override
    public Promise getSchedule(int entityType) {
        Promise promise = new Promise();
        try {
            List<ScheduledInstance> instanceList = new ArrayList<ScheduledInstance>();
            if (entityType == EntityType.Beneficiary.getIntValue()) {
                instanceList.addAll(beneficiaryDB.getBeneficiarySchedule());
            } else {
                instanceList.addAll(facilityDB.getFacilitySchedule());
            }
            Schedule schedule = new Schedule();
            schedule.setTotal(instanceList.size());
            schedule.setData(instanceList);
            promise.resolve(schedule);
        }
        catch (Exception e){
            promise.reject(e.getMessage());
        }
        return promise;
    }
}
