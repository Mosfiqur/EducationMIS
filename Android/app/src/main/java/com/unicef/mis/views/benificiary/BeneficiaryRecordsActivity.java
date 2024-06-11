package com.unicef.mis.views.benificiary;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.unicef.mis.R;
import com.unicef.mis.adapter.BeneficiaryIndicatorAdapter;
import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.databinding.ActivityBeneficiaryFinalListBinding;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IBeneficiaryRepository;
import com.unicef.mis.listner.IBenificiaryFinalListner;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.benificiary.indicator.BeneficiaryIndicatorModel;
import com.unicef.mis.model.benificiary.indicator.Indicator;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryDynamicCell;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.BeneficiaryRecordsViewModel;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class BeneficiaryRecordsActivity extends BaseActivity implements View.OnClickListener, IBenificiaryFinalListner {

    private BeneficiaryIndicatorAdapter indicatorAdapter;
    private LinearLayoutManager linearLayoutManager;
    private RecyclerView questioner_group_recycler;

    private BeneficiaryApi beneficaryApi;
    private String token = "";

    private SharedPreferences sharedPreferences;

    private ShimmerFrameLayout mShimmerViewContainer;

    private BeneficiaryRecordsActivity listener;
    private AppCompatTextView save_all_tv;

    private BeneficiaryDynamicCell beneficiaryDynamicCell;
    private BeneficiaryPost benificaryPost;

    private ActivityBeneficiaryFinalListBinding binding;
    private BeneficiaryRecordsViewModel viewModel;

    private AppCompatTextView status_tv;
    private Beneficiary beneficiary;
    private int instanceId;

    private int operationMode;
    private IBeneficiaryRepository repository;
    private List<Beneficiary> beneficiaryGetByIdModel;


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_beneficiary_final_list);
        viewModel = new ViewModelProvider(this).get(BeneficiaryRecordsViewModel.class);
        binding.setViewModel(viewModel);

        Intent intent = getIntent();
        Bundle extras = intent.getExtras();
        if (extras.containsKey(UIConstants.INTENT_EXTRA_BENEFICIARY)) {
            beneficiary = (Beneficiary) extras.getParcelable(UIConstants.INTENT_EXTRA_BENEFICIARY);
            viewModel.setBeneficiary(beneficiary);
        }
        if (extras.containsKey(UIConstants.INTENT_EXTRA_INSTANCE_ID)) {
            instanceId = extras.getInt(UIConstants.INTENT_EXTRA_INSTANCE_ID);
        }

        if (extras.containsKey(UIConstants.INTENT_EXTRA_OPERATION_MODE)) {
            operationMode = extras.getInt(UIConstants.INTENT_EXTRA_OPERATION_MODE);
        }

        binding.setLifecycleOwner(this);

        Singleton.getInstance().setContext(getApplicationContext());
        repository = RepositoryFactory.getBeneficiaryRepository(operationMode, this);
        beneficaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);

        listener = this;

        initViews();
        initListeners();

        viewModel.prepareView(operationMode);
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
        beneficiaryGetByIdModel = new ArrayList<>();
        questioner_group_recycler = findViewById(R.id.questioner_group_recycler);

        sharedPreferences = getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");

        mShimmerViewContainer = findViewById(R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() == 1) {
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        save_all_tv = findViewById(R.id.save_all_tv);
        if (operationMode == OperationMode.Offline.getIntValue()){
            save_all_tv.setText(UnicefApplication.getResourceString(R.string.save));
        } else {
            save_all_tv.setText(UnicefApplication.getResourceString(R.string.submit));
        }

        status_tv = findViewById(R.id.status_tv);


        getBeneficiaryRecords();
        getBeneficiaryGetById();

    }




    public void initListeners() {
        save_all_tv.setOnClickListener(this);
        status_tv.setOnClickListener(this);
    }

    private void getBeneficiaryGetById() {
        Promise promise;
        if (operationMode == OperationMode.Online.getIntValue() ){
            promise = repository.beneficiaryGetById(beneficiary.getEntityId(), instanceId);

        } else {
            promise = repository.beneficiaryGetById(beneficiary.getId(), instanceId);
        }

        promise.then(res ->{
            beneficiaryGetByIdModel.add((Beneficiary) res);
            return  beneficiaryGetByIdModel;
        }).error(err ->{
            showError(err);
        });
    }

    private void getBeneficiaryRecords() {
        showWait("Getting beneficiary records", this);
        Promise promise = repository.getRecords(instanceId, beneficiary);
        promise.then(res ->{
            hideWait();
            BeneficiaryIndicatorModel beneficiaryWithrecords = (BeneficiaryIndicatorModel) res;
            prepareRecordsView(beneficiaryWithrecords.getData().get(0).getIndicators(), beneficiaryGetByIdModel);
            return null;
        }).error(err -> showError(err));
    }

    private void uploadSingleBeneficiary() {
        //i changed here (Mamun) because getting id 0 while posting, changing getID() to getEntityId()
        BeneficiaryPost beneficiaryPost = new BeneficiaryPost(beneficiary.getEntityId(), instanceId, new ArrayList<BeneficiaryDynamicCell>());
        beneficiaryPost.setId(beneficiary.getId());
        ArrayList<Indicator> indicators = indicatorAdapter.getChangedValues();
        for (Indicator indicator : indicators) {
            BeneficiaryDynamicCell cell = new BeneficiaryDynamicCell(indicator.getValues(), indicator.getEntityDynamicColumnId());

            cell.setRecordId(indicator.getRecordId());
            cell.setDataType(indicator.getColumnDataType());
            beneficiaryPost.getDynamicCells().add(cell);

        }

        showWait("Saving beneficiary records...", this);
        Promise promise = repository.saveRecords(beneficiaryPost);
        System.out.println(beneficiaryPost);
        promise.then(res->{
            showToast(this, "Total " + beneficiaryPost.getDynamicCells().size() + " records saved");
            hideWait();
            return null;
        }).error(err->{
            showError(err);
        });
    }

    private void prepareRecordsView(List<Indicator> indicators, List<Beneficiary> beneficiaryGetByIdModel) {
        viewModel.setTotalCount(indicators.size());
        if (indicators.size() > 0) {
            indicatorAdapter = new BeneficiaryIndicatorAdapter(indicators, beneficiaryGetByIdModel,  getApplicationContext(), listener);
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
        switch (view.getId()) {

            case R.id.save_all_tv:
                boolean isActive = beneficiary.getActive();
                if (isActive == false){
//                    showToast(getApplicationContext(), "Disabled");
                    uploadSingleBeneficiary();
                } else {
                    showToast(getApplicationContext(), "Please Re-engage the beneficiary");

                }

                break;

            case R.id.status_tv:
                dialogUtil.showDialogYesNo(getResources().getString(R.string.deactive_string), (dialog, id) -> {
                    deActiveBeneficiary();
                }, (dialog, id) -> dialog.dismiss());
                break;
        }

    }

    private void deActiveBeneficiary() {
        boolean isActive = beneficiary.getActive();
        showWait("Changing active status...", this);
        Promise promise = repository.changeActiveStatus(!isActive, beneficiary);
        promise.then(res->{
            showToast(getResources().getString(R.string.deactived));
            beneficiary.setActive(!isActive);
            viewModel.setBeneficiary(beneficiary);
            if(operationMode == OperationMode.Online.getIntValue()){
                finish();
//                hideWait();
                return null;
            }
            hideWait();
            return null;
        }).error(err->{
            showError(err);
        });

    }



    @Override
    public void collectData(String values, long entityDynamicColumnId) {

        beneficiaryDynamicCell = new BeneficiaryDynamicCell(Collections.singletonList(values), (int) entityDynamicColumnId);

    }

}
