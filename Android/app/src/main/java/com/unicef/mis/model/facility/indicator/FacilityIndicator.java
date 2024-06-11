package com.unicef.mis.model.facility.indicator;

import java.util.ArrayList;
import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class FacilityIndicator {

    @SerializedName("facilityId")
    @Expose
    private Integer facilityId;
    @SerializedName("facilityName")
    @Expose
    private String facilityName;
    @SerializedName("instanceId")
    @Expose
    private Integer instanceId;
    @SerializedName("indicators")
    @Expose
    private List<Indicator> indicators = null;

    public FacilityIndicator(){
        indicators = new ArrayList<Indicator>();
    }

    public Integer getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(Integer facilityId) {
        this.facilityId = facilityId;
    }

    public String getFacilityName() {
        return facilityName;
    }

    public void setFacilityName(String facilityName) {
        this.facilityName = facilityName;
    }

    public Integer getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(Integer instanceId) {
        this.instanceId = instanceId;
    }

    public List<Indicator> getIndicators() {
        return indicators;
    }

    public void setIndicators(List<Indicator> indicators) {
        this.indicators = indicators;
    }

}