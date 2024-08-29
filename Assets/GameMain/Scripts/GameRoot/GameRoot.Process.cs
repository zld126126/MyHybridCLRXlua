using System;
using System.Collections;
using System.Collections.Generic;
using GameMain.Utils;
using HybridCLR;

namespace GameMain.Scripts
{
    public partial class GameRoot
    {
        private System.Reflection.Assembly _gameDll = null;

        private IEnumerator LoadGameDll()
        {
            SimpleLog.Log("[GameRoot::LoadGameDll] Start");
            yield return ItorLoadData("hotfix.bytes", "hotfix", dict =>
            {
                if (dict == null)
                {
                    SimpleLog.Log("[GameRoot::LoadGameDll] dict is null!");
                    return;
                }

                if (!dict.TryGetValue("Assembly-CSharp.dll", out var hotfixData))
                {
                    SimpleLog.Log("[GameRoot::LoadGameDll] can't find Assembly-CSharp.dll!");
                    return;
                }

                if (hotfixData == null || hotfixData.Length == 0)
                {
                    SimpleLog.Log("[GameRoot::LoadGameDll] can't find Assembly-CSharp.dll is empty");
                    return;
                }

                SimpleLog.Log("[GameRoot::LoadGameDll] load Assembly-CSharp.dll ok!!!");

                _gameDll = System.Reflection.Assembly.Load(hotfixData);
                if (_gameDll == null)
                {
                    SimpleLog.Log("[GameRoot::LoadGameDll] load Assembly-CSharp.dll failed");
                    return;
                }

                SimpleLog.Log("[GameRoot::LoadGameDll] load Assembly-CSharp.dll success");
            });
        }

        private static void LoadMetadataForAotAssembly(Dictionary<string, byte[]> dllBytes)
        {
#if !UNITY_EDITOR
            dllBytes ??= new Dictionary<string, byte[]>();

            SimpleLog.Log($"[App::LoadMetadataForAotAssembly] Count:{dllBytes.Count}");
            foreach (var dll in dllBytes)
            {
                SimpleLog.Log($"[App::LoadMetadataForAotAssembly] load {dll.Key}");

                try
                {
                    var err = RuntimeApi.LoadMetadataForAOTAssembly(dll.Value, HomologousImageMode.SuperSet);
                    if (err != LoadImageErrorCode.OK)
                    {
                        SimpleLog.Log($"[App::LoadMetadataForAotAssembly] load {dll.Key} ret:{err}");
                    }
                }
                catch (Exception ex)
                {
                    SimpleLog.Log($"[App::LoadMetadataForAotAssembly] load {dll.Key} exception");
                    SimpleLog.LogException(ex);
                }
            }

            SimpleLog.Log($"[App::LoadMetadataForAotAssembly] finished!");
#endif
        }

        private IEnumerator RunDll()
        {
            SimpleLog.Log("[GameRoot::RunDll] Begin");

#if !UNITY_EDITOR
            yield return  ItorLoadData("base.bytes", "base", res =>
            {
                res ??= new Dictionary<string, byte[]>();
                LoadMetadataForAotAssembly(res);
            });
#endif

            if (_gameDll == null)
            {
                SimpleLog.Log($"[GameRoot::RunDll] Dll is null");
                yield break;
            }

            yield return null;

            var appType = _gameDll.GetType("HotFix.App");
            if (appType == null)
            {
                SimpleLog.Log($"[GameRoot::RunDll] appType is null");
                yield break;
            }

            yield return null;

            var mainMethod = appType.GetMethod("Main");
            if (mainMethod == null)
            {
                SimpleLog.Log($"[GameRoot::RunDll] Main is null");
                yield break;
            }

            mainMethod.Invoke(null, null);
        }
    }
}
