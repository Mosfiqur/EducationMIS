package com.unicef.mis.views.auth;

import android.os.Bundle;

import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;

import com.unicef.mis.R;
import com.unicef.mis.databinding.ActivityLoginBinding;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.LoginViewModel;


public class LoginActivity extends BaseActivity {

    ActivityLoginBinding activityMainBinding;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        activityMainBinding = DataBindingUtil.setContentView(this, R.layout.activity_login);

        LoginViewModel loginViewModel =new ViewModelProvider(this).get(LoginViewModel.class);
        activityMainBinding.setUserLogin(loginViewModel);

        Singleton.getInstance().setContext(getApplicationContext());
    }
}
