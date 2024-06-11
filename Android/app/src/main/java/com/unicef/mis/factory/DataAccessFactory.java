package com.unicef.mis.factory;

import com.unicef.mis.dataaccess.BeneficiaryActiveStatusDataAccess;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.BlockDataAccess;
import com.unicef.mis.dataaccess.CampDataAccess;
import com.unicef.mis.dataaccess.CreateBeneficiaryDataAccess;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.dataaccess.SubBlockDataAccess;
import com.unicef.mis.dataaccess.UserDataAccess;

public class DataAccessFactory {
    public static BeneficiaryDataAccess getBeneficiaryDataAccess(){
        return new BeneficiaryDataAccess();
    }

    public static FacilityDataAccess getFacilityDataAccess() {
        return new FacilityDataAccess();
    }

    public static UserDataAccess getUserDataAccess() {
        return new UserDataAccess();
    }

    public static CampDataAccess getCampDataAccess() { return new CampDataAccess();}

    public static BlockDataAccess getBlockDataAccess() { return  new BlockDataAccess();}

    public static SubBlockDataAccess getSubBlockDataAccess () {return new SubBlockDataAccess();}

    public static CreateBeneficiaryDataAccess getCreateBeneficiaryDataAccess () { return new CreateBeneficiaryDataAccess();}

    public static BeneficiaryActiveStatusDataAccess getActiveDataAccess() {
        return  new BeneficiaryActiveStatusDataAccess();
    }

    /*
    * Implement other factory methods
    * */
}
