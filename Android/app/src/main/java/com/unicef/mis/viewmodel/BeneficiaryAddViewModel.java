package com.unicef.mis.viewmodel;

import android.app.DatePickerDialog;
import android.content.Context;
import android.content.Intent;
import android.view.View;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.R;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.dataaccess.UserDataAccess;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.CreateBeneficiaryModel;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.SubBlock;
import com.unicef.mis.model.auth.UserProfile;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.repository.BeneficiaryAddRepository;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;
import com.unicef.mis.views.benificiary.BeneficiaryListActivity;

import java.util.ArrayList;
import java.util.Calendar;

import static com.unicef.mis.util.BaseActivity.isNetworkAvailable;
import static com.unicef.mis.util.BaseActivity.showToast;


public class BeneficiaryAddViewModel extends ViewModel {
    public Context context;
    private BeneficiaryAddRepository repository;

    public MutableLiveData<FacilityListDatum> facility = new MutableLiveData<FacilityListDatum>();
    public MutableLiveData<Integer> operationMode = new MutableLiveData<Integer>();

    public MutableLiveData<Camp> camps;
    public MutableLiveData<PagedResponse<Block>> blocks;
    public MutableLiveData<PagedResponse<SubBlock>> subBlocks;

    public MutableLiveData<String> unchrId = new MutableLiveData<>();
    public MutableLiveData<String> facilityId = new MutableLiveData<>();
    public MutableLiveData<String> nameOfStudent = new MutableLiveData<>();
    public MutableLiveData<String> fathername = new MutableLiveData<>();
    public MutableLiveData<String> motherName = new MutableLiveData<>();
    public MutableLiveData<String> fcnId = new MutableLiveData<>();
    public MutableLiveData<String> beneficiaryCamp = new MutableLiveData<>();
    public MutableLiveData<String> beneficiaryBlock = new MutableLiveData<>();
    public MutableLiveData<String> beneficiarySubBlock = new MutableLiveData<>();
    public MutableLiveData<String> dob = new MutableLiveData<>();
    public MutableLiveData<String> sex = new MutableLiveData<>();
    public MutableLiveData<String> disabled = new MutableLiveData<>();
    public MutableLiveData<String> levelOfStudy = new MutableLiveData<>();
    public MutableLiveData<String> enrollmentDate = new MutableLiveData<>();
    public MutableLiveData<String> dataHolderName = new MutableLiveData<>();
    public MutableLiveData<String> dataHolderPhone = new MutableLiveData<>();
    public MutableLiveData<String> remarks = new MutableLiveData<>();
    public MutableLiveData<String> lfLocation = new MutableLiveData<>();
    public MutableLiveData<String> programmingPartner = new MutableLiveData<>();
    public MutableLiveData<String> implementationPartner = new MutableLiveData<>();

    private SQLiteDatabaseHelper db;
    private ArrayList<UserProfile> userProfile;

    private DatePickerDialog picker;
    final Calendar cldr = Calendar.getInstance();
    int day = cldr.get(Calendar.DAY_OF_MONTH);
    int month = cldr.get(Calendar.MONTH);
    int year = cldr.get(Calendar.YEAR);

    public BeneficiaryAddViewModel() {
        context = UnicefApplication.getAppContext();
        repository = RepositoryFactory.getBeneficiaryAddRepository();
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        camps = new MutableLiveData<>();
        blocks = new MutableLiveData<>();
        subBlocks = new MutableLiveData<>();

        UserDataAccess userDataAccess = db.getUserDataAccess();
        userProfile = new ArrayList<>(userDataAccess.getProfile());

        dataHolderName.setValue(userProfile.get(0).getFullName());
        dataHolderPhone.setValue(String.valueOf(userProfile.get(0).getPhoneNumber()));


    }

//    public void loadCamp(){
//        if (isNetworkAvailable()){
//            Promise promise = repository.showCamps();
//            promise.then(res -> setCampData((Camp) res));
//        } else {
//            showToast(context, UnicefApplication.getResourceString(R.string.no_internet));
//        }
//    }

//    public void loadBlock(){
//        if (isNetworkAvailable()){
//            Promise promise = repository.showBlocks();
//            promise.then(res -> setBlockdata((PagedResponse<Block>) res));
//        } else {
//            showToast(context, UnicefApplication.getResourceString(R.string.no_internet));
//        }
//    }
//
//    public void loadSubBlock(){
//        if (isNetworkAvailable()){
//            Promise promise = repository.showSubBlocks();
//            promise.then(res -> setSubBlock((PagedResponse<SubBlock>) res));
//        } else {
//            showToast(context, UnicefApplication.getResourceString(R.string.no_internet));
//        }
//    }




    public MutableLiveData<Camp> getCamps() {
        return camps;
    }

    public MutableLiveData<PagedResponse<Block>> getBlocks() {
        return blocks;
    }

    public MutableLiveData<PagedResponse<SubBlock>> getSubBlocks() {
        return subBlocks;
    }

    private boolean setCampData(Camp res) {
        camps.setValue(res);
        return true;
    }

    private boolean setBlockdata(PagedResponse<Block> res){
        blocks.setValue(res);
        return true;
    }

    private boolean setSubBlock(PagedResponse<SubBlock> res){
        subBlocks.setValue(res);
        return true;
    }



    public void selectdob(View view){
        picker = new DatePickerDialog(view.getContext(),
                (view1, year1, monthOfYear, dayOfMonth) -> dob.setValue(dayOfMonth + "/" + (monthOfYear + 1) + "/" + year), year, month, day);
        picker.show();
    }

    public void selectEnrollment(View view){
        picker = new DatePickerDialog(view.getContext(),
                (view1, year1, monthOfYear, dayOfMonth) -> enrollmentDate.setValue(dayOfMonth + "/" + (monthOfYear + 1) + "/" + year), year, month, day);
        picker.show();
    }

    public void save(View view){
        if (isNetworkAvailable()){
//            if (this.unchrId.toString().isEmpty()){
//                showToast(context, "Please Insert Unhcr Id");
//            } else if (this.nameOfStudent.getValue().length() ==0){
//                showToast(context, "Please Insert Beneficiary Name");
//            } else if (this.fathername.getValue().length() == 0){
//                showToast(context, "Please Insert Beneficiary Father Name");
//            } else if (this.motherName.getValue().length() ==0 ){
//                showToast(context, "Please Insert Beneficiary Mother Name");
//            } else if (this.fcnId.getValue().length() == 0){
//                showToast(context, "Please Insert FcnId");
//            }

//            else {
                Promise promise = repository.createBeneficiary(0, unchrId.getValue(), nameOfStudent.getValue(), fathername.getValue(), motherName.getValue(),
                        fcnId.getValue(), dob.getValue(), 1, true, 1, enrollmentDate.getValue(), Integer.valueOf(facilityId.getValue()), Singleton.getInstance().getCampId(),
                        Singleton.getInstance().getBlockId(), Singleton.getInstance().getSubBlock(), remarks.getValue(), true);

                promise.then( res -> gotoBeneficiary((CreateBeneficiaryModel) res))
                        .error(err ->{
                            showToast(context, UnicefApplication.getResourceString(R.string.sww));
                        });
//            }

        } else {
            showToast(context, "No Internet Connection");
        }
    }

    private boolean gotoBeneficiary(CreateBeneficiaryModel res) {

        Intent i = new Intent(UnicefApplication.getAppContext(), BeneficiaryListActivity.class);
        i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        showToast(UnicefApplication.getAppContext(), "Data Saved Successfully");
        UnicefApplication.getAppContext().startActivity(i);
        return true;
    }

    public void back(View view){
        Intent i = new Intent(UnicefApplication.getAppContext(), BeneficiaryListActivity.class);
//        i.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        UnicefApplication.getAppContext().startActivity(i);
    }

    public void prepareView(int operationMode, FacilityListDatum facility) {
        this.facility.setValue(facility);
        this.operationMode.setValue(operationMode);
        facilityId.setValue(String.valueOf(facility.getId()));
        Singleton.getInstance().setFacilityId(facility.getId());
        Singleton.getInstance().setFacilityCode(facility.getFacilityCode());
        Singleton.getInstance().setFacilityName(facility.getFacilityName());
        programmingPartner.setValue(facility.getProgrammingPartnerName());
        implementationPartner.setValue(facility.getImplemantationPartnerName());
    }


}
