package com.unicef.mis.listner;

import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;

import java.util.List;

public interface IFacilityListListener {
    void facilityList(List<FacilityListDatum> data);
}
