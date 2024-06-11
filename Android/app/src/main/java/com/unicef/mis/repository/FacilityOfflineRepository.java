package com.unicef.mis.repository;

import android.content.Context;

import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.interfaces.IFacilityRepository;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.facility.indicator.FacilityIndicatorModel;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.util.Promise;

public class FacilityOfflineRepository implements IFacilityRepository {
    private final FacilityDataAccess facilityDataAccess;
    private Context context;

    public FacilityOfflineRepository(Context context) {
        this.context = context;
        facilityDataAccess = DataAccessFactory.getFacilityDataAccess();
    }

    @Override
    public Promise searchFacilitiesForBeneficiary(int instanceId, QueryParamModel queryParam) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                PagedResponse<FacilityListDatum> result = facilityDataAccess.searchFacilities(queryParam);
                return result;
            }
        });
        return promise;
    }

    @Override
    public Promise searchFacilitiesByInstance(int instanceId, QueryParamModel queryParam) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                PagedResponse<FacilityListDatum> result = facilityDataAccess.searchFacilitiesByInstance(instanceId, queryParam);
                return result;
            }
        });
        return promise;
    }

    @Override
    public Promise getRecords(int instanceId, int facilityId) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                FacilityIndicatorModel result = facilityDataAccess.getFacilityRecords(instanceId, facilityId);
                return result;
            }
        });
        return promise;
    }

    @Override
    public Promise saveRecords(FacilityPost records) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {

                return facilityDataAccess.saveFacilityRecords(records);
            }
        });
        return promise;
    }

    @Override
    public Promise uploadFacilities(int instanceId) {
        Promise promise = new Promise();
        promise.resolve("Nothing to implement");
        return promise;
    }

    @Override
    public Promise facilityGetById(int id, int instanceId) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                FacilityListDatum result = facilityDataAccess.getFacilityGetById(id);
                return result;
            }
        });
        return promise;
    }
}
