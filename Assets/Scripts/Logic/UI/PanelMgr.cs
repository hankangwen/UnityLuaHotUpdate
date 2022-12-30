using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PanelMgr
{
    public void Init()
    {
        m_rootCanvas = GameObject.Find("Canvas").transform;
    }

    public BasePanel ShowPanel(string panelName, int resId)
    {
        if (null != panelName && m_panels.ContainsKey(panelName))
            return m_panels[panelName];

        var resCfg = ResourcesCfg.instance.GetResCfg(resId);
        if(null != resCfg)
        {
            return ShowPanel<BasePanel>(panelName, resCfg.editor_path);
        }
        else
        {
            Debug.LogError("ShowPanel Error, null == resCfg, resId: " + resId);
            return null;
        }
    }

    public T ShowPanel<T>(string panelName, string resPath) where T :BasePanel
    {
        if (null != panelName && m_panels.ContainsKey(panelName))
            return m_panels[panelName] as T;
        var prefab = ResourceMgr.instance.LoadAsset<GameObject>(resPath);
        var go = Object.Instantiate(prefab);
        go.transform.SetParent(m_rootCanvas, false);
        var panel = go.GetOrAddComponent<T>();
        panel.Initialize(panelName, go);
        if (null != panelName)
            m_panels.Add(panelName, panel);
        panel.Show();
        return panel;
    }

    public GameObject InstantiateUI(int resId)
    {
        var resCfg = ResourcesCfg.instance.GetResCfg(resId);
        if (null == resCfg)
        {
            return null;
        }
        var prefab = ResourceMgr.instance.LoadAsset<GameObject>(resCfg.editor_path);
        if (null == prefab)
            return null;
        var uiObj = Object.Instantiate(prefab);
        uiObj.transform.SetParent(m_rootCanvas, false);
        return uiObj;
    }

    public void HidePanel(string panelName)
    {
        if (m_panels.ContainsKey(panelName))
        {
            m_panels[panelName].Hide();
            m_panels.Remove(panelName);
        }
    }

    public void HideAllPanels()
    {
        foreach(var panel in m_panels.Values)
        {
            panel.Hide();
        }
        m_panels.Clear();
    }

    private Transform m_rootCanvas;

    private Dictionary<string, BasePanel> m_panels = new Dictionary<string, BasePanel>();

    private static PanelMgr s_instance;
    public static PanelMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new PanelMgr();
            return s_instance;
        }
    }
}
