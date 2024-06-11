package com.unicef.mis.adapter.selection;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.RelativeLayout;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.R;
import com.unicef.mis.interfaces.ISelectCamp;
import com.unicef.mis.model.Camp;

import java.util.List;

public class CampAdapter extends RecyclerView.Adapter<CampAdapter.Viewholder> {

    public List<Camp> campDatum;
    public Context context;
    public ISelectCamp listener;

    public CampAdapter(List<Camp> campDatum, Context context, ISelectCamp listener) {
        this.campDatum = campDatum;
        this.context = context;
        this.listener = listener;
    }

    @NonNull
    @Override
    public Viewholder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater= LayoutInflater.from(parent.getContext());
        View listAdapter=inflater.inflate(R.layout.selection_item,parent,false);
        CampAdapter.Viewholder viewHolder=new CampAdapter.Viewholder(listAdapter);
        context = parent.getContext();
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull Viewholder holder, int position) {
        holder.name_tv.setText(campDatum.get(position).getName());
        holder.selection.setOnClickListener(view -> {
            listener.selectionCamp(campDatum.get(position).getName(), campDatum.get(position).getId());
        });
    }

    @Override
    public int getItemCount() {
        return campDatum.size();
    }

    public class Viewholder extends RecyclerView.ViewHolder {
        RelativeLayout selection;
        AppCompatTextView name_tv;
        public Viewholder(@NonNull View itemView) {
            super(itemView);
            selection = itemView.findViewById (R.id.selection);
            name_tv = itemView.findViewById (R.id.name_tv);
        }
    }
}
