package com.unicef.mis.views.offline_data.beneficiary;

import android.content.Intent;
import android.os.Bundle;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;

import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.facebook.shimmer.ShimmerFrameLayout;
import com.google.android.material.floatingactionbutton.ExtendedFloatingActionButton;
import com.google.android.material.textfield.TextInputEditText;
import com.unicef.mis.R;
import com.unicef.mis.adapter.benificiary.BenificiaryNameAdapter;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.databinding.ActivityBeneficiaryListBinding;
import com.unicef.mis.listner.IMoveToFinalBenificary;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.BeneficiaryViewModel;
import com.unicef.mis.views.benificiary.NewBeneficiaryOfflineActivity;

import java.util.ArrayList;
import java.util.List;

public class OfflineBeneficiaryListActivity extends BaseActivity implements IMoveToFinalBenificary {
    private TextInputEditText search_tf;
    private RecyclerView benificiary_list_recycler;
    private LinearLayoutManager linearLayoutManager;
    private BenificiaryNameAdapter benificiaryNameListAdapter;

    private OfflineBeneficiaryListActivity listener;
    private List<Beneficiary> data;
    private ShimmerFrameLayout mShimmerViewContainer;

    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv, tv_facility_tv,id_facility_tv;

    private SQLiteDatabaseHelper db;
    private ArrayList<Beneficiary> benificiaries;
    private AppCompatTextView upload_all_tv;

    ActivityBeneficiaryListBinding binding;
    BeneficiaryViewModel viewModel;

    private ExtendedFloatingActionButton extended_fab;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_beneficiary_list);
//        binding = DataBindingUtil.setContentView(this, R.layout.activity_beneficiary_list);
//        viewModel = new ViewModelProvider(this).get(BeneficiaryViewModel.class);
//        binding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getApplicationContext());

        listener = this;

        data = new ArrayList<>();
        findControls();
        loadData();
    }

    private void findControls() {
        extended_fab = findViewById(R.id.extended_fab);

        search_tf = findViewById(R.id.search_tf);
        benificiary_list_recycler = findViewById(R.id.benificiary_list_recycler);
        tv_facility_tv = findViewById (R.id.tv_facility_tv);
        tv_facility_tv.setText(Singleton.getInstance().getFacilityName());
        id_facility_tv = findViewById (R.id.id_facility_tv);
        id_facility_tv.setText(String.valueOf(Singleton.getInstance().getFacilityId()));


        mShimmerViewContainer = findViewById(R.id.shimmer_view_container);
        if (Singleton.getInstance().getOfflineStatus() == 1) {
            mShimmerViewContainer.setVisibility(View.GONE);
        }

        no_content_iv = findViewById(R.id.no_content_iv);
        no_content_tv = findViewById(R.id.no_content_tv);
        upload_all_tv = findViewById(R.id.upload_all_tv);
        if (upload_all_tv != null) {
            upload_all_tv.setVisibility(View.GONE);
        }

        extended_fab.setOnClickListener(view -> {
            Intent i = new Intent(OfflineBeneficiaryListActivity.this, NewBeneficiaryOfflineActivity.class);
            startActivity(i);
        });
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

    @Override
    protected void onRestart() {
        super.onRestart();
        finish();
        overridePendingTransition(0, 0);
        startActivity(getIntent());
        overridePendingTransition(0, 0);
    }

    public void goBack(View view) {
        if (view.getId() == R.id.back) {
            finish();
        }
    }

    public void loadData() {
        db = SQLiteDatabaseHelper.getInstance(getApplicationContext());

        BeneficiaryDataAccess dataAccess = db.getBeneficiaryDataAccess();
        dialogUtil.showProgressDialog();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                return dataAccess.getBeneficiaryList(Singleton.getInstance().getId(), Singleton.getInstance().getIdInstance());
            }
        });
        promise.then(res -> onBeneficiariesFound(res))
                .error(err -> {
                    dialogUtil.dismissProgress();
                    showToast(UnicefApplication.getAppContext(), err.toString());
                    initListeners();
                });
    }

    private Object onBeneficiariesFound(Object res) {
        dialogUtil.dismissProgress();
        List<Beneficiary> benificiaryList = (List<Beneficiary>) res;
        benificiaries = new ArrayList<>(benificiaryList);
        initListeners();

        return null;
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

        if (benificiaries.size() <= 0) {
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);
        } else {
            no_content_iv.setVisibility(View.GONE);
            no_content_tv.setVisibility(View.GONE);

            benificiaryNameListAdapter = new BenificiaryNameAdapter(benificiaries, getApplicationContext(), this);
            linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
            benificiary_list_recycler.setLayoutManager(linearLayoutManager);
            benificiary_list_recycler.setAdapter(benificiaryNameListAdapter);
            benificiaryNameListAdapter.notifyDataSetChanged();
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
        benificiaryNameListAdapter = new BenificiaryNameAdapter(filteredList, getApplicationContext(), listener);
        linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
        benificiary_list_recycler.setLayoutManager(linearLayoutManager);
        benificiary_list_recycler.setAdapter(benificiaryNameListAdapter);

        benificiaryNameListAdapter.notifyDataSetChanged();
        benificiary_list_recycler.setNestedScrollingEnabled(false);

    }


    @Override
    public void moveToNextPage(String id, int beneficiaryId, String name) {
        Singleton.getInstance().setUnhcrId(id);
        Singleton.getInstance().setBenificiaryName(name);
        Singleton.getInstance().setBeneficiaryId(beneficiaryId);

        Intent intent = new Intent(OfflineBeneficiaryListActivity.this, OfflineBeneficiaryRecordsActivity.class);

        intent.putExtra("id", "1");
        intent.putExtra("description", "send description to 2nd activity");
        startActivityForResult(intent, 1);

    }


}
