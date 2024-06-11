package com.unicef.mis.util;


import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.Bundle;
import android.util.DisplayMetrics;
import android.view.View;
import android.widget.ScrollView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.fragment.app.Fragment;

import com.unicef.mis.R;
import com.unicef.mis.constants.UIConstants;
import com.unicef.mis.model.benificiary.schedule.ScheduledInstance;
import com.unicef.mis.views.ScheduledInstanceListFragment;
import com.unicef.mis.views.FacilityListFragment;

import org.jetbrains.annotations.NotNull;

import java.io.ByteArrayOutputStream;

public class BaseFragment extends Fragment {
    //base of all fragment class

    public DialogUtil dialogUtil;
    Context context;
    Bitmap bitmap;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

        //initialize dialog utils class for getting dialog util object
        dialogUtil = new DialogUtil(getActivity());
        context = getActivity();
    }

    public static void showToast(Context ctx, String msg) {
        Toast.makeText(ctx, msg, Toast.LENGTH_SHORT).show();
    }





    //method for fragment replace getChildFragmentManager()

    protected void initFragment(Fragment fragment, String id, int resId) {
        getChildFragmentManager()
                .beginTransaction()
                .add(resId, fragment, id)
                .addToBackStack(null)
                .commit();
    }


    public void replaceFragment(Fragment fragment, String newid, String oldId, int resId) {
        //Singleton.getInstance().setWhichFragmentItIs(fragment);
        getChildFragmentManager()
                .beginTransaction()
                .setCustomAnimations(R.anim.enter_from_right, R.anim.exit_to_left, R.anim.enter_from_left, R.anim.exit_to_right)
                .replace(resId, fragment, oldId)
                .addToBackStack(oldId)
                .commit();
    }

    //method for network available or not checking

    public boolean isNetworkAvailable() {

        ConnectivityManager connectivityManager
                = (ConnectivityManager) getActivity().getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo activeNetworkInfo = connectivityManager.getActiveNetworkInfo();
        return activeNetworkInfo != null && activeNetworkInfo.isConnectedOrConnecting();

    }


    //method for string concat
    public String[] concat(String[] A, String[] B) {
        int aLen = A.length;
        int bLen = B.length;
        String[] C = new String[aLen + bLen];
        System.arraycopy(A, 0, C, 0, aLen);
        System.arraycopy(B, 0, C, aLen, bLen);
        return C;
    }

    //method for load data from array
    public String[] loadArray(String arrayName, Context mContext) {
        SharedPreferences prefs = mContext.getSharedPreferences("preferencename", 0);
        int size = prefs.getInt(arrayName + "_size", 0);
        String[] array = new String[size];
        for (int i = 0; i < size; i++)
            array[i] = prefs.getString(arrayName + "_" + i, null);
        return array;
    }


    //method for save data in array
    public boolean saveArray(String[] array, String arrayName, Context mContext) {
        SharedPreferences prefs = mContext.getSharedPreferences("preferencename", 0);
        SharedPreferences.Editor editor = prefs.edit();
        editor.putInt(arrayName + "_size", array.length);
        for (int i = 0; i < array.length; i++)
            editor.putString(arrayName + "_" + i, array[i]);
        return editor.commit();
    }



    //method for convert bitmap to biye array
    protected byte[] convertBitmaptoByteArray(ScrollView relativeLayout) {
        bitmap = getBitmapFromView(relativeLayout, relativeLayout.getChildAt(0).getHeight(), relativeLayout.getChildAt(0).getWidth());
        ByteArrayOutputStream bStream = new ByteArrayOutputStream();
        bitmap.compress(Bitmap.CompressFormat.PNG, 100, bStream);
        byte[] byteArray = bStream.toByteArray();

        return byteArray;
    }

    //method for get bitmap
    protected Bitmap getBitmapFromView(View view, int height, int width) {
        Bitmap bitmap = Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
        Canvas canvas = new Canvas(bitmap);
        Drawable bgDrawable = view.getBackground();
        if (bgDrawable != null)
            bgDrawable.draw(canvas);
        else
            canvas.drawColor(Color.WHITE);
        view.draw(canvas);
        return bitmap;
    }


    //method for return screen size
    public int[] getScreenSIze() {
        DisplayMetrics displaymetrics = new DisplayMetrics();
        getActivity().getWindowManager().getDefaultDisplay().getMetrics(displaymetrics);
        int h = displaymetrics.heightPixels;
        int w = displaymetrics.widthPixels;
        int[] size = {w, h};
        return size;
    }

    @NotNull
    protected ScheduledInstanceListFragment getScheduledInstanceListFragment(int entityType, int operationMode) {
        ScheduledInstanceListFragment fragment = new ScheduledInstanceListFragment();
        Bundle args = new Bundle();
        args.putInt(UIConstants.KEY_ENTITY_TYPE, entityType);
        args.putInt(UIConstants.KEY_OPERATION_MODE, operationMode);
        fragment.setArguments(args);
        return fragment;
    }

    protected FacilityListFragment getFacilityListFragment(int entityType, int operationMode, ScheduledInstance instance){
        FacilityListFragment fragment = new FacilityListFragment();
        Bundle args = new Bundle();
        args.putInt(UIConstants.KEY_ENTITY_TYPE, entityType);
        args.putInt(UIConstants.KEY_OPERATION_MODE, operationMode);
        args.putParcelable(UIConstants.KEY_INSTANCE, instance);
        fragment.setArguments(args);
        return fragment;
    }


    //method for show error message form Api
    //No plan of implement it right now
//    public <T> void showErrorMessage(int responseCode, Response<T> response) {
//
//
//        try {
//            JSONObject jsonObject = null;
//            String errorMessage = "";
//            try {
//                jsonObject = new JSONObject(response.errorBody().string());
//                errorMessage = jsonObject.getString("error");
//
//
//            } catch (JSONException e) {
//                try {
//                    errorMessage = jsonObject.getString("message");
//
//                } catch (JSONException e1) {
//                    e1.printStackTrace();
//                }
//            }
//
//            Toast.makeText(getActivity(), errorMessage, Toast.LENGTH_LONG).show();
//
//        } catch (IOException e) {
//            e.printStackTrace();
//        }
//
//    }
}
