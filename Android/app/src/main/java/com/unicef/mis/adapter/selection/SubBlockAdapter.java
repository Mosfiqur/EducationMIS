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
import com.unicef.mis.interfaces.ISelectSubBlock;
import com.unicef.mis.model.SubBlock;

import java.util.List;

public class SubBlockAdapter extends RecyclerView.Adapter<SubBlockAdapter.ViewHolder>{

    public List<SubBlock> subBlockData;
    public Context context;
    public ISelectSubBlock listener;

    public SubBlockAdapter(List<SubBlock> subBlockData, Context context, ISelectSubBlock listener) {
        this.subBlockData = subBlockData;
        this.context = context;
        this.listener = listener;
    }

    @NonNull
    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        LayoutInflater inflater= LayoutInflater.from(parent.getContext());
        View listAdapter=inflater.inflate(R.layout.selection_item,parent,false);
        ViewHolder viewHolder=new ViewHolder(listAdapter);
        context = parent.getContext();
        return viewHolder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
        holder.name_tv.setText(subBlockData.get(position).getName());
        holder.selection.setOnClickListener(view -> {
            listener.selectionSubBlock(subBlockData.get(position).getName(), subBlockData.get(position).getId());
        });
    }

    @Override
    public int getItemCount() {
        return subBlockData.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        RelativeLayout selection;
        AppCompatTextView name_tv;
        public ViewHolder(@NonNull View itemView) {
            super(itemView);
            selection = itemView.findViewById (R.id.selection);
            name_tv = itemView.findViewById (R.id.name_tv);
        }
    }
}
