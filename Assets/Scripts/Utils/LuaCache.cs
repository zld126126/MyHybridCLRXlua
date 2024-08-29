using GameMain.Utils;
using DictLuaType = System.Collections.Generic.Dictionary<string, byte[]>;

namespace HotFix.Utils
{
    [XLua.BlackList]
    public static class LuaCache
    {
        private static readonly DictLuaType DictCustomCache = new DictLuaType(1024);

        public static bool ContainsKey(string strLuaName)
        {
            return DictCustomCache.ContainsKey(strLuaName);
        }

        public static byte[] Get(string strLuaName)
        {
            if (DictCustomCache.TryGetValue(strLuaName, out var code) && code != null && code.Length > 0)
            {
                return code;
            }

            return null;
        }

        public static void Set(string strLuaName, byte[] code)
        {
            if (DictCustomCache.ContainsKey(strLuaName))
            {
                SimpleLog.Log($"[LuaCache::Set] {strLuaName} already exists!!!!");
            }

            DictCustomCache[strLuaName] = code;
        }

        public static void Clear()
        {
            DictCustomCache.Clear();
        }

        public static byte[] LoadLua(ref string fileName)
        {
            if (!fileName.EndsWith(".lua.txt"))
            {
                fileName = fileName + ".lua.txt";
            }

            return Get(fileName);
        }
    }
}
