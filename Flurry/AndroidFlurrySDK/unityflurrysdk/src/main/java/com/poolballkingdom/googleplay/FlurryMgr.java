package com.poolballkingdom.googleplay;

import android.app.Activity;
import android.content.Context;

import com.flurry.android.FlurryAgent;


public class FlurryMgr {
    private FlurryMgr() {
    }

    private static class singlon {
        public static FlurryMgr INSTANCE = new FlurryMgr();
    }

    public static FlurryMgr Instance() {
        return singlon.INSTANCE;
    }

    //初始化
    public void Init(Activity activity, String apiKey) {
        new FlurryAgent.Builder()
                .withLogEnabled(true)
                .build(activity, apiKey);
    }

    //开始传输
    public void OnStartSessin(Activity activity) {
        FlurryAgent.onStartSession(activity);
    }

    //结束传递
    public void onStop(Activity activity) {
        FlurryAgent.onEndSession(activity);
    }
}
