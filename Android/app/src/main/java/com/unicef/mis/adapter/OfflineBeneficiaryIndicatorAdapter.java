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
import android.widget.ListView;
import android.widget.RelativeLayout;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.widget.AppCompatSpinner;
import androidx.appcompat.widget.AppCompatTextView;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.material.textfield.TextInputEditText;
import com.unicef.mis.R;

import com.unicef.mis.dataaccess.ListDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.listner.IBenificiaryFinalListner;
import com.unicef.mis.listner.IBeneficiaryDataInsert;
import com.unicef.mis.model.BeneficiaryIndicator;
import com.unicef.mis.model.ListItem;
import com.unicef.mis.model.PropertiesInfoModel;
import com.unicef.mis.util.UnicefApplication;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.List;

public class OfflineBeneficiaryIndicatorAdapter extends RecyclerView.Adapter<RecyclerView.ViewHolder> {

    private List<PropertiesInfoModel> dataSet;
    private Context mContext;
    private Date date;
    private Calendar myCalendar;
    DatePickerDialog.OnDateSetListener dateListener;
    DatePickerDialog dialog;
    SimpleDateFormat dateFormatter;
    String date_month_year = "";
    public IBeneficiaryDataInsert listener;
    public List<ArrayList<String>> MainValues = new ArrayList();

    private SQLiteDatabaseHelper db;

    public String s = "";
    public int e = 0;
    String recordValue = "";
    private IBenificiaryFinalListner benificiaryFinalListner;

    public OfflineBeneficiaryIndicatorAdapter(ArrayList<PropertiesInfoModel> dataSet, Context mContext, IBeneficiaryDataInsert listener) {
        this.dataSet = dataSet;
        this.mContext = mContext;
        this.listener = listener;
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());

    }

    public OfflineBeneficiaryIndicatorAdapter(List<PropertiesInfoModel> dataSet, Context mContext, IBeneficiaryDataInsert listener, IBenificiaryFinalListner benificiaryFinalListner) {
        this.dataSet = dataSet;
        this.mContext = mContext;
        this.listener = listener;
        this.benificiaryFinalListner = benificiaryFinalListner;
        db = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
    }

    public static class EdittextViewHolder extends RecyclerView.ViewHolder {
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, save_tv;
        TextInputEditText answer_tf;

        public EdittextViewHolder(@NonNull View itemView) {
            super(itemView);

            this.indicator_number = itemView.findViewById(R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById(R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById(R.id.indicator_bn_tv);
            this.answer_tf = itemView.findViewById(R.id.answer_tf);
            this.save_tv = itemView.findViewById(R.id.save_tv);
        }
    }

    public static class DatePickerHolder extends RecyclerView.ViewHolder {
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, select_date_tv, save_tv;
        RelativeLayout transaction_date_layout;

        public DatePickerHolder(@NonNull View itemView) {
            super(itemView);

            this.indicator_number = itemView.findViewById(R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById(R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById(R.id.indicator_bn_tv);
            this.select_date_tv = itemView.findViewById(R.id.select_date_tv);
            this.transaction_date_layout = itemView.findViewById(R.id.transaction_date_layout);
            this.save_tv = itemView.findViewById(R.id.save_tv);
        }
    }

    public static class SpinnerPickerHolder extends RecyclerView.ViewHolder {
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, save_tv;
        AppCompatSpinner answerSpinner;

        public SpinnerPickerHolder(@NonNull View itemView) {
            super(itemView);
            this.indicator_number = itemView.findViewById(R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById(R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById(R.id.indicator_bn_tv);
            this.answerSpinner = itemView.findViewById(R.id.answerSpinner);
            this.save_tv = itemView.findViewById(R.id.save_tv);
        }
    }

    public static class MultiSelectHolder extends RecyclerView.ViewHolder {
        AppCompatTextView indicator_number, indicator_en_tv, indicator_bn_tv, save_tv, select_data_tv;
        ListView multiselectlist;

        public MultiSelectHolder(@NonNull View itemView) {
            super(itemView);
            this.indicator_number = itemView.findViewById(R.id.indicator_number);
            this.indicator_en_tv = itemView.findViewById(R.id.indicator_en_tv);
            this.indicator_bn_tv = itemView.findViewById(R.id.indicator_bn_tv);
            this.multiselectlist = itemView.findViewById(R.id.multiselectlist);
            this.save_tv = itemView.findViewById(R.id.save_tv);
            this.select_data_tv = itemView.findViewById (R.id.select_data_tv);
        }
    }


    @NonNull
    @Override
    public RecyclerView.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view;
        mContext = parent.getContext();

        if (viewType == 1) {
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_edittext_layout, parent, false);
            return new EdittextViewHolder(view);
        }
        if (viewType == 2) {
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_edittext_layout, parent, false);
            return new EdittextViewHolder(view);
        }
        if (viewType == 3) {
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_edittext_layout, parent, false);
            return new EdittextViewHolder(view);
        } else if (viewType == 4) {
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_datepicker_layout, parent, false);
            return new DatePickerHolder(view);
        } else if (viewType == 5) {
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_spinner_layout, parent, false);
            return new SpinnerPickerHolder(view);
        } else if (viewType == 6) {
            view = LayoutInflater.from(parent.getContext()).inflate(R.layout.single_questionear_mulilist_layout, parent, false);
            return new MultiSelectHolder(view);
        }

        return null;
    }

    @Override
    public int getItemViewType(int position) {

        switch (dataSet.get(position).getDataType()) {
            case 1:
                return BeneficiaryIndicator.INT_TYPE;
            case 2:
                return BeneficiaryIndicator.VARCHAR_TYPE;
            case 3:
                return BeneficiaryIndicator.DECIMAL_TYPE;
            case 4:
                return BeneficiaryIndicator.DATETIME_TYPE;
            case 5:
                return BeneficiaryIndicator.BOOLEAN_TYPE;
            case 6:
                return BeneficiaryIndicator.LIST_TYPE;
            default:
                return -1;
        }

    }

    public ArrayList<PropertiesInfoModel> getChangedValues() {
        ArrayList<PropertiesInfoModel> indicators = new ArrayList<>();
        indicators.addAll(dataSet);
        return indicators;
    }

    @Override
    public void onBindViewHolder(@NonNull RecyclerView.ViewHolder holder, int position) {

        PropertiesInfoModel beneficiaryIndicator = dataSet.get(position);
        if (beneficiaryIndicator != null) {
            int i = 0;
            List<String> values = beneficiaryIndicator.getValues();

            switch (beneficiaryIndicator.getDataType()) {
                case BeneficiaryIndicator.INT_TYPE:

                    ((EdittextViewHolder) holder).indicator_number.setText("Indicator" + " " + beneficiaryIndicator.getEntityColumnId());
                    ((EdittextViewHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                    ((EdittextViewHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                    ((EdittextViewHolder) holder).answer_tf.setInputType(InputType.TYPE_CLASS_NUMBER);

//                    if (Singleton.getInstance().getIntValueForTest() == 1) {
//                        ((EdittextViewHolder) holder).save_tv.setVisibility(View.GONE);
//                    }

                    ((EdittextViewHolder) holder).save_tv.setOnClickListener(view -> {
                        String intValue = ((EdittextViewHolder) holder).answer_tf.getText().toString();
                        if (intValue != null && intValue.length() > 0) {
                            listener.collectData(intValue, beneficiaryIndicator);

                        }
                    });

                    if (values != null && values.size() > 0) {
                        ((EdittextViewHolder) holder).answer_tf.setText(values.get(0));
                    }

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
                                values.clear();
                                values.add(changedValue);
                                beneficiaryIndicator.setValues(values);
                            }
                        }
                    });

                    break;

                case BeneficiaryIndicator.VARCHAR_TYPE:
                    ((EdittextViewHolder) holder).indicator_number.setText("Indicator" + " " + beneficiaryIndicator.getEntityColumnId());
                    ((EdittextViewHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                    ((EdittextViewHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                    ((EdittextViewHolder) holder).answer_tf.setInputType(InputType.TYPE_CLASS_TEXT);
//
//                    if (Singleton.getInstance().getIntValueForTest() == 1) {
//                        ((EdittextViewHolder) holder).save_tv.setVisibility(View.GONE);
//                    }

                    ((EdittextViewHolder) holder).save_tv.setOnClickListener(view -> {
                        String strValue = ((EdittextViewHolder) holder).answer_tf.getText().toString();
                        if (strValue != null && strValue.length() > 0) {
                            listener.collectData(strValue, beneficiaryIndicator);

                        }
                    });

                    if (values != null && values.size() > 0) {
                        ((EdittextViewHolder) holder).answer_tf.setText(values.get(0));
                    }

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
                                values.clear();
                                values.add(changedValue);
                                beneficiaryIndicator.setValues(values);
                            }
                        }
                    });

                    break;
                case BeneficiaryIndicator.DECIMAL_TYPE:
                    ((EdittextViewHolder) holder).indicator_number.setText("Indicator" + " " + beneficiaryIndicator.getEntityColumnId());
                    ((EdittextViewHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                    ((EdittextViewHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                    ((EdittextViewHolder) holder).answer_tf.setInputType(InputType.TYPE_CLASS_TEXT);
//
//                    if (Singleton.getInstance().getIntValueForTest() == 1) {
//                        ((EdittextViewHolder) holder).save_tv.setVisibility(View.GONE);
//                    }

                    ((EdittextViewHolder) holder).save_tv.setOnClickListener(view -> {
                        String strValue = ((EdittextViewHolder) holder).answer_tf.getText().toString();
                        if (strValue != null && strValue.length() > 0) {
                            listener.collectData(strValue, beneficiaryIndicator);

                        }
                    });

                    if (values != null && values.size() > 0) {
                        ((EdittextViewHolder) holder).answer_tf.setText(values.get(0));
                    }

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
                                values.clear();
                                values.add(changedValue);
                                beneficiaryIndicator.setValues(values);
                            }
                        }
                    });

                    break;

                case BeneficiaryIndicator.DATETIME_TYPE:

                    myCalendar = Calendar.getInstance();
                    date = myCalendar.getTime();

                    ((DatePickerHolder) holder).indicator_number.setText("Indicator" + " " + beneficiaryIndicator.getEntityColumnId());
                    ((DatePickerHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                    ((DatePickerHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());


                    ((DatePickerHolder) holder).transaction_date_layout.setOnClickListener(view -> {
                        dialog = new DatePickerDialog(mContext, dateListener, myCalendar
                                .get(Calendar.YEAR), myCalendar.get(Calendar.MONTH),
                                myCalendar.get(Calendar.DAY_OF_MONTH));
                        dialog.setOnDismissListener(mOnDismissListener);
                        dialog.setButton(DatePickerDialog.BUTTON_POSITIVE, mContext.getResources().getString(R.string.ok), dialog);
                        dialog.setButton(DatePickerDialog.BUTTON_NEGATIVE, mContext.getResources().getString(R.string.cancel), dialog
                        );

                        dialog.show();

                    });

                    dateListener = (view, year, monthOfYear, dayOfMonth) -> {
                        myCalendar.set(Calendar.YEAR, year);
                        myCalendar.set(Calendar.MONTH, monthOfYear);
                        myCalendar.set(Calendar.DAY_OF_MONTH, dayOfMonth);

                        String myFormat = mContext.getResources().getString(R.string.date_display_format); //In which you need put here
                        SimpleDateFormat sdf = new SimpleDateFormat(myFormat);

                        date_month_year = sdf.format(myCalendar.getTime());
                        ((DatePickerHolder) holder).select_date_tv.setText(date_month_year);
                        if (date_month_year != null && date_month_year.length() > 0) {
                            values.clear();
                            values.add(date_month_year);
                            beneficiaryIndicator.setValues(values);
                        }

//                        DatePickerValues.add(((DatePickerHolder) holder).select_date_tv.getText().toString());
                    };

                    if (values != null && values.size() > 0) {
                        ((DatePickerHolder) holder).select_date_tv.setText(values.get(0));
                    }

//                    if (Singleton.getInstance().getIntValueForTest() == 1) {
//                        ((DatePickerHolder) holder).save_tv.setVisibility(View.GONE);
//                    }
                    ((DatePickerHolder) holder).save_tv.setOnClickListener(view -> {
                        String dateValue = ((DatePickerHolder) holder).select_date_tv.getText().toString();
                        listener.collectData(dateValue, beneficiaryIndicator);

                    });

                    break;

                case BeneficiaryIndicator.BOOLEAN_TYPE:

                    ArrayList<String> answer = new ArrayList<>();
                    answer.add("Yes");
                    answer.add("No");

                    ArrayAdapter<String> dataAdapter = new ArrayAdapter<String>(mContext, android.R.layout.simple_spinner_item, answer);
                    dataAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
                    ((SpinnerPickerHolder) holder).indicator_number.setText("Indicator" + " " + beneficiaryIndicator.getEntityColumnId());
                    ((SpinnerPickerHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                    ((SpinnerPickerHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                    ((SpinnerPickerHolder) holder).answerSpinner.setAdapter(dataAdapter);
                    if (values != null && values.size() > 0) {
                        String booleanValue = values.get(0);
                        if (booleanValue.toLowerCase().equals("yes")) {
                            ((SpinnerPickerHolder) holder).answerSpinner.setSelection(0);
                        } else {
                            ((SpinnerPickerHolder) holder).answerSpinner.setSelection(1);
                        }
                    }
                    ((SpinnerPickerHolder) holder).answerSpinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
                        @Override
                        public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                            String yesNoAnswer = parent.getItemAtPosition(position).toString();
                            String boolValue = yesNoAnswer.toLowerCase().equals("yes") ? "yes" : "no";
                            values.clear();
                            values.add(boolValue);
                            beneficiaryIndicator.setValues(values);
                        }

                        @Override
                        public void onNothingSelected(AdapterView<?> parent) {
                        }
                    });

//                    if (Singleton.getInstance().getIntValueForTest() == 1) {
//                        ((SpinnerPickerHolder) holder).save_tv.setVisibility(View.GONE);
//                    }

                    ((SpinnerPickerHolder) holder).save_tv.setOnClickListener(view -> {
                        String boolStr = ((SpinnerPickerHolder) holder).answerSpinner.getSelectedItem().toString();
                        String strValue = boolStr.toLowerCase().equals("yes") ? "yes" : "no";
                        listener.collectData(strValue, beneficiaryIndicator);

                    });

                    break;

                case BeneficiaryIndicator.LIST_TYPE:
                    ((MultiSelectHolder) holder).indicator_number.setText("Indicator" + " " + String.valueOf(beneficiaryIndicator.getEntityColumnId()));
                    ((MultiSelectHolder) holder).indicator_en_tv.setText(beneficiaryIndicator.getColumnName());
                    ((MultiSelectHolder) holder).indicator_bn_tv.setText(beneficiaryIndicator.getColumnNameInBangla());
                    ((MultiSelectHolder) holder).save_tv.setVisibility(View.GONE);

                    // setting peviously selected
                    List<String> previousSelections = new ArrayList<String>();
                    if(beneficiaryIndicator.getValues() != null && beneficiaryIndicator.getValues().size() > 0){
                        for (String value:beneficiaryIndicator.getValues()) {
                            for (ListItem li : beneficiaryIndicator.getListItem()) {
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
                        ListDataAccess listDataAccess = db.getListDataAccess();
                        final List<ListItem> listItems = listDataAccess.getListById(beneficiaryIndicator.getColumnListId()).getItems();
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

                        if (beneficiaryIndicator.getIsMultiValued() == true) {
                            String frnames[] = multiSelectAnswer.toArray(new String[multiSelectAnswer.size()]);
                            builderSingle.setMultiChoiceItems(frnames, checkedItems,
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
                                        for (ListItem li:beneficiaryIndicator.getListItem()){
                                            if(title.equals(li.getTitle())){
                                                if(!values.contains(String.valueOf(li.getValue()))){
                                                    values.add(String.valueOf(li.getValue()));
                                                }
                                            }
                                        }
                                    }
                                    beneficiaryIndicator.setValues(values);
                                }
                            });
                        } else {
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
                                        for (ListItem li: beneficiaryIndicator.getListItem()){
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

                        builderSingle.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
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


    private DialogInterface.OnDismissListener mOnDismissListener = dialog -> {
    };


    @Override
    public int getItemCount() {
        return dataSet.size();
    }

}