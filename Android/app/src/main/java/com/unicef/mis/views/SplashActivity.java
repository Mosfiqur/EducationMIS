package com.unicef.mis.views;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.os.Handler;
import android.view.Window;
import android.view.WindowManager;

import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;

import com.unicef.mis.BuildConfig;
import com.unicef.mis.R;
import com.unicef.mis.databinding.ActivitySplashBinding;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.SplashViewModel;
import com.unicef.mis.views.auth.LoginActivity;

public class SplashActivity extends BaseActivity {

    private SharedPreferences sharedPreferences;
    private AppCompatTextView version;

    private ActivitySplashBinding binding;
    private SplashViewModel viewModel;
    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_FULLSCREEN, WindowManager.LayoutParams.FLAG_FULLSCREEN);
        binding = DataBindingUtil.setContentView (this, R.layout.activity_splash);
        viewModel = new ViewModelProvider(this). get(SplashViewModel.class);
        binding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getApplicationContext());

        initViews();
        initListeners();
    }

    public void initViews(){
        sharedPreferences=getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
    }

    public void initListeners(){

        if (sharedPreferences.getString(ApplicationConstants.TOKEN, "").isEmpty()){
            loginMethod();
        }else {
            mainMethod();
        }


    }

    public void loginMethod(){
        new Handler().postDelayed(() -> {
            Intent intent = new Intent(SplashActivity.this, LoginActivity.class);
            startActivity(intent);
            finish();
        }, ApplicationConstants.APP_LOAD_TIME);

    }

    public void mainMethod(){
        new Handler().postDelayed(() -> {
            Intent intent = new Intent(SplashActivity.this, MainActivity.class);
            startActivity(intent);
            finish();
        }, ApplicationConstants.APP_LOAD_TIME);

    }


}
