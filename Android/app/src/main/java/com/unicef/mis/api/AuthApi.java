package com.unicef.mis.api;

import com.unicef.mis.model.auth.LoginBody;
import com.unicef.mis.model.auth.LoginModel;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;

import static com.unicef.mis.constants.APIConstants.AUTH;

public interface AuthApi {


    @POST(AUTH)
    Call<LoginModel> login(@Body LoginBody loginBody);
}
