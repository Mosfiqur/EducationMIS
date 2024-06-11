package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.unicef.mis.api.FacilityApi;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class FacilityScheduleRepository {
    public FacilityApi facilityApi;
    public SharedPreferences sharedPreferences;
    public static String token ="";

    public FacilityScheduleRepository(Context appContext) {
        facilityApi = RetrofitService.createService(FacilityApi.class, APIClient.BASE_URL, true);
        sharedPreferences= UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    public Promise callSchedule(){
        Promise promise = new Promise();
        Call<Schedule> getScheduleCall = facilityApi.getSchedule("Bearer "+token, EntityType.Facilitiy.toString());

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
                            for (int i =0; i<response.body().getData().size(); i++){
                                Singleton.getInstance().setIdInstance(response.body().getData().get(i).getId());
                            }


                        } catch (Exception e){
                            e.printStackTrace();
                        }

                    } else {
                        promise.reject("Server Error");
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
