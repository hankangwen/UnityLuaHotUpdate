using UnityEngine;
using LuaFramework;
using LuaInterface;

public class BasePanel : MonoBehaviour
{
    private LuaFunction onShow;
    private LuaFunction onHide;
    private LuaFunction onUpdate;
    private LuaFunction onRegistEvent;
    private LuaFunction onUnRegistEvet;
    private string panelName;
    public GameObject panelObj { get; private set; }

    public void Initialize(string panelName, GameObject panelObj)
    {
        this.panelName = panelName;
        this.panelObj = panelObj;
        LuaManager luaMgr = LuaHelper.GetLuaManager();
        if (null != luaMgr)
        {
            onShow = luaMgr.GetFunction(panelName + ".OnShow", false);
            onHide = luaMgr.GetFunction(panelName + ".OnHide", false);
            onUpdate = luaMgr.GetFunction(panelName + ".OnUpdate", false);
        }

    }

    public void Show()
    {
        if (null != onShow)
        {
            onShow.Call(panelObj);
        }
    }

    public void Hide()
    {
        Destroy(this.panelObj);
        Util.ClearMemory();
        if (null != onHide)
            onHide.Call();
    }

    private void Update()
    {
        if (null != onUpdate)
            onUpdate.Call();
    }
}
