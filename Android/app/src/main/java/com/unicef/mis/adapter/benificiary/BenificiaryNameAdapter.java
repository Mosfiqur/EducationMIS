package com.unicef.mis.adapter.benificiary;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.listner.IMoveToFinalBenificary;
import com.unicef.mis.model.Beneficiary;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.util.BaseActivity.showToast;

public class BenificiaryNameAdapter extends RecyclerView.Adapter<BenificiaryNameAdapter.ViewHolder> {
    private List<Beneficiary> benificiaryListData;
    private Context context;
    private IMoveToFinalBenificary listener;

    public BenificiaryNameAdapter(List<Beneficiary> benificiaryNameListData, Context context, IMoveToFinalBenificary listener) {
        this.benificiaryListData = benificiaryNameListData;
        this.context = context;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater= LayoutInflater.from(parent.getContext());
        View listAdapter=inflater.inflate(R.layout.single_benificiary_name_list,parent,false);
        BenificiaryNameAdapter.ViewHolder viewHolder=new BenificiaryNameAdapter.ViewHolder(listAdapter);
        context = parent.getContext();
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.name_tv.setText(benificiaryListData.get(position).getBeneficiaryName());
        holder.id_tv.setText(String.valueOf(benificiaryListData.get(position).getUnhcrId()));
        if (benificiaryListData.get(position).getCollectionStatus() == CollectionStatus.NotCollected.getIntValue()){
            holder.status_tv.setText("Not Collected");
            holder.status_tv.setBackgroundDrawable(context.getResources().getDrawable(R.drawable.not_collected_rectangle));
        } else if (benificiaryListData.get(position).getCollectionStatus() == CollectionStatus.Collected.getIntValue()){
            holder.status_tv.setText("Collected");
            holder.status_tv.setBackgroundDrawable(context.getResources().getDrawable(R.drawable.collected_rectangle));
        } else if (benificiaryListData.get(position).getCollectionStatus() == CollectionStatus.Approved.getIntValue()){
            holder.status_tv.setText("Approved");
            holder.status_tv.setBackgroundDrawable(context.getResources().getDrawable(R.drawable.collected_rectangle));
        } else if (benificiaryListData.get(position).getCollectionStatus() == CollectionStatus.Recollect.getIntValue()){
            holder.status_tv.setText("Recollect");
            holder.status_tv.setBackgroundDrawable(context.getResources().getDrawable(R.drawable.not_collected_rectangle));
        }

        holder.benificiary_name_list_relative.setOnClickListener(view ->{

            if (benificiaryListData.get(position).getCollectionStatus() == CollectionStatus.Approved.getIntValue()){
                showToast(context, "Data Alreadfy Approved");
            } else {
                listener.moveToNextPage(benificiaryListData.get(position).getUnhcrId(), benificiaryListData.get(position).getEntityId(), benificiaryListData.get(position).getBeneficiaryName());
            }
        });



    }

    @Override
    public int getItemCount() {
        return benificiaryListData.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        private AppCompatTextView name_tv, id_tv, status_tv;
        private RelativeLayout benificiary_name_list_relative;
        public ViewHolder(@NonNull View itemView) {
            super(itemView);

            name_tv = itemView.findViewById (R.id.name_tv);
            id_tv = itemView.findViewById (R.id.id_tv);
            status_tv = itemView.findViewById (R.id.status_tv);
            benificiary_name_list_relative = itemView.findViewById (R.id.benificiary_name_list_relative);
        }
    }

    public List<Beneficiary> getBenificiaryListData() {
        if(benificiaryListData == null){
            benificiaryListData = new ArrayList<Beneficiary>();
        }
        return benificiaryListData;
    }
}
