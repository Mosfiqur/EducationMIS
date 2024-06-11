package com.unicef.mis.views.benificiary;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;

import com.unicef.mis.R;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.databinding.ActivityBeneficiaryListBinding;
import com.unicef.mis.listner.IMoveToFinalBenificary;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.BeneficiaryViewModel;

public class BeneficiaryListActivity extends BaseActivity implements IMoveToFinalBenificary {
    ActivityBeneficiaryListBinding binding;
    BeneficiaryViewModel viewModel;
    public Context context = this;
    private FacilityListDatum facility;
    private int instanceId;
    private int operationMode;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_beneficiary_list);
        viewModel = new ViewModelProvider(this).get(BeneficiaryViewModel.class);
        viewModel.setContext(this);
        binding.setViewModel(viewModel);
        binding.setLifecycleOwner(this);
        Singleton.getInstance().setContext(getApplicationContext());

        Intent intent = getIntent();
        Bundle extras = intent.getExtras();

        if (extras.containsKey(UIConstants.KEY_OPERATION_MODE)){
            operationMode = extras.getInt(UIConstants.KEY_OPERATION_MODE);
        }
        if (extras.containsKey(UIConstants.INTENT_EXTRA_FACILITY)) {
            facility = (FacilityListDatum) extras.getParcelable(UIConstants.INTENT_EXTRA_FACILITY);
        }
        if(extras.containsKey(UIConstants.INTENT_EXTRA_INSTANCE_ID)){
            instanceId = extras.getInt(UIConstants.INTENT_EXTRA_INSTANCE_ID);
        }
        viewModel.prepareView(operationMode, instanceId, facility);
    }

    @Override
    public void onResume() {
        Log.e("DEBUG", "onResume of HomeFragment");
        super.onResume();
        viewModel.searchBeneficiaries();
    }

    @Override
    public void onPause() {
        Log.e("DEBUG", "OnPause of HomeFragment");
        super.onPause();
    }

    @Override
    protected void onRestart() {
        super.onRestart();
        finish();
        overridePendingTransition(0, 0);
        startActivity(getIntent());
        overridePendingTransition(0, 0);
    }

    public void goBack(View view){
        if (view.getId() == R.id.back){
            finish();
        }
    }

    @Override
    public void moveToNextPage(String id, int beneficiaryId, String name) {
        Singleton.getInstance().setUnhcrId(id);
        Singleton.getInstance().setBenificiaryName(name);
        Singleton.getInstance().setBeneficiaryId(beneficiaryId);

//        Intent i = new Intent(OnlineBeneficiaryListActivity.this, OnlineBeneficiaryRecordsActivity.class);
//        startActivity(i);

    }
}