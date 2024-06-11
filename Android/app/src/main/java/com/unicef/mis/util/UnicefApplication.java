package com.unicef.mis.util;

import android.app.Application;
import android.content.Context;
import android.content.IntentFilter;
import android.content.res.Resources;

import com.unicef.mis.service.NetworkStateChangeReceiver;

import static android.net.ConnectivityManager.CONNECTIVITY_ACTION;

public class UnicefApplication extends Application {
    private static final String WIFI_STATE_CHANGE_ACTION = "android.net.wifi.WIFI_STATE_CHANGED";

    private static Context context;

    public void onCreate() {
        super.onCreate();

        UnicefApplication.context = getApplicationContext();
        registerForNetworkChangeEvents(this);
    }

    public static Context getAppContext() {
        return UnicefApplication.context;
    }

    public static String getResourceString(int resourceId){
        Context appContext = getAppContext();
        if(appContext == null) return null;
        Resources resources = appContext.getResources();
        return resources.getString(resourceId);
    }

    public static void registerForNetworkChangeEvents(final Context context) {
        NetworkStateChangeReceiver networkStateChangeReceiver = new NetworkStateChangeReceiver();
        context.registerReceiver(networkStateChangeReceiver, new IntentFilter(CONNECTIVITY_ACTION));
        context.registerReceiver(networkStateChangeReceiver, new IntentFilter(WIFI_STATE_CHANGE_ACTION));
    }
}
