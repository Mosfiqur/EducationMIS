package com.unicef.mis.enumtype;

public enum EntityType {
    Beneficiary (1),
    Facilitiy (2);

    private int intValue;

    EntityType(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }
}
