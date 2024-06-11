package com.unicef.mis.repository;


import android.content.Context;
import android.content.SharedPreferences;

import com.unicef.mis.api.AuthApi;
import com.unicef.mis.dataaccess.UserDataAccess;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.model.auth.LoginBody;
import com.unicef.mis.model.auth.LoginModel;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.EMAIL;
import static com.unicef.mis.constants.ApplicationConstants.NETWORK_STATUS;
import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class UserRepository{

    private AuthApi authApi;
    private SharedPreferences sharedPreferences;
    private UserDataAccess dataAccess;

    //UserRepository Constructor
    public UserRepository(Context appContext) {
        authApi = RetrofitService.createService(AuthApi.class, APIClient.BASE_URL, true);
        sharedPreferences = appContext.getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        dataAccess = DataAccessFactory.getUserDataAccess();
    }

    public Promise authenticateOnline(String email, String password) {
        Promise promise = new Promise();
        LoginBody loginBody = new LoginBody(email, password);
        Call<LoginModel> callLogin = authApi.login(loginBody);
        callLogin.enqueue(new Callback<LoginModel>() {
            @Override
            public void onResponse(Call<LoginModel> call, Response<LoginModel> response) {
                if (response.isSuccessful()) {
                    if (response.code() == 200) {
                        LoginModel loginModel = response.body();
                        saveToSharedPreference(loginModel);
                        saveToLocal(loginModel);
                        promise.resolve(loginModel);
                    } else {
                        promise.reject("User email or password is incorrect");
                    }

                } else {
                    promise.reject("User email or password is incorrect");
                }
            }

            @Override
            public void onFailure(Call<LoginModel> call, Throwable t) {
                promise.reject(t.getMessage());
            }
        });

        return promise;
    }

    private void saveToLocal(LoginModel loginModel) {
        dataAccess.insertUser(loginModel.getUserProfile().getId(),
                loginModel.getUserProfile().getFullName(),
                loginModel.getUserProfile().getLevelId(),
                loginModel.getUserProfile().getLevelName(),
                loginModel.getUserProfile().getLevelRank(),
                loginModel.getUserProfile().getDesignationId(),
                loginModel.getUserProfile().getDesignationName(),
                loginModel.getUserProfile().getRoleId(),
                loginModel.getUserProfile().getRoleName(),
                loginModel.getUserProfile().getEmail(),
                String.valueOf(loginModel.getUserProfile().getPhoneNumber()),
                loginModel.getToken());
    }

    private void saveToSharedPreference(LoginModel loginModel) {
        SharedPreferences.Editor editor = sharedPreferences.edit();
        editor.putString(EMAIL, loginModel.getUserProfile().getEmail());
        editor.putString(TOKEN, loginModel.getToken());
        editor.putString(NETWORK_STATUS, "1");
        editor.commit();
    }

    public LoginModel authenticateOffline(String email, String password) {
        LoginModel loginModel = new LoginModel();
        //TODO: read shared preference whether email exists. If exists then read token. reject when no email.
        // Read saved user from SQLite
        return loginModel;
    }
}
