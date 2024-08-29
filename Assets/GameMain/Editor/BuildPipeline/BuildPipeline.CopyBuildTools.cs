using System;
using System.IO;
using GameMain.Utils;
using HybridCLR.Editor;
using UnityEditor;

namespace GameMain.Editor.BuildPipeline
{
    public partial class BuildPipeline
    {
        //为非标准版本准备的，比如在Unity2020.3.3上编译，如果不使用相应版本的资源，就会导致编译失败（可能是xlua使用了比较复杂的用法）
        private static void CopyBuildToolsPath()
        {
#if !UNITY_IOS
            var src = Path.Combine(EditorApplication.applicationContentsPath, "il2cpp", "build");
            src = Path.GetFullPath(src).Replace('\\', '/');
            if (!Directory.Exists(src))
            {
                throw new Exception($"[BuildPipeline::CopyBuildToolsPath] {src} not Exists!!!");
            }

            var dst = Path.Combine(SettingsUtil.LocalIl2CppDir, "build");
            dst = Path.GetFullPath(dst).Replace('\\', '/');
            if (!Directory.Exists(dst))
            {
                throw new Exception($"[BuildPipeline::CopyBuildToolsPath] {dst} not Exists!!!");
            }

            Directory.Delete(dst, true);
            PathUtility.CopyFolder(src, dst);
#endif
        }
    }
}
