package com.unicef.mis.views.upload.beneficiary;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.unicef.mis.R;
import com.unicef.mis.adapter.OfflineBeneficiaryIndicatorAdapter;
import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.listner.IBenificiaryFinalListner;
import com.unicef.mis.listner.IBeneficiaryDataInsert;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.PropertiesInfoModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryDynamicCell;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.model.benificiary.questionTest.QuestionModel;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class UploadBeneficiaryIndicator extends BaseActivity implements View.OnClickListener, IBeneficiaryDataInsert, IBenificiaryFinalListner {

    AppCompatTextView layer2_header_value_tv, layer3_header_value_tv;
    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv, status_tv;
    private OfflineBeneficiaryIndicatorAdapter indicatorAdapter;
    private ArrayList<QuestionModel> list;
    private LinearLayoutManager linearLayoutManager;
    private RecyclerView questioner_group_recycler;


    private ShimmerFrameLayout mShimmerViewContainer;

    private UploadBeneficiaryIndicator listener;
    private BeneficiaryDynamicCell beneficiaryDynamicCell;
    private BeneficiaryPost benificaryPost;

    private SQLiteDatabaseHelper db;
    private ArrayList<PropertiesInfoModel> indicators;

    public BeneficiaryApi benificaryApi;

    private String token ="";

    private SharedPreferences sharedPreferences;

    public ArrayList<Beneficiary> benificary;

    public AppCompatTextView save_all_tv;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_beneficiary_final_list);
        Singleton.getInstance().setContext(getApplicationContext());

        benificaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);

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
            Intent i = new Intent(getApplicationContext(), UploadBeneficiaryListActivity.class);
            db.getBeneficiaryDataAccess().updateCollectionStatus(Singleton.getInstance().getBenificiaryId());
            startActivity(i);
            finish();
        }
    }

    public void initViews() {

        sharedPreferences= getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
        status_tv = findViewById (R.id.status_tv);
        status_tv.setVisibility(View.GONE);

        save_all_tv = findViewById (R.id.save_all_tv);
        save_all_tv.setOnClickListener(this);

        db = SQLiteDatabaseHelper.getInstance(getApplicationContext());

        BeneficiaryDataAccess beneficiaryDataAccess = db.getBeneficiaryDataAccess();
        indicators = new ArrayList<PropertiesInfoModel>(beneficiaryDataAccess.getBeneficiaryIndicators(Singleton.getInstance().getIdInstance(), Singleton.getInstance().getBeneficiaryId()));

//        BeneficiaryDataAccess beneficiaryDataAccess = db.getBeneficiaryDataAccess();
//        Benificiary beneficiary = beneficiaryDataAccess.getBeneficiary(Singleton.getInstance().getIdInstance(), Singleton.getInstance().getBenificiaryId(), new IGenericApiCallBack() {
//            @Override
//            public void apiCallSuccessful(Object identifier, Object o) {
//
//            }
//
//            @Override
//            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
//
//            }
//        });
//        indicators = new ArrayList<BeneficiaryIndicator>();
//        indicators.addAll(beneficiary.getIndicators());
//        indicators = new ArrayList<>(beneficiaryDataAccess.getBeneficiaryIndicators(Singleton.getInstance().getIdInstance(), Singleton.getInstance().getId()));

        layer2_header_value_tv = findViewById(R.id.tv_facility_code);
        layer3_header_value_tv = findViewById(R.id.layer3_header_value_tv);

        layer2_header_value_tv.setText(String.valueOf( Singleton.getInstance().getUnhcrId() ));
        layer3_header_value_tv.setText(Singleton.getInstance().getBenificiaryName());

        no_content_iv = findViewById(R.id.no_content_iv);
        no_content_tv = findViewById(R.id.no_content_tv);
        questioner_group_recycler = findViewById(R.id.questioner_group_recycler);

        mShimmerViewContainer = findViewById(R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() == 1) {
            mShimmerViewContainer.setVisibility(View.GONE);
        }

    }

    public void initListeners() {
        if (indicators.size() <= 0) {
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);
        } else {
            no_content_iv.setVisibility(View.INVISIBLE);
            no_content_tv.setVisibility(View.INVISIBLE);
            indicatorAdapter = new OfflineBeneficiaryIndicatorAdapter(indicators, getApplicationContext(), listener, listener);
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
            case R.id.save_all_tv:
                if (isNetworkAvailable()){
                    callBeneficiaryIndicatorPost();
                } else {
                    showToast(UnicefApplication.getAppContext(), getResources().getString(R.string.no_internet));
                }

                break;
        }

    }

    @Override
    public void collectData(String strValue, PropertiesInfoModel indicator) {
        List<String> values = new ArrayList<String>();
        values.add(strValue);
        indicator.setValues(values);
        //TODO: Check
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
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                dialogUtil.dismissProgress();
            }
        });


    }

    public void callBeneficiaryIndicatorPost(){
        ArrayList<PropertiesInfoModel> indicators = indicatorAdapter.getChangedValues();
        ArrayList<BeneficiaryDynamicCell> beneficiaryDynamicCells = new ArrayList<>();
        beneficiaryDynamicCells.add(beneficiaryDynamicCell);

//       beneficiaryDynamicCell = new BeneficiaryDynamicCell(beneficiaryDynamicCells, indicators.get().getDynamicColumnId());
        benificaryPost = new BeneficiaryPost(Singleton.getInstance().getId(), Singleton.getInstance().getIdInstance(), beneficiaryDynamicCells);
        System.out.println(benificaryPost);

        if (isNetworkAvailable()) {
            Call<Void> getScheduleCall = benificaryApi.uploadBeneficiaryCollectedRecords("Bearer"+" "+ token, benificaryPost);

            getScheduleCall.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {

                    System.out.println(benificaryPost);
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            Toast.makeText(getApplicationContext(), "Data Saved Successfully", Toast.LENGTH_SHORT).show();


                        } else {
                            Toast.makeText(UnicefApplication.getAppContext(), "Something Went Wrong", Toast.LENGTH_SHORT).show();

                        }

                    }
                }


                @Override
                public void onFailure(Call<Void> call, Throwable t) {

                    Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                    t.printStackTrace();
                    System.out.println(t.getMessage());
                    System.out.println(call);
                }
            });
        } else {
            Toast.makeText(UnicefApplication.getAppContext(), "No Internet Connection", Toast.LENGTH_SHORT).show();
        }
    }

    @Override
    public void collectData(String values, long entityDynamicColumnId) {
        ArrayList<String> answer = new ArrayList<>();
        answer.add(values);

        beneficiaryDynamicCell = new BeneficiaryDynamicCell(answer, entityDynamicColumnId);
        System.out.println(beneficiaryDynamicCell);
    }

//    @Override
//    public void collectData(String values, BeneficiaryIndicator indicator) {
//        List<String> values1 = new ArrayList<String>();
//        values1.add(values);
//        indicator.setValues(values1);
//
//
//
//
//    }
}

