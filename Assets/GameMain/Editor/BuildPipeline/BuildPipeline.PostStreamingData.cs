using System;
using System.Collections.Generic;
using System.IO;
using HybridCLR.Editor;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor.BuildPipeline
{
    public partial class BuildPipeline
    {
        private static readonly List<string> LstCustomFiles = new List<string>
        {
            "hotfix.bytes",
            "base.bytes",
            "lua.bytes"
        };

        private static void PostStreamingData(BuildTarget target, string rootPath = null)
        {
            var srcPath = rootPath;
            if (string.IsNullOrEmpty(rootPath))
            {
                srcPath = $"{SettingsUtil.ProjectDir}/Temp/StagingArea/Data/StreamingAssets";
            }

            var streamingPath = Path.GetFullPath(srcPath).Replace('\\', '/');
            if (!streamingPath.EndsWith("/"))
            {
                streamingPath = streamingPath + "/";
            }

            if (!Directory.Exists(streamingPath))
            {
                throw new Exception($"[BuildPipeline] PostStreamingData {streamingPath} 不存在？？？？");
            }

            foreach (var customFile in LstCustomFiles)
            {
                var file = Path.Combine(Application.streamingAssetsPath, customFile);
                if (!File.Exists(file))
                {
                    throw new Exception($"[BuildPipeline] PostStreamingData {file} 不存在？？？？");
                }

                var dst = Path.Combine(streamingPath, customFile);
                File.Copy(file, dst, true);
            }
        }
    }
}
