package com.unicef.mis.util;

import com.loopj.android.http.SyncHttpClient;

public class APIClient {
    //    public static final String BASE_URL = "http://phantomasp.kaz.com.bd/api/";
    public static final String BASE_URL = "https://phantom-srv.kaz.com.bd:8086/api/";
//    public static final String BASE_URL = "http://localhost:5445/api/";
//        public static final String BASE_URL = "https://localhost:5446/api/";
    private static SyncHttpClient client = new SyncHttpClient();

    public static SyncHttpClient getInstance() {
        return client;
    }
}
