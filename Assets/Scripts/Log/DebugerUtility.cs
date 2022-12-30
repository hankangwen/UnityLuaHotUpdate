#if UNITY_EDITOR
using System;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CustomTools
{
    public class DebugerUtility
    {
        static DebugerUtility _instance;

        public static DebugerUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DebugerUtility();

                return _instance;
            }
        }

        string luaLogRedirectPath = @"Assets/Editor/Custom/LuaLogRedirect/LuaLogRedirect.cs";
        Regex regex = new Regex(Application.dataPath + ".*\\.lua:[0-9]+");

        public string StacktraceWithHyperlinks(string stacktraceText)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] strArray = stacktraceText.Split(new[]
            {
                '\n'
            }, StringSplitOptions.None);
            stringBuilder.Append(strArray[0] + "\n");
            for (int index = 1; index < strArray.Length; ++index)
            {
                Match match = regex.Match(strArray[index]);
                if (match.Success)
                {
                    var num1 = match.Index;
                    var value = match.Groups[0].Value;
                    if (value.Contains(@": in function"))
                    {
                        var temp = value.IndexOf(@": in function", StringComparison.Ordinal);
                        value = value.Substring(0, temp);
                    }

                    stringBuilder.Append(strArray[index].Substring(0, num1));
                    stringBuilder.Append("<a href=\"" + luaLogRedirectPath + "\" line=\"" + index + "\">");
                    stringBuilder.Append(value + "</a>");
                    stringBuilder.Append(strArray[index].Substring(num1 + value.Length));
                    stringBuilder.Append("\n");
                    continue;
                }

                stringBuilder.Append(strArray[index] + "\n");
            }

            if (stringBuilder.Length > 0)
                stringBuilder.Remove(stringBuilder.Length - 1, 1);
            return stringBuilder.ToString();
        }
    }
}
#endif