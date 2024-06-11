package com.unicef.mis.views.benificiary;

import android.app.DatePickerDialog;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.RelativeLayout;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatDialog;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatSpinner;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;


import com.google.android.material.textfield.TextInputEditText;
import com.unicef.mis.R;
import com.unicef.mis.adapter.selection.BlockAdapter;
import com.unicef.mis.adapter.selection.CampAdapter;
import com.unicef.mis.adapter.selection.SubBlockAdapter;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.BlockDataAccess;
import com.unicef.mis.dataaccess.CampDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.dataaccess.SubBlockDataAccess;
import com.unicef.mis.dataaccess.UserDataAccess;
import com.unicef.mis.databinding.ActivityAddBeneficiaryBinding;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.interfaces.ISelectBlock;
import com.unicef.mis.interfaces.ISelectCamp;
import com.unicef.mis.interfaces.ISelectSubBlock;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.SubBlock;
import com.unicef.mis.model.auth.UserProfile;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.util.AsyncTaskRunner;
import com.unicef.mis.util.BaseActivity;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.viewmodel.BeneficiaryAddViewModel;
import com.unicef.mis.views.MainActivity;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;

public class NewBeneficiaryOfflineActivity extends BaseActivity implements
        View.OnClickListener, ISelectCamp, ISelectBlock, ISelectSubBlock {
    private AppCompatTextView save_all_tv;
    private AppCompatImageView back;

    private SQLiteDatabaseHelper db;
    private ArrayList<UserProfile> userProfile;
    public AppCompatDialog dialog_subblock, dialog_camp, dialog_block;

    private AppCompatImageView close, no_content_iv;

    private RecyclerView benificiary_list_recycler;
    private LinearLayoutManager linearLayoutManager;

    private CampAdapter campAdapter;
    private BlockAdapter blockAdapter;
    private SubBlockAdapter subBlockAdapter;

    private TextInputEditText unchr, facilittid, name, fathername, mothername, fcnid, dataholdername, dataholderphone, remarks,
            lftlocation, programmingpartner, implementationpartner;
    private RelativeLayout beneficiaryCampField, beneficiaryBlockField, beneficiarySubBlockField, dobField, enrollmentDateField;
    private AppCompatTextView select_camp_tv, select_block_tv, select_subblock_tv, select_dob_tv, select_enrollment_tv, header, no_content_tv;
    private AppCompatSpinner sexSpinner, disabledSpinner, losSpinner;

    private DatePickerDialog picker;
    final Calendar cldr = Calendar.getInstance();
    int day = cldr.get(Calendar.DAY_OF_MONTH);
    int month = cldr.get(Calendar.MONTH);
    int year = cldr.get(Calendar.YEAR);

    private NewBeneficiaryOfflineActivity listener;
    private Context context = this;

    private ActivityAddBeneficiaryBinding binding;
    private BeneficiaryAddViewModel viewModel;

    private int operationMode;
    private FacilityListDatum facility;
    private int instanceId;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        binding = DataBindingUtil.setContentView(this, R.layout.activity_add_beneficiary);
        viewModel = new ViewModelProvider(this).get(BeneficiaryAddViewModel.class);
        binding.setViewModel(viewModel);


        Singleton.getInstance().setContext(getApplicationContext());

        listener = this;

        Intent intent = getIntent();
        Bundle extras = intent.getExtras();

        if (extras.containsKey(UIConstants.KEY_OPERATION_MODE)) {
            operationMode = extras.getInt(UIConstants.KEY_OPERATION_MODE);
        }
        if (extras.containsKey(UIConstants.INTENT_EXTRA_FACILITY)) {
            facility = (FacilityListDatum) extras.getParcelable(UIConstants.INTENT_EXTRA_FACILITY);
        }
        if (extras.containsKey(UIConstants.INTENT_EXTRA_INSTANCE_ID)) {
            instanceId = extras.getInt(UIConstants.INTENT_EXTRA_INSTANCE_ID);
        }

        viewModel.prepareView(operationMode, facility);
        db = SQLiteDatabaseHelper.getInstance(getApplicationContext());

        initViews();
        initListeners();
    }

    public void initViews() {
        save_all_tv = findViewById(R.id.save_all_tv);
        back = findViewById(R.id.back);

        unchr = findViewById(R.id.unchr);
        facilittid = findViewById(R.id.facilittid);
        name = findViewById(R.id.name);
        fathername = findViewById(R.id.fathername);
        mothername = findViewById(R.id.mothername);
        fcnid = findViewById(R.id.fcnid);
        dataholdername = findViewById(R.id.dataholdername);
        dataholderphone = findViewById(R.id.dataholderphone);
        remarks = findViewById(R.id.remarks);
        lftlocation = findViewById(R.id.lftlocation);
        programmingpartner = findViewById(R.id.programmingpartner);
        implementationpartner = findViewById(R.id.implementationpartner);

        beneficiaryCampField = findViewById(R.id.beneficiaryCampField);
        beneficiaryBlockField = findViewById(R.id.beneficiaryBlockField);
        beneficiarySubBlockField = findViewById(R.id.beneficiarySubBlockField);
        dobField = findViewById(R.id.dobField);
        enrollmentDateField = findViewById(R.id.enrollmentDateField);

        select_camp_tv = findViewById(R.id.select_camp_tv);
        select_block_tv = findViewById(R.id.select_block_tv);
        select_subblock_tv = findViewById(R.id.select_subblock_tv);
        select_dob_tv = findViewById(R.id.select_dob_tv);
        select_enrollment_tv = findViewById(R.id.select_enrollment_tv);

        sexSpinner = findViewById(R.id.sexSpinner);
        disabledSpinner = findViewById(R.id.disabledSpinner);
        losSpinner = findViewById(R.id.losSpinner);
    }

    public void initListeners() {
        UserDataAccess userDataAccess = db.getUserDataAccess();
        userProfile = new ArrayList<>(userDataAccess.getProfile());

        dataholdername.setText(userProfile.get(0).getFullName());
        dataholderphone.setText(String.valueOf(userProfile.get(0).getPhoneNumber()));

        facilittid.setText(Singleton.getInstance().getFacilityCode());

        programmingpartner.setText(Singleton.getInstance().getProgrammingPartnerName());
        implementationpartner.setText(Singleton.getInstance().getImplementationPartnerName());

        save_all_tv.setOnClickListener(this);
        back.setOnClickListener(this);
        dobField.setOnClickListener(this);
        enrollmentDateField.setOnClickListener(this);
        beneficiaryCampField.setOnClickListener(this);
        beneficiaryBlockField.setOnClickListener(this);
        beneficiarySubBlockField.setOnClickListener(this);

        selectLos();
        selectDisabled();
        selectSex();
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.save_all_tv:
                createBeneficiary();
                break;

            case R.id.back:
                finish();
                break;

            case R.id.dobField:
                picker = new DatePickerDialog(NewBeneficiaryOfflineActivity.this,
                        (view1, year, monthOfYear, dayOfMonth) -> select_dob_tv.setText(dayOfMonth + "/" + (monthOfYear + 1) + "/" + year), year, month, day);
                picker.show();
                break;

            case R.id.enrollmentDateField:
                picker = new DatePickerDialog(NewBeneficiaryOfflineActivity.this,
                        (view1, year, monthOfYear, dayOfMonth) -> select_enrollment_tv.setText(dayOfMonth + "/" + (monthOfYear + 1) + "/" + year), year, month, day);
                picker.show();
                break;

            case R.id.beneficiaryCampField:
                getCamp();
                break;

            case R.id.beneficiaryBlockField:
                if (Singleton.getInstance().getCampId() == null) {
                    showToast(UnicefApplication.getAppContext(), "Select Camp First");
                } else {
                    getBlock();
                }

                break;

            case R.id.beneficiarySubBlockField:
                if (Singleton.getInstance().getBlockId() == null) {
                    showToast(UnicefApplication.getAppContext(), "Select Block First");
                } else {
                    getSubBlock();
                }

                break;
        }
    }

    private void createBeneficiary() {
        if (unchr.getText().toString().length() <=0 ){
            showToast(context, "Please Insert Unhcr Id");
        }

        if (name.getText().toString().length()<= 0){
            showToast(context, "Please Insert Beneficiary Name");
        }

        if (fathername.getText().toString().length() <= 0){
            showToast(context, "Please Insert Beneficiary Father Name");
        }

        if (mothername.getText().toString().length() <=0 ){
            showToast(context, "Please Insert Beneficiary Mother Name");
        }

        if (fcnid.getText().toString().length() <= 0){
            showToast(context, "Please Insert FcnId");
        }

        if (select_camp_tv.getText().toString().length() <= 0 ){
            showToast(context, "Please Insert Camp Details");
        }

        if (select_block_tv.getText().toString().length() <= 0){
            showToast(context, "Please Insert Block Details");
        }

        if (select_subblock_tv.getText().toString().length() <= 0){
            showToast(context, "Please Insert Sub-block Details");
        }

        if (select_dob_tv.getText().toString().length() <= 0){
            showToast(context, "Please Insert Date Of Birth");
        }

        if (select_enrollment_tv.getText().toString().length() <= 0){
            showToast(context, "Please Insert Unhcr Id");
        } else {
            Beneficiary beneficiary = new Beneficiary();
            BeneficiaryDataAccess beneficiaryDataAccess = db.getBeneficiaryDataAccess();

            Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
                @Override
                public Object execute() {
                    beneficiary.setEntityId(Singleton.getInstance().getId());
                    beneficiary.setActive(true);
                    beneficiary.setFacilityId(Singleton.getInstance().getFacilityId());
                    beneficiary.setBeneficiaryName(name.getText().toString());
                    beneficiary.setUnhcrId(unchr.getText().toString());
                    beneficiary.setFatherName(fathername.getText().toString());
                    beneficiary.setMotherName(mothername.getText().toString());
                    beneficiary.setFcnId(fcnid.getText().toString());
                    beneficiary.setDateOfBirth(select_dob_tv.getText().toString());
                    beneficiary.setSex(Singleton.getInstance().getSex());
                    beneficiary.setDisabled(Singleton.getInstance().isDisabled());
                    beneficiary.setLevelOfStudy(Singleton.getInstance().getLos());
                    beneficiary.setEnrollmentDate(select_enrollment_tv.getText().toString());
                    //facility Camp id if needed
                    beneficiary.setBeneficiaryCampId(Singleton.getInstance().getCampId());
                    beneficiary.setBlockId(Singleton.getInstance().getBlockId());
                    beneficiary.setSubBlockId(Singleton.getInstance().getSubBlock());
                    beneficiary.setRemarks(remarks.getText().toString());
                    beneficiary.setCollectionStatus(CollectionStatus.NotCollected.getIntValue());
                    beneficiary.setFacilityName(Singleton.getInstance().getFacilityName());
                    beneficiary.setBeneficiaryCampName(Singleton.getInstance().getCampName());
                    beneficiary.setBlockName(Singleton.getInstance().getBlockName());
                    beneficiary.setSubBlockName(Singleton.getInstance().getSubBlockName());



                    beneficiaryDataAccess.createNewBeneficiary(instanceId, beneficiary);
//

                    return beneficiaryDataAccess;
                }
            });

            promise.then(res ->{
                showToast(getApplicationContext(), "New Beneficiary Created");
                Intent intent = new Intent(NewBeneficiaryOfflineActivity.this, MainActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
                startActivity(intent);
                finish();
                return  null;
            }).error(err ->{
                showToast(getApplicationContext(), "Something Went Wrong");
            });

//            promise.then(res ->{
//                Promise promise1 = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
//                    @Override
//                    public Object execute() {
//                        BeneficiaryGetByIdModel beneficiaryGetByIdModel = new BeneficiaryGetByIdModel();
//
//                        beneficiaryGetByIdModel.setBeneficiaryId(Singleton.getInstance().getBeneficiaryId());
//                        beneficiaryGetByIdModel.setId(Singleton.getInstance().getIdInstance());
//                        beneficiaryGetByIdModel.setFacilityName(Singleton.getInstance().getFacilityName());
//                        beneficiaryGetByIdModel.setBeneficiaryCampName(Singleton.getInstance().getCampName());
//                        beneficiaryGetByIdModel.setBlockName(Singleton.getInstance().getBlockName());
//                        beneficiaryGetByIdModel.setSubBlockName(Singleton.getInstance().getSubBlockName());
//
//                        beneficiaryDataAccess.saveBeneficiaryGetById(beneficiaryGetByIdModel, Singleton.getInstance().getIdInstance());
//                        return beneficiaryDataAccess;
//                    }
//                });
//                return null;
//            }).error(err ->{
//
//            });


        }


    }

    private void saveBeneficiary() {

        hideWait();
    }

    public void getCamp() {
        dialog_camp = new AppCompatDialog(this);
        dialog_camp.setContentView(R.layout.select_camp);
        header = dialog_camp.findViewById(R.id.header);
        close = dialog_camp.findViewById(R.id.close);
        benificiary_list_recycler = dialog_camp.findViewById(R.id.benificiary_list_recycler);
        no_content_iv = dialog_camp.findViewById(R.id.no_content_iv);
        no_content_tv = dialog_camp.findViewById(R.id.no_content_tv);

        header.setText("Select Camp");

        CampDataAccess campDataAccess = db.getCampDataAccess();
        List<Camp> campList = campDataAccess.getCamp();

        campAdapter = new CampAdapter(campList, context, listener);
        linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
        benificiary_list_recycler.setLayoutManager(linearLayoutManager);
        benificiary_list_recycler.setAdapter(campAdapter);
        benificiary_list_recycler.setNestedScrollingEnabled(false);

        close.setOnClickListener(view -> {
            dialog_camp.dismiss();
        });

        dialog_camp.show();

    }

    public void getBlock() {
        dialog_block = new AppCompatDialog(this);
        dialog_block.setContentView(R.layout.select_camp);
        header = dialog_block.findViewById(R.id.header);
        close = dialog_block.findViewById(R.id.close);
        no_content_iv = dialog_block.findViewById(R.id.no_content_iv);
        no_content_tv = dialog_block.findViewById(R.id.no_content_tv);
        benificiary_list_recycler = dialog_block.findViewById(R.id.benificiary_list_recycler);

        header.setText("Select Block");

        BlockDataAccess blockDataAccess = db.getBlockDataAccess();
        List<Block> blockList = blockDataAccess.getBlock(Singleton.getInstance().getCampId());
        blockAdapter = new BlockAdapter(blockList, context, listener);
        linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
        benificiary_list_recycler.setAdapter(blockAdapter);
        benificiary_list_recycler.setLayoutManager(linearLayoutManager);
        benificiary_list_recycler.setNestedScrollingEnabled(false);


        close.setOnClickListener(view -> {
            dialog_block.dismiss();
        });

        dialog_block.show();

    }

    public void getSubBlock() {
        dialog_subblock = new AppCompatDialog(this);
        dialog_subblock.setContentView(R.layout.select_camp);
        header = dialog_subblock.findViewById(R.id.header);
        close = dialog_subblock.findViewById(R.id.close);
        no_content_iv = dialog_subblock.findViewById(R.id.no_content_iv);
        no_content_tv = dialog_subblock.findViewById(R.id.no_content_tv);
        benificiary_list_recycler = dialog_subblock.findViewById(R.id.benificiary_list_recycler);

        header.setText("Select Sub Block");

        SubBlockDataAccess subBlockDataAccess = db.getSubBlockDataAccess();
        List<SubBlock> subBlockList = subBlockDataAccess.getSubBlock(Singleton.getInstance().getBlockId());
        subBlockAdapter = new SubBlockAdapter(subBlockList, context, listener);
        linearLayoutManager = new LinearLayoutManager(getApplicationContext(), RecyclerView.VERTICAL, false);
        benificiary_list_recycler.setAdapter(subBlockAdapter);
        benificiary_list_recycler.setLayoutManager(linearLayoutManager);
        benificiary_list_recycler.setNestedScrollingEnabled(false);

        close.setOnClickListener(view -> {
            dialog_subblock.dismiss();
        });

        dialog_subblock.show();
    }

    @Override
    public void selectionCamp(String name, int id) {
        Singleton.getInstance().setCampId(id);
        Singleton.getInstance().setCampName(name);
        select_camp_tv.setText(name);
        select_block_tv.setText("");
        select_subblock_tv.setText("");
        dialog_camp.dismiss();
    }

    @Override
    public void selectionBlock(String name, int id) {
        Singleton.getInstance().setBlockId(id);
        Singleton.getInstance().setBlockName(name);
        select_block_tv.setText(name);
        select_subblock_tv.setText("");
        dialog_block.dismiss();
    }

    @Override
    public void selectionSubBlock(String name, int id) {
        Singleton.getInstance().setSubBlock(id);
        Singleton.getInstance().setSubBlockName(name);
        select_subblock_tv.setText(name);
        dialog_subblock.dismiss();
    }

    private void selectLos() {
        ArrayList<String> answerLos = new ArrayList<>();

        answerLos.add("Level 1");
        answerLos.add("Level 2");
        answerLos.add("Level 3");
        answerLos.add("Level 4");

        ArrayAdapter<String> losAdapter = new ArrayAdapter<String>(context, android.R.layout.simple_spinner_item, answerLos);
        losAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        losSpinner.setAdapter(losAdapter);

        losSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if (position ==0){
                    Singleton.getInstance().setLos(1);
                } else if (position == 1){
                    Singleton.getInstance().setLos(2);
                } else if (position == 2){
                    Singleton.getInstance().setLos(3);
                } else if (position == 3){
                    Singleton.getInstance().setLos(4);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

    }

    private void selectDisabled() {
        ArrayList<String> answerDisabled = new ArrayList<>();

        answerDisabled.add("No");
        answerDisabled.add("Yes");


        ArrayAdapter<String> disabledAdapter = new ArrayAdapter<String>(context, android.R.layout.simple_spinner_item, answerDisabled);
        disabledAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        disabledSpinner.setAdapter(disabledAdapter);

        disabledSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if (position == 0){
                    Singleton.getInstance().setDisabled(false);
                } else {
                    Singleton.getInstance().setDisabled(true);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

    }

    private void selectSex() {
        ArrayList<String> answerSex = new ArrayList<>();

        answerSex.add("Male");
        answerSex.add("Female");
        answerSex.add("Others");


        ArrayAdapter<String> sexAdapter = new ArrayAdapter<String>(context, android.R.layout.simple_spinner_item, answerSex);
        sexAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);

        sexSpinner.setAdapter(sexAdapter);

        sexSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                if (position == 0){
                    Singleton.getInstance().setSex(1);
                } else if (position == 1){
                    Singleton.getInstance().setSex(2);
                } else if (position == 2){
                    Singleton.getInstance().setSex(3);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });
    }
}
