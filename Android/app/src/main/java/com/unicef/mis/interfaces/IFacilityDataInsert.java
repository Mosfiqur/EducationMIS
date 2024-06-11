package com.unicef.mis.interfaces;

import com.unicef.mis.model.BeneficiaryIndicator;
import com.unicef.mis.model.facility.indicator.Indicator;

public interface IFacilityDataInsert {
    void collectData(String values, Indicator indicator);
}
