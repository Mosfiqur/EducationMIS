package com.unicef.mis.views.offline_data.facility;

import android.os.Bundle;
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
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.viewmodel.ScheduleViewModel;
import com.unicef.mis.views.FacilityListFragment;

import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

public class OfflineFacilityInstanceListFragment extends BaseFragment implements IMoveToNextPage {
    private View view;
    private RecyclerView collect_facility_recycler;
    private BeneficiaryScheduleAdapter benificaryScheduleAdapter;
    private LinearLayoutManager linearLayoutManager;
    public OfflineFacilityInstanceListFragment listener;

    private SQLiteDatabaseHelper db;
    private ArrayList<ScheduledInstance> scheduledInstances;

    private CollectFacilityMainBinding binding;
    private ScheduleViewModel viewModel;

    private int entityType;
    private int operationMode;

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
        binding = DataBindingUtil.inflate(inflater, R.layout.collect_facility_main  , container, false);
        view =binding.getRoot();

        viewModel = new ViewModelProvider(this).get(ScheduleViewModel.class);
        viewModel.prepare(OperationMode.Offline.getIntValue(), EntityType.Facilitiy.getIntValue());
        viewModel.loadSchedule();
        binding.setViewModel(viewModel);

        listener = this;

        initViews();
        initListeners();

        return view;
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

        FacilityListFragment fragment = getFacilityListFragment(EntityType.Facilitiy.getIntValue(), OperationMode.Offline.getIntValue(), instance);
        ((AppCompatActivity) getActivity()).getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, fragment).commit();
    }
}
