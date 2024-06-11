package com.unicef.mis.model.auth;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class UserProfile {

    @SerializedName("id")
    @Expose
    private Integer id;
    @SerializedName("fullName")
    @Expose
    private String fullName;
    @SerializedName("passWord")
    @Expose
    private Object passWord;
    @SerializedName("levelId")
    @Expose
    private Integer levelId;
    @SerializedName("levelName")
    @Expose
    private String levelName;
    @SerializedName("levelRank")
    @Expose
    private Integer levelRank;
    @SerializedName("designationId")
    @Expose
    private Integer designationId;
    @SerializedName("designationName")
    @Expose
    private String designationName;
    @SerializedName("roleId")
    @Expose
    private Integer roleId;
    @SerializedName("roleName")
    @Expose
    private String roleName;
    @SerializedName("email")
    @Expose
    private String email;
    @SerializedName("phoneNumber")
    @Expose
    private Object phoneNumber;
    @SerializedName("eduSectorPartners")
    @Expose
    private Object eduSectorPartners;

    @SerializedName("Error")
    @Expose
    private String error;

    public UserProfile() {
    }

    public UserProfile(Integer id, String fullName, Integer levelId, String levelName,
                       Integer levelRank, Integer designationId, String designationName, Integer roleId,
                       String email, String roleName, Object phoneNumber) {
        this.id = id;
        this.fullName = fullName;
        this.passWord = passWord;
        this.levelId = levelId;
        this.levelName = levelName;
        this.levelRank = levelRank;
        this.designationId = designationId;
        this.designationName = designationName;
        this.roleId = roleId;
        this.roleName = roleName;
        this.email = email;
        this.phoneNumber = phoneNumber;
        this.eduSectorPartners = eduSectorPartners;
    }

    public String getError() {
        return error;
    }

    public void setError(String error) {
        this.error = error;
    }

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public Object getPassWord() {
        return passWord;
    }

    public void setPassWord(Object passWord) {
        this.passWord = passWord;
    }

    public Integer getLevelId() {
        return levelId;
    }

    public void setLevelId(Integer levelId) {
        this.levelId = levelId;
    }

    public String getLevelName() {
        return levelName;
    }

    public void setLevelName(String levelName) {
        this.levelName = levelName;
    }

    public Integer getLevelRank() {
        return levelRank;
    }

    public void setLevelRank(Integer levelRank) {
        this.levelRank = levelRank;
    }

    public Integer getDesignationId() {
        return designationId;
    }

    public void setDesignationId(Integer designationId) {
        this.designationId = designationId;
    }

    public String getDesignationName() {
        return designationName;
    }

    public void setDesignationName(String designationName) {
        this.designationName = designationName;
    }

    public Integer getRoleId() {
        return roleId;
    }

    public void setRoleId(Integer roleId) {
        this.roleId = roleId;
    }

    public String getRoleName() {
        return roleName;
    }

    public void setRoleName(String roleName) {
        this.roleName = roleName;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public Object getPhoneNumber() {
        return phoneNumber;
    }

    public void setPhoneNumber(Object phoneNumber) {
        this.phoneNumber = phoneNumber;
    }

    public Object getEduSectorPartners() {
        return eduSectorPartners;
    }

    public void setEduSectorPartners(Object eduSectorPartners) {
        this.eduSectorPartners = eduSectorPartners;
    }

}