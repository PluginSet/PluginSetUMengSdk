#if UNITY_ANDROID
using PluginSet.Core;
using UnityEngine;

namespace PluginSet.UMeng
{
    public class UMengAndroidApi
    {
        
#if UNITY_EDITOR
        private static IAndroidCallable _umengSdkHelper = null;
        private static IAndroidCallable UMengSdkHelper
#else
        private static AndroidJavaClass _umengSdkHelper;
        private static AndroidJavaClass UMengSdkHelper
#endif
    
        {
            get
            {
#if !UNITY_EDITOR
                if (_umengSdkHelper == null)
                    _umengSdkHelper = new AndroidJavaClass("com.pluginset.umeng.UMengSdkHelper");
#endif
                
                return _umengSdkHelper;
            }
        }
        
#if UNITY_EDITOR
        private static IAndroidCallable _umengAPMApi = null;
        private static IAndroidCallable UMengAPMApi
#else
        private static AndroidJavaClass _umengAPMApi;
        private static AndroidJavaClass UMengAPMApi
#endif
    
        {
            get
            {
#if !UNITY_EDITOR
                if (_umengAPMApi == null)
                    _umengAPMApi = new AndroidJavaClass("com.pluginset.umeng.UMengAPMApi");
#endif
                
                return _umengAPMApi;
            }
        }

        public static string GetOaid()
        {
            return UMengSdkHelper?.CallStatic<string>("GetOaid");
        }

        public static void InitSdk(bool debug, string pushSecret)
        {
            UMengSdkHelper?.CallStatic("InitSdk", AndroidHelper.CurrentActivity, debug, pushSecret);
        }

        public static void OnAppQuit()
        {
            UMengSdkHelper?.CallStatic("OnQuitApp", AndroidHelper.CurrentActivity);
        }

        public static void OnUserLogin(string userId)
        {
            UMengSdkHelper?.CallStatic("OnUserLogin", userId);
        }

        public static void OnUserLogout()
        {
            UMengSdkHelper?.CallStatic("OnUserLogout");
        }

        public static void OnPageStart(string page)
        {
            UMengSdkHelper?.CallStatic("OnPageStart", page);
        }
            
        public static void OnPageEnded(string page)
        {
            UMengSdkHelper?.CallStatic("OnPageEnded", page);
        }

        public static void CustomEvent(string eventName, string json)
        {
            UMengSdkHelper?.CallStatic("CustomEvent", AndroidHelper.CurrentActivity, eventName, json);
        }

        public static void InitApm(string appVersion, string subVersion, string buildId)
        {
            UMengAPMApi?.CallStatic("InitAPM", AndroidHelper.CurrentActivity, appVersion, subVersion, buildId);
        }

        public static void SetCrashCustomCallback(string customCallback)
        {
            UMengAPMApi?.CallStatic("SetCrashCustomCallback", customCallback);
        }

        public static void GenerateCustomLog(string err, string type)
        {
            UMengAPMApi?.CallStatic("GenerateCustomLog", err, type);
        }
    }
    
}
#endif