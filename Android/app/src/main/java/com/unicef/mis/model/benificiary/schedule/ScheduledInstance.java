package com.unicef.mis.model.benificiary.schedule;

import android.os.Parcel;
import android.os.Parcelable;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ScheduledInstance implements Parcelable {

    @SerializedName("id")
    @Expose
    private Integer id;
    @SerializedName("scheduleId")
    @Expose
    private Integer scheduleId;
    @SerializedName("title")
    @Expose
    private String title;
    @SerializedName("dataCollectionDate")
    @Expose
    private String dataCollectionDate;
    @SerializedName("status")
    @Expose
    private Integer status;
    @SerializedName("endDate")
    @Expose
    private String endDate;

    public ScheduledInstance(){

    }

    public ScheduledInstance(Integer id, Integer scheduleId, String title, String dataCollectionDate, String endDate, Integer status) {
        this.id = id;
        this.scheduleId = scheduleId;
        this.title = title;
        this.dataCollectionDate = dataCollectionDate;
        this.endDate = endDate;
        this.status = status;
    }

    protected ScheduledInstance(Parcel in) {
        if (in.readByte() == 0) {
            id = null;
        } else {
            id = in.readInt();
        }
        if (in.readByte() == 0) {
            scheduleId = null;
        } else {
            scheduleId = in.readInt();
        }
        title = in.readString();
        dataCollectionDate = in.readString();
        if (in.readByte() == 0) {
            status = null;
        } else {
            status = in.readInt();
        }
    }

    public static final Creator<ScheduledInstance> CREATOR = new Creator<ScheduledInstance>() {
        @Override
        public ScheduledInstance createFromParcel(Parcel in) {
            return new ScheduledInstance(in);
        }

        @Override
        public ScheduledInstance[] newArray(int size) {
            return new ScheduledInstance[size];
        }
    };

    public Integer getId() {
        return id;
    }

    public void setId(Integer id) {
        this.id = id;
    }

    public Integer getScheduleId() {
        return scheduleId;
    }

    public void setScheduleId(Integer scheduleId) {
        this.scheduleId = scheduleId;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getDataCollectionDate() {
        return dataCollectionDate;
    }

    public void setDataCollectionDate(String dataCollectionDate) {
        this.dataCollectionDate = dataCollectionDate;
    }

    public Integer getStatus() {
        return status;
    }

    public void setStatus(Integer status) {
        this.status = status;
    }

    public String getEndDate() {
        return endDate;
    }

    public void setEndDate(String endDate) {
        this.endDate = endDate;
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
        if (id == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(id);
        }
        if (scheduleId == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(scheduleId);
        }
        dest.writeString(title);
        dest.writeString(dataCollectionDate);
        if (status == null) {
            dest.writeByte((byte) 0);
        } else {
            dest.writeByte((byte) 1);
            dest.writeInt(status);
        }
    }
}