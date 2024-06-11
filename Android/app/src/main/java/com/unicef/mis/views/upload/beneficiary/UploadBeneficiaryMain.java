package com.unicef.mis.views.upload.beneficiary;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.FrameLayout;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;

import com.unicef.mis.R;
import com.unicef.mis.util.BaseFragment;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.util.Singleton;

public class UploadBeneficiaryMain extends BaseFragment {
    private View view;
    private FrameLayout fragmentContainer;



    @Nullable
    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        view = inflater.inflate(R.layout.fragment_main, container, false);
        Singleton.getInstance().setContext(getActivity());
        initViews();
        initListeners();
        return view;
    }

    public void initViews(){
        fragmentContainer = view.findViewById (R.id.fragment_container);
    }

    public void initListeners(){
        //call home fragment
        replaceFragment( new UploadBeneficiarySchedule(), ApplicationConstants.HOME_FRAGMENT,  ApplicationConstants.HOME_FRAGMENT, fragmentContainer.getId());
    }
}
