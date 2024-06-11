package com.unicef.mis.enumtype;

public enum GenderTypes {
    Male(1),
    Female(2),
    Others(3);

    private int intValue;

    GenderTypes(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static GenderTypes fromInt(int intValue) {
        for (GenderTypes genderTypes : GenderTypes.values()) {
            if (genderTypes.getIntValue() == intValue) {
                return genderTypes;
            }
        }
        return null;
    }
}
