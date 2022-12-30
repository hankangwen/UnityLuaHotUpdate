using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace XSJGame.Finder.Editor
{
    public class TextureReferences : EditorWindow
    {
        private static Dictionary<string, int> Guid2Count = new Dictionary<string, int>();
        private static Object _findFolder;

        private string _logMsg;
        private bool _groupEnabled;
        private bool _findEffPrefabs = false;
        private bool _findMaterials = false;
        private bool _findUiPrefabs = true;
        private bool _findAtlas = true;
        private string _effectPrefabPath = @"Assets/Package/Prefabs/Effect";
        private string _matPath = @"Assets/RawResources/Effects";
        private string _uiPrefabPath = @"Assets/Package/UI";
        private string _atlasPath = @"Assets/Package/Atlas";
        Vector2 scrollPos1;
        Vector2 scrollPos2;
        Vector2 scrollPos3;
        Vector2 scrollPos4;

        List<Object> effectPrefabList = new List<Object>();
        List<Object> matsList = new List<Object>();
        List<Object> uiPrefabList = new List<Object>();
        List<Object> atlasList = new List<Object>();
        
        TextureReferences()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }
        
        // [MenuItem("Assets/输出文件夹内图片的信息(UI)", true)]
        // private static bool ValidateLogFolderTextureMsg()
        // {
        //     bool open = Selection.objects.Length == 1 && 
        //                 Selection.GetFiltered(typeof(Object), SelectionMode.Assets)[0].GetType().Name == "DefaultAsset";
        //     if (!open)
        //         open = Selection.activeObject?.GetType().Name == "Texture2D";
        //     return open;
        // }
        //
        // [MenuItem("Assets/输出文件夹内图片的信息(UI)")]
        // static void ShowMyEditorWindow()
        // {
        //     GetWindow<TextureReferences>("查找图片引用(UI)").Show();
        // }

        void OnEnable()
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
            EditorGUILayout.BeginHorizontal();
            _findUiPrefabs = EditorGUILayout.Toggle("检索UI预制", _findUiPrefabs);
            _uiPrefabPath = EditorGUILayout.TextField("UI预制路径", _uiPrefabPath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            _findAtlas = EditorGUILayout.Toggle("检索图集", _findAtlas);
            _atlasPath = EditorGUILayout.TextField("图集路径", _atlasPath);
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

            if (_findEffPrefabs)
            {
                EditorGUILayout.BeginVertical();
                scrollPos1 = EditorGUILayout.BeginScrollView(scrollPos1);
                GUILayout.Label("对应的特效", EditorStyles.boldLabel);
                foreach (var item in effectPrefabList)
                {
                    EditorGUILayout.ObjectField("", item, typeof(Object), false);
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }

            if (_findMaterials)
            {
                EditorGUILayout.BeginVertical();
                scrollPos2 = EditorGUILayout.BeginScrollView(scrollPos2);
                GUILayout.Label("对应的材质球", EditorStyles.boldLabel);
                foreach (var item in matsList)
                {
                    EditorGUILayout.ObjectField("", item, typeof(Object), false);
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }

            if (_findUiPrefabs)
            {
                EditorGUILayout.BeginVertical();
                scrollPos3 = EditorGUILayout.BeginScrollView(scrollPos3);
                GUILayout.Label("对应的UI预制", EditorStyles.boldLabel);
                foreach (var item in uiPrefabList)
                {
                    EditorGUILayout.ObjectField("", item, typeof(Object), false);
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }

            if (_findAtlas)
            {
                EditorGUILayout.BeginVertical();
                scrollPos4 = EditorGUILayout.BeginScrollView(scrollPos4);
                GUILayout.Label("对应的图集", EditorStyles.boldLabel);
                foreach (var item in atlasList)
                {
                    EditorGUILayout.ObjectField("", item, typeof(Object), false);
                }
                EditorGUILayout.EndScrollView();
                EditorGUILayout.EndVertical();
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        private void LogTextureMsg(Object _findFolder)
        {
            string path = AssetDatabase.GetAssetPath(_findFolder);
            effectPrefabList = new List<Object>();
            matsList = new List<Object>();
            uiPrefabList = new List<Object>();
            atlasList = new List<Object>();

            if (_findFolder.GetType().Name == "Texture2D")
            {
                if (_findEffPrefabs)
                {
                    string[] effectPrefabs = Directory.GetFiles(_effectPrefabPath, "*.prefab", SearchOption.AllDirectories);
                    FindReference(path, effectPrefabs, ref effectPrefabList);
                }

                if (_findMaterials)
                {
                    string[] materials = Directory.GetFiles(_matPath, "*.mat", SearchOption.AllDirectories);
                    FindReference(path, materials, ref matsList, false);
                }
                
                if (_findUiPrefabs)
                {
                    string[] uiPrefabs = Directory.GetFiles(_uiPrefabPath, "*.prefab", SearchOption.AllDirectories);
                    // FindReference(path, uiPrefabs, ref uiPrefabList);
                    FindReferencesInPrefabs(path, uiPrefabs, ref uiPrefabList);
                }

                if (_findAtlas)
                {
                    string[] atlas = Directory.GetFiles(_atlasPath, "*.spriteatlas", SearchOption.AllDirectories);
                    FindReference(path, atlas, ref atlasList, false);
                }
            }
            else
            {
                string[] uiPrefabs = Directory.GetFiles(_uiPrefabPath, "*.prefab", SearchOption.AllDirectories);
                string[] atlas = Directory.GetFiles(_atlasPath, "*.spriteatlas", SearchOption.AllDirectories);
                
                string imgType = "*.PNG|*.JPG";
                string[] imgTypes = imgType.Split('|');
                for (int i = 0; i < imgTypes.Length; i++)
                {
                    string[] dirs = Directory.GetFiles(@path, imgTypes[i]);
                    for (int j = 0; j < dirs.Length; j++)
                    {
                        string target = dirs[j].Replace(@"\", @"/");
                        FindReference(target, uiPrefabs, ref uiPrefabList);
                        if(_findAtlas)
                            FindReference(target, atlas, ref atlasList, false);
                    }
                }
            }
        }

        private void FindReferencesInPrefabs(string target, string[] files, ref List<Object> references, bool isCalCount = true)
        {
            int count = 0;
            for (int i = 0; i < files.Length; i++)
            {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(files[i]);
                // UAtlasRef[] uAtlasRefs = go.GetComponentsInChildren<UAtlasRef>(true);
                Image[] uAtlasRefs = go.GetComponentsInChildren<Image>(true);
                for (int j = 0; j < uAtlasRefs.Length; j++)
                {
                    var uAtlas = uAtlasRefs[j];
                    // string spriteAtlasName = uAtlas.SpriteAtlasName;
                    // string spriteName = uAtlas.SpriteName;
                    // var spritePath = $"Assets/RawResources/UI/Items/Sprites/{spriteAtlasName}/{spriteName}.png";
                    // if (spritePath == target)
                    // {
                    //     var item = AssetDatabase.LoadMainAssetAtPath(files[i].Replace(Application.dataPath, "Assets"));
                    //     if(!references.Contains(item))
                    //         references.Add(item);
                    //     count++;
                    // }
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
        
        private void FindReference(string target, string[] files, ref List<Object> references, bool isCalCount = true)
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