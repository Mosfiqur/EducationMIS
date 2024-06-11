package com.unicef.mis.repository;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;

import androidx.annotation.RequiresApi;

import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.api.NotificationApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.notification.NotificationModel;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class NotificationRepository {

    public SharedPreferences sharedPreferences;
    public static String token = "";
    public NotificationApi notificationApi;

    public NotificationRepository(Context appContext) {
        notificationApi = RetrofitService.createService(NotificationApi.class, APIClient.BASE_URL, true);
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    public Promise getNotification(){
        Promise promise = new Promise();

        Call<PagedResponse<NotificationModel>> call = notificationApi.getAllNotifications("Bearer "+token);

        call.enqueue(new Callback<PagedResponse<NotificationModel>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<NotificationModel>> call, Response<PagedResponse<NotificationModel>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            PagedResponse<NotificationModel> notifications = (PagedResponse<NotificationModel>) response.body();

                        } catch (Exception e){
                            e.printStackTrace();
                        }

                    } else {
                        promise.reject("Server Error");
                    }
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<NotificationModel>> call, Throwable t) {

                promise.reject(t.getMessage());
            }
        });

        return promise;
    }
}
