using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XSJGame.Finder.Editor
{
    public class ScriptReferences : EditorWindow
    {
        private string _logMsg;
        private Vector2 _scrollPos3;
        private static Object _target;
        private List<Object> _uiPrefabList = new List<Object>();
        private List<string> _childNameList = new List<string>();
        private static string _uiPrefabPath = @"Assets/Package/UI";
        
        // [MenuItem("Assets/输出脚本挂在了哪些预制上")]
        // static void ShowMyEditorWindow()
        // {
        //     _target = Selection.GetFiltered(typeof(Object), SelectionMode.Assets)[0];
        //     GetWindow<ScriptReferences>("查找脚本引用").Show();
        // }

        void OnEnable()
        {
            _target = Selection.GetFiltered(typeof(Object), SelectionMode.Assets)[0];
        }
        
        private void OnGUI()
        {
            _uiPrefabPath = EditorGUILayout.TextField("UI预制路径", _uiPrefabPath);
            _target = EditorGUILayout.ObjectField("脚本", _target, typeof(Object), false);
            if (GUILayout.Button("执行"))
            {
                GetFileReference();
            }
            GUILayout.Label("提示输出", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(_logMsg);
            
            EditorGUILayout.BeginVertical();
            _scrollPos3 = EditorGUILayout.BeginScrollView(_scrollPos3);
            GUILayout.Label("对应的UI预制", EditorStyles.boldLabel);
            for (int i = 0; i < _uiPrefabList.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.ObjectField("", _uiPrefabList[i], typeof(Object), false, GUILayout.MaxWidth(200));
                EditorGUILayout.TextArea(_childNameList[i]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
        
        private void GetFileReference()
        {
            _uiPrefabList = new List<Object>();
            _childNameList = new List<string>();
            string target = "";
            if (Selection.activeObject != null)
            {
                target = AssetDatabase.GetAssetPath(Selection.activeObject);
            }
            if (string.IsNullOrEmpty(target))
            {
                return;
            }
            string[] files = Directory.GetFiles(_uiPrefabPath, "*.prefab", SearchOption.AllDirectories);
            var curType = typen(_target.name);
            for (int i = 0; i < files.Length; i++)
            {
                string[] source = AssetDatabase.GetDependencies(new string[]
                    {files[i].Replace(Application.dataPath, "Assets")});
                for (int j = 0; j < source.Length; j++)
                {
                    if (source[j] == target)
                    {
                        var item = AssetDatabase.LoadMainAssetAtPath(files[i].Replace(Application.dataPath, "Assets"));
                        _uiPrefabList.Add(item);
                        GameObject go = AssetDatabase.LoadAssetAtPath(files[i], typeof(System.Object)) as GameObject;
                        if (go != null)
                        {
                            string childName = "";
                            if (go.GetComponent(curType) != null)
                                childName = childName + go.name + " |"; 
                            GetChildName(go.transform, curType, ref childName);
                            _childNameList.Add(childName);
                        }
                    }
                }
            }
            _logMsg = "脚本被引用次数：" + _uiPrefabList.Count.ToString();
        }

        private void GetChildName(Transform go, Type type, ref string childName)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                var _child = go.transform.GetChild(i); 
                if (_child.GetComponent(type) != null)
                    childName = childName + _child.name + " |";

                if (_child.childCount != 0)
                    GetChildName(_child, type, ref childName);
            }
        }

        private Type typen(string typeName)
        {
            Type type = null;
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();
            int assemblyArrayLength = assemblyArray.Length;
            for (int i = 0; i < assemblyArrayLength; ++i)
            {
                type = assemblyArray[i].GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            for (int i = 0; (i < assemblyArrayLength); ++i)
            {
                Type[] typeArray = assemblyArray[i].GetTypes();
                int typeArrayLength = typeArray.Length;
                for (int j = 0; j < typeArrayLength; ++j)
                {
                    if (typeArray[j].Name.Equals(typeName))
                    {
                        return typeArray[j];
                    }
                }
            }

            return type;
        }
    }
}