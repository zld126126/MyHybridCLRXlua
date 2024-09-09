using System.Runtime.InteropServices;

namespace XLua.LuaDLL
{
    public partial class Lua
    {
        public static int luaopen_rapidjson(System.IntPtr L)
        {
            return XLuaBase.Lua.luaopen_rapidjson(L);
        }

        public static int luaopen_md5(System.IntPtr L)
        {
            return XLuaBase.Lua.luaopen_md5(L);
        }

        [MonoPInvokeCallback(typeof(XLuaBase.lua_CSFunction))]
        public static int LoadRapidJson(System.IntPtr L)
        {
            return luaopen_rapidjson(L);
        }

        [MonoPInvokeCallback(typeof(XLuaBase.lua_CSFunction))]
        public static int LoadMD5(System.IntPtr L)
        {
            return luaopen_md5(L);
        }
    }
}
