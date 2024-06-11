package com.unicef.mis.enumtype;

public enum DataTypes {
    Int(1),
    Text(2),
    Decimal(3),
    Datetime(4),
    Boolean(5),
    List(6);

    private int intValue;

    DataTypes(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static DataTypes fromInt(int intValue) {
        for (DataTypes dataType : DataTypes.values()) {
            if (dataType.getIntValue() == intValue) {
                return dataType;
            }
        }
        return null;
    }
}
