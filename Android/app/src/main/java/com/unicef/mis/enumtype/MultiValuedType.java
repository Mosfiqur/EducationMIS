package com.unicef.mis.enumtype;

public enum  MultiValuedType {
    mutiValueType(1),
    singleValueType(0);

    private int intValue;

    MultiValuedType(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static MultiValuedType fromInt(int intValue) {
        for (MultiValuedType multiValuedType : MultiValuedType.values()) {
            if (multiValuedType.getIntValue() == intValue) {
                return multiValuedType;
            }
        }
        return null;
    }
}
