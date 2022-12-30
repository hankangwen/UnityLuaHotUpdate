using System.Reflection;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor.Callbacks;
using UnityEngine;

public class LuaLogRedirect
{
    private static Type consoleWindowType;
    private static EditorWindow consoleWindow;
    private static int openInstanceID;
    private static int openLine;

    private static bool GetConsoleWindow()
    {
        if (consoleWindow == null)
        {
            Assembly unityEditorAssembly = Assembly.GetAssembly(typeof(EditorWindow));
            consoleWindowType = unityEditorAssembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo =
                consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            consoleWindow = fieldInfo.GetValue(null) as EditorWindow;

            if (consoleWindow == null)
                return false;
        }

        return true;
    }

    static string GetLogText()
    {
        // 如果console窗口时焦点窗口的话，获取stacktrace
        if ((object) EditorWindow.focusedWindow == consoleWindow)
        {
            // 找到类UnityEditor.ConsoleWindow中的成员m_ActiveText
            var text = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            if (text == null) return null;

            // 获得m_ActiveText的值，就是我们需要的stacktrace
            string value_active_text = text.GetValue(consoleWindow).ToString();
            // string result = LogTextFilter(value_active_text);
            string result = value_active_text;
            return result;
        }

        return null;
    }

    //过滤项目中自定义的输出层，如LogExtension_Client.lua等
    static string LogTextFilter(string value)
    {
        string result = value;
        string[] arr = value.Split(Environment.NewLine.ToCharArray());
        foreach (var item in arr)
        {
            if (item.Contains(".lua:") && !item.Contains("LogExtension_Client.lua:"))
            {
                result = item;
                break;
            }
        }

        return result;
    }

    static bool CheckLuaDebugFile(string curPathName)
    {
        if (curPathName.Contains("Debuger.cs") || curPathName.Contains("ToLua.cs"))
            return true;
        return false;
    }

    [OnOpenAssetAttribute(0)]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (!EditorWindow.focusedWindow.titleContent.text.Equals("Console")) //只对控制台的开启进行重定向
            return false;

        var curPathName = AssetDatabase.GetAssetPath(instanceID);
        if (curPathName.Contains("LuaLogRedirect.cs"))
            return OpenCustomLink(line);

        if (!CheckLuaDebugFile(curPathName)) //检查是不是要跳转到lua
            return false;

        if (openInstanceID == instanceID && openLine == line)
        {
            openInstanceID = -1;
            openLine = -1;
            return false;
        }

        if (!GetConsoleWindow())
            return false;

        return OpenLuaFile(GetLogText(), 0);
    }

    static bool OpenCustomLink(int line)
    {
        if (!GetConsoleWindow())
            return false;

        return OpenLuaFile(GetLogText(), line);
    }

    static bool OpenLuaFile(string logText, int line = 0)
    {
        string[] strArray = logText.Split(new string[1]
        {
            "\n"
        }, StringSplitOptions.None);
        var log = strArray[line];
        string value = String.Empty;
        if (line == 0)
        {
            var start = log.IndexOf('[') + 1;
            var end = log.IndexOf(']');
            
            CString sb = CString.Alloc(256);
            sb.Append(log, start, end - start);
            value = sb.ToString();
        }
        else
        {
            var pre = ">" + Application.dataPath;
            var after = "</a>";
            Regex regex = new Regex(pre + @"/.*.lua:.*" + after);
            Match match = regex.Match(log);
            value = match.Groups[0].Value.Trim();
            if (!value.Contains(".lua"))
                return false;

            value = value.Replace(pre, "");
            value = value.Replace(after, "");
            value = "Assets" + value;
        }

        var strs = value.Split(':');
        if (strs.Length >= 2)
        {
            var filePath = ConvertToRealFilePath(strs[0]);
            var lineNumber = int.Parse(strs[1].Split(']')[0]);

            strs = strs[0].Split('/');
            string fileName = strs[strs.Length - 1];

            if (fileName.Contains(".lua"))
            {
                var assetObj = AssetDatabase.LoadAssetAtPath<DefaultAsset>(filePath);
                openInstanceID = assetObj.GetInstanceID();
                openLine = lineNumber;
                AssetDatabase.OpenAsset(assetObj, lineNumber);
                return true;
            }
        }

        return false;
    }

    private static string ConvertToRealFilePath(string filePath)
    {
        string path = LuaConst.luaDir + "/" + filePath;
        if(!File.Exists(path))
            path = LuaConst.toluaDir + "/" + filePath;
        return path.Substring(path.IndexOf("Assets"));
    }
}