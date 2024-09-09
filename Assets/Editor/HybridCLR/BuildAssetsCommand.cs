using HybridCLR.Editor.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameMain.Scripts;
using GameMain.Utils;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;

namespace HybridCLR.Editor
{
    public static class BuildAssetsCommand
    {
        public static string HybridCLRBuildCacheDir => Application.dataPath + "/HybridCLRBuildCache";

        public static string AssetBundleOutputDir => $"{HybridCLRBuildCacheDir}/AssetBundleOutput";

        public static string AssetBundleSourceDataTempDir => $"{HybridCLRBuildCacheDir}/AssetBundleSourceData";


        public static string GetAssetBundleOutputDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleOutputDir}/{target}";
        }

        public static string GetAssetBundleTempDirByTarget(BuildTarget target)
        {
            return $"{AssetBundleSourceDataTempDir}/{target}";
        }

        public static string ToRelativeAssetPath(string s)
        {
            return s.Substring(s.IndexOf("Assets/"));
        }

        /// <summary>
        /// 将HotFix.dll和HotUpdatePrefab.prefab打入common包.
        /// 将HotUpdateScene.unity打入scene包.
        /// </summary>
        /// <param name="tempDir"></param>
        /// <param name="outputDir"></param>
        /// <param name="target"></param>
        private static void BuildAssetBundles(string tempDir, string outputDir, BuildTarget target)
        {
            Directory.CreateDirectory(tempDir);
            Directory.CreateDirectory(outputDir);
            
            List<AssetBundleBuild> abs = new List<AssetBundleBuild>();

            {
                var prefabAssets = new List<string>();
                string testPrefab = $"{Application.dataPath}/Prefabs/Cube.prefab";
                prefabAssets.Add(testPrefab);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                abs.Add(new AssetBundleBuild
                {
                    assetBundleName = "prefabs",
                    assetNames = prefabAssets.Select(s => ToRelativeAssetPath(s)).ToArray(),
                });
            }

            BuildPipeline.BuildAssetBundles(outputDir, abs.ToArray(), BuildAssetBundleOptions.None, target);
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        public static void BuildAssetBundleByTarget(BuildTarget target)
        {
            BuildAssetBundles(GetAssetBundleTempDirByTarget(target), GetAssetBundleOutputDirByTarget(target), target);
        }

        [MenuItem("Build/BuildAssetsAndCopyToStreamingAssets")]
        public static void BuildAndCopyABAOTHotUpdateDlls()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            BuildAssetBundleByTarget(target);
            CompileDllCommand.CompileDll(target);
            CopyABAOTHotUpdateDlls(target);
            AssetDatabase.Refresh();
        }
        
        private static void GenerateScripts(BuildTarget target, bool compileDll = true)
        {
            var hotDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            if (compileDll)
            {
                CompileDllCommand.CompileDll(target);
            }
            Generate(hotDllSrcDir, "hotfix.bytes", SettingsUtil.HotUpdateAssemblyFilesIncludePreserved);

            var aotDllDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            Generate(aotDllDir, "base.bytes", SettingsUtil.AOTAssemblyNames);

            var luaDir = Path.Combine(Application.dataPath, "Res", "lua");
            Generate(luaDir, "lua.bytes");
        }
        
        private static void Generate(string srcDir, string name, List<string> fileList = null)
        {
            var dstFile = Path.Combine(Application.streamingAssetsPath, name).Replace('\\', '/');
            if (File.Exists(dstFile))
            {
                File.Delete(dstFile);
            }

            var srcFullDir = Path.GetFullPath(srcDir).Replace('\\', '/');
            if (!srcFullDir.EndsWith("/"))
            {
                srcFullDir += "/";
            }

            if (fileList == null || fileList.Count == 0)
            {
                var files = Directory.GetFiles(srcFullDir, "*", SearchOption.AllDirectories);
                fileList = new List<string>(files.Length);
                foreach (var file in files)
                {
                    if (file.EndsWith(".meta"))
                    {
                        continue;
                    }

                    var tmp = file.Replace('\\', '/').Replace(srcFullDir, "");
                    fileList.Add(tmp);
                }
            }

            if (fileList.Count == 0)
            {
                SimpleLog.Log($"[BuildPipeline::GenerateScripts] {srcDir} filelist is empty!!! {name}");
                return;
            }

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write(fileList.Count);
            foreach (var file in fileList)
            {
                var filePath = $"{srcDir}/{file}";
                if (!File.Exists(filePath))
                {
                    filePath += ".dll";
                }

                if (!File.Exists(filePath))
                {
                    SimpleLog.Log($"[BuildPipeline::GenerateScripts]文件{filePath}不存在!");
                    continue;
                }

                bw.Write(file);
                var bytes = File.ReadAllBytes(filePath);
                bw.Write(bytes.Length);
                bw.Write(bytes);
            }
            
            var outBuffer = ms.ToArray();
            PathUtility.EnsureExistFileDirectory(dstFile);
            File.WriteAllBytes(dstFile, outBuffer);
        }

        [MenuItem("Build/BuildHybridClrXlua")]
        public static void BuildHybridClrXlua()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            GenerateLuaAssets(target);
            GenerateScripts(target);
        }

        private static bool CheckPlatform(BuildTarget target)
        {
            return target == EditorUserBuildSettings.activeBuildTarget;
        }
        
        private static void GenerateLuaAssets(BuildTarget target)
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

        public static void CopyABAOTHotUpdateDlls(BuildTarget target)
        {
            CopyAssetBundlesToStreamingAssets(target);
            CopyAOTAssembliesToStreamingAssets();
            CopyHotUpdateAssembliesToStreamingAssets();
        }


        //[MenuItem("HybridCLR/Build/BuildAssetbundle")]
        public static void BuildSceneAssetBundleActiveBuildTargetExcludeAOT()
        {
            BuildAssetBundleByTarget(EditorUserBuildSettings.activeBuildTarget);
        }

        public static void CopyAOTAssembliesToStreamingAssets()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            string aotAssembliesSrcDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            string aotAssembliesDstDir = Application.streamingAssetsPath;

            foreach (var dll in SettingsUtil.AOTAssemblyNames)
            {
                string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
                if (!File.Exists(srcDllPath))
                {
                    Debug.LogError($"ab中添加AOT补充元数据dll:{srcDllPath} 时发生错误,文件不存在。裁剪后的AOT dll在BuildPlayer时才能生成，因此需要你先构建一次游戏App后再打包。");
                    continue;
                }
                string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.dll.bytes";
                File.Copy(srcDllPath, dllBytesPath, true);
                Debug.Log($"[CopyAOTAssembliesToStreamingAssets] copy AOT dll {srcDllPath} -> {dllBytesPath}");
            }
        }

        public static void CopyHotUpdateAssembliesToStreamingAssets()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;

            string hotfixDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            string hotfixAssembliesDstDir = Application.streamingAssetsPath;
            foreach (var dll in SettingsUtil.HotUpdateAssemblyFilesExcludePreserved)
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}";
                string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.bytes";
                File.Copy(dllPath, dllBytesPath, true);
                Debug.Log($"[CopyHotUpdateAssembliesToStreamingAssets] copy hotfix dll {dllPath} -> {dllBytesPath}");
            }
        }

        public static void CopyAssetBundlesToStreamingAssets(BuildTarget target)
        {
            string streamingAssetPathDst = Application.streamingAssetsPath;
            Directory.CreateDirectory(streamingAssetPathDst);
            string outputDir = GetAssetBundleOutputDirByTarget(target);
            var abs = new string[] { "prefabs" };
            foreach (var ab in abs)
            {
                string srcAb = ToRelativeAssetPath($"{outputDir}/{ab}");
                string dstAb = ToRelativeAssetPath($"{streamingAssetPathDst}/{ab}");
                Debug.Log($"[CopyAssetBundlesToStreamingAssets] copy assetbundle {srcAb} -> {dstAb}");
                AssetDatabase.CopyAsset( srcAb, dstAb);
            }
        }
    }
}
