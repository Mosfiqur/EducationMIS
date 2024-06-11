package com.unicef.mis.views.offline_data.beneficiary;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.unicef.mis.R;
import com.unicef.mis.adapter.OfflineBeneficiaryIndicatorAdapter;
import com.unicef.mis.dataaccess.BeneficiaryActiveStatusDataAccess;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.databinding.ActivityBeneficiaryFinalListBinding;
import com.unicef.mis.listner.IBeneficiaryDataInsert;
import com.unicef.mis.model.PropertiesInfoModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryDynamicCell;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.BeneficiaryRecordsViewModel;
import com.unicef.mis.views.MainActivity;

import java.util.ArrayList;
import java.util.List;


public class OfflineBeneficiaryRecordsActivity extends BaseActivity implements View.OnClickListener, IBeneficiaryDataInsert {

    AppCompatTextView layer2_header_value_tv, layer3_header_value_tv;
    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv, status_tv;
    private AppCompatTextView save_all_tv;
    private OfflineBeneficiaryIndicatorAdapter indicatorAdapter;
    private LinearLayoutManager linearLayoutManager;
    private RecyclerView questioner_group_recycler;


    private ShimmerFrameLayout mShimmerViewContainer;

    private OfflineBeneficiaryRecordsActivity listener;
    private BeneficiaryDynamicCell beneficiaryDynamicCell;
    private BeneficiaryPost benificaryPost;

    private SQLiteDatabaseHelper db;
    private ArrayList<PropertiesInfoModel> indicators;
    private BeneficiaryActiveStatusDataAccess activeDataAccess;

    ActivityBeneficiaryFinalListBinding binding;
    BeneficiaryRecordsViewModel viewModel;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_beneficiary_final_list);
//        binding = DataBindingUtil.setContentView(this, R.layout.activity_beneficiary_final_list);
//        viewModel = new ViewModelProvider(this).get(OfflineIndicatorViewModel.class);
//        binding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getApplicationContext());

        db = SQLiteDatabaseHelper.getInstance(getApplicationContext());
        listener = this;

        initViews();
        initListeners();
    }

    @Override
    public void onResume() {
        Log.e("DEBUG", "onResume of HomeFragment");
        super.onResume();
        mShimmerViewContainer.startShimmerAnimation();
    }

    @Override
    public void onPause() {
        Log.e("DEBUG", "OnPause of HomeFragment");
        super.onPause();
    }

    public void goBack(View view) {
        if (view.getId() == R.id.back) {
            finish();
        }
    }

    public void initViews() {
        BeneficiaryDataAccess beneficiaryDataAccess = db.getBeneficiaryDataAccess();

        status_tv = findViewById (R.id.status_tv);
        int beneficiaryId = Singleton.getInstance().getBeneficiaryId();
        activeDataAccess = db.getActiveDataAccess();
        if (activeDataAccess.isActiveStatus(beneficiaryId).equals("false")){
            status_tv.setText("Make InActive");
            status_tv.setBackgroundDrawable(getResources().getDrawable(R.drawable.not_collected_rectangle));
        } else {
            status_tv.setText("Make Active");
            status_tv.setBackgroundDrawable(getResources().getDrawable(R.drawable.collected_rectangle));
        }

        status_tv.setOnClickListener(this);

        indicators = new ArrayList<>(beneficiaryDataAccess.getBeneficiaryIndicators(Singleton.getInstance().getIdInstance(), Singleton.getInstance().getBeneficiaryId()));

        layer2_header_value_tv = findViewById (R.id.tv_facility_code);
        layer3_header_value_tv = findViewById (R.id.layer3_header_value_tv);

        layer2_header_value_tv.setText(Singleton.getInstance().getUnhcrId());
        layer3_header_value_tv.setText(Singleton.getInstance().getBenificiaryName());

        no_content_iv = findViewById(R.id.no_content_iv);
        no_content_tv = findViewById(R.id.no_content_tv);
        questioner_group_recycler = findViewById(R.id.questioner_group_recycler);

        mShimmerViewContainer = findViewById(R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() == 1) {
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        save_all_tv = findViewById(R.id.save_all_tv);
        if(save_all_tv != null){
            save_all_tv.setText(getResources().getString(R.string.save));
            save_all_tv.setOnClickListener(v -> {
                saveAllRecords();
            });
        }
    }

    private void saveAllRecords() {
        ArrayList<PropertiesInfoModel> changedValues = indicatorAdapter.getChangedValues();
        dialogUtil.showProgressDialog();
        for (PropertiesInfoModel indicator:changedValues){
            if(indicator.getValues() == null || indicator.getValues().size() == 0){
                continue;
            }

            db.getBeneficiaryDataAccess().saveBeneficiaryIndicatorData(indicator, new IGenericApiCallBack() {
                @Override
                public void apiCallSuccessful(Object identifier, Object o) {
                    Toast.makeText(getApplicationContext(), "Data Saved Successfully", Toast.LENGTH_SHORT).show();
                }

                @Override
                public void apiCallFailed(boolean hasSpecificError, String errorMessage) {

                }
            });
        }
        dialogUtil.dismissProgress();
    }

    public void initListeners() {
        if (indicators.size() <= 0) {
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);
        } else {
            no_content_iv.setVisibility(View.INVISIBLE);
            no_content_tv.setVisibility(View.INVISIBLE);
            indicatorAdapter = new OfflineBeneficiaryIndicatorAdapter(indicators, getApplicationContext(), listener);
            linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
            questioner_group_recycler.setAdapter(indicatorAdapter);
            questioner_group_recycler.setLayoutManager(linearLayoutManager);
        }

        // Stopping Shimmer Effect's animation after data is loaded to ListView
        mShimmerViewContainer.stopShimmerAnimation();
        mShimmerViewContainer.setVisibility(View.GONE);

    }

    @Override
    public void onClick(View view) {
        switch (view.getId()){
            case R.id.status_tv:
                dialogUtil.showDialogYesNo(getResources().getString(R.string.deactive_string), (dialog, id) -> {
                    if (activeDataAccess.isActiveStatus(Singleton.getInstance().getBeneficiaryId()).equals("false")){
                        IGenericApiCallBack callBack = new IGenericApiCallBack() {
                            @Override
                            public void apiCallSuccessful(Object identifier, Object o) {
                                Intent i = new Intent(OfflineBeneficiaryRecordsActivity.this, MainActivity.class);
                                showToast(UnicefApplication.getAppContext(), "Beneficiary disabled");
                                startActivity(i);
                            }

                            @Override
                            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                                showToast(UnicefApplication.getAppContext(), "Something went wrong");
                            }
                        };
                        activeDataAccess.updateBeneficiaryStatus(Singleton.getInstance().getBeneficiaryId(), true, callBack);
                    } else {
                        IGenericApiCallBack callBack = new IGenericApiCallBack() {
                            @Override
                            public void apiCallSuccessful(Object identifier, Object o) {
                                Intent i = new Intent(OfflineBeneficiaryRecordsActivity.this, MainActivity.class);
                                showToast(UnicefApplication.getAppContext(), "Beneficiary active");
                                startActivity(i);
                            }

                            @Override
                            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                                showToast(UnicefApplication.getAppContext(), "Something went wrong");
                            }
                        };
                        activeDataAccess.updateBeneficiaryStatus(Singleton.getInstance().getBeneficiaryId(), false, callBack);
                    }

                }, (dialog, id) -> dialog.dismiss());

                break;

        }
    }

    @Override
    public void collectData(String strValue, PropertiesInfoModel indicator) {
        List<String> values = new ArrayList<String>();
        values.add(strValue);
        indicator.setValues(values);
        //TODO: Check Later
/*
        indicator.setBeneficiaryId(Singleton.getInstance().getBenificiaryId());
        indicator.setInstanceId(Singleton.getInstance().getIdInstance());
*/
        BeneficiaryDataAccess beneficiaryDataAccess = db.getBeneficiaryDataAccess();
        dialogUtil.showProgressDialog();
        beneficiaryDataAccess.saveBeneficiaryIndicatorData(indicator, new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                dialogUtil.dismissProgress();
                saveAllRecords();
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                dialogUtil.dismissProgress();
            }
        });
    }
}

