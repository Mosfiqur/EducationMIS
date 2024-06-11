package com.unicef.mis.util;

import androidx.annotation.NonNull;

public class UnicefUncaughtExceptionHandler implements Thread.UncaughtExceptionHandler {
    private Thread.UncaughtExceptionHandler systemExceptionHandler;
    private Thread thread;
    private Throwable exception;

    public UnicefUncaughtExceptionHandler(Thread.UncaughtExceptionHandler defaultUncaughtExceptionHandler) {
        systemExceptionHandler = defaultUncaughtExceptionHandler;
    }

    @Override
    public void uncaughtException(@NonNull Thread t, @NonNull Throwable e) {
        this.thread = t;
        exception = e;
        e.printStackTrace();
        System.exit(2);
        systemExceptionHandler.uncaughtException(thread, exception);
    }
}
