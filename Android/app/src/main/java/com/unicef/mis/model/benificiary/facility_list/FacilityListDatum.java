package com.unicef.mis.model.benificiary.facility_list;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.model.PropertiesInfoModel;

import java.util.List;

public class FacilityListDatum implements Parcelable {

    @SerializedName("properties")
    @Expose
    private List<PropertiesInfoModel> properties;
    @SerializedName("id")
    @Expose
    private int id;
    @SerializedName("facilityName")
    @Expose
    private String facilityName;

    @SerializedName("facilityCode")
    @Expose
    private String facilityCode;

    @SerializedName("campId")
    @Expose
    private Integer campId;
    @SerializedName("campName")
    @Expose
    private String campName;
    @SerializedName("facilityTypeId")
    @Expose
    private Integer facilityTypeId;
    @SerializedName("blockId")
    @Expose
    private Integer blockId;
    @SerializedName("blockName")
    @Expose
    private String blockName;

    @SerializedName("paraId")
    @Expose
    private String paraId;

    @SerializedName("paraName")
    @Expose
    private String paraName;
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
    @SerializedName("teacherId")
    @Expose
    private Integer teacherId;
    @SerializedName("teacherName")
    @Expose
    private String teacherName;
    @SerializedName("collectionStatus")
    @Expose
    private int collectionStatus;

    @SerializedName("programPartnerName")
    @Expose
    private String programPartnerName;
    @SerializedName("implementationPartnerName")
    @Expose
    private String implementationPartnerName;
    @SerializedName("unionName")
    @Expose
    private String unionName;
    @SerializedName("upazilaName")
    @Expose
    private String upazilaName;


    public FacilityListDatum(){

    }

    public FacilityListDatum(int id, String facilityName, Integer camp_id, String camp_name, String programming_partner_name, String implementation_partner_name){
        this.id = id;
        this.facilityName = facilityName;
        this.campId = camp_id;
        this.campName = camp_name;
        this.programmingPartnerName = programming_partner_name;
        this.implemantationPartnerName = implementation_partner_name;
    }

    protected FacilityListDatum(Parcel in) {
        id = in.readInt();
        facilityName = in.readString();
        facilityCode = in.readString();
        if (in.readByte() == 0) {
            campId = null;
        } else {
            campId = in.readInt();
        }
        campName = in.readString();
        if (in.readByte() == 0) {
            facilityTypeId = null;
        } else {
            facilityTypeId = in.readInt();
        }
        if (in.readByte() == 0) {
            blockId = null;
        } else {
            blockId = in.readInt();
        }
        blockName = in.readString();
        paraId = in.readString();
        paraName = in.readString();
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
            teacherId = null;
        } else {
            teacherId = in.readInt();
        }
        teacherName = in.readString();
        collectionStatus = in.readInt();
    }

    public static final Creator<FacilityListDatum> CREATOR = new Creator<FacilityListDatum>() {
        @Override
        public FacilityListDatum createFromParcel(Parcel in) {
            return new FacilityListDatum(in);
        }

        @Override
        public FacilityListDatum[] newArray(int size) {
            return new FacilityListDatum[size];
        }
    };

    public List<PropertiesInfoModel> getProperties() {
        return properties;
    }

    public void setProperties(List<PropertiesInfoModel> properties) {
        this.properties = properties;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getFacilityName() {
        return facilityName;
    }

    public void setFacilityName(String facilityName) {
        this.facilityName = facilityName;
    }

    public Integer getCampId() {
        return campId;
    }

    public void setCampId(Integer campId) {
        this.campId = campId;
    }

    public String getCampName() {
        return campName;
    }

    public void setCampName(String campName) {
        this.campName = campName;
    }

    public Integer getFacilityTypeId() {
        return facilityTypeId;
    }

    public void setFacilityTypeId(Integer facilityTypeId) {
        this.facilityTypeId = facilityTypeId;
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

    public String getParaId() {
        return paraId;
    }

    public void setParaId(String paraId) {
        this.paraId = paraId;
    }

    public String getParaName() {
        return paraName;
    }

    public void setParaName(String paraName) {
        this.paraName = paraName;
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

    public Integer getTeacherId() {
        return teacherId;
    }

    public void setTeacherId(Integer teacherId) {
        this.teacherId = teacherId;
    }

    public String getTeacherName() {
        return teacherName;
    }

    public void setTeacherName(String teacherName) {
        this.teacherName = teacherName;
    }

    public String getFacilityCode() {
        return facilityCode;
    }

    public void setFacilityCode(String facilityCode) {
        this.facilityCode = facilityCode;
    }



    public void setId(Integer id) {
        this.id = id;
    }

    public String getProgramPartnerName() {
        return programPartnerName;
    }

    public void setProgramPartnerName(String programPartnerName) {
        this.programPartnerName = programPartnerName;
    }

    public String getImplementationPartnerName() {
        return implementationPartnerName;
    }

    public void setImplementationPartnerName(String implementationPartnerName) {
        this.implementationPartnerName = implementationPartnerName;
    }

    public String getUnionName() {
        return unionName;
    }

    public void setUnionName(String unionName) {
        this.unionName = unionName;
    }

    public String getUpazilaName() {
        return upazilaName;
    }

    public void setUpazilaName(String upazilaName) {
        this.upazilaName = upazilaName;
    }



    /**
     * Describe the kinds of special objects contained in this Parcelable
     * instance's marshaled representation. For example, if the object will
     * include a file descriptor in the output of {@link #writeToParcel(Parcel, int)},
     * the return value of this method must include the
     * {@link #CONTENTS_FILE_DESCRIPTOR} bit.
     *
     * @return a bitmask indicating the set of special object types marshaled
     * by this Parcelable object instance.
     */
    @Override
    public int describeContents() {
        return 0;
    }

    /**
     * Flatten this object in to a Parcel.
     *
     * @param dest  The Parcel in which the object should be written.
     * @param flags Additional flags about how the object should be written.
     *              May be 0 or {@link #PARCELABLE_WRITE_RETURN_VALUE}.
     */
    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeInt(id);
        dest.writeString(facilityName);
        dest.writeString(facilityCode);
        if (campId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(campId);
        }
        dest.writeString(campName);
        if (facilityTypeId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(facilityTypeId);
        }
        if (blockId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(blockId);
        }
        dest.writeString(blockName);
        dest.writeString(paraId);
        dest.writeString(paraName);
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
        if (teacherId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(teacherId);
        }
        dest.writeString(teacherName);
        dest.writeInt(collectionStatus);
    }

    public int getCollectionStatus() {
        return collectionStatus;
    }

    public void setCollectionStatus(int collectionStatus) {
        this.collectionStatus = collectionStatus;
    }

    public String getCollectionStatusText(){
        CollectionStatus status = CollectionStatus.fromInt(collectionStatus);
        switch (status){
            case NoStatus:
                return "No status";
            case NotCollected:
                return "Not Collected";
            case Collected:
                return "Collected";
            case Approved:
                return "Approved";
            case Recollect:
                return "Recollect";
        }
        return "No Status";
    }
}