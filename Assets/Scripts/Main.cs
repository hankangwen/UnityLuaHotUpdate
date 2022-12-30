using UnityEngine;

/// <summary>
/// 游戏入口脚本
/// </summary>
public class Main : MonoBehaviour
{

    private void Awake()
    {
        // 初始化一些必要的管理器
        GameLogger.Init();
        VersionMgr.instance.Init();
        PanelMgr.instance.Init();

        // 预加载基础的Bundle
        ResourceMgr.instance.PreloadBaseBundle();

        // 显示更新界面
        UpdatePanel.Create(()=> 
        {
            // 更新完毕
            AfterHotUpdate();
        });
    }

    /// <summary>
    /// 热更新后
    /// </summary>
    private void AfterHotUpdate()
    {
        Debug.Log("AfterHotUpdate");

        ResourcesCfg.instance.LoadCfg();
        // 预加载Lua的AssetBundle
        ResourceMgr.instance.PreloadLuaBundles();

        // 启动lua框架
        AppFacade.Instance.StartUp();
    }
}