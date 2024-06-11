package com.unicef.mis.service;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;

import com.unicef.mis.util.UnicefApplication;

import static com.unicef.mis.util.BaseActivity.showToast;

public class MyBroadCastReceiver extends BroadcastReceiver {
    @Override
    public void onReceive(Context context, Intent intent) {
        if("android.intent.action.BOOT_COMPLETED".equals(intent.getAction())) {

            Intent serviceIntent = new Intent(UnicefApplication.getAppContext(), MyService.class);
            context.startService(serviceIntent);
        } else {
//            showToast(UnicefApplication.getAppContext(), "Alarm Manager just ran");
        }

    }
}
