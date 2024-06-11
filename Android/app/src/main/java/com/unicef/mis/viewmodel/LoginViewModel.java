package com.unicef.mis.viewmodel;

import android.content.Context;
import android.content.Intent;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.R;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.model.auth.LoginModel;
import com.unicef.mis.model.auth.User;
import com.unicef.mis.repository.UserRepository;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.views.MainActivity;

import static com.unicef.mis.util.BaseActivity.dialogUtil;
import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;


public class LoginViewModel extends ViewModel {
    private UserRepository userRepository;

    public MutableLiveData<String> email = new MutableLiveData<>();
    public MutableLiveData<String> password = new MutableLiveData<>();

    public User user;
    public Context context;

    //Constructor viewmodel class
    public LoginViewModel() {
        user = new User();
        this.context = UnicefApplication.getAppContext();
        userRepository = RepositoryFactory.getUserRepository();
    }

    //method to check validation
    public void onLoginClicked() {
        if (isNetworkAvailable() == false){
            showToast(context, "No Internet Connection");
        } else {
            if (email.equals(null) || password.equals(null)){
                showToast(context, "Please Insert A Valid Email/Password");
            } else {
                user.setEmail(email.getValue());
                user.setPassword(password.getValue());
            }


            if (user.isValid()) {
                authenticateUser();
            } else {
                showToast(context, context.getResources().getString(R.string.valid_email));
            }
        }

    }

    //
    public void authenticateUser() {
        String email = this.email.getValue().trim();
        String password = this.password.getValue().trim();
        if (isNetworkAvailable()) {
            if (email.length() == 0 || password.length() ==0){
                showToast(context, "Please Insert Valid Email/Password");
            } else {
                dialogUtil.showProgressDialog();
                Promise promise = userRepository.authenticateOnline(email, password);
                promise.then(res -> gotoMainActivity((LoginModel) res))
                        .error(err -> {
                            dialogUtil.dismissProgress();
                            showError(err);
                        });
            }


        } else {
            dialogUtil.showProgressDialog();
            Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
                @Override
                public Object execute() {
                    return userRepository.authenticateOffline(email, password);
                }
            });
            promise.then(res -> gotoMainActivity((LoginModel) res))
                    .error(err -> {
                        dialogUtil.dismissProgress();
                        showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.username_password_not_match));
                    });
        }
    }

    //what happen method when everything is matched. email & password matched
    private boolean gotoMainActivity(LoginModel loginModel) {
        dialogUtil.dismissProgress();
        Intent i = new Intent(UnicefApplication.getAppContext(), MainActivity.class);

        showToast(UnicefApplication.getAppContext(), "Welcome " + loginModel.getUserProfile().getFullName());
        i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        UnicefApplication.getAppContext().startActivity(i);
        return true;
    }

}
