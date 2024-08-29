using System.Collections.Generic;
using System.IO;
using GameMain.Utils;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor.BuildPipeline
{
    public partial class BuildPipeline
    {
        private static void Generate(string srcDir, string name, string password, List<string> fileList = null)
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

            using var output = new MemoryStream((int) ms.Length);
            if (!Utility.Compress(ms, output))
            {
                SimpleLog.LogError($"[BuildPipeline::GenerateScripts] {name}失败？？？？");
                return;
            }

            var outBuffer = output.ToArray();
            outBuffer = XXTEA.Encrypt(outBuffer, password);
            PathUtility.EnsureExistFileDirectory(dstFile);
            File.WriteAllBytes(dstFile, outBuffer);
        }

        private static void GenerateScripts(BuildTarget target, bool compileDll = true)
        {
            var hotDllSrcDir = SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target);
            if (compileDll)
            {
                CompileDllCommand.CompileDll(target);
            }

            Generate(hotDllSrcDir, "hotfix.bytes", "hotfix", SettingsUtil.HotUpdateAssemblyFilesIncludePreserved);

            var aotDllDir = SettingsUtil.GetAssembliesPostIl2CppStripDir(target);
            var manifest = Resources.Load<HotUpdateAssemblyManifest>("HotUpdateAssemblyManifest");
            var aotMetadataDll = new List<string>();
            if (manifest != null && manifest.AOTMetadataDlls != null && manifest.AOTMetadataDlls.Length > 0)
            {
                aotMetadataDll = new List<string>(manifest.AOTMetadataDlls);
            }
            Generate(aotDllDir, "base.bytes", "base", aotMetadataDll);

            var luaDir = Path.Combine(Application.dataPath, "Res", "lua");
            Generate(luaDir, "lua.bytes", "lua");
        }

        [MenuItem("HybridCLRxLua/GenerateScripts/ActiveBuildTarget", false, 100)]
        private static void GenerateDllActiveBuildTarget()
        {
            GenerateScripts(EditorUserBuildSettings.activeBuildTarget);
        }

        [MenuItem("HybridCLRxLua/GenerateScripts/Win64", false, 201)]
        private static void GenerateDllWin64()
        {
            GenerateScripts(BuildTarget.StandaloneWindows64);
        }

        [MenuItem("HybridCLRxLua/GenerateScripts/Android", false, 202)]
        private static void GenerateDllAndroid()
        {
            GenerateScripts(BuildTarget.Android);
        }

        [MenuItem("HybridCLRxLua/GenerateScripts/OSX", false, 203)]
        private static void GenerateDllOsx()
        {
            GenerateScripts(BuildTarget.StandaloneOSX);
        }

        [MenuItem("HybridCLRxLua/GenerateScripts/iOS", false, 204)]
        private static void GenerateDllIos()
        {
            GenerateScripts(BuildTarget.iOS);
        }
    }
}
