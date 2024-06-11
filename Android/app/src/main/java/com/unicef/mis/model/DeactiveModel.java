package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class DeactiveModel {
    @SerializedName("beneficiaryIds")
    @Expose
    private List<Integer> beneficairyIds = null;
    @SerializedName("instanceId")
    @Expose
    private Integer instanceId;

    public DeactiveModel(List<Integer> beneficairyIds, Integer instanceId) {
        this.beneficairyIds = beneficairyIds;
        this.instanceId = instanceId;
    }

    public List<Integer> getBeneficairyIds() {
        return beneficairyIds;
    }

    public void setBeneficairyIds(List<Integer> beneficairyIds) {
        this.beneficairyIds = beneficairyIds;
    }

    public Integer getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(Integer instanceId) {
        this.instanceId = instanceId;
    }

}
