package com.unicef.mis.factory;

import androidx.annotation.NonNull;
import androidx.lifecycle.ViewModel;
import androidx.lifecycle.ViewModelProvider;

import com.unicef.mis.repository.ScheduleOnlineRepository;
import com.unicef.mis.viewmodel.ScheduleViewModel;

public class BeneficiaryFactory extends ViewModelProvider.NewInstanceFactory{
    ScheduleOnlineRepository beneficiaryScheduleRepository;

    public BeneficiaryFactory(ScheduleOnlineRepository beneficiaryScheduleRepository) {
        this.beneficiaryScheduleRepository = beneficiaryScheduleRepository;
    }

    @NonNull
    @Override
    public <T extends ViewModel> T create(@NonNull Class<T> modelClass) {
//        return (T) new BeneficiaryScheduleViewModel(beneficiaryScheduleRepository);
        return (T) new ScheduleViewModel();
    }




}
