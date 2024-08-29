/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using System.Collections.Generic;
using System;
using XLua;
using System.Reflection;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEditor;

//配置的详细介绍请看Doc下《XLua的配置.doc》
public static class ExampleConfig
{
    /***************如果你全lua编程，可以参考这份自动化配置***************/
    //--------------begin 纯lua编程配置参考----------------------------
    private static List<string> exclude = new List<string>
    {
        "tvOS", "EnforceJobResult", "AtomicSafetyHandle", "AtomicSafetyErrorType", "PlayableGraph", "MaterialChecks",
        "RenderPipelineAsset", "GraphicsSettings", "ScriptableRenderContext", "LowLevel.Unsafe", "Analytics",
        "Tilemap", "NavMesh", "OffMeshLink", "UIElements", "VFX", "XR", "WindZone", "CanvasRenderer",
        "SkeletonRenderer","ParticleSystemRenderer", "BlendModeMaterials", "SpineSpriteAtlasAsset", "AssetManager.",
        "SkeletonDataCompatibility", "UMP.", "SDKNativeAPI", "ThridPart_Reporter", "GamepadSpeakerOutputType",
        "MikuLuaProfiler.", "InputManagerEntry", "ICSharpCode.SharpZipLib", "UnusedInXLua", "SevenZip",
        "SpriteTileMode", "Cinemachine", "PlayerLoop", "ClusterSerialization", "LightingSettings", "CurvyConnection",
        "HideInInspector", "ExecuteInEditMode", "AddComponentMenu", "ContextMenu", "RequireComponent",
        "DisallowMultipleComponent", "SerializeField", "AssemblyIsEditorAssembly", "Attribute", "Types",
        "UnitySurrogateSelector", "TrackedReference", "TypeInferenceRules", "FFTWindow", "RPC", "MasterServer",
        "BitStream", "HostData", "ConnectionTesterStatus", "GUI", "EventType", "EventModifiers", "FontStyle",
        "TextAlignment", "TextEditor", "TextEditorDblClickSnapping", "TextGenerator", "TextClipping", "Gizmos",
        "ADBannerView", "ADInterstitialAd", "Android", "Tizen", "jvalue", "Windows", "Flash", "AOT", "iOS",
        "CalendarIdentifier", "CalendarUnit", "CalendarUnit", "ClusterInput", "FullScreenMovieControlMode",
        "Apple", "Handheld", "LocalNotification", "NotificationServices", "RemoteNotificationType", "Motion",
        "FullScreenMovieScalingMode", "SamsungTV", "TextureCompressionQuality", "TouchScreenKeyboardType",
        "TouchScreenKeyboard", "UnityEngineInternal", "Terrain", "Tree", "SplatPrototype", "DetailPrototype",
        "DetailRenderMode", "MeshSubsetCombineUtility", "Social", "Enumerator", "SendMouseEvents", "Cursor",
        "ActionScript", "SimpleJson.Reflection", "CoroutineTween", "GraphicRebuildTracker", "Advertisements",
        "UnityEditor", "EventProvider",  "ClusterInput",  "UnityEngine.UI.ReflectionMethodsCache", "iPhone",
        "WSA", "NativeLeakDetectionMode", "WWWAudioExtensions", "UnityEngine.Experimental", "ClusterNetwork",
        "ContextMenu", "ClusterInput", "EventProvider", "iPhoneUtils", "GraphicRebuildTracker", "AddComponentMenu",
        "AssemblyIsEditorAssembly", "BeforeRenderOrderAttribute", "ColorUsageAttribute", "ContextMenuItemAttribute",
        "CreateAssetMenuAttribute", "CustomGridBrushAttribute", "DelayedAttribute", "DisallowMultipleComponent",
        "ExcludeFromObjectFactoryAttribute", "ExcludeFromPresetAttribute", "ExecuteAlways", "ExecuteInEditMode",
        "GradientUsageAttribute", "GUITargetAttribute", "HeaderAttribute", "HelpURLAttribute", "HideInInspector",
        "ImageEffectAfterScale", "ImageEffectAllowedInSceneView", "ImageEffectOpaque", "ImageEffectTransformsToLDR",
        "RPC", "ImageEffectUsesCommandBuffer", "InspectorNameAttribute", "MinAttribute", "PreferBinarySerialization",
        "WSA", "MultilineAttribute", "RangeAttribute", "RequireComponent", "RuntimeInitializeOnLoadMethodAttribute",
        "PropertyAttribute", "SerializeField", "SerializeReference", "SharedBetweenAnimatorsAttribute", "Ping",
        "SelectionBaseAttribute", "TextAreaAttribute", "TooltipAttribute", "UnityAPICompatibilityVersionAttribute",
        "UnityEngineInternal", "DrivenRectTransformTracker", "LightProbeGroup", "Curvy.Generator",
        "RemoteNotification", "MovieTexture", "NativeLeakDetection", "SpaceAttribute", "CurvyGizmoHelper",
        "CloudStreaming", "AudioSource", "UnityEngine.Debug"
    };

    [CSharpCallLua]
    public static List<Type> MyCsharpCallLuaTypes
    {
        get
        {
            List<Type> extraTypes = new List<Type>
            {
                typeof(UnityEngine.Events.UnityAction),
                typeof(UnityEngine.Events.UnityAction<UnityEngine.SceneManagement.Scene,
                    UnityEngine.SceneManagement.LoadSceneMode>),
            };
            return extraTypes;
        }
    }

    static bool isExcluded(Type type)
    {
        var fullName = type.FullName;
        for (int i = 0; i < exclude.Count; i++)
        {
            if (fullName.Contains(exclude[i]))
            {
                return true;
            }
        }

        return false;
    }

    private static string _il2cppManagedPath = string.Empty;

    private static string il2cppManagedPath
    {
        get
        {
            if (string.IsNullOrEmpty(_il2cppManagedPath))
            {
                var contentsPath = EditorApplication.applicationContentsPath;
                var extendPath = "";

                var buildTarget = EditorUserBuildSettings.activeBuildTarget;
#if UNITY_EDITOR_WIN
                switch (buildTarget)
                {
                    case BuildTarget.StandaloneWindows64:
                    case BuildTarget.StandaloneWindows:
                        extendPath = "PlaybackEngines/windowsstandalonesupport/Variations/il2cpp/Managed/";
                        break;
                    case BuildTarget.iOS:
                        extendPath = "PlaybackEngines/iOSSupport/Variations/il2cpp/Managed/";
                        break;
                    case BuildTarget.Android:
                        extendPath = "PlaybackEngines/AndroidPlayer/Variations/il2cpp/Managed/";
                        break;
                    default:
                        throw new Exception($"[xLua::LuaCallCSharp] 请选择合适的平台, 目前是:{buildTarget}");
                }
#elif UNITY_EDITOR_OSX
                switch (buildTarget)
                {
                    case BuildTarget.StandaloneOSX:
                        extendPath = "PlaybackEngines/MacStandaloneSupport/Variations/il2cpp/Managed/";
                        break;
                    case BuildTarget.iOS:
                        extendPath = "../../PlaybackEngines/iOSSupport/Variations/il2cpp/Managed/";
                        break;
                    case BuildTarget.Android:
                        extendPath = "../../PlaybackEngines/AndroidPlayer/Variations/il2cpp/Managed/";
                        break;
                    default:
                        throw new Exception($"[xLua::LuaCallCSharp] 请选择合适的平台, 目前是:{buildTarget}");
                }
#endif
                if (string.IsNullOrEmpty(extendPath))
                {
                    throw new Exception($"[xLua::LuaCallCSharp] 请选择合适的平台, 目前是:{buildTarget}");
                }

                _il2cppManagedPath = Path.Combine(contentsPath, extendPath).Replace('\\', '/');
            }

            return _il2cppManagedPath;
        }
    }

    private static IEnumerable<Type> _luaCallCSharp = null;
    [LuaCallCSharp]
    public static IEnumerable<Type> LuaCallCSharp
    {
        get
        {
            if (_luaCallCSharp != null)
            {
                return _luaCallCSharp;
            }

            var basePath = il2cppManagedPath;
            if (!Directory.Exists(basePath))
            {
                Debug.LogWarning($"[Xlua::ExampleConfig::LuaCallCSharp] can't find il2cpp's dlls [{basePath}]");
                basePath = basePath.Replace("/il2cpp/", "/mono/");
            }

            if (!Directory.Exists(basePath))
            {
                Debug.LogWarning($"[Xlua::ExampleConfig::LuaCallCSharp] can't find il2cpp's dlls [{basePath}]");
                return null;
            }

            var files = new List<string>(Directory.GetFiles(basePath, "*.dll"));

            var unityTypes =
                from ass in files.Select(s => Assembly.LoadFrom(s))
                from type in ass.GetExportedTypes()
                where (type.Namespace != null && !isExcluded(type) && type.BaseType != typeof(MulticastDelegate)
                       && !type.IsInterface /*|| (namespaces.Contains(type.Namespace))*/) /*&& !type.IsEnum*/
                select type;

            var assemblies = new[]
            {
                "GameMain",
                "Assembly-CSharp",
                "HybridCLR.Runtime"
            };
            var customTypes = from assembly in assemblies.Select(s => Assembly.Load(s))
                from type in assembly.GetExportedTypes()
                where type.Namespace == null || !type.Namespace.StartsWith("XLua")
                    && type.BaseType != typeof(MulticastDelegate)
                    && !type.IsInterface /*&& !type.IsEnum*/ && !isExcluded(type)
                select type;
            _luaCallCSharp = unityTypes.Concat(customTypes);

            return _luaCallCSharp;
        }
    }

    //自动把LuaCallCSharp涉及到的delegate加到CSharpCallLua列表，后续可以直接用lua函数做callback
    [CSharpCallLua]
    public static List<Type> CSharpCallLua
    {
        get
        {
            var lua_call_csharp = LuaCallCSharp;
            var delegate_types = new List<Type>();
            var flag = BindingFlags.Public | BindingFlags.Instance
                                           | BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly;
            foreach (var field in (from type in lua_call_csharp select type).SelectMany(type => type.GetFields(flag)))
            {
                if (typeof(Delegate).IsAssignableFrom(field.FieldType))
                {
                    delegate_types.Add(field.FieldType);
                }
            }

            foreach (var method in (from type in lua_call_csharp select type).SelectMany(type => type.GetMethods(flag)))
            {
                if (typeof(Delegate).IsAssignableFrom(method.ReturnType))
                {
                    delegate_types.Add(method.ReturnType);
                }

                foreach (var param in method.GetParameters())
                {
                    var paramType = param.ParameterType.IsByRef
                        ? param.ParameterType.GetElementType()
                        : param.ParameterType;
                    if (typeof(Delegate).IsAssignableFrom(paramType))
                    {
                        delegate_types.Add(paramType);
                    }
                }
            }

            return delegate_types.Where(t =>
                    t.BaseType == typeof(MulticastDelegate) && !hasGenericParameter(t) && !delegateHasEditorRef(t))
                .Distinct().ToList();
        }
    }
    //--------------end 纯lua编程配置参考----------------------------

    /***************热补丁可以参考这份自动化配置***************/
    //[Hotfix]
    //static IEnumerable<Type> HotfixInject
    //{
    //    get
    //    {
    //        return (from type in Assembly.Load("Assembly-CSharp").GetTypes()
    //                where type.Namespace == null || !type.Namespace.StartsWith("XLua")
    //                select type);
    //    }
    //}
    //--------------begin 热补丁自动化配置-------------------------
    static bool hasGenericParameter(Type type)
    {
        if (type.IsGenericTypeDefinition) return true;
        if (type.IsGenericParameter) return true;
        if (type.IsByRef || type.IsArray)
        {
            return hasGenericParameter(type.GetElementType());
        }

        if (type.IsGenericType)
        {
            foreach (var typeArg in type.GetGenericArguments())
            {
                if (hasGenericParameter(typeArg))
                {
                    return true;
                }
            }
        }

        return false;
    }

    static bool typeHasEditorRef(Type type)
    {
        if (type.Namespace != null && (type.Namespace == "UnityEditor" || type.Namespace.StartsWith("UnityEditor.")))
        {
            return true;
        }

        if (type.IsNested)
        {
            return typeHasEditorRef(type.DeclaringType);
        }

        if (type.IsByRef || type.IsArray)
        {
            return typeHasEditorRef(type.GetElementType());
        }

        if (type.IsGenericType)
        {
            foreach (var typeArg in type.GetGenericArguments())
            {
                if (typeHasEditorRef(typeArg))
                {
                    return true;
                }
            }
        }

        return false;
    }

    static bool delegateHasEditorRef(Type delegateType)
    {
        if (typeHasEditorRef(delegateType)) return true;
        var method = delegateType.GetMethod("Invoke");
        if (method == null)
        {
            return false;
        }

        if (typeHasEditorRef(method.ReturnType)) return true;
        return method.GetParameters().Any(pinfo => typeHasEditorRef(pinfo.ParameterType));
    }

    // 配置某Assembly下所有涉及到的delegate到CSharpCallLua下，Hotfix下拿不准那些delegate需要适配到lua function可以这么配置
    //[CSharpCallLua]
    //static IEnumerable<Type> AllDelegate
    //{
    //    get
    //    {
    //        BindingFlags flag = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
    //        List<Type> allTypes = new List<Type>();
    //        var allAssemblys = new Assembly[]
    //        {
    //            Assembly.Load("Assembly-CSharp")
    //        };
    //        foreach (var t in (from assembly in allAssemblys from type in assembly.GetTypes() select type))
    //        {
    //            var p = t;
    //            while (p != null)
    //            {
    //                allTypes.Add(p);
    //                p = p.BaseType;
    //            }
    //        }
    //        allTypes = allTypes.Distinct().ToList();
    //        var allMethods = from type in allTypes
    //                         from method in type.GetMethods(flag)
    //                         select method;
    //        var returnTypes = from method in allMethods
    //                          select method.ReturnType;
    //        var paramTypes = allMethods.SelectMany(m => m.GetParameters()).Select(pinfo => pinfo.ParameterType.IsByRef ? pinfo.ParameterType.GetElementType() : pinfo.ParameterType);
    //        var fieldTypes = from type in allTypes
    //                         from field in type.GetFields(flag)
    //                         select field.FieldType;
    //        return (returnTypes.Concat(paramTypes).Concat(fieldTypes)).Where(t => t.BaseType == typeof(MulticastDelegate) && !hasGenericParameter(t) && !delegateHasEditorRef(t)).Distinct();
    //    }
    //}
    //--------------end 热补丁自动化配置-------------------------

    //黑名单
   [BlackList] public static List<List<string>> BlackList = new List<List<string>>()
    {
        new List<string>() {"System.Xml.XmlNodeList", "ItemOf"},
        new List<string>() {"UnityEngine.WWW", "movie"},
        new List<string>() {"UnityEngine.Timeline.AnimationPlayableAsset", "LiveLink"},
#if UNITY_WEBGL
                new List<string>(){"UnityEngine.WWW", "threadPriority"},
#endif
        new List<string>() {"UnityEngine.Texture2D", "alphaIsTransparency"},
        new List<string>() {"UnityEngine.Security", "GetChainOfTrustValue"},
        new List<string>() {"UnityEngine.CanvasRenderer", "onRequestRebuild"},
        new List<string>() {"UnityEngine.Light", "areaSize"},
        new List<string>() {"UnityEngine.Light", "lightmapBakeType"},
        new List<string>() {"UnityEngine.WWW", "MovieTexture"},
        new List<string>() {"UnityEngine.WWW", "GetMovieTexture"},
        new List<string>() {"UnityEngine.AnimatorOverrideController", "PerformOverrideClipListCleanup"},
#if !UNITY_WEBPLAYER
        new List<string>() {"UnityEngine.Application", "ExternalEval"},
#endif
        new List<string>() {"UnityEngine.GameObject", "networkView"}, //4.6.2 not support
        new List<string>() {"UnityEngine.Component", "networkView"}, //4.6.2 not support
        new List<string>()
            {"System.IO.FileInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>() {"System.IO.FileInfo", "SetAccessControl", "System.Security.AccessControl.FileSecurity"},
        new List<string>()
            {"System.IO.DirectoryInfo", "GetAccessControl", "System.Security.AccessControl.AccessControlSections"},
        new List<string>()
            {"System.IO.DirectoryInfo", "SetAccessControl", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>()
        {
            "System.IO.DirectoryInfo", "CreateSubdirectory", "System.String",
            "System.Security.AccessControl.DirectorySecurity"
        },
        new List<string>() {"System.IO.DirectoryInfo", "Create", "System.Security.AccessControl.DirectorySecurity"},
        new List<string>() {"UnityEngine.MonoBehaviour", "runInEditMode"},


        //wjf add//
        // OnRequestRebuild
        //new List<string>(){ "UnityEngine.CanvasRenderer.OnRequestRebuild"},

        new List<string>() {"UnityEngine.Caching", "SetNoBackupFlag", "UnityEngine.CachedAssetBundle"},
        new List<string>() {"UnityEngine.Caching", "SetNoBackupFlag", "System.String", "UnityEngine.Hash128"},
        new List<string>() {"UnityEngine.Caching", "ResetNoBackupFlag", "UnityEngine.CachedAssetBundle"},
        new List<string>() {"UnityEngine.Caching", "ResetNoBackupFlag", "System.String", "UnityEngine.Hash128"},

        new List<string>() {"UnityEngine.AudioSettings", "GetSpatializerPluginNames"},
        new List<string>() {"UnityEngine.AudioSettings", "SetSpatializerPluginName", "System.String"},
        new List<string>() {"UnityEngine.Light", "SetLightDirty"},
        new List<string>() {"UnityEngine.Light", "shadowRadius"},
        new List<string>() {"UnityEngine.Light", "shadowAngle"},

        new List<string>()
            {"UnityEngine.TerrainData", "SetTerrainLayersRegisterUndo", "UnityEngine.TerrainLayer[]", "System.String"},

        new List<string>() {"UnityEngine.Terrain", "bakeLightProbesForTrees"},
        new List<string>() {"UnityEngine.Terrain", "deringLightProbesForTrees"},
        new List<string>() {"UnityEngine.AnimatorControllerParameter", "name", "System.String"},
        new List<string>() {"UnityEngine.Input", "IsJoystickPreconfigured", "System.String"},
        new List<string>() {"UnityEngine.ParticleSystemForceField", "FindAll"},
        new List<string>() {"UnityEngine.QualitySettings", "streamingMipmapsRenderersPerFrame", "System.Int32"},
        new List<string>() {"UnityEngine.Texture", "imageContentsHash", "System.Int32"},
        new List<string>() {"UnityEngine.UI.DefaultControls", "factory", "UnityEngine.UI.IFactoryControls"},
        new List<string>() {"UnityEngine.UI.Graphic", "OnRebuildRequested"},
        new List<string>() {"UnityEngine.UI.Text", "OnRebuildRequested"},

        new List<string>() {"UnityEngine.MeshRenderer", "scaleInLightmap", "System.Single"},
        new List<string>() {"UnityEngine.MeshRenderer", "receiveGI", "UnityEngine.ReceiveGI"},
        new List<string>() {"UnityEngine.MeshRenderer", "stitchLightmapSeams", "System.Boolean"},

        new List<string>() {"FluffyUnderware.Curvy.CurvySpline", "_newSelectionInstanceIDINTERNAL", "System.Int32"},
        new List<string>() {"FluffyUnderware.Curvy.CurvySpline", "ApplyControlPointsNames"},
        new List<string>() {"FluffyUnderware.DevTools.DTUtility", "FormatJson", "System.String"},

    };

#if UNITY_2018_1_OR_NEWER
    [BlackList] public static Func<MemberInfo, bool> MethodFilter = (memberInfo) =>
    {
        if (memberInfo.DeclaringType.IsGenericType &&
            memberInfo.DeclaringType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
        {
            if (memberInfo.MemberType == MemberTypes.Constructor)
            {
                ConstructorInfo constructorInfo = memberInfo as ConstructorInfo;
                var parameterInfos = constructorInfo.GetParameters();
                if (parameterInfos.Length > 0)
                {
                    if (typeof(System.Collections.IEnumerable).IsAssignableFrom(parameterInfos[0].ParameterType))
                    {
                        return true;
                    }
                }
            }
            else if (memberInfo.MemberType == MemberTypes.Method)
            {
                var methodInfo = memberInfo as MethodInfo;
                if (methodInfo.Name == "TryAdd" ||
                    methodInfo.Name == "Remove" && methodInfo.GetParameters().Length == 2)
                {
                    return true;
                }
            }
        }

        return false;
    };
#endif
}
