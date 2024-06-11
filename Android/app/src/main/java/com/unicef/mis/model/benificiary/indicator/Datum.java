package com.unicef.mis.model.benificiary.indicator;

import java.util.ArrayList;
import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.model.BeneficiaryIndicator;

public class Datum {

    @SerializedName("beneficiaryId")
    @Expose
    private Integer beneficiaryId;
    @SerializedName("beneficairyName")
    @Expose
    private String beneficairyName;
    @SerializedName("instanceId")
    @Expose
    private Integer instanceId;
    @SerializedName("indicators")
    @Expose
    private List<Indicator> indicators = null;

    public Datum(){
        indicators = new ArrayList<Indicator>();
    }

    public Integer getBeneficiaryId() {
        return beneficiaryId;
    }

    public void setBeneficiaryId(Integer beneficiaryId) {
        this.beneficiaryId = beneficiaryId;
    }

    public String getBeneficairyName() {
        return beneficairyName;
    }

    public void setBeneficairyName(String beneficairyName) {
        this.beneficairyName = beneficairyName;
    }

    public List<Indicator> getIndicators() {
        return indicators;
    }

    public void setIndicators(List<Indicator> indicators) {
        this.indicators = indicators;
    }

    public Integer getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(Integer instanceId) {
        this.instanceId = instanceId;
    }
}