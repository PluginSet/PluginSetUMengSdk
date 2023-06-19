using System.IO;
using PluginSet.Core;
using PluginSet.Core.Editor;
using UnityEditor;
using UnityEngine;

namespace PluginSetUAPM.Editor
{
    [BuildTools]
    public static class BuildUMengTools
    {
        [OnSyncEditorSetting]
        public static void OnSyncEditorSetting(BuildProcessorContext context)
        {
            if (!context.BuildTarget.Equals(BuildTarget.Android))
                return;
            
            var buildParams = context.BuildChannels.Get<BuildUMengParams>();
            if (!buildParams.Enable)
                return;
            
            if (buildParams.EnableAnalytics)
                context.Symbols.Add("ENABLE_UMENG_ANALYTICS");
            if (buildParams.EnableCrashReport)
                context.Symbols.Add("ENABLE_UMENG_CRASH_REPORT");
            
            context.AddLinkAssembly("PluginSet.UMeng");
            Global.CopyDependenciesInLib("com.pluginset.umeng");
        }
        
        [AndroidProjectModify]
        public static void OnAndroidProjectModify(BuildProcessorContext context, AndroidProjectManager projectManager)
        {
            var buildParams = context.BuildChannels.Get<BuildUMengParams>();
            if (!buildParams.Enable)
                return;
            
            projectManager.AddApplicationPlugin("umeng", "com.pluginset.umeng.UMengSdkPlugin");

            var manifest = projectManager.LibraryManifest;
            
            manifest.AddUsePermission("android.permission.INTERNET", "umeng");
            manifest.AddUsePermission("android.permission.ACCESS_WIFI_STATE", "umeng");
            manifest.AddUsePermission("android.permission.READ_PHONE_STATE", "umeng");
            manifest.AddUsePermission("android.permission.ACCESS_NETWORK_STATE", "umeng");
            
            manifest.SetMetaData("UMENG_APPKEY", buildParams.AppKey);
            manifest.SetMetaData("UMENG_CHANNEL", $"Channel_{context.Channel}");

            if (buildParams.IntegrationTesting && !context.ProductMode)
            {
                var nodes = manifest.findElements($"{AndroidConst.ACTIVITY_PATH}/intent-filter/action",AndroidConst.NS_PREFIX, "name", "android.intent.action.MAIN");
                if (nodes.Count <= 0)
                    return;

                var node = nodes[0].ParentNode?.ParentNode;
                if (node == null)
                    return;

                var elem = node.createSubElement("intent-filter");
                elem.createSubElement("action").SetAttribute("name", AndroidConst.NS_URI, "android.intent.action.VIEW");
                elem.createSubElement("category").SetAttribute("name", AndroidConst.NS_URI, "android.intent.category.DEFAULT");
                elem.createSubElement("category").SetAttribute("name", AndroidConst.NS_URI, "android.intent.category.BROWSABLE");
                elem.createSubElement("data").SetAttribute("scheme", AndroidConst.NS_URI, $"um.{buildParams.AppKey}");
            }
        }

        [BuildCompletedCallback]
        public static void UploadSymbols(BuildProcessorContext context)
        {
            var buildParams = context.BuildChannels.Get<BuildUMengParams>();
            if (!buildParams.Enable)
                return;
            
            if (!buildParams.EnableCrashReport)
                return;

            var savePath = Path.Combine(context.ProjectPath, "um_symbols");
            var isWindows = Application.platform == RuntimePlatform.WindowsEditor;
            var sevenZip = Path.Combine(EditorApplication.applicationContentsPath, "Tools", isWindows ? "7z.exe" : "7za");
            if (isWindows)
                sevenZip = $"\"{sevenZip}\"";
#if !UNITY_2020_1_OR_NEWER
            string symbolZip = Path.GetFileNameWithoutExtension(context.ProjectPath) + "-" + PlayerSettings.bundleVersion + "-v" + (object) PlayerSettings.Android.bundleVersionCode + ".symbols.zip";
            string path = Path.GetFullPath(Path.Combine(context.ProjectPath, "..", symbolZip));
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Cannot find symbol.zip at path : {path}");
                return;
            }
            Global.ExecuteCommand(sevenZip, "x", "-tzip", "-y", path, $"-o{savePath}");
#else
            string symbolPath = Path.GetFullPath(Path.Combine(context.ProjectPath, "unityLibrary", "symbols"));
            Global.CopyFilesTo(savePath, symbolPath, "*.so", SearchOption.AllDirectories);
#endif
#if UNITY_ANDROID
            CopyAndroidUnitySymbols(savePath, context.DebugMode);
#endif
            
            if (!Directory.Exists(savePath))
            {
                Debug.LogWarning($"Cannot find symbols file at path : {savePath}");
                return;
            }

            var version = PluginUtil.GetVersionString(context.VersionName, int.Parse(context.VersionCode));
            var buildPrefix = $"um_{version.Replace('.', '_')}";
            var zipFile = Path.GetFullPath(Path.Combine(context.ProjectPath, $"{buildPrefix}_symbols.zip"));
            Global.ExecuteCommand(sevenZip, false, "a", zipFile, savePath);

            var tool = Path.Combine(Global.GetPackageFullPath("com.pluginset.umeng"), "Tools", "upload.py");
            Global.ExecuteCommand("python", false, Global.GetFullPath(tool), buildParams.ApiKey, buildParams.ApiSecurity, buildParams.AppKey, version, zipFile);
        }

        private static void CopyAndroidUnitySymbols(string targetPath, bool debug)
        {
#if UNITY_EDITOR_WIN
            var unitySymbols = Path.Combine(EditorApplication.applicationContentsPath
#else
            var unitySymbols = Path.Combine(EditorApplication.applicationPath, ".."
#endif
                , "PlaybackEngines", "AndroidPlayer",
                "Variations", "il2cpp", debug ? "Development" : "Release", "Symbols");
            
            Debug.Log($"CopyAndroidUnitySymbols: {unitySymbols}, {Directory.Exists(unitySymbols)}");
            if (!Directory.Exists(unitySymbols))
                return;

            unitySymbols = Global.GetFullPath(unitySymbols);
            targetPath = Global.GetFullPath(targetPath);
            foreach (var file in Directory.GetFiles(unitySymbols, "*.sym.so", SearchOption.AllDirectories))
            {
                var newFile = Global.GetFullPath(file).Replace(".sym.so", ".so");
                newFile = newFile.Replace(unitySymbols, targetPath);
                Debug.Log($"CopyAndroidUnitySymbols: {file}, {newFile}");
                Global.CopyFile(file, newFile, null);
            }
        }
        
        private static void CopyAppleAppSymbolsInApp(string targetPath, string appPath)
        {
            // TODO
            Global.CopyFilesTo(targetPath, Path.Combine(appPath, "dSYMs"), "*.dSYM");
        }
    }
}