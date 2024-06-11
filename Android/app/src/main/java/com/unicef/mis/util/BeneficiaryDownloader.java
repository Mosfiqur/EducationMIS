package com.unicef.mis.util;

import android.util.Log;
import android.view.View;

import com.unicef.mis.R;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.dataaccess.BeneficiaryDataAccess;
import com.unicef.mis.dataaccess.BlockDataAccess;
import com.unicef.mis.dataaccess.CampDataAccess;
import com.unicef.mis.dataaccess.FacilityDataAccess;
import com.unicef.mis.dataaccess.ListDataAccess;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.dataaccess.SubBlockDataAccess;
import com.unicef.mis.interfaces.IAsyncMethod;
import com.unicef.mis.interfaces.IGenericApiCallBack;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.Block;
import com.unicef.mis.model.Camp;
import com.unicef.mis.model.IndicatorNew;
import com.unicef.mis.model.ListObject;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.SubBlock;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.Schedule;

import java.util.ArrayList;
import java.util.List;

import static com.unicef.mis.util.BaseActivity.hideWait;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class BeneficiaryDownloader {
    private static String TAG = BeneficiaryDownloader.class.getSimpleName();
    private View view;
    private SQLiteDatabaseHelper dbHelper;
    private List<FacilityListDatum> beneficiaryFacilities;
    private int instanceId;
    private int totalCount = 0;

    public BeneficiaryDownloader(int instanceId, View view){
        this.instanceId = instanceId;
        dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
        this.view = view;
    }

    public void startBeneficiaryDownload() {
        downloadAllCamps();
    }

    private void downloadAllCamps() {
        showWait("Downloading....", view.getContext());
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<Camp> pagedResponse = (PagedResponse<Camp>) o;
                saveCamps(pagedResponse);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };

        OfflineApiCalling.callCamp(callBack);
    }

    private void saveCamps(PagedResponse<Camp> response) {
        CampDataAccess campDataAccess = dbHelper.getCampDataAccess();
        List<Camp> campList = response.getData();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                return campDataAccess.saveOrUpdateCamp(campList);
            }
        });
        promise.then(res -> {
            downloadBlock();
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void downloadBlock() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<Block> pagedResponse = (PagedResponse<Block>) o;
                saveBlocks(pagedResponse);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        OfflineApiCalling.callBlock(callBack);
    }

    private void saveBlocks(PagedResponse<Block> response) {
        BlockDataAccess blockDataAccess = dbHelper.getBlockDataAccess();
        List<Block> blockList = response.getData();

        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                blockDataAccess.saveOrUpdateBlock(blockList);
                return null;
            }
        });

        promise.then(res -> {
            downloadSubBlockFromBlock();
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void downloadSubBlockFromBlock() {

        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<SubBlock> pagedResponse = (PagedResponse<SubBlock>) o;
                saveSubBlock(pagedResponse);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };

        OfflineApiCalling.callSubBlock(callBack);
    }

    private void saveSubBlock(PagedResponse<SubBlock> response) {
        SubBlockDataAccess subBlockDataAccess = dbHelper.getSubBlockDataAccess();
        List<SubBlock> subBlocks = response.getData();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                subBlockDataAccess.saveOrUpdateSubBlock(subBlocks);
                return null;
            }
        });

        promise.then(res -> {
            downloadScheduledInstances();
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void downloadScheduledInstances() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                Schedule schedule = (Schedule) o;
                saveScheduleInstance(schedule);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        OfflineApiCalling.callBenificaryScedule(callBack);
    }

    private void saveScheduleInstance(Schedule schedule) {
        BeneficiaryDataAccess beneficiaryDataAccess = dbHelper.getBeneficiaryDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.saveOrUpdateBeneficiarySchedule(schedule);
                return null;
            }
        });

        promise.then(res -> {
            downloadInstanceIndicators();
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void downloadInstanceIndicators() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<IndicatorNew> response = (PagedResponse<IndicatorNew>) o;
                saveBeneficiaryIndicatorLists(response.getData());
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }

        };

        OfflineApiCalling.callFacilityIndicator(this.instanceId, callBack);
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

    private void saveBeneficiaryIndicatorLists(List<IndicatorNew> indicatorList) {
        if (indicatorList == null || indicatorList.size() == 0) {
//            showError("There is no indicators for the instance");
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
            saveBeneficiaryInstanceIndicators(indicatorList);
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void saveBeneficiaryInstanceIndicators(List<IndicatorNew> indicatorList) {
        BeneficiaryDataAccess dataAccess = dbHelper.getBeneficiaryDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                dataAccess.saveBeneficiaryIndicators(instanceId, indicatorList);
                return null;
            }
        });

        promise.then(res -> {
            downloadBeneficiaryFacilities();
            return null;
        }).error(err -> {
            showError(err);
        });
    }


    private void downloadBeneficiaryFacilities() {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<FacilityListDatum> facilityList = (PagedResponse<FacilityListDatum>) o;
                saveBeneficiaryFacilities(facilityList);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showError(errorMessage);
            }
        };
        OfflineApiCalling.getAllFacilityForBeneficiaries(Singleton.getInstance().getIdInstance(), callBack);
    }

    private void saveBeneficiaryFacilities(PagedResponse<FacilityListDatum> facilityListData) {
        if (facilityListData == null) {
            hideWait();
            return;
        }

        beneficiaryFacilities = facilityListData.getData();
        FacilityDataAccess facilityDataAccess = dbHelper.getFacilityDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                facilityDataAccess.saveOrUpdateFacilityList(facilityListData.getData());
                return null;
            }
        });

        promise.then(res -> {
            downloadBeneficiaries();
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private void downloadBeneficiaries() {
        ArrayList<Promise> allPromises = new ArrayList<Promise>();
        for (FacilityListDatum facility : beneficiaryFacilities) {
            allPromises.add(downloadFacilityBeneficiaries(facility));
        }

        Promise[] promiseArray = allPromises.toArray(new Promise[allPromises.size()]);
        Promise.all(promiseArray).then(results->{
            Log.i(TAG, "Total: " + totalCount);
//            hideWait();
//            showToast(view.getContext(), "Downloaded successfully");
            return null;
        }).error(errors->{
            Log.i(TAG, "Total: " + totalCount);
            showError("An error occurred during download and save");
        });
    }

    private Promise downloadFacilityBeneficiaries(FacilityListDatum facility) {
        Promise promise = new Promise();
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                PagedResponse<Beneficiary> nameListModel = (PagedResponse<Beneficiary>) o;
                saveBeneficiaries(instanceId, nameListModel.getData(), promise);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {
                showToast(errorMessage);
                promise.reject(new Exception(errorMessage));
            }
        };
        QueryParamModel queryParam = new QueryParamModel();
        queryParam.setPageNumber(UIConstants.DEFAULT_PAGE_NUMBER);
        queryParam.setPageSize(Integer.MAX_VALUE);
        OfflineApiCalling.callBenificariesWithRecords(instanceId, facility.getId(), queryParam , callBack);
        return promise;
    }

    private void saveBeneficiaries(int instanceId, List<Beneficiary> beneficiaries, Promise rootPromise) {
        if (beneficiaries == null || beneficiaries.size() == 0) {
            String msg = UnicefApplication.getResourceString(R.string.no_beneficiary);
            showToast(UnicefApplication.getAppContext(), msg);
            rootPromise.resolve(msg);
            return;
        }
        totalCount += beneficiaries.size();
        BeneficiaryDataAccess beneficiaryDataAccess = dbHelper.getBeneficiaryDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.saveBeneficiaries(instanceId, beneficiaries);
                return null;
            }
        });

        promise.then(res -> {
//            rootPromise.resolve(null);
            downloadBeneficiaryById(beneficiaries);
            return null;
        }).error(err -> {
            rootPromise.reject((Exception)err);
        });


    }

    private void downloadBeneficiaryById(List<Beneficiary> beneficiaries) {
        IGenericApiCallBack callBack = new IGenericApiCallBack() {
            @Override
            public void apiCallSuccessful(Object identifier, Object o) {
                Beneficiary request = (Beneficiary) o;
                saveBeneficiaryById(request);
            }

            @Override
            public void apiCallFailed(boolean hasSpecificError, String errorMessage) {

            }
        };

        for(int idValue =0 ; idValue< beneficiaries.size(); idValue++){
            OfflineApiCalling.getBeneficiaryById(beneficiaries.get(idValue).getEntityId(),Singleton.getInstance().getIdInstance(), callBack);
        }
    }

    private void saveBeneficiaryById(Beneficiary request) {
        BeneficiaryDataAccess beneficiaryDataAccess = new BeneficiaryDataAccess();
        Promise promise = AsyncTaskRunner.executeAsync(new IAsyncMethod() {
            @Override
            public Object execute() {
                beneficiaryDataAccess.saveBeneficiaryGetById(request, Singleton.getInstance().getIdInstance());
                return null;
            }
        });

        promise.then(res ->{
            hideWait();
            showToast("All records successfully downloaded");
            return null;
        }).error(err ->{
            showError("Failed to Save Beneficiary By Id");
        });
    }
}
