package com.unicef.mis.util;

import android.os.AsyncTask;

import com.unicef.mis.interfaces.IAsyncMethod;

public class AsyncTaskRunner extends AsyncTask {
    private IAsyncMethod _asyncMethod;
    private Promise _promise;

    public AsyncTaskRunner(IAsyncMethod asyncMethod, Promise promise) {
        _asyncMethod = asyncMethod;
        _promise = promise;
    }

    @Override
    protected Object doInBackground(Object[] objects) {
        Object result = _asyncMethod.execute();
        return result;
    }

    @Override
    protected void onPostExecute(Object o) {
        super.onPostExecute(o);
        if (o instanceof Exception) {
            _promise.reject((Exception)o);
        } else {
            _promise.resolve(o);
        }
    }

    @Override
    protected void onPreExecute() {
        super.onPreExecute();
    }

    public static Promise executeAsync(IAsyncMethod asyncMethod){
        Promise promise = new Promise();
        AsyncTaskRunner runner = new AsyncTaskRunner(asyncMethod, promise);
        runner.execute();
        return promise;
    }
}
