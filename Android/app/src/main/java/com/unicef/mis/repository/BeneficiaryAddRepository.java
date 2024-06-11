package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;
import android.provider.ContactsContract;
import android.widget.Toast;

import androidx.annotation.RequiresApi;
import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.BlockDataAccess;
import com.unicef.mis.dataaccess.CampDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.dataaccess.SubBlockDataAccess;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.CreateBeneficiaryModel;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.SubBlock;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.UnicefApplication;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class BeneficiaryAddRepository {
    private BeneficiaryApi beneficiaryApi;
    private SharedPreferences sharedPreferences;
    private CampDataAccess campDataAccess ;
    private BlockDataAccess blockDataAccess;
    private SubBlockDataAccess subBlockDataAccess;
    private BeneficiaryDataAccess beneficiaryDataAccess;


    public BeneficiaryAddRepository(Context appContext) {
        campDataAccess = DataAccessFactory.getCampDataAccess();
        blockDataAccess = DataAccessFactory.getBlockDataAccess();
        subBlockDataAccess = DataAccessFactory.getSubBlockDataAccess();
        beneficiaryDataAccess = DataAccessFactory.getBeneficiaryDataAccess();
    }

    public Promise showCamps(){
        Promise promise = new Promise();
        promise.resolve(campDataAccess.getCamp());
        return promise;
    }

    public Promise showBlocks(int campId){
        Promise promise = new Promise();
        promise.resolve(blockDataAccess.getBlock(campId));
        return promise;
    }

    public Promise showSubBlocks(int blockId){
        Promise promise = new Promise();
        promise.resolve(subBlockDataAccess.getSubBlock(blockId));
        return promise;
    }

    public Promise createBeneficiary(int id, String unhcrId, String name, String fatherName, String motherName, String fcnId, String dateOfBirth,
                                     int sex, boolean disabled, int levelOfStudy, String enrollmentDate, int facilityId, int beneficiaryCampId,
                                     int blockId, int subBlockId, String remarks, boolean isApproved){
        Promise promise = new Promise();

        promise.resolve(createBeneficiary(id, unhcrId, name, fatherName, motherName, fcnId, dateOfBirth,
                sex, disabled, levelOfStudy, enrollmentDate, facilityId, beneficiaryCampId, blockId, subBlockId, remarks, isApproved));

        return promise;
    }
}
