package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class IndicatorNew {
    @SerializedName("entityDynamicColumnId")
    @Expose
    private Integer entityDynamicColumnId;
    @SerializedName("columnOrder")
    @Expose
    private Integer columnOrder;
    @SerializedName("indicatorName")
    @Expose
    private String columnName;
    @SerializedName("columnDataType")
    @Expose
    private Integer columnDataType;
    @SerializedName("listObject")
    @Expose
    private ListObject listObject;
    @SerializedName("indicatorNameInBangla")
    @Expose
    private String columnNameInBangla;
    @SerializedName("isMultivalued")
    @Expose
    private Boolean isMultiValued;
    @SerializedName("listItems")
    @Expose
    private List<ListItem> listItems;
    @SerializedName("values")
    @Expose
    private List<String> values = null;

    public IndicatorNew(){

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

    public List<ListItem> getListItems() {
        return listItems;
    }

    public void setListItems(List<ListItem> listItems) {
        this.listItems = listItems;
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
        if(isMultiValued == null){
            isMultiValued = false;
        }
        this.isMultiValued = multiValued;
    }

    public ListObject getListObject() {
        return listObject;
    }

    public void setListObject(ListObject listObject) {
        this.listObject = listObject;
    }

    public List<String> getValues() {
        return values;
    }

    public void setValues(List<String> values) {
        this.values = values;
    }
}
