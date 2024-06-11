package com.unicef.mis.viewmodel;

import android.content.Context;

import androidx.databinding.ObservableBoolean;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IScheduleRepository;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.UnicefApplication;

import org.jetbrains.annotations.NotNull;

import java.util.List;

import static com.unicef.mis.util.BaseActivity.hideWait;
import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class ScheduleViewModel extends ViewModel{
    public IScheduleRepository repository;

    private int operationMode;
    private int entityType;

    public MutableLiveData<List<ScheduledInstance>> scheduledInstanceList;
    public ObservableBoolean hasRecord = new ObservableBoolean(true);
    public MutableLiveData<String> generalMsg = new MutableLiveData<String>("Some generic msg");

    public Context context;

    public ScheduleViewModel() {
        context = UnicefApplication.getAppContext();
        scheduledInstanceList = new MutableLiveData<>();
        generalMsg.setValue("Vvvvv VVVV WWW");
    }

    public void loadSchedule(){
        Promise promise = repository.getSchedule(this.entityType);
        promise.then(res -> setData((Schedule) res))
                .error(err -> {
                    showToast(context, err.toString());
                });
    }


    public MutableLiveData<List<ScheduledInstance>> getScheduledInstanceList() {
        return scheduledInstanceList;
    }



    public boolean setData(@NotNull Schedule schedule) {
        hasRecord.set(schedule.getTotal() > 0);
        scheduledInstanceList.setValue(schedule.getData());
        return true;
    }

    public void prepare(int operationMode, int entityType) {
        repository = RepositoryFactory.getScheduleRepository(operationMode);
        this.operationMode = operationMode;
        this.entityType = entityType;
    }
}
