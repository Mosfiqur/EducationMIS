package com.unicef.mis.views.offline_data.facility;

import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.unicef.mis.R;
import com.unicef.mis.adapter.OfflineFacilityIndicatorAdapter;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.interfaces.IFacilityFinalListner;
import com.unicef.mis.model.BeneficiaryIndicator;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

public class OfflineFacilityRecordsActivity extends BaseActivity implements View.OnClickListener, IFacilityFinalListner {
    private AppCompatTextView header;
    private RecyclerView questioner_recycler;
    private AppCompatImageView back;

    private AppCompatTextView header_tv;
    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv, save_all_tv;

    private ShimmerFrameLayout mShimmerViewContainer;

    private OfflineFacilityIndicatorAdapter indicatorAdapter;
    private LinearLayoutManager linearLayoutManager;

    private OfflineFacilityRecordsActivity listener;
    private SQLiteDatabaseHelper db;
    private ArrayList<BeneficiaryIndicator> indicators;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.questioner_app_bar_main);
        Singleton.getInstance().setContext(getApplicationContext());

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

    public void initViews(){
        db = SQLiteDatabaseHelper.getInstance(getApplicationContext());
        FacilityDataAccess facilityDataAccess = db.getFacilityDataAccess();
        indicators = new ArrayList<>(facilityDataAccess.getFacilityIndicatorList(Singleton.getInstance().getIdInstance(),
                Singleton.getInstance().getFacilityId()));


        questioner_recycler = findViewById (R.id.questioner_recycler);
        header_tv = findViewById (R.id.header_tv);
        back = findViewById (R.id.back);
        save_all_tv = findViewById (R.id.save_all_tv);
        if(save_all_tv != null){
            save_all_tv.setText(getResources().getString(R.string.save));
            save_all_tv.setOnClickListener(v -> {
                saveAllRecords();
            });
        }

        mShimmerViewContainer = findViewById (R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() ==1){
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        header = findViewById (R.id.app_title);

        no_content_iv = findViewById (R.id.no_content_iv);
        no_content_tv = findViewById (R.id.no_content_tv);
    }

    public void initListeners(){
        header.setText(getResources().getString(R.string.app_name));
        header_tv.setText("Total"+" "+ indicators.size()+" "+"Question");

        if (indicators.size() <= 0){
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);

            mShimmerViewContainer.stopShimmerAnimation();
            mShimmerViewContainer.setVisibility(View.GONE);
        }
        else {
            no_content_iv.setVisibility(View.INVISIBLE);
            no_content_tv.setVisibility(View.INVISIBLE);
            indicatorAdapter = new OfflineFacilityIndicatorAdapter(indicators, getApplicationContext(), listener);
            linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
            questioner_recycler.setAdapter(indicatorAdapter);
            questioner_recycler.setLayoutManager(linearLayoutManager);
            questioner_recycler.setNestedScrollingEnabled(false);
        }

        back.setOnClickListener(this);
    }


    @Override
    public void onClick(View view) {
        switch (view.getId()){
            case R.id.back:
                finish();
                break;
        }
    }

    private void saveAllRecords() {
        dialogUtil.showProgressDialog("Saving Facility Indicator...");
        ArrayList<BeneficiaryIndicator> changedValues = indicatorAdapter.getChangedValues();

        for (BeneficiaryIndicator indicator:changedValues){
            if(indicator.getValues() == null || indicator.getValues().size() == 0){
                continue;
            }


            db.getFacilityDataAccess().updateFacilityRecord(indicator,new IGenericApiCallBack() {

                @Override
                public void apiCallSuccessful(Object identifier, Object o) {
                    dialogUtil.dismissProgress();


                }

                @Override
                public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                    dialogUtil.dismissProgress();
                }
            });
        }
        showToast(UnicefApplication.getAppContext(), UnicefApplication.getResourceString(R.string.data_save_success));
    }

    @Override
    public void collectData(String strValue, BeneficiaryIndicator indicator) {
        List<String> values = new ArrayList<String>();
        values.add(strValue);
        indicator.setValues(values);
        indicator.setFacilityId(Singleton.getInstance().getFacilityId());
        indicator.setInstanceId(Singleton.getInstance().getIdInstance());

        FacilityDataAccess facilityDataAccess = db.getFacilityDataAccess();
        dialogUtil.showProgressDialog();
        facilityDataAccess.updateFacilityRecord(indicator, new IGenericApiCallBack() {
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
