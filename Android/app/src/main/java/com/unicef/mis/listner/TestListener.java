package com.unicef.mis.listner;

import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;

import java.util.ArrayList;

public interface TestListener {
    public void success(ArrayList<ScheduledInstance> scheduledInstances);
    public void failed(String message);
}
