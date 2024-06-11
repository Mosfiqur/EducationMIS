package com.unicef.mis.api;

import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.CreateBeneficiaryModel;
import com.unicef.mis.model.DeactiveModel;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.indicator.BeneficiaryIndicatorModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.SubBlock;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Header;
import retrofit2.http.POST;
import retrofit2.http.Path;
import retrofit2.http.Query;

import static com.unicef.mis.constants.APIConstants.BENEFICIARY_GET_BY_ID;
import static com.unicef.mis.constants.APIConstants.GET_BENEFICIARY_RECORDS;
import static com.unicef.mis.constants.APIConstants.BENEFICIARY_INDICATOR;
import static com.unicef.mis.constants.APIConstants.BENEFICIARY_INDICATOR_POST;
import static com.unicef.mis.constants.APIConstants.CREATE_BENEFICIARY;
import static com.unicef.mis.constants.APIConstants.DEACTIVE_BENEFICIARY;
import static com.unicef.mis.constants.APIConstants.GET_ALL_FACILITIES_BENEFICIARIES;
import static com.unicef.mis.constants.APIConstants.GET_BLOCKS;
import static com.unicef.mis.constants.APIConstants.GET_CAMPS;
import static com.unicef.mis.constants.APIConstants.GET_SUB_BLOCKS;
import static com.unicef.mis.constants.APIConstants.RUNNING_INSTANCES;

public interface BeneficiaryApi {
    @GET(RUNNING_INSTANCES)
    Call<Schedule> getSchedule(
            @Header("Authorization") String contentRange,
            @Query("scheduleFor") String scheduleFor
    );

    @GET(GET_ALL_FACILITIES_BENEFICIARIES)
    Call<PagedResponse<FacilityListDatum>> getFacilityList(
            @Header("Authorization") String contentRange,
            @Query("instanceId") int instanceId
    );

    @GET(GET_BENEFICIARY_RECORDS)
    Call<PagedResponse<Beneficiary>> getBeneficiariesByFacility(
            @Header("Authorization") String contentRange,
            @Query("FacilityId") int FacilityId,
            @Query("InstanceId") int InstanceId,
            @Query("PageSize") int PageSize,
            @Query("PageNo") int PageNo,
            @Query("SearchText") String SearchText
    );

    @GET(BENEFICIARY_INDICATOR)
    Call<BeneficiaryIndicatorModel> getBenificiaryIndicator(
            @Header("Authorization") String contentRange,
            @Query("InstanceId") int InstanceId,
            @Query("BeneficiaryId") int BeneficiaryId
    );

    @POST(BENEFICIARY_INDICATOR_POST)
    Call<Void> uploadBeneficiaryCollectedRecords(
            @Header("Authorization") String contentRange,
            @Body BeneficiaryPost benificaryPost
    );

    @GET(GET_CAMPS)
    Call<PagedResponse<Camp>> getCamps(
            @Header("Authorization") String contentRange
    );

    @GET(GET_BLOCKS)
    Call<PagedResponse<Block>> getBlock(
            @Header("Authorization") String contentRange
    );

    @GET(GET_SUB_BLOCKS)
    Call<PagedResponse<SubBlock>> getSubBlocks(
            @Header("Authorization") String contentRange
    );

    @GET(GET_BLOCKS)
    Call<PagedResponse<Block>> getBlockByCampId(
            @Header("Authorization") String contentRange,
            @Query("campId") int campId
    );

    @GET(GET_SUB_BLOCKS)
    Call<PagedResponse<SubBlock>> getSubBlocksByBlock(
            @Header("Authorization") String contentRange,
            @Query("blockId") int block
    );

    @POST(CREATE_BENEFICIARY)
    Call<CreateBeneficiaryModel> createBeneficiary(
            @Header("Authorization") String contentRange,
            @Body CreateBeneficiaryModel beneficiary
    );

    @POST(CREATE_BENEFICIARY)
    Call<CreateBeneficiaryModel> createBeneficiaryOnline(
            @Header("Authorization") String contentRange,
            @Body Beneficiary beneficiary
    );

    @POST(DEACTIVE_BENEFICIARY)
    Call<Void> deactiveBeneficiary(
            @Header("Authorization") String contentRange,
            @Body DeactiveModel deactiveModel
    );

    @GET(BENEFICIARY_GET_BY_ID)
    Call<Beneficiary> getBeneficiaryGetById(
            @Header("Authorization") String contentRange,
            @Path("id") int id,
            @Path("instanceId") int instanceId
    );
}
