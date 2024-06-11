package com.unicef.mis.model.facility.indicator.post;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.ArrayList;
import java.util.List;

public class FacilityDynamicCell {
    private int recordId;
    private int dataType;
    @SerializedName("value")
    @Expose
    private List<String> value = null;
    @SerializedName("entityDynamicColumnId")
    @Expose
    private Integer entityDynamicColumnId;

    public FacilityDynamicCell(List<String> value, Integer entityDynamicColumnId) {
        this.value = value;
        this.entityDynamicColumnId = entityDynamicColumnId;
    }

    public FacilityDynamicCell() {
        value = new ArrayList<String>();
    }

    public List<String> getValue() {
        return value;
    }

    public void setValue(List<String> value) {
        this.value = value;
    }

    public Integer getEntityDynamicColumnId() {
        return entityDynamicColumnId;
    }

    public void setEntityDynamicColumnId(Integer entityDynamicColumnId) {
        this.entityDynamicColumnId = entityDynamicColumnId;
    }

    public int getRecordId() {
        return recordId;
    }

    public void setRecordId(int recordId) {
        this.recordId = recordId;
    }

    public int getDataType() {
        return dataType;
    }

    public void setDataType(int dataType) {
        this.dataType = dataType;
    }
}
