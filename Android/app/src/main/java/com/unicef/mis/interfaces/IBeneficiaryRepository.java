package com.unicef.mis.interfaces;

import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.Promise;

public interface IBeneficiaryRepository {
    Promise getBeneficiariesByFacility(int facilityId, int instanceId, QueryParamModel queryParam);

    Promise getRecords(int instanceId, Beneficiary beneficiary);

    Promise saveRecords(BeneficiaryPost beneficiaryPost);

    Promise changeActiveStatus(boolean activeStatus, Beneficiary beneficiary);

    Promise uploadBeneficiaries(int instanceId, int facilityId);

    Promise beneficiaryGetById(int id, int instanceId);

    Promise createBeneficiary(Beneficiary beneficiary);
}
