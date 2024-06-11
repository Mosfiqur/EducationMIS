package com.unicef.mis.repository;

import android.content.Context;

import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IFacilityRepository;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.facility.indicator.FacilityIndicatorModel;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.util.Promise;

import java.util.ArrayList;

//import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;

public class FacilityUploadRepository implements IFacilityRepository {
    private Context context;
    private final FacilityDataAccess facilityDataAccess;

    public FacilityUploadRepository(Context context) {
        this.context = context;
        facilityDataAccess = DataAccessFactory.getFacilityDataAccess();
    }

    @Override
    public Promise searchFacilitiesForBeneficiary(int instanceId, QueryParamModel queryParam) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                PagedResponse<FacilityListDatum> response = facilityDataAccess.getFacilitiesContainsBeneficiaryToUpload(queryParam);
                return response;
            }
        });
        return promise;
    }

    @Override
    public Promise searchFacilitiesByInstance(int instanceId, QueryParamModel queryParam) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                return facilityDataAccess.getFacilitiesForUpload(instanceId, queryParam);
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

        Promise  promise = new Promise();

        if(isNetworkAvailable() == false){
            showError("No Internet Connection");
        } else {
            Promise getFacilitiesToUploadPromise =  AsyncTaskRunner.executeAsync(new IAsyncMethod() {
                @Override
                public Object execute() {
                    ArrayList<FacilityPost> facilityPosts = facilityDataAccess.getFacilitiesToUpload(instanceId);
                    return facilityPosts;
                }
            });

            getFacilitiesToUploadPromise.then(res -> {
                ArrayList<FacilityPost> facilityPosts = (ArrayList<FacilityPost>) res;
                startUpload(facilityPosts, promise);
                return null;
            }).error(err -> {
                promise.reject(err);
            });
        }



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


    private void startUpload(ArrayList<FacilityPost> facilityPosts, Promise promise) {
        IFacilityRepository onlineRepo = RepositoryFactory.getFacilityRepository(OperationMode.Online.getIntValue(), context);
        ArrayList<Promise> allPromises = new ArrayList<Promise>();
        for (FacilityPost facilityPost:facilityPosts){
            allPromises.add(onlineRepo.saveRecords(facilityPost));
        }

        Promise[] promiseArray = allPromises.toArray(new Promise[allPromises.size()]);
        Promise.all(promiseArray).then(results->{
            Object[] successfullUploads = (Object[]) results;
            ArrayList<FacilityPost> listOfFacility = new ArrayList<FacilityPost>();
            for (int i = 0; i < successfullUploads.length; i++){
                listOfFacility.add((FacilityPost)successfullUploads[i]);
            }
            deleteSuccessfulUploads(listOfFacility, promise);
            return null;
        }).error(err->{
            showError(err);
        });
    }

    private void deleteSuccessfulUploads(ArrayList<FacilityPost> facilityPosts, Promise promise) {
        Promise deletePromise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                facilityDataAccess.deleteRecords(facilityPosts);
                return null;
            }
        });

        deletePromise.then(res->{
            promise.resolve(true);
            return null;
        }).error(err->{
            promise.reject(err);
            showError(err);
        });
    }
}
