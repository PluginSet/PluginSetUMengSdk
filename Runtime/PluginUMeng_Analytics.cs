#if UNITY_EDITOR || ENABLE_UMENG_ANALYTICS
using System.Collections.Generic;
using PluginSet.Core;
using PluginSet.Core.MiniJSON;

namespace PluginSet.UMeng
{
    public partial class PluginUMeng: IAnalytics
    {
        public void CustomEvent(string customEventName, Dictionary<string, object> eventData = null)
        {
#if UNITY_ANDROID
            var json = "{}";
            if (eventData != null)
                json = Json.Serialize(eventData);
            
            UMengAndroidApi.CustomEvent(customEventName, json);
#endif
        }
    }
}
#endif