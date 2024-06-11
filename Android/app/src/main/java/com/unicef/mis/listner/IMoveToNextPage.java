package com.unicef.mis.listner;

import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;

public interface IMoveToNextPage {
    void moveToNextPage(int id, String scheduleName, String scheduleDate, String endDate, ScheduledInstance instance);
}
