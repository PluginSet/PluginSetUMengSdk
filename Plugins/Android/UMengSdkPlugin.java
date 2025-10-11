package com.pluginset.umeng;

import android.app.Application;
import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;

import com.pluginset.core.IPluginSetApplicationLifecycle;
import com.umeng.commonsdk.UMConfigure;

import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

public class UMengSdkPlugin implements IPluginSetApplicationLifecycle {
    @Override
    public void onApplicationCreate(Application application) {
        String channel = null;
        // 尝试读取channelInfo.json中的渠道ID，这共用了PluginSet.Channel中使用的文件
        try {
            String channelInfoFile = "channelInfo.json";
            AssetManager assetManager = application.getAssets();
            // 1. 打开 assets 中的文件
            InputStream inputStream = assetManager.open(channelInfoFile);

            // 2. 读取文件内容
            BufferedReader reader = new BufferedReader(new InputStreamReader(inputStream));
            StringBuilder stringBuilder = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                stringBuilder.append(line);
            }
            String jsonString = stringBuilder.toString();

            // 3. 解析 JSON
            JSONObject jsonObject = new JSONObject(jsonString);
            int channelId = jsonObject.optInt("channelId", -1);
            if (channelId > 0) {
                channel = "Channel_" + channelId;
            }
        } catch (Exception e) {
            Log.d("UMLog", "Not found channelInfo.json");
        }

        Log.d("UMLog", "preInit sdk with channel " + channel);
        UMConfigure.preInit(application, (String)null, (String)channel);
    }

    @Override
    public void attachBaseContext(Application application, Context base) {

    }
}
