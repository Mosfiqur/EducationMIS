package com.unicef.mis.views.facility;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;

import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.annotation.Nullable;

import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.unicef.mis.R;
import com.unicef.mis.adapter.FacilityIndicatorAdapter;
import com.unicef.mis.api.FacilityApi;

import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.databinding.ActivityFacilityQuestionListBinding;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IFacilityRepository;
import com.unicef.mis.listner.IBenificiaryFinalListner;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.facility.indicator.FacilityIndicatorModel;
import com.unicef.mis.model.facility.indicator.Indicator;
import com.unicef.mis.model.facility.indicator.post.FacilityDynamicCell;
import com.unicef.mis.model.facility.indicator.post.FacilityPost;

import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.FacilityRecordsViewModel;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;


import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class FacilityRecordsActivity extends BaseActivity implements View.OnClickListener, IBenificiaryFinalListner {
    private AppCompatTextView header;
    private RecyclerView questioner_recycler;
    private AppCompatImageView back;

    private AppCompatTextView header_tv, save_all_tv;
    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv;

    private ShimmerFrameLayout mShimmerViewContainer;

    private FacilityApi facilityApi;
    private String token = "";

    private SharedPreferences sharedPreferences;

    private FacilityIndicatorAdapter indicatorAdapter;
    private LinearLayoutManager linearLayoutManager;
    private FacilityDynamicCell facilityDynamicCell;
    private FacilityRecordsActivity listener;
    private AppCompatImageView bottom_sheet_close_iv;
    private FacilityListDatum facility;
    private int instanceId;

    private ActivityFacilityQuestionListBinding binding;
    private FacilityRecordsViewModel viewModel;
    private int operationMode;
    private IFacilityRepository facilityRepository;
    private List<FacilityListDatum> facilityGetByIdModels;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_facility_question_list);
        viewModel = new ViewModelProvider(this).get(FacilityRecordsViewModel.class);
        binding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getApplicationContext());
        Intent intent = getIntent();
        Bundle extras = intent.getExtras();
        if (extras.containsKey(UIConstants.INTENT_EXTRA_FACILITY)) {
            facility = (FacilityListDatum) extras.getParcelable(UIConstants.INTENT_EXTRA_FACILITY);
            viewModel.setFacility(facility);
        }
        if (extras.containsKey(UIConstants.INTENT_EXTRA_INSTANCE_ID)) {
            instanceId = extras.getInt(UIConstants.INTENT_EXTRA_INSTANCE_ID);
        }
        if (extras.containsKey(UIConstants.INTENT_EXTRA_OPERATION_MODE)) {
            operationMode = extras.getInt(UIConstants.INTENT_EXTRA_OPERATION_MODE);
        }
        facilityRepository = RepositoryFactory.getFacilityRepository(operationMode, this);

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

    public void goBack(View view) {

        if (view.getId() == R.id.back) {
            finish();
        }
    }

    public void initViews() {
        facilityGetByIdModels = new ArrayList<>();
        questioner_recycler = findViewById(R.id.questioner_recycler);
        header_tv = findViewById(R.id.header_tv);

        bottom_sheet_close_iv = findViewById(R.id.bottom_sheet_close_iv);

        mShimmerViewContainer = findViewById(R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() == 1) {
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        sharedPreferences = getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");

        header = findViewById(R.id.app_title);

        no_content_iv = findViewById(R.id.no_content_iv);
        no_content_tv = findViewById(R.id.no_content_tv);
        save_all_tv = findViewById(R.id.save_all_tv);
        if (operationMode == OperationMode.Online.getIntValue()){
            save_all_tv.setText(UnicefApplication.getResourceString(R.string.submit));
        } else {
            save_all_tv.setText(UnicefApplication.getResourceString(R.string.save));
        }

        save_all_tv.setOnClickListener(this);

        getFacilityById();
        getFacilityRecords();




    }



    public void initListeners() {
    }

    private void uploadSingleFacility() {
        FacilityPost facilityPost = new FacilityPost(facility.getId(), instanceId, new ArrayList<FacilityDynamicCell>());
        ArrayList<Indicator> indicators = indicatorAdapter.getChangedValues();
        for (Indicator indicator : indicators) {
            FacilityDynamicCell cell = new FacilityDynamicCell(indicator.getValues(), indicator.getEntityDynamicColumnId());

            cell.setRecordId(indicator.getRecordId());
            cell.setDataType(indicator.getColumnDataType());
            facilityPost.getDynamicCells().add(cell);



        }
        if (facilityPost.getDynamicCells().size() == 0) {
            showToast(this, "No changes to submit");
            return;
        }

        showWait("Uploading facility records", this);
        Promise promise = facilityRepository.saveRecords(facilityPost);
        promise.then(res->{
            hideWait();
            showToast(getApplicationContext(), getResources().getString(R.string.data_save_success));
            return null;
        }).error(err->{
            showError(err);
        });
    }

    public void getFacilityRecords() {
        Promise promise = facilityRepository.getRecords(instanceId, facility.getId());
        promise.then(res -> {
            FacilityIndicatorModel facilityWithRecords = (FacilityIndicatorModel)res;
            header_tv.setText("Total " + facilityWithRecords.getData().get(0).getIndicators().size() + " Question");
            prepareRecordsView(facilityWithRecords.getData().get(0).getIndicators(), facilityGetByIdModels);
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void getFacilityById() {
        Promise promise = facilityRepository.facilityGetById(facility.getId(), instanceId);
        promise.then(res ->{
//            FacilityGetByIdModel res1 = (FacilityGetByIdModel) res;
            facilityGetByIdModels.add((FacilityListDatum) res);
            return facilityGetByIdModels;
        }).error(err ->{
            showError(err);
        });
    }

    private void prepareRecordsView(List<Indicator> indicators, List <FacilityListDatum> facilityGetByIdModel) {
        if(indicators == null || indicators.size() <= 0) {
            // following codes will be removed when databinding applied
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);

            // Stopping Shimmer Effect's animation after data is loaded to ListView
            mShimmerViewContainer.stopShimmerAnimation();
            mShimmerViewContainer.setVisibility(View.GONE);
        } else {
            no_content_iv.setVisibility(View.INVISIBLE);
            no_content_tv.setVisibility(View.INVISIBLE);
            indicatorAdapter = new FacilityIndicatorAdapter(indicators, facilityGetByIdModel, getApplicationContext(), listener);
            linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
            questioner_recycler.setAdapter(indicatorAdapter);
            questioner_recycler.setLayoutManager(linearLayoutManager);
            questioner_recycler.setNestedScrollingEnabled(false);
            // Stopping Shimmer Effect's animation after data is loaded to ListView
            mShimmerViewContainer.stopShimmerAnimation();
            mShimmerViewContainer.setVisibility(View.GONE);
        }
    }


    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.save_all_tv:
                uploadSingleFacility();
                break;
        }
    }

    @Override
    public void collectData(String values, long entityDynamicColumnId) {
        facilityDynamicCell = new FacilityDynamicCell(Collections.singletonList(values), (int) entityDynamicColumnId);
    }

}
