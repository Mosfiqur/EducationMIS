package com.unicef.mis.model.benificiary.indicator.post;


import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import java.util.ArrayList;
import java.util.Collection;
import java.util.Iterator;
import java.util.List;
import java.util.ListIterator;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class BeneficiaryDynamicCell {
    private int recordId;
    private int dataType;
    @SerializedName("value")
    @Expose
    private List<String> value = null;
    @SerializedName("entityDynamicColumnId")
    @Expose
    private long entityDynamicColumnId;

    public BeneficiaryDynamicCell(List<String> value, long entityDynamicColumnId) {
        this.value = value;
        this.entityDynamicColumnId = entityDynamicColumnId;
    }

    public BeneficiaryDynamicCell() {
        this.value = new ArrayList<String>();
    }

    public List<String> getValue() {
        return value;
    }

    public void setValue(List<String> value) {
        this.value = value;
    }

    public long getEntityDynamicColumnId() {
        return entityDynamicColumnId;
    }

    public void setEntityDynamicColumnId(long entityDynamicColumnId) {
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