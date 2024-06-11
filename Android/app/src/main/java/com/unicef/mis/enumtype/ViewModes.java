package com.unicef.mis.enumtype;

public enum ViewModes {
    ReadOnly(0),
    Edit(1),
    SingleSelect(2),
    MultiSelect(3),
    Create(4),
    Online(5),
    Offline(6),
    Upload(7);

    private int intValue;

    ViewModes(int intValue){
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static ViewModes fromInt(int intValue){
        for (ViewModes viewMode : ViewModes .values()) {
            if (viewMode.getIntValue() == intValue) { return viewMode; }
        }
        return null;
    }
}
