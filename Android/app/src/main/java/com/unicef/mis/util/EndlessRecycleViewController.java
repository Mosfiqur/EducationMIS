package com.unicef.mis.util;

import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.app.Activity;
import android.content.Context;
import android.os.Handler;
import android.view.View;

import com.unicef.mis.constants.UIConstants;

import java.lang.ref.WeakReference;
import java.util.List;

public class EndlessRecycleViewController {
    private WeakReference<Context> context;
    private int listViewLayoutResourceId;
    private int dataBindingVariableId;
    private EndlessRecyclerOnScrollListener endlessRecyclerOnScrollListener;
    private IActionListener iActionListener;
    private RecyclerView recyclerView;
    private ItemListAdapter itemListAdapter;
    private ItemListAdapter.OnItemBinding onItemBindingListener;
    private boolean addTouchListener = true;

    public void setItemAtPosition(int position, Object item) {
        if (itemListAdapter != null) {
            itemListAdapter.setItemAtPosition(position, item);
        }
    }

    public long getCurrentPage() {
        return endlessRecyclerOnScrollListener.current_page;
    }

    public void setOnItemBindingListener(ItemListAdapter.OnItemBinding onItemBindingListener) {
        this.onItemBindingListener = onItemBindingListener;
    }

    public void setAddTouchListener(boolean addTouchListener) {
        this.addTouchListener = addTouchListener;
    }

    public interface IActionListener {
        void onLoadMore(long currentPage);

        void onClickItem(int position);

        void onLayoutCompleted();
    }


    public EndlessRecycleViewController(Context context, RecyclerView recyclerView,
                                        int listViewLayoutResourceId, int dataBindingVariableId, IActionListener iActionListener) {
        this.context = new WeakReference<>(context);
        this.recyclerView = recyclerView;
        this.listViewLayoutResourceId = listViewLayoutResourceId;
        this.dataBindingVariableId = dataBindingVariableId;
        this.iActionListener = iActionListener;
    }

    public <T> T getItemAtPosition(int position, Class<T> type) {
        try {
            if (itemListAdapter.getItemList() == null || itemListAdapter.getItemList().get(position) == null)
                return null;
            return type.cast(itemListAdapter.getItemList().get(position));
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public Object getItemAtPosition(int position) {
        return itemListAdapter.getItemList().get(position);
    }

    private void resetEndlessScrollViewPageNumber() {
        if (endlessRecyclerOnScrollListener == null) return;
        endlessRecyclerOnScrollListener.reset();
    }

    private void initRecycleView(ItemListAdapter itemListAdapter) {
        if (itemListAdapter == null) return;

        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(context.get(), LinearLayoutManager.VERTICAL, false) {
            @Override

            public void onLayoutCompleted(final RecyclerView.State state) {
                super.onLayoutCompleted(state);
//                iActionListener.onLayoutCompleted();
            }
        };

        //LinearLayoutManager linearLayoutManager = new LinearLayoutManager(context.get());
        endlessRecyclerOnScrollListener = new EndlessRecyclerOnScrollListener(linearLayoutManager) {
            @Override
            public void onLoadMore(long current_page) {
                iActionListener.onLoadMore(current_page);
            }
        };
        recyclerView.setNestedScrollingEnabled(false);
        recyclerView.setLayoutManager(linearLayoutManager);


        recyclerView.setAdapter(itemListAdapter);
        if(addTouchListener) {
            recyclerView.addOnItemTouchListener(new RecyclerItemClickListener(context.get(), new RecyclerItemClickListener.OnItemClickListener() {
                @Override
                public void onItemClick(View view, int position, float x, float y) {
                    iActionListener.onClickItem(position);
                }
            }));
        }
        recyclerView.addOnScrollListener(endlessRecyclerOnScrollListener);

        iActionListener.onLayoutCompleted();
    }

    private <T> void updateRecycleViewListItemOnUI(final List<T> itemList, final long pageNumber) {
        if (((Activity) context.get()) == null) return;
        ((Activity) context.get()).runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (itemList == null || itemList.size() <= 0) return;
                if (itemListAdapter == null) {
                    itemListAdapter = new ItemListAdapter(context.get(), itemList, listViewLayoutResourceId, dataBindingVariableId);
                    itemListAdapter.setOnItemBinding(onItemBindingListener);
                    initRecycleView(itemListAdapter);
                } else {
                    if (pageNumber > UIConstants.DEFAULT_PAGE_NUMBER) {
                        itemListAdapter.getItemList().addAll(itemList);
                        itemListAdapter.notifyDataSetChanged();
                    } else {
                        itemListAdapter.getItemList().clear();
                        itemListAdapter.getItemList().addAll(itemList);
                        itemListAdapter.notifyDataSetChanged();
                    }
                }
            }
        });
    }

    private void clearRecycleViewListItemOnUI() {
        if (((Activity) context.get()) == null) return;
        ((Activity) context.get()).runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (itemListAdapter == null) return;
                itemListAdapter.getItemList().clear();
                itemListAdapter.notifyDataSetChanged();
            }
        });
    }


    //region Public Access
    public <T> void updateItemListRecycleView(final List<T> itemList, final long pageNumber) {
        int delayLoading = UIConstants.LIST_ITEM_LOADING_MIN_DELAY;
        if (((Activity) context.get()) == null) {
            delayLoading = UIConstants.LIST_ITEM_LOADING_MAX_DELAY;
        }

        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                updateRecycleViewListItemOnUI(itemList, pageNumber);
            }
        }, delayLoading);
    }

    public void clearItemListRecycleView() {
        clearRecycleViewListItemOnUI();
    }

    public void onNewSearchRequest() {
        resetEndlessScrollViewPageNumber();
    }

    public ItemListAdapter getItemListAdapter() {
        return itemListAdapter;
    }

    public void scrollToPosition(int position) {
        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                if (((Activity) context.get()) == null) return;
                ((Activity) context.get()).runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        recyclerView.scrollToPosition(position);
                    }
                });
            }
        }, 10);


//        recyclerView.smoothScrollToPosition(position);


//        recyclerView.getLayoutManager().smoothScrollToPosition(recyclerView,new RecyclerView.State(), position);

//        recyclerView.getLayoutManager().scrollToPosition(position);

//        RecyclerView.SmoothScroller smoothScroller = new LinearSmoothScroller(context.get()) {
//            @Override protected int getVerticalSnapPreference() {
//                return LinearSmoothScroller.SNAP_TO_START;
//            }
//        };
//
//        recyclerView.getLayoutManager().startSmoothScroll(smoothScroller);
//
//        smoothScroller.setTargetPosition(position);
    }

    public int getCurrentPosition() {
        return ((LinearLayoutManager) recyclerView.getLayoutManager()).findLastVisibleItemPosition();
    }

    //endregion
}
