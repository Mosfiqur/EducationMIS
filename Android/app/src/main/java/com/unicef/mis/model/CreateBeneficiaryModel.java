package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class CreateBeneficiaryModel {
    @SerializedName("id")
    @Expose
    private long id;
    @SerializedName("unhcrId")
    @Expose
    private String unhcrId;
    @SerializedName("name")
    @Expose
    private String name;
    @SerializedName("fatherName")
    @Expose
    private String fatherName;
    @SerializedName("motherName")
    @Expose
    private String motherName;
    @SerializedName("fcnId")
    @Expose
    private String fcnId;
    @SerializedName("dateOfBirth")
    @Expose
    private String dateOfBirth;
    @SerializedName("sex")
    @Expose
    private Integer sex;
    @SerializedName("disabled")
    @Expose
    private Boolean disabled;
    @SerializedName("levelOfStudy")
    @Expose
    private String levelOfStudy;
    @SerializedName("enrollmentDate")
    @Expose
    private String enrollmentDate;
    @SerializedName("facilityId")
    @Expose
    private long facilityId;
    @SerializedName("facilityCampId")
    @Expose
    private Integer facilityCampId;
    @SerializedName("beneficiaryCampId")
    @Expose
    private Integer beneficiaryCampId;
    @SerializedName("blockId")
    @Expose
    private Integer blockId;
    @SerializedName("subBlockId")
    @Expose
    private Integer subBlockId;
    @SerializedName("remarks")
    @Expose
    private String remarks;
    @SerializedName("isApproved")
    @Expose
    private Boolean isApproved;
    private int databaseId;

    @SerializedName("instanceId")
    @Expose
    private int instanceId;

    public CreateBeneficiaryModel() {

    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getUnhcrId() {
        return unhcrId;
    }

    public void setUnhcrId(String unhcrId) {
        this.unhcrId = unhcrId;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getFatherName() {
        return fatherName;
    }

    public void setFatherName(String fatherName) {
        this.fatherName = fatherName;
    }

    public String getMotherName() {
        return motherName;
    }

    public void setMotherName(String motherName) {
        this.motherName = motherName;
    }

    public String getFcnId() {
        return fcnId;
    }

    public void setFcnId(String fcnId) {
        this.fcnId = fcnId;
    }

    public String getDateOfBirth() {
        return dateOfBirth;
    }

    public void setDateOfBirth(String dateOfBirth) {
        this.dateOfBirth = dateOfBirth;
    }

    public Integer getSex() {
        return sex;
    }

    public void setSex(Integer sex) {
        this.sex = sex;
    }

    public Boolean getDisabled() {
        return disabled;
    }

    public void setDisabled(Boolean disabled) {
        this.disabled = disabled;
    }

    public String getLevelOfStudy() {
        return levelOfStudy;
    }

    public void setLevelOfStudy(String levelOfStudy) {
        this.levelOfStudy = levelOfStudy;
    }

    public String getEnrollmentDate() {
        return enrollmentDate;
    }

    public void setEnrollmentDate(String enrollmentDate) {
        this.enrollmentDate = enrollmentDate;
    }

    public long getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(long facilityId) {
        this.facilityId = facilityId;
    }

    public Integer getFacilityCampId() {
        return facilityCampId;
    }

    public void setFacilityCampId(Integer facilityCampId) {
        this.facilityCampId = facilityCampId;
    }

    public Integer getBeneficiaryCampId() {
        return beneficiaryCampId;
    }

    public void setBeneficiaryCampId(Integer beneficiaryCampId) {
        this.beneficiaryCampId = beneficiaryCampId;
    }

    public Integer getBlockId() {
        return blockId;
    }

    public void setBlockId(Integer blockId) {
        this.blockId = blockId;
    }

    public Integer getSubBlockId() {
        return subBlockId;
    }

    public void setSubBlockId(Integer subBlockId) {
        this.subBlockId = subBlockId;
    }

    public String getRemarks() {
        return remarks;
    }

    public void setRemarks(String remarks) {
        this.remarks = remarks;
    }

    public Boolean getIsApproved() {
        return isApproved;
    }

    public void setIsApproved(Boolean isApproved) {
        this.isApproved = isApproved;
    }

    public int getDatabaseId() {
        return databaseId;
    }

    public void setDatabaseId(int databaseId) {
        this.databaseId = databaseId;
    }

    public int getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(int instanceId) {
        this.instanceId = instanceId;
    }
}
