using System;
using System.IO;
using System.Reflection;
using GameMain.Scripts.Utils;
using UnityEngine;

namespace GameMain.Utils
{
    public class Utility
    {
        public static bool Compress(MemoryStream msInput, MemoryStream msOutput)
        {
            if (msInput == null || msInput.Length == 0)
            {
                return false;
            }

            msInput.Seek(0, SeekOrigin.Begin);

            var coder = new SevenZip.Compression.LZMA.Encoder();
            coder.WriteCoderProperties(msOutput);
            msOutput.Write(BitConverter.GetBytes(msInput.Length), 0, 8);
            coder.Code(msInput, msOutput, msInput.Length, -1, null);
            msOutput.Flush();

            return true;
        }

        public static bool Decompress(MemoryStream msInput, MemoryStream msOutput)
        {
#if UNITY_EDITOR
            return DecompressCS(msInput, msOutput);
#else
            var ret = RuntimeApi.Decompress(msInput.GetBuffer(), (int) msInput.Length);
            msOutput.Write(ret, 0, ret.Length);
            return ret.Length > 0;
#endif
        }

        private static bool DecompressCS(MemoryStream msInput, MemoryStream msOutput)
        {
            if (msInput == null || msInput.Length == 0)
            {
                return false;
            }

            msInput.Seek(0, SeekOrigin.Begin);

            var coder = new SevenZip.Compression.LZMA.Decoder();
            var properties = new byte[5];
            msInput.Read(properties, 0, 5);

            var fileLengthBytes = new byte[8];
            msInput.Read(fileLengthBytes, 0, 8);
            var fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

            msOutput.Capacity += (int) fileLength;
            coder.SetDecoderProperties(properties);
            coder.Code(msInput, msOutput, msInput.Length, fileLength, null);
            msOutput.Flush();

            return true;
        }

        public static void ResolveVolumeManager()
        {
            SimpleLog.Log("[Utility::ResolveVolumeManager] Start");

            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Static;

            #region 清理缓存的types

            var coreUtilsType = Type.GetType("UnityEngine.Rendering.CoreUtils,Unity.RenderPipelines.Core.Runtime");
            if (coreUtilsType == null)
            {
                SimpleLog.Log("[Utility::ResolveVolumeManager] 无需处理CoreUtils,你可能不在URP环境下");
                return;
            }

            coreUtilsType.GetField("m_AssemblyTypes", bindingFlags)?.SetValue(null, null);

            #endregion

            #region VolumeManager处理

            var volumeMgrType = Type.GetType("UnityEngine.Rendering.VolumeManager,Unity.RenderPipelines.Core.Runtime");
            if (volumeMgrType == null)
            {
                SimpleLog.Log("[Utility::ResolveVolumeManager] 无需处理VolumeManager,你可能不在URP环境下");
                return;
            }

            bindingFlags = BindingFlags.Public | BindingFlags.Static;
            var mgrInstanceField = volumeMgrType.GetProperty("instance", bindingFlags);
            if (mgrInstanceField == null)
            {
                SimpleLog.LogError("[Utility::ResolveVolumeManager] 无法找到VolumeManager的instance");
                return;
            }

            var mgrInstance = mgrInstanceField.GetValue(null);
            bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
            var reloadBaseTypesMethod = volumeMgrType.GetMethod("ReloadBaseTypes", bindingFlags);
            if (reloadBaseTypesMethod == null)
            {
                SimpleLog.Log($"[Utility::ResolveVolumeManager] reloadBaseTypesMethod is null");
                return;
            }

            // Mgr的stack重置
            bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var stackProperty = volumeMgrType.GetProperty("stack", bindingFlags);
            if (stackProperty == null)
            {
                SimpleLog.Log($"[Utility::ResolveVolumeManager] stackProperty is null");
                return;
            }

            bindingFlags = BindingFlags.Public | BindingFlags.Instance;
            var createStackMethod = volumeMgrType.GetMethod("CreateStack", bindingFlags);
            if (createStackMethod == null)
            {
                SimpleLog.LogError("[Utility::ResolveVolumeManager] 无法找到VolumeManager的CreateStack方法");
                return;
            }

            reloadBaseTypesMethod.Invoke(mgrInstance, null);
            stackProperty.SetValue(mgrInstance, createStackMethod.Invoke(mgrInstance, null));

            #endregion

            SimpleLog.Log($"[Utility::ResolveVolumeManager] Finished ");
        }
    }
}
