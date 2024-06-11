package com.unicef.mis.views;

import android.app.Activity;
import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.IntentSender;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.navigation.NavigationView;
import com.google.android.material.snackbar.Snackbar;
import com.google.android.play.core.appupdate.AppUpdateInfo;
import com.google.android.play.core.appupdate.AppUpdateManager;
import com.google.android.play.core.appupdate.AppUpdateManagerFactory;
import com.google.android.play.core.install.InstallStateUpdatedListener;
import com.google.android.play.core.install.model.AppUpdateType;
import com.google.android.play.core.install.model.InstallStatus;
import com.google.android.play.core.install.model.UpdateAvailability;
import com.google.android.play.core.tasks.OnSuccessListener;
import com.google.android.play.core.tasks.Task;
import com.unicef.mis.R;
import com.unicef.mis.adapter.NavigationDrawerListAdapter;
import com.unicef.mis.api.NotificationApi;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.model.Menu;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.SubMenu;
import com.unicef.mis.model.notification.NotificationModel;
import com.unicef.mis.service.MyBroadCastReceiver;
import com.unicef.mis.service.NetworkStateChangeReceiver;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.util.OfflineApiCalling;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.util.UnicefUncaughtExceptionHandler;
import com.unicef.mis.views.auth.LoginActivity;
import com.unicef.mis.views.home.HomeFragment;
import com.unicef.mis.views.notification.NotificationActivity;
import com.unicef.mis.views.offline_data.beneficiary.OfflineBeneficiaryInstanceListFragment;
import com.unicef.mis.views.offline_data.facility.OfflineFacilityInstanceListFragment;
import com.unicef.mis.views.upload.beneficiary.UploadBeneficiaryMain;
import com.unicef.mis.views.upload.facility.ScheduleFragment;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.app.ActionBarDrawerToggle;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.appcompat.widget.Toolbar;
import androidx.cardview.widget.CardView;
import androidx.core.view.GravityCompat;
import androidx.drawerlayout.widget.DrawerLayout;
import androidx.fragment.app.Fragment;
import androidx.fragment.app.FragmentManager;
import androidx.localbroadcastmanager.content.LocalBroadcastManager;

import android.text.Editable;
import android.text.TextWatcher;
import android.util.DisplayMetrics;
import android.view.View;
import android.view.MenuItem;
import android.widget.ExpandableListView;
import android.widget.FrameLayout;
import android.widget.RelativeLayout;
import android.widget.Toast;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.google.android.play.core.install.model.ActivityResult.RESULT_IN_APP_UPDATE_FAILED;
import static com.unicef.mis.constants.ApplicationConstants.MY_REQUEST_CODE;
import static com.unicef.mis.constants.ApplicationConstants.NETWORK_STATUS;
import static com.unicef.mis.constants.ApplicationConstants.TOKEN;
import static com.unicef.mis.service.NetworkStateChangeReceiver.IS_NETWORK_AVAILABLE;

public class MainActivity extends BaseActivity implements NavigationView.OnNavigationItemSelectedListener, ExpandableListView.OnGroupClickListener, ExpandableListView.OnChildClickListener,
        ExpandableListView.OnGroupExpandListener,ExpandableListView.OnGroupCollapseListener  {


    private ExpandableListView elvMyListView;
    private NavigationDrawerListAdapter adapter;
    private ArrayList<Menu> menus = null;

    private NavigationView navigationView;
    private DrawerLayout drawer;
    private FrameLayout fragmentContainer;

    private AppCompatTextView app_title, notification_tv;
    private RelativeLayout logout_layout, notification_relative;
    AlarmManager alarmManager;
    PendingIntent pendingIntent;
    private AppUpdateManager appUpdateManager;
    public NotificationApi notificationApi;
    public SharedPreferences sharedPreferences;
    public static String token = "";

    private Thread.UncaughtExceptionHandler defaultUncaughtExceptionHandler;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        defaultUncaughtExceptionHandler = Thread.getDefaultUncaughtExceptionHandler();
        Thread.setDefaultUncaughtExceptionHandler(new UnicefUncaughtExceptionHandler(defaultUncaughtExceptionHandler));

        Singleton.getInstance().setContext(getApplicationContext());

        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawer, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close);
        drawer.setDrawerListener(toggle);
        toggle.getDrawerArrowDrawable().setColor(getResources().getColor(R.color.green));
        toggle.syncState();

        navigationView = findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);

        alarmManager = (AlarmManager) getSystemService(Context.ALARM_SERVICE);

        Intent alarmIntent = new Intent(this, MyBroadCastReceiver.class);
        pendingIntent = PendingIntent.getBroadcast(this, 0, alarmIntent, 0);

        initViews();
        initListeners();
        initAppUpdate();
        initNetworkService();

        //call home fragment
        replaceFragment(new HomeFragment(), ApplicationConstants.HOME_FRAGMENT, fragmentContainer.getId());
    }

    private void initAppUpdate() {
        appUpdateManager = AppUpdateManagerFactory.create(UnicefApplication.getAppContext());
        appUpdateManager.registerListener(listener);

        Task<AppUpdateInfo> appUpdateInfoTask = appUpdateManager.getAppUpdateInfo();
        appUpdateInfoTask.addOnSuccessListener((OnSuccessListener<AppUpdateInfo>) appUpdateInfo -> {

            if (appUpdateInfo.updateAvailability() == UpdateAvailability.UPDATE_AVAILABLE
                    && appUpdateInfo.isUpdateTypeAllowed(AppUpdateType.IMMEDIATE)){
                requestUpdate(appUpdateInfo);
            }
            else if (appUpdateInfo.updateAvailability() == UpdateAvailability.DEVELOPER_TRIGGERED_UPDATE_IN_PROGRESS){

                notifyUser();
            }
            else
            {

            }
        });
    }

    private void initNetworkService() {
        IntentFilter intentFilter = new IntentFilter(NetworkStateChangeReceiver.NETWORK_AVAILABLE_ACTION);
        LocalBroadcastManager.getInstance(this).registerReceiver(new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                boolean isNetworkAvailable = intent.getBooleanExtra(IS_NETWORK_AVAILABLE, false);
                String networkStatus = isNetworkAvailable ? "connected" : "disconnected";

//                Snackbar.make(findViewById(R.id.activity_main), , Snackbar.LENGTH_LONG).show();
                if (isNetworkAvailable) {
                    IGenericApiCallBack iGenericApiCallBack = new IGenericApiCallBack() {
                        @Override
                        public void apiCallSuccessful(Object identifier, Object o) {

                        }

                        @Override
                        public void apiCallFailed(boolean hasSpecificError, String errorMessage) {

                        }
                    };
                    OfflineApiCalling.callFacilityScedule(iGenericApiCallBack);
                }


                showToast(getApplicationContext(), "Network Status: " + networkStatus);
            }
        }, intentFilter);
    }


    @Override
    public void onBackPressed() {
        FragmentManager fm = getSupportFragmentManager();
        if (fm.getBackStackEntryCount() >1) {
            fm.popBackStack();
        } else if (fm.getBackStackEntryCount() == 1) {
            dialogUtil.showDialogYesNo(getResources().getString(R.string.exit_msg)+" "+getResources().getString(R.string.app_name), (dialog, id) -> {

                        Intent intent = new Intent(Intent.ACTION_MAIN);
                        intent.addCategory(Intent.CATEGORY_HOME);
                        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                        startActivity(intent);
                        finish();

                    }, (dialog, id) -> dialog.dismiss()

            );
        }
        else {
            super.onBackPressed();

        }

    }

    public void initViews(){
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
        notificationApi = RetrofitService.createService(NotificationApi.class, APIClient.BASE_URL, true);

        fragmentContainer = findViewById(R.id.fragment_container);
        drawer = findViewById(R.id.drawer_layout);

        elvMyListView =  findViewById(R.id.dropdown_menu);
        menus = new ArrayList<>();
        menus = populateContinentData(menus);
        adapter = new NavigationDrawerListAdapter(this, menus);
        elvMyListView.setAdapter(adapter);
        elvMyListView.setOnGroupClickListener(this);
        elvMyListView.setOnChildClickListener(this);

        elvMyListView.setOnGroupExpandListener(this);
        elvMyListView.setOnGroupCollapseListener(this);

        app_title = findViewById (R.id.app_title);

        logout_layout = findViewById (R.id.logout_layout);
        notification_relative = findViewById( R.id.notification_relative);
        notification_tv = findViewById (R.id.notification_tv);

    }

    private void initListeners() {

        loadNotification();

        logout_layout.setOnClickListener(view -> {
            cancelService();
            if (isNetworkAvailable()){

                if (!isFinishing()) {
                    dialogUtil.showDialogYesNo(getResources().getString(R.string.logout_msg)+" "+getResources().getString(R.string.app_name), (dialog, id) -> {
                        
                        Intent intent = new Intent(UnicefApplication.getAppContext(), LoginActivity.class);
                        DataAccessFactory.getUserDataAccess().deleteProfile();
                        startActivity(intent);
                        finish();
                    }, (dialog, id) -> dialog.dismiss());
                } else {
                    showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.sww));
                }

            } else {
                showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.no_internet));

            }
        });

        notification_relative.setOnClickListener(v -> {
            Intent intent = new Intent(getApplicationContext(), NotificationActivity.class);
            startActivity(intent);
        });
    }


    @Override
    public boolean onNavigationItemSelected(@NonNull MenuItem item) {
        return false;
    }


    private ArrayList<Menu> populateContinentData(ArrayList<Menu> menus) {

        Menu MenuHome = new Menu(1, getResources().getString(R.string.home), null, R.drawable.home);
        menus.add(MenuHome);

        Menu MenuCollectFacility = new Menu(2, getResources().getString(R.string.collect_facility), null, R.drawable.facility);
        menus.add(MenuCollectFacility);

        Menu MenuCollectBenificiary = new Menu(3, getResources().getString(R.string.collect_benificiary), null, R.drawable.benificiary);
        menus.add(MenuCollectBenificiary);

        Menu Menu1 = new Menu(4, getResources().getString(R.string.offline_data), null, R.drawable.offline);
        ArrayList<SubMenu> offlineDataMenu = new ArrayList<>();
        offlineDataMenu.add(new SubMenu(getResources().getString(R.string.facility_data), 5));
        offlineDataMenu.add(new SubMenu(getResources().getString(R.string.benificiary_data), 6));

        Menu1.setSubMenu(offlineDataMenu);
        menus.add(Menu1);

        Menu Menu2 = new Menu(7, getResources().getString(R.string.upload), null, R.drawable.upload);
        ArrayList<SubMenu> offlineDataMenu2 = new ArrayList<>();
        offlineDataMenu2.add(new SubMenu(getResources().getString(R.string.facility_data), 8));
        offlineDataMenu2.add(new SubMenu(getResources().getString(R.string.benificiary_data), 9));

        Menu2.setSubMenu(offlineDataMenu2);
        menus.add(Menu2);


        return menus;
    }

    @Override
    public boolean onChildClick(ExpandableListView expandableListView, View view, int groupPosition, int childPosition, long l) {
        SubMenu c = adapter.getChild(groupPosition, childPosition);
        Fragment fragment;

        if (c.getId() == 5){
            ScheduledInstanceListFragment scheduledInstanceListFragment = getScheduledInstanceListFragment(EntityType.Facilitiy.getIntValue(), OperationMode.Offline.getIntValue());
            loadFragment(scheduledInstanceListFragment);
            app_title.setText(" Facility Schedule ");
            drawer.closeDrawer(GravityCompat.START);
        } else if (c.getId() ==6){
            fragment = new OfflineBeneficiaryInstanceListFragment();
            loadFragment(fragment);
            app_title.setText(" Beneficiary Schedule ");
            drawer.closeDrawer(GravityCompat.START);

        } else if(c.getId() == 8){
            fragment = new ScheduleFragment();
            loadFragment(fragment);
            app_title.setText(" Facility Schedule ");
            drawer.closeDrawer(GravityCompat.START);

        } else if(c.getId() == 9){
            fragment = new UploadBeneficiaryMain();
            loadFragment(fragment);

            app_title.setText(" Beneficiary Schedule ");
            drawer.closeDrawer(GravityCompat.START);
        }

        return false;
    }

    @Override
    public boolean onGroupClick(ExpandableListView expandableListView, View view, int groupPosition, long l) {
        Fragment fragment;
        if (adapter.getGroup(groupPosition).getId() == 1){
            fragment = new HomeFragment();
            loadFragment(fragment);
            startService();
            app_title.setText(getResources().getString(R.string.app_name));
            drawer.closeDrawer(GravityCompat.START);

        } else if (adapter.getGroup(groupPosition).getId() == 2){
            ScheduledInstanceListFragment scheduledInstanceListFragment = getScheduledInstanceListFragment(EntityType.Facilitiy.getIntValue(), OperationMode.Online.getIntValue());
            loadFragment(scheduledInstanceListFragment);
            app_title.setText(adapter.getGroup(groupPosition).getName());
            drawer.closeDrawer(GravityCompat.START);
        } else if (adapter.getGroup(groupPosition).getId() == 3){
            ScheduledInstanceListFragment scheduledInstanceListFragment = getScheduledInstanceListFragment(EntityType.Beneficiary.getIntValue(), OperationMode.Online.getIntValue());
            loadFragment(scheduledInstanceListFragment);
            Singleton.getInstance().setActionId("3");
            app_title.setText(adapter.getGroup(groupPosition).getName());
            drawer.closeDrawer(GravityCompat.START);
        }

        return false;
    }

    @Override
    public void onGroupExpand(int groupPosition) {
    }

    @Override
    public void onGroupCollapse(int groupPosition) {
    }

    public int GetPixelFromDips(float pixels) {
        // Get the screen's density scale
        final float scale = getResources().getDisplayMetrics().density;
        // Convert the dps to pixels, based on density scale
        return (int) (pixels * scale + 0.5f);
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus) {
        super.onWindowFocusChanged(hasFocus);
        DisplayMetrics metrics = new DisplayMetrics();
        getWindowManager().getDefaultDisplay().getMetrics(metrics);
        int width = metrics.widthPixels;
        elvMyListView.setIndicatorBounds(0,1);
        elvMyListView.setIndicatorBoundsRelative(width-GetPixelFromDips(35), width-GetPixelFromDips(5));
    }


    private void startService() {

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            alarmManager.setAndAllowWhileIdle(AlarmManager.RTC_WAKEUP, 0, pendingIntent);
        } else if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
            alarmManager.setExact(AlarmManager.RTC_WAKEUP, 0, pendingIntent);
        } else {
            alarmManager.set(AlarmManager.RTC_WAKEUP, 0, pendingIntent);
        }


    }

    private void cancelService() {
        alarmManager.cancel(pendingIntent);
//        Toast.makeText(getApplicationContext(), "Service Cancelled", Toast.LENGTH_LONG).show();
    }

    private void requestUpdate(AppUpdateInfo appUpdateInfo){
        try {
            appUpdateManager.startUpdateFlowForResult(appUpdateInfo,AppUpdateType.IMMEDIATE,MainActivity.this, MY_REQUEST_CODE);
            resume();
        } catch (IntentSender.SendIntentException e) {
            e.printStackTrace();
        }
    }

    private void notifyUser() {

        Snackbar snackbar =
                Snackbar.make(findViewById(R.id.message),
                        "An update has just been downloaded.",
                        Snackbar.LENGTH_INDEFINITE);
        snackbar.setAction("RESTART", new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                appUpdateManager.completeUpdate();
            }
        });
        snackbar.setActionTextColor(getResources().getColor(R.color.green));
        snackbar.show();
    }

    private void resume(){
        appUpdateManager.getAppUpdateInfo().addOnSuccessListener(appUpdateInfo -> {
            if (appUpdateInfo.installStatus() == InstallStatus.DOWNLOADED){
                notifyUser();
            }

        });
    }
    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == MY_REQUEST_CODE){
            switch (resultCode){
                case Activity.RESULT_OK:
                    if(resultCode != RESULT_OK){

                    }
                    break;
                case Activity.RESULT_CANCELED:

                    if (resultCode != RESULT_CANCELED){
                        Toast.makeText(this,"RESULT_CANCELED" +resultCode, Toast.LENGTH_LONG).show();
//                        Log.d("RESULT_CANCELED  :",""+resultCode);
                    }
                    break;
                case RESULT_IN_APP_UPDATE_FAILED:

                    if (resultCode != RESULT_IN_APP_UPDATE_FAILED){

                        Toast.makeText(this,"RESULT_IN_APP_UPDATE_FAILED" +resultCode, Toast.LENGTH_LONG).show();
                    }
            }
        }
    }

    InstallStateUpdatedListener listener = installState -> {
        if (installState.installStatus() == InstallStatus.DOWNLOADED){
            notifyUser();
        }
    };

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
                            assert notifications != null;
                            notification_tv.setText(String.valueOf(notifications.getTotal()));
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

}
