package com.unicef.mis.enumtype;

public enum OperationMode {
    Online (1),
    Offline (2),
    Upload(3);

    private int intValue;

    OperationMode(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static OperationMode fromInt(int intValue){
        for (OperationMode viewMode : OperationMode .values()) {
            if (viewMode.getIntValue() == intValue) { return viewMode; }
        }
        return null;
    }
}
