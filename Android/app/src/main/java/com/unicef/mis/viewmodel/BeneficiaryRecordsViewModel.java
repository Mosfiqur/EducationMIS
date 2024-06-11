package com.unicef.mis.viewmodel;

import android.content.Context;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.R;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.util.UnicefApplication;

public class BeneficiaryRecordsViewModel extends ViewModel {

    public Context context;
    public MutableLiveData <String> statusText = new MutableLiveData<String>("");
    public MutableLiveData <Beneficiary> beneficiary = new MutableLiveData<Beneficiary>();
    public MutableLiveData <Boolean> isActive = new MutableLiveData<Boolean>();
    public MutableLiveData <Integer> totalCount = new MutableLiveData<Integer>(0);
    public MutableLiveData<Integer> operationMode = new MutableLiveData<Integer>();

    public BeneficiaryRecordsViewModel() {
        context = UnicefApplication.getAppContext();
    }

    public void setBeneficiary(Beneficiary beneficiary) {
        this.beneficiary.setValue(beneficiary);
        this.isActive.setValue(beneficiary.getActive());
        String statusTxt = isActive.getValue() ? UnicefApplication.getResourceString(R.string.deactive) : UnicefApplication.getResourceString(R.string.active);
        this.statusText.setValue(statusTxt);
    }

    public void setTotalCount(int totalCount) {
        this.totalCount.setValue(totalCount);
    }

    public void prepareView(int operationMode) {
        this.operationMode.setValue(operationMode);
    }
}
