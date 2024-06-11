package com.unicef.mis.adapter;

import android.app.DatePickerDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.text.Editable;
import android.text.InputType;
import android.text.TextWatcher;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.DatePicker;
import android.widget.RelativeLayout;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.widget.AppCompatSpinner;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.material.textfield.TextInputEditText;
import com.unicef.mis.R;

import com.unicef.mis.listner.IBenificiaryFinalListner;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.ListItem;
import com.unicef.mis.model.benificiary.indicator.Indicator;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

public class BeneficiaryIndicatorAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {

    private List<Indicator> dataSet;
    private List<Beneficiary> beneficiaryGetByIdModel;
    private Context mContext;
    private Date date;
    private Calendar myCalendar;

    public IBenificiaryFinalListner listener;

    private int mYear, mMonth, mDay;

    String recordValue = "";

    public BeneficiaryIndicatorAdapter(List<Indicator> dataSet, List<Beneficiary> beneficiaryGetByIdModel, Context mContext, IBenificiaryFinalListner listener) {
        this.dataSet = dataSet;
        this.mContext = mContext;
        this.listener = listener;
        this.beneficiaryGetByIdModel = beneficiaryGetByIdModel;
    }

    public static class EdittextViewHolder extends RecyclerView.ViewHolder{
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, save_tv;
        TextInputEditText answer_tf;
        public EdittextViewHolder(@NonNull View itemView) {
            super(itemView);

            this.indicator_number = itemView.findViewById (R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById (R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById (R.id.indicator_bn_tv);
            this.answer_tf = itemView.findViewById (R.id.answer_tf);
            this.save_tv = itemView.findViewById (R.id.save_tv);



        }
    }

    public static class DatePickerHolder extends RecyclerView.ViewHolder{
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, select_date_tv, save_tv;
        RelativeLayout transaction_date_layout;

        public DatePickerHolder(@NonNull View itemView) {
            super(itemView);

            this.indicator_number = itemView.findViewById (R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById (R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById (R.id.indicator_bn_tv);
            this.select_date_tv = itemView.findViewById (R.id.select_date_tv);
            this.transaction_date_layout = itemView.findViewById (R.id.transaction_date_layout);
            this.save_tv = itemView.findViewById (R.id.save_tv);
        }
    }

    public static class SpinnerPickerHolder extends RecyclerView.ViewHolder{
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, save_tv;
//        AppCompatSpinner answerSpinner;
        AppCompatTextView answerSpinner;
        public SpinnerPickerHolder(@NonNull View itemView) {
            super(itemView);
            this.indicator_number = itemView.findViewById (R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById (R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById (R.id.indicator_bn_tv);
            this.answerSpinner = itemView.findViewById (R.id.answerSpinner);
            this.save_tv = itemView.findViewById (R.id.save_tv);
        }
    }

    public static class MultiSelectHolder extends RecyclerView.ViewHolder{
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, save_tv, select_data_tv;
        public MultiSelectHolder(@NonNull View itemView) {
            super(itemView);
            this.indicator_number = itemView.findViewById (R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById (R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById (R.id.indicator_bn_tv);
            this.select_data_tv = itemView.findViewById (R.id.select_data_tv);

            this.save_tv = itemView.findViewById (R.id.save_tv);
        }
    }



    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view;
        mContext = parent.getContext();

        if (viewType == 1){
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_edittext_layout, parent, false);
            return new EdittextViewHolder(view);
        }if (viewType == 2){
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_edittext_layout, parent, false);
            return new EdittextViewHolder(view);
        }if (viewType == 3){
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_edittext_layout, parent, false);
            return new EdittextViewHolder(view);
        } else if (viewType == 4){
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_datepicker_layout, parent, false);
            return new DatePickerHolder(view);
        } else if(viewType == 5){
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_spinner_layout, parent, false);
            return new SpinnerPickerHolder(view);
        } else if(viewType == 6){
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_mulilist_layout, parent, false);
            return new MultiSelectHolder(view);
        }

        return null;
    }

    @Override
    public int getItemViewType(int position) {

        switch (dataSet.get(position).getColumnDataType()) {
            case 1:
                return Indicator.INT_TYPE;
            case 2:
                return Indicator.VARCHAR_TYPE;
            case 3:
                return Indicator.DECIMAL_TYPE;
            case 4:
                return Indicator.DATETIME_TYPE;
            case 5:
                return Indicator.BOOLEAN_TYPE;
            case 6:
                return Indicator.LIST_TYPE;
            default:
                return position;
        }

    }


//    @Override
//    public long getItemId(int position) {
//        return super.getItemId(position);
//    }

    public ArrayList<Indicator> getChangedValues() {
        ArrayList<Indicator> indicators = new ArrayList<Indicator>();
        for (Indicator indicator : dataSet) {
            List<String> values = indicator.getValues();
            if (values != null && values.size() > 0) {
                String strValue = values.get(0);
                if (strValue != null && !strValue.isEmpty()) {
                    indicators.add(indicator);
                }
            }
        }
        return indicators;
    }

    @Override
    public void onBindViewHolder(@NonNull RecyclerView.ViewHolder holder, int position) {

        Indicator beneficiaryIndicator = dataSet.get(position);

        if (beneficiaryIndicator !=null){
            int i = 0;
            List<String> values = beneficiaryIndicator.getValues();


            if (beneficiaryIndicator != null) {

                switch (beneficiaryIndicator.getColumnDataType()) {

                    case Indicator.INT_TYPE:
                        ((EdittextViewHolder) holder).indicator_number.setText("Indicator" + " " + Integer.toString(beneficiaryIndicator.getEntityDynamicColumnId()));
                        ((EdittextViewHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                        ((EdittextViewHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                        ((EdittextViewHolder) holder).answer_tf.setInputType(InputType.TYPE_CLASS_NUMBER);
                        ((EdittextViewHolder) holder).save_tv.setVisibility(View.GONE);

                        if (values != null && values.size() > 0) {
                            ((EdittextViewHolder) holder).answer_tf.setText(values.get(0));
                        }

                        if (beneficiaryIndicator.getEntityDynamicColumnId() == 122 || beneficiaryIndicator.getEntityDynamicColumnId() == 132 || beneficiaryIndicator.getEntityDynamicColumnId() == 133 ||
                                beneficiaryIndicator.getEntityDynamicColumnId() == 134 || beneficiaryIndicator.getEntityDynamicColumnId() == 135){
                            ((EdittextViewHolder) holder).answer_tf.setEnabled(false);
                            if (beneficiaryIndicator.getEntityDynamicColumnId() == 132){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getFacilityName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 133){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getBeneficiaryCampName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 134){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getBlockName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 135){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getSubBlockName());
                            }
                        } else {
                            ((EdittextViewHolder) holder).answer_tf.addTextChangedListener(new TextWatcher() {
                                @Override
                                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                                }

                                @Override
                                public void onTextChanged(CharSequence s, int start, int before, int count) {

                                }

                                @Override
                                public void afterTextChanged(Editable s) {
                                    String changedValue = s.toString();
                                    if (changedValue != null && changedValue.length() > 0) {
                                        if (values != null && values.size() > 0) {
                                            values.clear();
                                            values.add(changedValue);
                                            beneficiaryIndicator.setValues(values);
                                        }

                                    }
                                }
                            });
                        }


                        break;

                    case Indicator.VARCHAR_TYPE:
                        ((EdittextViewHolder) holder).indicator_number.setText("Indicator" + " " + Integer.toString(beneficiaryIndicator.getEntityDynamicColumnId()));
                        ((EdittextViewHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                        ((EdittextViewHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                        ((EdittextViewHolder) holder).answer_tf.setInputType(InputType.TYPE_CLASS_TEXT);
                        ((EdittextViewHolder) holder).save_tv.setVisibility(View.GONE);

                        if (values != null && values.size() > 0) {
                            ((EdittextViewHolder) holder).answer_tf.setText(values.get(0));
                        }

                        if (beneficiaryIndicator.getEntityDynamicColumnId() == 122 || beneficiaryIndicator.getEntityDynamicColumnId() == 132 || beneficiaryIndicator.getEntityDynamicColumnId() == 133 ||
                                beneficiaryIndicator.getEntityDynamicColumnId() == 134 || beneficiaryIndicator.getEntityDynamicColumnId() == 135){
                            ((EdittextViewHolder) holder).answer_tf.setEnabled(false);
                            if (beneficiaryIndicator.getEntityDynamicColumnId() == 132){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getFacilityName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 133){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getBeneficiaryCampName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 134){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getBlockName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 135){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getSubBlockName());
                            }
                        } else {
                            ((EdittextViewHolder) holder).answer_tf.addTextChangedListener(new TextWatcher() {
                                @Override
                                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                                }

                                @Override
                                public void onTextChanged(CharSequence s, int start, int before, int count) {

                                }

                                @Override
                                public void afterTextChanged(Editable s) {
                                    String changedValue = s.toString();
                                    if (changedValue != null && changedValue.length() > 0) {
                                        if (values != null && values.size() > 0) {
                                            values.clear();
                                            values.add(changedValue);
                                            beneficiaryIndicator.setValues(values);
                                        }

                                    }
                                }
                            });
                        }




                        break;

                    case Indicator.DECIMAL_TYPE:
                        ((EdittextViewHolder) holder).indicator_number.setText("Indicator" + " " + Integer.toString(beneficiaryIndicator.getEntityDynamicColumnId()));
                        ((EdittextViewHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                        ((EdittextViewHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                        ((EdittextViewHolder) holder).answer_tf.setInputType(InputType.TYPE_NUMBER_FLAG_DECIMAL | InputType.TYPE_CLASS_NUMBER);
                        ((EdittextViewHolder) holder).save_tv.setVisibility(View.GONE);

                        if (values != null && values.size() > 0) {
                            ((EdittextViewHolder) holder).answer_tf.setText(values.get(0));

                        }

                        if (beneficiaryIndicator.getEntityDynamicColumnId() == 122 || beneficiaryIndicator.getEntityDynamicColumnId() == 132 || beneficiaryIndicator.getEntityDynamicColumnId() == 133 ||
                                beneficiaryIndicator.getEntityDynamicColumnId() == 134 || beneficiaryIndicator.getEntityDynamicColumnId() == 135){
                            ((EdittextViewHolder) holder).answer_tf.setEnabled(false);
                            if (beneficiaryIndicator.getEntityDynamicColumnId() == 132){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getFacilityName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 133){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getBeneficiaryCampName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 134){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getBlockName());
                            } if (beneficiaryIndicator.getEntityDynamicColumnId() == 135){
                                ((EdittextViewHolder) holder).answer_tf.setText(beneficiaryGetByIdModel.get(0).getSubBlockName());
                            }
                        } else {
                            ((EdittextViewHolder) holder).answer_tf.addTextChangedListener(new TextWatcher() {
                                @Override
                                public void beforeTextChanged(CharSequence s, int start, int count, int after) {

                                }

                                @Override
                                public void onTextChanged(CharSequence s, int start, int before, int count) {

                                }

                                @Override
                                public void afterTextChanged(Editable s) {
                                    String changedValue = s.toString();
                                    if (changedValue != null && changedValue.length() > 0) {
                                        if (values != null && values.size() > 0) {
                                            values.clear();
                                            values.add(changedValue);
                                            beneficiaryIndicator.setValues(values);
                                        }

                                    }
                                }
                            });
                        }



                        break;

                    case Indicator.DATETIME_TYPE:
                        myCalendar = Calendar.getInstance();
                        date = myCalendar.getTime();

                        ((DatePickerHolder) holder).indicator_number.setText("Indicator" + " " + Integer.toString(beneficiaryIndicator.getEntityDynamicColumnId()));
                        ((DatePickerHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                        ((DatePickerHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                        ((DatePickerHolder) holder).save_tv.setVisibility(View.GONE);

                        if (values != null && values.size() > 0) {
                            ((DatePickerHolder) holder).select_date_tv.setText(values.get(0));
                        }
                        ((DatePickerHolder) holder).transaction_date_layout.setOnClickListener(view -> {
                            Calendar calendar = Calendar.getInstance();
                            DatePickerDialog datePickerDialog = new DatePickerDialog(mContext, new DatePickerDialog.OnDateSetListener() {

                                        @Override
                                        public void onDateSet(DatePicker view, int year,
                                                              int monthOfYear, int dayOfMonth) {
                                            String[] MONTHS = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
                                            String date_month_year = String.format("%02d",dayOfMonth) + "-" + MONTHS[monthOfYear] + "-" + year;
                                            if (values != null && values.size() > 0) {
                                                values.clear();
                                                values.add(date_month_year);
                                                beneficiaryIndicator.setValues(values);
                                            }

                                            ((DatePickerHolder) holder).select_date_tv.setText(date_month_year);

                                        }
                                    }, calendar.get(Calendar.YEAR), calendar.get(Calendar.MONTH), calendar.get(Calendar.DAY_OF_MONTH));
                            datePickerDialog.show();
                        });

                        if (values != null && values.size() > 0) {

                        }

                        break;

                    case Indicator.BOOLEAN_TYPE:
                        ArrayList<String> answer = new ArrayList<>();

                        answer.add("Yes");
                        answer.add("No");

                        ((SpinnerPickerHolder) holder).indicator_number.setText("Indicator" + " " + Integer.toString(beneficiaryIndicator.getEntityDynamicColumnId()));
                        ((SpinnerPickerHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                        ((SpinnerPickerHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());

                        ((SpinnerPickerHolder) holder).save_tv.setVisibility(View.GONE);

                        if (values != null && values.size() > 0) {
                            ((SpinnerPickerHolder) holder).answerSpinner.setText(values.get(0));

                        }

                        ((SpinnerPickerHolder) holder).answerSpinner.setOnClickListener(v-> {
                            AlertDialog.Builder builderSingle = new AlertDialog.Builder(mContext);

                            ArrayAdapter<String> arrayAdapter;
                            arrayAdapter = new ArrayAdapter<String>(mContext, android.R.layout.simple_list_item_single_choice, answer);
                            boolean[] checkedItems = new boolean[answer.size()];
                            int checkedPosition = -1;
                            for (int j = 0; j < checkedItems.length; j++) {
                                if (checkedItems[j]) {
                                    checkedPosition = j;
                                    break;
                                }
                            }

                            int valueChecked = 0;

                            if (((SpinnerPickerHolder) holder).answerSpinner.getText().toString().equalsIgnoreCase("YES")){
                                valueChecked = 0;
                            } else if (((SpinnerPickerHolder) holder).answerSpinner.getText().toString().equalsIgnoreCase("No")){
                                valueChecked = 1;
                            }

                            builderSingle.setSingleChoiceItems(arrayAdapter, valueChecked, new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int position) {
                                    if (position ==0){
                                        values.clear();
                                        values.add("Yes");
                                        beneficiaryIndicator.setValues(values);
                                        ((SpinnerPickerHolder) holder).answerSpinner.setText("Yes");

                                    } else {
                                        values.clear();
                                        values.add("No");
                                        beneficiaryIndicator.setValues(values);
                                        ((SpinnerPickerHolder) holder).answerSpinner.setText("No");
                                    }

                                    dialog.dismiss();

                                }
                            }).setNegativeButton("cancel", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            });
                            builderSingle.show();
                        });
                        break;



//                        if (values != null && values.size() > 0) {
//                            String booleanValue = values.get(0);
//                            if (booleanValue.toLowerCase().equals("yes")) {
//                                ((SpinnerPickerHolder) holder).answerSpinner.setSelection(0);
//                            } else {
//                                ((SpinnerPickerHolder) holder).answerSpinner.setSelection(1);
//                            }
//                        }
//                        ((SpinnerPickerHolder) holder).answerSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
//                            @Override
//                            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
//                                String yesNoAnswer = parent.getItemAtPosition(position).toString();
//                                String boolValue = yesNoAnswer.toLowerCase().equals("yes") ? "yes" : "no";
//                                if (values != null && values.size() > 0) {
//                                    values.clear();
//                                    values.add(boolValue);
//                                    beneficiaryIndicator.setValues(values);
//                                }
//
//
////                                if (position == 0){
////                                    onNothingSelected(parent);
////                                } else {
////
////                                }
//
//                            }
//
//                            @Override
//                            public void onNothingSelected(AdapterView<?> parent) {
//                            }
//                        });



                    case Indicator.LIST_TYPE:
                        ((MultiSelectHolder) holder).indicator_number.setText("Indicator" + " " + String.valueOf(beneficiaryIndicator.getEntityDynamicColumnId()));
                        ((MultiSelectHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                        ((MultiSelectHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                        ((MultiSelectHolder) holder).save_tv.setVisibility(View.GONE);

                        // setting peviously selected
                        List<String> previousSelections = new ArrayList<String>();
                        if(beneficiaryIndicator.getValues() != null && beneficiaryIndicator.getValues().size() > 0){
                            for (String value:beneficiaryIndicator.getValues()) {
                                for (ListItem li : beneficiaryIndicator.getListItems()) {
                                    if(value.equals(String.valueOf(li.getValue()))){
                                        if(!previousSelections.contains(li.getTitle())) {
                                            previousSelections.add(li.getTitle());
                                        }
                                    }
                                }
                            }
                        }
                        if(previousSelections.size() > 0){
                            recordValue = android.text.TextUtils.join(", ", previousSelections);
                            ((MultiSelectHolder) holder).select_data_tv.setText(recordValue);
                        }

                        if(((MultiSelectHolder) holder).select_data_tv.hasOnClickListeners()){
                            return;
                        }

                        ((MultiSelectHolder) holder).select_data_tv.setOnClickListener(view -> {
                            AlertDialog.Builder builderSingle = new AlertDialog.Builder(mContext);
                            final List<ListItem> listItems = beneficiaryIndicator.getListItems();
                            boolean[] checkedItems = new boolean[listItems.size()];
                            ArrayList<String> multiSelectAnswer = new ArrayList<String>();
                            for (int index = 0; index < listItems.size(); index++) {
                                ListItem listItem = listItems.get(index);
                                multiSelectAnswer.add(listItem.getTitle());
                                checkedItems[index] = beneficiaryIndicator.getValues() != null && beneficiaryIndicator.getValues().contains(String.valueOf(listItem.getValue()));
                            }

                            ArrayAdapter<String> arrayAdapter;
                            List<String> selectedItems = new ArrayList<String>();
                            List<String> presentValues = beneficiaryIndicator.getValues();
                            if(presentValues != null){
                                for (String selectedValue:presentValues){
                                    for(ListItem li:listItems){
                                        if(selectedValue.equals(String.valueOf(li.getValue()))){
                                            if(!selectedItems.contains(li.getTitle())){
                                                selectedItems.add(li.getTitle());
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (beneficiaryIndicator.getMultiValued() == true) {
                                String frnames[] = multiSelectAnswer.toArray(new String[multiSelectAnswer.size()]);
                                builderSingle
                                        .setMultiChoiceItems(frnames, checkedItems,
                                        new DialogInterface.OnMultiChoiceClickListener() {
                                            public void onClick(DialogInterface dialog, int position, boolean isChecked) {
                                                ListItem listItem = listItems.get(position);
                                                if (isChecked) {
                                                    if (!selectedItems.contains(listItem.getTitle())) {
                                                        selectedItems.add(listItem.getTitle());
                                                    }
                                                } else {
                                                    if (selectedItems.contains(listItem.getTitle())) {
                                                        selectedItems.remove(listItem.getTitle());
                                                    }
                                                }
                                            }
                                        }).setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialogInterface, int position) {
                                        recordValue = android.text.TextUtils.join(", ", selectedItems);
                                        ((MultiSelectHolder) holder).select_data_tv.setText(recordValue);
                                        List<String> values = new ArrayList<String>();
                                        for (String title:selectedItems){
                                            for (ListItem li:beneficiaryIndicator.getListItems()){
                                                if(title.equals(li.getTitle())){
                                                    if(!values.contains(String.valueOf(li.getValue()))){
                                                        if (values != null && values.size() > 0) {
                                                            values.add(String.valueOf(li.getValue()));
                                                        }

                                                    }
                                                }
                                            }
                                        }
                                        beneficiaryIndicator.setValues(values);
                                    }
                                });
                            }

                            else {
                                arrayAdapter = new ArrayAdapter<String>(mContext, android.R.layout.simple_list_item_single_choice, multiSelectAnswer);
                                int checkedPosition = -1;
                                for (int j = 0; j < checkedItems.length; j++) {
                                    if (checkedItems[j]) {
                                        checkedPosition = j;
                                        break;
                                    }
                                }
                                builderSingle.setSingleChoiceItems(arrayAdapter, checkedPosition, new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialog, int position) {
                                        selectedItems.clear();
                                        ListItem selectedItem = listItems.get(position);
                                        selectedItems.add(selectedItem.getTitle());
                                    }
                                }).setPositiveButton("Ok", new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialog, int which) {
                                        String strName = "";
                                        List<String> values1 = new ArrayList<String>();
                                        if(selectedItems.size() > 0){
                                            strName = selectedItems.get(0);
                                            for (ListItem li:beneficiaryIndicator.getListItems()){
                                                if(strName.equals(li.getTitle())){
                                                    values1.add(String.valueOf(li.getValue()));
                                                }
                                            }
                                            beneficiaryIndicator.setValues(values1);
                                        }
                                        ((MultiSelectHolder) holder).select_data_tv.setText(strName);
                                    }
                                });
                            }

                            builderSingle
                                    .setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {
                                    dialog.dismiss();
                                }
                            });


                            builderSingle.show();

                        });

                        break;
                }
            }
        }
    }


//    private DialogInterface.OnDismissListener mOnDismissListener = dialog -> {
//    };


    @Override
    public int getItemCount() {
        return dataSet.size();
    }

}