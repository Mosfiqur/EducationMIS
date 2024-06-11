package com.unicef.mis.model.benificiary.benificiary_name_list;

import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Property {

    public static final int INT_TYPE = 1;
    public static final int VARCHAR_TYPE = 2;
    public static final int DECIMAL_TYPE = 3;
    public static final int DATETIME_TYPE = 4;
    public static final int BOOLEAN_TYPE = 5;
    public static final int LIST_TYPE = 6;

    @SerializedName("entityColumnId")
    @Expose
    private Integer entityColumnId;
    @SerializedName("properties")
    @Expose
    private String properties;
    @SerializedName("values")
    @Expose
    private List<String> values = null;
    @SerializedName("isVersionColumn")
    @Expose
    private Boolean isVersionColumn;
    @SerializedName("isFixed")
    @Expose
    private Boolean isFixed;
    @SerializedName("isMultiValued")
    @Expose
    private boolean isMultiValued;
    @SerializedName("dataType")
    @Expose
    private Integer dataType;
    @SerializedName("columnListId")
    @Expose
    private Object columnListId;
    @SerializedName("columnListName")
    @Expose
    private Object columnListName;
    @SerializedName("listItem")
    @Expose
    private List<ListItem> listItem = null;

    public Integer getEntityColumnId() {
        return entityColumnId;
    }

    public void setEntityColumnId(Integer entityColumnId) {
        this.entityColumnId = entityColumnId;
    }

    public String getProperties() {
        return properties;
    }

    public void setProperties(String properties) {
        this.properties = properties;
    }

    public List<String> getValues() {
        return values;
    }

    public void setValues(List<String> values) {
        this.values = values;
    }

    public Boolean getIsVersionColumn() {
        return isVersionColumn;
    }

    public void setIsVersionColumn(Boolean isVersionColumn) {
        this.isVersionColumn = isVersionColumn;
    }

    public Boolean getIsFixed() {
        return isFixed;
    }

    public void setIsFixed(Boolean isFixed) {
        this.isFixed = isFixed;
    }

    public boolean isMultiValued() {
        return isMultiValued;
    }

    public void setMultiValued(boolean multiValued) {
        isMultiValued = multiValued;
    }

    public Integer getDataType() {
        return dataType;
    }

    public void setDataType(Integer dataType) {
        this.dataType = dataType;
    }

    public Object getColumnListId() {
        return columnListId;
    }

    public void setColumnListId(Object columnListId) {
        this.columnListId = columnListId;
    }

    public Object getColumnListName() {
        return columnListName;
    }

    public void setColumnListName(Object columnListName) {
        this.columnListName = columnListName;
    }

    public List<ListItem> getListItem() {
        return listItem;
    }

    public void setListItem(List<ListItem> listItem) {
        this.listItem = listItem;
    }

}