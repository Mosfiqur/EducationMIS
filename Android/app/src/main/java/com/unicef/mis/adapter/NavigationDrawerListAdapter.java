package com.unicef.mis.adapter;

import android.content.Context;
import android.database.DataSetObserver;
import android.graphics.Typeface;
import android.util.DisplayMetrics;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseExpandableListAdapter;

import androidx.appcompat.widget.AppCompatImageView;
import androidx.appcompat.widget.AppCompatTextView;

import com.unicef.mis.R;
import com.unicef.mis.model.Menu;
import com.unicef.mis.model.SubMenu;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class NavigationDrawerListAdapter extends BaseExpandableListAdapter {
    Context context = null;
    ArrayList<Menu> originalMenuList;
    ArrayList<Menu> menuList;

    public NavigationDrawerListAdapter(Context context, ArrayList<Menu> data) {
        this.context = context;
        this.originalMenuList = new ArrayList<Menu>();
        this.originalMenuList.addAll(data);

        this.menuList = new ArrayList<Menu>();
        this.menuList.addAll(data);
    }



    @Override
    public void registerDataSetObserver(DataSetObserver dataSetObserver) {

    }

    @Override
    public void unregisterDataSetObserver(DataSetObserver dataSetObserver) {

    }

    @Override
    public void onGroupExpanded(int i) {

    }

    @Override
    public void onGroupCollapsed(int i) {

    }

    @Override
    public boolean isEmpty() {
        if (menuList.size() == 0)
            return true;
        else
            return false;
    }

    @Override
    public int getGroupCount() {
        return menuList.size();
    }

    @Override
    public int getChildrenCount(int i) {
        if (menuList.get(i).getSubMenu() == null){
            return 0;
        }
        else {
            return menuList.get(i).getSubMenu().size();
        }

    }

    @Override
    public Menu getGroup(int i) {

        return menuList.get(i);
    }

    @Override
    public SubMenu getChild(int groupPosition, int childPosition) {
        return menuList.get(groupPosition).getSubMenu().get(childPosition);
    }

    @Override
    public long getGroupId(int i) {
        return i;
    }

    @Override
    public long getChildId(int i, int i1) {
        return i1;
    }

    @Override
    public boolean hasStableIds() {
        return true;
    }

    @Override
    public View getGroupView(int position, boolean b, View contentView, ViewGroup parent) {

        Menu menu = menuList.get(position);
        if (contentView == null) {
            contentView = LayoutInflater.from(context).inflate(R.layout.menu_group, parent, false);
        }
        AppCompatTextView tvContinentName = contentView.findViewById(R.id.menu_name);
        AppCompatImageView imageView = contentView.findViewById(R.id.menu_ico);
        imageView.setImageResource(menu.getImg());
        tvContinentName.setText(menu.getName());


        return contentView;
    }

    @Override
    public View getChildView(int groupPosition, int childPosition, boolean b, View contentView, ViewGroup parent) {
        SubMenu subMenu = menuList.get(groupPosition).getSubMenu().get(childPosition);
        if (contentView == null) {
            contentView = LayoutInflater.from(context).inflate(R.layout.menu_item, parent, false);
        }
        AppCompatTextView tvCountryName = contentView.findViewById(R.id.lblListItem);



        tvCountryName.setText(subMenu.getName());

        return contentView;
    }

    @Override
    public boolean isChildSelectable(int i, int i1) {
        return true;
    }

    @Override
    public boolean areAllItemsEnabled() {
        return true;
    }

    @Override
    public long getCombinedChildId(long l, long l1) {
        return l1;
    }

    @Override
    public long getCombinedGroupId(long l) {
        return l;
    }
}
