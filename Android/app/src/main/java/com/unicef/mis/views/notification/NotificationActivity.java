package com.unicef.mis.views.notification;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.view.View;

import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.adapter.NotificationAdapter;
import com.unicef.mis.api.NotificationApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.databinding.ActivityNotificationBinding;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.notification.NotificationModel;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.NotificationViewModel;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class NotificationActivity extends BaseActivity implements View.OnClickListener {

    private ActivityNotificationBinding binding;
    private NotificationViewModel viewModel;
    private View view;

    //bangla code will be remove
    private RecyclerView notification_recyler;
    private NotificationAdapter notificationAdapter;
    private LinearLayoutManager linearLayoutManager;
    public SharedPreferences sharedPreferences;
    public static String token = "";
    public NotificationApi notificationApi;
    public AppCompatTextView tvRecordsCount, close_all;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_notification);
        viewModel = new ViewModelProvider(this).get(NotificationViewModel.class);
//        viewModel.setView(view);
//        viewModel.initViews();
        binding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getApplicationContext());

        initViews();
        initListeners();
    }

    public void goBack(View view){
        if (view.getId() == R.id.back){
            finish();
        }
    }

    public void initViews(){
        notificationApi = RetrofitService.createService(NotificationApi.class, APIClient.BASE_URL, true);
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
        notification_recyler = findViewById(R.id.notification_recyler);
        tvRecordsCount= findViewById (R.id.tvRecordsCount);
        close_all = findViewById (R.id.close_all);

    }

    public void initListeners(){
        loadNotification();
        close_all.setOnClickListener(this);
    }

    private void loadNotification() {
        Call<PagedResponse<NotificationModel>> call = notificationApi.getAllNotifications("Bearer "+token);

        call.enqueue(new Callback<PagedResponse<NotificationModel>>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<PagedResponse<NotificationModel>> call, Response<PagedResponse<NotificationModel>> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            PagedResponse<NotificationModel> notifications = (PagedResponse<NotificationModel>) response.body();

                            tvRecordsCount.setText("Total "+response.body().getTotal()+" Notifications ");
                            showNotification(notifications);
                        } catch (Exception e){
                            e.printStackTrace();
                        }

                    } else {
                        showToast(getApplicationContext(), "Server Error");
                    }
                }
            }


            @Override
            public void onFailure(Call<PagedResponse<NotificationModel>> call, Throwable t) {

                showToast(getApplicationContext(), t.getMessage());
            }
        });
    }

    private void showNotification(PagedResponse<NotificationModel> notifications) {
        notificationAdapter = new NotificationAdapter(notifications, getApplicationContext());
        linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
        notification_recyler.setAdapter(notificationAdapter);
        notification_recyler.setLayoutManager(linearLayoutManager);
        notification_recyler.setNestedScrollingEnabled(false);
    }


    @Override
    public void onClick(View v) {
        switch (v.getId()){
            case R.id.close_all:
                clearAll();
        }
    }

    private void clearAll() {
        Call<Void> call = notificationApi.clearNotification("Bearer "+token);

        call.enqueue(new Callback<Void>() {
            @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
            @Override
            public void onResponse(Call<Void> call, Response<Void> response) {

                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        try {
                            finish();
                            overridePendingTransition( 0, 0);
                            startActivity(getIntent());
                            overridePendingTransition( 0, 0);
                        } catch (Exception e){
                            e.printStackTrace();
                        }

                    } else {
                        showToast(getApplicationContext(), "Server Error");
                    }
                }
            }


            @Override
            public void onFailure(Call<Void> call, Throwable t) {

                showToast(getApplicationContext(), t.getMessage());
            }
        });
    }
}
