package com.unicef.mis.views;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.ViewModelProvider;

import com.unicef.mis.R;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.databinding.ActivityFacilityQuestionMainBinding;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.util.BaseFragment;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.FacilityViewModel;

public class FacilityListFragment extends BaseFragment {

    private View view;
    public ActivityFacilityQuestionMainBinding facilityDataBinding;
    public FacilityViewModel facilityViewModel;
    private int entityType;
    private int operationMode;
    private ScheduledInstance instance;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Bundle arguments = this.getArguments();

        if (arguments.containsKey(UIConstants.KEY_ENTITY_TYPE)){
            entityType = arguments.getInt(UIConstants.KEY_ENTITY_TYPE);
        }

        if (arguments.containsKey(UIConstants.KEY_OPERATION_MODE)){
            operationMode = arguments.getInt(UIConstants.KEY_OPERATION_MODE);
        }

        if (arguments.containsKey(UIConstants.KEY_INSTANCE)){
            instance = (ScheduledInstance)arguments.getParcelable(UIConstants.KEY_INSTANCE);
        }
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        Singleton.getInstance().setContext(getContext());
        facilityDataBinding = DataBindingUtil.inflate(inflater, R.layout.activity_facility_question_main, container, false);
        view = facilityDataBinding.getRoot();
        facilityViewModel = new ViewModelProvider(this).get(FacilityViewModel.class);
        facilityViewModel.setView(view);
        facilityViewModel.prepareView(entityType, operationMode, instance);
        facilityDataBinding.setViewModel(facilityViewModel);
        facilityDataBinding.setLifecycleOwner(this);
        facilityViewModel.initViews();

        return view;
    }

    @Override
    public void onResume() {
        Log.e("DEBUG", "onResume of HomeFragment");
        super.onResume();
        facilityViewModel.searchFacilities();
    }

    @Override
    public void onPause() {
        Log.e("DEBUG", "OnPause of HomeFragment");
        super.onPause();
    }
}
