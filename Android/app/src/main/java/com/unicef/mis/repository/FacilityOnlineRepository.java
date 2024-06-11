package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.RequiresApi;

import com.unicef.mis.R;
import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.api.FacilityApi;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.interfaces.IFacilityRepository;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.SubBlock;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.facility.indicator.FacilityIndicatorModel;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.OfflineApiCalling;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.UnicefApplication;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;
import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showToast;

public class FacilityOnlineRepository implements IFacilityRepository {

    public BeneficiaryApi beneficiaryApi;
    public FacilityApi facilityApi;
    public SharedPreferences sharedPreferences;
    public static String token ="";
    public int page =1, limit = 10;

    public FacilityOnlineRepository(Context appContext) {
        beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
        facilityApi = RetrofitService.createService(FacilityApi.class, APIClient.BASE_URL, true);
        sharedPreferences= UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

//    public Promise getFacility(int instanceId){
//        Promise promise = new Promise();
//
//        Call<PagedResponse<FacilityListDatum>> call = beneficiaryApi.getFacilityList ("Bearer"+" "+ token, instanceId);
//        call.enqueue(new Callback<PagedResponse<FacilityListDatum>>() {
//            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
//            @Override
//            public void onResponse(Call<PagedResponse<FacilityListDatum>> call, Response<PagedResponse<FacilityListDatum>> response) {
//                if (response.isSuccessful()) {
//                    if (response.code() == 200) {
//                        PagedResponse<FacilityListDatum> facilityList = response.body();
//                        promise.resolve(facilityList);
//
//                    } else {
//                        promise.reject("Internal Server Error");
//                    }
//                }
//            }
//
//
//            @Override
//            public void onFailure(Call<PagedResponse<FacilityListDatum>> call, Throwable t) {
//                promise.reject(t.getMessage());
//            }
//        });
//
//        return promise;
//
//    }
//
//    public Promise getFacilitySchedule(){
//        Promise promise = new Promise();
//        Call<Schedule> getScheduleCall = facilityApi.getSchedule("Bearer " + token, String.valueOf(EntityType.Facilitiy.getIntValue()));
//
//        getScheduleCall.enqueue(new Callback<Schedule>() {
//            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
//            @Override
//            public void onResponse(Call<Schedule> call, Response<Schedule> response) {
//
//                if (response.isSuccessful()) {
//                    if (response.code() == 200) {
//                        Log.d("hasan_test", "get Schedule data");
//                        Schedule schedule = (Schedule) response.body();
//                        promise.resolve(schedule);
//
//                    } else {
//                        promise.reject("Server Error");
//
//                    }
//                }
//            }
//
//
//            @Override
//            public void onFailure(Call<Schedule> call, Throwable t) {
//
//                promise.reject("Server Error");
//            }
//        });
//        return promise;
//    }
//
//    public Promise getBeneficiarySchedule(){
//        Promise promise = new Promise();
//        Call<Schedule> call = OfflineApiCalling.beneficiaryApi.getSchedule("Bearer " + token, String.valueOf(EntityType.Beneficiary.getIntValue()));
//
//        call.enqueue(new Callback<Schedule>() {
//            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
//            @Override
//            public void onResponse(Call<Schedule> call, Response<Schedule> response) {
//
//                if (response.isSuccessful()) {
//                    if (response.code() == 200) {
//                        try {
//                            Log.d("hasan_test", "get Schedule data");
//                            Schedule schedule = (Schedule) response.body();
//                            promise.resolve(schedule);
//
//
//                        } catch (Exception e) {
//                            e.printStackTrace();
//                            promise.reject("Server Error");
//                        }
//
//                    } else {
//                        promise.reject("Server Error");
//                    }
//                } else {
//                    promise.reject("Server Error");
//                }
//            }
//
//
//            @Override
//            public void onFailure(Call<Schedule> call, Throwable t) {
//
//                Toast.makeText(UnicefApplication.getAppContext(), "failed To Load Data", Toast.LENGTH_SHORT).show();
//            }
//        });
//        return promise;
//    }
//
//    public Promise getFacilityIndicator(int instanceId){
//        Promise promise = new Promise();
//        Call<PagedResponse<IndicatorNew>> call = facilityApi.getFacilityIndicatorByInstace("Bearer" + " " + token, instanceId);
//
//        call.enqueue(new Callback<PagedResponse<IndicatorNew>>() {
//            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
//            @Override
//            public void onResponse(Call<PagedResponse<IndicatorNew>> call, Response<PagedResponse<IndicatorNew>> response) {
//
//                if (response.isSuccessful()) {
//                    if (response.code() == 200) {
//                        PagedResponse<IndicatorNew> responseBody = (PagedResponse<IndicatorNew>) response.body();
//                        promise.resolve(responseBody);
//
//                    } else {
//                        promise.reject("Server Error");
//                    }
//
//                }
//            }
//
//
//            @Override
//            public void onFailure(Call<PagedResponse<IndicatorNew>> call, Throwable t) {
//                promise.reject("Server Error");
//            }
//        });
//
//        return promise;
//    }

    public Promise getCamp(){

        Promise promise = new Promise();

        Call<PagedResponse<Camp>> call = OfflineApiCalling.beneficiaryApi.getCamps("Bearer " + token);

        call.enqueue(new Callback<PagedResponse<Camp>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<Camp>> call, Response<PagedResponse<Camp>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            //need to work here
                            Log.d("hasan_test", "get Camp data");
                            PagedResponse<Camp> resp = response.body();
                            promise.resolve(resp);


                        } catch (Exception e) {
                            e.printStackTrace();
                            promise.reject("Server Error");
                        }

                    } else {
                        promise.reject("Server Error");
                    }
                } else {
                    promise.reject("Server Error");
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<Camp>> call, Throwable t) {

                showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
            }
        });

        return promise;

    }

    public Promise getBlock(){
        Promise promise = new Promise();
        Call<PagedResponse<Block>> call = OfflineApiCalling.beneficiaryApi.getBlock("Bearer " + token);

        call.enqueue(new Callback<PagedResponse<Block>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<Block>> call, Response<PagedResponse<Block>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            //need to work here
                            Log.d("hasan_test", "get Block data");
                            PagedResponse<Block> resp = response.body();
                            promise.resolve(resp);

                        } catch (Exception e) {
                            e.printStackTrace();
                            promise.reject("Server Error");
                        }

                    } else {
                        promise.reject("Server Error");
                    }
                } else {
                    promise.reject("Server Error");
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<Block>> call, Throwable t) {

                showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
            }
        });
        return promise;
    }

    public Promise getSubBlock(){
        Promise promise = new Promise();
        Call<PagedResponse<SubBlock>> call = OfflineApiCalling.beneficiaryApi.getSubBlocks("Bearer " + token);

        call.enqueue(new Callback<PagedResponse<SubBlock>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<SubBlock>> call, Response<PagedResponse<SubBlock>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            //need to work here
                            Log.d("hasan_test", "get subBlock data");
                            PagedResponse<SubBlock> resp = response.body();
                            promise.resolve(resp);

                        } catch (Exception e) {
                            e.printStackTrace();
                            promise.reject("Server Error");
                        }

                    } else {
                        promise.reject("Server Error");
                    }
                } else {
                    promise.reject("Server Error");
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<SubBlock>> call, Throwable t) {
                promise.reject("Server Error");
            }
        });
        return promise;
    }

    public Promise getFacilityRecords(int instanceId, QueryParamModel queryParam){
        Promise promise = new Promise();
        Call<PagedResponse<FacilityListDatum>> call = facilityApi.getFacilityRecords("Bearer " + token, instanceId, queryParam.getPageSize(), queryParam.getPageNumber(), queryParam.getSearchText());

        call.enqueue(new Callback<PagedResponse<FacilityListDatum>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<FacilityListDatum>> call, Response<PagedResponse<FacilityListDatum>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            //need to work here
                            Log.d("hasan_test", "get subBlock data");
                            PagedResponse<FacilityListDatum> resp = response.body();
                            promise.resolve(resp);
                        } catch (Exception e) {
                            e.printStackTrace();
                            promise.reject("Server Error");
                        }

                    } else {
                        promise.reject("Server Error");
                    }
                } else {
                    promise.reject("Server Error");
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<FacilityListDatum>> call, Throwable t) {
                promise.reject("Server Error");
            }
        });
        return promise;
    }

    @Override
    public Promise searchFacilitiesForBeneficiary(int instanceId, QueryParamModel queryParam) {
        Promise promise = new Promise();
        Call<PagedResponse<FacilityListDatum>> call = facilityApi.getFacilityPaginatedList("Bearer" + " " + token, instanceId, queryParam.getPageNumber(), queryParam.getPageSize(), queryParam.getSearchText());
        call.enqueue(new Callback<PagedResponse<FacilityListDatum>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<FacilityListDatum>> call, Response<PagedResponse<FacilityListDatum>> response) {
                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        if (response.isSuccessful() && response.body() != null) {
                            PagedResponse<FacilityListDatum> facilityList = response.body();
                            promise.resolve(facilityList);
                        } else {
                            promise.reject("No records found");
                        }
                    } else {
                        promise.reject("No records found");
                    }
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<FacilityListDatum>> call, Throwable t) {
                promise.reject(t);
            }
        });
        return promise;
    }

    @Override
    public Promise searchFacilitiesByInstance(int instanceId, QueryParamModel queryParam) {
        return getFacilityRecords(instanceId, queryParam);
    }

    @Override
    public Promise getRecords(int instanceId, int facilityId) {
        Promise promise = new Promise();
        if (isNetworkAvailable()) {
            Call<FacilityIndicatorModel> getScheduleCall = facilityApi.getFacilityIndicator("Bearer" + " " + token, instanceId, facilityId);

            getScheduleCall.enqueue(new Callback<FacilityIndicatorModel>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<FacilityIndicatorModel> call, Response<FacilityIndicatorModel> response) {

                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            FacilityIndicatorModel facilityWithRecords = (FacilityIndicatorModel)response.body();
                            promise.resolve(facilityWithRecords);
                        } else {
                            promise.reject("Internal server error");
                        }
                    }
                }

                @Override
                public void onFailure(Call<FacilityIndicatorModel> call, Throwable t) {
                    promise.reject(t.getMessage());
                }
            });
        } else {
            promise.reject("No internet connection");
        }
        return promise;
    }

    @Override
    public Promise saveRecords(FacilityPost records) {
        Promise promise = new Promise();

        if (isNetworkAvailable()) {
            Call<Void> call = facilityApi.uploadFacilityCollectedRecords("Bearer" + " " + token, records);
            call.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            promise.resolve(records);
                        }
                        else {
                            promise.reject("Failed to save facility records online");
                        }
                    }
                    else{
                        promise.reject("Failed to save facility records online");
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
    public Promise uploadFacilities(int instanceId) {
        Promise promise = new Promise();
        promise.resolve("Nothing to implement");
        return promise;
    }

    @Override
    public Promise facilityGetById(int id, int instanceId) {
        Promise promise = new Promise();

        Call<FacilityListDatum> call = facilityApi.getFacilityGetById("Bearer " + token, id, instanceId);

        call.enqueue(new Callback<FacilityListDatum>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<FacilityListDatum> call, Response<FacilityListDatum> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            FacilityListDatum facility = response.body();
                            promise.resolve(facility);

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
            public void onFailure(Call<FacilityListDatum> call, Throwable t) {
                promise.reject("Something went wrong");
            }
        });
        return promise;
    }
}
