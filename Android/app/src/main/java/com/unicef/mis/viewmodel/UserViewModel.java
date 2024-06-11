package com.unicef.mis.viewmodel;

import android.annotation.SuppressLint;
import android.content.Context;
import android.content.IntentSender;
import android.view.View;
import android.widget.Toast;

import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.google.android.material.snackbar.Snackbar;
import com.google.android.play.core.appupdate.AppUpdateInfo;
import com.google.android.play.core.appupdate.AppUpdateManager;
import com.google.android.play.core.appupdate.AppUpdateManagerFactory;
import com.google.android.play.core.install.model.AppUpdateType;
import com.google.android.play.core.install.model.InstallStatus;
import com.google.android.play.core.install.model.UpdateAvailability;
import com.google.android.play.core.tasks.OnSuccessListener;
import com.google.android.play.core.tasks.Task;
import com.unicef.mis.R;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.dataaccess.UserDataAccess;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.model.auth.User;
import com.unicef.mis.model.auth.UserProfile;
import com.unicef.mis.util.UnicefApplication;

import java.util.ArrayList;


public class UserViewModel extends ViewModel {
    private static final int MY_REQUEST_CODE = 1234;
    public MutableLiveData<String> name = new MutableLiveData<>();
    public MutableLiveData<String> roleName = new MutableLiveData<>();
    public MutableLiveData<String> phone = new MutableLiveData<>();
    public MutableLiveData<String> email = new MutableLiveData<>();
    public MutableLiveData<String> developer = new MutableLiveData<>();

    private SQLiteDatabaseHelper db;
    private ArrayList<UserProfile> userProfile;
    private Context context;


    public UserViewModel() {
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

        context = UnicefApplication.getAppContext();

        UserDataAccess userDataAccess = DataAccessFactory.getUserDataAccess();
        userProfile = new ArrayList<>(userDataAccess.getProfile());

        name.setValue(userProfile.get(0).getFullName());
        roleName.setValue(userProfile.get(0).getRoleName());

        if (String.valueOf(userProfile.get(0).getPhoneNumber()).equalsIgnoreCase("null") ){
            phone.setValue(UnicefApplication.getResourceString(R.string.no_phone_number));
        }else {
            phone.setValue(String.valueOf(userProfile.get(0).getPhoneNumber()));

        }

        email.setValue(userProfile.get(0).getEmail());

        developer.setValue("Download Database (Dev Testing)");


    }

//
}
