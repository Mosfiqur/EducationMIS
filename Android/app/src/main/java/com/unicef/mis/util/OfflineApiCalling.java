package com.unicef.mis.util;

import android.content.Context;
import android.content.SharedPreferences;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Build;
import android.util.Log;
import android.widget.Toast;

import androidx.annotation.RequiresApi;

import com.unicef.mis.R;
import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.api.FacilityApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.listner.ISchedualList;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.SubBlock;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;
import static com.unicef.mis.util.BaseActivity.showToast;

public class OfflineApiCalling {
    public static BeneficiaryApi beneficiaryApi;
    public static FacilityApi facilityApi;
    public static String token = "";

    public static SQLiteDatabaseHelper db;

    public static SharedPreferences sharedPreferences;

    public static int i = 0, j = 0, k = 0;

    public static ISchedualList listener = scheduledInstances -> {

    };


    public static void token() {
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    //method for network available or not checking
    public static boolean isNetworkAvailable(Context context) {

        ConnectivityManager connectivityManager
                = (ConnectivityManager) context.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnectedOrConnecting();

    }

    public static void callBenificaryScedule(IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
            Call<Schedule> getScheduleCall = beneficiaryApi.getSchedule("Bearer " + token, String.valueOf(EntityType.Beneficiary.getIntValue()));

            getScheduleCall.enqueue(new Callback<Schedule>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Schedule> call, Response<Schedule> response) {

                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            try {
                                Log.d("hasan_test", "get Schedule data");
                                Schedule schedule = (Schedule) response.body();
                                callBack.apiCallSuccessful(null, schedule);

                            } catch (Exception e) {
                                e.printStackTrace();
                                callBack.apiCallFailed(true, e.getMessage());
                            }

                        } else {
                            Toast.makeText(UnicefApplication.getAppContext(), "Something Went Wrong", Toast.LENGTH_SHORT).show();
                            callBack.apiCallFailed(true, "Something Went Wrong");
                        }
                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                }


                @Override
                public void onFailure(Call<Schedule> call, Throwable t) {

                    Toast.makeText(UnicefApplication.getAppContext(), "failed To Load Data", Toast.LENGTH_SHORT).show();
                }
            });
        } else {
            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.no_internet));
        }

    }

    public static void callFacilityScedule(IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {

            facilityApi = RetrofitService.createService(FacilityApi.class, APIClient.BASE_URL, true);
            Call<Schedule> getScheduleCall = facilityApi.getSchedule("Bearer " + token, String.valueOf(EntityType.Facilitiy.getIntValue()));

            getScheduleCall.enqueue(new Callback<Schedule>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Schedule> call, Response<Schedule> response) {

                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            Log.d("hasan_test", "get Schedule data");
                            Schedule schedule = (Schedule) response.body();
                            callBack.apiCallSuccessful(null, schedule);

                        } else {
                            Toast.makeText(UnicefApplication.getAppContext(), "Something Went Wrong", Toast.LENGTH_SHORT).show();

                        }
                    }
                }


                @Override
                public void onFailure(Call<Schedule> call, Throwable t) {

                    Toast.makeText(UnicefApplication.getAppContext(), "failed To Load Data", Toast.LENGTH_SHORT).show();
                }
            });
        } else {
            Toast.makeText(UnicefApplication.getAppContext(), "No Internet Connection", Toast.LENGTH_SHORT).show();
        }

    }

    public static void getAllFacility(int instanceId, IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<FacilityListDatum>> call = facilityApi.getFacilityList("Bearer" + " " + token, instanceId);

            call.enqueue(new Callback<PagedResponse<FacilityListDatum>>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<PagedResponse<FacilityListDatum>> call, Response<PagedResponse<FacilityListDatum>> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {

                            Log.d("hasan_test", "get Schedule data");
                            PagedResponse<FacilityListDatum> facilityList = response.body();
                            callBack.apiCallSuccessful(null, facilityList);

                        } else {
                            Toast.makeText(UnicefApplication.getAppContext(), "Something Went Wrong", Toast.LENGTH_SHORT).show();
                        }
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<FacilityListDatum>> call, Throwable t) {
                    Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                    System.out.println(t.getMessage());
                }
            });
        } else {
            Toast.makeText(UnicefApplication.getAppContext(), "No Internet Connection", Toast.LENGTH_SHORT).show();
        }
    }

    public static void getAllFacilityForBeneficiaries(int instanceId, IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<FacilityListDatum>> call = beneficiaryApi.getFacilityList("Bearer" + " " + token, instanceId);

            call.enqueue(new Callback<PagedResponse<FacilityListDatum>>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<PagedResponse<FacilityListDatum>> call, Response<PagedResponse<FacilityListDatum>> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {

                            Log.d("hasan_test", "get Schedule data");
                            PagedResponse<FacilityListDatum> facilityList = response.body();
                            callBack.apiCallSuccessful(null, facilityList);

                        } else {
                            Toast.makeText(UnicefApplication.getAppContext(), "Something Went Wrong", Toast.LENGTH_SHORT).show();
                        }
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<FacilityListDatum>> call, Throwable t) {
                    Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                    System.out.println(t.getMessage());
                }
            });
        } else {
            Toast.makeText(UnicefApplication.getAppContext(), "No Internet Connection", Toast.LENGTH_SHORT).show();
        }
    }

    public static void callFacilityIndicator(int instanceId, IGenericApiCallBack callBack) {
        token();

        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            facilityApi = RetrofitService.createService(FacilityApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<IndicatorNew>> call = facilityApi.getFacilityIndicatorByInstace("Bearer" + " " + token, instanceId);

            call.enqueue(new Callback<PagedResponse<IndicatorNew>>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<PagedResponse<IndicatorNew>> call, Response<PagedResponse<IndicatorNew>> response) {
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            PagedResponse<IndicatorNew> responseBody = (PagedResponse<IndicatorNew>) response.body();
                            callBack.apiCallSuccessful(null, responseBody);
                        } else {
                            callBack.apiCallFailed(true, "An error occured");
                        }
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<IndicatorNew>> call, Throwable t) {

                    Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                    System.out.println(t.getMessage());
                    callBack.apiCallFailed(true, t.getMessage());
                }
            });
        } else {
            callBack.apiCallFailed(true, "An error occured");
        }
    }

    public static void callBenificariesWithRecords(int instanceId, int facilityId, QueryParamModel queryParam, IGenericApiCallBack callBack) {
        token();
        Call<PagedResponse<Beneficiary>> getScheduleCall = beneficiaryApi.getBeneficiariesByFacility("Bearer" + " " + token, facilityId, instanceId, queryParam.getPageSize(), queryParam.getPageNumber(), queryParam.getSearchText());

        getScheduleCall.enqueue(new Callback<PagedResponse<Beneficiary>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<Beneficiary>> call, Response<PagedResponse<Beneficiary>> response) {
                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        PagedResponse<Beneficiary> responseBody = (PagedResponse<Beneficiary>) response.body();
                        callBack.apiCallSuccessful(null, responseBody);
                    } else {
                        callBack.apiCallFailed(true, "An error occured");
                    }
                } else {
                    callBack.apiCallFailed(true, "An error occured");
                }
            }

            @Override
            public void onFailure(Call<PagedResponse<Beneficiary>> call, Throwable t) {
                Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                System.out.println(t.getMessage());
                callBack.apiCallFailed(true, t.getMessage());
            }
        });
    }


    public static void callCamp(IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<Camp>> call = beneficiaryApi.getCamps("Bearer " + token);

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
                                callBack.apiCallSuccessful(null, resp);

                            } catch (Exception e) {
                                e.printStackTrace();
                                callBack.apiCallFailed(true, e.getMessage());
                            }

                        } else {
                            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                            callBack.apiCallFailed(true, "Something Went Wrong");
                        }
                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<Camp>> call, Throwable t) {

                    showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                }
            });

        } else {
            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.no_internet));
        }

    }

    public static void callBlock(IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<Block>> getBlock = beneficiaryApi.getBlock("Bearer " + token);

            getBlock.enqueue(new Callback<PagedResponse<Block>>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<PagedResponse<Block>> call, Response<PagedResponse<Block>> response) {

                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            try {
                                //need to work here
                                Log.d("hasan_test", "get Block data");
                                PagedResponse<Block> resp = response.body();
                                callBack.apiCallSuccessful(null, resp);

                            } catch (Exception e) {
                                e.printStackTrace();
                                callBack.apiCallFailed(true, e.getMessage());
                            }

                        } else {
                            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                            callBack.apiCallFailed(true, "Something Went Wrong");
                        }
                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<Block>> call, Throwable t) {

                    showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                }
            });
        } else {
            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.no_internet));
        }
    }

    public static void callSubBlock(IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            beneficiaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<SubBlock>> call = beneficiaryApi.getSubBlocks("Bearer " + token);

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
                                callBack.apiCallSuccessful(null, resp);

                            } catch (Exception e) {
                                e.printStackTrace();
                                callBack.apiCallFailed(true, e.getMessage());
                            }

                        } else {
                            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                            callBack.apiCallFailed(true, "Something Went Wrong");
                        }
                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<SubBlock>> call, Throwable t) {

                    showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                }
            });
        } else {
            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.no_internet));
        }
    }

    public static void getFacilityRecords(int instanceId, IGenericApiCallBack callBack) {
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        if (isNetworkAvailable(UnicefApplication.getAppContext())) {
            facilityApi = RetrofitService.createService(FacilityApi.class, APIClient.BASE_URL, true);
            Call<PagedResponse<FacilityListDatum>> call = facilityApi.getFacilityRecords("Bearer " + token, instanceId, Integer.MAX_VALUE, UIConstants.DEFAULT_PAGE_NUMBER, null);

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
                                callBack.apiCallSuccessful(null, resp);

                            } catch (Exception e) {
                                e.printStackTrace();
                                callBack.apiCallFailed(true, e.getMessage());
                            }

                        } else {
                            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                            callBack.apiCallFailed(true, "Something Went Wrong");
                        }
                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                }


                @Override
                public void onFailure(Call<PagedResponse<FacilityListDatum>> call, Throwable t) {

                    showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                }
            });
        } else {
            showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.no_internet));
        }
    }

    public static void getFacilityById(int facilityId, int instanceId, IGenericApiCallBack callBack){
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        Call<FacilityListDatum> getScheduleCall = facilityApi.getFacilityGetById("Bearer " + token, facilityId, instanceId);

        getScheduleCall.enqueue(new Callback<FacilityListDatum>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<FacilityListDatum> call, Response<FacilityListDatum> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            FacilityListDatum result = response.body();
                            callBack.apiCallSuccessful(null, result);
                        } catch (Exception e) {
                            e.printStackTrace();
                            callBack.apiCallFailed(true, e.getMessage());
                        }

                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                } else {
                    callBack.apiCallFailed(true, "Something Went Wrong");
                }
            }


            @Override
            public void onFailure(Call<FacilityListDatum> call, Throwable t) {
                showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
            }
        });

    }

    public static void getBeneficiaryById(int id, int instanceId, IGenericApiCallBack callBack){
        token();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        Call<Beneficiary> getScheduleCall = beneficiaryApi.getBeneficiaryGetById("Bearer " + token,  id, instanceId);

        getScheduleCall.enqueue(new Callback<Beneficiary>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<Beneficiary> call, Response<Beneficiary> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            Beneficiary result = response.body();
                            callBack.apiCallSuccessful(null, result);

                        } catch (Exception e) {
                            e.printStackTrace();
                            callBack.apiCallFailed(true, e.getMessage());
                        }

                    } else {
                        callBack.apiCallFailed(true, "Something Went Wrong");
                    }
                } else {
                    callBack.apiCallFailed(true, "Something Went Wrong");
                }
            }


            @Override
            public void onFailure(Call<Beneficiary> call, Throwable t) {
                showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
            }
        });
    }
}
