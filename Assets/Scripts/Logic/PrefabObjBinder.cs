using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using System;
using UnityEngine.UI;

public class PrefabObjBinder : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public UObject obj;
    }

    public Item[] items = new Item[0];

    private Dictionary<string, Item> m_itemsDic = new Dictionary<string, Item>();

    public UObject GetObj(string name)
    {
        if (0 == m_itemsDic.Count)
        {
            for (int i = 0, len = items.Length; i < len; ++i)
            {
                m_itemsDic[items[i].name] = items[i];
            }
        }
        if (m_itemsDic.ContainsKey(name))
        {
            return m_itemsDic[name].obj;
        }
        return null;
    }

    public T GetObj<T>(string name) where T : UObject
    {
        return GetObj(name) as T;
    }

    public Button SetBtnClick(string name, Action cb)
    {
        var obj = GetObj(name);
        if (null == obj)
        {
            Debug.LogError("SetClick Error, null == obj, name:" + name);
            return null;
        }
        var btn = obj as Button;
        btn.onClick.AddListener(() =>
        {
            cb?.Invoke();
        });
        return btn;
    }

    public Text SetText(string name, string text)
    {
        var obj = GetObj(name);
        if (null == obj)
        {
            Debug.LogError("SetText Error, null == obj, name:" + name);
            return null;
        }
        var txt = obj as Text;
        txt.text = text;
        return txt;
    }

    public Toggle SetToggle(string name, Action<bool> cb)
    {
        var obj = GetObj(name);
        if (null == obj)
        {
            Debug.LogError("SetToggle Error, null == obj, name:" + name);
            return null;
        }
        var tgl = obj as Toggle;
        tgl.onValueChanged.AddListener((v) =>
        {
            cb?.Invoke(v);
        });
        return tgl;
    }
}
