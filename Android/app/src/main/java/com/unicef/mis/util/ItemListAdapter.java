package com.unicef.mis.util;

import android.content.Context;
import android.graphics.Color;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;
import androidx.databinding.ViewDataBinding;
import androidx.recyclerview.widget.RecyclerView;

import java.util.ArrayList;
import java.util.List;


public class ItemListAdapter<T> extends RecyclerView.Adapter<ItemListAdapter.BindingHolder> {
    private static String TAG = ItemListAdapter.class.getSimpleName();

    private OnItemBinding onItemBinding;

    public void setItemAtPosition(int position, T item) {
        getItemList().set(position, item);
        notifyItemChanged(position);
    }

    public void setOnItemBinding(ItemListAdapter.OnItemBinding onItemBinding) {
        this.onItemBinding = onItemBinding;
    }

    public static class BindingHolder extends RecyclerView.ViewHolder {
        private ViewDataBinding binding;
        public BindingHolder(View rowView) {
            super(rowView);
            binding = DataBindingUtil.bind(rowView);
        }
        public ViewDataBinding getBinding() {
            return binding;
        }
    }

    private boolean willAllowMultiChoice = false;
    public void setWillAllowMultiChoice(boolean willAllowMultiChoice) {
        this.willAllowMultiChoice = willAllowMultiChoice;
    }

    private boolean multiChoiceMode;
    private Context context;
    private List<T> itemList;
    private List<T> selectedItemList= new ArrayList<>();
    public int layoutResourceId;
    private int dataBindingVariableId;

    public ItemListAdapter(Context context, List<T> itemList, int layoutResourceId, int dataBindingVariableId) {
        this.context = context;
        this.itemList = itemList;
        this.layoutResourceId = layoutResourceId;
        this.dataBindingVariableId = dataBindingVariableId;
    }

    public List<T> getItemList() {
        return itemList;
    }

    @Override
    public BindingHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext())
                .inflate(layoutResourceId,parent,false);
        return new BindingHolder(view);
    }

    @Override
    public void onBindViewHolder(BindingHolder holder, int position) {
        T item = itemList.get(position);
        ViewDataBinding binding = holder.getBinding();
        binding.setVariable(dataBindingVariableId,item);
        if(onItemBinding != null){
            onItemBinding.setBindings(binding);
        }

        if(willAllowMultiChoice) {
            setMultiChoiceModeOperation(holder, item);
        }
        holder.getBinding().executePendingBindings();
    }

    private void setMultiChoiceModeOperation(final BindingHolder holder, final T item) {
        holder.getBinding().getRoot().setOnLongClickListener(new View.OnLongClickListener() {
            @Override
            public boolean onLongClick(View view) {
                multiChoiceMode = true;
                return false;
            }
        });
        holder.getBinding().getRoot().setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(!multiChoiceMode)return;
                processMultiItemSelection(item, holder);
            }
        });
    }

    private void processMultiItemSelection(T item, BindingHolder holder) {
        if(selectedItemList.contains(item)){
            selectedItemList.remove(item);
            holder.getBinding().getRoot().setBackgroundColor(Color.parseColor("#ffffff"));
        }else {
            selectedItemList.add(item);
            holder.getBinding().getRoot().setBackgroundColor(Color.parseColor("#479480"));
        }
    }

    @Override
    public int getItemCount() {
        return itemList.size();
    }

    public interface OnItemBinding{
        void setBindings(ViewDataBinding dataBinding);
    }
}
