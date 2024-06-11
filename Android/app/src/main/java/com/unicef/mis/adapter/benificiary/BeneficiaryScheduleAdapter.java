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
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

public class BeneficiaryScheduleAdapter extends RecyclerView.Adapter<BeneficiaryScheduleAdapter.ViewHolder> {
    private List<ScheduledInstance> instanceList;
    private Context context;
    private IMoveToNextPage listner;

    public BeneficiaryScheduleAdapter(List<ScheduledInstance> getSchedule) {
        this.instanceList = getSchedule;
    }

    public BeneficiaryScheduleAdapter(List<ScheduledInstance> getSchedule, Context context, IMoveToNextPage listner) {
        this.instanceList = getSchedule;
        this.context = context;
        this.listner = listner;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater= LayoutInflater.from(parent.getContext());
        View listAdapter=inflater.inflate(R.layout.facility_main_single,parent,false);
        BeneficiaryScheduleAdapter.ViewHolder viewHolder=new BeneficiaryScheduleAdapter.ViewHolder(listAdapter);
        context = parent.getContext();
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.shedule_value.setText(instanceList.get(position).getTitle());


        String getDateValue = instanceList.get(position).getDataCollectionDate();
        String getEndValue = instanceList.get(position).getEndDate();
        String[] split1 = getDateValue.split("T", 2);
        String[] split2 = getEndValue.split("T", 2);

        SimpleDateFormat sdf = new SimpleDateFormat("yy-MM-dd");
        SimpleDateFormat sdf1 = new SimpleDateFormat("dd MMM yyyy");

        Date d = null;
        try {
            d = sdf.parse(split1[0]);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        Date dEnd = null;
        try {
            dEnd = sdf.parse(split2[0]);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        String s = sdf1.format(d);
        String s1 = sdf1.format(dEnd);


        holder.month_value.setText(s);
        holder.end_month_value.setText(s1);


        holder.faciluty_main_card_single.setOnClickListener(view -> {
            ScheduledInstance instance = instanceList.get(position);
            listner.moveToNextPage(instance.getId(), instanceList.get(position).getTitle(), s, s1, instance);
        });
    }

    @Override
    public int getItemCount() {
        return instanceList.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        AppCompatTextView shedule_value, month_value, end_month_value;
        CardView faciluty_main_card_single;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);

            shedule_value = itemView.findViewById (R.id.schedule_value);
            month_value = itemView.findViewById (R.id.month_value);
            end_month_value = itemView.findViewById (R.id.end_month_value);
            faciluty_main_card_single = itemView.findViewById (R.id.faciluty_main_card_single);
        }
    }
}
