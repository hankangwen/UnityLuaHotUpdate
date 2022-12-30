using UnityEngine;
using System.Collections.Generic;
using LuaInterface;



namespace LuaFramework
{
    public class GameManager : Manager
    {
        protected static bool initialize = false;
        private List<string> downloadFiles = new List<string>();

        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Awake()
        {
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init()
        {
            DontDestroyOnLoad(gameObject);  //防止销毁自己

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;
            OnInitialize();
        }


        void OnInitialize()
        {
            LuaManager.InitStart();
            LuaManager.DoFile("Logic/Game");         //加载游戏
            LuaManager.DoFile("Logic/Network");      //加载网络
            NetManager.OnInit();                     //初始化网络
            Util.CallMethod("Game", "OnInitOK");     //初始化完成

            initialize = true;
        }


        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy()
        {
            if (NetManager != null)
            {
                NetManager.Unload();
            }
            if (LuaManager != null)
            {
                LuaManager.Close();
            }
            Debug.Log("~GameManager was destroyed");
        }
    }
}