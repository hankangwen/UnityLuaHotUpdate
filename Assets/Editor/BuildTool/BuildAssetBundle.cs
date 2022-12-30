using UnityEngine;
using UnityEditor;


public class BuildAssetBundle
{
    public static void Build()
    {
        string targetPath = Application.streamingAssetsPath + "/res";
        BuildUtils.BuildLuaBundle(targetPath);
        BuildUtils.BuildNormalCfgBundle(targetPath);
        BuildUtils.BuildGameResBundle(targetPath);
        GameLogger.Log("BuildAssetBundle Done");
    }
}
