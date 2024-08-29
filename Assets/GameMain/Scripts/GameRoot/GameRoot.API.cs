using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using GameMain.Utils;
using UnityEngine;
using UnityEngine.Networking;

namespace GameMain.Scripts
{
    public partial class GameRoot
    {
        public void LoadAssetBundle(string abName, Action<AssetBundle> callback)
        {
            StartCoroutine(ItorLoadAssetBundle(abName, callback));
        }

        public void LoadData(string fileName, string password, Action<Dictionary<string, byte[]>> callback)
        {
            StartCoroutine(ItorLoadData(fileName, password, callback));
        }
    }
}
