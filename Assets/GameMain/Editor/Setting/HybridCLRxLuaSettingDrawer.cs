using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace GameMain.Editor.Setting
{
    [CustomEditor(typeof(HybridCLRxLuaSetting))]
    public class HybridClRxLuaSettingDrawer : UnityEditor.Editor
    {
        private SerializedObject _thisClass;
        private SerializedProperty _compileXluaBeforeBuildPlayer;
        private SerializedProperty _enabledEncryptGlobalMetadata;
        private SerializedProperty _customGlobalMetadataPassword;
        private SerializedProperty _backupOriginMetadataFilePath;
        private SerializedProperty _enableGlobalMetadataCompress;

        private void OnEnable()
        {
            _thisClass = new SerializedObject(target);

            _compileXluaBeforeBuildPlayer = _thisClass.FindProperty("CompileXluaBeforeBuildPlayer");
            _enabledEncryptGlobalMetadata = _thisClass.FindProperty("EnabledEncryptGlobalMetadata");
            _backupOriginMetadataFilePath = _thisClass.FindProperty("BackupOriginMetadataFilePath");
        }

        public override void OnInspectorGUI()
        {
            _thisClass.Update();

            EditorGUILayout.PropertyField(_compileXluaBeforeBuildPlayer);

            EditorGUILayout.PropertyField(_enabledEncryptGlobalMetadata);
            if (_enabledEncryptGlobalMetadata.boolValue)
            {
                EditorGUILayout.PropertyField(_backupOriginMetadataFilePath);

                var curPath = _backupOriginMetadataFilePath.stringValue;
                if (!string.IsNullOrEmpty(curPath) && curPath.Length > 0)
                {
                    if (!Directory.Exists(curPath))
                    {
                        _backupOriginMetadataFilePath.stringValue = string.Empty;
                    }
                }

                if (GUILayout.Button("修改原始文件存放的目录"))
                {
                    var path = EditorUtility.SaveFolderPanel("请选择存放原始文件的目录",
                        Application.dataPath, "");
                    if (string.IsNullOrEmpty(path))
                    {
                        EditorUtility.DisplayDialog("警告", "请选择合法的目录用以存储文件", "关闭");
                    }
                    else if (!Directory.Exists(path))
                    {
                        EditorUtility.DisplayDialog("警告", $"你选择的目录不存在", "关闭");
                    }
                    else
                    {
                        _backupOriginMetadataFilePath.stringValue = path;
                    }
                }
            }

            _thisClass.ApplyModifiedProperties();
        }
    }
}
