package com.unicef.mis.viewmodel;

import android.content.ContentValues;
import android.content.Context;

import androidx.lifecycle.ViewModel;

import com.unicef.mis.util.UnicefApplication;

public class SplashViewModel extends ViewModel{

    private Context context;

    public SplashViewModel() {
        context = UnicefApplication.getAppContext();
    }


}
