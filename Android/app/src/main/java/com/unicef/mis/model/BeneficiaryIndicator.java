package com.unicef.mis.model;

import androidx.annotation.Nullable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class BeneficiaryIndicator {

    public static final int INT_TYPE = 1;
    public static final int VARCHAR_TYPE = 2;
    public static final int DECIMAL_TYPE = 3;
    public static final int DATETIME_TYPE = 4;
    public static final int BOOLEAN_TYPE = 5;
    public static final int LIST_TYPE = 6;


    private long id;
    @SerializedName("entityColumnId")
    @Expose
    private long dynamicColumnId;
    private long beneficiaryId;
    private long instanceId;
    private long facilityId;
    @SerializedName("properties")
    @Expose
    private String columnName;
    @SerializedName("columnNameInBangla")
    @Expose
    private String columnNameInBangla;
    @SerializedName("dataType")
    @Expose
    @Nullable
    private Integer columnDataType;
    @SerializedName("isMultiValued")
    @Expose
    private Boolean isMultiValued;
    @SerializedName("values")
    @Expose
    private List<String> values;
    @SerializedName("columnListId")
    @Expose
    private long listId;
    @SerializedName("columnListName")
    @Expose
    private String listName;
    @SerializedName("listItem")
    @Expose
    private List<ListItem> listItems;
    private int status;
    @SerializedName("listObjectId")
    @Expose
    private Integer listObjectId;
    @SerializedName("listObject")
    @Expose
    private ListObject listObject;


    public BeneficiaryIndicator() {
    }

    public ListObject getListObject() {
        return listObject;
    }

    public void setListObject(ListObject listObject) {
        this.listObject = listObject;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public long getDynamicColumnId() {
        return dynamicColumnId;
    }

    public void setDynamicColumnId(long dynamicColumnId) {
        this.dynamicColumnId = dynamicColumnId;
    }

    public long getBeneficiaryId() {
        return beneficiaryId;
    }

    public void setBeneficiaryId(long beneficiaryId) {
        this.beneficiaryId = beneficiaryId;
    }

    public long getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(long facilityId) {
        this.facilityId = facilityId;
    }

    public long getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(long instanceId) {
        this.instanceId = instanceId;
    }

    public String getColumnName() {
        return columnName;
    }

    public void setColumnName(String columnName) {
        this.columnName = columnName;
    }

    public String getColumnNameInBangla() {
        return columnNameInBangla;
    }

    public void setColumnNameInBangla(String columnNameInBangla) {
        this.columnNameInBangla = columnNameInBangla;
    }

    public Integer getColumnDataType() {
        return columnDataType;
    }

    public void setColumnDataType(Integer columnDataType) {
        this.columnDataType = columnDataType;
    }

    public long getListId() {
        return listId;
    }

    public void setListId(long listId) {
        this.listId = listId;
    }

    public List<ListItem> getListItems() {
        return listItems;
    }

    public void setListItems(List<ListItem> listItems) {
        this.listItems = listItems;
    }

    public String getListName() {
        return listName;
    }

    public void setListName(String listName) {
        this.listName = listName;
    }

    public List<String> getValues() {
        return values;
    }

    public void setValues(List<String> values) {
        this.values = values;
    }

    public boolean getIsMultiValued() {
        return isMultiValued;
    }

    public void setIsMultiValued(boolean multiValued) {
        isMultiValued = multiValued;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public Integer getListObjectId() {
        return listObjectId;
    }

    public void setListObjectId(Integer listObjectId) {
        this.listObjectId = listObjectId;
    }


}
