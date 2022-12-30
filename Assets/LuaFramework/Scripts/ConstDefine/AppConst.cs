using UnityEngine;

namespace LuaFramework
{
    public class AppConst
    {
#if UNITY_EDITOR
        // Lua代码AssetBundle模式
        public const bool LuaBundleMode = false;
#else
        public const bool LuaBundleMode = true;                   
#endif
        // 游戏帧频
        public const int GameFrameRate = 30;


        // 素材目录 
        public const string AssetDir = "StreamingAssets";

        // Web服务器地址，用于热更
#if UNITY_EDITOR
        public const string WebUrl = "http://localhost:80/";
        // public const string WebUrl = "http://1.14.208.107:80/";
#else
        public const string WebUrl = "http://1.14.208.107:80/";
#endif   

        // 游戏服务器IP
        public const string SocketAddress = "";
        // 游戏服务器端口
        public const int SocketPort = 0;

        public static string FrameworkRoot
        {
            get
            {
                return Application.dataPath + "/LuaFramework";
            }
        }
    }
}