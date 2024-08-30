using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HybridCLR;
using UnityEngine;
using UnityEngine.Networking;

namespace GameMain.Scripts
{
    public partial class GameRoot
    {
        private static Assembly _hotUpdateAss;

        #region download assets

        private static Dictionary<string, byte[]> s_assetDatas = new Dictionary<string, byte[]>();

        public static byte[] ReadBytesFromStreamingAssets(string dllName)
        {
            return s_assetDatas[dllName];
        }

        private string GetWebRequestPath(string asset)
        {
            var path = $"{Application.streamingAssetsPath}/{asset}";
            if (!path.Contains("://"))
            {
                path = "file://" + path;
            }

            return path;
        }

        private static List<string> AOTMetaAssemblyFiles { get; } = new List<string>()
        {
            "mscorlib.dll.bytes",
            "System.dll.bytes",
            "System.Core.dll.bytes",
        };

        IEnumerator DownLoadAssets(Action onDownloadComplete)
        {
            var assets = new List<string>
            {
                "prefabs",
                "Assembly-CSharp",
            }.Concat(AOTMetaAssemblyFiles);

            foreach (var asset in assets)
            {
                string dllPath = GetWebRequestPath(asset);
                Debug.Log($"start download asset:{dllPath}");
                UnityWebRequest www = UnityWebRequest.Get(dllPath);
                yield return www.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
#else
            if (www.isHttpError || www.isNetworkError)
            {
                Debug.Log(www.error);
            }
#endif
                else
                {
                    // Or retrieve results as binary data
                    byte[] assetData = www.downloadHandler.data;
                    Debug.Log($"dll:{asset}  size:{assetData.Length}");
                    s_assetDatas[asset] = assetData;
                }
            }

            onDownloadComplete();
        }

        #endregion

        /// <summary>
        /// 为aot assembly加载原始metadata， 这个代码放aot或者热更新都行。
        /// 一旦加载后，如果AOT泛型函数对应native实现不存在，则自动替换为解释模式执行
        /// </summary>
        private static void LoadMetadataForAOTAssemblies()
        {
            /// 注意，补充元数据是给AOT dll补充元数据，而不是给热更新dll补充元数据。
            /// 热更新dll不缺元数据，不需要补充，如果调用LoadMetadataForAOTAssembly会返回错误
            /// 
            HomologousImageMode mode = HomologousImageMode.SuperSet;
            foreach (var aotDllName in AOTMetaAssemblyFiles)
            {
                byte[] dllBytes = ReadBytesFromStreamingAssets(aotDllName);
                // 加载assembly对应的dll，会自动为它hook。一旦aot泛型函数的native函数不存在，用解释器版本代码
                LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, mode);
                Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. mode:{mode} ret:{err}");
            }
        }

        private IEnumerator LoadGameDll2()
        {
            SimpleLog.Log("[GameRoot::LoadGameDll2] Download");
            StartCoroutine(DownLoadAssets(() =>
            {
                SimpleLog.Log("[GameRoot::LoadGameDll2] Begin");
                LoadMetadataForAOTAssemblies();
                //Run_InstantiateComponentByAsset();

#if !UNITY_EDITOR
                _hotUpdateAss = Assembly.Load(ReadBytesFromStreamingAssets("HotUpdate.dll.bytes"));
#else
                _hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies()
                    .First(a => a.GetName().Name == "HotUpdate");
#endif
                var appType = _hotUpdateAss.GetType("HotFix.App");
                if (appType == null)
                {
                    SimpleLog.Log($"[GameRoot::RunDll] appType is null");
                    return;
                }

                var mainMethod = appType.GetMethod("Main");
                if (mainMethod == null)
                {
                    SimpleLog.Log($"[GameRoot::RunDll] Main is null");
                    return;
                }
                mainMethod.Invoke(null, null);
                SimpleLog.Log("[GameRoot::LoadGameDll2] Start");
            }));
            yield return null;
        }

        private static void Run_InstantiateComponentByAsset()
        {
            // 通过实例化assetbundle中的资源，还原资源上的热更新脚本
            AssetBundle ab = AssetBundle.LoadFromMemory(ReadBytesFromStreamingAssets("prefabs"));
            GameObject cube = ab.LoadAsset<GameObject>("Cube");
            GameObject.Instantiate(cube);
        }

        private IEnumerator RunDll2()
        {
            yield return null;
        }
    }
}