package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Camp {

    @SerializedName("id")
    @Expose
    private Integer id;
    @SerializedName("ssid")
    @Expose
    private String ssid;
    @SerializedName("name")
    @Expose
    private String name;
    @SerializedName("nameAlias")
    @Expose
    private String nameAlias;
    @SerializedName("unionId")
    @Expose
    private Integer unionId;

    public Camp() {
    }

    public Camp(Integer id, String ssid, String name, String nameAlias, Integer unionId) {
        this.id = id;
        this.ssid = ssid;
        this.name = name;
        this.nameAlias = nameAlias;
        this.unionId = unionId;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getSsid() {
        return ssid;
    }

    public void setSsid(String ssid) {
        this.ssid = ssid;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getNameAlias() {
        return nameAlias;
    }

    public void setNameAlias(String nameAlias) {
        this.nameAlias = nameAlias;
    }

    public Integer getUnionId() {
        return unionId;
    }

    public void setUnionId(Integer unionId) {
        this.unionId = unionId;
    }

}