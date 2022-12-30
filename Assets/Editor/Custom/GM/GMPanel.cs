using LuaFramework;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GMPanel : OdinEditorWindow
{
    [MenuItem("Window/GMPanel")]
    public static void ShowWin()
    {
        var win = GetWindow<GMPanel>();
        win.Show();
    }
    
    [Button("重连Rider")]
    [HideInEditorMode]
    [HorizontalGroup("ButtonGroup1")]
    [PropertyOrder(1)]
    void ReconnectRider()
    {
        AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua).ConnectEmmyLua();
    }
    
    [Button("启动Rider监听")]
    [HideInEditorMode]
    [HorizontalGroup("ButtonGroup1")]
    [PropertyOrder(2)]
    void StartRiderListen()
    {
        AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua).ListenEmmyLua();
    }
    
    [Button("打开Main场景")]
    [HorizontalGroup("ButtonGroup2")]
    [HideInPlayMode]
    [PropertyOrder(1)]
    void OpenMainCity()
    {
        string MainScene = "Assets/Scenes/Main.unity";
        EditorSceneManager.OpenScene(MainScene);
    }
}