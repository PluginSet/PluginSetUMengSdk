package com.pluginset.umeng;

import android.content.Context;

import com.umeng.umcrash.UMCrash;
import com.umeng.umcrash.UMCrashCallback;

public class UMengAPMApi {
    private static String customCallback = "";

    public static void InitAPM(Context context, String appVersion, String subVersion, String buildId)
    {
        UMCrash.setAppVersion(appVersion, subVersion, buildId);

        UMCrash.registerUMCrashCallback(new UMCrashCallback() {
            @Override
            public String onCallback() {
                return customCallback;
            }
        });
    }

    public static void SetCrashCustomCallback(String callback)
    {
        customCallback = callback;
    }

    public static void GenerateCustomLog(String e, String type)
    {
        UMCrash.generateCustomLog(e, type);
    }
}
