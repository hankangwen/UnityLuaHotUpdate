//using UnityEngine;
//using System.Collections;
//using LuaFramework;

//public class StartUpCommand : ControllerCommand {

//    public override void Execute(IMessage message) {

//        LuaInterface.LuaFileUtils.Instance.Init();

//        //-----------------关联命令-----------------------
//        AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));

//        //-----------------初始化管理器-----------------------
//        AppFacade.Instance.AddManager<LuaManager>(ManagerName.Lua);
//        AppFacade.Instance.AddManager<NetworkManager>(ManagerName.Network);
//        AppFacade.Instance.AddManager<GameManager>(ManagerName.Game);
//    }
//}