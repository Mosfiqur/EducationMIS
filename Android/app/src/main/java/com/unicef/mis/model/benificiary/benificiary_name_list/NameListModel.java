package com.unicef.mis.model.benificiary.benificiary_name_list;


import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.model.Beneficiary;

import java.util.List;

public class NameListModel {

    @SerializedName("pageNumber")
    @Expose
    private Integer pageNumber;
    @SerializedName("pageSize")
    @Expose
    private Integer pageSize;
    @SerializedName("data")
    @Expose
    private List<Beneficiary> data = null;
    @SerializedName("total")
    @Expose
    private Integer total;

    public Integer getPageNumber() {
        return pageNumber;
    }

    public void setPageNumber(Integer pageNumber) {
        this.pageNumber = pageNumber;
    }

    public Integer getPageSize() {
        return pageSize;
    }

    public void setPageSize(Integer pageSize) {
        this.pageSize = pageSize;
    }

    public List<Beneficiary> getData() {
        return data;
    }

    public void setData(List<Beneficiary> data) {
        this.data = data;
    }

    public Integer getTotal() {
        return total;
    }

    public void setTotal(Integer total) {
        this.total = total;
    }

}