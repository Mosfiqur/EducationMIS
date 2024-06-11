package com.unicef.mis.viewmodel;

import android.content.Context;
import android.view.View;

import androidx.appcompat.widget.SearchView;
import androidx.lifecycle.MutableLiveData;
import androidx.lifecycle.ViewModel;

import com.unicef.mis.R;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.enumtype.EntityType;
import com.unicef.mis.model.PagedResponse;
import com.unicef.mis.model.QueryParamModel;
import com.unicef.mis.model.benificiary.facility_list.FacilityListDatum;
import com.unicef.mis.model.notification.NotificationModel;
import com.unicef.mis.repository.NotificationRepository;
import com.unicef.mis.util.Promise;
import com.unicef.mis.util.UnicefApplication;

import org.jetbrains.annotations.NotNull;

import static com.unicef.mis.util.BaseActivity.showError;
import static com.unicef.mis.util.BaseActivity.showToast;
import static com.unicef.mis.util.BaseActivity.showWait;

public class NotificationViewModel extends ViewModel {
//    public NotificationRepository repository;
//    public Context context;
//    private View view;
//    private SearchView searchControl;
//    private String searchText;
//    private QueryParamModel queryParam;
//
//    public NotificationViewModel() {
//        context = UnicefApplication.getAppContext();
//        queryParam = new QueryParamModel(UIConstants.DEFAULT_PAGE_NUMBER, UIConstants.PAGINATED_LIST_DEFAULT_PAGE_SIZE, searchText);
//    }
//
//    public void goBack(){
//        showToast(UnicefApplication.getAppContext(), "Action Rquired");
//    }
//
//
//    private boolean showNotifications(@NotNull NotificationModel notificationModel) {
//        return true;
//    }
//
//    public void setView(View view) {
//        this.view = view;
//    }
//
//    public void initViews() {
//        initSearchView();
//        initRecyclerView();
//    }
//
//    private void initRecyclerView() {
//    }
//
//    private void initSearchView() {
//        if (searchControl != null) return;
//        searchControl = (SearchView) view.findViewById(R.id.svSearchNotifications);
//        if (searchControl != null) {
//            searchControl.setOnQueryTextListener(new SearchView.OnQueryTextListener() {
//                @Override
//                public boolean onQueryTextSubmit(String s) {
//                    searchText = s;
//                    resetQueryParam();
//                    searchNotifications();
//                    return true;
//                }
//
//                @Override
//                public boolean onQueryTextChange(String s) {
//                    if (s == null || s.equals("")) {
//                        searchText = s;
//                        resetQueryParam();
//                        searchNotifications();
//                    }
//                    return true;
//                }
//            });
//        }
//    }

    private void searchNotifications() {
//        int pageNumber = queryParam.getPageNumber();
//        if (!willProceedForBeneficiarySearch(pageNumber)) return;
//        if (endlessRecycleViewController != null && pageNumber == UIConstants.DEFAULT_PAGE_NUMBER) {
//            endlessRecycleViewController.onNewSearchRequest();
//        }
//        showWait("Searching facilities", view.getContext());
//        Promise promise = null;
//        if (entityType.getValue() == EntityType.Beneficiary.getIntValue()) {
//            promise = repository.searchFacilitiesForBeneficiary(queryParam);
//        } else {
//            promise = repository.searchFacilitiesByInstance(instanceId, queryParam);
//        }
//
//        promise.then(res -> {
//            PagedResponse<FacilityListDatum> facilities = (PagedResponse<FacilityListDatum>) res;
//            onFacilitiesFound(facilities);
//            return null;
//        }).error(err -> {
//            showError(err);
//        });
    }

//    private void resetQueryParam() {
//        queryParam.setPageNumber(UIConstants.DEFAULT_PAGE_NUMBER);
//        queryParam.setSearchText(searchText);
//    }
}
