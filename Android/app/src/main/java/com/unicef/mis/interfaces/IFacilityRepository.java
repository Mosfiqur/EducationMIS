package com.unicef.mis.interfaces;

import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;
import com.unicef.mis.util.Promise;

public interface IFacilityRepository {
    Promise searchFacilitiesForBeneficiary(int instanceId, QueryParamModel queryParam);
    Promise searchFacilitiesByInstance(int instanceId, QueryParamModel queryParam);
    Promise getRecords(int instanceId, int facilityId);
    Promise saveRecords(FacilityPost records);
    Promise uploadFacilities(int instanceId);
    Promise facilityGetById(int id, int instanceId);
}
