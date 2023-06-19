package com.pluginset.umeng;

import android.app.Application;
import android.content.Context;

import com.pluginset.core.IPluginSetApplicationLifecycle;
import com.umeng.commonsdk.UMConfigure;

public class UMengSdkPlugin implements IPluginSetApplicationLifecycle {
    @Override
    public void onApplicationCreate(Application application) {
        UMConfigure.preInit(application, (String)null, (String)null);
    }

    @Override
    public void attachBaseContext(Application application, Context base) {

    }
}
