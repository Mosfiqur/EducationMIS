package com.unicef.mis.enumtype;

public enum LevelOfStudy {

    Level_1  (1),
    Level_2  (2),
    Level_3  (3),
    Level_4  (4),
    Level_5  (5);

    private int intValue;

    LevelOfStudy(int intValue) {
        this.intValue = intValue;
    }

    public int getIntValue() {
        return intValue;
    }

    public static LevelOfStudy fromInt(int intValue) {
        for (LevelOfStudy levelOfStudy : LevelOfStudy.values()) {
            if (levelOfStudy.getIntValue() == intValue) {
                return levelOfStudy;
            }
        }
        return null;
    }
}
