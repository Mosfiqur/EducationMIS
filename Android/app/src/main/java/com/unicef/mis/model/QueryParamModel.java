package com.unicef.mis.model;

public class QueryParamModel {
    private int pageNumber;
    private int pageSize;
    private String searchText;

    public QueryParamModel() {
    }

    public QueryParamModel(int pageNumber, int pageSize, String searchText){
        this.pageNumber = pageNumber;
        this.pageSize = pageSize;
        this.searchText = searchText;
    }

    public int getPageNumber() {
        return pageNumber;
    }

    public void setPageNumber(int pageNumber) {
        this.pageNumber = pageNumber;
    }

    public int getPageSize() {
        return pageSize;
    }

    public void setPageSize(int pageSize) {
        this.pageSize = pageSize;
    }

    public String getSearchText() {
        return searchText;
    }

    public void setSearchText(String searchText) {
        this.searchText = searchText;
    }
}
