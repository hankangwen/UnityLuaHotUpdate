using UnityEngine;
using System.Collections;
using LuaInterface;

namespace LuaFramework {
    public class LuaManager : Manager {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;

        // Use this for initialization
        void Awake() {
            loader = new LuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
        }

        public void InitStart() {
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
            this.StartMain();
            this.StartLooper();
        }

        void StartLooper() {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson() {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        void StartMain() {
#if UNITY_EDITOR
            OverrideLuaPrint();
            ConnectEmmyLua();
            // ListenEmmyLua();
#endif
            
            lua.DoFile("Main.lua");

            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;
        }

#if UNITY_EDITOR
        private void OverrideLuaPrint()
        {
            //如果移动了ToLua目录，自己手动修复吧，只是例子就不做配置了
            string fullPath = Application.dataPath + "\\Editor/Custom/LuaLogRedirect";
            lua.AddSearchPath(fullPath);     
            
            string str =
                @"
                    pcall(function print(...)
                        
                    end)                              
                ";
            lua.DoString(str, "LuaManager.cs");
        }
        
        public void ConnectEmmyLua()
        {
            string str =
                @"
                    pcall(function()
                        package.cpath = package.cpath .. ';' .. UnityEngine.Application.dataPath .. '/../OtherTools/Emmylua/emmy_core.dll'
                        local dbg = require('emmy_core')
	                    local value = dbg.tcpConnect('localhost', 9966)
	                    print(string.format('Connect EmmyLua=%s', value))
                    end)                              
                ";
            lua.DoString(str, "LuaManager.cs");
        } 
        
        public void ListenEmmyLua()
        {
            string str =
                @"          
                    pcall(function()
                        package.cpath = package.cpath .. ';' .. UnityEngine.Application.dataPath .. '/../OtherTools/Emmylua/emmy_core.dll'
                        local dbg = require('emmy_core')
	                    local value = dbg.tcpListen('localhost', 9967)
	                    print(string.format('Listen EmmyLua=%s', value))
                    end)                               
                ";
            lua.DoString(str, "LuaManager.cs");
        }
#endif
        
        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs() {
            lua.OpenLibs(LuaDLL.luaopen_pb);      
            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            this.OpenCJson();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath()
        {
            string rootPath = AppConst.FrameworkRoot;
            lua.AddSearchPath(rootPath + "/Lua");
            lua.AddSearchPath(rootPath + "/ToLua/Lua");
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle() {
            if (loader.beZip) {
                loader.AddBundle(ResourceMgr.instance.GetAssetBundle("lua_update.bundle"));
                loader.AddBundle(ResourceMgr.instance.GetAssetBundle("lua.bundle"));
            }
        }

        public void DoFile(string filename) {
            lua.DoFile(filename);
        }

        // Update is called once per frame
        public object[] CallFunction(string funcName, params object[] args) {
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                return func.LazyCall(args);
            }
            return null;
        }

        
        public LuaFunction GetFunction(string funcName, bool logMiss = true)
        {
            return lua.GetFunction(funcName, logMiss);
        }

        public void LuaGC() {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() {
            loop.Destroy();
            loop = null;

            lua.Dispose();
            lua = null;
            loader = null;
        }
    }
}