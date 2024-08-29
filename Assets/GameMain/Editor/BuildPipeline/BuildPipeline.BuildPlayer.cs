using System;
using System.IO;
using GameMain.Utils;
using GameMain.Editor.Setting;
using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;
using HybridCLR.Editor.Commands;
using UnityEditor.Compilation;

namespace GameMain.Editor.BuildPipeline
{
    public partial class BuildPipeline
    {
        private static HybridCLRxLuaSetting _hybridClRxLuaSetting = null;

        private static void ReloadHybridClRxLuaSetting()
        {
            var assets = AssetDatabase.FindAssets("HybridCLRxLuaSetting");
            if (assets.Length == 0)
            {
                throw new Exception($"[BuildPipeline::ReloadHybridClRxLuaSetting] HybridCLRxLuaSetting找不到!!!");
            }

            var filePath = AssetDatabase.GUIDToAssetPath(assets[0]);
            _hybridClRxLuaSetting = AssetDatabase.LoadAssetAtPath<HybridCLRxLuaSetting>(filePath);
            if (_hybridClRxLuaSetting == null)
            {
                _hybridClRxLuaSetting = ScriptableObject.CreateInstance<HybridCLRxLuaSetting>();
            }
        }

        private static void Prepare()
        {
            ReloadHybridClRxLuaSetting();

            PrepareMetadata();
        }

        [MenuItem("HybridCLRxLua/Export/ActiveBuildTarget", false, 102)]
        private static void Build_ActiveBuild()
        {
            ReloadHybridClRxLuaSetting();

            if (_hybridClRxLuaSetting.CompileXluaBeforeBuildPlayer)
            {
                EditorUtility.DisplayProgressBar("waiting...", "正在生成xLua wrap!", 0);
                CSObjectWrapEditor.Generator.GenAll();

                CompilationPipeline.RequestScriptCompilation();

                CompilationPipeline.compilationStarted += CompilationStarted;
                CompilationPipeline.compilationFinished += CompilationFinished;
            }
            else
            {
                Real_Build_ActiveBuild();
            }
        }

        private static void CompilationStarted(object _)
        {
            EditorUtility.DisplayProgressBar("waiting...", "正在生成xLua wrap!", 50);
            CompilationPipeline.compilationStarted -= CompilationStarted;
        }

        private static void CompilationFinished(object _)
        {
            CompilationPipeline.compilationFinished -= CompilationFinished;
            EditorUtility.DisplayProgressBar("waiting...", "正在生成xLua wrap!", 100);
            ReloadHybridClRxLuaSetting();
            EditorUtility.ClearProgressBar();
            Real_Build_ActiveBuild();
        }

        private static void Real_Build_ActiveBuild()
        {
            BuildAssetBundleActiveBuildTarget();

            switch (EditorUserBuildSettings.activeBuildTarget)
            {
                case BuildTarget.Android:
                    Build_Android();
                    break;
                case BuildTarget.StandaloneWindows64:
                    Build_Win64();
                    break;
                case BuildTarget.StandaloneOSX:
                    Build_MacOSX();
                    break;
                case BuildTarget.iOS:
                    Build_iOS();
                    break;
                default:
                    SimpleLog.LogError($"[BuildPipeline::CheckPlatform]" +
                                       $"{EditorUserBuildSettings.activeBuildTarget}" +
                                       $"not support..." +
                                       $"请先切到合适平台再打包");
                    break;
            }
        }

        private static BuildPlayerOptions _bpOption = new BuildPlayerOptions
        {
            scenes = new[] {"Assets/Scenes/GameMain.unity"},
            options = BuildOptions.CompressWithLz4,
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone,
        };

        private static void Build_Win64()
        {
            SimpleLog.Log($"[BuildPipeline::Build_Win64] Begin");

            var outputPath = $"{SettingsUtil.ProjectDir}/Release-Win64";
            var location = $"{outputPath}/HybridCLRXLua.exe";

            SimpleLog.Log($"[BuildPipeline::Build_Win64] BuildPlayer");

            _bpOption.locationPathName = location;
            _bpOption.target = BuildTarget.StandaloneWindows64;
            _bpOption.targetGroup = BuildTargetGroup.Standalone;
            var report = UnityEditor.BuildPipeline.BuildPlayer(_bpOption);
            SimpleLog.Log($"[BuildPipeline::Build_Win64] End");
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                SimpleLog.Log($"[BuildPipeline::Build_Win64] BuildPlayer 打包失败:{report.summary.result}");
            }
            else
            {
                Application.OpenURL($"file:///{outputPath}");
            }
        }

        private static void Build_Android()
        {
            SimpleLog.Log($"[BuildPipeline::Build_Android] Begin");

            var outputPath = $"{SettingsUtil.ProjectDir}/Release-Android/";
            var location = "";
            if (EditorUserBuildSettings.exportAsGoogleAndroidProject)
            {
                location = outputPath + "/HybridCLRXLua";
            }
            else
            {
                location = outputPath + "/HybridCLRXLua.apk";
            }

            SimpleLog.Log($"[BuildPipeline::Build_Android] BuildPlayer");

            _bpOption.locationPathName = location;
            _bpOption.target = BuildTarget.Android;
            _bpOption.targetGroup = BuildTargetGroup.Android;
            var report = UnityEditor.BuildPipeline.BuildPlayer(_bpOption);
            SimpleLog.Log($"[BuildPipeline::Build_Android] End");
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                SimpleLog.Log($"[BuildPipeline::Build_Android] BuildPlayer 打包失败:{report.summary.result}");
            }
            else
            {
                Application.OpenURL($"file:///{outputPath}");
            }
        }

        private static void Build_MacOSX()
        {
            SimpleLog.Log($"[BuildPipeline::Build_MacOSX] Begin");

            var outputPath = $"{SettingsUtil.ProjectDir}/Release-MacOSX";
            var location = $"{outputPath}/HybridCLRXLua.app";

            SimpleLog.Log($"[BuildPipeline::Build_MacOSX] BuildPlayer");

            _bpOption.locationPathName = location;
            _bpOption.target = BuildTarget.StandaloneOSX;
            _bpOption.targetGroup = BuildTargetGroup.Standalone;
            var report = UnityEditor.BuildPipeline.BuildPlayer(_bpOption);
            SimpleLog.Log($"[BuildPipeline::Build_MacOSX] End");
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                SimpleLog.Log($"[BuildPipeline::Build_MacOSX] BuildPlayer 打包失败:{report.summary.result}");
            }
            else
            {
                Application.OpenURL($"file:///{location}");
            }
        }

        private static void Build_iOS()
        {
            SimpleLog.Log($"[BuildPipeline::Build_iOS] Begin");

            var outputPath = $"{SettingsUtil.ProjectDir}/Release-iOS/";
            SimpleLog.Log($"[BuildPipeline::Build_iOS] BuildPlayer");

            _bpOption.locationPathName = outputPath;
            _bpOption.target = BuildTarget.iOS;
            _bpOption.targetGroup = BuildTargetGroup.iOS;
            var report = UnityEditor.BuildPipeline.BuildPlayer(_bpOption);
            SimpleLog.Log($"[BuildPipeline::Build_iOS] End");
            if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                SimpleLog.Log($"[BuildPipeline::Build_iOS] BuildPlayer 打包失败:{report.summary.result}");
            }
            else
            {
                Application.OpenURL($"file:///{outputPath}");
            }
        }
    }
}
