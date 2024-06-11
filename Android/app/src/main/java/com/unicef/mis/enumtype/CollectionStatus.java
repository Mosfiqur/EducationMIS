package com.unicef.mis.enumtype;

public enum CollectionStatus {
    NoStatus(0),
    NotCollected(1),
    Collected(2),
    Approved(3),
    Requested_Inactive(6),
    Recollect(4);

    private int intValue;

    CollectionStatus(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static CollectionStatus fromInt(int intValue){
        for (CollectionStatus status : CollectionStatus.values()) {
            if (status.getIntValue() == intValue) { return status; }
        }
        return null;
    }
}
