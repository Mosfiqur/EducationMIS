package com.unicef.mis.model.benificiary.indicator;

import com.unicef.mis.model.ListItem;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.ArrayList;
import java.util.List;

public class Indicator {

    public static final int INT_TYPE = 1;
    public static final int VARCHAR_TYPE = 2;
    public static final int DECIMAL_TYPE = 3;
    public static final int DATETIME_TYPE = 4;
    public static final int BOOLEAN_TYPE = 5;
    public static final int LIST_TYPE = 6;


    @SerializedName("entityDynamicColumnId")
    @Expose
    private Integer entityDynamicColumnId;
    @SerializedName("columnOrder")
    @Expose
    private Integer columnOrder;
    @SerializedName("columnName")
    @Expose
    private String columnName;
    @SerializedName("columnDataType")
    @Expose
    private Integer columnDataType;
    @SerializedName("listObjectId")
    @Expose
    private Integer listObjectId;
    @SerializedName("listObjectName")
    @Expose
    private String listObjectName;
    @SerializedName("listItems")
    @Expose
    private List<ListItem> listItems = null;
    @SerializedName("dataCollectionDate")
    @Expose
    private String dataCollectionDate;
    @SerializedName("values")
    @Expose
    private List<String> values = null;
    @SerializedName("columnListId")
    @Expose
    private long listId;
    @SerializedName("columnNameInBangla")
    @Expose
    private String columnNameInBangla;
    @SerializedName("isMultiValued")
    @Expose
    private Boolean isMultiValued;

    private int status;
    private int recordId;

    public Indicator() {
        values = new ArrayList<String>();
    }

    public Integer getEntityDynamicColumnId() {
        return entityDynamicColumnId;
    }


    public void setEntityDynamicColumnId(Integer entityDynamicColumnId) {
        this.entityDynamicColumnId = entityDynamicColumnId;
    }

    public Integer getColumnOrder() {
        return columnOrder;
    }

    public void setColumnOrder(Integer columnOrder) {
        this.columnOrder = columnOrder;
    }

    public String getColumnName() {
        return columnName;
    }

    public void setColumnName(String columnName) {
        this.columnName = columnName;
    }

    public Integer getColumnDataType() {
        return columnDataType;
    }

    public void setColumnDataType(Integer columnDataType) {
        this.columnDataType = columnDataType;
    }

    public Integer getListObjectId() {
        return listObjectId;
    }

    public void setListObjectId(Integer listObjectId) {
        this.listObjectId = listObjectId;
    }

    public String getListObjectName() {
        return listObjectName;
    }

    public void setListObjectName(String listObjectName) {
        this.listObjectName = listObjectName;
    }

    public List<ListItem> getListItems() {
        return listItems;
    }

    public void setListItems(List<ListItem> listItems) {
        this.listItems = listItems;
    }

    public String getDataCollectionDate() {
        return dataCollectionDate;
    }

    public void setDataCollectionDate(String dataCollectionDate) {
        this.dataCollectionDate = dataCollectionDate;
    }

    public List<String> getValues() {
        return values;
    }

    public void setValues(List<String> values) {
        this.values = values;
    }

    public String getColumnNameInBangla() {
        return columnNameInBangla;
    }

    public void setColumnNameInBangla(String columnNameInBangla) {
        this.columnNameInBangla = columnNameInBangla;
    }

    public Boolean getMultiValued() {
        if(isMultiValued == null){
            isMultiValued = false;
        }
        return isMultiValued;
    }

    public void setMultiValued(Boolean multiValued) {
        if(multiValued == null){
            multiValued = false;
        }
        isMultiValued = multiValued;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public int getRecordId() {
        return recordId;
    }

    public void setRecordId(int recordId) {
        this.recordId = recordId;
    }

    public long getListId() {
        return listId;
    }

    public void setListId(long listId) {
        this.listId = listId;
    }
}