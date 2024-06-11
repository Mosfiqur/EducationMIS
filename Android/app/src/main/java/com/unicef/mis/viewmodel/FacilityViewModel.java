package com.unicef.mis.viewmodel;

import android.content.Context;
import android.content.SharedPreferences;
import android.view.View;

import androidx.appcompat.widget.SearchView;
import androidx.databinding.ObservableBoolean;
import androidx.databinding.ViewDataBinding;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;
import androidx.recyclerview.widget.RecyclerView;

import com.unicef.mis.BR;
import com.unicef.mis.R;
import com.unicef.mis.constants.ApplicationConstants;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.dataaccess.SQLiteDatabaseHelper;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.factory.RepositoryFactory;
import com.unicef.mis.interfaces.IFacilityRepository;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityList;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.util.BeneficiaryDownloader;
import com.unicef.mis.util.EndlessRecycleViewController;
import com.unicef.mis.util.FacilityDownloader;
import com.unicef.mis.util.ItemListAdapter;
import com.unicef.mis.util.NavigationHelper;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.Singleton;
import com.unicef.mis.util.UnicefApplication;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import static com.unicef.mis.constants.ApplicationConstants.TOKEN;
import static com.unicef.mis.util.BaseActivity.hideWait;
import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class FacilityViewModel extends ViewModel implements ItemListAdapter.OnItemBinding {
    private static String TAG = FacilityViewModel.class.getSimpleName();

    private IFacilityRepository repository;
    public MutableLiveData<List<FacilityListDatum>> facilityList;
    public List<FacilityList> facility;
    public List<FacilityListDatum> facilityListData;
    public MutableLiveData<String> generalMsg = new MutableLiveData<String>("");

    public MutableLiveData<Integer> entityType = new MutableLiveData<Integer>();
    public MutableLiveData<Integer> operationMode = new MutableLiveData<Integer>();
    private int instanceId;
    public MutableLiveData<String> instanceTitle = new MutableLiveData<String>();
    public MutableLiveData<String> instanceDate = new MutableLiveData<String>();
    public MutableLiveData<String> instanceEndDate = new MutableLiveData<String>();

    public ObservableBoolean hasRecord = new ObservableBoolean(true);
    private String searchText;
    private SearchView searchControl;
    private QueryParamModel queryParam;
    private EndlessRecycleViewController endlessRecycleViewController;

    private SQLiteDatabaseHelper dbHelper;

    public Context context;
    private View view;
    public SharedPreferences sharedPreferences;
    public static String token = "";
    public MutableLiveData<Long> totalCount = new MutableLiveData<Long>((long) 0);
    private List<FacilityListDatum> beneficiaryFacilities;


    public FacilityViewModel() {
        this.context = UnicefApplication.getAppContext();
        facilityList = new MutableLiveData<>();
        facilityListData = new ArrayList<>();
        dbHelper = SQLiteDatabaseHelper.getInstance(UnicefApplication.getAppContext());
        queryParam = new QueryParamModel(UIConstants.DEFAULT_PAGE_NUMBER, UIConstants.PAGINATED_LIST_DEFAULT_PAGE_SIZE, searchText);
        sharedPreferences = UnicefApplication.getAppContext().getSharedPreferences(ApplicationConstants.APP_PREFERENCES, Context.MODE_PRIVATE);
        token = sharedPreferences.getString(TOKEN, "");
    }

    public MutableLiveData<List<FacilityListDatum>> getFacilityList() {
        return facilityList;
    }

    public void setView(View view) {
        this.view = view;
    }

    private void resetQueryParam() {
        queryParam.setPageNumber(UIConstants.DEFAULT_PAGE_NUMBER);
        queryParam.setSearchText(searchText);
    }

    public void initViews() {
        initSearchView();
        initRecyclerView();
    }

    private void initRecyclerView() {
        RecyclerView recyclerView = (RecyclerView) view.findViewById(R.id.facility_list_recycler);
        endlessRecycleViewController = new EndlessRecycleViewController(view.getContext(), recyclerView, R.layout.facility_main_questionear_single, BR.facility,
                new EndlessRecycleViewController.IActionListener() {
                    @Override
                    public void onLoadMore(long currentPage) {
                        queryParam.setPageNumber((int) currentPage);
                        searchFacilities();
                    }

                    @Override
                    public void onClickItem(int position) {
                        FacilityListDatum selectedFacility = endlessRecycleViewController.getItemAtPosition(position, FacilityListDatum.class);
                        if (selectedFacility == null) return;
                        if (entityType.getValue() == EntityType.Facilitiy.getIntValue()) {
                            NavigationHelper.gotoFacilityRecordsPage(Singleton.getInstance().getIdInstance(), operationMode.getValue(), selectedFacility);
                        } else {
                            NavigationHelper.gotoOnlineBeneficiaryFacilityListPage(Singleton.getInstance().getIdInstance(), operationMode.getValue(), selectedFacility);
                        }
                    }

                    @Override
                    public void onLayoutCompleted() {

                    }
                });
        endlessRecycleViewController.setOnItemBindingListener(this);
    }

    public void searchFacilities() {
        int pageNumber = queryParam.getPageNumber();
        if (!willProceedForBeneficiarySearch(pageNumber)) return;
        if (endlessRecycleViewController != null && pageNumber == UIConstants.DEFAULT_PAGE_NUMBER) {
            endlessRecycleViewController.onNewSearchRequest();
        }
        showWait("Searching facilities", view.getContext());
        Promise promise;
        if (entityType.getValue() == EntityType.Beneficiary.getIntValue()) {
            promise = repository.searchFacilitiesForBeneficiary(Singleton.getInstance().getIdInstance(), queryParam);
        } else {
            promise = repository.searchFacilitiesByInstance(instanceId, queryParam);
        }
//        hideWait();
        promise.then(res -> {

            PagedResponse<FacilityListDatum> facilities = (PagedResponse<FacilityListDatum>) res;
            onFacilitiesFound(facilities);
            return null;
        }).error(err -> {
            showError(err);
        });
    }

    private boolean willProceedForBeneficiarySearch(long pageNumber) {
        resetTotalCountBeneficiaryResultListIfNeeded(pageNumber);
        return totalCount.getValue() > ((pageNumber - 1) * UIConstants.PAGINATED_LIST_DEFAULT_PAGE_SIZE);
    }

    private void resetTotalCountBeneficiaryResultListIfNeeded(long pageNumber) {
        //If page number is equal to starting page then reset total result counter to default again
        if (pageNumber == UIConstants.DEFAULT_PAGE_NUMBER) {
            totalCount.setValue(UIConstants.PAGINATED_LIST_INITIAL_TOTAL_COUNT);
        }
    }

    private void onFacilitiesFound(PagedResponse<FacilityListDatum> response) {
        List<FacilityListDatum> listData = response.getData();
        totalCount.setValue(Long.valueOf(response.getTotal()));
        if (listData.size() == 0) {
            generalMsg.setValue("No records found");
            endlessRecycleViewController.clearItemListRecycleView();
            hideWait();
            return;
        }
        this.generalMsg.setValue("Total " + totalCount.getValue() + " facilities");
        endlessRecycleViewController.updateItemListRecycleView(listData, queryParam.getPageNumber());
        hideWait();
    }

    private void initSearchView() {
        if (searchControl != null) return;
        searchControl = (SearchView) view.findViewById(R.id.svSearchFacilities);
        if (searchControl != null) {
            searchControl.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
                @Override
                public boolean onQueryTextSubmit(String s) {
                    searchText = s;
                    resetQueryParam();
                    searchFacilities();
                    return true;
                }

                @Override
                public boolean onQueryTextChange(String s) {
                    if (s == null || s.equals("")) {
                        searchText = s;
                        resetQueryParam();
                        searchFacilities();
                    }
                    return true;
                }
            });
        }
    }

    public void makeAllAvailableOffline() {
        if (entityType.getValue() == EntityType.Facilitiy.getIntValue()) {
            FacilityDownloader downloader = new FacilityDownloader(view);
            downloader.startFacilityDownload();
        } else {
            BeneficiaryDownloader downloader = new BeneficiaryDownloader(this.instanceId, view);
            downloader.startBeneficiaryDownload();
        }
    }

    public void prepareView(int entityType, int operationMode, ScheduledInstance instance) {
        this.operationMode.setValue(operationMode);
        this.entityType.setValue(entityType);
        this.repository = RepositoryFactory.getFacilityRepository(operationMode, view.getContext());
        this.instanceId = instance.getId();
        this.instanceTitle.setValue(instance.getTitle());
        String scheduledDate = instance.getDataCollectionDate();
        String scheduleEndDate = instance.getEndDate();
        String[] split1 = scheduledDate.split("T", 2);
//        String[] split2 = scheduleEndDate.split("T", 2);

        SimpleDateFormat sdf = new SimpleDateFormat("yy-MM-dd");
        SimpleDateFormat sdf1 = new SimpleDateFormat("dd MMM yyyy");

        Date d = null;
        Date endDate = null;

        try {
            d = sdf.parse(split1[0]);

        } catch (ParseException e) {
            e.printStackTrace();
        }

        try{
            endDate = sdf.parse(split1[0]);
        } catch (ParseException e){
            e.printStackTrace();
        }

        String formattedDate = sdf1.format(d);
        String formatEndDate = sdf1.format(endDate);
        instanceDate.setValue(formattedDate);
        instanceEndDate.setValue(formatEndDate);
    }

    @Override
    public void setBindings(ViewDataBinding dataBinding) {
        dataBinding.setVariable(BR.viewModel, this);
    }

    public void uploadFacilities(){
        showWait("Uploading Facilities...", view.getContext());
        Promise promise = repository.uploadFacilities(instanceId);
        promise.then(res->{
            hideWait();
            showToast(view.getContext(), "Facilities uploaded successfully");
            searchFacilities();
            return null;
        }).error(err->{
            showError(err);
        });
    }
}
