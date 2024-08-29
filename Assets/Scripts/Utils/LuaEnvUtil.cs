using System;
using System.Text;
using UnityEngine;

using XLua;
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;

namespace HotFix.Utils
{
    public static class LuaEnvUtil
    {
        private static LuaEnv _luaEnv = null;
        private static LuaTable _scriptEnv = null;
        private static LuaEnv.CustomLoader _loader = null;

        [BlackList]
        public static void Reset(string commandLine, LuaEnv.CustomLoader loader)
        {
            Clear();

            _luaEnv = new LuaEnv();
#if UNITY_EDITOR
            _luaEnv.translator.debugDelegateBridgeRelease = true;
#endif

            _scriptEnv = _luaEnv.NewTable();

            var meta = _luaEnv.NewTable();
            meta.Set("__index", _luaEnv.Global);
            _scriptEnv.SetMetaTable(meta);
            meta.Dispose();
            _scriptEnv.Set("CommandLine", commandLine);

            _loader = loader;
            if (_loader != null)
            {
                _luaEnv.AddLoader(_loader);
            }

            customFunc = null;
        }

        public static LuaTable Global => _luaEnv?.Global;

        public static T Get<T>(string name)
        {
            T ret = default;
            if (_scriptEnv != null)
            {
                ret = _scriptEnv.Get<T>(name);
            }

            return ret;
        }

        public static void DoString(string code, string name = "chunk")
        {
            DoString(Encoding.UTF8.GetBytes(code), name);
        }

        public static void DoString(byte[] code, string name = "chunk", string encryptName = "")
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (code == null || code.Length == 0)
            {
                return;
            }

            _luaEnv.DoString(code, name, _scriptEnv);
        }

        public static string GetLuaStack()
        {
            var L = _luaEnv.L;
            var oldTop = LuaAPI.lua_gettop(L);

            var _ = LuaAPI.xlua_getglobal(L, "debug");
            LuaAPI.xlua_pushasciistring(L, "traceback");
            LuaAPI.xlua_pgettable(L, -2);
            var __ = LuaAPI.lua_pcall(L, 0, 1, 0);
            var luaStack = LuaAPI.lua_tostring(L, -1);
            LuaAPI.lua_pop(L, 1);
            LuaAPI.lua_settop(L, oldTop);

            return string.IsNullOrEmpty(luaStack) ? "" : luaStack;
        }

        [BlackList]
        public static void Clear()
        {
            _scriptEnv?.Dispose();
            _scriptEnv = null;

            _luaEnv?.RmeoveLoader(_loader);
            _luaEnv?.Dispose();
            _luaEnv = null;
        }

        static float GCInterval = 0.5f; //0.5 second 
        static float lastGCTime = 0;

        [BlackList]
        public static void Update()
        {
            if (Time.time - lastGCTime > GCInterval)
            {
                _luaEnv?.Tick();
                lastGCTime = Time.time;
            }
        }

        private static Action<string> customFunc = null;

        public static void RegisterCmd(Action<string> func)
        {
            customFunc += func;
        }

        public static void CallCmd(string cmd)
        {
            customFunc?.Invoke(cmd);
        }
    }
}
