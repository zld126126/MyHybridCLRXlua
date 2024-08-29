using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameMain.Utils;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor.BuildPipeline
{
    public partial class BuildPipeline
    {
        private static string HybridClrBuildCacheDir => Application.dataPath + "/HybridCLRBuildCache";
        private static string AssetBundleOutputDir => $"{HybridClrBuildCacheDir}/AssetBundleOutput";
        private static string AssetBundleSourceDataTempDir => $"{HybridClrBuildCacheDir}/AssetBundleSourceData";

        private static string GetAssetBundleOutputDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleOutputDir}/{target}";
        }

        private static string GetAssetBundleTempDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleSourceDataTempDir}/{target}";
        }

        private static string ToRelativeAssetPath(string s)
        {
            var idx = s?.IndexOf("Assets/");
            if (idx == null || idx.Value < 0)
            {
                return "";
            }

            return s.Substring(idx.Value);
        }

        private static bool CheckPlatform(BuildTarget target)
        {
            return target == EditorUserBuildSettings.activeBuildTarget;
        }

        private static void BuildAssetBundleByTarget(BuildTarget target)
        {
            if (!CheckPlatform(target))
            {
                throw new Exception($"[BuildPipeline::CheckPlatform] 请先切到{target}平台再打包");
            }

            var outDir = GetAssetBundleOutputDirByTarget(target);
            PathUtility.EnsureExistDirectory(outDir);

            var buildTag = BuildAssetBundleOptions.DeterministicAssetBundle |
                           BuildAssetBundleOptions.ChunkBasedCompression |
                           BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;
            var abs = new List<AssetBundleBuild>();
            foreach (var _dir in Directory.GetDirectories("Assets/Res"))
            {
                var dir = _dir.Replace('\\', '/');
                if (dir.EndsWith("lua"))
                {
                    continue;
                }

                var assets = new List<string>();
                var abBuild = new AssetBundleBuild
                {
                    assetBundleName = dir.Replace("Assets/Res/", "")
                };
                var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    if (file.EndsWith(".meta"))
                    {
                        continue;
                    }

                    assets.Add(file);
                }

                abBuild.assetNames = assets.Select(ToRelativeAssetPath).ToArray();
                abs.Add(abBuild);
            }

            UnityEditor.BuildPipeline.BuildAssetBundles(outDir, abs.ToArray(), buildTag, target);

            var streamingAssetPathDst = $"{Application.streamingAssetsPath}";
            Directory.CreateDirectory(streamingAssetPathDst);

            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);

            foreach (var ab in abs)
            {
                AssetDatabase.CopyAsset(ToRelativeAssetPath($"{outDir}/{ab.assetBundleName}"),
                    ToRelativeAssetPath($"{streamingAssetPathDst}/{ab.assetBundleName}"));
            }
        }

        [MenuItem("HybridCLRxLua/AssetBundle/ActiveBuildTarget", false, 101)]
        private static void BuildAssetBundleActiveBuildTarget()
        {
            BuildAssetBundleByTarget(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("HybridCLRxLua/AssetBundle/Win64", false, 201)]
        private static void BuildAssetBundleWin64()
        {
            BuildAssetBundleByTarget(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("HybridCLRxLua/AssetBundle/Android", false, 202)]
        private static void BuildAssetBundleAndroid()
        {
            BuildAssetBundleByTarget(BuildTarget.Android);
        }

        [MenuItem("HybridCLRxLua/AssetBundle/OSX", false, 203)]
        private static void BuildAssetBundleMac()
        {
            BuildAssetBundleByTarget(BuildTarget.StandaloneOSX);
        }

        [MenuItem("HybridCLRxLua/AssetBundle/iOS", false, 204)]
        private static void BuildAssetBundleIos()
        {
            BuildAssetBundleByTarget(BuildTarget.iOS);
        }
    }
}
