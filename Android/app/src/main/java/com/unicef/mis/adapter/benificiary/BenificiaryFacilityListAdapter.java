package com.unicef.mis.adapter.benificiary;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.cardview.widget.CardView;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.listner.IMoveToNextPage;
import com.unicef.mis.model.benificiary.facility_list.FacilityList;

import java.util.List;

public class BenificiaryFacilityListAdapter extends RecyclerView.Adapter<BenificiaryFacilityListAdapter.ViewHolder> {

    private List<FacilityList> facilityList;
    private Context context;
    private IMoveToNextPage listener;

    private BenificiaryFacilityListAdapter(List<FacilityList> facilityList, Context context, IMoveToNextPage listener){
        this.facilityList = facilityList;
        this.context = context;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater= LayoutInflater.from(parent.getContext());
        View listAdapter=inflater.inflate(R.layout.facility_main_questionear_single,parent,false);
        BenificiaryFacilityListAdapter.ViewHolder viewHolder=new BenificiaryFacilityListAdapter.ViewHolder(listAdapter);
        context = parent.getContext();
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {

    }

    @Override
    public int getItemCount() {
        return facilityList.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        AppCompatTextView header_tv;
        CardView questioner_group_card;
        public ViewHolder(@NonNull View itemView) {
            super(itemView);
        }
    }
}
