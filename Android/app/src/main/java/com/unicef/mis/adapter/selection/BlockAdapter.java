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
import com.unicef.mis.interfaces.ISelectBlock;
import com.unicef.mis.model.Block;

import java.util.List;

public class BlockAdapter extends RecyclerView.Adapter<BlockAdapter.ViewHolder> {
    public List<Block> blockData;
    public Context context;
    public ISelectBlock listener;

    public BlockAdapter(List<Block> blockData, Context context, ISelectBlock listener) {
        this.blockData = blockData;
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
        holder.name_tv.setText(blockData.get(position).getName());
        holder.selection.setOnClickListener(view -> {
            listener.selectionBlock(blockData.get(position).getName(), blockData.get(position).getId());
        });
    }

    @Override
    public int getItemCount() {
        return blockData.size();
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
