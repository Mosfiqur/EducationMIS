package com.unicef.mis.model.benificiary.benificiary_name_list;

import java.util.List;
import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class Datum {

    @SerializedName("properties")
    @Expose
    private List<Property> properties;
    @SerializedName("entityId")
    @Expose
    private Integer entityId;
    @SerializedName("beneficiaryName")
    @Expose
    private String beneficiaryName;
    @SerializedName("progressId")
    @Expose
    private String progressId;
    @SerializedName("fcnId")
    @Expose
    private String fcnId;
    @SerializedName("fatherName")
    @Expose
    private String fatherName;
    @SerializedName("motherName")
    @Expose
    private String motherName;
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
    private Integer levelOfStudy;
    @SerializedName("enrollmentDate")
    @Expose
    private String enrollmentDate;
    @SerializedName("facilityId")
    @Expose
    private Integer facilityId;
    @SerializedName("facilityName")
    @Expose
    private String facilityName;
    @SerializedName("facilityCampId")
    @Expose
    private Integer facilityCampId;
    @SerializedName("facilityCampName")
    @Expose
    private String facilityCampName;
    @SerializedName("beneficiaryCampId")
    @Expose
    private Integer beneficiaryCampId;
    @SerializedName("beneficiaryCampName")
    @Expose
    private String beneficiaryCampName;
    @SerializedName("blockId")
    @Expose
    private Integer blockId;
    @SerializedName("blockName")
    @Expose
    private String blockName;
    @SerializedName("subBlockId")
    @Expose
    private Integer subBlockId;
    @SerializedName("subBlockName")
    @Expose
    private String subBlockName;
    @SerializedName("programmingPartnerId")
    @Expose
    private Integer programmingPartnerId;
    @SerializedName("programmingPartnerName")
    @Expose
    private String programmingPartnerName;
    @SerializedName("implemantationPartnerId")
    @Expose
    private Integer implemantationPartnerId;
    @SerializedName("implemantationPartnerName")
    @Expose
    private String implemantationPartnerName;
    @SerializedName("collectionStatus")
    @Expose
    private Integer collectionStatus;

    public List<Property> getProperties() {
        return properties;
    }

    public void setProperties(List<Property> properties) {
        this.properties = properties;
    }

    public Integer getEntityId() {
        return entityId;
    }

    public void setEntityId(Integer entityId) {
        this.entityId = entityId;
    }

    public String getBeneficiaryName() {
        return beneficiaryName;
    }

    public void setBeneficiaryName(String beneficiaryName) {
        this.beneficiaryName = beneficiaryName;
    }

    public String getProgressId() {
        return progressId;
    }

    public void setProgressId(String progressId) {
        this.progressId = progressId;
    }

    public String getFcnId() {
        return fcnId;
    }

    public void setFcnId(String fcnId) {
        this.fcnId = fcnId;
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

    public Integer getLevelOfStudy() {
        return levelOfStudy;
    }

    public void setLevelOfStudy(Integer levelOfStudy) {
        this.levelOfStudy = levelOfStudy;
    }

    public String getEnrollmentDate() {
        return enrollmentDate;
    }

    public void setEnrollmentDate(String enrollmentDate) {
        this.enrollmentDate = enrollmentDate;
    }

    public Integer getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(Integer facilityId) {
        this.facilityId = facilityId;
    }

    public String getFacilityName() {
        return facilityName;
    }

    public void setFacilityName(String facilityName) {
        this.facilityName = facilityName;
    }

    public Integer getFacilityCampId() {
        return facilityCampId;
    }

    public void setFacilityCampId(Integer facilityCampId) {
        this.facilityCampId = facilityCampId;
    }

    public String getFacilityCampName() {
        return facilityCampName;
    }

    public void setFacilityCampName(String facilityCampName) {
        this.facilityCampName = facilityCampName;
    }

    public Integer getBeneficiaryCampId() {
        return beneficiaryCampId;
    }

    public void setBeneficiaryCampId(Integer beneficiaryCampId) {
        this.beneficiaryCampId = beneficiaryCampId;
    }

    public String getBeneficiaryCampName() {
        return beneficiaryCampName;
    }

    public void setBeneficiaryCampName(String beneficiaryCampName) {
        this.beneficiaryCampName = beneficiaryCampName;
    }

    public Integer getBlockId() {
        return blockId;
    }

    public void setBlockId(Integer blockId) {
        this.blockId = blockId;
    }

    public String getBlockName() {
        return blockName;
    }

    public void setBlockName(String blockName) {
        this.blockName = blockName;
    }

    public Integer getSubBlockId() {
        return subBlockId;
    }

    public void setSubBlockId(Integer subBlockId) {
        this.subBlockId = subBlockId;
    }

    public String getSubBlockName() {
        return subBlockName;
    }

    public void setSubBlockName(String subBlockName) {
        this.subBlockName = subBlockName;
    }

    public Integer getProgrammingPartnerId() {
        return programmingPartnerId;
    }

    public void setProgrammingPartnerId(Integer programmingPartnerId) {
        this.programmingPartnerId = programmingPartnerId;
    }

    public String getProgrammingPartnerName() {
        return programmingPartnerName;
    }

    public void setProgrammingPartnerName(String programmingPartnerName) {
        this.programmingPartnerName = programmingPartnerName;
    }

    public Integer getImplemantationPartnerId() {
        return implemantationPartnerId;
    }

    public void setImplemantationPartnerId(Integer implemantationPartnerId) {
        this.implemantationPartnerId = implemantationPartnerId;
    }

    public String getImplemantationPartnerName() {
        return implemantationPartnerName;
    }

    public void setImplemantationPartnerName(String implemantationPartnerName) {
        this.implemantationPartnerName = implemantationPartnerName;
    }

    public Integer getCollectionStatus() {
        return collectionStatus;
    }

    public void setCollectionStatus(Integer collectionStatus) {
        this.collectionStatus = collectionStatus;
    }

}