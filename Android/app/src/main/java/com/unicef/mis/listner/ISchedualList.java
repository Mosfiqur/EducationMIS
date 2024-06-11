package com.unicef.mis.listner;

import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;

import java.util.ArrayList;
import java.util.List;

public interface ISchedualList {
   // final ArrayList<ScheduledInstance> scheduledInstances = new ArrayList<>();

    void schedule(List<ScheduledInstance> scheduledInstances);

}
