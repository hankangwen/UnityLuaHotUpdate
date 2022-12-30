﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaInterface_LuaInjectionStationWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaInterface.LuaInjectionStation), typeof(System.Object));
		L.RegFunction("CacheInjectFunction", CacheInjectFunction);
		L.RegFunction("New", _CreateLuaInterface_LuaInjectionStation);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegConstant("NOT_INJECTION_FLAG", 0);
		L.RegConstant("INVALID_INJECTION_FLAG", 255);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateLuaInterface_LuaInjectionStation(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				LuaInterface.LuaInjectionStation obj = new LuaInterface.LuaInjectionStation();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: LuaInterface.LuaInjectionStation.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CacheInjectFunction(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 1);
			byte arg1 = (byte)LuaDLL.luaL_checknumber(L, 2);
			LuaFunction arg2 = ToLua.CheckLuaFunction(L, 3);
			LuaInterface.LuaInjectionStation.CacheInjectFunction(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

