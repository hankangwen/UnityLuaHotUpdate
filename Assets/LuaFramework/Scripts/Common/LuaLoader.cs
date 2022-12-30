using UnityEngine;
using System.Collections.Generic;
using System.IO;
using LuaInterface;

namespace LuaFramework
{
    /// <summary>
    /// 集成自LuaFileUtils，重写里面的ReadFile，
    /// </summary>
    public class LuaLoader : LuaFileUtils
    {
        public LuaLoader()
        {
            instance = this;
            beZip = AppConst.LuaBundleMode;
        }

        /// <summary>
        /// 添加打入Lua代码的AssetBundle
        /// </summary>
        public void AddBundle(AssetBundle bundle)
        {
            if (null != bundle)
            {
                luaBundleList.Add(bundle);
                GameLogger.Log("LuaLoader.AddBundle: " + bundle.name);
            }
        }

        /// <summary>
        /// 当LuaVM加载Lua文件的时候，这里就会被调用，
        /// 用户可以自定义加载行为，只要返回byte[]即可。
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override byte[] ReadFile(string fileName)
        {
            if (!beZip)
            {
                string path = FindFile(fileName);
                byte[] str = null;

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
#if !UNITY_WEBPLAYER
                    str = File.ReadAllBytes(path);
#else
                    throw new LuaException("can't run in web platform, please switch to other platform");
#endif
                }

                return str;
            }
            else
            {
                return ReadBytesFromAssetBundle(fileName);
            }
        }


        /// <summary>
        /// 从AssetBundle读取lua代码
        /// </summary>
        /// <param name="fileName">lua脚本名</param>
        /// <returns></returns>
        private byte[] ReadBytesFromAssetBundle(string fileName)
        {
            fileName = "assets/luabundle/" + fileName;

            TextAsset luaCode = null;
            for (int i = 0, cnt = luaBundleList.Count; i < cnt; ++i)
            {
                var bundle = luaBundleList[i];
                luaCode = bundle.LoadAsset<TextAsset>(fileName + ".bytes");
                if (null != luaCode)
                    break;
                else
                {
                    // require过来的没有.lua后缀
                    var extension = Path.GetExtension(fileName);
                    if (string.IsNullOrEmpty(extension))
                    {
                        luaCode = bundle.LoadAsset<TextAsset>(fileName + ".lua.bytes");
                        if (null != luaCode)
                            break;
                    }
                }
            }
            if (null != luaCode)
            {
                // 因为打Lua AssetBundle之前对lua脚本做了加密，所以这里需要进行解密
                var butes = AESEncrypt.Decrypt(luaCode.bytes);
                return butes;
            }
            else
            {
                Debug.LogError("LuaFileUtils.ReadBytesFromAssetBundle Error, null == luaCode, fileName:" + fileName);
                return null;
            }
        }

        private List<AssetBundle> luaBundleList = new List<AssetBundle>();
    }
}