package com.unicef.mis.viewmodel;

import android.content.Context;

import androidx.lifecycle.ViewModel;

import com.unicef.mis.util.UnicefApplication;

public class AddCampViewModel extends ViewModel {

    public Context context;

    public AddCampViewModel() {
        context = UnicefApplication.getAppContext();
    }
}
