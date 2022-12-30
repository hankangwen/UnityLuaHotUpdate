using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XSJGame.Finder.Editor
{
    public class EffectTextureReferences : EditorWindow
    {
        private static Dictionary<string, int> Guid2Count = new Dictionary<string, int>();
        // private static EffectTextureReferences _myEditorWindow;
        private static Object _findFolder;

        private string _logMsg;
        private bool _groupEnabled;
        private bool _findEffPrefabs = true;
        private bool _findMaterials = true;
        private string _effectPrefabPath = @"Assets/Package/Prefabs/Effect";
        private string _matPath = @"Assets/RawResources/Effects";
        Vector2 scrollPos1;
        Vector2 scrollPos2;

        List<Object> prefabList = new List<Object>();
        List<Object> matsList = new List<Object>();
        
        EffectTextureReferences()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }
        
        // [MenuItem("Assets/输出文件夹内图片的信息(特效)", true)]
        // private static bool ValidateLogFolderTextureMsg()
        // {
        //     bool open = Selection.objects.Length == 1 && 
        //                 Selection.GetFiltered(typeof(Object), SelectionMode.Assets)[0].GetType().Name == "DefaultAsset";
        //     if (!open)
        //         open = Selection.activeObject?.GetType().Name == "Texture2D";
        //     return open;
        // }
        //
        // [MenuItem("Assets/输出文件夹内图片的信息(特效)")]
        // static void ShowMyEditorWindow()
        // {
        //     // if (_myEditorWindow != null)
        //     // {
        //     //     _myEditorWindow.Close();
        //     //     _myEditorWindow = null;
        //     // }
        //     
        //     GetWindow<EffectTextureReferences>("查找图片引用(特效)").Show();
        // }

        private void OnEnable()
        {
            Object[] arrs = Selection.GetFiltered(typeof(Object), SelectionMode.Assets);
            _findFolder = arrs[0];
        }

        private void OnGUI()
        {
            _groupEnabled = EditorGUILayout.BeginToggleGroup("配置路径", _groupEnabled);
            
            EditorGUILayout.BeginHorizontal();
            _findEffPrefabs = EditorGUILayout.Toggle("检索特效", _findEffPrefabs);
            _effectPrefabPath = EditorGUILayout.TextField("特效预制路径", _effectPrefabPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _findMaterials = EditorGUILayout.Toggle("检索材质球", _findMaterials);
            _matPath = EditorGUILayout.TextField("材质球路径", _matPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndToggleGroup();
            
            _findFolder = EditorGUILayout.ObjectField("目录或图片", _findFolder, typeof(Object), false);
            if (GUILayout.Button("执行"))
            {
                _logMsg = "";
                if (_findFolder == null)
                    _logMsg = "目录不能为空！！！";
                else
                {
                    LogTextureMsg(_findFolder);
                }
            }
            
            GUILayout.Label("错误输出", EditorStyles.boldLabel);
            EditorGUILayout.TextArea(_logMsg);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1);
            GUILayout.Label("对应的预制", EditorStyles.boldLabel);
            foreach (var item in prefabList)
            {
                EditorGUILayout.ObjectField("", item, typeof(Object), false);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
            GUILayout.Label("对应的材质球", EditorStyles.boldLabel);
            foreach (var item in matsList)
            {
                EditorGUILayout.ObjectField("", item, typeof(Object), false);
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        
        private void LogTextureMsg(Object _findFolder)
        {
            string path = AssetDatabase.GetAssetPath(_findFolder);
            List<Object> fileList = new List<Object>();
            prefabList = new List<Object>();
            matsList = new List<Object>();

            if (_findFolder.GetType().Name == "Texture2D")
            {
                if (_findEffPrefabs)
                {
                    string[] files = Directory.GetFiles(_effectPrefabPath, "*.prefab", SearchOption.AllDirectories);
                    FindReference(path, files, ref fileList, ref prefabList);
                }

                if (_findMaterials)
                {
                    string[] materials = Directory.GetFiles(_matPath, "*.mat", SearchOption.AllDirectories);
                    FindReference(path, materials, ref fileList, ref matsList, false);
                }
            }
            else
            {
                // string imgType = "*.PNG|*.JPG";
                // string[] imgTypes = imgType.Split('|');
                // for (int i = 0; i < imgTypes.Length; i++)
                // {
                //     string[] dirs = Directory.GetFiles(@path, imgTypes[i]);
                //     for (int j = 0; j < dirs.Length; j++)
                //     {
                //         string target = dirs[j].Replace(@"\", @"/");
                //         FindReference(target, files, ref fileList, ref prefabList);
                //         // if(_findEffPrefabs)
                //         //     FindReference(target, atlas, ref fileList, ref atlasList, false);
                //     }
                // }
            }
            
            Selection.objects = fileList.ToArray();
        }
        
        private void FindReference(string target, string[] files, ref List<Object> fileList, ref List<Object> references, bool isCalCount = true)
        {
            int count = 0;
            for (int i = 0; i < files.Length; i++)
            {
                string[] source = AssetDatabase.GetDependencies(new string[] { files[i].Replace(Application.dataPath, "Assets") });
                for (int j = 0; j < source.Length; j++)
                {
                    if (source[j] == target)
                    {
                        var item = AssetDatabase.LoadMainAssetAtPath(files[i].Replace(Application.dataPath, "Assets"));
                        if(!fileList.Contains(item))
                            fileList.Add(item);
                        if(!references.Contains(item))
                            references.Add(item);
                        count++;
                    }
                }
            }

            if (isCalCount)
            {
                if(count==0)
                    _logMsg += "该资源未被引用:" + target + "\n";
                var imgGuid = AssetDatabase.AssetPathToGUID(target);
                if (Guid2Count.ContainsKey(imgGuid))
                    Guid2Count.Remove(imgGuid);
                Guid2Count.Add(imgGuid, count);
            }
            else
            {
                if(count==0)
                    _logMsg += "该资源未打入图集:" + target + "\n";
            }
        }
        
        private static void ProjectWindowItemOnGUI(string guid, Rect rect)
        {
            if (Guid2Count.TryGetValue(guid, out var count))
            {
                var centeredStyle = GUI.skin.GetStyle("Label");
                centeredStyle.alignment = TextAnchor.MiddleRight;
                centeredStyle.padding.right = 5;
                GUI.Label(rect, count.ToString(), centeredStyle);
                EditorApplication.RepaintProjectWindow();
            }
        }
        
    }
}