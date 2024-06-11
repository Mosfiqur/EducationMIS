package com.unicef.mis.viewmodel;

import android.content.Context;

import androidx.databinding.ObservableBoolean;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.util.UnicefApplication;

public class SelectionViewModel extends ViewModel {

    public ObservableBoolean hasRecord = new ObservableBoolean(true);

    public Context context;
    public MutableLiveData<String> header = new MutableLiveData<>();

    public SelectionViewModel() {
        context = UnicefApplication.getAppContext();
    }
}
