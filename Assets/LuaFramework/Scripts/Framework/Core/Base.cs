using UnityEngine;
using System.Collections;
using LuaFramework;
using System.Collections.Generic;

public class Base : MonoBehaviour {
    private AppFacade m_Facade;
    private LuaManager m_LuaMgr;
  
    private NetworkManager m_NetMgr;
 

    /// <summary>
    /// 注册消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    protected void RegisterMessage(IView view, List<string> messages) {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RegisterViewCommand(view, messages.ToArray());
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    /// <param name="view"></param>
    /// <param name="messages"></param>
    protected void RemoveMessage(IView view, List<string> messages) {
        if (messages == null || messages.Count == 0) return;
        Controller.Instance.RemoveViewCommand(view, messages.ToArray());
    }

    protected AppFacade facade {
        get {
            if (m_Facade == null) {
                m_Facade = AppFacade.Instance;
            }
            return m_Facade;
        }
    }

    protected LuaManager LuaManager {
        get {
            if (m_LuaMgr == null) {
                m_LuaMgr = facade.GetManager<LuaManager>(ManagerName.Lua);
            }
            return m_LuaMgr;
        }
    }


    protected NetworkManager NetManager {
        get {
            if (m_NetMgr == null) {
                m_NetMgr = facade.GetManager<NetworkManager>(ManagerName.Network);
            }
            return m_NetMgr;
        }
    }
}
