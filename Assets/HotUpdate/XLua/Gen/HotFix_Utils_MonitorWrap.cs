#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class HotFixUtilsMonitorWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(HotFix.Utils.Monitor);
			Utils.BeginObjectRegister(type, L, translator, 0, 9, 0, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddBatteryMon", _m_AddBatteryMon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopBatteryMon", _m_StopBatteryMon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveBatteryMon", _m_RemoveBatteryMon);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopPing", _m_StopPing);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddPing", _m_AddPing);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemovePing", _m_RemovePing);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddTrackNetwork", _m_AddTrackNetwork);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveTrackNetwork", _m_RemoveTrackNetwork);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "StopTrackNetwork", _m_StopTrackNetwork);
			
			
			
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 1, 1);
			
			
            
			Utils.RegisterFunc(L, Utils.CLS_GETTER_IDX, "PingWatchIP", _g_get_PingWatchIP);
            
			Utils.RegisterFunc(L, Utils.CLS_SETTER_IDX, "PingWatchIP", _s_set_PingWatchIP);
            
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					var gen_ret = new HotFix.Utils.Monitor();
					translator.Push(L, gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to HotFix.Utils.Monitor constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddBatteryMon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<UnityEngine.BatteryStatus, float> _callback = translator.GetDelegate<System.Action<UnityEngine.BatteryStatus, float>>(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.AddBatteryMon( _callback );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopBatteryMon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopBatteryMon(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveBatteryMon(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ulong _id = LuaAPI.lua_touint64(L, 2);
                    
                    gen_to_be_invoked.RemoveBatteryMon( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopPing(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopPing(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddPing(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<int> _callback = translator.GetDelegate<System.Action<int>>(L, 2);
                    System.Action _timeout = translator.GetDelegate<System.Action>(L, 3);
                    
                        var gen_ret = gen_to_be_invoked.AddPing( _callback, _timeout );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemovePing(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ulong _id = LuaAPI.lua_touint64(L, 2);
                    
                    gen_to_be_invoked.RemovePing( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddTrackNetwork(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<UnityEngine.NetworkReachability> _curStatus = translator.GetDelegate<System.Action<UnityEngine.NetworkReachability>>(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.AddTrackNetwork( _curStatus );
                        LuaAPI.lua_pushuint64(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveTrackNetwork(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    ulong _id = LuaAPI.lua_touint64(L, 2);
                    
                    gen_to_be_invoked.RemoveTrackNetwork( _id );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_StopTrackNetwork(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                HotFix.Utils.Monitor gen_to_be_invoked = (HotFix.Utils.Monitor)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.StopTrackNetwork(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PingWatchIP(RealStatePtr L)
        {
		    try {
            
			    LuaAPI.lua_pushstring(L, HotFix.Utils.Monitor.PingWatchIP);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_PingWatchIP(RealStatePtr L)
        {
		    try {
                
			    HotFix.Utils.Monitor.PingWatchIP = LuaAPI.lua_tostring(L, 1);
            
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
