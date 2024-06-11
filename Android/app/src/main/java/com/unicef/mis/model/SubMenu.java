package com.unicef.mis.model;

public class SubMenu {
    private String name = "";
    private int id = -1;

    /**
     * public constructor for Country class
     *
     * @param name
     * @param id
     */
    public SubMenu(String name, int id) {
        this.name = name;
        this.id = id;
    }

    /**
     * method return country name
     *
     * @return
     */
    public String getName() {
        return name;
    }

    /**
     * method set country name
     *
     * @param name
     */
    public void setName(String name) {
        this.name = name;
    }

    /**
     * method used to get country id
     *
     * @return
     */
    public int getId() {
        return id;
    }

    /**
     * method used to set country id
     *
     * @param id
     */
    public void setId(int id) {
        this.id = id;
    }

}
