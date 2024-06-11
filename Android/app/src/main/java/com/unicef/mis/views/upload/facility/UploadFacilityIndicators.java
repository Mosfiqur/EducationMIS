package com.unicef.mis.views.upload.facility;

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
import com.unicef.mis.adapter.OfflineFacilityIndicatorAdapter;
import com.unicef.mis.api.FacilityApi;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.interfaces.IFacilityFinalListner;
import com.unicef.mis.model.BeneficiaryIndicator;
import com.unicef.mis.model.facility.indicator.post.FacilityDynamicCell;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.views.MainActivity;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class UploadFacilityIndicators extends BaseActivity  implements View.OnClickListener, IFacilityFinalListner {
    private AppCompatTextView header;
    private RecyclerView questioner_recycler;
    private AppCompatImageView back;

    private AppCompatTextView header_tv;
    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv, save_all_tv;

    private ShimmerFrameLayout mShimmerViewContainer;

    private OfflineFacilityIndicatorAdapter indicatorAdapter;
    private LinearLayoutManager linearLayoutManager;

    private UploadFacilityIndicators listener;
    private SQLiteDatabaseHelper db;
    private ArrayList<BeneficiaryIndicator> indicators;
    private FacilityDynamicCell facilityDynamicCell;

    private FacilityApi facilityApi;
    private String token ="";

    private SharedPreferences sharedPreferences;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.questioner_app_bar_main);
        Singleton.getInstance().setContext(getApplicationContext());

        facilityApi = RetrofitService.createService(FacilityApi.class, APIClient.BASE_URL, true);

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
            save_all_tv.setText(getResources().getString(R.string.upload));
            save_all_tv.setOnClickListener(v -> {
                if (isNetworkAvailable()){
                    saveAllRecords();
                } else {
                    showToast(UnicefApplication.getAppContext(), getResources().getString(R.string.no_internet));
                }

            });
        }

        mShimmerViewContainer = findViewById (R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() ==1){
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        header = findViewById (R.id.app_title);

        no_content_iv = findViewById (R.id.no_content_iv);
        no_content_tv = findViewById (R.id.no_content_tv);

        sharedPreferences= getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
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
        FacilityPost facilityPost = new FacilityPost(Singleton.getInstance().getFacilityId(), Singleton.getInstance().getIdInstance(), new ArrayList<FacilityDynamicCell>());
        ArrayList<BeneficiaryIndicator> indicators = indicatorAdapter.getChangedValues();
        for (BeneficiaryIndicator indicator : indicators) {
            Long i = new Long(indicator.getDynamicColumnId());
            FacilityDynamicCell cell = new FacilityDynamicCell(indicator.getValues(), i.intValue());
            facilityPost.getDynamicCells().add(cell);
        }

        if (isNetworkAvailable()) {
            Call<Void> call = facilityApi.uploadFacilityCollectedRecords("Bearer" + " " + token, facilityPost);
            dialogUtil.showProgressDialog("Uploading facility");
            call.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    dialogUtil.dismissProgress();
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            showToast(getApplicationContext(), getResources().getString(R.string.data_save_success));
                            Intent i = new Intent(UploadFacilityIndicators.this, MainActivity.class);
                            startActivity(i);
                            finish();
                        }

                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    dialogUtil.dismissProgress();
                    Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                    t.printStackTrace();
                }
            });
        } else {
            Toast.makeText(UnicefApplication.getAppContext(), "No Internet Connection", Toast.LENGTH_SHORT).show();
            //callBack.apiCallFailed(true, "No internet connection");
        }

    }

    @Override
    public void collectData(String strValue, BeneficiaryIndicator indicator) {
        List<String> values = new ArrayList<String>();
        values.add(strValue);
        indicator.setValues(values);
    }
}
