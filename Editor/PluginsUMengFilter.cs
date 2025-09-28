using System;
using System.Collections.Generic;
using PluginSet.Core.Editor;
using UnityEditor;

namespace PluginSet.UMeng.Editor
{
    [InitializeOnLoad]
    public static class PluginsUMengFilter
    {
        static PluginsUMengFilter()
        {
            PluginFilter.RegisterFilter("com.pluginset.umeng/Plugins/Android", new Dictionary<string, Func<string, BuildProcessorContext, bool>>
            {
                {"UMengSdkHelper.java", IsEnable()},
                {"UMengSdkPlugin.java", IsEnable()},
                {"UMengAPMApi.java", IsEnableCrashReport()},
            });
            
        }
        
        private static Func<string, BuildProcessorContext, bool> IsEnable()
        {
            return delegate(string _, BuildProcessorContext context)
            {
                if (!context.BuildTarget.Equals(BuildTarget.Android))
                    return true;
                
                var bulidParams = context.BuildChannels.Get<BuildUMengParams>();
                return !bulidParams.Enable;
            };
        }
        
        private static Func<string, BuildProcessorContext, bool> IsEnableAnalytics()
        {
            return delegate(string _, BuildProcessorContext context)
            {
                if (!context.BuildTarget.Equals(BuildTarget.Android))
                    return true;
                
                var bulidParams = context.BuildChannels.Get<BuildUMengParams>();
                return !bulidParams.Enable || !bulidParams.EnableAnalytics;
            };
        }
        
        private static Func<string, BuildProcessorContext, bool> IsEnableCrashReport()
        {
            return delegate(string _, BuildProcessorContext context)
            {
                if (!context.BuildTarget.Equals(BuildTarget.Android))
                    return true;
                
                var bulidParams = context.BuildChannels.Get<BuildUMengParams>();
                return !bulidParams.Enable || !bulidParams.EnableCrashReport;
            };
        }
    }
}