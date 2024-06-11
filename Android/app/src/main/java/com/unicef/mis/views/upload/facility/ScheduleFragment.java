package com.unicef.mis.views.upload.facility;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.adapter.benificiary.BeneficiaryScheduleAdapter;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.listner.IMoveToNextPage;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.util.BaseFragment;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.views.FacilityListFragment;

import java.util.ArrayList;

public class ScheduleFragment extends BaseFragment implements IMoveToNextPage {
    private View view;
    private RecyclerView collect_facility_recycler;
    private BeneficiaryScheduleAdapter benificaryScheduleAdapter;
    private LinearLayoutManager linearLayoutManager;
    public ScheduleFragment listener;

    private AppCompatImageView no_content_iv;
    private AppCompatTextView no_content_tv;

    private SQLiteDatabaseHelper db;
    private ArrayList<ScheduledInstance> scheduledInstances;

    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.offline_recycler_list  , container, false);
        Singleton.getInstance().setContext(getContext());
        collect_facility_recycler = view.findViewById (R.id.collect_facility_recycler);

        listener = this;


        initViews();
        initListeners();

        return view;
    }



    public void initViews(){

        db = SQLiteDatabaseHelper.getInstance(getContext());
        FacilityDataAccess facilityDataAccess = new FacilityDataAccess();
        scheduledInstances = new ArrayList<>(db.getFacilityDataAccess().getFacilitySchedule());

        no_content_iv = view.findViewById (R.id.no_content_iv);
        no_content_tv = view.findViewById (R.id.no_content_tv);


    }

    public void initListeners(){
        if (scheduledInstances.size() ==0){
            no_content_iv.setVisibility(View.VISIBLE);
            no_content_tv.setText(getResources().getString(R.string.no_content_found));
            no_content_tv.setVisibility(View.VISIBLE);
        } else{
            benificaryScheduleAdapter = new BeneficiaryScheduleAdapter( scheduledInstances, getContext(), listener);
            linearLayoutManager = new LinearLayoutManager(getContext(), RecyclerView.VERTICAL, false);
            collect_facility_recycler.setAdapter(benificaryScheduleAdapter);
            collect_facility_recycler.setLayoutManager(linearLayoutManager);
            collect_facility_recycler.setNestedScrollingEnabled(false);
        }
    }


    @Override
    public void moveToNextPage(int id, String scheduleName, String scheduleDate, String endDate, ScheduledInstance instance) {
        Singleton.getInstance().setIdInstance(id);

        FacilityListFragment fragment = getFacilityListFragment(EntityType.Facilitiy.getIntValue(), OperationMode.Upload.getIntValue(), instance);
        ((AppCompatActivity) getActivity()).getSupportFragmentManager().beginTransaction().replace(R.id.fragment_container, fragment).commit();
    }
}
