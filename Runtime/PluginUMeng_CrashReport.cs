#if UNITY_EDITOR || ENABLE_UMENG_CRASH_REPORT
using PluginSet.Core;
using UnityEngine;

namespace PluginSet.UMeng
{
    public partial class PluginUMeng
    {
        private void InitCrash(string subVersion)
        {
#if UNITY_ANDROID
            var version = Application.version;
            var build = Devices.GetVersionCode();
            UMengAndroidApi.InitApm(PluginUtil.GetVersionString(version, build), subVersion, build.ToString());
#endif
            AddEventListener(PluginConstants.NOTIFY_REPORT, delegate(PluginsEventContext context)
            {
                OnPluginCoreReportError(context.Data is ReportError data ? data : default);
            });
        }
        
        private void OnPluginCoreReportError(ReportError error)
        {
            if (error.ThreadName.Equals("MainThread"))
                return;

#if UNITY_ANDROID
            UMengAndroidApi.GenerateCustomLog(error.StackTrack, $"{error.LogType}:{error.Condition}");
#endif
        }
    }
}
#endif