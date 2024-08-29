using UnityEngine;

namespace GameMain.Editor.Setting
{
    public class HybridCLRxLuaSetting : ScriptableObject
    {
        [Header("打包player前先生成xlua脚本")]
        public bool CompileXluaBeforeBuildPlayer = true;

        [Header("是否启用global-metadata.dat加密")]
        public bool EnabledEncryptGlobalMetadata = true;

        [Header("如果启用加密，保存原始文件到此目录")]
        public string BackupOriginMetadataFilePath = "";
    }
}