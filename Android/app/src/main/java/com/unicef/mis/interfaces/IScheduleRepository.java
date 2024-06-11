package com.unicef.mis.interfaces;

import android.os.Build;

import androidx.annotation.RequiresApi;

import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.repository.ScheduleOnlineRepository;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.Singleton;

import org.json.JSONException;
import org.json.JSONObject;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public interface IScheduleRepository {
    Promise getSchedule(int entityType);
}
