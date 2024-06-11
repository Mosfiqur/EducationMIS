package com.unicef.mis.repository;

import android.content.Context;

import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.interfaces.IBeneficiaryRepository;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.indicator.BeneficiaryIndicatorModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.util.Promise;

public class BeneficiaryOfflineRepository implements IBeneficiaryRepository {
    private BeneficiaryDataAccess beneficiaryDataAccess;
    private Context context;

    public BeneficiaryOfflineRepository(Context context) {
        this.context = context;
        beneficiaryDataAccess = DataAccessFactory.getBeneficiaryDataAccess();
    }

    @Override
    public Promise getBeneficiariesByFacility(int facilityId, int instanceId, QueryParamModel queryParam) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                PagedResponse<Beneficiary> response = beneficiaryDataAccess.getBeneficiariesByFacility(facilityId, instanceId, queryParam);
                return response;
            }
        });
        return promise;
    }

    @Override
    public Promise getRecords(int instanceId, Beneficiary beneficiary) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                BeneficiaryIndicatorModel result = beneficiaryDataAccess.getBeneficiaryRecords(instanceId, beneficiary.getId());
                return result;
            }
        });
        return promise;
    }

    @Override
    public Promise saveRecords(BeneficiaryPost beneficiaryPost) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.saveRecords(beneficiaryPost);
                return null;
            }
        });
        return promise;
    }

    @Override
    public Promise changeActiveStatus(boolean activeStatus, Beneficiary beneficiary) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.changeActiveStatus(activeStatus, beneficiary.getId());
                return true;
            }
        });
        return promise;
    }

    @Override
    public Promise uploadBeneficiaries(int instanceId, int facilityId) {
        Promise promise = new Promise();
        promise.resolve("NO implementation needed");
        return promise;
    }

    @Override
    public Promise beneficiaryGetById(int id, int instanceId) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                Beneficiary request = beneficiaryDataAccess.getBeneficiaryGetByIdModel(id, instanceId);
                return request;
            }
        });
        return promise;
    }

    @Override
    public Promise createBeneficiary(Beneficiary beneficiary) {
        Promise promise = new Promise();
        promise.resolve("Need to implement and use this api");
        return promise;
    }
}
