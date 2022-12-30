using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaFramework;

public class AppFacade : Facade
{
    private static AppFacade _instance;

    public AppFacade() : base()
    {
    }

    public static AppFacade Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppFacade();
            }
            return _instance;
        }
    }

    override protected void InitFramework()
    {
        base.InitFramework();
    }

    /// <summary>
    /// 启动框架
    /// </summary>
    public void StartUp()
    {
  

        //-----------------关联命令-----------------------
        RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

        //-----------------初始化管理器-----------------------
        AddManager<LuaManager>(ManagerName.Lua);
        AddManager<NetworkManager>(ManagerName.Network);
        AddManager<GameManager>(ManagerName.Game);
    }
}


