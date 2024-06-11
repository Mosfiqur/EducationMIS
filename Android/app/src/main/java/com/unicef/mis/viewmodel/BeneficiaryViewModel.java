package com.unicef.mis.viewmodel;

import android.app.Activity;
import android.content.Context;

import androidx.appcompat.widget.SearchView;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.BR;
import com.unicef.mis.R;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.enumtype.CollectionStatus;
import com.unicef.mis.enumtype.OperationMode;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IBeneficiaryRepository;
import com.unicef.mis.model.Beneficiary;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.util.EndlessRecycleViewController;
import com.unicef.mis.util.NavigationHelper;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.util.List;

import static com.unicef.mis.util.BaseActivity.hideWait;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class BeneficiaryViewModel extends ViewModel {
    private static String TAG = BeneficiaryViewModel.class.getSimpleName();

    private IBeneficiaryRepository repository;
    public MutableLiveData<FacilityListDatum> facility = new MutableLiveData<FacilityListDatum>();
    private String searchText;
    public Context context;
    private QueryParamModel queryParam;
    private EndlessRecycleViewController endlessRecycleViewController;
    private Context activityContext;
    private SearchView searchControl;

    public MutableLiveData<Integer> operationMode = new MutableLiveData<Integer>();

    public MutableLiveData<Long> totalCount = new MutableLiveData<Long>(Long.valueOf(0));
    public MutableLiveData<String> generalMsg = new MutableLiveData<String>("");
    private int instanceId;

    public BeneficiaryViewModel() {
        context = UnicefApplication.getAppContext();
        queryParam = new QueryParamModel(UIConstants.DEFAULT_PAGE_NUMBER, UIConstants.PAGINATED_LIST_DEFAULT_PAGE_SIZE, searchText);
    }

    public void addBeneficiary() {
        if (operationMode.getValue() == OperationMode.Online.getIntValue())
        {
            NavigationHelper.gotoBeneficiaryOnlineAdd(operationMode.getValue(), instanceId, facility.getValue());
        } else {
            NavigationHelper.gotoBeneficiaryOfflineAdd(operationMode.getValue(), instanceId, facility.getValue());
        }
//
    }

    private void initViews() {
        initSearchView();
        initRecyclerView();
    }

    private void initRecyclerView() {
        if (endlessRecycleViewController == null) {
            RecyclerView recyclerView = (RecyclerView) ((Activity) activityContext).findViewById(R.id.benificiary_list_recycler);
            endlessRecycleViewController = new EndlessRecycleViewController(activityContext, recyclerView, R.layout.single_benificiary_name_list, BR.beneficiary,
                    new EndlessRecycleViewController.IActionListener() {
                        @Override
                        public void onLoadMore(long currentPage) {
                            queryParam.setPageNumber((int) currentPage);
                            searchBeneficiaries();
                        }

                        @Override
                        public void onClickItem(int position) {
                            Beneficiary selectedBeneficiary =
                                    endlessRecycleViewController.getItemAtPosition(position, Beneficiary.class);
                            if (selectedBeneficiary == null) return;
//                            if (operationMode.getValue() == OperationMode.Online.getIntValue())
//                            {
//
//                            } else {
//                                NavigationHelper.gotoOnlineBeneficiaryRecordsPage
//                                        (Singleton.getInstance().getIdInstance(), operationMode.getValue(), selectedBeneficiary);
//                            }

                            if (selectedBeneficiary.getCollectionStatus() == CollectionStatus.Requested_Inactive.getIntValue())
                            {
                                showToast("Access Denied");
                            } else {
                                NavigationHelper.gotoOnlineBeneficiaryRecordsPage
                                        (Singleton.getInstance().getIdInstance(), operationMode.getValue(), selectedBeneficiary);
                            }

                        }

                        @Override
                        public void onLayoutCompleted() {

                        }
                    });
        }
    }

    private void initSearchView() {
        searchControl = (SearchView) ((Activity) activityContext).findViewById(R.id.svSearchBeneficiaries);
        if (searchControl != null) {
            searchControl.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
                @Override
                public boolean onQueryTextSubmit(String s) {
                    searchText = s;
                    resetQueryParam();
                    searchBeneficiaries();
                    return true;
                }

                @Override
                public boolean onQueryTextChange(String s) {
                    if (s == null || s.equals("")) {
                        searchText = s;
                        resetQueryParam();
                        searchBeneficiaries();
                    }
                    return true;
                }
            });
        }
    }

    private void resetQueryParam() {
        queryParam.setPageNumber(UIConstants.DEFAULT_PAGE_NUMBER);
        queryParam.setSearchText(searchText);
    }

    public void searchBeneficiaries() {
        int pageNumber = queryParam.getPageNumber();
        if (!willProceedForFacilitySearch(pageNumber)) return;
        if (endlessRecycleViewController != null && pageNumber == UIConstants.DEFAULT_PAGE_NUMBER) {
            endlessRecycleViewController.onNewSearchRequest();
        }

        showWait("Getting beneficiaries...", activityContext);
        Promise promise = repository.getBeneficiariesByFacility(facility.getValue().getId(), instanceId, queryParam);
        promise.then(res->{
            hideWait();
            PagedResponse<Beneficiary> response = (PagedResponse<Beneficiary>) res;
            onBeneficiariesFound(response);
            return null;
        }).error(err->{
            hideWait();
            showError(err);
        });
    }

    private boolean willProceedForFacilitySearch(int pageNumber) {
        resetTotalCountIfNeeded(pageNumber);
        return totalCount.getValue() > ((pageNumber - 1) * UIConstants.PAGINATED_LIST_DEFAULT_PAGE_SIZE);
    }

    private void resetTotalCountIfNeeded(int pageNumber) {
        //If page number is equal to starting page then reset total result counter to default again
        if (pageNumber == UIConstants.DEFAULT_PAGE_NUMBER) {
            totalCount.setValue(UIConstants.PAGINATED_LIST_INITIAL_TOTAL_COUNT);
        }
    }

    private void onBeneficiariesFound(PagedResponse<Beneficiary> response) {
        hideWait();
        List<Beneficiary> listData = response.getData();
        totalCount.setValue(Long.valueOf(response.getTotal()));
        if (listData.size() == 0) {
            generalMsg.setValue("No records found");
            endlessRecycleViewController.clearItemListRecycleView();
            return;
        }
        this.generalMsg.setValue("Total " + totalCount.getValue() + " beneficiaries");
        endlessRecycleViewController.updateItemListRecycleView(listData, queryParam.getPageNumber());
    }

    public void setContext(Context context) {
        activityContext = context;
    }


    public void prepareView(int operationMode, int instanceId, FacilityListDatum facility) {
        this.facility.setValue(facility);
        this.operationMode.setValue(operationMode);
        this.instanceId = instanceId;
        this.repository = RepositoryFactory.getBeneficiaryRepository(operationMode, activityContext);
        initViews();
    }

    public void uploadBeneficiaries(){
        showWait("Uploading beneficiaries...", activityContext);
        Promise promise = repository.uploadBeneficiaries(instanceId, facility.getValue().getId());
        promise.then(res->{
            hideWait();
            showToast(activityContext, "Beneficiaries uploaded successfully");
            searchBeneficiaries();
            return null;
        }).error(err->{
            showError(err);
        });
    }
}
