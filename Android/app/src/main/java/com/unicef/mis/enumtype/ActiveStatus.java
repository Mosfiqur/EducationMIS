package com.unicef.mis.enumtype;

public enum ActiveStatus {

    Deactive(1),
    Active(0);

    private int intValue;

    ActiveStatus(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static ActiveStatus fromInt(int intValue){
        for (ActiveStatus status : ActiveStatus.values()) {
            if (status.getIntValue() == intValue) { return status; }
        }
        return null;
    }
}
