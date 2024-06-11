package com.unicef.mis.views.home;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;

import com.unicef.mis.R;
import com.unicef.mis.dataaccess.UserDataAccess;
import com.unicef.mis.databinding.ActivityHomeBinding;
import com.unicef.mis.factory.DataAccessFactory;
import com.unicef.mis.model.auth.UserProfile;
import com.unicef.mis.util.BaseFragment;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.LoginViewModel;
import com.unicef.mis.viewmodel.UserViewModel;

import java.util.ArrayList;

public class HomeFragment extends BaseFragment {
    private View view;
    ActivityHomeBinding userDetailsBinding;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        userDetailsBinding = DataBindingUtil.inflate(inflater, R.layout.activity_home, container, false);
        view = userDetailsBinding.getRoot();
        UserViewModel userViewModel = new ViewModelProvider(this).get(UserViewModel.class);
        userDetailsBinding.setUserDetails(userViewModel);
        Singleton.getInstance().setContext(getContext());
        return view;
    }
}
