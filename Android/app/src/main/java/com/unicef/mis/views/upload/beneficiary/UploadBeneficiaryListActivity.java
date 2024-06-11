package com.unicef.mis.views.upload.beneficiary;

import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;
import android.widget.RelativeLayout;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.annotation.RequiresApi;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.cardview.widget.CardView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.google.android.material.textfield.TextInputEditText;
import com.unicef.mis.R;
import com.unicef.mis.adapter.benificiary.BenificiaryNameAdapter;
import com.unicef.mis.api.BeneficiaryApi;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.databinding.ActivityBeneficiaryListBinding;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.listner.IMoveToFinalBenificary;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.PropertiesInfoModel;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryDynamicCell;
import com.unicef.mis.model.benificiary.indicator.post.BeneficiaryPost;
import com.unicef.mis.util.APIClient;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.RetrofitService;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.BeneficiaryViewModel;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;

public class UploadBeneficiaryListActivity extends BaseActivity implements IMoveToFinalBenificary {
    private TextInputEditText search_tf;
    private RecyclerView benificiary_list_recycler;
    private LinearLayoutManager linearLayoutManager;
    private BenificiaryNameAdapter beneficiaryAdapter;

    private UploadBeneficiaryListActivity listener;
    private List<Beneficiary> data;
    private ShimmerFrameLayout mShimmerViewContainer;

    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv;
    private AppCompatTextView upload_all_tv;

    private SQLiteDatabaseHelper db;
    private ArrayList<Beneficiary> benificiaries;
    private RelativeLayout layer1;
    private View viewline;
    public BeneficiaryApi benificaryApi;
    private String token = "";
    private int uploadBeneficiaryIndex = 0;

    private SharedPreferences sharedPreferences;

    private ArrayList<Beneficiary> beneficiry;
    private ActivityBeneficiaryListBinding binding;
    private BeneficiaryViewModel viewModel;

    private CardView facility_info;


    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_beneficiary_list);
        viewModel = new ViewModelProvider(this).get(BeneficiaryViewModel.class);
        binding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getApplicationContext());
        benificaryApi = RetrofitService.createService(BeneficiaryApi.class, APIClient.BASE_URL, true);

        listener = this;

        data = new ArrayList<>();


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
        sharedPreferences = getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");

        facility_info = findViewById (R.id.facility_info);
        facility_info.setVisibility(View.GONE);

        db = SQLiteDatabaseHelper.getInstance(getApplicationContext());
        benificiaries = new ArrayList<>(db.getBeneficiaryDataAccess().getBeneficiaryListUpload(Singleton.getInstance().getId()));

        //Hjave to look here is there anything wrong or not
        //ArrayList<Benificiary> allBeneficiaries = new ArrayList<>(db.getBeneficiaryDataAccess().getBeneficiaryListUpload(Singleton.getInstance().getId(), Singleton.getInstance().getIdInstance()));
        //benificiaries.addAll(allBeneficiaries);

//        for (Benificiary benificiary : allBeneficiaries) {
//            if (benificiary.getCollectionStatus() == CollectionStatus.Collected.getIntValue()) {
//                benificiaries.add(benificiary);
//            }
//        }


        layer1 = findViewById(R.id.layer1);
        viewline = findViewById(R.id.line1);


        search_tf = findViewById(R.id.search_tf);
        benificiary_list_recycler = findViewById(R.id.benificiary_list_recycler);

        mShimmerViewContainer = findViewById(R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() == 1) {
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        no_content_iv = findViewById(R.id.no_content_iv);
        no_content_tv = findViewById(R.id.no_content_tv);
        upload_all_tv = findViewById(R.id.upload_all_tv);
        if (upload_all_tv != null) {
            upload_all_tv.setOnClickListener(v -> {
                if (benificiaries.size() == 0) {
                    return;
                }
                uploadBeneficiaries();
            });
        }
    }

    private void uploadBeneficiaries() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                Toast.makeText(getApplicationContext(), "Data Saved Successfully", Toast.LENGTH_SHORT).show();
                reloadData();
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                reloadData();
            }
        };
        uploadSingleBeneficiary(benificiaries.get(uploadBeneficiaryIndex), callBack);
    }

    private void reloadData() {
        benificiaries.clear();

        BeneficiaryDataAccess dataAccess = db.getBeneficiaryDataAccess();
        dialogUtil.showProgressDialog();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                return dataAccess.getBeneficiaryList(Singleton.getInstance().getId(), Singleton.getInstance().getIdInstance());
            }
        });
        promise.then(res->onDataLoaded(res))
                .error(err->{
                    dialogUtil.dismissProgress();
                    showToast(UnicefApplication.getAppContext(), err.toString());
                });
    }

    private Object onDataLoaded(Object res) {
        List<Beneficiary> beneficiaries = (List<Beneficiary>)res;
        ArrayList<Beneficiary> allBeneficiaries = new ArrayList<Beneficiary>(beneficiaries);
        for (Beneficiary benificiary : allBeneficiaries) {
            if (benificiary.getCollectionStatus() == CollectionStatus.Collected.getIntValue()) {
                benificiaries.add(benificiary);
            }
        }

        beneficiaryAdapter.getBenificiaryListData().clear();
        if(benificiaries.size() > 0){
            beneficiaryAdapter.getBenificiaryListData().addAll(benificiaries);
        }
        beneficiaryAdapter.notifyDataSetChanged();
        dialogUtil.dismissProgress();
        return null;
    }

    private void uploadSingleBeneficiary(Beneficiary benificiary, IGenericApiCallBack callBack) {
        int instanceId = Singleton.getInstance().getIdInstance();
        Integer beneficiaryId = benificiary.getEntityId();
        BeneficiaryPost benificaryPost = new BeneficiaryPost(beneficiaryId, instanceId, new ArrayList<BeneficiaryDynamicCell>());
        ArrayList<PropertiesInfoModel> indicators = db.getBeneficiaryDataAccess().getBeneficiaryIndicators(instanceId, beneficiaryId);
        for (PropertiesInfoModel indicator : indicators) {
            BeneficiaryDynamicCell cell = new BeneficiaryDynamicCell(indicator.getValues(), indicator.getEntityColumnId());
            benificaryPost.getDynamicCells().add(cell);
        }

        if (isNetworkAvailable()) {
            Call<Void> call = benificaryApi.uploadBeneficiaryCollectedRecords("Bearer" + " " + token, benificaryPost);
            dialogUtil.showProgressDialog("Uploading beneficiary");
            call.enqueue(new Callback<Void>() {
                @RequiresApi(api = Build.VERSION_CODES.JELLY_BEAN)
                @Override
                public void onResponse(Call<Void> call, Response<Void> response) {
                    dialogUtil.dismissProgress();
                    if (response.isSuccessful()) {
                        if (response.code() == 200) {
                            db.getBeneficiaryDataAccess().deleteBeneficiary(benificiary.getEntityId(), instanceId);
                        } else {
                            Toast.makeText(UnicefApplication.getAppContext(), "Something Went Wrong", Toast.LENGTH_SHORT).show();
                        }
                        uploadBeneficiaryIndex++;
                        if(uploadBeneficiaryIndex > benificiaries.size() - 1){
                            callBack.apiCallSuccessful(null, null);
                            return;
                        }
                        uploadSingleBeneficiary(benificiaries.get(uploadBeneficiaryIndex), callBack);
                    }
                }

                @Override
                public void onFailure(Call<Void> call, Throwable t) {
                    dialogUtil.dismissProgress();
                    Toast.makeText(UnicefApplication.getAppContext(), t.getMessage(), Toast.LENGTH_SHORT).show();
                    t.printStackTrace();
                    uploadBeneficiaryIndex++;
                    if(uploadBeneficiaryIndex > benificiaries.size() - 1){
                        callBack.apiCallSuccessful(null, null);
                        return;
                    }
                    uploadSingleBeneficiary(benificiaries.get(uploadBeneficiaryIndex), callBack);
                }
            });
        } else {
            Toast.makeText(UnicefApplication.getAppContext(), "No Internet Connection", Toast.LENGTH_SHORT).show();
            callBack.apiCallFailed(true, "No internet connection");
        }
    }

    public void initListeners() {

        search_tf.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

                search(s);
            }

            @Override
            public void afterTextChanged(Editable s) {
            }
        });

        this.data = benificiaries;

        if (benificiaries.size() == 0) {
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);
        } else {
            beneficiaryAdapter = new BenificiaryNameAdapter(benificiaries, getApplicationContext(), this);
            linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
            benificiary_list_recycler.setLayoutManager(linearLayoutManager);
            benificiary_list_recycler.setAdapter(beneficiaryAdapter);
            beneficiaryAdapter.notifyDataSetChanged();
            benificiary_list_recycler.setNestedScrollingEnabled(false);
        }

        // Stopping Shimmer Effect's animation after data is loaded to ListView
        mShimmerViewContainer.stopShimmerAnimation();
        mShimmerViewContainer.setVisibility(View.GONE);
    }

    private void search(CharSequence s) {
        final ArrayList<Beneficiary> filteredList = new ArrayList<>();

        if (data != null) {

            for (int i = 0; i < data.size(); i++) {

                if (data.get(i).getBeneficiaryName() != null) {
                    final String supplierName = data.get(i).getBeneficiaryName();

                    if (supplierName.toLowerCase().contains(s)) {

                        filteredList.add(data.get(i));
                    }

                }

            }
        }

        no_content_iv.setVisibility(View.INVISIBLE);
        no_content_tv.setVisibility(View.INVISIBLE);
        beneficiaryAdapter = new BenificiaryNameAdapter(filteredList, getApplicationContext(), listener);
        linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
        benificiary_list_recycler.setLayoutManager(linearLayoutManager);
        benificiary_list_recycler.setAdapter(beneficiaryAdapter);

        beneficiaryAdapter.notifyDataSetChanged();
        benificiary_list_recycler.setNestedScrollingEnabled(false);


    }


    @Override
    public void moveToNextPage(String id, int beneficiaryId, String name) {
        Singleton.getInstance().setUnhcrId(id);
        Singleton.getInstance().setBenificiaryName(name);
        Singleton.getInstance().setBeneficiaryId(beneficiaryId);

        Intent i = new Intent(UploadBeneficiaryListActivity.this, UploadBeneficiaryIndicator.class);
        Singleton.getInstance().setIntValueForTest(1);
        startActivity(i);
    }


}

