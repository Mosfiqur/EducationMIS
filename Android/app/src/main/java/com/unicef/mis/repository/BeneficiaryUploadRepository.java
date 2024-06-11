package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IBeneficiaryRepository;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.CreateBeneficiaryModel;
import com.unicef.mis.model.DeactiveModel;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.indicator.BeneficiaryIndicatorModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;
import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class BeneficiaryUploadRepository implements IBeneficiaryRepository {
    public BeneficiaryApi beneficiaryApi;
    private BeneficiaryDataAccess beneficiaryDataAccess;
    public SharedPreferences sharedPreferences;
    private Context context;
    public static String token = "";
    private int instanceId;
    private int uploadingFacilityId;

    public BeneficiaryUploadRepository(Context context) {
        this.context = context;
        beneficiaryDataAccess = DataAccessFactory.getBeneficiaryDataAccess();
        beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    @Override
    public Promise getBeneficiariesByFacility(int facilityId, int instanceId, QueryParamModel queryParam) {
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                PagedResponse<Beneficiary> response = beneficiaryDataAccess.getBeneficiariesByFacilityForUpload(facilityId, instanceId, queryParam);
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
        this.instanceId = instanceId;
        uploadingFacilityId = facilityId;
        if (isNetworkAvailable() == false){
            showError("No Internet Connection");
        } else {
            Promise getNewBeneficiariesPromise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
                @Override
                public Object execute() {
                    ArrayList<CreateBeneficiaryModel> newBeneficiaries = beneficiaryDataAccess.getNewBeneficiaries(facilityId);
                    return newBeneficiaries;
                }
            });

            getNewBeneficiariesPromise.then(res -> {
                ArrayList<CreateBeneficiaryModel> newBeneficiaries = (ArrayList<CreateBeneficiaryModel>) res;
                if (newBeneficiaries.size() > 0) {
                    uploadNew(newBeneficiaries, promise);
                } else {
                    getInactiveBeneficiaries(promise);
                }
                return null;
            }).error(err -> {
                showError(err);
                showWait("Uploading...", context);
                getInactiveBeneficiaries(promise);
            });
        }



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
        promise.resolve("No implementation needed");
        return promise;
    }

    private Promise createNewBeneficiary(CreateBeneficiaryModel beneficiary) {
        Promise promise = new Promise();
        if(beneficiary.getFcnId() == null || beneficiary.getFcnId().isEmpty()){
            beneficiary.setFcnId("fake fcn id");
        }
        if(beneficiary.getUnhcrId() == null || beneficiary.getUnhcrId().isEmpty()){
            beneficiary.setUnhcrId("fake unhcr id");
        }
        Call<CreateBeneficiaryModel> call = beneficiaryApi.createBeneficiary("Bearer" + " " + token, beneficiary);

        call.enqueue(new Callback<CreateBeneficiaryModel>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<CreateBeneficiaryModel> call, Response<CreateBeneficiaryModel> response) {
                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        CreateBeneficiaryModel newBeneficiary = (CreateBeneficiaryModel) response.body();
                        beneficiary.setId(newBeneficiary.getId());
                        promise.resolve(beneficiary);
                    } else {
                        promise.reject("Internal server error");
                    }
                } else {
                    promise.reject("Internal server error");
                }
            }

            @Override
            public void onFailure(Call<CreateBeneficiaryModel> call, Throwable t) {
                promise.reject(t.getMessage());
            }
        });

        return promise;
    }


    private void uploadNew(ArrayList<CreateBeneficiaryModel> newBeneficiaries, Promise promise) {
        ArrayList<Promise> allPromoises = new ArrayList<Promise>();
        for (CreateBeneficiaryModel beneficiary : newBeneficiaries) {
            allPromoises.add(this.createNewBeneficiary(beneficiary));
        }

        Promise[] promiseArray = allPromoises.toArray(new Promise[allPromoises.size()]);
        Promise.all(promiseArray).then(results -> {
            Object[] beneficiaryObjects = (Object[]) results;
            ArrayList<CreateBeneficiaryModel> beneficiaries = new ArrayList<CreateBeneficiaryModel>();
            for (int i = 0; i < beneficiaryObjects.length; i++) {
                beneficiaries.add((CreateBeneficiaryModel) beneficiaryObjects[i]);
            }
            updateBeneficiariesEntityId(beneficiaries, promise);
            return null;
        }).error(err -> {
            showError(err);
            getInactiveBeneficiaries(promise);
        });
    }

    private void updateBeneficiariesEntityId(ArrayList<CreateBeneficiaryModel> beneficiaries, Promise promise) {
        Promise promise1 = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.updateBeneficiariesEntityId(beneficiaries);
                return null;
            }
        });

        promise1.then(res -> {
            getInactiveBeneficiaries(promise);
            return null;
        }).error(err -> {
            showError(err);
            getInactiveBeneficiaries(promise);
        });
    }


    private void getInactiveBeneficiaries(Promise promise) {
        Promise promise1 = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                ArrayList<Beneficiary> inactiveBeneficiaries = beneficiaryDataAccess.getInactiveBeneficiaries(uploadingFacilityId);
                return inactiveBeneficiaries;
            }
        });

        promise1.then(res -> {
            ArrayList<Beneficiary> inactiveBeneficiaries = (ArrayList<Beneficiary>) res;
            Promise promise2 = deactivateBeneficiaries(inactiveBeneficiaries);
            promise2.then(res2 -> {
                deleteInactiveBeneficiaries(inactiveBeneficiaries, promise);
                return null;
            }).error(err -> {
                showError(err);
                showWait("Uploading...", context);
                getCollectedBeneficiaries(promise);
            });
            return null;
        }).error(err -> {
            showError(err);
            showWait("Uploading...", context);
            getCollectedBeneficiaries(promise);
        });
    }

    private void deleteInactiveBeneficiaries(ArrayList<Beneficiary> inactiveBeneficiaries, Promise promise) {
        Promise promise1 = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.deleteInactiveBeneficiaries(inactiveBeneficiaries);
                return null;
            }
        });

        promise1.then(res -> {
            getCollectedBeneficiaries(promise);
            return null;
        }).error(err -> {
            showError(err);
            getCollectedBeneficiaries(promise);
        });
    }


    private Promise deactivateBeneficiaries(ArrayList<Beneficiary> inactiveBeneficiaries) {
        Promise promise1 = new Promise();
        List<Integer> beneficairyIds = new ArrayList<Integer>();
        for (Beneficiary beneficiary : inactiveBeneficiaries) {
            beneficairyIds.add(Integer.valueOf(beneficiary.getEntityId()));
        }
        DeactiveModel deactiveModel = new DeactiveModel(beneficairyIds, this.instanceId);
        if (isNetworkAvailable()) {
            Call<Void> call = beneficiaryApi.deactiveBeneficiary("Bearer" + " " + token, deactiveModel);
            call.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            promise1.resolve(null);
                        } else {
//                            promise1.reject("Internal server error");
                        }
                    } else {
//                        promise1.reject("Internal server error");
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    promise1.reject(t.getMessage());
                }
            });
        } else {
            promise1.reject("No internet connection");
        }

        return promise1;
    }

    private void getCollectedBeneficiaries(Promise promise) {
        Promise promise1 = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                ArrayList<BeneficiaryPost> beneficiaryPosts = beneficiaryDataAccess.getBeneficiariesToUpload(instanceId);
                return beneficiaryPosts;
            }
        });

        promise1.then(res -> {
            ArrayList<BeneficiaryPost> beneficiaryPosts = (ArrayList<BeneficiaryPost>) res;
            uploadCollectedRecords(beneficiaryPosts, promise);
            return null;
        }).error(err -> {
            promise.reject(err);
        });
    }

    private void uploadCollectedRecords(ArrayList<BeneficiaryPost> beneficiaryPosts, Promise promise) {
        IBeneficiaryRepository onlineRepo = RepositoryFactory.getBeneficiaryRepository(OperationMode.Online.getIntValue(), context);
        ArrayList<Promise> allPromises = new ArrayList<Promise>();
        for (BeneficiaryPost beneficiaryPost : beneficiaryPosts) {
            allPromises.add(onlineRepo.saveRecords(beneficiaryPost));
        }

        Promise[] promiseArray = allPromises.toArray(new Promise[allPromises.size()]);
        Promise.all(promiseArray).then(results -> {
            Object[] successfullUploads = (Object[]) results;
            ArrayList<BeneficiaryPost> posts = new ArrayList<BeneficiaryPost>();
            for (int i = 0; i < successfullUploads.length; i++) {
                posts.add((BeneficiaryPost) successfullUploads[i]);
            }
            deleteSuccessfulUploads(posts, promise);
            return null;
        }).error(err -> {
            showToast(context, (String)err);
            promise.reject(err);
        });
    }

    private void deleteSuccessfulUploads(ArrayList<BeneficiaryPost> posts, Promise promise) {
        Promise deletePromise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.deleteRecords(posts);
                return null;
            }
        });

        deletePromise.then(res->{
            promise.resolve(true);
            return null;
        }).error(err->{
            promise.reject(err);
        });
    }

}
