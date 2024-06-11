package com.unicef.mis.model.benificiary.indicator.post;

import java.util.ArrayList;
import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class BeneficiaryPost {
    private int id;
    @SerializedName("beneficiaryId")
    @Expose
    private Integer beneficiaryId;
    @SerializedName("instanceId")
    @Expose
    private Integer instanceId;
    @SerializedName("dynamicCells")
    @Expose
    private List<BeneficiaryDynamicCell> dynamicCells = null;

    public BeneficiaryPost() {
        dynamicCells = new ArrayList<BeneficiaryDynamicCell>();
    }

    public Integer getBeneficiaryId() {
        return beneficiaryId;
    }

    public BeneficiaryPost(Integer beneficiaryId, Integer instanceId, List<BeneficiaryDynamicCell> dynamicCells) {
        this.beneficiaryId = beneficiaryId;
        this.instanceId = instanceId;
        this.dynamicCells = dynamicCells;
    }

    public void setBeneficiaryId(Integer beneficiaryId) {
        this.beneficiaryId = beneficiaryId;
    }

    public Integer getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(Integer instanceId) {
        this.instanceId = instanceId;
    }

    public List<BeneficiaryDynamicCell> getDynamicCells() {
        return dynamicCells;
    }

    public void setDynamicCells(List<BeneficiaryDynamicCell> dynamicCells) {
        this.dynamicCells = dynamicCells;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }
}