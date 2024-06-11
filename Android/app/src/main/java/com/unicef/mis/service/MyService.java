package com.unicef.mis.service;

import android.app.AlarmManager;
import android.app.IntentService;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;


import androidx.annotation.Nullable;

import java.util.Calendar;

public class MyService extends IntentService {
    public MyService() {
        super("MyService");
    }

    @Override
    protected void onHandleIntent(@Nullable Intent intent) {

//        AlarmManager alarmManager = (AlarmManager) getSystemService(Context.ALARM_SERVICE);
//
//        Intent alarmIntent = new Intent(this, MyBroadCastReceiver.class);
//        PendingIntent pendingIntent = PendingIntent.getBroadcast(this, 0, alarmIntent, 0);
////        alarmManager.setInexactRepeating(AlarmManager.RTC_WAKEUP, 0, 10000, pendingIntent);
//        alarmManager.setRepeating(AlarmManager.RTC_WAKEUP, 0, 10000, pendingIntent);

        intent = new Intent(this, MyBroadCastReceiver.class);
        intent.putExtra("key", "Alert");
        PendingIntent pendingIntent = PendingIntent.getBroadcast(this.getApplicationContext(), 0, intent, 0);

        AlarmManager alarmManager = (AlarmManager) getSystemService(ALARM_SERVICE);
        Calendar calendar=Calendar.getInstance();

        // Calendar.set(int year, int month, int day, int hourOfDay, int minute, int second)
        calendar.set(2013, Calendar.OCTOBER, 13, 18, 55, 40);
        alarmManager.setRepeating(AlarmManager.RTC_WAKEUP, calendar.getTimeInMillis(), 1000, pendingIntent);
    }
}
