using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ResourcesCfg
{
    public void LoadCfg()
    {
        var text = ResourceMgr.instance.LoadCfgFile("resources.bytes");
        var list = JsonMapper.ToObject<List<ResourcesCfgItem>>(text);
        for (int i = 0, cnt = list.Count; i < cnt; ++i)
        {
            var item = list[i];
            if(!m_resCfg.ContainsKey(item.id))
            {
                m_resCfg.Add(item.id, item);
            }
            else
            {
                Debug.LogError("resources.bytes 有资源id重复, id: " + item.id);
            }
        }
    }

    public ResourcesCfgItem GetResCfg(int resId)
    {
        if (m_resCfg.ContainsKey(resId))
            return m_resCfg[resId];
        GameLogger.LogError("ResourcesCfg.GetResCfg Error, resId: " + resId);
        return null;
    }

    private Dictionary<int, ResourcesCfgItem> m_resCfg = new Dictionary<int, ResourcesCfgItem>();


    private static ResourcesCfg s_instance;
    public static ResourcesCfg instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new ResourcesCfg();
            return s_instance;
        }
    }
}

public class ResourcesCfgItem
{
    public int id;
    public string editor_path;
}
