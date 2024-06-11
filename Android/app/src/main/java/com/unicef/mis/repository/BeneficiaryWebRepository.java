package com.unicef.mis.repository;

import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.RetrofitService;

public class BeneficiaryWebRepository {
    //Use this service to call web apis
    private final BeneficiaryApi service;

    public BeneficiaryWebRepository(){
        service = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);
    }

    /**
     * All web api calls should be implemented here
     * */
}
