package com.unicef.mis.model.facility;

public class FacilityMainModel {
    String shedule, month;

    public FacilityMainModel(String shedule, String month) {
        this.shedule = shedule;
        this.month = month;
    }

    public String getShedule() {
        return shedule;
    }

    public void setShedule(String shedule) {
        this.shedule = shedule;
    }

    public String getMonth() {
        return month;
    }

    public void setMonth(String month) {
        this.month = month;
    }
}
