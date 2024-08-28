using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace GameMain.Scripts
{
    public partial class GameRoot
    {
        private IEnumerator ItorLoadAssetBundle(string abName, Action<AssetBundle> callback)
        {
            SimpleLog.Log("[GameRoot::LoadAssetBundle] Begin");

            AssetBundle ab = null;

            var writablePath = Path.Combine(Application.persistentDataPath, abName).Replace('\\', '/');
            if (File.Exists(writablePath))
            {
                SimpleLog.Log($"[GameRoot::LoadAssetBundle] LoadAssetBundle from {writablePath}");
                var loadFileAsync = AssetBundle.LoadFromFileAsync(writablePath);
                yield return loadFileAsync;

                ab = loadFileAsync.assetBundle;
                if (ab != null)
                {
                    callback?.Invoke(ab);
                    yield break;
                }
            }

            var streamingPath = Path.Combine(Application.streamingAssetsPath, abName).Replace('\\', '/');
            if (!streamingPath.Contains("://"))
            {
                streamingPath = "file://" + streamingPath;
            }

            SimpleLog.Log($"[GameRoot::LoadAssetBundle] LoadAssetBundle from {streamingPath}");

            using var www = UnityWebRequestAssetBundle.GetAssetBundle(streamingPath);
            yield return www.SendWebRequest();
#if UNITY_2020_1_OR_NEWER
            if (www.result != UnityWebRequest.Result.Success)
            {
                SimpleLog.Log($"[GameRoot::LoadAssetBundle] LoadAssetBundle from {streamingPath} error:{www.error}");
                callback?.Invoke(ab);
                yield break;
            }
#else
            if (www.isHttpError || www.isNetworkError)
            {
                SimpleLog.Log($"[GameRoot::LoadAssetBundle] LoadAssetBundle from {streamingPath} error:{www.error}");
                callback?.Invoke(ab);
                yield break;
            }
#endif
            ab = ((DownloadHandlerAssetBundle) www.downloadHandler)?.assetBundle;
            var abTips = ab == null ? "null" : "not null";
            SimpleLog.Log($"[GameRoot::LoadAssetBundle] LoadAssetBundle OK, AssetBundle is {abTips}");

            callback?.Invoke(ab);
        }
    }
}
