package com.unicef.mis.model.facility;

public class FacilityQuestionModel {
    String header;
    long facility_id;
    String targeted_population, status, facility_status;

    public FacilityQuestionModel(String header) {
        this.header = header;
    }

    public FacilityQuestionModel(String header, long facility_id, String targeted_population, String status, String facility_status) {
        this.header = header;
        this.facility_id = facility_id;
        this.targeted_population = targeted_population;
        this.status = status;
        this.facility_status = facility_status;
    }

    public String getHeader() {
        return header;
    }

    public void setHeader(String header) {
        this.header = header;
    }

    public long getFacility_id() {
        return facility_id;
    }

    public void setFacility_id(long facility_id) {
        this.facility_id = facility_id;
    }

    public String getTargeted_population() {
        return targeted_population;
    }

    public void setTargeted_population(String targeted_population) {
        this.targeted_population = targeted_population;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getFacility_status() {
        return facility_status;
    }

    public void setFacility_status(String facility_status) {
        this.facility_status = facility_status;
    }
}
