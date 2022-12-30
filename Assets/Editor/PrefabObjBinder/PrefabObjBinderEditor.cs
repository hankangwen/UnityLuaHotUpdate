// PrefabObjBinder编辑器

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PrefabObjBinderEditor : EditorWindow
{
    private GameObject m_prefabObjBinderObj;
    private PrefabObjBinder m_binder;
    private List<PrefabObjBinder.Item> m_itemList;
    private List<PrefabObjBinder.Item> m_searchMatchItemList = new List<PrefabObjBinder.Item>();
    private Vector2 m_scrollViewPos;
    private List<Component> m_comList = new List<Component>();
    private string m_itemName;



    private string m_itemNameSearch;
    private string m_selectedItemName;
    private string m_lockBtnName;
    private Object m_itemObj;
    private bool m_lock;
    private string m_componentStr;
    enum ItemOption
    {
        AddItem,
        RemoveItem,
        ClearItems,
        SearchItems
    }

    private GUIStyle m_labelSytleYellow;
    private GUIStyle m_labelStyleNormal;


    public static void ShowWindow()
    {
        var window = GetWindow<PrefabObjBinderEditor>();
        window.titleContent = new GUIContent("预设对象绑定", AssetPreview.GetMiniTypeThumbnail(typeof(UnityEngine.EventSystems.EventSystem)), "decent");
        window.Init();
    }

    [MenuItem("GameObject/PrefabObjBinder Window", priority = 0)]
    public static void PrefabObjBinderWindow()
    {
        if (Selection.activeGameObject.GetComponent<PrefabObjBinder>())
            ShowWindow();
        else
            Debug.LogError("no PrefabObjBinder on this GameObject");
    }

    void Awake()
    {
        m_labelStyleNormal = new GUIStyle(EditorStyles.miniButton);
        m_labelStyleNormal.fontSize = 12;
        m_labelStyleNormal.normal.textColor = Color.white;

        m_labelSytleYellow = new GUIStyle(EditorStyles.miniButton);
        m_labelSytleYellow.fontSize = 12;
        m_labelSytleYellow.normal.textColor = Color.yellow;

    }

    void OnEnable()
    {
        EditorApplication.update += Repaint;
    }

    void OnDisable()
    {
        EditorApplication.update -= Repaint;
    }

    void Init()
    {
        m_itemList = new List<PrefabObjBinder.Item>();
        m_comList = new List<Component>();
        m_lockBtnName = "锁定item组件列表";
        m_componentStr = string.Empty;
        m_lock = false;
        if (Selection.activeGameObject.GetComponent<PrefabObjBinder>())
        {
            m_prefabObjBinderObj = Selection.activeGameObject;
            OnRefreshBtnClicked();
        }
    }

    void OnGUI()
    {
        BeginBox(new Rect(0, 0, 3 * Screen.width / 10f, Screen.height));
        DrawSearchBtn();
        DrawSearchItemList();
        EndBox();

        BeginBox(new Rect(3 * Screen.width / 10f, 0, 3 * Screen.width / 10f, Screen.height));
        DrawLockBtn();
        GUILayout.Space(2);
        DrawComponentList();
        EndBox();

        BeginBox(new Rect(6 * Screen.width / 10f, 0, 4 * Screen.width / 10f, Screen.height));
        DrawPrefabObjBinderField();
        GUILayout.Space(2);
        DrawItemField();
        EndBox();
    }

    private void DrawSearchBtn()
    {
        GUILayout.BeginHorizontal();
        string before = m_itemNameSearch;
        string after = EditorGUILayout.TextField("", before, "SearchTextField");
        if (before != after) m_itemNameSearch = after;

        if (GUILayout.Button("", "SearchCancelButton"))
        {
            m_itemNameSearch = "";
            GUIUtility.keyboardControl = 0;
        }
        ComponentOperation(m_binder, ItemOption.SearchItems, after);
        GUILayout.EndHorizontal();
    }

    private void DrawPrefabObjBinderField()
    {
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("prefab");
        var oldObj = m_prefabObjBinderObj;
        m_prefabObjBinderObj = EditorGUILayout.ObjectField(m_prefabObjBinderObj, typeof(GameObject), true) as GameObject;

        
        EditorGUILayout.EndHorizontal();
        if (!m_prefabObjBinderObj)
        {
            EditorGUILayout.HelpBox("Select a PrefabObjBinder Object", MessageType.Warning);
        }
        else if (oldObj != m_prefabObjBinderObj)
        {
            m_binder = m_prefabObjBinderObj.GetComponent<PrefabObjBinder>();
        }
    }

    private void BeginBox(Rect rect)
    {
        rect.height -= 20;
        GUILayout.BeginArea(rect);
        GUILayout.Box("", GUILayout.Width(rect.width), GUILayout.Height(rect.height));
        GUILayout.EndArea();
        GUILayout.BeginArea(rect);
    }

    private void EndBox()
    {
        GUILayout.EndArea();
    }

    private void DrawSearchItemList()
    {
        if (null == m_prefabObjBinderObj || null == m_binder)
            m_searchMatchItemList.Clear();
        m_scrollViewPos = EditorGUILayout.BeginScrollView(m_scrollViewPos);
        foreach (var item in m_searchMatchItemList)
        {
            GUILayout.BeginHorizontal();
            item.name = EditorGUILayout.TextField(item.name);
            item.obj = EditorGUILayout.ObjectField(item.obj, typeof(GameObject), true);
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                m_itemList.Remove(item);
                m_binder.items = m_itemList.ToArray();
                GUILayout.EndHorizontal();
                break;
            }
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawItemField()
    {
        EditorGUILayout.BeginVertical();

        GUILayout.Label(string.IsNullOrEmpty(m_componentStr) ? "null" : m_componentStr);
        m_itemName = EditorGUILayout.TextField(m_itemName);
        if (GUILayout.Button(new GUIContent("Add Item", "添加item"), GUILayout.Height(80)))
        {
            ComponentOperation(m_binder, ItemOption.AddItem);
        }
        if (GUILayout.Button(new GUIContent("Delete Item", "删除指定的item")))
        {
            if (m_prefabObjBinderObj != null)
            {
                if (string.IsNullOrEmpty(m_itemName))
                    Debug.LogWarning("请输入要删除的项目名称");
                else
                    ComponentOperation(m_binder, ItemOption.RemoveItem);
            }
        }
        if (GUILayout.Button(new GUIContent("Refresh", "刷新")))
        {
            OnRefreshBtnClicked();
        }
        ItemTip();
    }

    private void OnRefreshBtnClicked()
    {
        if (null != m_prefabObjBinderObj)
            m_binder = m_prefabObjBinderObj.GetComponent<PrefabObjBinder>();
        if (null == m_binder)
        {
            m_itemList.Clear();
            m_comList.Clear();
        }
    }

    private void DrawLockBtn()
    {
        if (GUILayout.Button(new GUIContent(m_lockBtnName, m_lockBtnName), EditorStyles.toolbarButton))
        {
            m_lock = !m_lock;
            if (m_lock == false)
                m_lockBtnName = "锁定item组件列表";
            else
                m_lockBtnName = "解锁item组件列表";
        }
    }

    private void DrawComponentList()
    {
        var go = Selection.activeObject as GameObject; //获取选中对象
        if (go && m_lock == false)
        {
            Component[] components = go.GetComponents<Component>();
            m_comList.Clear();
            m_comList.AddRange(components);
            m_selectedItemName = go.name;
        }

        if (go == null)
        {
            m_comList.Clear();
            m_selectedItemName = "无选中对象";
        }

        if (go && GUILayout.Button("GameObject", "GameObject" == m_componentStr ? m_labelSytleYellow : m_labelStyleNormal))
        {
            m_itemObj = go;
            m_componentStr = "GameObject";
        }

        foreach (var com in m_comList)
        {

            GUILayout.Space(2);
            var comType = com.GetType().ToString();
            comType = comType.Replace("UnityEngine.UI.", "");
            comType = comType.Replace("UnityEngine.", "");
            if (GUILayout.Button(comType, comType == m_componentStr ? m_labelSytleYellow : m_labelStyleNormal))
            {
                m_itemObj = com;
                m_componentStr = comType;
            }
        }

        EditorGUILayout.EndVertical();
    }

    #region private method
    private void ComponentOperation(PrefabObjBinder binder, ItemOption option, string name = " ")
    {
        if (null == binder) return;
        PrefabObjBinder.Item item = new PrefabObjBinder.Item();
        switch (option)
        {
            case ItemOption.AddItem:
                AddItem(item, binder);
                break;

            case ItemOption.RemoveItem:
                RemoveItem(item, binder);
                break;

            case ItemOption.ClearItems:
                ClearItem(item, binder);
                break;

            case ItemOption.SearchItems:
                SearchItem(item, binder, name);
                break;
        }
        binder.items = m_itemList.ToArray();
        // 这样enabled一下，才能触发预设的Override
        binder.enabled = false;
        binder.enabled = true;
    }

    private void AddItem(PrefabObjBinder.Item item, PrefabObjBinder binder)
    {
        item.name = m_itemName;
        item.obj = m_itemObj;
        m_itemList = binder.items.ToList();
        List<string> nameList = new List<string>();
        foreach (var obj in m_itemList)
        {
            nameList.Add(obj.name);
        }
        if (!string.IsNullOrEmpty(m_itemName) && m_itemObj != null)
        {
            if (nameList.Contains(m_itemName))
            {
                Debug.LogError("重复元素");
                m_itemList.Add(item);
            }
            else
                m_itemList.Add(item);
        }
 
    }

    private void RemoveItem(PrefabObjBinder.Item item, PrefabObjBinder Ps)
    {
        item.name = m_itemName;

        m_itemList = Ps.items.ToList();
        for (int i = 0; i < m_itemList.Count; i++)
        {
            if (m_itemList[i].name.ToLower() == item.name.ToLower())
            {
                m_itemList.Remove(m_itemList[i]);
                break;
            }
        }
    }

    private void ClearItem(PrefabObjBinder.Item item, PrefabObjBinder Ps)
    {
        item.name = m_itemName;
        item.obj = m_itemObj;
        m_itemList = Ps.items.ToList();

        for (int i = 0; i < m_itemList.Count; i++)
        {
            if (m_itemList[i].obj == null || string.IsNullOrEmpty(m_itemList[i].name))
            {
                m_itemList.Remove(m_itemList[i]);
            }
        }
    }

    private void SearchItem(PrefabObjBinder.Item item, PrefabObjBinder binder, string name)
    {
        m_itemList = binder.items.ToList();
        m_searchMatchItemList.Clear();

        foreach (var o in m_itemList)
        {
            if (string.IsNullOrEmpty(name))
            {
                m_searchMatchItemList.Add(o);
            }
            else
            {
                if (o.name.ToLower().Contains(name.ToLower()))
                {
                    m_searchMatchItemList.Add(o);
                }
                else if (null != o.obj)
                {
                    var objName = o.obj.name;
                    if (objName.ToLower().Contains(name.ToLower()))
                    {
                        m_searchMatchItemList.Add(o);
                    }
                }
            }
        }
    }

    private void ItemTip()
    {
        if (string.IsNullOrEmpty(m_itemName) || m_itemObj == null)
        {
            string msg = string.Empty;
            if (m_itemObj == null)
            {
                msg = "请选择项目组件";
            }
            else if (string.IsNullOrEmpty(m_itemName))
            {
                msg = "请输入要添加的项的名字";
            }

            EditorGUILayout.HelpBox(msg, MessageType.Warning);
            EditorGUILayout.Space();
        }
    }

    #endregion
}
