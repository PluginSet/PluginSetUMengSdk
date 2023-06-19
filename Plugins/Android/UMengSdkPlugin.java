package com.pluginset.umeng;

import android.content.Context;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.MotionEvent;

import com.pluginset.core.IPluginSetActivityLifecycle;
import com.umeng.commonsdk.UMConfigure;

public class UMengSdkPlugin implements IPluginSetActivityLifecycle {
    @Override
    public void onCreate(Context context, Bundle savedInstanceState) {
        UMConfigure.preInit(context, "", "");
    }

    @Override
    public void onUnityPlayerUnloaded(Context context) {

    }

    @Override
    public void onUnityPlayerQuitted(Context context) {

    }

    @Override
    public void onNewIntent(Context context, Intent intent) {

    }

    @Override
    public void onDestroy(Context context) {

    }

    @Override
    public void onPause(Context context) {

    }

    @Override
    public void onResume(Context context) {

    }

    @Override
    public void onStart(Context context) {

    }

    @Override
    public void onRestart(Context context) {

    }

    @Override
    public void onStop(Context context) {

    }

    @Override
    public void onLowMemory(Context context) {

    }

    @Override
    public void onTrimMemory(Context context, int level) {

    }

    @Override
    public void onConfigurationChanged(Context context, Configuration newConfig) {

    }

    @Override
    public void onWindowFocusChanged(Context context, boolean hasFocus) {

    }

    @Override
    public boolean dispatchKeyEvent(Context context, KeyEvent event) {
        return false;
    }

    @Override
    public boolean onKeyUp(Context context, int keyCode, KeyEvent event) {
        return false;
    }

    @Override
    public boolean onKeyDown(Context context, int keyCode, KeyEvent event) {
        return false;
    }

    @Override
    public boolean onTouchEvent(Context context, MotionEvent event) {
        return false;
    }

    @Override
    public void onActivityResult(Context context, int requestCode, int resultCode, Intent data) {

    }
}
