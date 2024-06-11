package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class PropertiesInfoModel {
    @SerializedName("Id")
    @Expose
    private long id;
    private String columnName;
    @SerializedName("entityColumnId")
    @Expose
    private Integer entityColumnId;
    private int instanceId;
    @SerializedName("properties")
    @Expose
    private String properties;
    @SerializedName("columnNameInBangla")
    @Expose
    private String columnNameInBangla;
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
    private Boolean isMultiValued;
    @SerializedName("dataType")
    @Expose
    private Integer dataType;
    @SerializedName("columnListId")
    @Expose
    private long columnListId;
    @SerializedName("columnListName")
    @Expose
    private Object columnListName;
    @SerializedName("status")
    @Expose
    private int status;
    @SerializedName("listItem")
    @Expose
    private List<ListItem> listItem = null;

    public PropertiesInfoModel() {
    }

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

    public String getColumnNameInBangla() {
        return columnNameInBangla;
    }

    public void setColumnNameInBangla(String columnNameInBangla) {
        this.columnNameInBangla = columnNameInBangla;
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

    public Boolean getIsMultiValued() {
        return isMultiValued;
    }

    public void setIsMultiValued(Boolean isMultiValued) {
        this.isMultiValued = isMultiValued;
    }

    public Integer getDataType() {
        return dataType;
    }

    public void setDataType(Integer dataType) {
        this.dataType = dataType;
    }

    public long getColumnListId() {
        return columnListId;
    }

    public void setColumnListId(long columnListId) {
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

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getColumnName() {
        return columnName;
    }

    public void setColumnName(String columnName) {
        this.columnName = columnName;
    }

    public int getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(int instanceId) {
        this.instanceId = instanceId;
    }
}
