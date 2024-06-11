package com.unicef.mis.model.facility.indicator.post;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryDynamicCell;

import java.util.ArrayList;
import java.util.List;

public class FacilityPost {
    private int id;
    @SerializedName("facilityId")
    @Expose
    private int facilityId;
    @SerializedName("instanceId")
    @Expose
    private int instanceId;
    @SerializedName("dynamicCells")
    @Expose
    private List<FacilityDynamicCell> dynamicCells = null;

    public FacilityPost(int facilityId, int instanceId, List<FacilityDynamicCell> dynamicCells) {
        this.facilityId = facilityId;
        this.instanceId = instanceId;
        this.dynamicCells = dynamicCells;
    }

    public FacilityPost() {
        this.dynamicCells = new ArrayList<FacilityDynamicCell>();
    }

    public int getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(int facilityId) {
        this.facilityId = facilityId;
    }

    public int getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(int instanceId) {
        this.instanceId = instanceId;
    }

    public List<FacilityDynamicCell> getDynamicCells() {
        return dynamicCells;
    }

    public void setDynamicCells(List<FacilityDynamicCell> dynamicCells) {
        this.dynamicCells = dynamicCells;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }
}
