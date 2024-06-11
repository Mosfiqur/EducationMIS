package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.interfaces.IBeneficiaryRepository;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.DeactiveModel;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.indicator.BeneficiaryIndicatorModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.util.Collections;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;
import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;

public class BeneficiaryOnlineRepository implements IBeneficiaryRepository {

    public BeneficiaryApi benificaryApi;
    public SharedPreferences sharedPreferences;
    public static String token = "";

    public BeneficiaryOnlineRepository(Context context) {
        benificaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    @Override
    public Promise getBeneficiariesByFacility(int facilityId, int instanceId, QueryParamModel queryParam) {
        Promise promise = new Promise();
        Call<PagedResponse<Beneficiary>> getScheduleCall = benificaryApi.getBeneficiariesByFacility("Bearer" + " " + token, facilityId, instanceId, queryParam.getPageSize(), queryParam.getPageNumber(), queryParam.getSearchText());
        getScheduleCall.enqueue(new Callback<PagedResponse<Beneficiary>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<Beneficiary>> call, Response<PagedResponse<Beneficiary>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        PagedResponse<Beneficiary> nameList = response.body();
                        promise.resolve(nameList);
                    } else {
                        promise.reject("Internal server Error");
                    }
                }
                else{
                    promise.reject("Internal server error");
                }
            }

            @Override
            public void onFailure(Call<PagedResponse<Beneficiary>> call, Throwable t) {
                promise.reject(t.getMessage());
            }
        });

        return promise;
    }

    @Override
    public Promise getRecords(int instanceId, Beneficiary beneficiary) {
        Promise promise = new Promise();

        Call<BeneficiaryIndicatorModel> getScheduleCall = benificaryApi.getBenificiaryIndicator
                ("Bearer" + " " + token, instanceId, beneficiary.getEntityId());

        getScheduleCall.enqueue(new Callback<BeneficiaryIndicatorModel>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<BeneficiaryIndicatorModel> call, Response<BeneficiaryIndicatorModel> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        BeneficiaryIndicatorModel beneficiaryWithrecords = (BeneficiaryIndicatorModel) response.body();
                        promise.resolve(beneficiaryWithrecords);
                    } else {
                        promise.reject("Internal Server Error");
                    }

                } else {
                    promise.reject("Internal Server Error");
                }
            }

            @Override
            public void onFailure(Call<BeneficiaryIndicatorModel> call, Throwable t) {
                promise.reject(t.getMessage());
            }
        });

        return promise;
    }

    @Override
    public Promise saveRecords(BeneficiaryPost beneficiaryPost) {
        Promise promise = new Promise();
        if (isNetworkAvailable()) {
            Call<Void> call = benificaryApi.uploadBeneficiaryCollectedRecords("Bearer" + " " + token, beneficiaryPost);
            call.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            promise.resolve(beneficiaryPost);
                        }
                        else{
                            promise.reject("Internal server error");
                        }
                    }
                    else{
                        promise.reject("Internal server error");
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    promise.reject(t.getMessage());
                }
            });
        } else {
            promise.reject("No Internet Connection");
        }

        return promise;
    }

    @Override
    public Promise changeActiveStatus(boolean activeStatus, Beneficiary beneficiary) {
        Promise promise = new Promise();
        List<Integer> beneficairyIds = Collections.singletonList(beneficiary.getEntityId());
        int instanceId = Singleton.getInstance().getIdInstance();
        DeactiveModel deactiveModel = new DeactiveModel(beneficairyIds, instanceId);
        if (isNetworkAvailable()) {
            Call<Void> call = benificaryApi.deactiveBeneficiary("Bearer" + " " + token, deactiveModel);
            call.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            promise.resolve(beneficiary);
                        }
                        else {
                            promise.reject("Internal server error");
                        }
                    }
                    else{
                        promise.reject("Internal server error");
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    promise.reject(t.getMessage());
                }
            });
        } else {
            promise.reject("NO internet connection");
        }

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
        Promise promise = new Promise();
        Call<Beneficiary> call = benificaryApi.getBeneficiaryGetById("Bearer " + token,  id, instanceId);

        call.enqueue(new Callback<Beneficiary>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<Beneficiary> call, Response<Beneficiary> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            Beneficiary beneficiary = response.body();
                            promise.resolve(beneficiary);

                        } catch (Exception e) {
                            e.printStackTrace();
                            promise.reject("Something went wrong");
                        }

                    } else {
                        promise.reject("Something went wrong");
                    }
                } else {
                    promise.reject("Something went wrong");

                }
            }


            @Override
            public void onFailure(Call<Beneficiary> call, Throwable t) {
                promise.reject("Something went wrong");
            }
        });
        return promise;
    }

    @Override
    public Promise createBeneficiary(Beneficiary beneficiary) {
        Promise promise = new Promise();
        promise.resolve("Online beneficiary create not allowed");
        return promise;
    }
}
