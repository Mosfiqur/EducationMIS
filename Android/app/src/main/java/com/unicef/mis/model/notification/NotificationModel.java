package com.unicef.mis.model.notification;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;
import com.unicef.mis.model.PagedResponse;

public class NotificationModel {
    @SerializedName("notificationTypeId")
    @Expose
    private int notificationTypeId;

    @SerializedName("actor")
    @Expose
    private int actor;

    @SerializedName("user")
    @Expose
    private int user;

    @SerializedName("uri")
    @Expose
    private String uri;

    @SerializedName("data")
    @Expose
    private String data;

    @SerializedName("details")
    @Expose
    private String details;

    @SerializedName("isActed")
    @Expose
    private boolean isActed;

    @SerializedName("isDeleted")
    @Expose
    private boolean isDeleted;

    @SerializedName("notificationType")
    @Expose
    private String notificationType;

    @SerializedName("id")
    @Expose
    private int id;

    @SerializedName("dateCreated")
    @Expose
    private String dateCreated;

    @SerializedName("createdBy")
    @Expose
    private int createdBy;

    @SerializedName("lastUpdated")
    @Expose
    private String lastUpdated;

    @SerializedName("updatedBy")
    @Expose
    private int updatedBy;

    public NotificationModel() {
    }

    public int getNotificationTypeId() {
        return notificationTypeId;
    }

    public void setNotificationTypeId(int notificationTypeId) {
        this.notificationTypeId = notificationTypeId;
    }

    public int getActor() {
        return actor;
    }

    public void setActor(int actor) {
        this.actor = actor;
    }

    public int getUser() {
        return user;
    }

    public void setUser(int user) {
        this.user = user;
    }

    public String getUri() {
        return uri;
    }

    public void setUri(String uri) {
        this.uri = uri;
    }

    public String getData() {
        return data;
    }

    public void setData(String data) {
        this.data = data;
    }

    public String getDetails() {
        return details;
    }

    public void setDetails(String details) {
        this.details = details;
    }

    public boolean isActed() {
        return isActed;
    }

    public void setActed(boolean acted) {
        isActed = acted;
    }

    public boolean isDeleted() {
        return isDeleted;
    }

    public void setDeleted(boolean deleted) {
        isDeleted = deleted;
    }

    public String getNotificationType() {
        return notificationType;
    }

    public void setNotificationType(String notificationType) {
        this.notificationType = notificationType;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getDateCreated() {
        return dateCreated;
    }

    public void setDateCreated(String dateCreated) {
        this.dateCreated = dateCreated;
    }

    public int getCreatedBy() {
        return createdBy;
    }

    public void setCreatedBy(int createdBy) {
        this.createdBy = createdBy;
    }

    public String getLastUpdated() {
        return lastUpdated;
    }

    public void setLastUpdated(String lastUpdated) {
        this.lastUpdated = lastUpdated;
    }

    public int getUpdatedBy() {
        return updatedBy;
    }

    public void setUpdatedBy(int updatedBy) {
        this.updatedBy = updatedBy;
    }
}
