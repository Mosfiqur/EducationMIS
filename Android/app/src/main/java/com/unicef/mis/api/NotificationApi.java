package com.unicef.mis.api;

import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.notification.NotificationModel;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Query;

import static com.unicef.mis.constants.APIConstants.CLEAR_NOTIFICATION;
import static com.unicef.mis.constants.APIConstants.GET_ALL_NOTIFICATIONS;
import static com.unicef.mis.constants.APIConstants.READ_NOTIFICATION;

public interface NotificationApi {
    @GET(GET_ALL_NOTIFICATIONS)
    Call<PagedResponse<NotificationModel>> getAllNotifications(
            @Header("Authorization") String contentRange
    );

    @POST(READ_NOTIFICATION)
    Call<Void> readNotification(
            @Header("Authorization") String contentRange,
            @Query("notificationId") String notificationId
    );

    @POST(CLEAR_NOTIFICATION)
    Call<Void> clearNotification(
            @Header("Authorization") String contentRange
    );
}
