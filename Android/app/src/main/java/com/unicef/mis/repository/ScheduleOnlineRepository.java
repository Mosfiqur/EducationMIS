package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.interfaces.IScheduleRepository;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import org.json.JSONException;
import org.json.JSONObject;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class ScheduleOnlineRepository implements IScheduleRepository {
    public BeneficiaryApi benificaryApi;
    public SharedPreferences sharedPreferences;
    public static String token = "";

    public ScheduleOnlineRepository(Context appContext) {
        benificaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    @Override
    public Promise getSchedule(int entityType) {
        Promise promise = new Promise();
        Call<Schedule> getScheduleCall = benificaryApi.getSchedule("Bearer " + ScheduleOnlineRepository.token, String.valueOf(entityType));

        getScheduleCall.enqueue(new Callback<Schedule>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<Schedule> call, Response<Schedule> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {

                            Schedule schedule = response.body();
                            Singleton.getInstance().setScheduledInstances(schedule.getData());
                            promise.resolve(schedule);
                            for (int i = 0; i < response.body().getData().size(); i++) {
                                Singleton.getInstance().setIdInstance(response.body().getData().get(i).getId());
                            }


                        } catch (Exception e) {
                            e.printStackTrace();
                        }

                    } else {
                        promise.reject("Server Error");
                    }
                } else {
                    try {
                        JSONObject jsonErrorObject = new JSONObject(response.errorBody().string());
                        promise.reject(jsonErrorObject.getString("Message"));
                    } catch (JSONException jsonEx) {
                        promise.reject(response.raw().message());
                        jsonEx.printStackTrace();
                    } catch (Exception e) {
                        promise.reject(e.getMessage());
                        e.printStackTrace();
                    }
                }
            }


            @Override
            public void onFailure(Call<Schedule> call, Throwable t) {

                promise.reject(t.getMessage());
            }
        });
        return promise;
    }
}
