package com.unicef.mis.model;



import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.R;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.util.UnicefApplication;


import java.util.List;

import static com.unicef.mis.enumtype.CollectionStatus.NotCollected;

public class Beneficiary implements Parcelable {
    private int id;
    private String name;
    private int instanceId;
    @SerializedName("isActive")
    @Expose
    private Boolean isActive;
    @SerializedName("unhcrId")
    @Expose
    private String unhcrId;

    @SerializedName("entityId")
    @Expose
    private Integer entityId;
    @SerializedName("beneficiaryName")
    @Expose
    private String beneficiaryName;
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
    @SerializedName("properties")
    @Expose
    private List<PropertiesInfoModel> indicators;

    private Integer beneficiaryId;

    protected Beneficiary(Parcel in) {
        id = in.readInt();
        byte tmpIsActive = in.readByte();
        isActive = tmpIsActive == 0 ? null : tmpIsActive == 1;
        unhcrId = in.readString();
        if (in.readByte() == 0) {
            entityId = null;
        } else {
            entityId = in.readInt();
        }
        beneficiaryName = in.readString();
        if (in.readByte() == 0) {
            facilityId = null;
        } else {
            facilityId = in.readInt();
        }
        facilityName = in.readString();
        if (in.readByte() == 0) {
            facilityCampId = null;
        } else {
            facilityCampId = in.readInt();
        }
        facilityCampName = in.readString();
        if (in.readByte() == 0) {
            beneficiaryCampId = null;
        } else {
            beneficiaryCampId = in.readInt();
        }
        beneficiaryCampName = in.readString();
        if (in.readByte() == 0) {
            blockId = null;
        } else {
            blockId = in.readInt();
        }
        blockName = in.readString();
        if (in.readByte() == 0) {
            subBlockId = null;
        } else {
            subBlockId = in.readInt();
        }
        subBlockName = in.readString();
        if (in.readByte() == 0) {
            programmingPartnerId = null;
        } else {
            programmingPartnerId = in.readInt();
        }
        programmingPartnerName = in.readString();
        if (in.readByte() == 0) {
            implemantationPartnerId = null;
        } else {
            implemantationPartnerId = in.readInt();
        }
        implemantationPartnerName = in.readString();
        if (in.readByte() == 0) {
            collectionStatus = null;
        } else {
            collectionStatus = in.readInt();
        }
        fatherName = in.readString();
        motherName = in.readString();
        dateOfBirth = in.readString();
        if (in.readByte() == 0) {
            sex = null;
        } else {
            sex = in.readInt();
        }
        byte tmpDisabled = in.readByte();
        disabled = tmpDisabled == 0 ? null : tmpDisabled == 1;
        fcnId = in.readString();
        if (in.readByte() == 0) {
            levelOfStudy = null;
        } else {
            levelOfStudy = in.readInt();
        }
        enrollmentDate = in.readString();
        remarks = in.readString();
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeInt(id);
        dest.writeByte((byte) (isActive == null ? 0 : isActive ? 1 : 2));
        dest.writeString(unhcrId);
        if (entityId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(entityId);
        }
        dest.writeString(beneficiaryName);
        if (facilityId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(facilityId);
        }
        dest.writeString(facilityName);
        if (facilityCampId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(facilityCampId);
        }
        dest.writeString(facilityCampName);
        if (beneficiaryCampId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(beneficiaryCampId);
        }
        dest.writeString(beneficiaryCampName);
        if (blockId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(blockId);
        }
        dest.writeString(blockName);
        if (subBlockId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(subBlockId);
        }
        dest.writeString(subBlockName);
        if (programmingPartnerId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(programmingPartnerId);
        }
        dest.writeString(programmingPartnerName);
        if (implemantationPartnerId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(implemantationPartnerId);
        }
        dest.writeString(implemantationPartnerName);
        if (collectionStatus == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(collectionStatus);
        }
        dest.writeString(fatherName);
        dest.writeString(motherName);
        dest.writeString(dateOfBirth);
        if (sex == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(sex);
        }
        dest.writeByte((byte) (disabled == null ? 0 : disabled ? 1 : 2));
        dest.writeString(fcnId);
        if (levelOfStudy == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(levelOfStudy);
        }
        dest.writeString(enrollmentDate);
        dest.writeString(remarks);
    }

    @Override
    public int describeContents() {
        return 0;
    }

    public static final Creator<Beneficiary> CREATOR = new Creator<Beneficiary>() {
        @Override
        public Beneficiary createFromParcel(Parcel in) {
            return new Beneficiary(in);
        }

        @Override
        public Beneficiary[] newArray(int size) {
            return new Beneficiary[size];
        }
    };

    public Integer getEntityId() {
        return entityId;
    }

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
    @SerializedName("fcnId")
    @Expose
    private String fcnId;
    @SerializedName("levelOfStudy")
    @Expose
    private Integer levelOfStudy;
    @SerializedName("enrollmentDate")
    @Expose
    private String enrollmentDate;

    @SerializedName("remarks")
    @Expose
    private String remarks;

    public Beneficiary(){

    }

    public Beneficiary(int collectionStatus){
        this.collectionStatus = collectionStatus;
    }

    public Beneficiary(Integer entityId,Boolean isActive){
        this.entityId = entityId;
        this.isActive = isActive;
    }

    public Beneficiary(Integer entityId, Boolean isActive, Integer facilityId, String beneficiaryName, String unhcrId,
                       String fatherName, String motherName, String fcnId, String dateOfBirth, Integer sex, Boolean disabled,
                       Integer levelOfStudy, String enrollmentDate, Integer facilityCampId, Integer beneficiaryCampId,
                       Integer blockId, Integer subBlockId, String remarks, Integer collectionStatus)
    {
        this.isActive = isActive;
        this.unhcrId = unhcrId;
        this.entityId = entityId;
        this.beneficiaryName = beneficiaryName;
        this.facilityId = facilityId;
        this.facilityCampId = facilityCampId;
        this.beneficiaryCampId = beneficiaryCampId;
        this.blockId = blockId;
        this.subBlockId = subBlockId;
        this.collectionStatus = collectionStatus;
        this.fatherName = fatherName;
        this.motherName = motherName;
        this.dateOfBirth = dateOfBirth;
        this.sex = sex;
        this.disabled = disabled;
        this.fcnId = fcnId;
        this.levelOfStudy = levelOfStudy;
        this.enrollmentDate = enrollmentDate;
        this.remarks = remarks;
    }

    public String getUnhcrId() {
        return unhcrId;
    }

    public void setUnhcrId(String unhcrId) {
        this.unhcrId = unhcrId;
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

    public Integer getFacilityId() {
        return facilityId;
    }

    public void setFacilityId(Integer facilityId) {
        this.facilityId = facilityId;
    }

    public Boolean getActive() {
        return isActive;
    }

    public void setActive(Boolean active) {
        isActive = active;
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

    public List<PropertiesInfoModel> getIndicators() {
        return indicators;
    }

    public void setIndicators(List<PropertiesInfoModel> indicators) {
        this.indicators = indicators;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
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

    public String getFcnId() {
        return fcnId;
    }

    public void setFcnId(String fcnId) {
        this.fcnId = fcnId;
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

    public String getRemarks() {
        return remarks;
    }

    public void setRemarks(String remarks) {
        this.remarks = remarks;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getInstanceId() {
        return instanceId;
    }

    public void setInstanceId(int instanceId) {
        this.instanceId = instanceId;
    }

    public String getCollectionStatusText(){
//        if(!this.isActive){
//            return "Inactive";
//        }
        CollectionStatus status = CollectionStatus.fromInt(collectionStatus);
        switch (status){
            case NotCollected:
                return "Not Collected";
            case Collected:
                return "Collected";
            case Approved:
                return "Approved";
            case Recollect:
                return "Recollect";

            case Requested_Inactive:
                return "Req. Inactive";
        }
        return "No Status";
    }


}