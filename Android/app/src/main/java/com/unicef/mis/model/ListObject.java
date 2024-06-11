package com.unicef.mis.model;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.ArrayList;
import java.util.List;

public class ListObject {
    @SerializedName("id")
    @Expose
    private long id;
    @SerializedName("name")
    @Expose
    private String name;
    @SerializedName("listItems")
    @Expose
    private List<ListItem> items;

    public ListObject(){
        items = new ArrayList<ListItem>();
    }

    public ListObject(long listId, String listName, List<ListItem> listItems) {
        this.id = listId;
        this.name = listName;
        this.items = new ArrayList<ListItem>();
        this.items.addAll(listItems);
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public List<ListItem> getItems() {
        return items;
    }

    public void setItems(List<ListItem> items) {
        this.items = items;
    }
}
