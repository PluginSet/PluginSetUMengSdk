package com.pluginset.umeng;

import android.content.Context;
import android.util.Log;

import com.umeng.analytics.MobclickAgent;
import com.umeng.commonsdk.UMConfigure;
import com.umeng.commonsdk.listener.OnGetOaidListener;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;

public class UMengSdkHelper {
    private static boolean _sdkOaidGet;
    private static String _sdkOaidValue;

    public static void InitSdk(Context context, final boolean debug, String pushSecret)
    {
        UMConfigure.setLogEnabled(debug);
        UMConfigure.init(context, UMConfigure.DEVICE_TYPE_PHONE, pushSecret);

        if (debug) {
            Log.d("UMengSDK", "init sdk ===================================== ");
        }
        
        try {
            UMConfigure.getOaid(context, new OnGetOaidListener() {
                @Override
                public void onGetOaid(String s) {
                    _sdkOaidGet = true;
                    _sdkOaidValue = s;
                    if (debug) {
                        Log.d("UMengSDK", "get oaid success, id=" + s);
                    }
                }
            });
        } catch (Exception e) {
            e.printStackTrace();
            _sdkOaidGet = true;
            _sdkOaidValue = "";
        }
    }
    
    public static String GetOaid()
    {
        if (_sdkOaidGet) {
            return _sdkOaidValue;
        }
        return "";
    }

    public static void OnQuitApp(Context context)
    {
        MobclickAgent.onKillProcess(context);
    }

    public static void OnUserLogin(String userId)
    {
        MobclickAgent.onProfileSignIn(userId);
    }

    public static void OnUserLogout()
    {
        MobclickAgent.onProfileSignOff();
    }

    public static void OnPageStart(String page)
    {
        MobclickAgent.onPageStart(page);
    }

    public static void OnPageEnded(String page)
    {
        MobclickAgent.onPageEnd(page);
    }

    public static void CustomEvent(Context context, String event, String json)
    {
        JSONObject obj = null;
        try {
            obj = new JSONObject(json);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        Map<String, Object> map = new HashMap<>();

        if (obj != null)
        {
            Iterator<String> keys = obj.keys();
            while (keys.hasNext()) {
                String key = keys.next();
                try {
                    Object value = obj.get(key);
                    map.put(key, value);
                }
                catch (JSONException e)
                {
                    Log.d("UM CustomEvent", "get object fail with key" + key);
                }
            }

        }
        MobclickAgent.onEventObject(context, event, map);
    }

}
