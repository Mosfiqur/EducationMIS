package com.unicef.mis.model;

import java.util.ArrayList;

public class Menu {
    private int id = -1;
    private String name = "";
    private ArrayList<SubMenu> subMenu = null;
    private int img = -1;

    public Menu(int id, String name, ArrayList<SubMenu> subMenu, int img) {
        this.id = id;
        this.name = name;
        this.subMenu = subMenu;
        this.img = img;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public ArrayList<SubMenu> getSubMenu() {
        return subMenu;
    }

    public void setSubMenu(ArrayList<SubMenu> subMenu) {
        this.subMenu = subMenu;
    }

    public int getImg() {
        return img;
    }

    public void setImg(int img) {
        this.img = img;
    }
}
