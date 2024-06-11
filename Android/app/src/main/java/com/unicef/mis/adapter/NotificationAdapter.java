package com.unicef.mis.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.adapter.selection.BlockAdapter;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.notification.NotificationModel;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

public class NotificationAdapter extends RecyclerView.Adapter<NotificationAdapter.ViewHolder> {

    private PagedResponse<NotificationModel> notificationModel;
    private Context context;

    public NotificationAdapter(PagedResponse<NotificationModel> notificationModel, Context context){
        this.notificationModel = notificationModel;
        this.context = context;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater= LayoutInflater.from(parent.getContext());
        View listAdapter=inflater.inflate(R.layout.activity_notification_single,parent,false);
        NotificationAdapter.ViewHolder viewHolder=new NotificationAdapter.ViewHolder(listAdapter);
        context = parent.getContext();
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.notification_tv.setText(notificationModel.getData().get(position).getDetails());

        String getDateValue = notificationModel.getData().get(position).getDateCreated();
        String[] split1 = getDateValue.split("T", 2);

        SimpleDateFormat sdf = new SimpleDateFormat("yy-MM-dd");
        SimpleDateFormat sdf1 = new SimpleDateFormat("dd MMM yyyy");

        Date d = null;
        try {
            d = sdf.parse(split1[0]);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        String s = sdf1.format(d);

        holder.notification_date.setText(s);
    }

    @Override
    public int getItemCount() {
        return notificationModel.getData().size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        private RelativeLayout notification_relative;
        private AppCompatTextView notification_tv, notification_date;

        public ViewHolder(@NonNull View itemView) {
            super(itemView);

            notification_relative = itemView.findViewById (R.id.notification_relative);
            notification_tv = itemView.findViewById (R.id.notification_tv);
            notification_date = itemView.findViewById (R.id.notification_date);
        }
    }
}
