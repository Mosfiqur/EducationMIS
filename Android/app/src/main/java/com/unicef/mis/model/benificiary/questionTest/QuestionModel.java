package com.unicef.mis.model.benificiary.questionTest;

public class QuestionModel {
    public static final int Edittext_TYPE=0;
    public static final int datepicker_TYPE=1;
    public static final int list_TYPE=2;
    public static final int multiselect_TYPE = 3;

    public int type;
    public int data;
    public String text;

    public int position;
    public String indicator_en, indicator_bn;

    public QuestionModel(int type, String indicator_en, String indicator_bn) {
        this.type = type;
        this.indicator_en = indicator_en;
        this.indicator_bn = indicator_bn;
    }
}
