using PluginSet.Core;
using PluginSet.Core.Editor;
using UnityEngine;

namespace PluginSetUAPM.Editor
{
    [BuildChannelsParams("UMeng", "友盟SDK参数")]
    [VisibleCaseBoolValue("SupportAndroid", true)]
    public class BuildUMengParams: ScriptableObject
    {
        [Tooltip("是否启用友盟SDK")]
        public bool Enable;

        [Tooltip("是否启用数据收集（关闭状态下会有一些自动收集的数据）")]
        public bool EnableAnalytics;

        [Tooltip("是否启用崩溃数据上报系统")]
        public bool EnableCrashReport;

        [Tooltip("友盟SDK提供的APPKEY")]
        public string AppKey;
        
        [Tooltip("友盟OpenAPI访问凭证")]
        public string ApiKey;
        
        [Tooltip("友盟OpenAPI访问凭证")]
        public string ApiSecurity;

        // [Tooltip("推送SDK使用的推送密钥")]
        // public string PushSecret;
    }
}