package com.unicef.mis.viewmodel;

import android.content.Context;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.R;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.repository.FacilityScheduleRepository;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.UnicefApplication;

import org.jetbrains.annotations.NotNull;

import java.util.List;

import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showToast;

public class FacilityScheduleViewModel extends ViewModel {
    public FacilityScheduleRepository facilityScheduleRepository;
    public MutableLiveData<List<ScheduledInstance>> scheduledInstanceList;

    public Context context;

    public FacilityScheduleViewModel() {
        context = UnicefApplication.getAppContext();
        facilityScheduleRepository = RepositoryFactory.getFacilityScheduleRepository();
        scheduledInstanceList = new MutableLiveData<>();
        loadSchedule();
    }

    private void loadSchedule() {
        if (isNetworkAvailable()){
            Promise promise = facilityScheduleRepository.callSchedule();
            promise.then(res -> scheduleList((Schedule) res))
                    .error(err -> {
                        showToast(context, UnicefApplication.getResourceString(R.string.username_password_not_match));
                    });

        } else {
            showToast(context, "Server Error");
        }
    }

    public MutableLiveData<List<ScheduledInstance>> getScheduledInstanceList() {
        return scheduledInstanceList;
    }

    private boolean scheduleList(@NotNull Schedule schedule) {
        scheduledInstanceList.setValue(schedule.getData());

        return true;
    }
}
