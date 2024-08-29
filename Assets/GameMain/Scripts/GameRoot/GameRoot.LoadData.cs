using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameMain.Scripts.Utils;
using GameMain.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace GameMain.Scripts
{
    public partial class GameRoot
    {
        private IEnumerator ItorLoadData(string fileName, string password, Action<Dictionary<string, byte[]>> callback)
        {
            SimpleLog.Log($"[GameRoot::LoadData] {fileName}");
            byte[] bytes = null;
            var writablePath = Path.Combine(Application.persistentDataPath, fileName).Replace('\\', '/');
            if (File.Exists(writablePath))
            {
                SimpleLog.Log($"[GameRoot::LoadData] {fileName} from {writablePath}");
                bytes = File.ReadAllBytes(writablePath);
            }

            if (bytes == null || bytes.Length == 0)
            {
                var streamingPath = Path.Combine(Application.streamingAssetsPath, fileName).Replace('\\', '/');
                if (!streamingPath.Contains("://"))
                {
                    streamingPath = "file://" + streamingPath;
                }

                SimpleLog.Log($"[GameRoot::LoadData] {fileName} from {streamingPath}");

                var req = UnityWebRequest.Get(streamingPath);
                yield return req.SendWebRequest();

#if UNITY_2020_1_OR_NEWER
                if (req.result != UnityWebRequest.Result.Success)
                {
                    SimpleLog.Log($"[GameRoot::LoadData] error:{req.error}");
                    callback?.Invoke(null);
                    yield break;
                }
#else
                if (req.isHttpError || req.isNetworkError)
                {
                    SimpleLog.Log($"[GameRoot::LoadData] error:{req.error}");
                    callback?.Invoke(null);
                    yield break;
                }
#endif
                bytes = req.downloadHandler?.data;
            }

            if (bytes == null || bytes.Length == 0)
            {
                SimpleLog.Log($"[GameRoot::LoadData] bytes is empty!");
                callback?.Invoke(null);
                yield break;
            }

            SimpleLog.Log($"[GameRoot::LoadData] {fileName} load ok!");

            yield return null;

            bytes = XXTEA.Decrypt(bytes, password);

            SimpleLog.Log($"[GameRoot::LoadData] begin decompressfast....{fileName}");
            using var ms = new MemoryStream(bytes, 0, bytes.Length, false, true);
            using var output = new MemoryStream(bytes.Length);
            if (!GameMain.Utils.Utility.Decompress(ms, output))
            {
                SimpleLog.Log($"[GameRoot::LoadData] DecompressFast {fileName} error!");
                callback?.Invoke(null);
                yield break;
            }

            SimpleLog.Log($"[GameRoot::LoadData] end decompressfast....{fileName}");

            output.Seek(0, SeekOrigin.Begin);
            var dictBytes = new Dictionary<string, byte[]>();
            {
                using var br = new BinaryReader(output);
                var count = br.ReadInt32();
                for (var i = 0; i < count; ++i)
                {
                    var fn = br.ReadString();
                    var cnt = br.ReadInt32();
                    var buf = br.ReadBytes(cnt);

                    dictBytes.Add(fn, buf);
                }
            }

            SimpleLog.Log($"[GameRoot::LoadData] {fileName} finished!");

            callback?.Invoke(dictBytes);
        }
    }
}
