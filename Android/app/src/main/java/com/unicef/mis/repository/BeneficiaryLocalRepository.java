package com.unicef.mis.repository;

import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.util.Promise;

public class BeneficiaryLocalRepository {
    private BeneficiaryDataAccess dataAccess;
    public BeneficiaryLocalRepository(){
        dataAccess = DataAccessFactory.getBeneficiaryDataAccess();
    }

    public Promise getBeneficiaries(int facilityId){
        return new Promise();
    }
}
