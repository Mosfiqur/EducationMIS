package com.unicef.mis.api;

import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.facility.indicator.FacilityIndicatorModel;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

import static com.unicef.mis.constants.APIConstants.FACILITY_GET_BY_ID;
import static com.unicef.mis.constants.APIConstants.FACILITY_INDICATOR;
import static com.unicef.mis.constants.APIConstants.FACILITY_INDICATOR_BY_INSTANCE;
import static com.unicef.mis.constants.APIConstants.FACILITY_INDICATOR_POST;
import static com.unicef.mis.constants.APIConstants.GET_ALL_FACILITIES;
import static com.unicef.mis.constants.APIConstants.GET_ALL_FACILITIES_BENEFICIARIES;
import static com.unicef.mis.constants.APIConstants.GET_FACILITY_RECORDS;
import static com.unicef.mis.constants.APIConstants.RUNNING_INSTANCES;

public interface FacilityApi {
    @GET(RUNNING_INSTANCES)
    Call<Schedule> getSchedule(
            @Header("Authorization") String contentRange,
            @Query("scheduleFor") String scheduleFor
    );

    @GET(FACILITY_INDICATOR)
    Call<FacilityIndicatorModel> getFacilityIndicator(
            @Header("Authorization") String contentRange,
            @Query("InstanceId") int InstanceId,
            @Query("FacilityId") int FacilityId
    );

    @GET(GET_ALL_FACILITIES)
    Call<PagedResponse<FacilityListDatum>> getFacilityList(
            @Header("Authorization") String contentRange,
            @Query("instanceId") int instanceId
    );

    @GET(GET_ALL_FACILITIES_BENEFICIARIES)
    Call<PagedResponse<FacilityListDatum>> getFacilityPaginatedList(
            @Header("Authorization") String contentRange,
            @Query("instanceId") int instanceId,
            @Query ("PageNo") int PageNo,
            @Query ("PageSize") int PageSize,
            @Query ("SearchText") String SearchText
    );

    @POST(FACILITY_INDICATOR_POST)
    Call<Void> uploadFacilityCollectedRecords(
            @Header("Authorization") String contentRange,
            @Body FacilityPost facilityPost
    );

    @GET(FACILITY_INDICATOR_BY_INSTANCE)
    Call<PagedResponse<IndicatorNew>> getFacilityIndicatorByInstace(
            @Header("Authorization") String contentRange,
            @Query("InstanceId") int InstanceId
    );

    @GET(GET_FACILITY_RECORDS)
    Call<PagedResponse<FacilityListDatum>> getFacilityRecords(
            @Header("Authorization") String contentRange,
            @Query("InstanceId") int InstanceId,
            @Query ("PageSize") int PageSize,
            @Query ("PageNo") int PageNo,
            @Query ("SearchText") String SearchText
    );

    @GET(FACILITY_GET_BY_ID)
    Call<FacilityListDatum> getFacilityGetById(
            @Header("Authorization") String contentRange,
            @Path("id") int id,
            @Path("instanceId") int instanceId
    );
}
