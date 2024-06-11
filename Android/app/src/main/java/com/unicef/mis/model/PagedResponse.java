package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

public class PagedResponse<T> {
    @SerializedName("pageNumber")
    @Expose
    private Integer pageNumber;
    @SerializedName("pageSize")
    @Expose
    private Integer pageSize;
    @SerializedName("total")
    @Expose
    private Integer total;
    @SerializedName("notActedTotal")
    @Expose
    private String notActedTotal;
    @SerializedName("data")
    @Expose
    private List<T> data;


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

    public Integer getTotal() {
        return total;
    }

    public void setTotal(Integer total) {
        this.total = total;
    }

    public List<T> getData() {
        return data;
    }

    public void setData(List<T> data) {
        this.data = data;
    }

    public String getNotActedTotal() {
        return notActedTotal;
    }

    public void setNotActedTotal(String notActedTotal) {
        this.notActedTotal = notActedTotal;
    }
}
