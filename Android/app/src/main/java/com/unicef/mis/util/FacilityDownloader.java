package com.unicef.mis.util;

import android.view.View;

import com.unicef.mis.R;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.dataaccess.ListDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.ListObject;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.Schedule;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.util.BaseActivity.hideWait;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class FacilityDownloader {
    private View view;
    private SQLiteDatabaseHelper dbHelper;

    public FacilityDownloader(View view){
        dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
        this.view = view;
    }

    public void startFacilityDownload() {
        showWait("Downloading...", view.getContext());
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                Schedule schedule = (Schedule) o;
                saveSelectedInstance(schedule);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        OfflineApiCalling.callFacilityScedule(callBack);
    }

    private void saveSelectedInstance(Schedule schedule) {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                downloadIndicatorsForInstance();
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };

        FacilityDataAccess facilityDataAccess = dbHelper.getFacilityDataAccess();
        ScheduledInstance selectedInstance = null;
        for (ScheduledInstance instance : schedule.getData()) {
            if (instance.getId().equals(Singleton.getInstance().getIdInstance())) {
                selectedInstance = instance;
                break;
            }
        }
        facilityDataAccess.saveOrUpdateInstance(selectedInstance, callBack);
    }

    private void downloadIndicatorsForInstance() {
        int selectedInstanceId = Singleton.getInstance().getIdInstance();
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<IndicatorNew> response = (PagedResponse<IndicatorNew>) o;
                saveFacilityIndicatorLists(response.getData());
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }

        };

        OfflineApiCalling.callFacilityIndicator(selectedInstanceId, callBack);
    }

    private List<ListObject> getListObjects(List<IndicatorNew> indicators) {
        ArrayList<ListObject> listObjects = new ArrayList<>();
        for (IndicatorNew indicator : indicators) {
            if (indicator.getListObject() != null) {
                listObjects.add(indicator.getListObject());
            }
        }
        return listObjects;
    }

    private void saveFacilityIndicatorLists(List<IndicatorNew> indicatorList) {

        if (indicatorList == null || indicatorList.size() == 0) {
            showError(UnicefApplication.getResourceString(R.string.no_instance_indicators));
            return;
        }

        List<ListObject> lists = getListObjects(indicatorList);
        ListDataAccess listDataAccess = dbHelper.getListDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                listDataAccess.saveOrUpdateLists(lists);
                return null;
            }
        });

        promise.then(res -> {
            saveFacilityInstanceIndicators(indicatorList);
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void saveFacilityInstanceIndicators(List<IndicatorNew> indicatorList) {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                downloadFacilities();
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        FacilityDataAccess facilityDataAccess = dbHelper.getFacilityDataAccess();
        facilityDataAccess.saveFacilityIndicators(Singleton.getInstance().getIdInstance(), indicatorList, callBack);
    }

    private void downloadFacilities() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<FacilityListDatum> facilityList = (PagedResponse<FacilityListDatum>) o;
                saveFacilities(facilityList);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        OfflineApiCalling.getAllFacility(Singleton.getInstance().getIdInstance(), callBack);
    }

    private void saveFacilities(PagedResponse<FacilityListDatum> facilityListData) {
        if (facilityListData == null) return;

        FacilityDataAccess facilityDataAccess = dbHelper.getFacilityDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                facilityDataAccess.saveOrUpdateFacilityList(facilityListData.getData());
                return null;
            }
        });

        promise.then(res -> {
            downloadFacilitiesRecords();
            return null;
        }).error(err -> {
            showError(err.toString());
        });

    }

    private void downloadFacilitiesRecords() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<FacilityListDatum> response = (PagedResponse<FacilityListDatum>) o;
                saveFacilitiesRecords(response.getData());
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        OfflineApiCalling.getFacilityRecords(Singleton.getInstance().getIdInstance(), callBack);
    }

    private void saveFacilitiesRecords(List<FacilityListDatum> facilities) {
        FacilityDataAccess dataAccess = new FacilityDataAccess();
        int instanceId = Singleton.getInstance().getIdInstance();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                return dataAccess.saveRecords(instanceId, facilities);
            }
        });
        promise.then(res -> {

            downloadFacilityGetById(facilities);
            return null;
        }).error(err -> {
            showError("Failed to save facilities records");
        });
    }

    private void downloadFacilityGetById(List<FacilityListDatum> facilities) {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                FacilityListDatum result = (FacilityListDatum) o;
                saveFacilityById(result);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {

            }
        };

        for (int idValue=0; idValue< facilities.size(); idValue++)
        {
            //TODO: Please make sure if this is the right param
            int selectedInstanceId = Singleton.getInstance().getIdInstance();
            OfflineApiCalling.getFacilityById(facilities.get(idValue).getId(), selectedInstanceId, callBack);
        }



    }

    private void saveFacilityById(FacilityListDatum result) {
        FacilityDataAccess facilityDataAccess = new FacilityDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                facilityDataAccess.saveFacilityGetById(result);
                return null;
            }
        });

        promise.then(res ->{
            hideWait();
            showToast("All records successfully downloaded");
            return null;
        }).error(err ->{
            showError("Failed to save facility By Id");
        });
    }

}
