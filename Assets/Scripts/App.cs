using System;
using System.Collections;
using System.Collections.Generic;
using GameMain.Scripts;
using GameMain.Utils;
using HotFix.Utils;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HotFix
{
    public class App
    {
        private static IEnumerator RunLua(Action callback)
        {
            SimpleLog.Log($"[App::RunLua]");
            LuaCache.Clear();
            SimpleLog.Log($"[App::RunLua] LuaEnvUtil.Reset");
            if (LuaEnvUtil.Global == null)
            {
                LuaEnvUtil.Reset("", LuaCache.LoadLua);
            }

            var dataLoaded = false;
            GameRoot.Instance.LoadData("lua.bytes", "lua", scripts =>
            {
                dataLoaded = true;
                scripts ??= new Dictionary<string, byte[]>();
                foreach (var kv in scripts)
                {
                    LuaCache.Set(kv.Key, kv.Value);
                }
            });
            while (!dataLoaded)
            {
                yield return null;
            }

            var code = LuaCache.Get("Launcher.lua.txt");
            if (code == null)
            {
                SimpleLog.Log($"[App::RunLua] code is empty");
                yield break;
            }

            LuaEnvUtil.DoString(code);

            callback?.Invoke();
        }

        public static void Main()
        {
            SimpleLog.Log("[App::Main] Begin");

            var go = new GameObject("HotFixMain");
            Object.DontDestroyOnLoad(go);

            GameMain.Utils.Utility.ResolveVolumeManager();

            GameRoot.Instance.StartCoroutine(RunLua(() => { SimpleLog.Log("[App::Main] End"); }));
        }
    }
}
