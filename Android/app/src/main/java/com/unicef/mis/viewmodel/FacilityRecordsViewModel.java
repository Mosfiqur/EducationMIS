package com.unicef.mis.viewmodel;

import android.content.Context;
import android.content.Intent;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.views.FacilityListFragment;

public class FacilityRecordsViewModel extends ViewModel {
    public Context context;
    public MutableLiveData<FacilityListDatum> facility = new MutableLiveData<>();
    public MutableLiveData<String> btnName = new MutableLiveData<>();
    public MutableLiveData<Integer> operationMode = new MutableLiveData<Integer>();

    public FacilityRecordsViewModel() {
        context = UnicefApplication.getAppContext();
    }

    public void goBack(){
        UnicefApplication.getAppContext().startActivity(new Intent(UnicefApplication.getAppContext(), FacilityListFragment.class));
    }

    public void setFacility(FacilityListDatum facility){
        this.facility.setValue(facility);
    }


}
