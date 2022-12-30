using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;
using System.IO;

public class ResourceMgr
{
    /// <summary>
    /// 预加载基础的Bundle资源
    /// </summary>
    public void PreloadBaseBundle()
    {
        LoadAssetBundle("baseres.bundle");
    }

    /// <summary>
    /// 预加载lua的AssetBundle
    /// </summary>
    public void PreloadLuaBundles()
    {
        if (File.Exists(updatePath + "/lua_update.bundle"))
            LoadAssetBundle("lua_update.bundle");
        LoadAssetBundle("lua.bundle");
    }

    public T LoadAsset<T>(int resId) where T : UObject
    {
        var resCfg = ResourcesCfg.instance.GetResCfg(resId);
        return LoadAsset<T>(resCfg.editor_path);
    }

    public T LoadAsset<T>(string resPath) where T : UObject
    {
        if (m_assets.ContainsKey(resPath))
            return m_assets[resPath] as T;

        T obj = null;
#if UNITY_EDITOR
        obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>("Assets/GameRes/" + resPath);
#else
        // resources.bytes的第一级目录为AssetBundle
        var abName = resPath.Substring(0, resPath.IndexOf("/")).ToLower() + ".bundle";
        var fname = Path.GetFileName(resPath);
        AssetBundle ab = null;
        if(File.Exists(updatePath + "/" + fname))
        {
            // 热更的资源，是一个独立的AssetBundle文件，以fname为文件名
            ab = LoadAssetBundle(fname);
        }
        else
        {
            ab = LoadAssetBundle(abName);
        }
        if (null != ab)
        {
            var assetName = fname.Substring(0, fname.IndexOf("."));
            obj = ab.LoadAsset<T>(assetName);
        }  
#endif

        if (null != obj)
            m_assets[resPath] = obj;
        return obj;
    }

    private AssetBundle LoadAssetBundle(string abName)
    {
        if (m_bundles.ContainsKey(abName))
            return m_bundles[abName];


        AssetBundle bundle = null;
        if (File.Exists(updatePath + "/" + abName))
        {
            // 优先从update目录（热更新目录）中查找资源
            bundle = AssetBundle.LoadFromFile(updatePath + "/" + abName);
        }
        else
        {
            bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/res/" + abName);
        }

        /*
        // 如果对AssetBundle做了加密处理，则需要使用流式读取，进行解密后再通过AssetBundle.LoadFromMemory加载AssetBundle
        byte[] stream = null;
        stream = File.ReadAllBytes(path + "/" + abName);
        // TOOD 对stream做解密

        var bundle = AssetBundle.LoadFromMemory(stream); 
        */

        if (null != bundle)
        {
            m_bundles[abName] = bundle;
        }
        return bundle;
    }



    public string LoadCfgFile(string cfgFileName)
    {
        TextAsset asset = null;
#if UNITY_EDITOR
        asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/GameRes/Config/" + cfgFileName);
#else
        var bundle = LoadAssetBundle("normal_cfg.bundle");
        if (null != bundle)
            asset = bundle.LoadAsset<TextAsset>(cfgFileName);
#endif
        if (null != asset)
        {
            return asset.text;
        }
        else
        {
            Debug.LogError("AssetBundleMgr.LoadCfgFile Error, null == asset, cfgFileName: " + cfgFileName);
            return null;
        }
    }

    public AssetBundle GetAssetBundle(string abName)
    {
        if (m_bundles.ContainsKey(abName))
        {
            return m_bundles[abName];
        }
        Debug.LogError("GetAssetBundle Error, not contains: " + abName);
        return null;
    }

    private Dictionary<string, AssetBundle> m_bundles = new Dictionary<string, AssetBundle>();
    private Dictionary<string, UObject> m_assets = new Dictionary<string, UObject>();

    public string updatePath
    {
        get
        {
            return Application.persistentDataPath + "/update/";
        }
    }

    private static ResourceMgr s_instance;
    public static ResourceMgr instance
    {
        get
        {
            if (null == s_instance)
                s_instance = new ResourceMgr();
            return s_instance;
        }
    }
}
