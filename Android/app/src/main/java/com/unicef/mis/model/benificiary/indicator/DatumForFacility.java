package com.unicef.mis.model.benificiary.indicator;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class DatumForFacility {
    @SerializedName("facilityId")
    @Expose
    private Integer facilityId;
    @SerializedName("beneficairyName")
    @Expose
    private String beneficairyName;
    @SerializedName("indicators")
    @Expose
    private List<Indicator> indicators = null;

    public Integer getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(Integer facilityId) {
        this.facilityId = facilityId;
    }

    public String getBeneficairyName() {
        return beneficairyName;
    }

    public void setBeneficairyName(String beneficairyName) {
        this.beneficairyName = beneficairyName;
    }

    public List<com.unicef.mis.model.benificiary.indicator.Indicator> getIndicators() {
        return indicators;
    }

    public void setIndicators(List<com.unicef.mis.model.benificiary.indicator.Indicator> indicators) {
        this.indicators = indicators;
    }
}
