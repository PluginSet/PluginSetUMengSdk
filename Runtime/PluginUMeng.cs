#if UNITY_EDITOR || ENABLE_UMENG_ANALYTICS || ENABLE_UMENG_CRASH_REPORT
using System.Collections;
using System.Collections.Generic;
using PluginSet.Core;

namespace PluginSetUAPM.Runtime
{
#if !UNITY_EDITOR
    [PluginRegister]
#endif
    public partial class PluginUMeng: PluginBase, IStartPlugin, IUserSet, IPrivacyAuthorizationCallback
    {
        public override string Name => "UAPM";
        public int StartOrder => PluginsStartOrder.SdkDefault - 1;
        public bool IsRunning { get; private set; }

        private void InitSdk()
        {
            var pushSecret = "";
            var subVersion = "";
#if UNITY_ANDROID
#if DEBUG
            subVersion = "debug";
            UMengAndroidApi.InitSdk(true, pushSecret);
#else
            subVersion = "release";
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
    }
}
#endif