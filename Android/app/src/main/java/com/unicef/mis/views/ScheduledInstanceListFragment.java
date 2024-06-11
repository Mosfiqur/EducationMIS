package com.unicef.mis.views;

import android.os.Bundle;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.adapter.benificiary.BeneficiaryScheduleAdapter;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.databinding.CollectFacilityMainBinding;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.listner.IMoveToNextPage;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.util.BaseFragment;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.ScheduleViewModel;

import java.util.List;
import java.util.Objects;

/**
 * BeneficiarySchedule name should be ScheduleFragment. This should be used everywhere
 * */
public class ScheduledInstanceListFragment extends BaseFragment implements IMoveToNextPage {
    private View view;
    private RecyclerView collect_facility_recycler;
    private BeneficiaryScheduleAdapter benificaryScheduleAdapter;
    private LinearLayoutManager linearLayoutManager;
    public ScheduledInstanceListFragment listener;
    private int entityType;
    private int operationMode;
    
    public CollectFacilityMainBinding collectFacilityMainBinding;
    public ScheduleViewModel viewModel;

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
    }

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        collectFacilityMainBinding = DataBindingUtil.inflate(inflater, R.layout.collect_facility_main, container, false);
        view = collectFacilityMainBinding.getRoot();

        viewModel = new ViewModelProvider(this).get(ScheduleViewModel.class);
        viewModel.prepare(operationMode, entityType);
        viewModel.loadSchedule();
        collectFacilityMainBinding.setViewModel(viewModel);

        Singleton.getInstance().setContext(getContext());

        listener = this;

        initViews();
        initListeners();

        return view;
    }

    @Override
    public void onViewCreated(@NonNull View view, @Nullable Bundle savedInstanceState) {
        super.onViewCreated(view, savedInstanceState);
    }

        @Override
    public void onResume() {
        Log.e("DEBUG", "onResume of HomeFragment");
        super.onResume();
    }

    @Override
    public void onPause() {
        Log.e("DEBUG", "OnPause of HomeFragment");
        super.onPause();
    }

    public void initViews(){
        collect_facility_recycler = view.findViewById (R.id.collect_facility_recycler);
    }

    public void initListeners(){
       viewModel.getScheduledInstanceList().observe(Objects.requireNonNull(getActivity()), scheduleListObserver);
    }

    Observer<List<ScheduledInstance>> scheduleListObserver = new Observer<List<ScheduledInstance>>() {
        @Override
        public void onChanged(List<ScheduledInstance> scheduledInstances) {
            if (scheduledInstances.size() > 0){
                benificaryScheduleAdapter = new BeneficiaryScheduleAdapter( scheduledInstances, getContext(), listener);
                linearLayoutManager = new LinearLayoutManager(getContext(), RecyclerView.VERTICAL, false);
                collect_facility_recycler.setAdapter(benificaryScheduleAdapter);
                collect_facility_recycler.setLayoutManager(linearLayoutManager);
                collect_facility_recycler.setNestedScrollingEnabled(false);
            }
        }
    };


    @Override
    public void moveToNextPage(int id, String scheduleName, String scheduleDate, String endDate, ScheduledInstance instance) {
        Singleton.getInstance().setIdInstance(id);
        Singleton.getInstance().setScheduleName(scheduleName);
        Singleton.getInstance().setScheduleDate( scheduleDate );
        Singleton.getInstance().setEndDate(endDate);
        FacilityListFragment fragment = getFacilityListFragment(entityType, operationMode, instance);
        ((AppCompatActivity) getActivity()).getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, fragment).commit();
    }
}

