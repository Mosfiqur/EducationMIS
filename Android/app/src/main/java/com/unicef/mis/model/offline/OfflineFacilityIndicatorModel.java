package com.unicef.mis.model.offline;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.model.ListItem;

import java.util.List;

public class OfflineFacilityIndicatorModel {



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
    @SerializedName("columnListId")
    @Expose
    private long listId;
    @SerializedName("listObjectName")
    @Expose
    private String listObjectName;

    @SerializedName("dataCollectionDate")
    @Expose
    private String dataCollectionDate;
    @SerializedName("values")
    @Expose
    private List<String> values = null;
    @SerializedName("columnListName")
    @Expose
    private String listName;

    @SerializedName("columnNameInBangla")
    @Expose
    private String columnNameInBangla;
//    @SerializedName("isMultiValued")
//    @Expose
//    private Boolean isMultiValued;

    @SerializedName("isMultiValued")
    @Expose
    private int isMultiValued;

    private List<ListItem> listItems;

    private Integer instanceId;

    public OfflineFacilityIndicatorModel(int entityDynamicColumnId, int instanceId, String columnName, Integer columnDataType,
                                         int isMultiValued, int listId) {

        this.entityDynamicColumnId = entityDynamicColumnId;
        this.instanceId = instanceId;
        this.columnName = columnName;
        this.columnDataType = columnDataType;
        this.isMultiValued = isMultiValued;
        this.listId = listId;

    }

    public Integer getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(Integer instanceId) {
        this.instanceId = instanceId;
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

    public long getListId() {
        return listId;
    }

    public void setListId(long listId) {
        this.listId = listId;
    }

    public String getListName() {
        return listName;
    }

    public void setListName(String listName) {
        this.listName = listName;
    }

    public String getColumnNameInBangla() {
        return columnNameInBangla;
    }

    public void setColumnNameInBangla(String columnNameInBangla) {
        this.columnNameInBangla = columnNameInBangla;
    }

//    public Boolean getMultiValued() {
//        return isMultiValued;
//    }
//
//    public void setMultiValued(Boolean multiValued) {
//        isMultiValued = multiValued;
//    }


    public int getIsMultiValued() {
        return isMultiValued;
    }

    public void setIsMultiValued(int isMultiValued) {
        this.isMultiValued = isMultiValued;
    }
}
