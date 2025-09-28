#if UNITY_EDITOR || ENABLE_UMENG_ANALYTICS || ENABLE_UMENG_CRASH_REPORT
using System;
using System.Collections;
using System.Collections.Generic;
using PluginSet.Core;
using UnityEngine;

namespace PluginSet.UMeng
{
#if !UNITY_EDITOR
    [PluginRegister]
#endif
    public partial class PluginUMeng: PluginBase, IStartPlugin, IUserSet, IPrivacyAuthorizationCallback, ICustomPlugin
    {
        public override string Name => "UAPM";

        public int StartOrder => PluginsStartOrder.SdkDefault - 1;
        public bool IsRunning { get; private set; }

        private void InitSdk()
        {
            Debug.Log("InitSDK ===================== ");
            var pushSecret = "";
#if ENABLE_UMENG_CRASH_REPORT
            var subVersion = "";
#endif
#if UNITY_ANDROID
#if true // DEBUG
#if ENABLE_UMENG_CRASH_REPORT
            subVersion = "debug";
#endif
            UMengAndroidApi.InitSdk(true, pushSecret);
#else
#if ENABLE_UMENG_CRASH_REPORT
            subVersion = "release";
#endif
            UMengAndroidApi.InitSdk(false, pushSecret);
#endif
#endif
            
#if ENABLE_UMENG_CRASH_REPORT
            InitCrash(subVersion);
#endif
        }
        
        public IEnumerator StartPlugin()
        {
            if (IsRunning)
                yield break;
            IsRunning = true;
        }

        public void DisposePlugin(bool isAppQuit = false)
        {
            if (isAppQuit)
            {
#if UNITY_ANDROID
                UMengAndroidApi.OnAppQuit();
#endif
            }
        }
        
        public void OnPrivacyAuthorization()
        {
            InitSdk();
        }

        public void SetUserInfo(bool isNewUser, string userId, Dictionary<string, object> pairs = null)
        {
            if (string.IsNullOrEmpty(userId))
            {
                ClearUserInfo();
                return;
            }
            
#if UNITY_ANDROID
            UMengAndroidApi.OnUserLogin(userId);
#endif
        }

        public void ClearUserInfo()
        {
#if UNITY_ANDROID
            UMengAndroidApi.OnUserLogout();
#endif
        }

        public void FlushUserInfo()
        {
        }
        
        public void CustomCall(string func, Action<Result> callback = null, string json = null)
        {
            if (func == "GetOaid")
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                string oaid = UMengAndroidApi.GetOaid();
                callback?.Invoke(new Result()
                {
                    Success = true,
                    PluginName = Name,
                    Code = PluginConstants.SuccessCode,
                    Data = oaid,
                });
#else
                callback?.Invoke(new Result()
                {
                    Success = false,
                    PluginName = Name,
                    Code = PluginConstants.FailDefaultCode,
                    Data = string.Empty,
                });
#endif
            }
        }
    }
}
#endif